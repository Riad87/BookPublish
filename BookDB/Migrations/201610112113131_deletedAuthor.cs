namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedAuthor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "Delete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "Delete");
        }
    }
}
