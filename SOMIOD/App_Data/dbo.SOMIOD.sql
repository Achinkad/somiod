DROP TABLE subscriptions,data,modules,applications;
CREATE TABLE [dbo].[applications] (
    [Id]           INT           NOT NULL,
    [name]         NVARCHAR (50) NULL,
    [creationd_dt] DATETIME2 (7) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) 
);
CREATE TABLE [dbo].[modules] (
    [Id]          INT           NOT NULL,
    [name]        NVARCHAR (50) NULL,
    [creation_dt] DATETIME2 (7) NULL,
    [parent]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_parent_app] FOREIGN KEY ([parent]) REFERENCES [dbo].[applications] ([Id])
);

CREATE TABLE [dbo].[data] (
    [Id]          INT            NOT NULL,
    [content]     NVARCHAR (MAX) NULL,
    [creation_dt] DATETIME2 (7)  NULL,
    [parent]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_parent_module_data] FOREIGN KEY ([parent]) REFERENCES [dbo].[modules] ([Id])
);

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

