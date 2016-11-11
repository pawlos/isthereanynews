namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class strippedtext : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssEntries", "StrippedText", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RssEntries", "StrippedText");
        }
    }
}
