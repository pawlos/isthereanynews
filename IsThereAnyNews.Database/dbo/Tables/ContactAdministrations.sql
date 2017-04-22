CREATE TABLE [dbo].[ContactAdministrations] (
    [Id]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [Created] DATETIME       NOT NULL,
    [Name]    NVARCHAR (MAX) NULL,
    [Email]   NVARCHAR (MAX) NULL,
    [Topic]   NVARCHAR (MAX) NULL,
    [Message] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.ContactAdministrations] PRIMARY KEY CLUSTERED ([Id] ASC)
);

