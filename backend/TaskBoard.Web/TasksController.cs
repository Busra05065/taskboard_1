using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TaskBoard.Web.Models;

namespace TaskBoard.Web.Controllers
{
    public class TasksController : Controller
    {
        
        private static readonly List<TaskItem> _tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Arayüzü düzenle", Description = "Header ve footer düzenlemeleri", Priority = "high", Status = "Açık" },
            new TaskItem { Id = 2, Title = "API taslağı hazırla", Description = "Endpoint sözleşmeleri", Priority = "normal", Status = "Devam Ediyor" },
            new TaskItem { Id = 3, Title = "Veritabanı bağlantısını kur", Description = "DbContext ayarları", Priority = "low", Status = "Tamamlandı" }
        };

        private static int _nextId = 4;

        
        [HttpGet]
        public IActionResult Index()
        {
            return View(_tasks);
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateTaskViewModel();
            return View(model);
        }

        
        [HttpPost]
        public IActionResult Create(CreateTaskViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                
                return View(model);
            }

            
            var newTask = new TaskItem
            {
                Id = _nextId++,
                Title = model.Title.Trim(),
                Description = model.Description?.Trim(),
                Priority = model.Priority,
                Status = "Açık"
            };

            _tasks.Add(newTask);

            
            return RedirectToAction(nameof(Index));
        }
    }
}