using CampusCourses.Data.Entities.Enums;

namespace CampusCourses.Data.DTO.Course
{
    public class CampusCoursePreviewModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public int startYear { get; set; }
        public int maximumStudentsCount { get; set; }
        public int remainingSlotsCount { get; set; }
        public CourseStatuse status { get; set; }
        public Semester semester { get; set; }
    }
}
