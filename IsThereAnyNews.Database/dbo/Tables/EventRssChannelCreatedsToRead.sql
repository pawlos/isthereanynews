CREATE TABLE [dbo].[EventRssChannelCreatedsToRead] (
    [Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] BIGINT NOT NULL, 
    [Created] DATETIME NOT NULL, 
    [Updated] DATETIME NOT NULL,
    [IsViewed] BIT NOT NULL, 
    [IsSkipped] BIT NOT NULL, 
    [EventRssChannelCreatedId] BIGINT NOT NULL, 
    CONSTRAINT [FK_EventRssChannelCreatedsToRead_EventRssChannelCreateds] FOREIGN KEY ([EventRssChannelCreatedId]) REFERENCES [EventRssChannelCreateds]([Id])  ON DELETE CASCADE
);

