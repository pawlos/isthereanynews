namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Navigationproperties : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.RssEntries", "RssChannelId");
            AddForeignKey("dbo.RssEntries", "RssChannelId", "dbo.RssChannels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RssEntries", "RssChannelId", "dbo.RssChannels");
            DropIndex("dbo.RssEntries", new[] { "RssChannelId" });
        }
    }
}
