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

namespace TaskTrackerDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var teamMemberIds = await _context.MyTeams
                .Where(mt => mt.UserId == userId)
                .Select(mt => mt.TeamMemberId)
                .ToListAsync();

            var model = new DashboardViewModel();

                // Tasks assigned to the User
                model.UserActiveTasks = await _context.ProjectTasks
                    .Where(t => t.AssignedToId == userId && t.Status != "Completed")
                    .Include(t => t.Project)
                    .OrderBy(t => t.DueDate)
                    .AsNoTracking()
                    .ToListAsync();

                model.UserCompletedTasks = await _context.ProjectTasks
                    .Where(t => t.AssignedToId == userId && t.Status == "Completed")
                    .Include(t => t.Project)
                    .OrderByDescending(t => t.EndDate)
                    .AsNoTracking()
                    .ToListAsync();

                // Projects where User created or has tasks assigned to him/her
                model.UserProjects = await _context.Projects
                    .Where(p => (p.AssigneeId == userId || p.Tasks.Any(t => t.AssignedToId == userId)) && !p.CompletionDate.HasValue)
                    .Include(p => p.Tasks)
                    .OrderByDescending(p => p.StartDate)
                    .AsNoTracking()
                    .ToListAsync();

                model.CompletedProjects = await _context.Projects
                    .Where(p => (p.AssigneeId == userId || p.Tasks.Any(t => t.AssignedToId == userId)) && p.CompletionDate.HasValue)
                    .Include(p => p.Tasks) 
                    .OrderByDescending(p => p.CompletionDate)
                    .AsNoTracking()
                    .ToListAsync();

                // Team tasks
                model.TeamActiveTasks = await _context.ProjectTasks
                    .Where(t => teamMemberIds.Contains(t.AssignedToId) && t.Status != "Completed")
                    .Include(t => t.Project)
                    .OrderBy(t => t.DueDate)
                    .AsNoTracking()
                    .ToListAsync();

                model.TeamCompletedTasks = await _context.ProjectTasks
                    .Where(t => teamMemberIds.Contains(t.AssignedToId) && t.Status == "Completed")
                    .Include(t => t.Project)
                    .OrderByDescending(t => t.EndDate)
                    .AsNoTracking()
                    .ToListAsync();

            return View(model);
        }




        
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }






        public async Task<IActionResult> ProjectDetails(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Discussions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }





        // Project Actions
        public async Task<IActionResult> Projects()
        {
            var projects = await _context.Projects
            .Include(p => p.Tasks)
            .ToListAsync();
            return View(projects);
        }

        
        
        
        
        //Private Helper for team-member-loading logic
        private async Task PopulateTeamMembersInViewBag()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var myTeamIds = await _context.MyTeams
                .Where(mt => mt.UserId == currentUser.Id)
                .Select(mt => mt.TeamMemberId)
                .ToListAsync();

            var teamMembers = await _context.Users
                .Where(u => myTeamIds.Contains(u.Id)|| u.Id == currentUser.Id) 
                .ToListAsync();

            ViewBag.TeamMembers = teamMembers;
        }






        [HttpGet]
        public async Task<IActionResult> CreateProject()
        {
            try
            {
                // Get team members for the dropdown
                var currentUser = await _userManager.GetUserAsync(User);
                var teamMembers = await _context.MyTeams
                    .Where(t => t.UserId == currentUser.Id)
                    .Include(t => t.TeamMember)
                    .Select(t => t.TeamMember)
                    .ToListAsync();

                if (!teamMembers.Any(m => m.Id == currentUser.Id))
                    {
                        teamMembers.Add(currentUser);
                    }

                ViewBag.TeamMembers = teamMembers ?? new List<ApplicationUser>();
                ViewBag.CurrentUserId = currentUser.Id;

                ViewBag.TaskTemplates = await _context.TaskTemplates
                    .Where(t => t.UserId == currentUser.Id)
                    .Select(t => new { t.Id, t.TemplateName })
                    .ToListAsync();
                
                return View(new ProjectCreateDto());
            }
            catch (Exception ex)
            {
                // Log error
                return View("Error");
            }
        }





        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate TeamMembers if validation fails
                var user = await _userManager.GetUserAsync(User); 
                ViewBag.TeamMembers = await _context.MyTeams
                    .Where(t => t.UserId == user.Id)
                    .Include(t => t.TeamMember)
                    .Select(t => t.TeamMember)
                    .ToListAsync();
                    
                ViewBag.CurrentUserId = user.Id;
                return View(model);
            }

            var currentUser = await _userManager.GetUserAsync(User);

            // Create project
            var project = new Project
            {
                Name = model.Name,
                CreatedBy = currentUser.FullName,
                AssigneeId = currentUser.Id,
                StartDate = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(1) // Default 1 month duration
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Add tasks
                foreach (var taskDto in model.Tasks)
                {
                    var assignedUser = await _userManager.FindByIdAsync(taskDto.AssignedToId);
                    var task = new ProjectTask
                    {
                        Title = taskDto.Title,
                        AssignedToId = taskDto.AssignedToId,
                        AssignedUserName = assignedUser.FullName,
                        AssigneeId = currentUser.Id,
                        DueDate = taskDto.DueDate.Value,
                        ProjectId = project.Id,
                        StartDate = DateTime.Now,
                        Status = "Not Started"
                    };

                    _context.ProjectTasks.Add(task);
                }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
    