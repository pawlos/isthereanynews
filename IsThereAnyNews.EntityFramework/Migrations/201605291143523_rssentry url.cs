namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rssentryurl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssEntries", "Url", c => c.String(nullable:false, defaultValue:string.Empty));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RssEntries", "Url");
        }
    }
}
