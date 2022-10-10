using Microsoft.EntityFrameworkCore;

namespace FacebookAPI_CSharp.DataAccess.Models
{
    public class FacebookDBContext: DbContext
    {
        public FacebookDBContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
