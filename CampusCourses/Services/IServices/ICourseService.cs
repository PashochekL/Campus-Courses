using CampusCourses.Data.DTO.Course;

namespace CampusCourses.Services.IServices
{
    public interface ICourseService
    {
        public Task<CampusCourseDetailsModel> getCampusDetails(Guid courseId, Guid userId);
        //public Task signUpCampusCourse(Guid courseId, Guid userId);
        public Task<CampusCourseDetailsModel> createNotification(Guid courseId, AddCampusCourseNotificationModel addNotificationModel, Guid userId);
        public Task<CampusCoursePreviewModel> createNewCourse(Guid groupId, CreateCampusCourseModel createCampusCourseModel, Guid userId);
    }
}
