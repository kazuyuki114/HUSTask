using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Entities;
[Table("project_assignment")]
public class ProjectAssignment
{
       [Column("id")]
       public int Id { get; set; }

       [Column("task_id")] // Correct column name
       public int TaskId { get; set; }

       [ForeignKey("TaskId")]
       public UserTask? ProjectTask { get; set; } // This maps to `task_id`

       [Column("project_id")]
       public int ProjectId { get; set; }

       [ForeignKey("ProjectId")]
       public Project? Project { get; set; }

       [Column("user_id")]
       public int UserId { get; set; }

       [ForeignKey("UserId")]
       public User? User { get; set; }

       [Column("assigned_at")]
       public DateTime AssignedTime { get; set; }
}
