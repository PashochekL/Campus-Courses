using CampusCourses.Data;
using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.Entities.Enums;
using CampusCourses.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace CampusCourses.Services
{
    public class HelperService
    {
        private readonly AppDBContext _dbContext;

        public HelperService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
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

            var students = course.Students.Select(s => new CampusCourseStudentModel()
            {
                id = s.Account.Id,
                email = s.Account.Email,
                name = s.Account.FullName,
                status = s.Status,
                midtermResult = s.MidtermResult,
                finalResult = s.FinalResult

            }).ToList();

            foreach (var student in students)
            {
                var choosedStudent = course.Students.FirstOrDefault(s => s.Account.Id == student.id);

                if (account.isStudent && choosedStudent != null)
                {
                    student.midtermResult = choosedStudent.MidtermResult;
                    student.finalResult = choosedStudent.FinalResult;
                }
                else if (account.isTeacher && choosedStudent != null)
                {
                    student.midtermResult = choosedStudent.MidtermResult;
                    student.finalResult = choosedStudent.FinalResult;
                }
            }

            var inQueue = course.Students.Count(q => q.Status == StudentStatuses.InQueue);

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

            return courseInf;
        }
    }
}
