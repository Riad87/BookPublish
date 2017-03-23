namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class themebool : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Themes", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Themes", "Active", c => c.Int(nullable: false));
        }
    }
}
