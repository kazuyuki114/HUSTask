namespace BackEnd.Models;

public class UpdateUserModel
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public UpdateContactModel? Contact { get; set; }
}