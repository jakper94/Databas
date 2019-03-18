CREATE TABLE [dbo].[Tbl_Nominee]
(
	[Nom_Id] INT	IDENTITY (1, 1) NOT NULL,
    [Nom_FirstName] NCHAR(50) NULL, 
    [Nom_LastName] NCHAR(50) NULL, 
    [Nom_ImgLink]   NVARCHAR(MAX) NULL,
	[Nom_Votes] INT NULL,
	[Nom_Year] INT NULL,
	CONSTRAINT [Pk_Tbl_Nominee] PRIMARY KEY CLUSTERED ([Nom_Id] ASC)
)
