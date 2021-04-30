namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelchanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sales", "CategoryId", "dbo.Category");
            DropIndex("dbo.Sales", new[] { "CategoryId" });
            CreateTable(
                "dbo.Bill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BillNo = c.Int(nullable: false),
                        BillDate = c.DateTime(nullable: false),
                        VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Sales", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.Sales", "BillId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sales", "ProductId");
            CreateIndex("dbo.Sales", "BillId");
            AddForeignKey("dbo.Sales", "BillId", "dbo.Bill", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Sales", "ProductId", "dbo.Inventory", "Id", cascadeDelete: true);
            DropColumn("dbo.Sales", "SalesDate");
            DropColumn("dbo.Sales", "VAT");
            DropColumn("dbo.Sales", "Name");
            DropColumn("dbo.Sales", "Size");
            DropColumn("dbo.Sales", "Color");
            DropColumn("dbo.Sales", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sales", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.Sales", "Color", c => c.String());
            AddColumn("dbo.Sales", "Size", c => c.String());
            AddColumn("dbo.Sales", "Name", c => c.String());
            AddColumn("dbo.Sales", "VAT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Sales", "SalesDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Sales", "ProductId", "dbo.Inventory");
            DropForeignKey("dbo.Sales", "BillId", "dbo.Bill");
            DropIndex("dbo.Sales", new[] { "BillId" });
            DropIndex("dbo.Sales", new[] { "ProductId" });
            DropColumn("dbo.Sales", "BillId");
            DropColumn("dbo.Sales", "ProductId");
            DropTable("dbo.Bill");
            CreateIndex("dbo.Sales", "CategoryId");
            AddForeignKey("dbo.Sales", "CategoryId", "dbo.Category", "Id", cascadeDelete: true);
        }
    }
}
