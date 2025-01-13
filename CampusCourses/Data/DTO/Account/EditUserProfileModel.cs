using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Account
{
    public class EditUserProfileModel
    {
        [Required(ErrorMessage = "Поле fullName обязательно")]
        [MinLength(1, ErrorMessage = "Значение поля fullName должно быть минимум 1")]
        public string fullName { get; set; }
        [Required(ErrorMessage = "Поле birthDate обязательно")]
        public DateTime birthDate { get; set; }
    }
}
