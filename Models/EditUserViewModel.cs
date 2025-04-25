using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class EditUserViewModel
{
    public string Id { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Display(Name = "Role")]
    public string Role { get; set; }

    public List<SelectListItem> Roles { get; set; }

    [Display(Name = "Manager (if Member)")]
    public string ManagerId { get; set; }

    public List<SelectListItem> Managers { get; set; }
}
