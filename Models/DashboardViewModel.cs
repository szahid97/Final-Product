using TaskTrackerDemo.Models;
using System.Collections.Generic;

namespace TaskTrackerDemo.Models
{
    public class DashboardViewModel
    {
        public List<ProjectTask> UserActiveTasks { get; set; } = new List<ProjectTask>();
        public List<ProjectTask> UserCompletedTasks { get; set; } = new List<ProjectTask>();
        public List<Project> UserProjects { get; set; } = new List<Project>();
        public List<Project> CompletedProjects { get; set; } = new List<Project>();
        public List<ProjectTask> TeamActiveTasks { get; set; } = new List<ProjectTask>();
        public List<ProjectTask> TeamCompletedTasks { get; set; } = new List<ProjectTask>();
    }
}