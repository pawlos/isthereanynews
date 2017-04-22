CREATE TABLE [dbo].[ItanExceptions] (
    [Id]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [Created]    DATETIME         NOT NULL,
    [Message]    NVARCHAR (MAX)   NULL,
    [Source]     NVARCHAR (MAX)   NULL,
    [Typeof]     NVARCHAR (MAX)   NULL,
    [Stacktrace] NVARCHAR (MAX)   NULL,
    [ErrorId]    UNIQUEIDENTIFIER NOT NULL,
    [UserId]     BIGINT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.ItanExceptions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

