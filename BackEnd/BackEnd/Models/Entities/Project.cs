using System.ComponentModel.DataAnnotations.Schema;


namespace BackEnd.Models.Entities;
[Table("project")]
public class Project
{   
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("description")]
    public string Description { get; set; }
    [Column("start_date")]
    public DateOnly StartDate { get; set; }
    [Column("end_date")]
    public DateOnly EndDate { get; set; }
    [Column("status")]
    public string Status { get; set; } = "active"; // Default value of 'active'

}