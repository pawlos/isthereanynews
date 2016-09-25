namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedidnameintable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey(
                "dbo.UserSubscriptionEntryToReads",
                "EventRssUserInteraction_Id",
                "dbo.EventRssUserInteractions");
            DropIndex("dbo.UserSubscriptionEntryToReads", new[] { "EventRssUserInteraction_Id" });
            RenameColumn(
                table: "dbo.UserSubscriptionEntryToReads",
                name: "EventRssUserInteraction_Id",
                newName: "EventRssUserInteractionId");
            AlterColumn("dbo.UserSubscriptionEntryToReads", "EventRssUserInteractionId", c => c.Long(nullable: false));
            CreateIndex("dbo.UserSubscriptionEntryToReads", "EventRssUserInteractionId");
            AddForeignKey(
                "dbo.UserSubscriptionEntryToReads",
                "EventRssUserInteractionId",
                "dbo.EventRssUserInteractions",
                "Id",
                cascadeDelete: true);
            DropColumn("dbo.UserSubscriptionEntryToReads", "EventRssViewedId");
        }

        public override void Down()
        {
            AddColumn("dbo.UserSubscriptionEntryToReads", "EventRssViewedId", c => c.Long(nullable: false));
            DropForeignKey("dbo.UserSubscriptionEntryToReads", "EventRssUserInteractionId", "dbo.EventRssUserInteractions");
            DropIndex("dbo.UserSubscriptionEntryToReads", new[] { "EventRssUserInteractionId" });
            AlterColumn("dbo.UserSubscriptionEntryToReads", "EventRssUserInteractionId", c => c.Long());
            RenameColumn(table: "dbo.UserSubscriptionEntryToReads", name: "EventRssUserInteractionId", newName: "EventRssUserInteraction_Id");
            CreateIndex("dbo.UserSubscriptionEntryToReads", "EventRssUserInteraction_Id");
            AddForeignKey("dbo.UserSubscriptionEntryToReads", "EventRssUserInteraction_Id", "dbo.EventRssUserInteractions", "Id");
        }
    }
}
