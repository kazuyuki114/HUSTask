using BackEnd.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data;

public class WebDbContext : DbContext
{
    public WebDbContext(DbContextOptions options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserTask>()
            .HasKey(t => t.Id); // Explicitly set the primary key
        modelBuilder.Entity<Task>().HasNoKey(); // Use only if `Task` is intended to be keyless
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<UserTask> Tasks { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Priority> Priorities { get; set; }
    public DbSet<PersonTaskList> PersonTaskLists { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectAssignment> ProjectAssignments { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Log> Logs { get; set; }
    
}