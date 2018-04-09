using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KTU_Mobile_Data
{
    public class Data : IdentityDbContext<ApplicationUser>
    {
        public Data(DbContextOptions<Data> options) : base(options)
        {
        }

        public DbSet<User> Members { get; set; }
        public DbSet<Main_page> Main_page { get; set; }
        public DbSet<log> Log { get; set; }
        public DbSet<Category> Categorys { get; set; }
    }
}