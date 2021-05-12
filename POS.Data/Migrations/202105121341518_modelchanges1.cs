namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelchanges1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inventory", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.InventoryHistory", "Inventory_Id", "dbo.Inventory");
            DropForeignKey("dbo.Sales", "ProductId", "dbo.Inventory");
            DropForeignKey("dbo.Sales", "BillNo", "dbo.Bill");
            DropIndex("dbo.Inventory", new[] { "CategoryId" });
            DropIndex("dbo.InventoryHistory", new[] { "Inventory_Id" });
            DropIndex("dbo.Sales", new[] { "ProductId" });
            DropPrimaryKey("dbo.Category");
            DropPrimaryKey("dbo.Inventory");
            DropPrimaryKey("dbo.InventoryHistory");
            DropPrimaryKey("dbo.Sales");
            DropPrimaryKey("dbo.Bill");
            DropPrimaryKey("dbo.User");
            AddColumn("dbo.Bill", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Category", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Inventory", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Inventory", "CategoryId", c => c.Long(nullable: false));
            AlterColumn("dbo.InventoryHistory", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.InventoryHistory", "Inventory_Id", c => c.Long());
            AlterColumn("dbo.Sales", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Sales", "ProductId", c => c.Long(nullable: false));
            AlterColumn("dbo.User", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Category", "Id");
            AddPrimaryKey("dbo.Inventory", "Id");
            AddPrimaryKey("dbo.InventoryHistory", "Id");
            AddPrimaryKey("dbo.Sales", "Id");
            AddPrimaryKey("dbo.Bill", "Id");
            AddPrimaryKey("dbo.User", "Id");
            CreateIndex("dbo.Inventory", "CategoryId");
            CreateIndex("dbo.InventoryHistory", "Inventory_Id");
            CreateIndex("dbo.Sales", "ProductId");
            AddForeignKey("dbo.Inventory", "CategoryId", "dbo.Category", "Id", cascadeDelete: true);
            AddForeignKey("dbo.InventoryHistory", "Inventory_Id", "dbo.Inventory", "Id");
            AddForeignKey("dbo.Sales", "ProductId", "dbo.Inventory", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Sales", "BillNo", "dbo.Bill", "Id", cascadeDelete: true);
            DropColumn("dbo.Bill", "BillNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bill", "BillNo", c => c.Long(nullable: false, identity: true));
            DropForeignKey("dbo.Sales", "BillNo", "dbo.Bill");
            DropForeignKey("dbo.Sales", "ProductId", "dbo.Inventory");
            DropForeignKey("dbo.InventoryHistory", "Inventory_Id", "dbo.Inventory");
            DropForeignKey("dbo.Inventory", "CategoryId", "dbo.Category");
            DropIndex("dbo.Sales", new[] { "ProductId" });
            DropIndex("dbo.InventoryHistory", new[] { "Inventory_Id" });
            DropIndex("dbo.Inventory", new[] { "CategoryId" });
            DropPrimaryKey("dbo.User");
            DropPrimaryKey("dbo.Bill");
            DropPrimaryKey("dbo.Sales");
            DropPrimaryKey("dbo.InventoryHistory");
            DropPrimaryKey("dbo.Inventory");
            DropPrimaryKey("dbo.Category");
            AlterColumn("dbo.User", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Sales", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.Sales", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.InventoryHistory", "Inventory_Id", c => c.Int());
            AlterColumn("dbo.InventoryHistory", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Inventory", "CategoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Inventory", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Category", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Bill", "Id");
            AddPrimaryKey("dbo.User", "Id");
            AddPrimaryKey("dbo.Bill", "BillNo");
            AddPrimaryKey("dbo.Sales", "Id");
            AddPrimaryKey("dbo.InventoryHistory", "Id");
            AddPrimaryKey("dbo.Inventory", "Id");
            AddPrimaryKey("dbo.Category", "Id");
            CreateIndex("dbo.Sales", "ProductId");
            CreateIndex("dbo.InventoryHistory", "Inventory_Id");
            CreateIndex("dbo.Inventory", "CategoryId");
            AddForeignKey("dbo.Sales", "BillNo", "dbo.Bill", "BillNo", cascadeDelete: true);
            AddForeignKey("dbo.Sales", "ProductId", "dbo.Inventory", "Id", cascadeDelete: true);
            AddForeignKey("dbo.InventoryHistory", "Inventory_Id", "dbo.Inventory", "Id");
            AddForeignKey("dbo.Inventory", "CategoryId", "dbo.Category", "Id", cascadeDelete: true);
        }
    }
}
