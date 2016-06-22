namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rsschannelcreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventRssChannelCreateds",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        RssChannelId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RssChannels", t => t.RssChannelId, cascadeDelete: true)
                .Index(t => t.RssChannelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventRssChannelCreateds", "RssChannelId", "dbo.RssChannels");
            DropIndex("dbo.EventRssChannelCreateds", new[] { "RssChannelId" });
            DropTable("dbo.EventRssChannelCreateds");
        }
    }
}