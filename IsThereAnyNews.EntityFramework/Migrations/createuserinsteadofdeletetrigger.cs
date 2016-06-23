namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Createuserinsteadofdeletetrigger : DbMigration
    {
        public override void Up()
        {
            string queryToExecute =
            @"
            CREATE TRIGGER User_InsteadOfDelete
            on Users
            INSTEAD OF Delete
            AS
            BEGIN
                DELETE FROM UserSubscriptions WHERE UserSubscriptions.FollowerId IN (SELECT Id FROM deleted)
                DELETE FROM UserSubscriptions WHERE UserSubscriptions.ObservedId IN (SELECT Id FROM deleted)
                DELETE FROM Users WHERE Id IN (SELECT Id FROM deleted)
            END
            ";
            this.Sql(queryToExecute);
        }

        public override void Down()
        {
            string queryToExecute = "DROP TRIGGER User_InsteadOfDelete";
            this.Sql(queryToExecute);
        }
    }
}
