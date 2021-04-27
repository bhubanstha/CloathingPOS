namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventorymodelchange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventory", "RetailRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inventory", "RetailRate");
        }
    }
}
