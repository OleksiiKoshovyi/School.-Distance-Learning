CREATE TABLE [dbo].[Admins] (
    [AdminId]  INT           IDENTITY (1, 1) NOT NULL,
    [Login]    VARCHAR (25)  NOT NULL,
    [Password] VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([AdminId] ASC),
    CHECK ([Login]>(5)),
    CHECK ([Password]>(6)),
    UNIQUE NONCLUSTERED ([Login] ASC)
);

