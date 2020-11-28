CREATE TABLE [dbo].[TeacherSubjectGroup] (
    [TeacherSubjectGroupId] INT IDENTITY (1, 1) NOT NULL,
    [TeacherSubjectId]      INT NOT NULL,
    [GroupId]               INT NOT NULL,
    PRIMARY KEY CLUSTERED ([TeacherSubjectGroupId] ASC),
    FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([GroupId]) ON DELETE CASCADE,
    FOREIGN KEY ([TeacherSubjectId]) REFERENCES [dbo].[TeacherSubject] ([TeacherSubjectId]) ON DELETE CASCADE,
    UNIQUE NONCLUSTERED ([TeacherSubjectId] ASC, [GroupId] ASC)
);

