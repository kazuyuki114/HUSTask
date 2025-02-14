using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Entities
{
    [Table("contacts")] // Map to the "users" table
    public class Contact
    {   
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("gender")]
        public string Gender { get; set; }
        [Column("occupation")]
        public string Occupation { get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("dob")]
        public DateOnly Dob { get; set; }
        [Column("notes")]
        public string Notes { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        
        public User User { get; set; } // Navigation property
    }
}