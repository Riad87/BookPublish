namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarginNameRemove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Stockist_margin", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Stockist_margin", "Name", c => c.String());
        }
    }
}
