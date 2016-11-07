CREATE TABLE [dbo].[Kontrahenci](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdFirmy] [int] NOT NULL,
	[KodKontrahenta] [nvarchar](30) NOT NULL,
	[NazwaKontrahenta] [nvarchar](500) NOT NULL,
	[NIP] [nvarchar](20) NULL,
	[Miejscowosc] [nvarchar](100) NULL,
	[Ulica] [nvarchar](100) NULL,
	[NumerDomu] [nvarchar](5) NULL,
	[NumerLokalu] [nvarchar](5) NULL,
	[KodPocztowy] [nvarchar](10) NULL,
	[Panstwo] [nvarchar](30) NULL,
	[NumerKontaBankowego] [nvarchar](50) NULL,
	[CzyDomyslny] [bit] NULL,
	[IdKontrahentaNadrzednego] [int] NULL,
	[CzyVatowiec] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Kontrahenci]  WITH CHECK ADD  CONSTRAINT [FK_Kontrahenci_Firma] FOREIGN KEY([IdFirmy])
REFERENCES [dbo].[Firmy] ([Id])
GO

ALTER TABLE [dbo].[Kontrahenci] CHECK CONSTRAINT [FK_Kontrahenci_Firma]
GO
ALTER TABLE [dbo].[Kontrahenci]  WITH CHECK ADD  CONSTRAINT [FK_Kontrahenci_Kontrahenci] FOREIGN KEY([IdKontrahentaNadrzednego])
REFERENCES [dbo].[Kontrahenci] ([Id])
GO

ALTER TABLE [dbo].[Kontrahenci] CHECK CONSTRAINT [FK_Kontrahenci_Kontrahenci]
GO
/****** Object:  Index [KontrahenciDefault]    Script Date: 2016-11-07 09:41:49 ******/
CREATE UNIQUE NONCLUSTERED INDEX [KontrahenciDefault] ON [dbo].[Kontrahenci]
(
	[CzyDomyslny] ASC
)
WHERE ([CzyDomyslny]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]