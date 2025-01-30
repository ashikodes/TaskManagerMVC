using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManagerMVC.Models;
using TaskManagerMVC.Data;

public class TaskItemsController : Controller
{
    private readonly ApplicationDbContext _context;

    public TaskItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Action methods to manage TaskItems...
}
