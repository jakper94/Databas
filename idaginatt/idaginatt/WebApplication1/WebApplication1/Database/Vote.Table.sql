CREATE TABLE [dbo].[Tbl_Vote]
(
	[Vo_Id] INT IDENTITY (1, 1) NOT NULL, 
    [Vo_Motivation] NVARCHAR(100) NULL, 
    [Vo_VotedOn] INT NOT NULL,
	CONSTRAINT [Pk_Tbl_Vote] PRIMARY KEY CLUSTERED ([Vo_Id] ASC),
	CONSTRAINT [FK_Tbl_Nominee] FOREIGN KEY ([Vo_VotedOn]) REFERENCES [dbo].[Tbl_Nominee] ([Nom_Id]) ON DELETE CASCADE
)
