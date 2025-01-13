using CampusCourses.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Course
{
    public class CreateCampusCourseModel
    {
        [Required(ErrorMessage = "Поле name обязательно")]
        [StringLength(100), MinLength(1)]
        public string name { get; set; }

        [Required(ErrorMessage = "Поле startYear обязательно")]
        [Range(2020, 2027, ErrorMessage = "Значение startYear должно быть между 2020 и 2027 годами")]
        public int startYear { get; set; }

        [Required(ErrorMessage = "Поле maximumStudentsCount обязательно")]
        [Range(1, 200, ErrorMessage = "Значение maximumStudentsCount должно быть между 1 и 200")]
        public int maximumStudentsCount { get; set; }

        [Required(ErrorMessage = "Поле semester обязательно")]
        public Semester semester { get; set; }

        [Required(ErrorMessage = "Поле requirements обязательно")]
        [MinLength(1, ErrorMessage = "Значение поля requirements должно быть минимум 1")]
        public string requirements { get; set; }

        [Required(ErrorMessage = "Поле annotaitons обязательно")]
        [MinLength(1, ErrorMessage = "Значение поля annotaitons должно быть минимум 1")]
        public string annotaitons { get; set; }

        [Required(ErrorMessage = "Поле mainTeacherId обязательно")]
        public Guid mainTeacherId { get; set; }
    }
}
