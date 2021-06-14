namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pdfPassword_in_shop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shop", "PdfPassword", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shop", "PdfPassword");
        }
    }
}
