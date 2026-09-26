using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskBoard.Web.Data;
using TaskBoard.Web.Models;

namespace TaskBoard.Web.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskBoardDbContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskService(TaskBoardDbContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TaskResponse>> GetAllAsync()
        {
            return await _context.TaskItems
                .Select(t => new TaskResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<TaskResponse?> GetByIdAsync(int id)
        {
            var t = await _context.TaskItems.FindAsync(id);
            if (t == null) return null;

            return new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            };
        }

        public async Task<TaskResponse> CreateAsync(CreateTaskDto request)
        {
            
            if (string.IsNullOrWhiteSpace(request?.Title))
            {
                _logger.LogWarning("Geçersiz görev ekleme denemesi: Başlık boş.");
                throw new ArgumentException("Görev başlığı zorunludur.");
            }

            var cleanTitle = request.Title.Trim();
            _logger.LogInformation("Yeni görev oluşturuluyor: {Title}", cleanTitle);

            var task = new TaskItem
            {
                Title = cleanTitle,
                Description = request.Description?.Trim(),
                Priority = string.IsNullOrWhiteSpace(request.Priority) ? "normal" : request.Priority.Trim().ToLower(),
                Status = "open",
                CreatedAt = DateTime.UtcNow
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Görev başarıyla oluşturuldu. ID: {TaskId}", task.Id);

            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<TaskResponse?> UpdateAsync(int id, UpdateTaskDto request)
        {
            
            if (string.IsNullOrWhiteSpace(request?.Title))
            {
                _logger.LogWarning("Geçersiz görev güncelleme denemesi: ID {TaskId} için başlık boş.", id);
                throw new ArgumentException("Görev başlığı zorunludur.");
            }

            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                _logger.LogWarning("Güncellenmek istenen görev bulunamadı. ID: {TaskId}", id);
                return null;
            }

            var cleanTitle = request.Title.Trim();
            _logger.LogInformation("Görev güncelleniyor. ID: {TaskId}, Yeni Başlık: {Title}", id, cleanTitle);

            task.Title = cleanTitle;
            if (request.Description != null) task.Description = request.Description.Trim();
            if (!string.IsNullOrWhiteSpace(request.Priority)) task.Priority = request.Priority.Trim().ToLower();

            await _context.SaveChangesAsync();

            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                CreatedAt = task.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                _logger.LogWarning("Silinmek istenen görev bulunamadı. ID: {TaskId}", id);
                return false;
            }

            _logger.LogInformation("Görev siliniyor. ID: {TaskId}", id);
            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Görev başarıyla silindi. ID: {TaskId}", id);
            return true;
        }
    }
}