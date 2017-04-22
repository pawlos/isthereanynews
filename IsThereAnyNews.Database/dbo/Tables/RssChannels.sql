CREATE TABLE [dbo].[RssChannels] (
    [Id]                 BIGINT             IDENTITY (1, 1) NOT NULL,
    [Created]            DATETIME           NOT NULL,
    [Updated]            DATETIME           NOT NULL,
    [Title]              NVARCHAR (MAX)     NULL,
    [Url]                NVARCHAR (MAX)     NULL,
    [RssLastUpdatedTime] DATETIMEOFFSET (7) NOT NULL,
    CONSTRAINT [PK_dbo.RssChannels] PRIMARY KEY CLUSTERED ([Id] ASC)
);

