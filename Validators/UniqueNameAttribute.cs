using LabApi.Models;
using System.ComponentModel.DataAnnotations;

namespace LabApi.Validators
{
    public class UniqueNameAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var context = (ITIDbContext)validationContext.GetService(typeof(ITIDbContext))!;
            var studentName = value as string;
            if (studentName == null)
            {
                return new ValidationResult("Name is required");
            }
            var department = context.Departments.SingleOrDefault(e => e.Name == studentName);
            if (department != null)
            {
                return new ValidationResult("Name already exists");
            }
            return ValidationResult.Success!;
        }
    }
}
