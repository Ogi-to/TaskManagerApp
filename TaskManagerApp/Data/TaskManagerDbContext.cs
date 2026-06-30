using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data.Models;

namespace TaskManagerApp.Data
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options)
        {
        }
        public TaskManagerDbContext()
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>()
       .Property(s => s.Type)
       .HasConversion<string>();

            modelBuilder.Entity<State>().HasData(
            new State { Id = 1, Type = StateType.NotStarted },
            new State { Id = 2, Type = StateType.InProgress },
            new State { Id = 3, Type = StateType.Completed },
            new State { Id = 4, Type = StateType.Overdue }
        );

            //For USERS RELATIONS
            modelBuilder.Entity<UsersRelations>()
                .HasKey(ur => new { ur.InitiatorId, ur.RelatedUserId });

            modelBuilder.Entity<UsersRelations>()
                .HasOne(ur => ur.Initiator)
                .WithMany(u => u.SentRelations)
                .HasForeignKey(ur => ur.InitiatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsersRelations>()
                .HasOne(ur => ur.RelatedUser)
                .WithMany(u => u.ReceivedRelations)
                .HasForeignKey(ur => ur.RelatedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            //For TASKS USERS
            modelBuilder.Entity<UsersTasks>()
                .HasKey(tu => new { tu.TaskId, tu.UserId });

            modelBuilder.Entity<UsersTasks>()
                .HasOne(tu => tu.Task)
                .WithMany(t => t.TasksUsers)
                .HasForeignKey(tu => tu.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsersTasks>()
                .HasOne(tu => tu.User)
                .WithMany(u => u.UsersTasks)
                .HasForeignKey(tu => tu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //For CHALLENGES USERS
            modelBuilder.Entity<UsersChallenges>()
                .HasKey(cu => new { cu.ChallengeId, cu.UserId });
            
            modelBuilder.Entity<UsersChallenges>()
                .HasOne(cu => cu.Challenge)
                .WithMany(c => c.UsersChallenges)
                .HasForeignKey(cu => cu.ChallengeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsersChallenges>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.UsersChallenges)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //For TASKS CATEGORIES
            modelBuilder.Entity<TasksCategories>()
                .HasKey(tc => new { tc.TaskId, tc.CategoryId });
                
            modelBuilder.Entity<TasksCategories>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.TasksCategories)
                .HasForeignKey(tc => tc.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TasksCategories>()
                .HasOne(tc => tc.Category)
                .WithMany(c => c.TasksCategories)
                .HasForeignKey(tc => tc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //For USER STATS
            modelBuilder.Entity<User>()
            .HasOne(u => u.Stats)
            .WithOne(s => s.User)
            .HasForeignKey<UserStats>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        }



        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<UsersTasks> UsersTasks { get; set; }
        public DbSet<TasksCategories> TasksCategories { get; set; }
        public DbSet<UsersChallenges> UsersChallenges { get; set; }
        public DbSet<UsersRelations> UsersRelations { get; set; }
        public DbSet<UserStats> UserStats { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<EmailCode> EmailCodes { get; set; }
    }
}

