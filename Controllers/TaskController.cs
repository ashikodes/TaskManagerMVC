using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerMVC.Models;
using TaskManagerMVC.Data;

public class TaskController : Controller
{
    private readonly ApplicationDbContext _context;

    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var tasks = _context.Tasks.ToList();
        return View(tasks);
    }

    [Route("task/create")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // GET: Task/:id
    [Route("task/{id}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null) return NotFound();
        
        return View(task);
    }

    // GET: Task/Edit/:id
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        
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
        if (id != task.Id) return NotFound();

        if (ModelState.IsValid)
        {
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
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
