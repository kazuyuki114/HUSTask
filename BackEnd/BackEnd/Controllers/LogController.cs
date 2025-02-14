using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models.Entities;
using BackEnd.Data;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly WebDbContext _context;

        public LogController(WebDbContext context)
        {
            _context = context;
        }
        
        /// Get logs by userId
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetLogsByUserId(int userId)
        {
            try
            {
                var logs = await _context.Set<Log>()
                    .Where(log => log.UserId == userId)
                    .OrderByDescending(log => log.Timestamp)
                    .Select(log => new
                    {
                        log.Id,
                        log.UserId,
                        log.Description,
                        Timestamp = log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") // Format Timestamp
                    })
                    .ToListAsync();

                if (logs == null || !logs.Any())
                {
                    return NotFound(new { message = "No logs found for the specified user." });
                }

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching logs.", error = ex.Message });
            }
        }

        /// Post a new log
        [HttpPost]
        public async Task<IActionResult> PostLog([FromBody] Log log)
        {
            if (log == null || log.UserId <= 0 || string.IsNullOrEmpty(log.Description))
            {
                return BadRequest(new { message = "Invalid log data." });
            }

            try
            {
                log.Timestamp = DateTime.UtcNow.AddHours(7); // Set the current timestamp
                _context.Set<Log>().Add(log);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetLogsByUserId), new { userId = log.UserId }, log);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the log.", error = ex.Message });
            }
        }
    }
}
