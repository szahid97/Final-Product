using Microsoft.AspNetCore.Identity;
using TaskTrackerDemo.Models;
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public bool IsManager { get; set; }
    public string? ManagerId { get; set; }
    public ApplicationUser? Manager { get; set; }
    public ICollection<ApplicationUser>? TeamMembers { get; set; }
    public List<Project> ManagedProjects { get; set; }
    public List<ProjectTask> AssignedTasks { get; set; }
    public List<ProjectTeamMember> TeamMemberships { get; set; }

}