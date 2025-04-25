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
    public class ProjectDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectDetailsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: ProjectDetails/5
        public async Task<IActionResult> ProjectDetails(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.AssignedUser)
                .Include(p => p.Discussions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            await PopulateTeamMembersInViewBag();
            var users = await _userManager.Users.ToListAsync();
            var viewModel = new ProjectDetailsViewModel
              {
                Project = project,
                AllUsers = users
            };
            return View(viewModel);
        }


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


            [HttpPost]
            public async Task<IActionResult> EditTask(int taskId, string title, DateTime dueDate)
            {
                var task = await _context.ProjectTasks.FindAsync(taskId);
                if (task == null)
                {
                    return NotFound();
                }

                var oldTitle = task.Title;
                var oldDueDate = task.DueDate;

                task.Title = title;
                task.DueDate = dueDate;
                _context.Update(task);

                // Add discussion entry
                var currentUser = await _userManager.GetUserAsync(User);
                var message = $"{currentUser.FullName} changed task '{oldTitle}' (Due: {oldDueDate:dd MMM yyyy}) to '{title}' (Due: {dueDate:dd MMM yyyy})";
                await AddDiscussionEntry(task.ProjectId, message);

                await _context.SaveChangesAsync();
                return Ok();
            }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var task = await _context.ProjectTasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            var projectId = task.ProjectId;
            var taskTitle = task.Title;

            _context.ProjectTasks.Remove(task);

            // Add discussion entry
            var currentUser = await _userManager.GetUserAsync(User);
            var message = $"{currentUser.FullName} deleted task '{taskTitle}'";
            await AddDiscussionEntry(projectId, message);

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> ReassignTask(int taskId, string assignedToId)
        {
            var task = await _context.ProjectTasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            var oldAssignee = await _userManager.FindByIdAsync(task.AssignedToId);
            var newAssignee = await _userManager.FindByIdAsync(assignedToId);

            task.AssignedToId = assignedToId;
            _context.Update(task);

            // Add discussion entry
            var currentUser = await _userManager.GetUserAsync(User);
            var message = $"{currentUser.FullName} reassigned task '{task.Title}' from {oldAssignee.FullName} to {newAssignee.FullName}";
            await AddDiscussionEntry(task.ProjectId, message);

            await _context.SaveChangesAsync();
            return Ok();
        }

        private async Task AddDiscussionEntry(int projectId, string message)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var discussion = new ProjectDiscussion
            {
                ProjectId = projectId,
                User = currentUser.FullName,
                Message = message,
                Timestamp = DateTime.Now
            };
            _context.ProjectDiscussions.Add(discussion);
            await _context.SaveChangesAsync();
        }

        //Change Status in Project Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .ThenInclude(p => p.Tasks)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null) 
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (task.AssignedToId != userId)
                return Forbid();

            task.Status = status;

            var project = task.Project;
            if (project.Tasks.All(t => t.Status == "Completed"))
            {
                project.CompletionDate = DateTime.Now;
            }
            else
            {
                // Optional: Reset CompletionDate if project becomes incomplete again
                project.CompletionDate = null;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        //Assigning Viewers to Project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddViewers(int projectId, List<string> viewerIds)
        {
            var project = await _context.Projects
                .Include(p => p.Viewers)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            var currentUser = await _userManager.GetUserAsync(User);
            if (project == null || project.AssigneeId != currentUser.Id)
                return Forbid();

            foreach (var viewerId in viewerIds)
            {
                if (!project.Viewers.Any(v => v.ViewerId == viewerId))
                {
                    project.Viewers.Add(new ProjectViewer
                    {
                        ViewerId = viewerId,
                        ProjectId = projectId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ProjectDetails", new { id = projectId });
        }


        //Add message to discussion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDiscussion(int projectId, string message)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var discussion = new ProjectDiscussion
            {
                ProjectId = projectId,
                User = currentUser.FullName,
                Message = message,
                Timestamp = DateTime.Now
            };

            _context.ProjectDiscussions.Add(discussion);
            await _context.SaveChangesAsync();

            // Redirect back to the same page to show updated discussions
            return RedirectToAction("ProjectDetails", new { id = projectId });
        }

    }

}