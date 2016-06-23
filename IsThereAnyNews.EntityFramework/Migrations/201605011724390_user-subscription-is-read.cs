namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Usersubscriptionisread : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSubscriptionEntryToReads", "IsRead", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserSubscriptionEntryToReads", "IsRead");
        }
    }
}
