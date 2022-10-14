using Microsoft.EntityFrameworkCore;

namespace FacebookAPI.DataAccess.Models
{
    public class FacebookDBContext: DbContext
    {
        public FacebookDBContext()
        {

        }

        public FacebookDBContext(DbContextOptions<FacebookDBContext> options): base(options)
        {

        }        

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Friend> Friends { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(SecretConfig.ConnectionString, new MySqlServerVersion(new Version(8, 0, 29)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>()
                .HasKey(f => new
                {
                    f.SendId,
                    f.ReceiverId
                });
            modelBuilder.Entity<Friend>()
                .HasOne(s => s.Sender)
                .WithMany(f => f.SenderFriends)
                .HasForeignKey(f => f.SendId);
            modelBuilder.Entity<Friend>()
                .HasOne(r => r.Receiver)
                .WithMany(f => f.ReceiverFriends)
                .HasForeignKey(f => f.ReceiverId);
        }
    }
}
