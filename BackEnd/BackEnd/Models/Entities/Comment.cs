using System.ComponentModel.DataAnnotations.Schema;


namespace BackEnd.Models.Entities;
[Table("comment")]
public class Comment
{   [Column("id")]
    public int Id {get; set;}
    [Column("content")]
    public string Content {get; set;}
    [Column("updated_at")]
    public DateTime Updated_at{get; set;}
    [Column("user_id")]
    public int UserId {get; set;}
    [Column("task_id")]
    public int TaskId {get; set;}

    public User? commentedUser {get; set;}
}