CREATE TABLE [dbo].[SocialLogins] (
    [Id]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [SocialId] NVARCHAR (MAX) NULL,
    [Provider] INT            NOT NULL,
    [UserId]   BIGINT         NOT NULL,
    [Created]  DATETIME       NOT NULL,
    [Updated]  DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.SocialLogins] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.SocialLogins_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[SocialLogins]([UserId] ASC);

