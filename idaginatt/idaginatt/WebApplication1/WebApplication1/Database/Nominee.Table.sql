CREATE TABLE [dbo].[Tbl_Nominee]
(
	[Nom_Id] INT	IDENTITY (1, 1) NOT NULL,
    [Nom_FirstName] NCHAR(10) NULL, 
    [Nom_LastName] NCHAR(10) NULL, 
    [Nom_ImgLink] NCHAR(50) NULL,
	[Nom_Votes] NCHAR(50) NULL,
	CONSTRAINT [Pk_Tbl_Nominee] PRIMARY KEY CLUSTERED ([Nom_Id] ASC)
)
