-- =============================================
-- CurrDate date: <CurrDate Date>
-- =============================================
CREATE FUNCTION GetOddness
(
	@CurrDate DATE
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Oddness INT

	-- Add the T-SQL statements to compute the return value here
	SELECT @Oddness = dbo.GetWeekNumber(@CurrDate) % 2 + 1

	-- Return the result of the function
	RETURN @Oddness

END
