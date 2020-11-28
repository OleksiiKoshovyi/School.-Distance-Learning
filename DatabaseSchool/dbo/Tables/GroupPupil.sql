CREATE TABLE [dbo].[GroupPupil] (
    [GroupPupilId] INT IDENTITY (1, 1) NOT NULL,
    [GroupId]      INT NOT NULL,
    [PupilId]      INT NOT NULL,
    PRIMARY KEY CLUSTERED ([GroupPupilId] ASC),
    FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Groups] ([GroupId]) ON DELETE CASCADE,
    FOREIGN KEY ([PupilId]) REFERENCES [dbo].[Pupils] ([PupilId]) ON DELETE CASCADE,
    UNIQUE NONCLUSTERED ([GroupId] ASC, [PupilId] ASC)
);

