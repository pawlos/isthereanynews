CREATE TABLE [dbo].[EventRssChannelCreateds] (
    [Id]           BIGINT   IDENTITY (1, 1) NOT NULL,
    [Created]      DATETIME NOT NULL,
    [RssChannelId] BIGINT   NOT NULL,
    CONSTRAINT [PK_dbo.EventRssChannelCreateds] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.EventRssChannelCreateds_dbo.RssChannels_RssChannelId] FOREIGN KEY ([RssChannelId]) REFERENCES [dbo].[RssChannels] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RssChannelId]
    ON [dbo].[EventRssChannelCreateds]([RssChannelId] ASC);

