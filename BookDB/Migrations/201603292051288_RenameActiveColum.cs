namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameActiveColum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Presses", "Active", c => c.Int(nullable: false));
            DropColumn("dbo.Presses", "Acitve");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Presses", "Acitve", c => c.Int(nullable: false));
            DropColumn("dbo.Presses", "Active");
        }
    }
}
