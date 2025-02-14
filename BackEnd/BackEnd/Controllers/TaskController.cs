using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Models.Entities;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly WebDbContext _dbContext;

        public TaskController(WebDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddTask(UserTask userTask)
        {
            var currentTime = DateTime.UtcNow.AddHours(7);

            // Ensure the backend sets CreatedAt and UpdatedAt
            userTask.CreatedAt = currentTime;
            userTask.UpdatedAt = currentTime;

            _dbContext.Tasks.Add(userTask);
            await _dbContext.SaveChangesAsync();

            return Ok(userTask);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            // Retrieve the task by ID
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            return Ok(task);
        }
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var task = await _dbContext.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Task deleted successfully." });
        }
        
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskModel updatedTask)
        {
            // Find the existing task by ID
            var existingTask = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (existingTask == null)
            {
                return NotFound("Task not found.");
            }

            // Update the fields that can be modified
            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.Status = updatedTask.Status;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.PriorityId = updatedTask.PriorityId;

            // Automatically update the UpdatedAt field
            existingTask.UpdatedAt = DateTime.UtcNow.AddHours(7);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();
            return Ok(existingTask);
        }


    }
}
