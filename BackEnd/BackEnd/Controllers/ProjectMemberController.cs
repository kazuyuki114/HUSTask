using Microsoft.AspNetCore.Mvc;
using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;

using BackEnd.Data;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectMemberController : ControllerBase
    {
        private readonly WebDbContext _context;

        public ProjectMemberController(WebDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjectMember/project/{projectId}
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMembersByProject(int projectId)
        {
            var members = await _context.ProjectMembers
                .Where(pm => pm.ProjectId == projectId)
                .Include(pm => pm.User)
                .Select(pm => new
                {
                    pm.UserId,
                    pm.ProjectId,
                    pm.Role,
                    User = new
                    {
                        pm.User.Name,
                        pm.User.Email // Exclude password and other sensitive fields
                    }
                })
                .ToListAsync();

            if (!members.Any())
            {
                return NotFound($"No members found for ProjectId {projectId}.");
            }

            return Ok(members);
        }


        // GET: api/ProjectMember/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetProjectsByUser(int userId)
        {
            var projects = await _context.ProjectMembers
                .Where(pm => pm.UserId == userId)
                .Include(pm => pm.Project) // Include the related Project entity
                .Select(pm => new 
                {
                    Project = pm.Project, // Full project information
                    UserRole = pm.Role // User's role in the project
                })
                .ToListAsync();

            if (!projects.Any())
            {
                return NotFound($"No projects found for UserId {userId}.");
            }

            return Ok(projects);
        }
        
        // GET: api/ProjectMember/project/{projectId}/user/{userId}
        [HttpGet("project/{projectId}/user/{userId}")]
        public async Task<ActionResult<object>> GetUserRoleInProject(int projectId, int userId)
        {
            var projectMember = await _context.ProjectMembers
                .Where(pm => pm.ProjectId == projectId && pm.UserId == userId)
                .Select(pm => new 
                {
                    pm.Role // The user's role in the project (e.g., Leader, Member, etc.)
                })
                .FirstOrDefaultAsync();

            if (projectMember == null)
            {
                return NotFound($"User with ID {userId} is not a member of the project with ID {projectId}.");
            }

            return Ok(projectMember);
        }
        
        // POST: api/ProjectMember
        [HttpPost]
        public async Task<ActionResult<ProjectMember>> CreateProjectMember([FromBody] ProjectMember projectMember)
        {
            if (projectMember == null)
            {
                return BadRequest("ProjectMember data is required.");
            }
            projectMember.ParticipatedAt = DateTime.UtcNow.AddHours(7);
            _context.ProjectMembers.Add(projectMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMembersByProject), new { projectId = projectMember.ProjectId }, projectMember);
        }

        // DELETE: api/ProjectMember?userId={userId}&projectId={projectId}
        [HttpDelete]
        public async Task<IActionResult> DeleteProjectMember([FromQuery] int userId, [FromQuery] int projectId)
        {
            var projectMember = await _context.ProjectMembers.FirstOrDefaultAsync(pm => pm.UserId == userId && pm.ProjectId == projectId);
            if (projectMember == null)
            {
                return NotFound($"ProjectMember with UserId {userId} and ProjectId {projectId} not found.");
            }

            _context.ProjectMembers.Remove(projectMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
