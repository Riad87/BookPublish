namespace BookDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountTypes",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Details = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Partners", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Partners",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Depots",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DepotName = c.String(),
                        City = c.String(),
                        Address = c.String(),
                        Zip = c.String(),
                        Type_ID = c.Int(nullable: false),
                        Partner_ID = c.Int(),
                        Stockist_margin_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Depot_type", t => t.Type_ID, cascadeDelete: true)
                .ForeignKey("dbo.Partners", t => t.Partner_ID)
                .ForeignKey("dbo.Stockist_margin", t => t.Stockist_margin_ID)
                .Index(t => t.Type_ID)
                .Index(t => t.Partner_ID)
                .Index(t => t.Stockist_margin_ID);
            
            CreateTable(
                "dbo.Depot_type",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AuthorName = c.String(),
                        Acitve = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ISBN = c.String(),
                        Name = c.String(),
                        ItemNumber = c.Int(nullable: false),
                        NetValue = c.Int(nullable: false),
                        Vat = c.Int(nullable: false),
                        GrossValue = c.Int(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                        Author_ID = c.Int(nullable: false),
                        Cover_ID = c.Int(),
                        Depot_ID = c.Int(),
                        Theme_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Authors", t => t.Author_ID, cascadeDelete: true)
                .ForeignKey("dbo.Covers", t => t.Cover_ID)
                .ForeignKey("dbo.Depots", t => t.Depot_ID)
                .ForeignKey("dbo.Themes", t => t.Theme_ID)
                .Index(t => t.Author_ID)
                .Index(t => t.Cover_ID)
                .Index(t => t.Depot_ID)
                .Index(t => t.Theme_ID);
            
            CreateTable(
                "dbo.Covers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CoverName = c.String(),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ThemeName = c.String(),
                        Active = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Presses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Partner = c.Int(nullable: false),
                        Name = c.String(),
                        City = c.String(),
                        Address = c.String(),
                        Zip = c.String(),
                        Country = c.String(),
                        Acitve = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Pressures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        AfterPressure = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Book_ID = c.Int(),
                        Press_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Books", t => t.Book_ID)
                .ForeignKey("dbo.Presses", t => t.Press_ID)
                .Index(t => t.Book_ID)
                .Index(t => t.Press_ID);
            
            CreateTable(
                "dbo.Stockist_margin",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Discount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Depots", "Stockist_margin_ID", "dbo.Stockist_margin");
            DropForeignKey("dbo.Pressures", "Press_ID", "dbo.Presses");
            DropForeignKey("dbo.Pressures", "Book_ID", "dbo.Books");
            DropForeignKey("dbo.Books", "Theme_ID", "dbo.Themes");
            DropForeignKey("dbo.Books", "Depot_ID", "dbo.Depots");
            DropForeignKey("dbo.Books", "Cover_ID", "dbo.Covers");
            DropForeignKey("dbo.Books", "Author_ID", "dbo.Authors");
            DropForeignKey("dbo.AccountTypes", "ID", "dbo.Partners");
            DropForeignKey("dbo.Depots", "Partner_ID", "dbo.Partners");
            DropForeignKey("dbo.Depots", "Type_ID", "dbo.Depot_type");
            DropIndex("dbo.Pressures", new[] { "Press_ID" });
            DropIndex("dbo.Pressures", new[] { "Book_ID" });
            DropIndex("dbo.Books", new[] { "Theme_ID" });
            DropIndex("dbo.Books", new[] { "Depot_ID" });
            DropIndex("dbo.Books", new[] { "Cover_ID" });
            DropIndex("dbo.Books", new[] { "Author_ID" });
            DropIndex("dbo.Depots", new[] { "Stockist_margin_ID" });
            DropIndex("dbo.Depots", new[] { "Partner_ID" });
            DropIndex("dbo.Depots", new[] { "Type_ID" });
            DropIndex("dbo.AccountTypes", new[] { "ID" });
            DropTable("dbo.Stockist_margin");
            DropTable("dbo.Pressures");
            DropTable("dbo.Presses");
            DropTable("dbo.Themes");
            DropTable("dbo.Covers");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
            DropTable("dbo.Depot_type");
            DropTable("dbo.Depots");
            DropTable("dbo.Partners");
            DropTable("dbo.AccountTypes");
        }
    }
}
