namespace TaskTrackerDemo.Models
{
    public class ProjectDiscussion
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public string User { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
