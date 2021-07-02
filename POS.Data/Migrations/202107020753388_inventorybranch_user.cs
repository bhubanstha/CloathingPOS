namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventorybranch_user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventory", "BranchId", c => c.Long());
            AddColumn("dbo.Inventory", "UserId", c => c.Long());
            CreateIndex("dbo.Inventory", "BranchId");
            CreateIndex("dbo.Inventory", "UserId");
            AddForeignKey("dbo.Inventory", "BranchId", "dbo.Branch", "Id");
            AddForeignKey("dbo.Inventory", "UserId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventory", "UserId", "dbo.User");
            DropForeignKey("dbo.Inventory", "BranchId", "dbo.Branch");
            DropIndex("dbo.Inventory", new[] { "UserId" });
            DropIndex("dbo.Inventory", new[] { "BranchId" });
            DropColumn("dbo.Inventory", "UserId");
            DropColumn("dbo.Inventory", "BranchId");
        }
    }
}
