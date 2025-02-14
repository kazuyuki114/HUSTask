using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Entities
{
    [Table("users")] // Map to the "users" table
    public class User
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        // Navigation property
        public Contact Contact { get; set; }
    }
}