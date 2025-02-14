using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;

namespace BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriorityController : ControllerBase
{
    private readonly WebDbContext _dbContext;

    public PriorityController(WebDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get all priorities
    [HttpGet]
    public async Task<IActionResult> GetAllPriorities()
    {
        var priorities = await _dbContext.Priorities.ToListAsync();
        return Ok(priorities);
    }
}