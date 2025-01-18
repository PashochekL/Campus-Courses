using CampusCourses.Data.Entities.Enums;

namespace CampusCourses.Data.DTO.Student
{
    public class StudentViewModel
    {
        public Guid id { get; set; }
        public StudentStatuses Status { get; set; }
    }
}
