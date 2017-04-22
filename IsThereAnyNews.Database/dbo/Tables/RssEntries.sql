CREATE TABLE [dbo].[RssEntries] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [Created]         DATETIME       NOT NULL,
    [Updated]         DATETIME       NOT NULL,
    [RssChannelId]    BIGINT         NOT NULL,
    [Url]             NVARCHAR (MAX) NULL,
    [PublicationDate] DATETIME       NOT NULL,
    [Title]           NVARCHAR (MAX) NULL,
    [PreviewText]     NVARCHAR (MAX) NULL,
    [RssId]           NVARCHAR (MAX) NULL,
    [StrippedText]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.RssEntries] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.RssEntries_dbo.RssChannels_RssChannelId] FOREIGN KEY ([RssChannelId]) REFERENCES [dbo].[RssChannels] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RssChannelId]
    ON [dbo].[RssEntries]([RssChannelId] ASC);

