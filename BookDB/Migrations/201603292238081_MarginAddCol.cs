namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarginAddCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stockist_margin", "Partner_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Stockist_margin", "Partner_ID");
            AddForeignKey("dbo.Stockist_margin", "Partner_ID", "dbo.Partners", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stockist_margin", "Partner_ID", "dbo.Partners");
            DropIndex("dbo.Stockist_margin", new[] { "Partner_ID" });
            DropColumn("dbo.Stockist_margin", "Partner_ID");
        }
    }
}
