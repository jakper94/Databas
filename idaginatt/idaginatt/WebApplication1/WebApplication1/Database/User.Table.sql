CREATE TABLE [dbo].[Tbl_User]
(
	[Us_Id] INT IDENTITY (1, 1) NOT NULL,
	[Us_UserName] NCHAR(8) NULL, 
	[Us_Password] NCHAR(10) NULL,
    [Us_FirstName] NVARCHAR(20) NULL, 
    [Us_LastName] NVARCHAR(50) NULL, 
    [Us_HasVoted] BIT NULL, 
    [Us_IsAdmin] BIT NULL, 
    [Us_Class] NCHAR(4) NULL, 
    [Us_HasChangedPassword] BIT NULL,
	CONSTRAINT [Pk_Tbl_User] PRIMARY KEY CLUSTERED ([Us_Id] ASC),
	
)

