using CampusCourses.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Course
{
    public class EditCourseStudentMarkModel
    {
        [Required(ErrorMessage = "Поле markType обязательно")]
        public MarkType markType { get; set; }
        [Required(ErrorMessage = "Поле mark обязательно")]
        public StudentMarks mark {  get; set; }
    }
}
