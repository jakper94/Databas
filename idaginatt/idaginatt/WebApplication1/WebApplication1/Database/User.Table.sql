CREATE TABLE [dbo].[Tbl_User]
(
	[Us_Id] INT IDENTITY (1, 1) NOT NULL,
	[Us_UserName] NCHAR(10) NULL, 
	[Us_Password] NCHAR(50) NULL,
    [Us_FirstName] NCHAR(10) NULL, 
    [Us_LastName] NCHAR(10) NULL, 
    [Us_HasVoted] NCHAR(10) NULL, 
    [Us_IsAdmin] NCHAR(10) NULL, 
    [Us_Class] NCHAR(10) NULL, 
    [Us_HasChangedPassword] NCHAR(10) NULL,
	CONSTRAINT [Pk_Tbl_User] PRIMARY KEY CLUSTERED ([Us_Id] ASC),
	
)

