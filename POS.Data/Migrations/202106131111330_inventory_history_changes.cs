namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventory_history_changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventoryHistory", "PurchaseRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.InventoryHistory", "RetailRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.InventoryHistory", "Rate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InventoryHistory", "Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.InventoryHistory", "RetailRate");
            DropColumn("dbo.InventoryHistory", "PurchaseRate");
        }
    }
}
