namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inventory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        FirstPurchaseDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        Size = c.String(),
                        Color = c.String(),
                        CategoryId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId_Id)
                .Index(t => t.CategoryId_Id);
            
            CreateTable(
                "dbo.InventoryHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PurchaseDate = c.DateTime(nullable: false),
                        Inventory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inventory", t => t.Inventory_Id)
                .Index(t => t.Inventory_Id);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SalesQuantity = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesDate = c.DateTime(nullable: false),
                        VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Name = c.String(),
                        Size = c.String(),
                        Color = c.String(),
                        CategoryId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId_Id)
                .Index(t => t.CategoryId_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        DisplayName = c.String(),
                        Password = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        PromptForPasswordReset = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        DeactivationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sales", "CategoryId_Id", "dbo.Category");
            DropForeignKey("dbo.InventoryHistory", "Inventory_Id", "dbo.Inventory");
            DropForeignKey("dbo.Inventory", "CategoryId_Id", "dbo.Category");
            DropIndex("dbo.Sales", new[] { "CategoryId_Id" });
            DropIndex("dbo.InventoryHistory", new[] { "Inventory_Id" });
            DropIndex("dbo.Inventory", new[] { "CategoryId_Id" });
            DropTable("dbo.User");
            DropTable("dbo.Sales");
            DropTable("dbo.InventoryHistory");
            DropTable("dbo.Inventory");
            DropTable("dbo.Category");
        }
    }
}
