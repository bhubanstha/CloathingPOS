namespace POS.Data.Migrations
{
    using Org.BouncyCastle.Crypto.Engines;
    using POS.Model;
    using POS.Utilities.Encryption;
    using System;
    using System.Data.Entity.Migrations;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<POS.Data.POSDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(POS.Data.POSDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            BouncyCastleEncryption encryption = new BouncyCastleEncryption(Encoding.UTF8, new AesEngine());
            
            context.User.AddOrUpdate(
                    u=> u.Id,
                    new User { 
                        Id=1,
                        UserName = "sysadmin", 
                        DisplayName = "System Admin", 
                        Password = encryption.EncryptAsAsync("123").Result, 
                        IsActive = true,
                        IsAdmin = true,
                        PromptForPasswordReset = false,
                        CreatedDate = DateTime.Now,  
                        DeactivationDate = null 
                    },
                    new User
                    {
                        Id=2,
                        UserName = "hom",
                        DisplayName = "Hom Bdr. Tamang",
                        Password = encryption.EncryptAsAsync("123456").Result,
                        IsAdmin = true,
                        IsActive = true,
                        PromptForPasswordReset = true,
                        CreatedDate = DateTime.Now,
                        DeactivationDate = null
                    }
                );

            context.Categories.AddOrUpdate(
                    c => c.Id,
                    new Category
                    {
                        Id = 1,
                        Name = "Men"
                    },
                    new Category
                    {
                        Id = 2,
                        Name = "Woman"
                    },
                    new Category
                    {
                        Id = 3,
                        Name = "Men Upper"
                    },
                    new Category
                    {
                        Id = 4,
                        Name = "Woman Upper"
                    },
                    new Category
                    {
                        Id = 5,
                        Name = "Children"
                    }

                ); 
        }
    }
}
