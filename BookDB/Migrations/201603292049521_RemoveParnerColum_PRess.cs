namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveParnerColum_PRess : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Presses", "Partner");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Presses", "Partner", c => c.Int(nullable: false));
        }
    }
}
