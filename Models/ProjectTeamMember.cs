using TaskTrackerDemo.Models;
using Microsoft.AspNetCore.Identity;

public class ProjectTeamMember
{
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; }
}