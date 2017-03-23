namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coveractivebool : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Covers", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Covers", "Active", c => c.Int(nullable: false));
        }
    }
}
