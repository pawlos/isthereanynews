CREATE TABLE [dbo].[RssChannels] (
    [Id]                 BIGINT             IDENTITY (1, 1) NOT NULL,
    [Created]            DATETIME           NOT NULL,
    [Updated]            DATETIME           NOT NULL,
    [Title]              NVARCHAR (MAX)     NULL,
    [Url]                NVARCHAR (MAX)     NULL,
    [RssLastUpdatedTime] DATETIMEOFFSET (7) NOT NULL,
    [SubmitterId] BIGINT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_dbo.RssChannels] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_RssChannel_User_Submitter] FOREIGN KEY ([SubmitterId]) REFERENCES [dbo].[Users]([Id])
);

