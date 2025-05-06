

using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManegmentProject.Enums;

namespace TaskManegmentProject.DBcontcion
{
    public class TMContextDB:IdentityDbContext<ApplicationUser,IdentityRole,string>
    {

        public DbSet<WorkSpace> WorkSpaces { get; set; }
        public DbSet<MemberWorkSpace> MemberWorkSpace { get; set; }
        public DbSet<MessageChat> MessageChat { get; set; }
        public DbSet<MessageMentions> MessageMentions { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<MyTask> MyTask { get; set; }
        public DbSet<TaskAssignment> TaskAssignment { get; set; }



        public TMContextDB(DbContextOptions option):base(option)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MemberWorkSpace>()
                .Property(p => p.MemberRole)
                .HasConversion<string>()
                .HasDefaultValue(MemberRole.Editor);
            
            modelBuilder.Entity<Notification>()
                .Property(p =>p.Action)
                .HasConversion<string>();

            modelBuilder.Entity<MyTask>()
                  .HasMany(t => t.Notifications) 
                  .WithOne(n => n.Task)         
                  .HasForeignKey(n => n.TaskId)  
                  .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
