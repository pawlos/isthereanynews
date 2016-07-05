namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m0001_initial : DbMigration
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
                "dbo.RssEntries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        RssChannelId = c.Long(nullable: false),
                        Url = c.String(),
                        PublicationDate = c.DateTime(nullable: false),
                        Title = c.String(),
                        PreviewText = c.String(),
                        RssId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RssChannels", t => t.RssChannelId, cascadeDelete: true)
                .Index(t => t.RssChannelId);
            
            CreateTable(
                "dbo.RssEntriesToRead",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        IsViewed = c.Boolean(nullable: false),
                        RssChannelSubscriptionId = c.Long(nullable: false),
                        RssEntryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RssChannelSubscriptions", t => t.RssChannelSubscriptionId, cascadeDelete: true)
                .ForeignKey("dbo.RssEntries", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.RssChannelSubscriptionId);
            
            CreateTable(
                "dbo.RssChannelSubscriptions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        Title = c.String(),
                        UserId = c.Long(nullable: false),
                        RssChannelId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RssChannels", t => t.RssChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RssChannelId);
            
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
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        UserId = c.Long(nullable: false),
                        RoleType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
            
            CreateTable(
                "dbo.FeatureRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                        RssEntryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RssEntries", t => t.RssEntryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RssEntryId);
            
            CreateTable(
                "dbo.EventRssChannelUpdates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        RssChannelId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RssChannels", t => t.RssChannelId, cascadeDelete: true)
                .Index(t => t.RssChannelId);
            
            CreateTable(
                "dbo.UserSubscriptions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        FollowerId = c.Long(nullable: false),
                        ObservedId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.FollowerId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.ObservedId, cascadeDelete: false)
                .Index(t => t.FollowerId)
                .Index(t => t.ObservedId);
            
            CreateTable(
                "dbo.UserSubscriptionEntryToReads",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        EventRssViewedId = c.Long(nullable: false),
                        UserSubscriptionId = c.Long(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventRssVieweds", t => t.EventRssViewedId, cascadeDelete: true)
                .ForeignKey("dbo.UserSubscriptions", t => t.UserSubscriptionId, cascadeDelete: true)
                .Index(t => t.EventRssViewedId)
                .Index(t => t.UserSubscriptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSubscriptions", "ObservedId", "dbo.Users");
            DropForeignKey("dbo.UserSubscriptions", "FollowerId", "dbo.Users");
            DropForeignKey("dbo.UserSubscriptionEntryToReads", "UserSubscriptionId", "dbo.UserSubscriptions");
            DropForeignKey("dbo.UserSubscriptionEntryToReads", "EventRssViewedId", "dbo.EventRssVieweds");
            DropForeignKey("dbo.EventRssChannelUpdates", "RssChannelId", "dbo.RssChannels");
            DropForeignKey("dbo.FeatureRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.FeatureRequests", "RssEntryId", "dbo.RssEntries");
            DropForeignKey("dbo.EventRssClickeds", "UserId", "dbo.Users");
            DropForeignKey("dbo.EventRssClickeds", "RssEntryId", "dbo.RssEntries");
            DropForeignKey("dbo.EventRssChannelCreateds", "RssChannelId", "dbo.RssChannels");
            DropForeignKey("dbo.RssEntries", "RssChannelId", "dbo.RssChannels");
            DropForeignKey("dbo.RssEntriesToRead", "Id", "dbo.RssEntries");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.SocialLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.RssChannelSubscriptions", "UserId", "dbo.Users");
            DropForeignKey("dbo.EventRssVieweds", "UserId", "dbo.Users");
            DropForeignKey("dbo.EventRssVieweds", "RssEntryId", "dbo.RssEntries");
            DropForeignKey("dbo.RssEntriesToRead", "RssChannelSubscriptionId", "dbo.RssChannelSubscriptions");
            DropForeignKey("dbo.RssChannelSubscriptions", "RssChannelId", "dbo.RssChannels");
            DropIndex("dbo.UserSubscriptionEntryToReads", new[] { "UserSubscriptionId" });
            DropIndex("dbo.UserSubscriptionEntryToReads", new[] { "EventRssViewedId" });
            DropIndex("dbo.UserSubscriptions", new[] { "ObservedId" });
            DropIndex("dbo.UserSubscriptions", new[] { "FollowerId" });
            DropIndex("dbo.EventRssChannelUpdates", new[] { "RssChannelId" });
            DropIndex("dbo.FeatureRequests", new[] { "RssEntryId" });
            DropIndex("dbo.FeatureRequests", new[] { "UserId" });
            DropIndex("dbo.EventRssClickeds", new[] { "RssEntryId" });
            DropIndex("dbo.EventRssClickeds", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.SocialLogins", new[] { "UserId" });
            DropIndex("dbo.EventRssVieweds", new[] { "RssEntryId" });
            DropIndex("dbo.EventRssVieweds", new[] { "UserId" });
            DropIndex("dbo.RssChannelSubscriptions", new[] { "RssChannelId" });
            DropIndex("dbo.RssChannelSubscriptions", new[] { "UserId" });
            DropIndex("dbo.RssEntriesToRead", new[] { "RssChannelSubscriptionId" });
            DropIndex("dbo.RssEntriesToRead", new[] { "Id" });
            DropIndex("dbo.RssEntries", new[] { "RssChannelId" });
            DropIndex("dbo.EventRssChannelCreateds", new[] { "RssChannelId" });
            DropTable("dbo.UserSubscriptionEntryToReads");
            DropTable("dbo.UserSubscriptions");
            DropTable("dbo.EventRssChannelUpdates");
            DropTable("dbo.FeatureRequests");
            DropTable("dbo.EventRssClickeds");
            DropTable("dbo.UserRoles");
            DropTable("dbo.SocialLogins");
            DropTable("dbo.EventRssVieweds");
            DropTable("dbo.Users");
            DropTable("dbo.RssChannelSubscriptions");
            DropTable("dbo.RssEntriesToRead");
            DropTable("dbo.RssEntries");
            DropTable("dbo.RssChannels");
            DropTable("dbo.EventRssChannelCreateds");
        }
    }
}
