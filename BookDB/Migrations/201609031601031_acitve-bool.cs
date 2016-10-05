namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class acitvebool : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Authors", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Authors", "Active", c => c.Int(nullable: false));
        }
    }
}
