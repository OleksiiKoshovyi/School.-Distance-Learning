CREATE TABLE [dbo].[Grades] (
    [GradeId]   INT         IDENTITY (1, 1) NOT NULL,
    [FirstYear] INT         NOT NULL,
    [Letter]    VARCHAR (1) NOT NULL,
    PRIMARY KEY CLUSTERED ([GradeId] ASC),
    UNIQUE NONCLUSTERED ([FirstYear] ASC, [Letter] ASC)
);


GO

CREATE TRIGGER [dbo].[Grades_INSERT] 
ON [dbo].[Grades] AFTER INSERT
AS 
/*IF IS_MEMBER ('db_owner') = 0*/
BEGIN
   INSERT INTO Groups(GradeId) SELECT GradeId FROM inserted
END
