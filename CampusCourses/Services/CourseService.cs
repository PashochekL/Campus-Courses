using CampusCourses.Data;
using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.DTO.Student;
using CampusCourses.Data.Entities;
using CampusCourses.Data.Entities.Enums;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CampusCourses.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDBContext _dbContext;
        private readonly HelperService _helperService;

        public CourseService(AppDBContext dbContext, HelperService helperService)
        {
            _dbContext = dbContext;
            _helperService = helperService;
        }

        public async Task<CampusCourseDetailsModel> getCampusDetails(Guid courseId, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);
            var course = await _helperService.checkAvailabilityCourse(courseId);
            var courseInf = await _helperService.createCampusDetailsModel(courseId, account);

            return courseInf;
        }

        public async Task<StudentViewModel> signUpCampusCourse(Guid courseId, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);
            var courseExists = await _helperService.checkAvailabilityCourse(courseId);

            var course = await _dbContext.Courses
                .Where(c => c.Id == courseId)
                .Include(c => c.Teachers)
                .Include(c => c.Students)
                .FirstOrDefaultAsync();

            if(course.Status != CourseStatuse.OpenForAssigning || course.RemainingSlotsCount == 0)
                throw new BadRequestException("Нельзя присоедениться к курсу, когда нет набора студентов либо свободных мест");

            if (course.Teachers.FirstOrDefault(t => t.UserId == userId) != null)
                throw new ForbiddenException("Преподаватель курса не может записаться на этот же курс в роли студента");

            var student = course.Students.Where(s => s.Status != StudentStatuses.Declined).FirstOrDefault(c => c.UserId == userId);

            if (student != null) 
                throw new ForbiddenException("Нельзя повторно записаться на курс");

            StudentViewModel studentViewModel = new StudentViewModel();

            var newStudent = course.Students.FirstOrDefault(c => c.UserId == userId);

            if (newStudent != null && newStudent.Status == StudentStatuses.Declined)
            {
                newStudent.Status = StudentStatuses.InQueue;
                _dbContext.Students.Update(newStudent);

                studentViewModel.id = newStudent.UserId;
                studentViewModel.Status = newStudent.Status;
            }
            else 
            {
                var createStudent = new Student()
                {
                    UserId = userId,
                    CourseId = courseId,
                    Status = StudentStatuses.InQueue
                };
                _dbContext.Students.Add(createStudent);

                studentViewModel.id = createStudent.UserId;
                studentViewModel.Status = createStudent.Status;
            }
            await _dbContext.SaveChangesAsync();

            return studentViewModel;
        }

        public async Task<CampusCourseDetailsModel> editCourseStatus(Guid courseId, EditCourseStatusModel editCourseStatusModel, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);
            var course = await _helperService.checkAvailabilityCourse(courseId);

            var teacher = await _dbContext.Teachers.Where(t => t.CourseId == courseId && t.UserId == userId).FirstOrDefaultAsync();

            if (!account.isAdmin && teacher == null) throw new ForbiddenException("У вас недостаточно прав");

            if (course.Status == CourseStatuse.Finished) throw new BadRequestException("Нельзя сменить статус у завершенного курса");

            else if (course.Status == CourseStatuse.OpenForAssigning && editCourseStatusModel.statuse == CourseStatuse.Created)
                throw new BadRequestException("Нельзя сменить статус у курса в обратной последовательности");

            else if (course.Status == CourseStatuse.Started && 
                (editCourseStatusModel.statuse == CourseStatuse.Created || editCourseStatusModel.statuse == CourseStatuse.OpenForAssigning))
            {
                throw new BadRequestException("Нельзя сменить статус у курса в обратной последовательности");
            }
            else if (course.Status == editCourseStatusModel.statuse) throw new BadRequestException("Нельзя сменить статус курса на такой же");

            course.Status = editCourseStatusModel.statuse;

            _dbContext.Courses.Update(course);
            await _dbContext.SaveChangesAsync();

            var courseInf = await _helperService.createCampusDetailsModel(courseId, account);

            return courseInf;
        }

        public async Task<CampusCourseDetailsModel> editStatusStudent(Guid courseId, Guid studentId, Guid userId, EditCourseStudentStatusModel EditModel)
        {
            var account = await _helperService.checkAutorize(userId);
            var course = await _helperService.checkAvailabilityCourse(courseId);

            var student = await _helperService.checkStudent(courseId, studentId);

            var teacher = await _dbContext.Teachers.Where(t => t.CourseId == courseId && t.UserId == userId).FirstOrDefaultAsync();

            if (!account.isAdmin && teacher == null) throw new ForbiddenException("У вас недостаточно прав");

            if (student.Status == EditModel.status) throw new BadRequestException("Нельзя сменить статус на такой же");

            if (student.Status == StudentStatuses.Declined && EditModel.status == StudentStatuses.Accepted) 
                throw new BadRequestException("Нельзя принять студента, которого нет в очереди");

            if (student.Status == StudentStatuses.Accepted && (EditModel.status == StudentStatuses.InQueue || EditModel.status == StudentStatuses.Declined))
                throw new BadRequestException("Нельзя отправить студента, проходящего курс, обратно в очередь или выгнать с курса");

            if (course.RemainingSlotsCount == 0) throw new BadRequestException("Нет свободных мест на курсе");

            student.Status = EditModel.status;
            _dbContext.Students.Update(student);

            if (EditModel.status == StudentStatuses.Accepted)
            {
                course.RemainingSlotsCount -= 1;
                _dbContext.Courses.Update(course);
            }
            await _dbContext.SaveChangesAsync();

            var courseInf = await _helperService.createCampusDetailsModel(courseId, account);

            return courseInf;
        }

        public async Task<CampusCourseDetailsModel> createNotification(Guid courseId, AddCampusCourseNotificationModel addNotificationModel, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);
            var course = await _helperService.checkAvailabilityCourse(courseId);

            var teacher = await _dbContext.Teachers.Where(t => t.CourseId == courseId && t.UserId == userId).FirstOrDefaultAsync();

            if (!account.isAdmin && teacher == null) throw new ForbiddenException("У вас недостаточно прав");

            var notification = new Notification()
            {
                Text = addNotificationModel.text,
                IsImportant = addNotificationModel.isImportant,
                CreatedDate = DateTime.UtcNow,
                CourseId = courseId
            };
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            var courseInf = await _helperService.createCampusDetailsModel(courseId, account);

            return courseInf;
        }

        public async Task<CampusCourseDetailsModel> editMark(Guid courseId, Guid studentId, EditCourseStudentMarkModel editCourseStudentMarkModel, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);
            var course = await _helperService.checkAvailabilityCourse(courseId);
            var student = await _helperService.checkStudent(courseId, studentId);

            var checkRole = await _dbContext.Teachers.Where(s => s.CourseId == courseId).FirstOrDefaultAsync(acc => acc.UserId == userId);

            if (!account.isAdmin && checkRole == null) throw new ForbiddenException("У вас недостаточно прав");

            if (student.Status != StudentStatuses.Accepted) throw new BadRequestException("Нельзя сменить оценку студента, который не принят на курс");

            if (editCourseStudentMarkModel.markType == MarkType.Midterm)
            {
                student.MidtermResult = editCourseStudentMarkModel.mark;
                _dbContext.Entry(student).Property(s => s.MidtermResult).IsModified = true;
            }    
            else
            {
                student.FinalResult = editCourseStudentMarkModel.mark;
                _dbContext.Entry(student).Property(s => s.FinalResult).IsModified = true;
            }
            await _dbContext.SaveChangesAsync();

            var courseInf = await _helperService.createCampusDetailsModel(courseId, account);

            return courseInf;
        }

        public async Task<CampusCoursePreviewModel> createNewCourse(Guid groupId, CreateCampusCourseModel createCampusCourseModel, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);

            if (!account.isAdmin) throw new ForbiddenException("У вас нет прав администратора");

            var teacherAcc = await _dbContext.Accounts.Include(acc => acc.TeachingCourses).FirstOrDefaultAsync(t => t.Id == createCampusCourseModel.mainTeacherId);

            if (teacherAcc == null) throw new BadRequestException("Такого пользователя не существует");

            var currentYear = DateTime.Today.Year;

            if (createCampusCourseModel.startYear < currentYear) throw new BadRequestException("Нельзя создать курс, начало которого будет в прошлом");

            if (createCampusCourseModel.startYear < currentYear) throw new BadRequestException("Нельзя создать курс, начало которого будет в прошлом");

            if (createCampusCourseModel.startYear == currentYear && createCampusCourseModel.semester == Semester.Spring && DateTime.Today.Month > 6)
                throw new BadRequestException("Курс, который должен был начаться весной этого года, не может быть создан после июня");

            var newCourse = new Course()
            {
                Id = new Guid(),
                Name = createCampusCourseModel.name,
                StartYear = createCampusCourseModel.startYear,
                MaximumStudentsCount = createCampusCourseModel.maximumStudentsCount,
                RemainingSlotsCount = createCampusCourseModel.maximumStudentsCount,
                Semester = createCampusCourseModel.semester,
                Status = CourseStatuse.Created,
                Requirements = createCampusCourseModel.requirements,
                Annotations = createCampusCourseModel.annotaitons,
                GroupId = groupId,
                CreatedDate = DateTime.UtcNow,
            };
            _dbContext.Courses.Add(newCourse);

            var newTeacher = new Teacher()
            {
                UserId = createCampusCourseModel.mainTeacherId,
                mainTeacher = true,
                CourseId = newCourse.Id,
            };
            _dbContext.Teachers.Add(newTeacher);
            await _dbContext.SaveChangesAsync();

            CampusCoursePreviewModel campusCoursePreviewModel = new CampusCoursePreviewModel()
            {
                id = newCourse.Id,
                semester = newCourse.Semester,
                startYear = newCourse.StartYear,
                maximumStudentsCount = newCourse.MaximumStudentsCount,
                name = newCourse.Name,
                remainingSlotsCount = newCourse.RemainingSlotsCount,
                status = newCourse.Status,
            };
            return campusCoursePreviewModel;
        }

        public async Task<CampusCourseDetailsModel> editAnnotations(Guid courseId, EditCampusCourseRequirementsAndAnnotationsModel editModel, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);
            var course = await _helperService.checkAvailabilityCourse(courseId);

            var teacher = await _dbContext.Teachers.Where(t => t.CourseId == courseId && t.UserId == userId).FirstOrDefaultAsync();

            if (!account.isAdmin && teacher == null) throw new ForbiddenException("У вас недостаточно прав");

            course.Annotations = editModel.annotations;
            course.Requirements = editModel.requirements;

            _dbContext.Courses.Update(course);
            await _dbContext.SaveChangesAsync();

            var courseInf = await _helperService.createCampusDetailsModel(courseId, account);

            return courseInf;
        }

        public async Task<CampusCourseDetailsModel> editCourse(Guid courseId, EditCampusCourseModel editModel, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);
            await _helperService.checkAvailabilityCourse(courseId);

            if (!account.isAdmin) throw new ForbiddenException("У вас нет прав администратора");

            var teacherAcc = await _dbContext.Accounts.Include(acc => acc.TeachingCourses).FirstOrDefaultAsync(t => t.Id == editModel.mainTeacherId);

            if (teacherAcc == null) throw new BadRequestException("Такого пользователя не существует");

            var currentYear = DateTime.Today.Year;

            if (editModel.startYear < currentYear) throw new BadRequestException("Нельзя создать курс, начало которого будет в прошлом");
            if (editModel.startYear == currentYear && editModel.semester == Semester.Spring && DateTime.Today.Month > 6)
                throw new BadRequestException("Курс, который должен был начаться весной этого года, не может быть создан после июня");

            var course = await _dbContext.Courses
                .Where(c => c.Id == courseId)
                .Include(c => c.Teachers)
                .ThenInclude(c => c.Account)
                .Include(c => c.Students)
                .ThenInclude(c => c.Account)
                .FirstOrDefaultAsync();

            if (course == null) throw new NotFoundException("Курс не найден");

            if (course.Students.Count(s => s.Status == StudentStatuses.Accepted) > editModel.maximumStudentsCount)
            {
                throw new BadRequestException("Значение maximumStudentsCount не может быть меньше количества студентов, принятых на курс");
            }

            var mainTeacher = course.Teachers.FirstOrDefault(t => t.mainTeacher == true);
            var mainTeacherAccount = mainTeacher?.Account;

            if (mainTeacher != null && mainTeacher.UserId != teacherAcc.Id)
            {
                if (mainTeacher.mainTeacher)
                {
                    mainTeacher.mainTeacher = false;
                    _dbContext.Teachers.Update(mainTeacher);
                }

                var checkTeacher = course.Teachers.FirstOrDefault(t => t.UserId == teacherAcc.Id);
                if (checkTeacher != null)
                {
                    checkTeacher.mainTeacher = true;
                    _dbContext.Teachers.Update(checkTeacher);
                }
                else
                {
                    var newTeacher = new Teacher()
                    {
                        UserId = teacherAcc.Id,
                        CourseId = courseId,
                        mainTeacher = true
                    };
                    _dbContext.Teachers.Add(newTeacher);
                }
            }

            course.Name = editModel.name;
            course.StartYear = editModel.startYear;
            course.MaximumStudentsCount = editModel.maximumStudentsCount;
            course.RemainingSlotsCount = editModel.maximumStudentsCount - course.Students.Where(s => s.Status == StudentStatuses.Accepted).ToList().Count;
            course.Semester = editModel.semester;
            course.Requirements = editModel.requirements;
            course.Annotations = editModel.annotaitons;

            _dbContext.Courses.Update(course);
            await _dbContext.SaveChangesAsync();

            var courseInf = await _helperService.createCampusDetailsModel(courseId, account);

            return courseInf;
        }

        public async Task deleteCourse(Guid courseId, Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);
            var course = await _helperService.checkAvailabilityCourse(courseId);

            if (!account.isAdmin) throw new ForbiddenException("У вас нет прав администратора");

            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CampusCourseDetailsModel> addTeacher(Guid courseId, AddTeacherToCourseModel addTeacherToCourseModel, Guid adminId)
        {
            var accountAdmin = await _helperService.checkAutorize(adminId);
            var course = await _helperService.checkAvailabilityCourse(courseId);

            if (!Guid.TryParse(addTeacherToCourseModel.userId.ToString(), out Guid userId))
                throw new BadRequestException("Неверный формат id пользователя");

            var userAccount = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == userId);

            if (userAccount == null) throw new NotFoundException("Пользователь не найден");

            var checkRules = await _dbContext.Teachers.Where(s => s.CourseId == courseId).FirstOrDefaultAsync(acc => acc.UserId == adminId);

            if (checkRules != null && !(accountAdmin.isAdmin == true || checkRules.mainTeacher == true))
            {
                throw new ForbiddenException("У вас недостаточно прав");
            }

            var student = await _dbContext.Students.Where(s => s.CourseId == courseId).FirstOrDefaultAsync(acc => acc.UserId == userId);

            if (student != null)
            {
                throw new BadRequestException("Студент курса не может быть назначен его преподавателем");
            }

            var teacher = await _dbContext.Teachers.Where(s => s.CourseId == courseId).FirstOrDefaultAsync(acc => acc.UserId == userId);

            if (teacher != null) throw new BadRequestException("Пользователь уже является преподавателем этого курса");

            var newTeacher = new Teacher()
            {
                UserId = userAccount.Id,
                CourseId = courseId,
            };
            _dbContext.Teachers.Add(newTeacher);
            await _dbContext.SaveChangesAsync();

            var courseInf = await _helperService.createCampusDetailsModel(courseId, accountAdmin);

            return courseInf;
        }

        public async Task<List<CampusCoursePreviewModel>> getMyCourse(Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);

            var courses = await _dbContext.Courses.Where(c => c.Students.Any(t => t.Account.Id == userId)).ToListAsync();

            var myCourses = courses.Select(s => new CampusCoursePreviewModel()
            {
                id = s.Id,
                name = s.Name,
                maximumStudentsCount = s.MaximumStudentsCount,
                remainingSlotsCount = s.RemainingSlotsCount,
                semester = s.Semester,
                startYear = s.StartYear,
                status = s.Status
            }).ToList();

            return myCourses;
        }

        public async Task<List<CampusCoursePreviewModel>> getCourseTeaching(Guid userId)
        {
            var account = await _helperService.checkAutorize(userId);

            var courses = await _dbContext.Courses.Where(c => c.Teachers.Any(t => t.Account.Id == userId)).ToListAsync();

            var coursesTeaching = courses.Select(s => new CampusCoursePreviewModel()
            {
                id = s.Id,
                name = s.Name,
                maximumStudentsCount = s.MaximumStudentsCount,
                remainingSlotsCount = s.RemainingSlotsCount,
                semester = s.Semester,
                startYear = s.StartYear,
                status = s.Status
            }).ToList();

            return coursesTeaching;
        }

        public async Task<List<CampusCoursePreviewModel>> getCourseList(int page, int size, Sort? sort, 
            string? search, bool? hasPlacesAndOpen, Semester? semester)
        {
            page = page < 1 ? 1 : page;
            size = size < 1 ? 10 : size;

            var courseQuery = _dbContext.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                courseQuery = courseQuery.Where(course => course.Name.Contains(search));
            }

            if (semester.HasValue)
            {
                courseQuery = courseQuery.Where(course => course.Semester == semester.Value);
            }

            if (hasPlacesAndOpen.HasValue && hasPlacesAndOpen.Value)
            {
                courseQuery = courseQuery.Where(course => course.RemainingSlotsCount > 0 && course.Status == CourseStatuse.Started);
            }

            if (sort.HasValue)
            {
                courseQuery = _helperService.sorting(courseQuery, sort);
            }

            var courses = await courseQuery.ToListAsync();

            var coursesPagination = courses.Skip((page - 1) * size).Take(size);

            var coursesInf = coursesPagination.Select(c => new CampusCoursePreviewModel()
            {
                id = c.Id,
                name = c.Name,
                maximumStudentsCount = c.MaximumStudentsCount,
                remainingSlotsCount = c.RemainingSlotsCount,
                semester = c.Semester,
                startYear = c.StartYear,
                status = c.Status
            }).ToList();

            return coursesInf;
        }
    }
}
