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
    CONSTRAINT [CK__Pupils__Login__2E1BDC42] CHECK (len([Login])>=(5)),
    CONSTRAINT [CK__Pupils__Password__2F10007B] CHECK (len([Password])>=(6)),
    FOREIGN KEY ([GradeId]) REFERENCES [dbo].[Grades] ([GradeId]),
    UNIQUE NONCLUSTERED ([Login] ASC)
);



