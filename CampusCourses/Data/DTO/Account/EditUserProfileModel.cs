using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Account
{
    public class EditUserProfileModel
    {
        [Required(ErrorMessage = "Поле fullName обязательно")]
        public string fullName { get; set; }
        [Required(ErrorMessage = "Поле birthDate обязательно")]
        public DateTime birthDate { get; set; }
    }
}
