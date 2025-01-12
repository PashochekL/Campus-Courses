using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.DTO.Group
{
    public class CreateCampusGroupModel
    {
        [Required(ErrorMessage = "Поле name обязательно")]
        [StringLength(100)]
        public string name { get; set; }
    }
}
