using System.ComponentModel.DataAnnotations;

namespace CampusCourses.Data.Entities
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
