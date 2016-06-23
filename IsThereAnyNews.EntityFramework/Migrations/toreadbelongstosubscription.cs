namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Toreadbelongstosubscription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSubscriptionEntryToReads", "UserSubscriptionId", c => c.Long(nullable: false));
            CreateIndex("dbo.UserSubscriptionEntryToReads", "UserSubscriptionId");
            AddForeignKey("dbo.UserSubscriptionEntryToReads", "UserSubscriptionId", "dbo.UserSubscriptions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSubscriptionEntryToReads", "UserSubscriptionId", "dbo.UserSubscriptions");
            DropIndex("dbo.UserSubscriptionEntryToReads", new[] { "UserSubscriptionId" });
            DropColumn("dbo.UserSubscriptionEntryToReads", "UserSubscriptionId");
        }
    }
}
