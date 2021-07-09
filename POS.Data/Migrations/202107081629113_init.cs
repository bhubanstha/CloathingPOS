namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branch",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BranchName = c.String(nullable: false, maxLength: 50),
                        BranchAddress = c.String(nullable: false, maxLength: 300),
                        ShopId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shop", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId);
            
            CreateTable(
                "dbo.Shop",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        PANNumber = c.String(maxLength: 20),
                        LogoPath = c.String(maxLength: 200),
                        CalculateVATOnSales = c.Boolean(nullable: false),
                        PrintInvoice = c.Boolean(nullable: false),
                        PdfPassword = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Brand",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inventory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PurchaseRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RetailRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        FirstPurchaseDate = c.DateTime(nullable: false, storeType: "date"),
                        IsDeleted = c.Boolean(nullable: false),
                        BranchId = c.Long(),
                        UserId = c.Long(),
                        Code = c.String(nullable: false, maxLength: 50),
                        BarCode = c.String(maxLength: 40, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Size = c.String(nullable: false, maxLength: 5),
                        Color = c.String(nullable: false, maxLength: 9),
                        ColorName = c.String(nullable: false, maxLength: 20),
                        CategoryId = c.Long(nullable: false),
                        BrandId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.Brand", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.BranchId)
                .Index(t => t.UserId)
                .Index(t => t.CategoryId)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.InventoryHistory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        PurchaseRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RetailRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseDate = c.DateTime(nullable: false),
                        InventoryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inventory", t => t.InventoryId, cascadeDelete: true)
                .Index(t => t.InventoryId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 15),
                        DisplayName = c.String(nullable: false, maxLength: 40),
                        Password = c.String(nullable: false, maxLength: 60),
                        IsAdmin = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        PromptForPasswordReset = c.Boolean(nullable: false),
                        ProfileImage = c.String(maxLength: 30),
                        CreatedDate = c.DateTime(nullable: false),
                        DeactivationDate = c.DateTime(),
                        LastPasswordChangeDate = c.DateTime(),
                        BranchId = c.Long(),
                        CanAccessAllBranch = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SalesQuantity = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductId = c.Long(nullable: false),
                        BillNo = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bill", t => t.BillNo, cascadeDelete: true)
                .ForeignKey("dbo.Inventory", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.BillNo);
            
            CreateTable(
                "dbo.Bill",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BillDate = c.DateTime(nullable: false),
                        VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillTo = c.String(maxLength: 100),
                        BillingAddress = c.String(maxLength: 200),
                        BillingPAN = c.String(maxLength: 20),
                        BranchId = c.Long(),
                        UserId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branch", t => t.BranchId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.BranchId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sales", "ProductId", "dbo.Inventory");
            DropForeignKey("dbo.Sales", "BillNo", "dbo.Bill");
            DropForeignKey("dbo.Bill", "UserId", "dbo.User");
            DropForeignKey("dbo.Bill", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.Inventory", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.InventoryHistory", "InventoryId", "dbo.Inventory");
            DropForeignKey("dbo.Inventory", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.Inventory", "BrandId", "dbo.Brand");
            DropForeignKey("dbo.Inventory", "BranchId", "dbo.Branch");
            DropForeignKey("dbo.Branch", "ShopId", "dbo.Shop");
            DropIndex("dbo.Bill", new[] { "UserId" });
            DropIndex("dbo.Bill", new[] { "BranchId" });
            DropIndex("dbo.Sales", new[] { "BillNo" });
            DropIndex("dbo.Sales", new[] { "ProductId" });
            DropIndex("dbo.User", new[] { "BranchId" });
            DropIndex("dbo.InventoryHistory", new[] { "InventoryId" });
            DropIndex("dbo.Inventory", new[] { "BrandId" });
            DropIndex("dbo.Inventory", new[] { "CategoryId" });
            DropIndex("dbo.Inventory", new[] { "UserId" });
            DropIndex("dbo.Inventory", new[] { "BranchId" });
            DropIndex("dbo.Branch", new[] { "ShopId" });
            DropTable("dbo.Bill");
            DropTable("dbo.Sales");
            DropTable("dbo.User");
            DropTable("dbo.InventoryHistory");
            DropTable("dbo.Inventory");
            DropTable("dbo.Category");
            DropTable("dbo.Brand");
            DropTable("dbo.Shop");
            DropTable("dbo.Branch");
        }
    }
}
