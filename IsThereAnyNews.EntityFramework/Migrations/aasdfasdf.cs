namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Aasdfasdf : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RssChannels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        Title = c.String(),
                        Url = c.String(),
                        RssLastUpdatedTime = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RssChannelSubscriptions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        RssChannelId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RssChannels", t => t.RssChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RssChannelId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RssEntriesToRead",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        RssChannelSubscriptionId = c.Long(nullable: false),
                        RssEntryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RssChannelSubscriptions", t => t.RssChannelSubscriptionId, cascadeDelete: true)
                .ForeignKey("dbo.RssEntries", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.RssChannelSubscriptionId);
            
            CreateTable(
                "dbo.RssEntries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        RssChannelId = c.Long(nullable: false),
                        PublicationDate = c.DateTime(nullable: false),
                        Title = c.String(),
                        PreviewText = c.String(),
                        RssId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        DisplayName = c.String(),
                        Email = c.String(),
                        LastReadTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocialLogins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SocialId = c.String(),
                        Provider = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SocialLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.RssChannelSubscriptions", "UserId", "dbo.Users");
            DropForeignKey("dbo.RssEntriesToRead", "Id", "dbo.RssEntries");
            DropForeignKey("dbo.RssEntriesToRead", "RssChannelSubscriptionId", "dbo.RssChannelSubscriptions");
            DropForeignKey("dbo.RssChannelSubscriptions", "RssChannelId", "dbo.RssChannels");
            DropIndex("dbo.SocialLogins", new[] { "UserId" });
            DropIndex("dbo.RssEntriesToRead", new[] { "RssChannelSubscriptionId" });
            DropIndex("dbo.RssEntriesToRead", new[] { "Id" });
            DropIndex("dbo.RssChannelSubscriptions", new[] { "UserId" });
            DropIndex("dbo.RssChannelSubscriptions", new[] { "RssChannelId" });
            DropTable("dbo.SocialLogins");
            DropTable("dbo.Users");
            DropTable("dbo.RssEntries");
            DropTable("dbo.RssEntriesToRead");
            DropTable("dbo.RssChannelSubscriptions");
            DropTable("dbo.RssChannels");
        }
    }
}
