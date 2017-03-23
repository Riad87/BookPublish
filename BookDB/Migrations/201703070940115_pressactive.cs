namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pressactive : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Presses", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Presses", "Active", c => c.Int(nullable: false));
        }
    }
}
