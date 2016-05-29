namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rss_clicked_event : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventRssClickeds",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        UserId = c.Long(nullable: false),
                        RssEntryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RssEntries", t => t.RssEntryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RssEntryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventRssClickeds", "UserId", "dbo.Users");
            DropForeignKey("dbo.EventRssClickeds", "RssEntryId", "dbo.RssEntries");
            DropIndex("dbo.EventRssClickeds", new[] { "RssEntryId" });
            DropIndex("dbo.EventRssClickeds", new[] { "UserId" });
            DropTable("dbo.EventRssClickeds");
        }
    }
}
