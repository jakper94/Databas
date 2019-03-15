CREATE TABLE [dbo].[Tbl_User]
(
	[Us_UserName] NCHAR(8) NOT NULL, 
	[Us_Password] NCHAR(10) NULL,
    [Us_FirstName] NVARCHAR(20) NULL, 
    [Us_LastName] NVARCHAR(50) NULL, 
    [Us_HasVoted] BIT NULL, 
    [Us_IsAdmin] BIT NULL, 
    [Us_Class] NCHAR(4) NULL, 
	CONSTRAINT [Pk_Tbl_User] PRIMARY KEY ([Us_UserName]),
	
)

