namespace POS.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelchanges121 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sales", "BillId", "dbo.Bill");
            DropIndex("dbo.Sales", new[] { "BillId" });
            RenameColumn(table: "dbo.Sales", name: "BillId", newName: "BillNo");
            DropPrimaryKey("dbo.Bill");
            AlterColumn("dbo.Sales", "BillNo", c => c.Long(nullable: false));
            AlterColumn("dbo.Bill", "BillNo", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Bill", "BillNo");
            CreateIndex("dbo.Sales", "BillNo");
            AddForeignKey("dbo.Sales", "BillNo", "dbo.Bill", "BillNo", cascadeDelete: true);
            DropColumn("dbo.Bill", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bill", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Sales", "BillNo", "dbo.Bill");
            DropIndex("dbo.Sales", new[] { "BillNo" });
            DropPrimaryKey("dbo.Bill");
            AlterColumn("dbo.Bill", "BillNo", c => c.Int(nullable: false));
            AlterColumn("dbo.Sales", "BillNo", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Bill", "Id");
            RenameColumn(table: "dbo.Sales", name: "BillNo", newName: "BillId");
            CreateIndex("dbo.Sales", "BillId");
            AddForeignKey("dbo.Sales", "BillId", "dbo.Bill", "Id", cascadeDelete: true);
        }
    }
}
