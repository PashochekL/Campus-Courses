using CampusCourses.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Course
{
    public class EditCourseStatusModel
    {
        [Required(ErrorMessage = "Поле statuse обязательно")]
        public CourseStatuse statuse { get; set; }
    }
}
