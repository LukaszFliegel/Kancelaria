CREATE TABLE [dbo].[KontaBankowe](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdFirmy] [int] NOT NULL,
	[NumerKonta] [nvarchar](50) NOT NULL,
	[Nazwa] [nvarchar](200) NULL,
	[CzyDomyslny] [bit] NULL,
	[NazwaBanku] [nvarchar](300) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[KontaBankowe]  WITH CHECK ADD  CONSTRAINT [FK_KontaBankowe_Firmy] FOREIGN KEY([IdFirmy])
REFERENCES [dbo].[Firmy] ([Id])
GO

ALTER TABLE [dbo].[KontaBankowe] CHECK CONSTRAINT [FK_KontaBankowe_Firmy]
GO
/****** Object:  Index [KontaBankoweDefault]    Script Date: 2016-11-07 09:41:49 ******/
CREATE UNIQUE NONCLUSTERED INDEX [KontaBankoweDefault] ON [dbo].[KontaBankowe]
(
	[CzyDomyslny] ASC,
	[IdFirmy] ASC
)
WHERE ([CzyDomyslny]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [KontaBankoweUniq1]    Script Date: 2016-11-07 09:41:49 ******/
CREATE UNIQUE NONCLUSTERED INDEX [KontaBankoweUniq1] ON [dbo].[KontaBankowe]
(
	[Id] ASC,
	[IdFirmy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]