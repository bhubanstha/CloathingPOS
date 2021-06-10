namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addshop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shop",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Address = c.String(nullable: false, maxLength: 200),
                        PANNumber = c.String(maxLength: 20),
                        LogoPath = c.String(maxLength: 200),
                        CalculateVATOnSales = c.Boolean(nullable: false),
                        PrintInvoice = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Shop");
        }
    }
}
