using Microsoft.EntityFrameworkCore;

namespace FacebookAPI_CSharp.DataAccess.Models
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(SecretConfig.ConnectionString, new MySqlServerVersion(new Version(8, 0, 29)));
            }
        }
    }
}
