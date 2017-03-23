namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class szamlaszam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Presses", "TaxNumber", c => c.String());
            AddColumn("dbo.Presses", "AccountNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Presses", "AccountNumber");
            DropColumn("dbo.Presses", "TaxNumber");
        }
    }
}
