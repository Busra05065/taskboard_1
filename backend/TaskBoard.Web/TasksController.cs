using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TaskBoard.Web.Models;

namespace TaskBoard.Web.Controllers
{
    public class TasksController : Controller
    {
        
        public IActionResult Index()
        {
            
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Title = "Arayüzü düzenle", Priority = "high", Status = "Açık" },
                new TaskItem { Id = 2, Title = "API taslağı hazırla", Priority = "normal", Status = "Devam Ediyor" },
                new TaskItem { Id = 3, Title = "Veritabanı bağlantısını kur", Priority = "low", Status = "Tamamlandı" }
            };

            return View(tasks);
        }
    }
}