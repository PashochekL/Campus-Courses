using CampusCourses.Data;
using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.Entities;
using CampusCourses.Data.Entities.Enums;
using CampusCourses.Services.Exceptions;
using CampusCourses.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CampusCourses.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDBContext _dbContext;

        public CourseService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CampusCoursePreviewModel> createNewCourse(Guid groupId, CreateCampusCourseModel createCampusCourseModel, Guid userId)
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Id == userId);

            if (account == null) throw new UnauthorizedException("Пользователь не авторизован");

            if (!account.isAdmin) throw new ForbiddenException("У вас нет прав администратора");

            var course = new Course
            {
                Name = createCampusCourseModel.name,
                StartYear = createCampusCourseModel.startYear,
                MaximumStudentsCount = createCampusCourseModel.maximumStudentsCount,
                RemainingSlotsCount = createCampusCourseModel.maximumStudentsCount,
                Semester = createCampusCourseModel.semester,
                Status = CourseStatuse.Started,
                Requirements = createCampusCourseModel.requirements,
                Annotations = createCampusCourseModel.annotaitons,
                
            };
        }
    }
}
