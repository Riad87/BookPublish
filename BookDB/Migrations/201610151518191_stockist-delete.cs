namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stockistdelete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stockist_margin", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Stockist_margin", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stockist_margin", "Deleted");
            DropColumn("dbo.Stockist_margin", "Active");
        }
    }
}
