using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackerDemo.Models
{
    public class Project
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Project name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;
        
        [Required]
        public string CreatedBy { get; set; } = null!;
        
        [ForeignKey("Assignee")]
        public string AssigneeId { get; set; }
        public ApplicationUser Assignee { get; set; }
        public virtual List<ProjectTeamMember> TeamMembers { get; set; } = new();
        public virtual ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
        [NotMapped]
        public int TaskCount => Tasks?.Count ?? 0;
        public virtual List<ProjectDiscussion> Discussions { get; set; } = new();
        public DateTime? CompletionDate { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddMonths(1); // Default 1 month duration
        public ICollection<ProjectViewer> Viewers { get; set; } = new List<ProjectViewer>();
    }
}
