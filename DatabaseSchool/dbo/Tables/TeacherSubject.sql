CREATE TABLE [dbo].[TeacherSubject] (
    [TeacherSubjectId] INT IDENTITY (1, 1) NOT NULL,
    [TeacherId]        INT NOT NULL,
    [SubjectId]        INT NOT NULL,
    [HoursNumber]      INT NOT NULL,
    PRIMARY KEY CLUSTERED ([TeacherSubjectId] ASC),
    CHECK ([HoursNumber]>(0) AND [HoursNumber]<=(40)),
    FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Subjects] ([SubjectId]) ON DELETE CASCADE,
    FOREIGN KEY ([TeacherId]) REFERENCES [dbo].[Teachers] ([TeacherId]) ON DELETE CASCADE,
    UNIQUE NONCLUSTERED ([TeacherId] ASC, [SubjectId] ASC)
);

