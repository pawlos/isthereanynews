CREATE TABLE [dbo].[UserSubscriptionEntryToReads] (
    [Id]                        BIGINT   IDENTITY (1, 1) NOT NULL,
    [Created]                   DATETIME NOT NULL,
    [Updated]                   DATETIME NOT NULL,
    [UserSubscriptionId]        BIGINT   NOT NULL,
    [IsRead]                    BIT      NOT NULL,
    [EventRssUserInteractionId] BIGINT   NOT NULL,
    [IsSkipped]                 BIT      NOT NULL DEFAULT ((0)) ,
    [IsViewed]                  BIT      NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_dbo.UserSubscriptionEntryToReads] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserSubscriptionEntryToReads_dbo.EventRssUserInteractions_EventRssUserInteractionId] FOREIGN KEY ([EventRssUserInteractionId]) REFERENCES [dbo].[EventRssUserInteractions] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.UserSubscriptionEntryToReads_dbo.UserSubscriptions_UserSubscriptionId] FOREIGN KEY ([UserSubscriptionId]) REFERENCES [dbo].[UserSubscriptions] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_EventRssUserInteractionId]
    ON [dbo].[UserSubscriptionEntryToReads]([EventRssUserInteractionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserSubscriptionId]
    ON [dbo].[UserSubscriptionEntryToReads]([UserSubscriptionId] ASC);

