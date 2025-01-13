using CampusCourses.Data.Entities.Enums;

namespace CampusCourses.Data.Entities
{
    public class Student
    {
        public Guid UserId { get; set; }
        public Account Account { get; set; }

        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public StudentStatuses Status { get; set; }
        public StudentMarks MidtermResult { get; set; }
        public StudentMarks FinalResult { get; set; }
    }
}
