CREATE TABLE [dbo].[Holidays] (
    [HolidayId]   INT          IDENTITY (1, 1) NOT NULL,
    [HolidayName] VARCHAR (50) NOT NULL,
    [StartDate]   DATE         NOT NULL,
    [Duration]    INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([HolidayId] ASC),
    CHECK ([Duration]>(0) AND [Duration]<(13))
);

