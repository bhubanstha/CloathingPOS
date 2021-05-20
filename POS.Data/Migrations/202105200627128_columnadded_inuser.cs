namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class columnadded_inuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "LastPasswordChangeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "LastPasswordChangeDate");
        }
    }
}
