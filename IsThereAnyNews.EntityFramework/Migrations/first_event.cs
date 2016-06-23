namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstEvent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventRssVieweds",
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
            DropForeignKey("dbo.EventRssVieweds", "UserId", "dbo.Users");
            DropForeignKey("dbo.EventRssVieweds", "RssEntryId", "dbo.RssEntries");
            DropIndex("dbo.EventRssVieweds", new[] { "RssEntryId" });
            DropIndex("dbo.EventRssVieweds", new[] { "UserId" });
            DropTable("dbo.EventRssVieweds");
        }
    }
}
