using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LabApi.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Location is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 50 characters")]
        public string Location { get; set; } = null!;
        public string ManagerName { get; set; } = null!;
        public List<Student> Students { get; set; } = null!;
    }
}