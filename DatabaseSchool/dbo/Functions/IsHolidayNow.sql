-- =============================================
-- CurrDate date: <CurrDate Date>
-- =============================================
CREATE FUNCTION IsHolidayNow
(
	-- Add the parameters for the function here
	@CurrDate DATE
)
RETURNS BIT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @IsHoliday BIT

	-- Add the T-SQL statements to compute the return value here
	SELECT @IsHoliday = CASE WHEN (SELECT MAX(DATEADD(WEEK, Duration, StartDate))
		FROM Holidays WHERE StartDate <= @CurrDate) >= @CurrDate THEN 1 ELSE 0 END

	-- Return the result of the function
	RETURN @IsHoliday

END
