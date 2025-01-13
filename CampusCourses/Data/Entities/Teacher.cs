using System.Numerics;

namespace CampusCourses.Data.Entities
{
    public class Teacher
    {
        public Guid UserId { get; set; }
        public Account Account { get; set; }

        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public bool mainTeacher { get; set; }
    }
}
