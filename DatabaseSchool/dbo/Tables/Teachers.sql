CREATE TABLE [dbo].[Teachers] (
    [TeacherId]       INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]       VARCHAR (25)  NOT NULL,
    [SurName]         VARCHAR (25)  NOT NULL,
    [Patronymic]      VARCHAR (25)  NULL,
    [DOB]             DATE          NOT NULL,
    [RecruitmentDate] DATE          NOT NULL,
    [Login]           VARCHAR (25)  NOT NULL,
    [Password]        VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([TeacherId] ASC),
    CONSTRAINT [CK__Teachers__Login__32E0915F] CHECK (len([Login])>=(5)),
    CONSTRAINT [CK__Teachers__Passwo__33D4B598] CHECK (len([Password])>=(6)),
    UNIQUE NONCLUSTERED ([Login] ASC)
);



