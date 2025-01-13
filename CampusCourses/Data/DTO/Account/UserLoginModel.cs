using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Account
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Поле email обязательно")]
        [MinLength(1, ErrorMessage = "Значение поля email должно быть минимум 1")]
        public string email { get; set; }
        [Required(ErrorMessage = "Поле password обязательно")]
        [MinLength(1, ErrorMessage = "Значение поля password должно быть минимум 1")]
        public string password { get; set; }
    }
}
