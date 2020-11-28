CREATE TABLE [dbo].[Teachers] (
    [TeacherId]       INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]       VARCHAR (25)  NOT NULL,
    [SurName]         VARCHAR (25)  NOT NULL,
    [Patronymic]      VARCHAR (25)  NULL,
    [DOB]             DATE          NOT NULL,
    [RecruitmentDate] DATE          NOT NULL,
    [Login]           VARCHAR (25)  NOT NULL,
    [Password]        VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([TeacherId] ASC),
    CHECK ([Login]>(5)),
    CHECK ([Password]>(6)),
    UNIQUE NONCLUSTERED ([Login] ASC)
);

