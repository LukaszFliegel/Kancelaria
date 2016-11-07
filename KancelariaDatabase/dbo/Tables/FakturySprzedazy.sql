CREATE TABLE [dbo].[FakturySprzedazy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdFirmy] [int] NOT NULL,
	[IdRoku] [int] NOT NULL,
	[NumerFaktury] [nvarchar](50) NOT NULL,
	[DataFaktury] [date] NOT NULL,
	[TerminPlatnosci] [date] NOT NULL,
	[Opis] [nvarchar](1000) NULL,
	[IdKontrahenta] [int] NOT NULL,
	[CzyZaliczka] [bit] NOT NULL,
	[IdInwestycji] [int] NULL,
	[IdKontaBankowegoFirmy] [int] NOT NULL,
	[IdSposobuPlatnosci] [int] NOT NULL,
	[DataSprzedazy] [date] NULL,
	[CzyUmowa] [bit] NOT NULL,
	[IdFakturyKorygujacej] [int] NULL,
	[Uwagi] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_FakturySprzedazy_UQ1] UNIQUE NONCLUSTERED 
(
	[Id] ASC,
	[IdKontrahenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FakturySprzedazy]  WITH CHECK ADD  CONSTRAINT [FK_FakturySprzedazy_FakturySprzedazy] FOREIGN KEY([IdFakturyKorygujacej])
REFERENCES [dbo].[FakturySprzedazy] ([Id])
GO

ALTER TABLE [dbo].[FakturySprzedazy] CHECK CONSTRAINT [FK_FakturySprzedazy_FakturySprzedazy]
GO
ALTER TABLE [dbo].[FakturySprzedazy]  WITH CHECK ADD  CONSTRAINT [FK_FakturySprzedazy_Firmy] FOREIGN KEY([IdFirmy])
REFERENCES [dbo].[Firmy] ([Id])
GO

ALTER TABLE [dbo].[FakturySprzedazy] CHECK CONSTRAINT [FK_FakturySprzedazy_Firmy]
GO
ALTER TABLE [dbo].[FakturySprzedazy]  WITH CHECK ADD  CONSTRAINT [FK_FakturySprzedazy_Inwestycje] FOREIGN KEY([IdInwestycji])
REFERENCES [dbo].[Inwestycje] ([Id])
GO

ALTER TABLE [dbo].[FakturySprzedazy] CHECK CONSTRAINT [FK_FakturySprzedazy_Inwestycje]
GO
ALTER TABLE [dbo].[FakturySprzedazy]  WITH CHECK ADD  CONSTRAINT [FK_FakturySprzedazy_KontaBankoweFirmy] FOREIGN KEY([IdKontaBankowegoFirmy], [IdFirmy])
REFERENCES [dbo].[KontaBankowe] ([Id], [IdFirmy])
GO

ALTER TABLE [dbo].[FakturySprzedazy] CHECK CONSTRAINT [FK_FakturySprzedazy_KontaBankoweFirmy]
GO
ALTER TABLE [dbo].[FakturySprzedazy]  WITH CHECK ADD  CONSTRAINT [FK_FakturySprzedazy_Kontrahenci] FOREIGN KEY([IdKontrahenta])
REFERENCES [dbo].[Kontrahenci] ([Id])
GO

ALTER TABLE [dbo].[FakturySprzedazy] CHECK CONSTRAINT [FK_FakturySprzedazy_Kontrahenci]
GO
ALTER TABLE [dbo].[FakturySprzedazy]  WITH CHECK ADD  CONSTRAINT [FK_FakturySprzedazy_LataObrotowe] FOREIGN KEY([IdRoku])
REFERENCES [dbo].[LataObrotowe] ([Id])
GO

ALTER TABLE [dbo].[FakturySprzedazy] CHECK CONSTRAINT [FK_FakturySprzedazy_LataObrotowe]
GO
ALTER TABLE [dbo].[FakturySprzedazy]  WITH CHECK ADD  CONSTRAINT [FK_FakturySprzedazy_SposobyPlatnosci] FOREIGN KEY([IdSposobuPlatnosci])
REFERENCES [dbo].[SposobyPlatnosci] ([Id])
GO

ALTER TABLE [dbo].[FakturySprzedazy] CHECK CONSTRAINT [FK_FakturySprzedazy_SposobyPlatnosci]
GO
ALTER TABLE [dbo].[FakturySprzedazy] ADD  DEFAULT ((0)) FOR [CzyZaliczka]