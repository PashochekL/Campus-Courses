using CampusCourses.Data.Entities.Enums;
using CampusCourses.Data.Entities;

namespace CampusCourses.Data.DTO.Course
{
    public class CampusCourseDetailsModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public int startYear { get; set; }
        public int maximumStudentsCount { get; set; }
        public int studentsEnrolledCount { get; set; }
        public int? studentsInQueueCount { get; set; }
        public string requirements { get; set; }
        public string annotations { get; set; }
        public CourseStatuse status { get; set; }
        public Semester semester { get; set; }
        public ICollection<CampusCourseStudentModel>? Students { get; set; } = new List<CampusCourseStudentModel>();
        public ICollection<CampusCourseTeacherModel>? Teachers { get; set; } = new List<CampusCourseTeacherModel>();
        public ICollection<CampusCourseNotificationModel>? Notifications { get; set; } = new List<CampusCourseNotificationModel>();
    }
}
