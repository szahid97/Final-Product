using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackerDemo.Models
{
    public class ProjectTask
    {
        public int Id { get; set; }
        

        [Required(ErrorMessage = "Project reference is required")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        
        [Required(ErrorMessage = "Task title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = null!;

        
        [ForeignKey("AssignedUser")]
        public string AssignedToId { get; set; } = null!; // User ID of the person the task is assigned TO


        public ApplicationUser AssignedUser { get; set; }

        public string AssignedUserName { get; set; }

        
        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7); // Default 1 week from now
        
        
        public string Status { get; set; } = "Not Started";


        public DateTime StartDate { get; set; } = DateTime.Now;


        public DateTime? EndDate { get; set; }


        public Project? Project { get; set; }


        [ForeignKey("Assignee")]
        public string AssigneeId { get; set; } = null!;

        public ApplicationUser Assignee { get; set; } = null!;
    }
}