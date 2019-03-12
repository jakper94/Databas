CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Username] NCHAR(10) NOT NULL, 
    [Password] NCHAR(30) NOT NULL, 
    [FirstName] NCHAR(30) NULL, 
    [LastName] NCHAR(30) NULL, 
    [HasVoted] NCHAR(10) NULL, 
    [Class] NCHAR(10) NULL, 
    [IsAdmin] NCHAR(10) NULL
)
