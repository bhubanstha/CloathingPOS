using Microsoft.EntityFrameworkCore;
using POS.Core.Model;
using System.Configuration;

namespace POS.Core.Repo
{
    public class POSDataContext: DbContext
    {
        public POSDataContext(): base()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["CloathingPOS"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Add(new DataTypePropertyAttributeConvention());

        }

        public DbSet<Brand> Brands{ get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryHistory> InventoryHistories { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Shop> Shops { get; set; }
    }
}
