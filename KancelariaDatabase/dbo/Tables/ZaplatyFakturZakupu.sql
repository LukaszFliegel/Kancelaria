CREATE TABLE [dbo].[ZaplatyFakturZakupu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdFakturyZakupu] [int] NOT NULL,
	[Kwota] [decimal](16, 2) NOT NULL,
	[DataZaplaty] [date] NOT NULL,
	[Opis] [nvarchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ZaplatyFakturZakupu]  WITH CHECK ADD  CONSTRAINT [FK_FakturyZakupu_ZaplatyFakturZakupu] FOREIGN KEY([IdFakturyZakupu])
REFERENCES [dbo].[FakturyZakupu] ([Id])
GO

ALTER TABLE [dbo].[ZaplatyFakturZakupu] CHECK CONSTRAINT [FK_FakturyZakupu_ZaplatyFakturZakupu]