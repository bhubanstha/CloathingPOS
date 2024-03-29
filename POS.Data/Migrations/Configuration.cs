﻿namespace POS.Data.Migrations
{
    using POS.Model;
    using POS.Utilities.Encryption;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<POS.Data.POSDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(POS.Data.POSDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            if (!context.User.Any())
            {
                BouncyCastleEncryption encryption = new BouncyCastleEncryption();
                context.User.AddOrUpdate(
                        u => u.Id,
                        new User
                        {
                            Id = 1,
                            UserName = "sysadmin",
                            DisplayName = "System Admin",
                            Password = encryption.EncryptAsAsync("Nepal@123").Result,
                            IsActive = true,
                            IsAdmin = true,
                            PromptForPasswordReset = false,
                            CreatedDate = DateTime.Now,
                            DeactivationDate = null,
                            BranchId = 1,
                            CanAccessAllBranch = true,
                        },
                        new User
                        {
                            Id = 2,
                            UserName = "admin",
                            DisplayName = "Default Administrator",
                            Password = encryption.EncryptAsAsync("123456").Result,
                            IsAdmin = true,
                            IsActive = true,
                            PromptForPasswordReset = true,
                            CreatedDate = DateTime.Now,
                            DeactivationDate = null,
                            BranchId = 1,
                            CanAccessAllBranch = true,
                        }
                    );
            }

            if (!context.Categories.Any())
            {
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
                        Name = "Children"
                    }

                );
            }
            if (!context.Brands.Any())
            {
                context.Brands.AddOrUpdate(
                    b => b.Id,
                    new Brand
                    {
                        Id = 1,
                        Name = "Adidas"
                    },
                    new Brand
                    {
                        Id = 2,
                        Name = "Armani"
                    },
                    new Brand
                    {
                        Id = 3,
                        Name = "Ralpha Lauren"
                    },
                    new Brand
                    {
                        Id = 4,
                        Name = "Chanel"
                    },
                    new Brand
                    {
                        Id = 5,
                        Name = "Prada"
                    },
                    new Brand
                    {
                        Id = 6,
                        Name = "Hermes"
                    },
                    new Brand
                    {
                        Id = 7,
                        Name = "Gucci"
                    },
                    new Brand
                    {
                        Id = 8,
                        Name = "Louis Vuitton"
                    },
                    new Brand
                    {
                        Id = 9,
                        Name = "Nike"
                    }
                 );
            }
            if (!context.Shops.Any())
            {
                context.Shops.AddOrUpdate(
                        s => s.Id,
                        new Shop
                        {
                            Id = 1,
                            Name = "Give Your Shop Name",
                            LogoPath = "companyLogo1.png",
                            PANNumber = "123456",
                            CalculateVATOnSales = true,
                            PrintInvoice = false,
                            PdfPassword = "123"
                        }
                    );
            }

            if(!context.Branches.Any())
            {
                context.Branches.AddOrUpdate(
                    b => b.Id,
                    new Branch
                    {
                        Id = 1,
                        ShopId = 1,
                        BranchName = "Main Branch",
                        BranchAddress = "Your Main Branch Address"
                    }
                    );
            }
        }
    }
}
