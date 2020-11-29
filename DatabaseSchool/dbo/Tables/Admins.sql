CREATE TABLE [dbo].[Admins] (
    [AdminId]  INT           IDENTITY (1, 1) NOT NULL,
    [Login]    VARCHAR (25)  NOT NULL,
    [Password] VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([AdminId] ASC),
    CONSTRAINT [CK__Admins__Login__25869641] CHECK (len([Login])>=(5)),
    CONSTRAINT [CK__Admins__Password__267ABA7A] CHECK (len([Password])>=(6)),
    UNIQUE NONCLUSTERED ([Login] ASC)
);



