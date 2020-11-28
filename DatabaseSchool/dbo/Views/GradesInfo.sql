CREATE VIEW GradesInfo AS
SELECT GradeId, (dbo.GetGradeNumber(GETDATE(), FirstYear))|Letter AS GradeName 
FROM Grades;