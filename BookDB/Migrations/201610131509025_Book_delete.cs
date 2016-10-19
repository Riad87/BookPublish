namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Book_delete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Deleted");
        }
    }
}
