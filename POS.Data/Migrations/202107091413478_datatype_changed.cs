namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datatype_changed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InventoryHistory", "PurchaseDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InventoryHistory", "PurchaseDate", c => c.DateTime(nullable: false));
        }
    }
}
