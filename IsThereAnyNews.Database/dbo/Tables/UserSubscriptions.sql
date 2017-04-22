CREATE TABLE [dbo].[UserSubscriptions] (
    [Id]         BIGINT   IDENTITY (1, 1) NOT NULL,
    [Created]    DATETIME NOT NULL,
    [Updated]    DATETIME NOT NULL,
    [FollowerId] BIGINT   NOT NULL,
    [ObservedId] BIGINT   NOT NULL,
    CONSTRAINT [PK_dbo.UserSubscriptions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserSubscriptions_dbo.Users_FollowerId] FOREIGN KEY ([FollowerId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_dbo.UserSubscriptions_dbo.Users_ObservedId] FOREIGN KEY ([ObservedId]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FollowerId]
    ON [dbo].[UserSubscriptions]([FollowerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ObservedId]
    ON [dbo].[UserSubscriptions]([ObservedId] ASC);

