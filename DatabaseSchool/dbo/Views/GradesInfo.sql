CREATE VIEW GradesInfo AS
SELECT GradeId, CONCAT(STR(dbo.GetGradeNumber(GETDATE(), FirstYear)),' ',Letter) AS GradeName 
FROM Grades;