CREATE TABLE [dbo].[GradeSubject] (
    [GradeSubjectId] INT IDENTITY (1, 1) NOT NULL,
    [GradeId]        INT NOT NULL,
    [SubjectId]      INT NOT NULL,
    [HoursNumber]    INT NOT NULL,
    PRIMARY KEY CLUSTERED ([GradeSubjectId] ASC),
    CHECK ([HoursNumber]>(0) AND [HoursNumber]<=(40)),
    FOREIGN KEY ([GradeId]) REFERENCES [dbo].[Grades] ([GradeId]) ON DELETE CASCADE,
    FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subjects] ([SubjectId]) ON DELETE CASCADE,
    UNIQUE NONCLUSTERED ([GradeId] ASC, [SubjectId] ASC)
);

