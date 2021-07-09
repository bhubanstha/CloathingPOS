namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class columnnamechanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sales", "SalesRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Sales", "Rate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sales", "Rate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Sales", "SalesRate");
        }
    }
}
