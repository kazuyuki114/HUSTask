using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Entities
{       
    [Table("task")] 
    public class UserTask
    
    {   [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("due_date")]
        public DateOnly? DueDate { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [Column("priority_id")]
        public int? PriorityId { get; set; }

        // Navigation property for the foreign key relationship
        public Priority? Priority { get; set; }
    }
}
