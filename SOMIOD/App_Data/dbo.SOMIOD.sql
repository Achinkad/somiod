DROP TABLE subscriptions,data,modules,applications;
CREATE TABLE [dbo].[applications] (
    [Id]           INT              IDENTITY,
    [name]         NVARCHAR (50)    NOT NULL UNIQUE,
    [creation_dt]  DATETIME2 (7)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[modules] (
    [Id]          INT               IDENTITY,
    [name]        NVARCHAR (50)     NOT NULL UNIQUE,
    [creation_dt] DATETIME2 (7)     NULL,
    [parent]      INT               NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_parent_app] FOREIGN KEY ([parent]) REFERENCES [dbo].[applications] ([Id])
);

CREATE TABLE [dbo].[data] (
    [Id]          INT            IDENTITY,
    [content]     NVARCHAR (MAX) NOT NULL,
    [creation_dt] DATETIME2 (7)  NULL,
    [parent]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_parent_module_data] FOREIGN KEY ([parent]) REFERENCES [dbo].[modules] ([Id])
);

CREATE TABLE [dbo].[subscriptions] (
    [Id]          INT            IDENTITY,
    [name]        NVARCHAR (50)  NOT NULL,
    [creation_dt] DATETIME2 (7)  NULL,
    [event]       NVARCHAR (MAX) NOT NULL,
    [endpoint]    NVARCHAR (50)  NOT NULL,
    [parent]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_parent_module_sub] FOREIGN KEY ([parent]) REFERENCES [dbo].[modules] ([Id])
);

