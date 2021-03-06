USE [ActiveQuestion]
GO
/****** Object:  Table [dbo].[Common]    Script Date: 2021/12/13 下午 09:09:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Common](
	[FAQID] [int] IDENTITY(1,1) NOT NULL,
	[Count] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Text] [nvarchar](500) NOT NULL,
	[SelectionType] [int] NOT NULL,
	[IsMust] [bit] NOT NULL,
	[Option] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_common] PRIMARY KEY CLUSTERED 
(
	[FAQID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Q_d1]    Script Date: 2021/12/13 下午 09:09:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Q_d1](
	[D_Guid] [uniqueidentifier] NOT NULL,
	[M_Guid] [uniqueidentifier] NOT NULL,
	[D_title] [nvarchar](50) NOT NULL,
	[D_mustKeyin] [bit] NOT NULL,
	[SelectionType] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[Selection] [nvarchar](100) NULL,
 CONSTRAINT [PK_Q_d1] PRIMARY KEY CLUSTERED 
(
	[D_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Q_M]    Script Date: 2021/12/13 下午 09:09:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Q_M](
	[M_id] [int] IDENTITY(1,1) NOT NULL,
	[M_Guid] [uniqueidentifier] NOT NULL,
	[M_title] [nvarchar](150) NOT NULL,
	[M_state] [int] NOT NULL,
	[start_time] [datetime] NOT NULL,
	[end_time] [datetime] NULL,
	[Count] [int] NULL,
	[Summary] [nvarchar](250) NULL,
 CONSTRAINT [PK_Q_M] PRIMARY KEY CLUSTERED 
(
	[M_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Static]    Script Date: 2021/12/13 下午 09:09:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Static](
	[StaticID] [int] IDENTITY(1,1) NOT NULL,
	[M_Guid] [uniqueidentifier] NOT NULL,
	[D_Guid] [uniqueidentifier] NOT NULL,
	[OptionText] [nvarchar](1000) NULL,
	[Count] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Replay]    Script Date: 2021/12/13 下午 09:09:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Replay](
	[UserReplay_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_Guid] [uniqueidentifier] NOT NULL,
	[D_Guid] [uniqueidentifier] NOT NULL,
	[AnswerText] [nvarchar](1000) NULL,
 CONSTRAINT [PK_User_Replay] PRIMARY KEY CLUSTERED 
(
	[UserReplay_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 2021/12/13 下午 09:09:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfo](
	[User_Guid] [uniqueidentifier] NOT NULL,
	[M_Guid] [uniqueidentifier] NULL,
	[Account] [varchar](50) NULL,
	[Pwd] [varchar](50) NULL,
	[Level] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[Phone] [varchar](20) NULL,
	[Email] [nvarchar](50) NULL,
	[Age] [int] NULL,
	[CreateDate] [date] NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[User_Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
