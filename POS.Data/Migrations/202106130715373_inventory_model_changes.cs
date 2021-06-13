namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventory_model_changes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inventory", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Inventory", "Size", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.Inventory", "Color", c => c.String(nullable: false, maxLength: 9));
            AlterColumn("dbo.Inventory", "ColorName", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inventory", "ColorName", c => c.String(maxLength: 30));
            AlterColumn("dbo.Inventory", "Color", c => c.String(maxLength: 10));
            AlterColumn("dbo.Inventory", "Size", c => c.String(maxLength: 5));
            AlterColumn("dbo.Inventory", "Name", c => c.String());
        }
    }
}
