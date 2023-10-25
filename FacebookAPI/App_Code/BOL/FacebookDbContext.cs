using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.App_Code.BOL
{
    public class FacebookDbContext : DbContext
    {
        public FacebookDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }
        public DbSet<IssueTask> IssueTasks { get; set; }
        public DbSet<IssueTaskType> IssueTaskTypes { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(SecretConfig.ConnectionString, new MySqlServerVersion(SecretConfig.Version));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>(entity =>
            {
                entity.Property(e => e.ProfilePicture)
                .HasDefaultValue(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.IsAdmin)
                .HasDefaultValue(false);
            });
        }
    }
}
