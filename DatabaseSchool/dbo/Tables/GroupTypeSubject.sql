CREATE TABLE [dbo].[GroupTypeSubject] (
    [GroupTypeSubjectId] INT IDENTITY (1, 1) NOT NULL,
    [GroupTypeId]        INT NOT NULL,
    [SubjectId]          INT NOT NULL,
    PRIMARY KEY CLUSTERED ([GroupTypeSubjectId] ASC),
    FOREIGN KEY ([GroupTypeId]) REFERENCES [dbo].[GroupTypes] ([GroupTypeId]) ON DELETE CASCADE,
    FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subjects] ([SubjectId]) ON DELETE CASCADE,
    UNIQUE NONCLUSTERED ([GroupTypeId] ASC, [SubjectId] ASC)
);

