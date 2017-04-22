CREATE TABLE [dbo].[RssChannelSubscriptions] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Created]      DATETIME       NOT NULL,
    [Updated]      DATETIME       NOT NULL,
    [Title]        NVARCHAR (MAX) NULL,
    [UserId]       BIGINT         NOT NULL,
    [RssChannelId] BIGINT         NOT NULL,
    CONSTRAINT [PK_dbo.RssChannelSubscriptions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.RssChannelSubscriptions_dbo.RssChannels_RssChannelId] FOREIGN KEY ([RssChannelId]) REFERENCES [dbo].[RssChannels] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.RssChannelSubscriptions_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[RssChannelSubscriptions]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RssChannelId]
    ON [dbo].[RssChannelSubscriptions]([RssChannelId] ASC);

