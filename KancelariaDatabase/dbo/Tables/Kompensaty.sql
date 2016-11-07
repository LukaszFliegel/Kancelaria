CREATE TABLE [dbo].[Kompensaty](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdFirmy] [int] NOT NULL,
	[IdRoku] [int] NOT NULL,
	[NumerKompensaty] [nvarchar](50) NOT NULL,
	[IdKontrahenta] [int] NOT NULL,
	[DataKompensaty] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Kompensaty_UQ1] UNIQUE NONCLUSTERED 
(
	[Id] ASC,
	[IdKontrahenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Kompensaty]  WITH CHECK ADD  CONSTRAINT [FK_Kompensaty_Firma] FOREIGN KEY([IdFirmy])
REFERENCES [dbo].[Firmy] ([Id])
GO

ALTER TABLE [dbo].[Kompensaty] CHECK CONSTRAINT [FK_Kompensaty_Firma]
GO
ALTER TABLE [dbo].[Kompensaty]  WITH CHECK ADD  CONSTRAINT [FK_Kompensaty_Kontrahenci] FOREIGN KEY([IdKontrahenta])
REFERENCES [dbo].[Kontrahenci] ([Id])
GO

ALTER TABLE [dbo].[Kompensaty] CHECK CONSTRAINT [FK_Kompensaty_Kontrahenci]
GO
ALTER TABLE [dbo].[Kompensaty]  WITH CHECK ADD  CONSTRAINT [FK_Kompensaty_LataObrotowe] FOREIGN KEY([IdRoku])
REFERENCES [dbo].[LataObrotowe] ([Id])
GO

ALTER TABLE [dbo].[Kompensaty] CHECK CONSTRAINT [FK_Kompensaty_LataObrotowe]