namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userimage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "ProfileImage", c => c.String(maxLength: 30));
            AlterColumn("dbo.User", "DisplayName", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.User", "Password", c => c.String(nullable: false, maxLength: 60));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "Password", c => c.String());
            AlterColumn("dbo.User", "DisplayName", c => c.String());
            DropColumn("dbo.User", "ProfileImage");
        }
    }
}
