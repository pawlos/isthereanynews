namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contact_administration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactAdministrations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Topic = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactAdministrationEvents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        ContactAdministrationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactAdministrations", t => t.ContactAdministrationId, cascadeDelete: true)
                .Index(t => t.ContactAdministrationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactAdministrationEvents", "ContactAdministrationId", "dbo.ContactAdministrations");
            DropIndex("dbo.ContactAdministrationEvents", new[] { "ContactAdministrationId" });
            DropTable("dbo.ContactAdministrationEvents");
            DropTable("dbo.ContactAdministrations");
        }
    }
}
