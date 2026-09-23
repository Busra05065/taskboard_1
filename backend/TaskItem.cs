using System;

namespace TaskBoard.ConsoleApp
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Priority { get; set; } = "Normal";
        public TaskStatus Status { get; set; } = TaskStatus.Open;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
    }
}