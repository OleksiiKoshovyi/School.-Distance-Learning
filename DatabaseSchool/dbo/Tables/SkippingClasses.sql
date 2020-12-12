CREATE TABLE [dbo].[SkippingClasses] (
    [SkippingClassId] INT  IDENTITY (1, 1) NOT NULL,
    [SkippingDate]    DATE NOT NULL,
    [PupilId]         INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([SkippingClassId] ASC),
    FOREIGN KEY ([PupilId]) REFERENCES [dbo].[Pupils] ([PupilId]),
    UNIQUE NONCLUSTERED ([SkippingDate] ASC, [PupilId] ASC)
);





