using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Entities;
[Table("project_member")]
public class ProjectMember
{
    [Column("id")]
    public int Id { get; set; }
    [Column("user_id")]
    public int UserId { get; set; }
    [Column("project_id")]
    public int ProjectId { get; set; }
    [Column("role")]
    public string Role { get; set; }
    [Column("participated_at")]
    public DateTime ParticipatedAt { get; set; }
    public User? User { get; set; }
    public Project? Project { get; set; }
}