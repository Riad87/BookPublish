namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class All_delete_Column : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partners", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Depots", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Depot_type", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Covers", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Themes", "Deleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Presses", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Presses", "Deleted");
            DropColumn("dbo.Themes", "Deleted");
            DropColumn("dbo.Covers", "Deleted");
            DropColumn("dbo.Depot_type", "Deleted");
            DropColumn("dbo.Depots", "Deleted");
            DropColumn("dbo.Partners", "Deleted");
        }
    }
}
