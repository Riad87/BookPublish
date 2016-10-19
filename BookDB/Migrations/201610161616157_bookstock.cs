namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookstock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "IN_Stock", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "IN_Stock");
        }
    }
}
