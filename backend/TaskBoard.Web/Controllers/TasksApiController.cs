using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Web.Models;
using TaskBoard.Web.Services;

namespace TaskBoard.Web.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksApiController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksApiController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound(new { message = $"ID: {id} olan görev bulunamadı." });
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto request)
        {
            try
            {
                var result = await _taskService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto request)
        {
            try
            {
                var result = await _taskService.UpdateAsync(id, request);
                if (result == null) return NotFound(new { message = $"ID: {id} olan kayıt bulunamadı." });
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _taskService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = $"ID: {id} olan kayıt bulunamadı." });
            return NoContent();
        }
    }
}