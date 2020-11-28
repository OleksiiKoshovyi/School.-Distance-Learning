CREATE TABLE [dbo].[Pupils] (
    [PupilId]    INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]  VARCHAR (25)  NOT NULL,
    [SurName]    VARCHAR (25)  NOT NULL,
    [Patronymic] VARCHAR (25)  NULL,
    [DOB]        DATE          NOT NULL,
    [GradeId]    INT           NOT NULL,
    [Login]      VARCHAR (25)  NOT NULL,
    [Password]   VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([PupilId] ASC),
    CHECK ([Login]>(5)),
    CHECK ([Password]>(6)),
    FOREIGN KEY ([GradeId]) REFERENCES [dbo].[Grades] ([GradeId]),
    UNIQUE NONCLUSTERED ([Login] ASC)
);

