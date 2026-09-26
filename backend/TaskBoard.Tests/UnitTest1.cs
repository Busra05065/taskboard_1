using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Web.Data;
using TaskBoard.Web.Models;
using TaskBoard.Web.Services;
using Xunit;

namespace TaskBoard.Tests
{
    public class TaskServiceTests
    {
        private TaskBoardDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskBoardDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TaskBoardDbContext(options);
        }

        private TaskService CreateService(TaskBoardDbContext context)
        {
            return new TaskService(context);
        }

        [Fact]
        public async Task CreateAsync_ShouldRejectEmptyTitle()
        {
            using var context = CreateDbContext();
            var service = CreateService(context);
            var request = new CreateTaskDto { Title = "" };

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request));
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateTask_WhenValidTitleProvided()
        {
            using var context = CreateDbContext();
            var service = CreateService(context);
            var request = new CreateTaskDto { Title = "Test Görevi", Priority = "high" };

            var result = await service.CreateAsync(request);

            Assert.NotNull(result);
            Assert.Equal("Test Görevi", result.Title);
            Assert.Equal("high", result.Priority);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenTaskDoesNotExist()
        {
            using var context = CreateDbContext();
            var service = CreateService(context);

            var result = await service.DeleteAsync(9999);

            Assert.False(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTasks()
        {
            using var context = CreateDbContext();
            var service = CreateService(context);

            await service.CreateAsync(new CreateTaskDto { Title = "Görev 1" });
            await service.CreateAsync(new CreateTaskDto { Title = "Görev 2" });

            var result = await service.GetAllAsync(new TaskQuery { PageSize = 10 });

            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            using var context = CreateDbContext();
            var service = CreateService(context);
            var request = new UpdateTaskDto { Title = "Yeni Başlık" };

            var result = await service.UpdateAsync(9999, request);

            Assert.Null(result);
        }
    }
}