using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Course
{
    public class AddTeacherToCourseModel
    {
        [Required(ErrorMessage = "Поле userId обязательно")]
        public string? userId { get; set; }
    }
}
