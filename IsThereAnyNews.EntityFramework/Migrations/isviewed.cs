namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Isviewed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RssEntriesToRead", "IsViewed", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.RssEntriesToRead", "IsViewed");
        }
    }
}
