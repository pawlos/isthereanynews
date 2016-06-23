namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Featurerequest : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeatureRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.FeatureRequests", "RssEntryId", "dbo.RssEntries");
            DropIndex("dbo.FeatureRequests", new[] { "RssEntryId" });
            DropIndex("dbo.FeatureRequests", new[] { "UserId" });
            DropTable("dbo.FeatureRequests");
        }
    }
}
