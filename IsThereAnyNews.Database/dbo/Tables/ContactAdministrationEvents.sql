CREATE TABLE [dbo].[ContactAdministrationEvents] (
    [Id]                      BIGINT   IDENTITY (1, 1) NOT NULL,
    [Created]                 DATETIME NOT NULL,
    [ContactAdministrationId] BIGINT   NOT NULL,
    CONSTRAINT [PK_dbo.ContactAdministrationEvents] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ContactAdministrationEvents_dbo.ContactAdministrations_ContactAdministrationId] FOREIGN KEY ([ContactAdministrationId]) REFERENCES [dbo].[ContactAdministrations] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ContactAdministrationId]
    ON [dbo].[ContactAdministrationEvents]([ContactAdministrationId] ASC);

