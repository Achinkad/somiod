CREATE TABLE [dbo].[subscriptions] (
    [Id]          INT            NOT NULL,
    [name]        NVARCHAR (50)  NULL,
    [creation_dt] DATETIME2 (7)  NULL,
    [event]       NVARCHAR (MAX) NULL,
    [endpoint]    NVARCHAR (50)  NULL,
    [parent]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_parent_module_sub] FOREIGN KEY ([parent]) REFERENCES [dbo].[modules] ([Id])
);

