CREATE TABLE [dbo].[PozycjeFakturZakupu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NumerPozycji] [int] NOT NULL,
	[IdFaktury] [int] NOT NULL,
	[CenaJednostkowa] [decimal](16, 2) NOT NULL,
	[StawkaVat] [int] NOT NULL,
	[IdInwestycji] [int] NOT NULL,
	[Opis] [nvarchar](1000) NULL,
	[Ilosc] [int] NOT NULL,
	[IdJednostkiMiary] [int] NOT NULL,
	[CzyBrutto] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PozycjeFakturZakupu]  WITH CHECK ADD  CONSTRAINT [FK_PozycjeFakturZakupu_FakturyZakupu] FOREIGN KEY([IdFaktury])
REFERENCES [dbo].[FakturyZakupu] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[PozycjeFakturZakupu] CHECK CONSTRAINT [FK_PozycjeFakturZakupu_FakturyZakupu]
GO
ALTER TABLE [dbo].[PozycjeFakturZakupu]  WITH CHECK ADD  CONSTRAINT [FK_PozycjeFakturZakupu_Inwestycje] FOREIGN KEY([IdInwestycji])
REFERENCES [dbo].[Inwestycje] ([Id])
GO

ALTER TABLE [dbo].[PozycjeFakturZakupu] CHECK CONSTRAINT [FK_PozycjeFakturZakupu_Inwestycje]
GO
ALTER TABLE [dbo].[PozycjeFakturZakupu]  WITH CHECK ADD  CONSTRAINT [FK_PozycjeFakturZakupu_JednostkiMiary] FOREIGN KEY([IdJednostkiMiary])
REFERENCES [dbo].[JednostkiMiary] ([Id])
GO

ALTER TABLE [dbo].[PozycjeFakturZakupu] CHECK CONSTRAINT [FK_PozycjeFakturZakupu_JednostkiMiary]
GO
ALTER TABLE [dbo].[PozycjeFakturZakupu] ADD  DEFAULT ((0)) FOR [CenaJednostkowa]
GO
ALTER TABLE [dbo].[PozycjeFakturZakupu] ADD  DEFAULT ((23)) FOR [StawkaVat]
GO
ALTER TABLE [dbo].[PozycjeFakturZakupu] ADD  DEFAULT ((0)) FOR [CzyBrutto]