using POS.Data.Conventions;
using POS.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class POSDataContext: DbContext
    {
        public POSDataContext():
            base(ConfigurationManager.ConnectionStrings["CloathingPOS"].ConnectionString)
        {

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Add(new DataTypePropertyAttributeConvention());
            modelBuilder.Entity<User>()
                .HasOptional<Branch>(x => x.Branch)
                .WithOptionalDependent()
                .WillCascadeOnDelete(false);

            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Brand> Brands{ get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryHistory> InventoryHistories { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Branch> Branches { get; set; }
    }
}
