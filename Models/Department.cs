using LabApi.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LabApi.Models
{
    public class Department
    {
        public int Id { get; set; }
        [UniqueName]

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Location is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 50 characters")]
        [RegularExpression("^(EG|USA)$", ErrorMessage = "Location must be EG or USA")]
        public string Location { get; set; } = null!;
        public string ManagerName { get; set; } = null!;
        public List<Student> Students { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;

    }
}