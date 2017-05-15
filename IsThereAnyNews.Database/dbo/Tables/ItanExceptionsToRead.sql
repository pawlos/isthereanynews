CREATE TABLE [dbo].[ItanExceptionsToRead]
(
    [Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] BIGINT NOT NULL, 
    [Created] DATETIME NOT NULL, 
    [Updated] DATETIME NOT NULL,
    [IsViewed] BIT NOT NULL, 
    [IsSkipped] BIT NOT NULL, 
    [ItanExceptionId] BIGINT NOT NULL, 
    CONSTRAINT [FK_ItanExceptionsToRead_ToTable] FOREIGN KEY ([ItanExceptionId]) REFERENCES ItanExceptions([Id])  ON DELETE CASCADE
)
