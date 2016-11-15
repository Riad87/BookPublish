namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class depot_pressure_Fk : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Depot_ID", "dbo.Depots");
            DropIndex("dbo.Books", new[] { "Depot_ID" });
            AddColumn("dbo.Pressures", "Depot_ID", c => c.Int());
            CreateIndex("dbo.Pressures", "Depot_ID");
            AddForeignKey("dbo.Pressures", "Depot_ID", "dbo.Depots", "ID");
            DropColumn("dbo.Books", "Depot_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "Depot_ID", c => c.Int());
            DropForeignKey("dbo.Pressures", "Depot_ID", "dbo.Depots");
            DropIndex("dbo.Pressures", new[] { "Depot_ID" });
            DropColumn("dbo.Pressures", "Depot_ID");
            CreateIndex("dbo.Books", "Depot_ID");
            AddForeignKey("dbo.Books", "Depot_ID", "dbo.Depots", "ID");
        }
    }
}
