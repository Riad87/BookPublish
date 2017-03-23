namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class partnersreq : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Partners", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Partners", "Name", c => c.String());
        }
    }
}
