namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventoryhistory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InventoryHistory", "Inventory_Id", "dbo.Inventory");
            DropIndex("dbo.InventoryHistory", new[] { "Inventory_Id" });
            RenameColumn(table: "dbo.InventoryHistory", name: "Inventory_Id", newName: "InventoryId");
            AlterColumn("dbo.InventoryHistory", "InventoryId", c => c.Long(nullable: false));
            CreateIndex("dbo.InventoryHistory", "InventoryId");
            AddForeignKey("dbo.InventoryHistory", "InventoryId", "dbo.Inventory", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InventoryHistory", "InventoryId", "dbo.Inventory");
            DropIndex("dbo.InventoryHistory", new[] { "InventoryId" });
            AlterColumn("dbo.InventoryHistory", "InventoryId", c => c.Long());
            RenameColumn(table: "dbo.InventoryHistory", name: "InventoryId", newName: "Inventory_Id");
            CreateIndex("dbo.InventoryHistory", "Inventory_Id");
            AddForeignKey("dbo.InventoryHistory", "Inventory_Id", "dbo.Inventory", "Id");
        }
    }
}
