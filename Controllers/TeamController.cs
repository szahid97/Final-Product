using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerDemo.Models;
using TaskTrackerDemo.Data;

namespace TaskTrackerDemo.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TeamController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        //SearchAvailableUsers 
        [HttpGet]
        public async Task<IActionResult> SearchAvailableUsers(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Json(new List<string>());
            }

            var suggestions = await _context.Users
                .Where(u => u.FullName.Contains(searchTerm))
                .Select(u => u.FullName)
                .Take(5)
                .ToListAsync();

            return Json(suggestions);
        }

        //SearchMyTeam
        [HttpGet]
        public async Task<IActionResult> SearchMyTeam(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Json(new List<string>());
            }

            var currentUser = await _userManager.GetUserAsync(User);

            var myTeamMemberIds = await _context.MyTeams
                .Where(t => t.UserId == currentUser.Id)
                .Select(t => t.TeamMemberId)
                .ToListAsync();

            var suggestions = await _context.Users
                .Where(u => myTeamMemberIds.Contains(u.Id) && u.FullName.Contains(searchTerm))
                .Select(u => u.FullName)
                .Take(5)
                .ToListAsync();

            return Json(suggestions);
        }



        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var allUsers = await _userManager.Users.ToListAsync();

            // Get IDs of users already in team
            var myTeamIds = await _context.MyTeams
                .Where(t => t.UserId == currentUser.Id)
                .Select(t => t.TeamMemberId)
                .ToListAsync();

            // Filter out self and already added members
            var availableUsers = allUsers
                .Where(u => u.Id != currentUser.Id && !myTeamIds.Contains(u.Id))
                .ToList();

            // If search applied
            if (!string.IsNullOrEmpty(searchTerm))
            {
                availableUsers = availableUsers
                    .Where(u => u.FullName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                            || u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                ViewBag.SearchTerm = searchTerm;
            }

            var myTeamMembers = allUsers
                .Where(u => myTeamIds.Contains(u.Id))
                .ToList();

            var viewModel = new MyTeamViewModel
            {
                AvailableUsers = availableUsers,
                MyTeamMembers = myTeamMembers
            };

            return View(viewModel);
        }

        //AddToTeamButton
        [HttpPost]
        public async Task<IActionResult> AddToTeam([FromBody] List<string> selectedUserIds)
        {
            if (selectedUserIds == null || !selectedUserIds.Any())
            {
                return BadRequest("No users selected.");
            }

            var currentUser = await _userManager.GetUserAsync(User);

            foreach (var userId in selectedUserIds)
            {
                var alreadyExists = await _context.MyTeams
                    .AnyAsync(t => t.UserId == currentUser.Id && t.TeamMemberId == userId);

                if (!alreadyExists)
                {
                    var teamEntry = new MyTeam
                    {
                        UserId = currentUser.Id,
                        TeamMemberId = userId
                    };

                    _context.MyTeams.Add(teamEntry);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        //RemoveFromTeamButton
        [HttpPost]
        public async Task<IActionResult> RemoveFromTeam([FromBody] List<string> teamMemberIds)
        {
            if (teamMemberIds == null || !teamMemberIds.Any())
            {
                return BadRequest("No users selected.");
            }

            var currentUser = await _userManager.GetUserAsync(User);

            var entriesToRemove = await _context.MyTeams
                .Where(t => t.UserId == currentUser.Id && teamMemberIds.Contains(t.TeamMemberId))
                .ToListAsync();

            _context.MyTeams.RemoveRange(entriesToRemove);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

    }
}
