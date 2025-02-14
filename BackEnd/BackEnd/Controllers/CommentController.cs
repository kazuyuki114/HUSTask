using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Models.Entities;

namespace BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly WebDbContext _context;

    public CommentController(WebDbContext context)
    {
        _context = context;
    }

    // GET: api/Comment/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Comment>> GetComment(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.commentedUser)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return NotFound();
        }

        return comment;
    }
    // GET: api/Comment/task/{taskId}
    [HttpGet("task/{taskId}")]
    public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByTaskId(int taskId)
    {
        var comments = await _context.Comments
            .Include(c => c.commentedUser) 
            .Where(c => c.TaskId == taskId)
            .ToListAsync();

        
        if (!comments.Any())
        {
            return NotFound();
        }

        return comments;
    }

    // POST: api/Comment
    [HttpPost]
    public async Task<ActionResult<Comment>> PostComment(Comment comment)
    {
        if (!_context.Users.Any(u => u.Id == comment.UserId) || !_context.Tasks.Any(t => t.Id == comment.TaskId))
        {
            return BadRequest("Invalid UserId or TaskId.");
        }

        comment.Updated_at = DateTime.UtcNow.AddHours(7);
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
    }

    // PUT: api/Comment/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(int id, Comment updatedComment)
    {
        if (id != updatedComment.Id)
        {
            return BadRequest("Comment ID mismatch.");
        }

        var existingComment = await _context.Comments.FindAsync(id);
        if (existingComment == null)
        {
            return NotFound();
        }

        existingComment.Content = updatedComment.Content;
        existingComment.Updated_at = DateTime.UtcNow.AddHours(7);

        _context.Entry(existingComment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Comment/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.Id == id);
    }
}
