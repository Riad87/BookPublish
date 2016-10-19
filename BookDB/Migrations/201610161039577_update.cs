namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pressures", "NotSold", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pressures", "NotSold");
        }
    }
}
