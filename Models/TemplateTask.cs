public class TemplateTask
{
    public int Id { get; set; }
    public string Title { get; set; }

    public int TaskTemplateId { get; set; }
    public TaskTemplate TaskTemplate { get; set; }
}
