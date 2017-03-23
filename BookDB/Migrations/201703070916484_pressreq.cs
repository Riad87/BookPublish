namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pressreq : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Presses", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Presses", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Presses", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Presses", "Zip", c => c.String(nullable: false));
            AlterColumn("dbo.Presses", "Country", c => c.String(nullable: false));
            AlterColumn("dbo.Presses", "TaxNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Presses", "AccountNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Presses", "AccountNumber", c => c.String());
            AlterColumn("dbo.Presses", "TaxNumber", c => c.String());
            AlterColumn("dbo.Presses", "Country", c => c.String());
            AlterColumn("dbo.Presses", "Zip", c => c.String());
            AlterColumn("dbo.Presses", "Address", c => c.String());
            AlterColumn("dbo.Presses", "City", c => c.String());
            AlterColumn("dbo.Presses", "Name", c => c.String());
        }
    }
}
