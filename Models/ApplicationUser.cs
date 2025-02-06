using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TaskManagerMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
