using Microsoft.EntityFrameworkCore;

namespace GoEASy.DBConnection
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet ví dụ:
        // public DbSet<Customer> Customers { get; set; }
    }
}
