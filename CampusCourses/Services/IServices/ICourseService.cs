using CampusCourses.Data.DTO.Course;
using CampusCourses.Data.DTO.Student;
using CampusCourses.Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CampusCourses.Services.IServices
{
    public interface ICourseService
    {
        public Task<CampusCourseDetailsModel> getCampusDetails(Guid courseId, Guid userId);
        public Task<StudentViewModel> signUpCampusCourse(Guid courseId, Guid userId);
        public Task<CampusCourseDetailsModel> editCourseStatus(Guid courseId, EditCourseStatusModel editCourseStatusModel, Guid userId);
        public Task<CampusCourseDetailsModel> editStatusStudent(Guid courseId, Guid studentId, Guid userId, EditCourseStudentStatusModel EditModel);
        public Task<CampusCourseDetailsModel> createNotification(Guid courseId, AddCampusCourseNotificationModel addNotificationModel, Guid userId);
        public Task<CampusCourseDetailsModel> editMark(Guid courseId, Guid studentId, EditCourseStudentMarkModel editCourseStudentMarkModel, Guid userId);
        public Task<CampusCoursePreviewModel> createNewCourse(Guid groupId, CreateCampusCourseModel createCampusCourseModel, Guid userId);
        public Task<CampusCourseDetailsModel> editAnnotations(Guid courseId, EditCampusCourseRequirementsAndAnnotationsModel editModel, Guid userId);
        public Task<CampusCourseDetailsModel> editCourse(Guid courseId, EditCampusCourseModel editModel, Guid userId);
        public Task deleteCourse(Guid courseId, Guid userId);
        public Task<CampusCourseDetailsModel> addTeacher(Guid courseId, AddTeacherToCourseModel addTeacherToCourseModel, Guid userId);
        public Task<List<CampusCoursePreviewModel>> getMyCourse(Guid userId);
        public Task<List<CampusCoursePreviewModel>> getCourseTeaching(Guid userId);
        public Task<List<CampusCoursePreviewModel>> getCourseList(int page, int size, Sort? sort, string? search, bool? hasPlacesAndOpen, Semester? semester);
    }
}
