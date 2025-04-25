using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TaskTrackerDemo.Models;

namespace TaskTrackerDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=TaskTracker.db");
            options.EnableSensitiveDataLogging(); // Temporary for debugging
        }
        
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectTask> ProjectTasks { get; set; } = null!;
        public DbSet<ProjectDiscussion> ProjectDiscussions { get; set; } = null!;
        public DbSet<MyTeam> MyTeams { get; set; } = null!;
        public DbSet<TaskTemplate> TaskTemplates { get; set; }
        public DbSet<TemplateTask> TemplateTasks { get; set; }
        public DbSet<ProjectViewer> ProjectViewers { get; set; }
        public ICollection<ProjectViewer> ViewedProjects { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ProjectTask>()
                .HasOne(p => p.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId);

            modelBuilder.Entity<ProjectDiscussion>()
                .HasOne(d => d.Project)
                .WithMany(p => p.Discussions)
                .HasForeignKey(d => d.ProjectId);

            modelBuilder.Entity<ProjectTeamMember>()
                .HasKey(pt => new { pt.ProjectId, pt.UserId });

            modelBuilder.Entity<ProjectTeamMember>()
                .HasOne(p => p.Project)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(p => p.ProjectId);

            modelBuilder.Entity<ProjectTeamMember>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.Assignee)     
                .WithMany(u => u.AssignedTasks)       
                .HasForeignKey(t => t.AssignedToId)   
                .OnDelete(DeleteBehavior.Restrict); 
            modelBuilder.Entity<ProjectTask>()
                .HasOne(t => t.AssignedUser)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MyTeam>()
                .HasOne(t => t.TeamMember)
                .WithMany()
                .HasForeignKey(t => t.TeamMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskTemplate>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}