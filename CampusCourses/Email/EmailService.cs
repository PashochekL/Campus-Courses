using CampusCourses.Data;
using CampusCourses.Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace CampusCourses.Email
{
    public class EmailService
    {
        private readonly AppDBContext _dbContext;

        public EmailService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<(string Email, string CourseName)>> GetStudentsEmailsForTomorrowCoursesAsync()
        {
            var tomorrow = DateTime.Now.AddDays(1);

            var result = await _dbContext.Courses
                .Where(c => c.StartYear == tomorrow.Year &&
                            c.Semester == GetCurrentSemester() &&
                            c.Status == CourseStatuse.OpenForAssigning)
                .SelectMany(c => c.Students
                    .Where(s => s.Status == StudentStatuses.Accepted)
                    .Select(s => new { s.Account.Email, CourseName = c.Name }))
                .ToListAsync();

            return result.Select(x => (x.Email, x.CourseName)).ToList();
        }

        private Semester GetCurrentSemester()
        {
            var month = DateTime.Now.Month;
            return month is >= 1 and <= 6 ? Semester.Spring : Semester.Autumn;
        }
    }
}
