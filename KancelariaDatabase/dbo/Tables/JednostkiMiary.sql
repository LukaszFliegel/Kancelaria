CREATE TABLE [dbo].[JednostkiMiary](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KodJednostkiMiary] [nvarchar](30) NOT NULL,
	[OpisJednostkiMiary] [nvarchar](300) NULL,
	[CzyDomyslna] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [JednostkiMiaryDefault]    Script Date: 2016-11-07 09:41:49 ******/
CREATE UNIQUE NONCLUSTERED INDEX [JednostkiMiaryDefault] ON [dbo].[JednostkiMiary]
(
	[CzyDomyslna] ASC
)
WHERE ([CzyDomyslna]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]