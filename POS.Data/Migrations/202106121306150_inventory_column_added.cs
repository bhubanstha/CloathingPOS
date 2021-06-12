namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventory_column_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventory", "ColorName", c => c.String(maxLength: 30));
            AlterColumn("dbo.Inventory", "Size", c => c.String(maxLength: 5));
            AlterColumn("dbo.Inventory", "Color", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inventory", "Color", c => c.String());
            AlterColumn("dbo.Inventory", "Size", c => c.String());
            DropColumn("dbo.Inventory", "ColorName");
        }
    }
}
