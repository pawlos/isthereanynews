namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _001 : DbMigration
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
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        DisplayName = c.String(),
                        Email = c.String(),
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
            DropForeignKey("dbo.RssChannelSubscriptions", "RssChannelId", "dbo.RssChannels");
            DropIndex("dbo.SocialLogins", new[] { "UserId" });
            DropIndex("dbo.RssChannelSubscriptions", new[] { "UserId" });
            DropIndex("dbo.RssChannelSubscriptions", new[] { "RssChannelId" });
            DropTable("dbo.SocialLogins");
            DropTable("dbo.Users");
            DropTable("dbo.RssChannelSubscriptions");
            DropTable("dbo.RssChannels");
        }
    }
}
