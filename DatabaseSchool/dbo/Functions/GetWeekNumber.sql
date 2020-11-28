-- =============================================
-- CurrDate date: <Create Date, ,>
-- =============================================
CREATE FUNCTION [dbo].[GetWeekNumber]
(
	@CurrDate DATE
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @WeekNumber INT

	-- Add the T-SQL statements to compute the return value here
	SELECT @WeekNumber = DATEDIFF(week, dbo.GetFirstSeptember(@CurrDate), @CurrDate) 
		- (SELECT SUM(Duration) FROM Holidays WHERE StartDate <= @CurrDate)

	-- Return the result of the function
	RETURN @WeekNumber

END
