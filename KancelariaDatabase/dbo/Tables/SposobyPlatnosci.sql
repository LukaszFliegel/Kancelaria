CREATE TABLE [dbo].[SposobyPlatnosci](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KodSposobuPlatnosci] [nvarchar](30) NOT NULL,
	[OpisSposobuPlatnosci] [nvarchar](300) NULL,
	[CzyDomyslny] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [SposobyPlatnosciDefault]    Script Date: 2016-11-07 09:41:49 ******/
CREATE UNIQUE NONCLUSTERED INDEX [SposobyPlatnosciDefault] ON [dbo].[SposobyPlatnosci]
(
	[CzyDomyslny] ASC
)
WHERE ([CzyDomyslny]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]