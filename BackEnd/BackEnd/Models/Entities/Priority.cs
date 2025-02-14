namespace BackEnd.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

[Table("priority")]
public class Priority
{   
    [Column("id")]
    public int Id { get; set; }
    [Column("title")]
    public string Title { get; set; } = string.Empty;
    [Column("description")]
    public string Description { get; set; } = string.Empty;
}