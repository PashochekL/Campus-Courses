using CampusCourses.Data.DTO.Course;

namespace CampusCourses.Services.IServices
{
    public interface ICourseService
    {
        public Task<CampusCoursePreviewModel> createNewCourse(Guid groupId, CreateCampusCourseModel createCampusCourseModel, Guid userId);
    }
}
