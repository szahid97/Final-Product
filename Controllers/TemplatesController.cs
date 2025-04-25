using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerDemo.Data;
using Microsoft.AspNetCore.Identity;

[Route("Templates")]
public class TemplatesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;


    public TemplatesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] TemplateSaveDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Tasks == null || !dto.Tasks.Any())
            return BadRequest("Invalid template");

        var userId = _userManager.GetUserId(User);
        var template = new TaskTemplate
        {
            TemplateName = dto.Name,
            UserId = userId,
            Tasks = dto.Tasks.Select(t => new TemplateTask { Title = t }).ToList()
        };

        _context.TaskTemplates.Add(template);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var userId = _userManager.GetUserId(User);

        var template = await _context.TaskTemplates
            .Include(t => t.Tasks)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (template == null) return NotFound();
        return Json(template);
    }

    [HttpGet("All")]
    public async Task<IActionResult> All()
    {
        var templates = await _context.TaskTemplates
            .Select(t => new { t.Id, t.TemplateName })
            .ToListAsync();

        return Json(templates);
    }
}
