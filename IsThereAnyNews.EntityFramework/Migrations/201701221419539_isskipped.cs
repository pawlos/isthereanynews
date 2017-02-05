namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isskipped : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssEntriesToRead", "IsSkipped", c => c.Boolean(nullable: false));
            AddColumn("dbo.UserSubscriptionEntryToReads", "IsSkipped", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSubscriptionEntryToReads", "IsSkipped");
            DropColumn("dbo.RssEntriesToRead", "IsSkipped");
        }
    }
}
