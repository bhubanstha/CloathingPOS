namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sales_branch_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bill", "BranchId", c => c.Long());
            AddColumn("dbo.Bill", "UserId", c => c.Long());
            CreateIndex("dbo.Bill", "BranchId");
            CreateIndex("dbo.Bill", "UserId");
            AddForeignKey("dbo.Bill", "BranchId", "dbo.Branch", "Id");
            AddForeignKey("dbo.Bill", "UserId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bill", "UserId", "dbo.User");
            DropForeignKey("dbo.Bill", "BranchId", "dbo.Branch");
            DropIndex("dbo.Bill", new[] { "UserId" });
            DropIndex("dbo.Bill", new[] { "BranchId" });
            DropColumn("dbo.Bill", "UserId");
            DropColumn("dbo.Bill", "BranchId");
        }
    }
}
