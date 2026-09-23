using System;

namespace TaskBoard.Web.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Priority { get; set; } = "normal";
        public string Status { get; set; } = "Açık";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}