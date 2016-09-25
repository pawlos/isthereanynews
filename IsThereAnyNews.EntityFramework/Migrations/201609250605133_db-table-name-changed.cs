namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class dbtablenamechanged : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EventRssVieweds", newName: "EventRssUserInteractions");
            DropForeignKey("dbo.UserSubscriptionEntryToReads", "EventRssViewedId", "dbo.EventRssVieweds");
            DropIndex("dbo.UserSubscriptionEntryToReads", new[] { "EventRssViewedId" });
            AddColumn("dbo.UserSubscriptionEntryToReads", "EventRssUserInteraction_Id", c => c.Long());
            CreateIndex("dbo.UserSubscriptionEntryToReads", "EventRssUserInteraction_Id");
            AddForeignKey("dbo.UserSubscriptionEntryToReads", "EventRssUserInteraction_Id", "dbo.EventRssUserInteractions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSubscriptionEntryToReads", "EventRssUserInteraction_Id", "dbo.EventRssUserInteractions");
            DropIndex("dbo.UserSubscriptionEntryToReads", new[] { "EventRssUserInteraction_Id" });
            DropColumn("dbo.UserSubscriptionEntryToReads", "EventRssUserInteraction_Id");
            CreateIndex("dbo.UserSubscriptionEntryToReads", "EventRssViewedId");
            AddForeignKey("dbo.UserSubscriptionEntryToReads", "EventRssViewedId", "dbo.EventRssVieweds", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.EventRssUserInteractions", newName: "EventRssVieweds");
        }
    }
}
