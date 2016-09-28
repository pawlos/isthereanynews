namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class featurerequestextended : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeatureRequests", "StreamType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeatureRequests", "StreamType");
        }
    }
}
