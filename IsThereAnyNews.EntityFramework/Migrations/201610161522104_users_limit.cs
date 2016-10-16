namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class users_limit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationConfigurations", "UsersLimit", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationConfigurations", "UsersLimit");
        }
    }
}
