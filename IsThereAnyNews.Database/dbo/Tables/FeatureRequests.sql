CREATE TABLE [dbo].[FeatureRequests] (
    [Id]         BIGINT   IDENTITY (1, 1) NOT NULL,
    [Created]    DATETIME NOT NULL,
    [Updated]    DATETIME NOT NULL,
    [Type]       INT      NOT NULL,
    [UserId]     BIGINT   NOT NULL,
    [RssEntryId] BIGINT   NOT NULL,
    [StreamType] INT      DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.FeatureRequests] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.FeatureRequests_dbo.RssEntries_RssEntryId] FOREIGN KEY ([RssEntryId]) REFERENCES [dbo].[RssEntries] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.FeatureRequests_dbo.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[FeatureRequests]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_RssEntryId]
    ON [dbo].[FeatureRequests]([RssEntryId] ASC);

