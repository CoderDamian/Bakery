using Microsoft.EntityFrameworkCore;
using DomainModel;
using Persistence.Mappings;
using Oracle.ManagedDataAccess.Client;

namespace Persistence
{
    public class BakeryDbContext : DbContext
    {
        public BakeryDbContext(DbContextOptions<BakeryDbContext> options)
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (OracleConfiguration.TnsAdmin is null)
            {
                OracleConfiguration.TnsAdmin = @"C:\Users\Fmla\Documents\OracleWallet\MyERP\";
                OracleConfiguration.WalletLocation = OracleConfiguration.TnsAdmin;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfiguration(new ProductMap());
    }
}