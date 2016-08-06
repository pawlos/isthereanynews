namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exceptionevents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventItanExceptions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        ErrorId = c.Guid(nullable: false),
                        ItanExceptionId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ItanExceptions", t => t.ItanExceptionId, cascadeDelete: true)
                .Index(t => t.ItanExceptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventItanExceptions", "ItanExceptionId", "dbo.ItanExceptions");
            DropIndex("dbo.EventItanExceptions", new[] { "ItanExceptionId" });
            DropTable("dbo.EventItanExceptions");
        }
    }
}
