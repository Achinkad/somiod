CREATE TABLE [dbo].[modules] (
    [Id]          INT           NOT NULL,
    [name]        NVARCHAR (50) NULL,
    [creation_dt] DATETIME2 (7) NULL,
    [parent]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_parent_app] FOREIGN KEY ([parent]) REFERENCES [dbo].[applications] ([Id])
);


