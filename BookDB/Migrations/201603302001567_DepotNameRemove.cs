namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepotNameRemove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Depots", "DepotName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Depots", "DepotName", c => c.String());
        }
    }
}
