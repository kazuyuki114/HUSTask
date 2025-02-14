using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Entities;
[Table("logs")]
public class Log
{
    [Column("id")]
    public int Id {get; set;}
    [Column("user_id")]
    public int UserId {get; set;}
    [Column("description")]
    public string? Description {get; set;}
    [Column("timestamp")]
    public DateTime Timestamp {get; set;}
}