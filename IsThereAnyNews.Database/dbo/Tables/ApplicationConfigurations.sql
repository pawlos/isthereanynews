CREATE TABLE [dbo].[ApplicationConfigurations] (
    [Id]                    BIGINT   IDENTITY (1, 1) NOT NULL,
    [Created]               DATETIME NOT NULL,
    [Updated]               DATETIME NOT NULL,
    [RegistrationSupported] INT      NOT NULL,
    [UsersLimit]            BIGINT   DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.ApplicationConfigurations] PRIMARY KEY CLUSTERED ([Id] ASC)
);

