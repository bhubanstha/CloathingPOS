namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class code_and_barcode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventory", "Code", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Inventory", "Barcode", c => c.String(maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inventory", "Barcode");
            DropColumn("dbo.Inventory", "Code");
        }
    }
}
