namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rsslastupdateonchannel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssChannels", "RssLastUpdatedTime", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RssChannels", "RssLastUpdatedTime");
        }
    }
}
