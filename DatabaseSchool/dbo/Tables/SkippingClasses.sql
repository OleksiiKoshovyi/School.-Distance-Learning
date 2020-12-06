CREATE TABLE [dbo].[SkippingClasses] (
    [SkippingClassId] INT IDENTITY (1, 1) NOT NULL,
    [TimetableId]     INT NOT NULL,
    [PupilId]         INT NOT NULL,
    [WeekNumber]      INT NOT NULL,
    PRIMARY KEY CLUSTERED ([SkippingClassId] ASC),
    FOREIGN KEY ([PupilId]) REFERENCES [dbo].[Pupils] ([PupilId]),
    FOREIGN KEY ([TimetableId]) REFERENCES [dbo].[Timetables] ([TimetableId]),
    UNIQUE NONCLUSTERED ([TimetableId] ASC, [PupilId] ASC, [WeekNumber] ASC)
);



