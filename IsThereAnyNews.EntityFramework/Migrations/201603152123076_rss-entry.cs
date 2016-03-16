namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class rssentry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RssEntries",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Created = c.DateTime(nullable: false),
                    Updated = c.DateTime(nullable: false),
                    RssChannelId = c.Long(nullable: false),
                    PublicationDate = c.DateTime(nullable: false),
                    Title = c.String(),
                    PreviewText = c.String(),
                    RssId = c.String(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.RssEntries");
        }
    }
}
