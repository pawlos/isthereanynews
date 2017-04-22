CREATE TABLE [dbo].[EventRssUserInteractions] (
    [Id]              BIGINT   IDENTITY (1, 1) NOT NULL,
    [Created]         DATETIME NOT NULL,
    [UserId]          BIGINT   NOT NULL,
    [RssEntryId]      BIGINT   NOT NULL,
    [InteractionType] INT      DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.EventRssUserInteractions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.EventRssVieweds_dbo.RssEntries_RssEntryId] FOREIGN KEY ([RssEntryId]) REFERENCES [dbo].[RssEntries] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.EventRssVieweds_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[EventRssUserInteractions]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RssEntryId]
    ON [dbo].[EventRssUserInteractions]([RssEntryId] ASC);

