CREATE TABLE [dbo].[Users] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Created]      DATETIME       NOT NULL,
    [Updated]      DATETIME       NOT NULL,
    [DisplayName]  NVARCHAR (MAX) NULL,
    [Email]        NVARCHAR (MAX) NULL,
    [LastReadTime] DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

            CREATE TRIGGER User_InsteadOfDelete
            on Users
            INSTEAD OF Delete
            AS
            BEGIN
                DELETE FROM UserSubscriptions WHERE UserSubscriptions.FollowerId IN (SELECT Id FROM deleted)
                DELETE FROM UserSubscriptions WHERE UserSubscriptions.ObservedId IN (SELECT Id FROM deleted)
                DELETE FROM Users WHERE Id IN (SELECT Id FROM deleted)
            END
            