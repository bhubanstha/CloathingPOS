namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shop", "Address", c => c.String());
            AddColumn("dbo.Shop", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shop", "Discriminator");
            DropColumn("dbo.Shop", "Address");
        }
    }
}
