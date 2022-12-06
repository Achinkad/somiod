CREATE TABLE [dbo].[applications] (
    [Id]           INT           NOT NULL,
    [name]         NVARCHAR (50) NULL,
    [creationd_dt] DATETIME2 (7) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


