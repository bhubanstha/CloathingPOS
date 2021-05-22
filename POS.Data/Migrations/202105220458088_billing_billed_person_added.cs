namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billing_billed_person_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bill", "BillTo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bill", "BillTo");
        }
    }
}
