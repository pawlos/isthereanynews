namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itanexceptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItanExceptions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Message = c.String(),
                        Source = c.String(),
                        Typeof = c.String(),
                        Stacktrace = c.String(),
                        ErrorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ItanExceptions");
        }
    }
}
