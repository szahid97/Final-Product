using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackerDemo.Dtos
   {
       public class ProjectCreateDto
       {
           [Required(ErrorMessage = "Project name is required")]
           [StringLength(100)]
           public string Name { get; set; } = default!;
           public List<TaskCreateDto> Tasks { get; set; } = new();

           public DateTime DueDate { get; set; } = DateTime.Now.AddMonths(1);
       }

       public class TaskCreateDto
       {
           [Required(ErrorMessage = "Task title is required")]
           [StringLength(200)]
           public string Title { get; set; } = null!;

           [Required(ErrorMessage = "Please assign the task to a team member")]
           public string AssignedToId { get; set; } = null!; 
            public string AssigneeId { get; set; } = null!;

           [Required(ErrorMessage = "Due date is required")]
           [DataType(DataType.Date)]
           [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
           public DateTime? DueDate { get; set; }
       }
   }