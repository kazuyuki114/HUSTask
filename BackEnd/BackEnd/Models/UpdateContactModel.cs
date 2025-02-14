namespace BackEnd.Models;

public class UpdateContactModel
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    public string? Occupation { get; set; }
    public string? Phone { get; set; }
    public string? Notes { get; set; }
    public DateOnly? Dob { get; set; }
}