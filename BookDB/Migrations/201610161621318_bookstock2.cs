namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookstock2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "IN_Stock");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "IN_Stock", c => c.Int(nullable: false));
        }
    }
}
