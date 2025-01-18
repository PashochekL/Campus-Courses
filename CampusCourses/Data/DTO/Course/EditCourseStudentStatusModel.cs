using CampusCourses.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Course
{
    public class EditCourseStudentStatusModel
    {
        [Required(ErrorMessage = "Поле status обязательно")]
        public StudentStatuses status { get; set; }
    }
}
