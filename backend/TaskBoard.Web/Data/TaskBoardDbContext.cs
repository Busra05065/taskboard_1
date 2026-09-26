using System;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Web.Models;

namespace TaskBoard.Web.Data
{
    public class TaskBoardDbContext : DbContext
    {
        public TaskBoardDbContext(DbContextOptions<TaskBoardDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem { Id = 1, Title = "Veritabanı Şeması Tasarımı", Priority = "high", Status = "open", CreatedAt = new DateTime(2026, 1, 1) },
                new TaskItem { Id = 2, Title = "EF Core Migration Hazırlığı", Priority = "normal", Status = "in-progress", CreatedAt = new DateTime(2026, 1, 2) },
                new TaskItem { Id = 3, Title = "Seed Verilerini Doğrulama", Priority = "low", Status = "completed", CreatedAt = new DateTime(2026, 1, 3) }
            );
        }
    }
}