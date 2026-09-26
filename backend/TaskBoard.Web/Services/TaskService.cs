using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Web.Data;
using TaskBoard.Web.Models;

namespace TaskBoard.Web.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskBoardDbContext _context;

        public TaskService(TaskBoardDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskResponse>> GetAllAsync()
        {
            return await _context.TaskItems
                .Select(t => MapToResponse(t))
                .ToListAsync();
        }

        public async Task<TaskResponse?> GetByIdAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            return task == null ? null : MapToResponse(task);
        }

        public async Task<TaskResponse> CreateAsync(CreateTaskDto request)
        {
            // Testin beklediği hata fırlatma kontrolü (Guard Clause)
            if (string.IsNullOrWhiteSpace(request?.Title))
            {
                throw new ArgumentException("Görev başlığı zorunludur.");
            }

            var task = new TaskItem
            {
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                Priority = string.IsNullOrWhiteSpace(request.Priority) ? "normal" : request.Priority.Trim().ToLower(),
                Status = "open",
                CreatedAt = DateTime.UtcNow
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();

            return MapToResponse(task);
        }

        public async Task<TaskResponse?> UpdateAsync(int id, UpdateTaskDto request)
        {
            if (string.IsNullOrWhiteSpace(request?.Title))
            {
                throw new ArgumentException("Görev başlığı zorunludur.");
            }

            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return null;

            task.Title = request.Title.Trim();
            if (request.Description != null) task.Description = request.Description.Trim();
            if (!string.IsNullOrWhiteSpace(request.Priority)) task.Priority = request.Priority.Trim().ToLower();

            await _context.SaveChangesAsync();

            return MapToResponse(task);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return false;

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        private static TaskResponse MapToResponse(TaskItem t)
        {
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
    }
}