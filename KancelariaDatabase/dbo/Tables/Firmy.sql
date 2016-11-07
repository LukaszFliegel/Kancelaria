CREATE TABLE [dbo].[Firmy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NazwaSkrocona] [nvarchar](50) NOT NULL,
	[NazwaPelna] [nvarchar](300) NOT NULL,
	[Ulica] [nvarchar](100) NULL,
	[Miejscowosc] [nvarchar](100) NULL,
	[NumerDomu] [nvarchar](5) NULL,
	[NumerLokalu] [nvarchar](5) NULL,
	[Panstwo] [nvarchar](30) NULL,
	[KodPocztowy] [nvarchar](10) NULL,
	[NIP] [nvarchar](20) NULL,
	[Regon] [bigint] NULL,
	[KRS] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]