namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthorsColumRename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "Active", c => c.Int(nullable: false));
            DropColumn("dbo.Authors", "Acitve");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Authors", "Acitve", c => c.Int(nullable: false));
            DropColumn("dbo.Authors", "Active");
        }
    }
}
