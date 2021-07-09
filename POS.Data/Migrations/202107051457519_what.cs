namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class what : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Shop", "Address");
            DropColumn("dbo.Shop", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shop", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Shop", "Address", c => c.String());
        }
    }
}
