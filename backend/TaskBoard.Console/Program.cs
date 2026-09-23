using System;
using System.Collections.Generic;

namespace TaskBoard.ConsoleApp
{
    internal class Program
    {
       
        private static List<string> tasks = new List<string>();

        static void Main(string[] args)
        {
            Console.Title = "TaskBoard Console - Görev Takibi";
            bool isRunning = true;

            while (isRunning)
            {
                ShowMenu();
                Console.Write("\nSeçiminiz: ");
                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        ListTasks();
                        break;
                    case "2":
                        AddTask();
                        break;
                    case "0":
                        Console.WriteLine("\nUygulamadan çıkılıyor. İyi çalışmalar!");
                        isRunning = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Hatalı seçim yaptınız! Lütfen menüdeki (1, 2 veya 0) seçeneklerinden birini girin.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        
        private static void ShowMenu()
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine("    TASKBOARD CONSOLE PANEL   ");
            Console.WriteLine("==============================");
            Console.WriteLine("1 - Görevleri Listele");
            Console.WriteLine("2 - Yeni Görev Ekle");
            Console.WriteLine("0 - Çıkış");
        }

        
        private static void ListTasks()
        {
            Console.WriteLine("\n--- MEVCUT GÖREVLER ---");

            if (tasks.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Henüz kayıtlı bir görev bulunmuyor.");
                Console.ResetColor();
                return;
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }

        
        private static void AddTask()
        {
            Console.WriteLine("\n--- YENİ GÖREV EKLE ---");

            Console.Write("Görev Başlığı: ");
            string? title = Console.ReadLine()?.Trim();

            
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hata: Görev başlığı boş bırakılamaz!");
                Console.ResetColor();
                return;
            }

            Console.Write("Öncelik (1: Düşük, 2: Normal, 3: Yüksek) [Varsayılan: 2]: ");
            string? priorityChoice = Console.ReadLine()?.Trim();

            string priority = priorityChoice switch
            {
                "1" => "Düşük",
                "3" => "Yüksek",
                _ => "Normal"
            };

            string taskEntry = $"[{priority}] {title} (Tarih: {DateTime.Now:yyyy-MM-dd HH:mm})";
            tasks.Add(taskEntry);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Görev başarıyla eklendi!");
            Console.ResetColor();
        }
    }
}
