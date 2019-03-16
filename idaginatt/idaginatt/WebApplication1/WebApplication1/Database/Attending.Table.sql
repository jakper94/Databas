CREATE TABLE [dbo].[Tbl_Attending]
(
	[At_Id] INT IDENTITY (1, 1) NOT NULL, 
	[At_User] NCHAR(8),
    [At_FoodPref] NVARCHAR(MAX) NULL,
    [At_Year] INT NOT NULL,
	CONSTRAINT [Pk_Tbl_Attending] PRIMARY KEY CLUSTERED ([At_Id] ASC),
	CONSTRAINT [FK_Tbl_User] FOREIGN KEY ([At_User]) REFERENCES [dbo].[Tbl_User] ([Us_UserName])

)
