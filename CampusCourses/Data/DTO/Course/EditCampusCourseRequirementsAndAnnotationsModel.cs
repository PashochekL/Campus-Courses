using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Course
{
    public class EditCampusCourseRequirementsAndAnnotationsModel
    {
        [Required(ErrorMessage = "Поле requirements обязательно")]
        [MinLength(1, ErrorMessage = "Значение поля requirements должно быть минимум 1")]
        public string requirements { get; set; }
        [Required(ErrorMessage = "Поле annotations обязательно")]
        [MinLength(1, ErrorMessage = "Значение поля annotations должно быть минимум 1")]
        public string annotations { get; set; }
    }
}
