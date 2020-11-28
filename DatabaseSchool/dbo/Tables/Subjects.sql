CREATE TABLE [dbo].[Subjects] (
    [SubjectId]   INT          IDENTITY (1, 1) NOT NULL,
    [SubjectName] VARCHAR (50) NOT NULL,
    [Complexity]  INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([SubjectId] ASC),
    CHECK ([Complexity]>(0))
);

