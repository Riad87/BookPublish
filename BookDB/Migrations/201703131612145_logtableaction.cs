namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logtableaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Action", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "Action");
        }
    }
}
