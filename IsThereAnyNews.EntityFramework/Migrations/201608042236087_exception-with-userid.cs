namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exceptionwithuserid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItanExceptions", "UserId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItanExceptions", "UserId");
        }
    }
}
