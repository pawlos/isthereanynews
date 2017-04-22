CREATE TABLE [dbo].[EventItanExceptions] (
    [Id]              BIGINT           IDENTITY (1, 1) NOT NULL,
    [Created]         DATETIME         NOT NULL,
    [ErrorId]         UNIQUEIDENTIFIER NOT NULL,
    [ItanExceptionId] BIGINT           NOT NULL,
    CONSTRAINT [PK_dbo.EventItanExceptions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.EventItanExceptions_dbo.ItanExceptions_ItanExceptionId] FOREIGN KEY ([ItanExceptionId]) REFERENCES [dbo].[ItanExceptions] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ItanExceptionId]
    ON [dbo].[EventItanExceptions]([ItanExceptionId] ASC);

