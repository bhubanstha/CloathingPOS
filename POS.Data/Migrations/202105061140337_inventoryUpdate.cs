namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventoryUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventory", "IsDeleted", c => c.Boolean(nullable: false,defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inventory", "IsDeleted");
        }
    }
}
