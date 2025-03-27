using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LabApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
        public string Name { get; set; }
        [Range(16, 60, ErrorMessage = "Age must be between 16 and 60")]
        public int Age { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "address must be between 2 and 50 characters")]
        public string Address { get; set; }
        public string Image { get; set; }=null!;
        public DateOnly DoB { get; set; }
        [ForeignKey(nameof(Department))]
        public int DeptId { get; set; }
        public Department? Department { get; set; } = null!;
    }
}