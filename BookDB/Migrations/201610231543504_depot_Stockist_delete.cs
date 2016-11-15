namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class depot_Stockist_delete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Depots", "Stockist_margin_ID", "dbo.Stockist_margin");
            DropIndex("dbo.Depots", new[] { "Stockist_margin_ID" });
            DropColumn("dbo.Depots", "Stockist_margin_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Depots", "Stockist_margin_ID", c => c.Int());
            CreateIndex("dbo.Depots", "Stockist_margin_ID");
            AddForeignKey("dbo.Depots", "Stockist_margin_ID", "dbo.Stockist_margin", "ID");
        }
    }
}
