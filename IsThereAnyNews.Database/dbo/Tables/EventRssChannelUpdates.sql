CREATE TABLE [dbo].[EventRssChannelUpdates] (
    [Id]           BIGINT   IDENTITY (1, 1) NOT NULL,
    [Created]      DATETIME NOT NULL,
    [RssChannelId] BIGINT   NOT NULL,
    CONSTRAINT [PK_dbo.EventRssChannelUpdates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.EventRssChannelUpdates_dbo.RssChannels_RssChannelId] FOREIGN KEY ([RssChannelId]) REFERENCES [dbo].[RssChannels] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RssChannelId]
    ON [dbo].[EventRssChannelUpdates]([RssChannelId] ASC);

