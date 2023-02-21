using Microsoft.EntityFrameworkCore;
using WebClient.Models;

namespace WebClient.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite(@"Data source = bakery.db");

        public DbSet<Product> Products { get; set; }
    }
}
