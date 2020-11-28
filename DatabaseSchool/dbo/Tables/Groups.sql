CREATE TABLE [dbo].[Groups] (
    [GroupId]     INT IDENTITY (1, 1) NOT NULL,
    [GroupTypeId] INT NULL,
    [GradeId]     INT NOT NULL,
    PRIMARY KEY CLUSTERED ([GroupId] ASC),
    FOREIGN KEY ([GradeId]) REFERENCES [dbo].[Grades] ([GradeId]),
    FOREIGN KEY ([GroupTypeId]) REFERENCES [dbo].[GroupTypes] ([GroupTypeId])
);

