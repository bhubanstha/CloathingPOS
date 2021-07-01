namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class branch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branch",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BranchName = c.String(nullable: false, maxLength: 50),
                        BranchAddress = c.String(nullable: false, maxLength: 300),
                        ShopId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shop", t => t.ShopId, cascadeDelete: true)
                .Index(t => t.ShopId);
            
            DropColumn("dbo.Shop", "BranchName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shop", "BranchName", c => c.String(nullable: false, maxLength: 50));
            DropForeignKey("dbo.Branch", "ShopId", "dbo.Shop");
            DropIndex("dbo.Branch", new[] { "ShopId" });
            DropTable("dbo.Branch");
        }
    }
}
