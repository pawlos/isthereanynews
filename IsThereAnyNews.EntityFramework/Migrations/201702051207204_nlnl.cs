namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nlnl : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RssEntriesToRead", "Id", "dbo.RssEntries");
        }
        
        public override void Down()
        {
        }
    }
}
