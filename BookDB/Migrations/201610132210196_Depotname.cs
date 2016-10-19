namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Depotname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Depots", "Depot_Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Depots", "Depot_Name");
        }
    }
}
