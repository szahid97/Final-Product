using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using TaskTrackerDemo.Data;
using TaskTrackerDemo.Models;
using TaskTrackerDemo.Dtos;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

public class WorkstationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public WorkstationController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Workstation()
{
    var currentUser = await _userManager.GetUserAsync(User);
    var currentUserId = currentUser.Id;
    var isManager = currentUser.IsManager;

    // Get all relevant project IDs first (more efficient)
    var projectIds = new HashSet<int>();

    // 1. Projects I created
    var myProjectIds = await _context.Projects
        .Where(p => p.AssigneeId == currentUserId)
        .Select(p => p.Id)
        .ToListAsync();
    projectIds.UnionWith(myProjectIds);

    // 2. Projects where I have active tasks
    var projectsWithMyTasksIds = await _context.ProjectTasks
        .Where(t => t.AssignedToId == currentUserId && t.Status != "Completed")
        .Select(t => t.ProjectId)
        .Distinct()
        .ToListAsync();
    projectIds.UnionWith(projectsWithMyTasksIds);

    // 3. Projects where I’m a viewer
    var viewerProjectIds = await _context.ProjectViewers
        .Where(v => v.ViewerId == currentUserId)
        .Select(v => v.ProjectId)
        .ToListAsync();
    projectIds.UnionWith(viewerProjectIds);

    if (isManager)
    {
        // 3. Projects where my team has active tasks
        var teamMemberIds = await _context.Users
            .Where(u => u.ManagerId == currentUserId)
            .Select(u => u.Id)
            .ToListAsync();

        var projectsWithTeamTasksIds = await _context.ProjectTasks
            .Where(t => teamMemberIds.Contains(t.AssignedToId) && t.Status != "Completed")
            .Select(t => t.ProjectId)
            .Distinct()
            .ToListAsync();
        projectIds.UnionWith(projectsWithTeamTasksIds);
    }

    // Now get full project details with tasks
    var projects = await _context.Projects
        .Where(p => projectIds.Contains(p.Id))
        .Include(p => p.Tasks)
        .OrderByDescending(p => p.StartDate)
        .ToListAsync();

    // Debug output (check your server logs)
    Console.WriteLine($"Total projects found: {projects.Count}");
    Console.WriteLine($"Projects I created: {myProjectIds.Count}");
    Console.WriteLine($"Projects with my tasks: {projectsWithMyTasksIds.Count}");
    return View(projects);
}
}