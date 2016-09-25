namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tabledropped : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventRssClickeds", "RssEntryId", "dbo.RssEntries");
            DropForeignKey("dbo.EventRssClickeds", "UserId", "dbo.Users");
            DropIndex("dbo.EventRssClickeds", new[] { "UserId" });
            DropIndex("dbo.EventRssClickeds", new[] { "RssEntryId" });
            DropTable("dbo.EventRssClickeds");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.EventRssClickeds", "RssEntryId");
            CreateIndex("dbo.EventRssClickeds", "UserId");
            AddForeignKey("dbo.EventRssClickeds", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EventRssClickeds", "RssEntryId", "dbo.RssEntries", "Id", cascadeDelete: true);
        }
    }
}
