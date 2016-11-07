CREATE TABLE [dbo].[TypyInwestycji](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KodTypuInwestycji] [nvarchar](30) NOT NULL,
	[NazwaTypuInwestycji] [nvarchar](30) NOT NULL,
	[CzyDomyslny] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[TypyInwestycji] ADD  DEFAULT ((0)) FOR [CzyDomyslny]