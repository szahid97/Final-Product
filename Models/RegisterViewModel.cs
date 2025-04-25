using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class RegisterViewModel : IValidatableObject
{
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Assign Role (optional)")]
    public string Role { get; set; }

    public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();

    public string? ManagerId { get; set; }

    public List<SelectListItem> Managers { get; set; } = new List<SelectListItem>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Role == "Member" && string.IsNullOrEmpty(ManagerId))
        {
            yield return new ValidationResult("Manager must be selected for a Member role.", new[] { "ManagerId" });
        }
    }
}
