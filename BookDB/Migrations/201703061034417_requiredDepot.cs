namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredDepot : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Depots", "Depot_Name", c => c.String(nullable: false));
            AlterColumn("dbo.Depot_type", "Type", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Depot_type", "Type", c => c.String());
            AlterColumn("dbo.Depots", "Depot_Name", c => c.String());
        }
    }
}
