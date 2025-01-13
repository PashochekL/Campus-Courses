using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Course
{
    public class AddCampusCourseNotificationModel
    {
        [Required(ErrorMessage = "Поле text обязательно")]
        [MinLength(1, ErrorMessage = "Значение поля name должно быть минимум 1")]
        public string text { get; set; }
        [Required(ErrorMessage = "Поле isImportant обязательно")]
        public bool isImportant { get; set; }
    }
}
