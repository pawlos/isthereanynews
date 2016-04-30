namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class followuser : DbMigration
    {
        public override void Up()
        {
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

            // deletion of UserSubscription will be handled in database, using triggers
            
            CreateTable(
                "dbo.UserSubscriptionEntryToReads",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        EventRssViewedId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventRssVieweds", t => t.EventRssViewedId, cascadeDelete: true)
                .Index(t => t.EventRssViewedId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSubscriptionEntryToReads", "EventRssViewedId", "dbo.EventRssVieweds");
            DropForeignKey("dbo.UserSubscriptions", "ObservedId", "dbo.Users");
            DropForeignKey("dbo.UserSubscriptions", "FollowerId", "dbo.Users");
            DropIndex("dbo.UserSubscriptionEntryToReads", new[] { "EventRssViewedId" });
            DropIndex("dbo.UserSubscriptions", new[] { "ObservedId" });
            DropIndex("dbo.UserSubscriptions", new[] { "FollowerId" });
            DropTable("dbo.UserSubscriptionEntryToReads");
            DropTable("dbo.UserSubscriptions");
        }
    }
}
