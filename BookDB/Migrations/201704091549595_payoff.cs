namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payoff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PayOffs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ISBN = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PayOffs");
        }
    }
}
