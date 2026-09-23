using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskBoard.ConsoleApp
{
    public class TaskService
    {
        private readonly List<TaskItem> _tasks = new List<TaskItem>();
        private int _nextId = 1;

        
        public List<TaskItem> GetAll()
        {
            return _tasks.ToList();
        }

        
        public List<TaskItem> GetByStatus(TaskStatus status)
        {
            return _tasks.Where(t => t.Status == status).ToList();
        }

        
        public (bool IsSuccess, string Message) Add(string title, string priority)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return (false, "Görev başlığı boş bırakılamaz!");
            }

            
            bool alreadyExists = _tasks.Any(t => t.Title.Equals(title.Trim(), StringComparison.OrdinalIgnoreCase));
            if (alreadyExists)
            {
                return (false, "Bu başlıkla zaten bir görev mevcut!");
            }

            var newTask = new TaskItem
            {
                Id = _nextId++,
                Title = title.Trim(),
                Priority = priority,
                Status = TaskStatus.Open,
                CreatedAt = DateTime.Now
            };

            _tasks.Add(newTask);
            return (true, "Görev başarıyla eklendi.");
        }

        
        public (bool IsSuccess, string Message) MarkAsDone(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return (false, "Belirtilen numarada bir görev bulunamadı.");
            }

            if (task.Status == TaskStatus.Done)
            {
                return (false, "Bu görev zaten tamamlanmış!");
            }

            task.Status = TaskStatus.Done;
            task.CompletedAt = DateTime.Now; 
            return (true, $"'{task.Title}' başlıklı görev tamamlandı olarak işaretlendi.");
        }
    }
}