using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.Entities
{
    [Table("person_task_list")]
    public class PersonTaskList
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("task_id")]
        public int TaskId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }
        [ForeignKey("TaskId")]
        public UserTask userTask { get; set; }
    }
}