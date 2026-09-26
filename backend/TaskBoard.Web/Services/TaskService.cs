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
            var task = new TaskItem
            {
                Title = request.Title.Trim(),
                Description = request.Description,
                Priority = string.IsNullOrWhiteSpace(request.Priority) ? "normal" : request.Priority,
                Status = "open",
                CreatedAt = DateTime.UtcNow
            };

            _context.TaskItems.Add(task);
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

        public async Task<TaskResponse?> UpdateAsync(int id, UpdateTaskDto request)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return null;

            task.Title = request.Title.Trim();
            if (request.Description != null) task.Description = request.Description;
            if (!string.IsNullOrWhiteSpace(request.Priority)) task.Priority = request.Priority;

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
            if (task == null) return false;

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}