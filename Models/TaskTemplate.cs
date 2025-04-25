public class TaskTemplate
{
    public int Id { get; set; }
    public string TemplateName { get; set; }
    public List<TemplateTask> Tasks { get; set; } = new();
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}
