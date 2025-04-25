using System.Collections.Generic;
using TaskTrackerDemo.Models;

namespace TaskTrackerDemo.Models
{
    public class MyTeamViewModel
    {
        public List<ApplicationUser> AvailableUsers { get; set; } = new List<ApplicationUser>();
        public List<ApplicationUser> MyTeamMembers { get; set; } = new List<ApplicationUser>();
    }
}
