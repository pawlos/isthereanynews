CREATE TABLE [dbo].[EventRssChannelUpdateToRead]
(
    [Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] BIGINT NOT NULL, 
    [Created] DATETIME NOT NULL, 
    [Updated] DATETIME NOT NULL,
    [IsViewed] BIT NOT NULL, 
    [IsSkipped] BIT NOT NULL, 
    [EventRssChannelUpdatedId] BIGINT NOT NULL, 
    CONSTRAINT [FK_EventRssChannelUpdateToRead_EventRssChannelUpdates] FOREIGN KEY ([EventRssChannelUpdatedId]) REFERENCES [EventRssChannelUpdates]([Id])  ON DELETE CASCADE
)
