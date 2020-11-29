-- =============================================
-- CurrDate date: <CurrDate Date>
-- =============================================
CREATE FUNCTION GetSemesterNumber
(
	@CurrDate DATE
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @SemestrNumber int

	-- Add the T-SQL statements to compute the return value here
	SELECT @SemestrNumber = (CASE WHEN MONTH(@CurrDate) < 9 THEN 2 ELSE 1 END);

	-- Return the result of the function
	RETURN @SemestrNumber

END
