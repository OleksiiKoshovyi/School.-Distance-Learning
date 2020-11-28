CREATE TABLE [dbo].[Timetables] (
    [TimetableId]           INT IDENTITY (1, 1) NOT NULL,
    [WeekDayNumber]         INT NOT NULL,
    [LessonNumber]          INT NOT NULL,
    [Oddness]               INT NOT NULL,
    [TeacherSubjectGroupId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([TimetableId] ASC),
    CHECK ([LessonNumber]>(0) AND [LessonNumber]<(10)),
    CHECK ([Oddness]>=(0) AND [Oddness]<=(2)),
    CHECK ([WeekDayNumber]>(0) AND [WeekDayNumber]<(8)),
    FOREIGN KEY ([TeacherSubjectGroupId]) REFERENCES [dbo].[TeacherSubjectGroup] ([TeacherSubjectGroupId]) ON DELETE CASCADE
);

