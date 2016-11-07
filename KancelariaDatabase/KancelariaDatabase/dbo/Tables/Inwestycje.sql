CREATE TABLE [dbo].[Inwestycje](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdFirmy] [int] NOT NULL,
	[NumerInwestycji] [nvarchar](300) NOT NULL,
	[NumerUmowy] [nvarchar](300) NULL,
	[CzyDomyslny] [bit] NULL,
	[IdTypuInwestycji] [int] NOT NULL,
	[Opis] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Inwestycje]  WITH CHECK ADD  CONSTRAINT [FK_Inwestycje_Firmy] FOREIGN KEY([IdFirmy])
REFERENCES [dbo].[Firmy] ([Id])
GO

ALTER TABLE [dbo].[Inwestycje] CHECK CONSTRAINT [FK_Inwestycje_Firmy]
GO
ALTER TABLE [dbo].[Inwestycje]  WITH CHECK ADD  CONSTRAINT [FK_Inwestycje_TypyInwestycji] FOREIGN KEY([IdTypuInwestycji])
REFERENCES [dbo].[TypyInwestycji] ([Id])
GO

ALTER TABLE [dbo].[Inwestycje] CHECK CONSTRAINT [FK_Inwestycje_TypyInwestycji]
GO
/****** Object:  Index [InwestycjeDefault]    Script Date: 2016-11-07 09:41:49 ******/
CREATE UNIQUE NONCLUSTERED INDEX [InwestycjeDefault] ON [dbo].[Inwestycje]
(
	[CzyDomyslny] ASC
)
WHERE ([CzyDomyslny]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]