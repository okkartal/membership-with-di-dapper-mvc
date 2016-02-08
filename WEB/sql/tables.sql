USE [Membership]
GO

/****** Object:  Table [dbo].[Member]    Script Date: 8.2.2016 22:13:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Member](
	[Id] [int] IDENTITY(3016020,1) NOT NULL,
	[Email] [varchar](64) NULL,
	[PasswordHash] [varchar](100) NULL,
	[PasswordSalt] [varchar](50) NULL,
	[NickName] [varchar](100) NULL,
	[Name] [varchar](50) NULL,
	[Surname] [varchar](50) NULL,
	 
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
;


