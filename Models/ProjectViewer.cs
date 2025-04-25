using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackerDemo.Models
{
    public class ProjectViewer
    {
        public int Id { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [ForeignKey("Viewer")]
        public string ViewerId { get; set; }

        public Project Project { get; set; }
        public ApplicationUser Viewer { get; set; }
    }
}
