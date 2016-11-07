CREATE TABLE [dbo].[KompensatyPowiazania](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdKompensaty] [int] NOT NULL,
	[IdKontrahenta] [int] NOT NULL,
	[IdFakturySprzedazy] [int] NULL,
	[IdFakturyZakupu] [int] NULL,
	[Kwota] [decimal](16, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[KompensatyPowiazania]  WITH CHECK ADD  CONSTRAINT [FK_KompensatyPowiazania_FakturySprzedazy] FOREIGN KEY([IdFakturySprzedazy], [IdKontrahenta])
REFERENCES [dbo].[FakturySprzedazy] ([Id], [IdKontrahenta])
GO

ALTER TABLE [dbo].[KompensatyPowiazania] CHECK CONSTRAINT [FK_KompensatyPowiazania_FakturySprzedazy]
GO
ALTER TABLE [dbo].[KompensatyPowiazania]  WITH CHECK ADD  CONSTRAINT [FK_KompensatyPowiazania_FakturyZakupu] FOREIGN KEY([IdFakturyZakupu], [IdKontrahenta])
REFERENCES [dbo].[FakturyZakupu] ([Id], [IdKontrahenta])
GO

ALTER TABLE [dbo].[KompensatyPowiazania] CHECK CONSTRAINT [FK_KompensatyPowiazania_FakturyZakupu]
GO
ALTER TABLE [dbo].[KompensatyPowiazania]  WITH CHECK ADD  CONSTRAINT [FK_KompensatyPowiazania_Kompensaty] FOREIGN KEY([IdKompensaty], [IdKontrahenta])
REFERENCES [dbo].[Kompensaty] ([Id], [IdKontrahenta])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[KompensatyPowiazania] CHECK CONSTRAINT [FK_KompensatyPowiazania_Kompensaty]