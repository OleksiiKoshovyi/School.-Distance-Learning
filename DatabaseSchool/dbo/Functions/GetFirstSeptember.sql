-- =============================================
-- CurrDate date: <Create Date, ,>
-- =============================================
CREATE FUNCTION GetFirstSeptember
(
	@CurrDate DATE
)
RETURNS DATE
AS
BEGIN
	-- Declare the return variable here
	DECLARE @FirstSeptember DATE

	-- Add the T-SQL statements to compute the return value here
	SELECT @FirstSeptember = DATEFROMPARTS(
		DATEPART(YEAR, @CurrDate) - dbo.GetSemesterNumber(@CurrDate) + 1,
		9,
		1)

	-- Return the result of the function
	RETURN @FirstSeptember

END
