using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Entities
{   
    [Table("tag")]
    public class Tag
    {   
        [Column("id")]
        public int Id { get; set; } // Represents the primary key (id)
        [Column("name")]
        public string TagName { get; set; } // Represents the tag name (tag_name)
    }
}
