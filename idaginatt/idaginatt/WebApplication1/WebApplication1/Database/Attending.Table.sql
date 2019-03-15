﻿CREATE TABLE [dbo].[Tbl_Attending]
(
	[At_Id] INT IDENTITY (1, 1) NOT NULL, 
    [At_FoodPred] NVARCHAR(MAX) NULL,
    [At_Year] INT NULL,
	CONSTRAINT [Pk_Tbl_Attending] PRIMARY KEY CLUSTERED ([At_Id] ASC), 
)
