namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inventorymodelchange1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inventory", "CategoryId_Id", "dbo.Category");
            DropForeignKey("dbo.Sales", "CategoryId_Id", "dbo.Category");
            DropIndex("dbo.Inventory", new[] { "CategoryId_Id" });
            DropIndex("dbo.Sales", new[] { "CategoryId_Id" });
            RenameColumn(table: "dbo.Inventory", name: "CategoryId_Id", newName: "CategoryId");
            RenameColumn(table: "dbo.Sales", name: "CategoryId_Id", newName: "CategoryId");
            AlterColumn("dbo.Inventory", "CategoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Sales", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Inventory", "CategoryId");
            CreateIndex("dbo.Sales", "CategoryId");
            AddForeignKey("dbo.Inventory", "CategoryId", "dbo.Category", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Sales", "CategoryId", "dbo.Category", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sales", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.Inventory", "CategoryId", "dbo.Category");
            DropIndex("dbo.Sales", new[] { "CategoryId" });
            DropIndex("dbo.Inventory", new[] { "CategoryId" });
            AlterColumn("dbo.Sales", "CategoryId", c => c.Int());
            AlterColumn("dbo.Inventory", "CategoryId", c => c.Int());
            RenameColumn(table: "dbo.Sales", name: "CategoryId", newName: "CategoryId_Id");
            RenameColumn(table: "dbo.Inventory", name: "CategoryId", newName: "CategoryId_Id");
            CreateIndex("dbo.Sales", "CategoryId_Id");
            CreateIndex("dbo.Inventory", "CategoryId_Id");
            AddForeignKey("dbo.Sales", "CategoryId_Id", "dbo.Category", "Id");
            AddForeignKey("dbo.Inventory", "CategoryId_Id", "dbo.Category", "Id");
        }
    }
}
