namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class interactiontypeaddedtotable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventRssUserInteractions", "InteractionType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventRssUserInteractions", "InteractionType");
        }
    }
}
