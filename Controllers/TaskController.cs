using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TaskManagerMVC.Models;
using TaskManagerMVC.Data;
using System.Security.Claims;

[Authorize]
public class TaskController : Controller
{
    private readonly ApplicationDbContext _context;

    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var tasks = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        return View(tasks);
    }

    [Route("task/create")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // GET: Task/:id
    [HttpGet]
    [Route("task/{id}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task == null) return Forbid();
        
        return View(task);
    }

    // GET: Task/Edit/:id
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task == null) return Forbid();
        
        return View(task);
    }

    // POST: Task/Create
    [Route("task/create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description")] TaskItem task)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            task.UserId = userId;

            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(task);
    }

    // POST: Task/Edit/:id
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted")] TaskItem task)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id != task.Id && userId != task.UserId) return NotFound();

        if (ModelState.IsValid)
        {
            task.UserId = userId;
            _context.Update(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = task.Id });
        }
        return View(task);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
