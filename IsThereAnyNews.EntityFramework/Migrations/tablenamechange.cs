namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tablenamechange : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RssChannelUpdates", newName: "EventRssChannelUpdates");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.EventRssChannelUpdates", newName: "RssChannelUpdates");
        }
    }
}
