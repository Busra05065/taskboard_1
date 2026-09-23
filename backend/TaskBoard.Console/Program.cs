using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskBoard.ConsoleApp
{
    
    public enum TaskStatus
    {
        Open = 1,
        InProgress = 2,
        Done = 3
    }

    
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Priority { get; set; } = "Normal";
        public TaskStatus Status { get; set; } = TaskStatus.Open;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
    }

    
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

    
    internal class Program
    {
        private static readonly TaskService _taskService = new TaskService();

        static void Main(string[] args)
        {
            Console.Title = "TaskBoard - OOP & LINQ Console";
            bool isRunning = true;

            while (isRunning)
            {
                ShowMenu();
                Console.Write("\nSeçiminiz: ");
                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        ListAllTasks();
                        break;
                    case "2":
                        ListOpenTasks();
                        break;
                    case "3":
                        AddNewTask();
                        break;
                    case "4":
                        CompleteTask();
                        break;
                    case "0":
                        Console.WriteLine("\nProgram sonlandırıldı. Başarılar!");
                        isRunning = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Geçersiz seçim! Lütfen menüdeki rakamlardan birini girin.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("\n=================================");
            Console.WriteLine("     TASKBOARD OOP CONSOLE       ");
            Console.WriteLine("=================================");
            Console.WriteLine("1 - Tüm Görevleri Listele");
            Console.WriteLine("2 - Sadece Açık Görevleri Listele (LINQ)");
            Console.WriteLine("3 - Yeni Görev Ekle");
            Console.WriteLine("4 - Görevi Tamamla");
            Console.WriteLine("0 - Çıkış");
        }

        private static void PrintTasks(List<TaskItem> list, string title)
        {
            Console.WriteLine($"\n--- {title} ---");
            if (list.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Listelenecek görev bulunamadı.");
                Console.ResetColor();
                return;
            }

            foreach (var task in list)
            {
                string completedInfo = task.CompletedAt.HasValue 
                    ? $" (Tamamlandı: {task.CompletedAt.Value:HH:mm:ss})" 
                    : "";

                Console.WriteLine($"[ID: {task.Id}] [{task.Priority}] {task.Title} | Durum: {task.Status}{completedInfo}");
            }
        }

        private static void ListAllTasks()
        {
            var tasks = _taskService.GetAll();
            PrintTasks(tasks, "TÜM GÖREVLER");
        }

        private static void ListOpenTasks()
        {
            var openTasks = _taskService.GetByStatus(TaskStatus.Open);
            PrintTasks(openTasks, "AÇIK GÖREVLER (LINQ)");
        }

        private static void AddNewTask()
        {
            Console.WriteLine("\n--- YENİ GÖREV EKLE ---");
            Console.Write("Görev Başlığı: ");
            string? title = Console.ReadLine();

            Console.Write("Öncelik (1: Düşük, 2: Normal, 3: Yüksek) [Varsayılan: 2]: ");
            string? priorityChoice = Console.ReadLine()?.Trim();

            string priority = priorityChoice switch
            {
                "1" => "Düşük",
                "3" => "Yüksek",
                _ => "Normal"
            };

            var result = _taskService.Add(title ?? string.Empty, priority);
            if (result.IsSuccess)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result.Message);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Hata: {result.Message}");
            }
            Console.ResetColor();
        }

        private static void CompleteTask()
        {
            Console.WriteLine("\n--- GÖREVİ TAMAMLA ---");
            Console.Write("Tamamlanacak Görev ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var result = _taskService.MarkAsDone(id);
                Console.ForegroundColor = result.IsSuccess ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine(result.Message);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hata: Geçerli bir sayı girmelisiniz!");
                Console.ResetColor();
            }
        }
    }
}