CREATE TABLE [dbo].[UserRoles] (
    [Id]       BIGINT   IDENTITY (1, 1) NOT NULL,
    [Created]  DATETIME NOT NULL,
    [UserId]   BIGINT   NOT NULL,
    [RoleType] INT      NOT NULL,
    CONSTRAINT [PK_dbo.UserRoles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserRoles_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[UserRoles]([UserId] ASC);

