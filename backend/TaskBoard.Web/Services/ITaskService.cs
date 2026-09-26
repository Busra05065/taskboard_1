using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBoard.Web.Models;

namespace TaskBoard.Web.Services
{
    public interface ITaskService
    {
        Task<PagedResult<TaskResponse>> GetAllAsync(TaskQuery? query = null);
        Task<TaskResponse?> GetByIdAsync(int id);
        Task<TaskResponse> CreateAsync(CreateTaskDto request);
        Task<TaskResponse?> UpdateAsync(int id, UpdateTaskDto request);
        Task<bool> DeleteAsync(int id);
    }
}