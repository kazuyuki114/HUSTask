namespace BackEnd.Models;

public class UpdateTaskModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateOnly DueDate { get; set; }
    public int PriorityId { get; set; }
}