using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Group
{
    public class CreateCampusGroupModel
    {
        [Required(ErrorMessage = "Поле name обязательно")]
        [StringLength(100), MinLength(1, ErrorMessage = "Значение поля name должно быть минимум 1")]
        public string name { get; set; }
    }
}
