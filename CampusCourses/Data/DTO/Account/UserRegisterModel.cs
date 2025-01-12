using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Account
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Поле fullName обязательно")]
        [StringLength(100)]
        public string fullName { get; set; }

        [Required(ErrorMessage = "Поле birthDate обязательно")]
        public DateTime birthDate { get; set; }

        [Required(ErrorMessage = "Поле email обязательно")]
        [StringLength(100)]
        public string email { get; set; }

        [Required(ErrorMessage = "Поле password обязательно")]
        [StringLength(32, MinimumLength = 6)]
        public string password { get; set; }

        [Required(ErrorMessage = "Поле confirmPassword обязательно")]
        [StringLength(32, MinimumLength = 6)]
        public string confirmPassword { get; set; }
    }
}
