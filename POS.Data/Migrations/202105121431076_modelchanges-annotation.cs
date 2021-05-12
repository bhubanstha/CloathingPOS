namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelchangesannotation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "UserName", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "UserName", c => c.String());
        }
    }
}
