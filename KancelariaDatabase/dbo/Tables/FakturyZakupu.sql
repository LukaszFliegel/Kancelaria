CREATE TABLE [dbo].[FakturyZakupu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdFirmy] [int] NOT NULL,
	[IdRoku] [int] NOT NULL,
	[NumerFaktury] [nvarchar](50) NOT NULL,
	[WlasnyNumerFaktury] [nvarchar](50) NULL,
	[DataFaktury] [date] NOT NULL,
	[TerminPlatnosci] [date] NOT NULL,
	[Opis] [nvarchar](1000) NULL,
	[IdKontrahenta] [int] NOT NULL,
	[IdSposobuPlatnosci] [int] NOT NULL,
	[CzyZaliczka] [bit] NOT NULL,
	[DataZakupu] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_FakturyZakupu_UQ1] UNIQUE NONCLUSTERED 
(
	[Id] ASC,
	[IdKontrahenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [FakturyZakupuNumerFakturyUQ] UNIQUE NONCLUSTERED 
(
	[IdFirmy] ASC,
	[IdRoku] ASC,
	[NumerFaktury] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FakturyZakupu]  WITH CHECK ADD  CONSTRAINT [FK_FakturyZakupu_Firmy] FOREIGN KEY([IdFirmy])
REFERENCES [dbo].[Firmy] ([Id])
GO

ALTER TABLE [dbo].[FakturyZakupu] CHECK CONSTRAINT [FK_FakturyZakupu_Firmy]
GO
ALTER TABLE [dbo].[FakturyZakupu]  WITH CHECK ADD  CONSTRAINT [FK_FakturyZakupu_Kontrahenci] FOREIGN KEY([IdKontrahenta])
REFERENCES [dbo].[Kontrahenci] ([Id])
GO

ALTER TABLE [dbo].[FakturyZakupu] CHECK CONSTRAINT [FK_FakturyZakupu_Kontrahenci]
GO
ALTER TABLE [dbo].[FakturyZakupu]  WITH CHECK ADD  CONSTRAINT [FK_FakturyZakupu_Lata] FOREIGN KEY([IdRoku])
REFERENCES [dbo].[LataObrotowe] ([Id])
GO

ALTER TABLE [dbo].[FakturyZakupu] CHECK CONSTRAINT [FK_FakturyZakupu_Lata]
GO
ALTER TABLE [dbo].[FakturyZakupu]  WITH CHECK ADD  CONSTRAINT [FK_FakturyZakupu_SposobyPlatnosci] FOREIGN KEY([IdSposobuPlatnosci])
REFERENCES [dbo].[SposobyPlatnosci] ([Id])
GO

ALTER TABLE [dbo].[FakturyZakupu] CHECK CONSTRAINT [FK_FakturyZakupu_SposobyPlatnosci]
GO
ALTER TABLE [dbo].[FakturyZakupu] ADD  DEFAULT ((0)) FOR [CzyZaliczka]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Numer faktury' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FakturyZakupu', @level2type=N'COLUMN',@level2name=N'NumerFaktury'