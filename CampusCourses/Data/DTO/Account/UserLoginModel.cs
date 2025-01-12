using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Account
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Поле email обязательно")]
        public string email { get; set; }

        [Required(ErrorMessage = "Поле password обязательно")]
        public string password { get; set; }
    }
}
