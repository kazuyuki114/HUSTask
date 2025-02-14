using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Entities;
[Table("task_tag_link")]
public class TaskTagLink
{   
    [Column("task_id")]
    int TaskId { get; set; }
    [Column("tag_id")]
    int TagId { get; set; }
    
}