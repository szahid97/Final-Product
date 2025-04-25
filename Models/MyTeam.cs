using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackerDemo.Models
{
    public class MyTeam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // The user who is creating the team

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public string TeamMemberId { get; set; }  // The user added to the team

        [ForeignKey("TeamMemberId")]
        public ApplicationUser TeamMember { get; set; }
    }
}
