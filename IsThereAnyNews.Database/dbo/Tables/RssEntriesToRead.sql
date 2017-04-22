CREATE TABLE [dbo].[RssEntriesToRead] (
    [Id]                       BIGINT   NOT NULL,
    [Created]                  DATETIME NOT NULL,
    [Updated]                  DATETIME NOT NULL,
    [IsRead]                   BIT      NOT NULL,
    [IsViewed]                 BIT      NOT NULL,
    [RssChannelSubscriptionId] BIGINT   NOT NULL,
    [RssEntryId]               BIGINT   NOT NULL,
    [IsSkipped]                BIT      DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.RssEntriesToRead] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.RssEntriesToRead_dbo.RssChannelSubscriptions_RssChannelSubscriptionId] FOREIGN KEY ([RssChannelSubscriptionId]) REFERENCES [dbo].[RssChannelSubscriptions] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Id]
    ON [dbo].[RssEntriesToRead]([Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RssChannelSubscriptionId]
    ON [dbo].[RssEntriesToRead]([RssChannelSubscriptionId] ASC);

