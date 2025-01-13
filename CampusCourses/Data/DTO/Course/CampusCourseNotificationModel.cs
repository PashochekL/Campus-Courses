using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Course
{
    public class CampusCourseNotificationModel
    {
        public string text { get; set; }
        public bool isImportant { get; set; }
    }
}
