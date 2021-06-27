namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shopbranchadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shop", "BranchName", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shop", "BranchName");
        }
    }
}
