CREATE TABLE [dbo].[Homeworks] (
    [HomeworkId]            INT  IDENTITY (1, 1) NOT NULL,
    [PassDate]              DATE NOT NULL,
    [TeacherSubjectGroupId] INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([HomeworkId] ASC),
    FOREIGN KEY ([TeacherSubjectGroupId]) REFERENCES [dbo].[TeacherSubjectGroup] ([TeacherSubjectGroupId]) ON DELETE CASCADE,
    UNIQUE NONCLUSTERED ([PassDate] ASC, [TeacherSubjectGroupId] ASC)
);

