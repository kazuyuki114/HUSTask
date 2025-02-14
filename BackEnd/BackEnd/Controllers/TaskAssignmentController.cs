using BackEnd.Models;
using BackEnd.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;


namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAssignmentController : ControllerBase
    {
        private readonly WebDbContext _dbContext;

        public TaskAssignmentController(WebDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to assign a user to a task
        [HttpPost("assign")]
        public async Task<IActionResult> AssignTaskToUser([FromBody] TaskAssignmentModel taskAssignment)
        {
            // Validate input
            if (taskAssignment.UserId <= 0 || taskAssignment.TaskId <= 0)
            {
                return BadRequest("Invalid TaskId or UserId.");
            }

            // Check if the task exists
            var task = await _dbContext.Tasks.FindAsync(taskAssignment.TaskId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            // Check if the user exists
            var user = await _dbContext.Users.FindAsync(taskAssignment.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the assignment already exists
            var existingAssignment = await _dbContext.PersonTaskLists
                .FirstOrDefaultAsync(ptl => ptl.TaskId == taskAssignment.TaskId && ptl.UserId == taskAssignment.UserId);

            if (existingAssignment != null)
            {
                return BadRequest("This task is already assigned to the user.");
            }

            // Create a new entry in the person_task_list table
            var personTaskList = new PersonTaskList
            {
                TaskId = taskAssignment.TaskId,
                UserId = taskAssignment.UserId
            };

            _dbContext.PersonTaskLists.Add(personTaskList);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "User assigned to task successfully." });
        }
        
        // Method to get all the task of userId
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            // Validate input
            if (userId <= 0)
            {
                return BadRequest("Invalid UserId.");
            }

            // Retrieve tasks assigned to the user
            var tasks = await _dbContext.PersonTaskLists
                .Where(ptl => ptl.UserId == userId)
                .Include(ptl => ptl.userTask)  // Include task details
                .Select(ptl => new
                {
                    ptl.userTask.Id,
                    ptl.userTask.Title,
                    ptl.userTask.Description,
                    ptl.userTask.DueDate,
                    ptl.userTask.Status,
                    ptl.userTask.PriorityId,
                    CreatedAt = ptl.userTask.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),  // Format CreatedAt
                    UpdatedAt = ptl.userTask.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")   // Format UpdatedAt
                })
                .ToListAsync();

            if (tasks.Count == 0)
            {
                return NotFound("No tasks found for this user.");
            }

            return Ok(tasks);
        }

    }
}
