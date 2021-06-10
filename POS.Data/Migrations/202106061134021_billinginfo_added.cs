namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class billinginfo_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bill", "BillingAddress", c => c.String(maxLength: 200));
            AddColumn("dbo.Bill", "BillingPAN", c => c.String(maxLength: 20));
            AlterColumn("dbo.Bill", "BillTo", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Bill", "BillTo", c => c.String());
            DropColumn("dbo.Bill", "BillingPAN");
            DropColumn("dbo.Bill", "BillingAddress");
        }
    }
}
