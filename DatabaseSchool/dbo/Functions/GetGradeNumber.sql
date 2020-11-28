-- =============================================
-- CurrDate date: <Create Date, ,>
-- =============================================
CREATE FUNCTION GetGradeNumber
(
	@CurrDate DATETIME,
	@FirstYear INT
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @GradeNumber INT

	-- Add the T-SQL statements to compute the return value here
	SELECT @GradeNumber = YEAR(@CurrDate) - @FirstYear + 2 - [dbo].[GetSemesterNumber](@CurrDate)

	-- Return the result of the function
	RETURN @GradeNumber

END
