using CampusCourses.Data;
using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.Entities;
using CampusCourses.Data.Entities.Enums;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

            var courseExists = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (courseExists == null) throw new BadRequestException("Такого курса не существует");

            var courseInf = await _helperService.createCampusDetailsModel(courseId, account);

            return courseInf;
        }  //если препод запрашивает, то видит оценки всех студентов, а если студент, то видит лишь свои оценки





        /*public async Task signUpCampusCourse(Guid courseId, Guid userId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

            var 
        }*/

        public async Task<CampusCourseDetailsModel> createNotification(Guid courseId, AddCampusCourseNotificationModel addNotificationModel, Guid userId)
        {
            var account = await _dbContext.Accounts.Include(acc => acc.TeachingCourses).FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

            var teacher = account.TeachingCourses.Any(teach => teach.CourseId == courseId);

            if (!(account.isAdmin || teacher)) throw new ForbiddenException("У вас недостаточно прав");

            var courseExists = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (courseExists == null) throw new BadRequestException("Такого курса не существует");

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

        public async Task<CampusCoursePreviewModel> createNewCourse(Guid groupId, CreateCampusCourseModel createCampusCourseModel, Guid userId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

            if (!account.isAdmin) throw new ForbiddenException("У вас нет прав администратора");

            var teacherAcc = await _dbContext.Accounts.Include(acc => acc.TeachingCourses).FirstOrDefaultAsync(t => t.Id == createCampusCourseModel.mainTeacherId);

            if (teacherAcc == null) throw new BadRequestException("Такого преподавателя не существует");

            var currentYear = DateTime.Today.Year;

            if (createCampusCourseModel.startYear < currentYear)
            {
                throw new BadRequestException("Нельзя создать курс, начало которого будет в прошлом");
            }

            if (createCampusCourseModel.startYear == currentYear && createCampusCourseModel.semester == Semester.Spring && DateTime.Today.Month > 6)
            {
                throw new BadRequestException("Курс, который должен был начаться весной этого года, не может быть создан после июня");
            }

            var newCourse = new Course()
            {
                Id = new Guid(),
                Name = createCampusCourseModel.name,
                StartYear = createCampusCourseModel.startYear,
                MaximumStudentsCount = createCampusCourseModel.maximumStudentsCount,
                RemainingSlotsCount = createCampusCourseModel.maximumStudentsCount,
                Semester = createCampusCourseModel.semester,
                Status = CourseStatuse.Started,
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

            if (teacherAcc.TeachingCourses.Count == 1)
            {
                teacherAcc.isTeacher = true;
                _dbContext.Update(teacherAcc);
            }
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
    }
}
