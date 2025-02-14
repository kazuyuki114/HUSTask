using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models.Entities;
using BackEnd.Data;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectAssignmentController : ControllerBase
    {
        private readonly WebDbContext _context;

        public ProjectAssignmentController(WebDbContext context)
        {
            _context = context;
        }
        
        // GET: api/ProjectAssignment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectAssignment>> GetProjectAssignment(int id)
        {
            var projectAssignment = await _context.ProjectAssignments
                                                   .Include(pa => pa.Project)
                                                   .Include(pa => pa.User)
                                                   .Include(pa => pa.ProjectTask)
                                                   .FirstOrDefaultAsync(pa => pa.Id == id);

            if (projectAssignment == null)
            {
                return NotFound();
            }

            return Ok(projectAssignment);
        }
        
        // GET: api/ProjectAssignment/user/{userId}/project/{projectId}
        [HttpGet("user/{userId}/project/{projectId}")]
        public async Task<ActionResult<IEnumerable<ProjectAssignment>>> GetTasksForUserInProject(int userId, int projectId)
        {
            try
            {
                var projectAssignments = await _context.ProjectAssignments
                    .Where(pa => pa.UserId == userId && pa.ProjectId == projectId)
                    .Include(pa => pa.Project)
                    .Include(pa => pa.User)
                    .Include(pa => pa.ProjectTask) // Refers to `task_id`
                    .ToListAsync();

                if (projectAssignments == null || !projectAssignments.Any())
                {
                    return NotFound(new { Message = "No assignments found for this user and project." });
                }

                return Ok(projectAssignments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        // POST: api/ProjectAssignment
        [HttpPost]
        public async Task<ActionResult<ProjectAssignment>> PostProjectAssignment(ProjectAssignment projectAssignment)
        {
            if (projectAssignment == null)
            {
                return BadRequest();
            }

            projectAssignment.AssignedTime = DateTime.UtcNow.AddHours(7);
            _context.ProjectAssignments.Add(projectAssignment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjectAssignment), new { id = projectAssignment.Id }, projectAssignment);
        }
        
        // DELETE: api/ProjectAssignment/project/{projectId}/task/{taskId}/user/{userId}
        [HttpDelete("project/{projectId}/task/{taskId}/user/{userId}")]
        public async Task<IActionResult> DeleteProjectAssignment(int projectId, int taskId, int userId)
        {
            var projectAssignment = await _context.ProjectAssignments
                .FirstOrDefaultAsync(pa => pa.ProjectId == projectId 
                                           && pa.TaskId == taskId 
                                           && pa.UserId == userId);

            if (projectAssignment == null)
            {
                return NotFound();
            }

            _context.ProjectAssignments.Remove(projectAssignment);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

    }
}
