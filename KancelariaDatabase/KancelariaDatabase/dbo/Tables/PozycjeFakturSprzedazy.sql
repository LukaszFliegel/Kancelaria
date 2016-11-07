CREATE TABLE [dbo].[PozycjeFakturSprzedazy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NumerPozycji] [int] NOT NULL,
	[IdFaktury] [int] NOT NULL,
	[CenaJednostkowa] [decimal](16, 2) NOT NULL,
	[StawkaVat] [int] NOT NULL,
	[Opis] [nvarchar](1000) NULL,
	[IdJednostkiMiary] [int] NOT NULL,
	[Ilosc] [int] NOT NULL,
	[CzyBrutto] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PozycjeFakturSprzedazy]  WITH CHECK ADD  CONSTRAINT [FK_PozycjeFakturSprzedazy_FakturySprzedazy] FOREIGN KEY([IdFaktury])
REFERENCES [dbo].[FakturySprzedazy] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[PozycjeFakturSprzedazy] CHECK CONSTRAINT [FK_PozycjeFakturSprzedazy_FakturySprzedazy]
GO
ALTER TABLE [dbo].[PozycjeFakturSprzedazy]  WITH CHECK ADD  CONSTRAINT [FK_PozycjeFakturSprzedazy_JednostkiMiary] FOREIGN KEY([IdJednostkiMiary])
REFERENCES [dbo].[JednostkiMiary] ([Id])
GO

ALTER TABLE [dbo].[PozycjeFakturSprzedazy] CHECK CONSTRAINT [FK_PozycjeFakturSprzedazy_JednostkiMiary]
GO
ALTER TABLE [dbo].[PozycjeFakturSprzedazy] ADD  DEFAULT ((0)) FOR [CenaJednostkowa]
GO
ALTER TABLE [dbo].[PozycjeFakturSprzedazy] ADD  DEFAULT ((23)) FOR [StawkaVat]
GO
ALTER TABLE [dbo].[PozycjeFakturSprzedazy] ADD  DEFAULT ((0)) FOR [CzyBrutto]