using CampusCourses.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.Entities
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int MaximumStudentsCount { get; set; }
        public int RemainingSlotsCount { get; set; }
        public Semester Semester { get; set; }
        public CourseStatuse Status { get; set; }
        public string Requirements { get; set; }
        public string Annotations { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
