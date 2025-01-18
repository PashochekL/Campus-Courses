using CampusCourses.Data;
using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.Entities.Enums;
using CampusCourses.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using CampusCourses.Services.Exceptions;
using Microsoft.Extensions.Hosting;

namespace CampusCourses.Services
{
    public class HelperService
    {
        private readonly AppDBContext _dbContext;

        public HelperService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Account> checkAutorize(Guid userId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

            return account;
        }

        public async Task<Course> checkAvailabilityCourse(Guid courseId)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null) throw new NotFoundException("Такого курса не существует");

            return course;
        }

        public async Task<Student> checkStudent(Guid courseId, Guid studentId)
        {
            var student = await _dbContext.Students.Where(s => s.CourseId == courseId).FirstOrDefaultAsync(acc => acc.UserId == studentId);

            if (student == null) throw new NotFoundException("Такого студента нет на курсе");

            return student;
        }

        public IQueryable<Course> sorting(IQueryable<Course> course, Sort? sort)
        {
            switch (sort)
            { 
                case Sort.CreateAsc:
                    course = course.OrderBy(p => p.CreatedDate);
                    break;
                case Sort.CreateDesc:
                    course = course.OrderByDescending(p => p.CreatedDate);
                    break;
            }
            return course;
        }

        public async Task<CampusCourseDetailsModel> createCampusDetailsModel(Guid courseId, Account account)
        {
            var course = await _dbContext.Courses
                .Where(c => c.Id == courseId)
                .Include(c => c.Teachers)
                .ThenInclude(t => t.Account)
                .Include(c => c.Students)
                .ThenInclude(s => s.Account)
                .Include(c => c.Notifications)
                .FirstOrDefaultAsync();

            var notifications = course.Notifications.Select(n => new CampusCourseNotificationModel()
            {
                text = n.Text,
                isImportant = n.IsImportant
            }).ToList();

            var teachers = course.Teachers.Select(t => new CampusCourseTeacherModel()
            {
                email = t.Account.Email,
                name = t.Account.FullName,
                isMain = t.mainTeacher
            }).ToList();

            List<CampusCourseStudentModel> students = null;

            if ((course.Students.FirstOrDefault(s => s.UserId == account.Id) != null && !account.isAdmin) || 
                (!account.isAdmin && course.Students.FirstOrDefault(s => s.UserId == account.Id) == null && course.Teachers.FirstOrDefault(s => s.UserId == account.Id) == null))
            {
                var studentsList = course.Students.Where(s => s.Status == StudentStatuses.Accepted).ToList();

                students = await createCampusCourseStudentModel(studentsList);
            }
            else
            {
                students = await createCampusCourseStudentModel(course.Students);
            }

            foreach (var student in students)
            {
                if (account.isStudent && student.id == account.Id && !account.isAdmin)
                {
                    if (student.midtermResult == null) student.midtermResult = StudentMarks.NotDefined;

                    if (student.finalResult == null) student.finalResult = StudentMarks.NotDefined;
                }
                else if (account.isAdmin || (course.Teachers.FirstOrDefault(t => t.UserId == account.Id) != null))
                {
                    if (student.midtermResult == null && student.status == StudentStatuses.Accepted) student.midtermResult = StudentMarks.NotDefined;

                    if (student.finalResult == null && student.status == StudentStatuses.Accepted) student.finalResult = StudentMarks.NotDefined;
                }
            }

            var inQueue = course.Students.Count(s => s.Status == StudentStatuses.InQueue);

            var courseInf = new CampusCourseDetailsModel()
            {
                id = course.Id,
                name = course.Name,
                semester = course.Semester,
                startYear = course.StartYear,
                status = course.Status,
                studentsEnrolledCount = course.MaximumStudentsCount - course.RemainingSlotsCount,
                studentsInQueueCount = inQueue,
                maximumStudentsCount = course.MaximumStudentsCount,
                annotations = course.Annotations,
                requirements = course.Requirements,
                Teachers = teachers,
                Students = students,
                Notifications = notifications
            };

            if ((!account.isAdmin || !account.isTeacher) && course.Teachers.Where(t => t.CourseId == courseId).FirstOrDefault(t => t.UserId == account.Id) == null)
            {
                courseInf.studentsInQueueCount = null;
            }

            return courseInf;
        }
        
        public async Task<List<CampusCourseStudentModel>> createCampusCourseStudentModel(IEnumerable<Student> studentsList)
        {
            var students = studentsList.Select(s => new CampusCourseStudentModel()
            {
                id = s.Account.Id,
                email = s.Account.Email,
                name = s.Account.FullName,
                status = s.Status,
                midtermResult = s.MidtermResult,
                finalResult = s.FinalResult

            }).ToList();

            return students;
        }
    }
}
