CREATE TABLE [dbo].[data] (
    [Id]          INT            NOT NULL,
    [content]     NVARCHAR (MAX) NULL,
    [creation_dt] DATETIME2 (7)  NULL,
    [parent]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_parent_module_data] FOREIGN KEY ([parent]) REFERENCES [dbo].[modules] ([Id])
);


