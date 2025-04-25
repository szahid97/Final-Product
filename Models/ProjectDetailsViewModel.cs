namespace TaskTrackerDemo.Models
{
    using System.Collections.Generic;
    using TaskTrackerDemo.Models;

    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }
        public List<ApplicationUser> AllUsers { get; set; }
    }
}
