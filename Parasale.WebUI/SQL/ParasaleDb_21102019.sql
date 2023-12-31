USE [master]
GO
/****** Object:  Database [ParasaleDb]    Script Date: 10/21/2019 7:11:14 PM ******/
CREATE DATABASE [ParasaleDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ParasaleDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\ParasaleDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ParasaleDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\ParasaleDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ParasaleDb] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ParasaleDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ParasaleDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ParasaleDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ParasaleDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ParasaleDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ParasaleDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [ParasaleDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ParasaleDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ParasaleDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ParasaleDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ParasaleDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ParasaleDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ParasaleDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ParasaleDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ParasaleDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ParasaleDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ParasaleDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ParasaleDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ParasaleDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ParasaleDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ParasaleDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ParasaleDb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [ParasaleDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ParasaleDb] SET RECOVERY FULL 
GO
ALTER DATABASE [ParasaleDb] SET  MULTI_USER 
GO
ALTER DATABASE [ParasaleDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ParasaleDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ParasaleDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ParasaleDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ParasaleDb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ParasaleDb', N'ON'
GO
ALTER DATABASE [ParasaleDb] SET QUERY_STORE = OFF
GO
USE [ParasaleDb]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [ParasaleDb]
GO
/****** Object:  User [IIS APPPOOL\ParasaleDemo]    Script Date: 10/21/2019 7:11:14 PM ******/
CREATE USER [IIS APPPOOL\ParasaleDemo] FOR LOGIN [IIS APPPOOL\ParasaleDemo] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\ParasaleDemo]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\ParasaleDemo]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/21/2019 7:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 10/21/2019 7:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 10/21/2019 7:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 10/21/2019 7:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 10/21/2019 7:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 10/21/2019 7:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 10/21/2019 7:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[LastNotifiedOn] [datetime2](7) NOT NULL,
	[LastObjectionCount] [int] NOT NULL,
	[IsManager] [bit] NOT NULL,
	[AssignedManager] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 10/21/2019 7:11:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Objection]    Script Date: 10/21/2019 7:11:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Objection](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InitialObjection] [nvarchar](max) NULL,
	[PitchKeyword] [nvarchar](max) NULL,
	[ValidPitchResponse] [nvarchar](max) NULL,
	[BadPitchResponse] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Objection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectionLogs]    Script Date: 10/21/2019 7:11:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectionLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsCompleted] [bit] NOT NULL,
	[ObjectionId] [int] NULL,
	[AppUserId] [nvarchar](450) NULL,
	[ObjectionTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ObjectionLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectionNotification]    Script Date: 10/21/2019 7:11:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectionNotification](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PushedByUserId] [nvarchar](max) NULL,
	[PushedToUserId] [nvarchar](max) NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[IsCleared] [bit] NOT NULL,
	[ObjectionId] [int] NULL,
 CONSTRAINT [PK_ObjectionNotification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SpeechToText]    Script Date: 10/21/2019 7:11:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpeechToText](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](max) NULL,
	[Recieved] [bit] NOT NULL,
	[AppUserId] [nvarchar](450) NULL,
 CONSTRAINT [PK_SpeechToText] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190904132207_initial', N'2.1.8-servicing-32085')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190905070113_speechtotextTable', N'2.1.8-servicing-32085')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190905073810_objectionsTable', N'2.1.8-servicing-32085')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190905073852_speechtotextTableUpdate', N'2.1.8-servicing-32085')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190905074018_speechtotextTableUpdate2', N'2.1.8-servicing-32085')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190905090129_objectionlogTable', N'2.1.8-servicing-32085')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190905101648_appuserchanged', N'2.1.8-servicing-32085')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190905105149_appuserObjectionCount', N'2.1.8-servicing-32085')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190905122746_objectiontimeadded', N'2.1.8-servicing-32085')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190923142850_userupdated', N'2.1.11-servicing-32099')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190923145724_usermanagerupdate', N'2.1.11-servicing-32099')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190923145745_usermanagerupdate2', N'2.1.11-servicing-32099')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190924101603_objectionsupdated', N'2.1.11-servicing-32099')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190924121858_objectionnotification', N'2.1.11-servicing-32099')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190924140931_ObjectionNotificationupdate', N'2.1.11-servicing-32099')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20190925071525_initial2', N'2.1.11-servicing-32099')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description], [CreatedDate]) VALUES (N'd53897f2-6ae3-4e27-9c19-c17b8a8ea30c', N'Admin', N'ADMIN', N'bc6f5db3-9ab1-44f9-b6d0-3b9dc10eb995', N'Role for managing Application', CAST(N'2019-09-24T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description], [CreatedDate]) VALUES (N'dfef417f-65fe-477d-b957-153de8aebbc4', N'User', N'USER', N'7d620dbb-c55d-4e56-8c7f-80851f775efa', N'Role for Application User', CAST(N'2019-09-24T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description], [CreatedDate]) VALUES (N'ffac8fa0-20c8-42f7-bdac-4b8be2c0b4aa', N'Manager', N'MANAGER', N'4c36b23d-a4ef-419c-a75c-9be3b48f2cb9', N'Role for Manage Users', CAST(N'2019-09-24T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'8917f82e-c4d9-48ab-836b-6e758b9a6e6f', N'd53897f2-6ae3-4e27-9c19-c17b8a8ea30c')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'665d5350-cc9b-4d4f-9b70-26fbab9a6b53', N'dfef417f-65fe-477d-b957-153de8aebbc4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6a3834af-7ba7-49c1-99ab-3064833ea2f3', N'dfef417f-65fe-477d-b957-153de8aebbc4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'898006f2-817c-4744-94d5-2c9edc07802f', N'dfef417f-65fe-477d-b957-153de8aebbc4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'8fd69ba2-4baa-4ca9-8011-28895fa34775', N'dfef417f-65fe-477d-b957-153de8aebbc4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'665d5350-cc9b-4d4f-9b70-26fbab9a6b53', N'ffac8fa0-20c8-42f7-bdac-4b8be2c0b4aa')
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Name], [LastNotifiedOn], [LastObjectionCount], [IsManager], [AssignedManager]) VALUES (N'665d5350-cc9b-4d4f-9b70-26fbab9a6b53', N'swajrizwan', N'SWAJRIZWAN', N'swajrizwan@hotmail.com', N'SWAJRIZWAN@HOTMAIL.COM', 0, N'AQAAAAEAACcQAAAAENbnU0NFhJQPl2z2uDbX5evMW+fsQVV2HaC0VNYmFv1zGZ3CZUyw1Bc5kUvYzVoS8w==', N'ANGD7HD7LE3QCNBR7NSVDCVD24T35RUL', N'b4f43320-fe56-4e13-b594-94becb9058a7', NULL, 0, 0, NULL, 1, 0, N'Swaj Rizwan', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 0, 1, NULL)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Name], [LastNotifiedOn], [LastObjectionCount], [IsManager], [AssignedManager]) VALUES (N'6a3834af-7ba7-49c1-99ab-3064833ea2f3', N'testuser12', N'TESTUSER12', N'TestUser12@gmail.com', N'TESTUSER12@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEOBzyeWcM86WlC+JNb+EaPLyyDtpcQy2CfTQsAIwvPzE8MBflyKPTPfi1GNK7rw3vA==', N'RW7L5VNJ7EVVKDWCOBQLR5RGVGTMP3SF', N'dbf34eda-6019-477d-b92f-9d86a191fc0a', NULL, 0, 0, NULL, 1, 0, N'Test User', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Name], [LastNotifiedOn], [LastObjectionCount], [IsManager], [AssignedManager]) VALUES (N'8917f82e-c4d9-48ab-836b-6e758b9a6e6f', N'admin', N'ADMIN', N'admin@superuser.com', N'ADMIN@SUPERUSER.COM', 0, N'AQAAAAEAACcQAAAAEC5k+wwMrn5r9fvbBLyBWiAlzen3LrsoJO0tIqdAauYhbcPToNBrGbwRESkkPwTMYA==', N'AFLAX5CSRCMCRBVYBTSDK337PCZHKJ2X', N'4ecad1ac-1958-4187-b3cb-d4d5af800621', NULL, 0, 0, NULL, 1, 0, N'Admin', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 0, 0, NULL)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Name], [LastNotifiedOn], [LastObjectionCount], [IsManager], [AssignedManager]) VALUES (N'898006f2-817c-4744-94d5-2c9edc07802f', N'testuser', N'TESTUSER', N'TestUser@gmail.com', N'TESTUSER@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEKSVguo10QbKQJVMT2kE5RB5Q9cmhl0pjVZQIN9+R8vd40wada+7ln6FXzmyt/471g==', N'ZRVX4PBBCA5C4Z422YQUTIHADC77MBVY', N'0dad1004-6983-4713-b757-e2a586496142', NULL, 0, 0, NULL, 1, 0, N'Test User', CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 0, 0, N'665d5350-cc9b-4d4f-9b70-26fbab9a6b53')
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [Name], [LastNotifiedOn], [LastObjectionCount], [IsManager], [AssignedManager]) VALUES (N'8fd69ba2-4baa-4ca9-8011-28895fa34775', N'saimrizwan', N'SAIMRIZWAN', N'saimrizwan135@gmail.com', N'SAIMRIZWAN135@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEGmP7uCpnhF5l5rTKmHUzEooyT6VJ96fnGdXjH0SN1eH1+ulI6bDtVOa13WcP2WTjw==', N'TPAECEQBQZ7XZ6UEVRKBSQCQRZTGQ4FG', N'34ba8a96-28d9-4ebb-bc28-81686eed9abd', NULL, 0, 0, NULL, 1, 0, N'Saim Rizwan', CAST(N'2019-09-30T00:00:00.0000000' AS DateTime2), 2, 0, N'665d5350-cc9b-4d4f-9b70-26fbab9a6b53')
SET IDENTITY_INSERT [dbo].[Objection] ON 

INSERT [dbo].[Objection] ([Id], [InitialObjection], [PitchKeyword], [ValidPitchResponse], [BadPitchResponse], [UserId]) VALUES (28, N'How are you', N'fine', N'good job', N'try again', N'665d5350-cc9b-4d4f-9b70-26fbab9a6b53')
INSERT [dbo].[Objection] ([Id], [InitialObjection], [PitchKeyword], [ValidPitchResponse], [BadPitchResponse], [UserId]) VALUES (29, N'What is your favourite Sport', N'cricket', N'good job', N'try again', NULL)
INSERT [dbo].[Objection] ([Id], [InitialObjection], [PitchKeyword], [ValidPitchResponse], [BadPitchResponse], [UserId]) VALUES (30, N'What is your favourite Sport', N'cricket, football', N'good job', N'try again', N'665d5350-cc9b-4d4f-9b70-26fbab9a6b53')
SET IDENTITY_INSERT [dbo].[Objection] OFF
SET IDENTITY_INSERT [dbo].[ObjectionLogs] ON 

INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (116, 0, 28, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-26T19:41:50.8067741' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (117, 0, 28, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T14:58:09.1212405' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (118, 0, 30, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T15:03:19.3252286' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (119, 1, 30, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T15:05:41.3228858' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (120, 1, 28, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T15:30:55.1400897' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (121, 1, 30, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T16:32:11.1928449' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (122, 0, 30, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T16:33:07.9859239' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (123, 0, 28, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T16:34:15.4915898' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (124, 0, 30, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T16:38:03.0532416' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (125, 1, 30, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T16:40:57.4138812' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (126, 0, 30, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T16:41:27.9260540' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (127, 0, 30, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T16:42:58.0152325' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (128, 0, 30, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T16:46:59.3611725' AS DateTime2))
INSERT [dbo].[ObjectionLogs] ([Id], [IsCompleted], [ObjectionId], [AppUserId], [ObjectionTime]) VALUES (129, 0, 28, N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-30T16:48:02.6333027' AS DateTime2))
SET IDENTITY_INSERT [dbo].[ObjectionLogs] OFF
SET IDENTITY_INSERT [dbo].[ObjectionNotification] ON 

INSERT [dbo].[ObjectionNotification] ([Id], [PushedByUserId], [PushedToUserId], [CreatedTime], [IsCleared], [ObjectionId]) VALUES (29, N'665d5350-cc9b-4d4f-9b70-26fbab9a6b53', N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-26T19:42:13.2350796' AS DateTime2), 0, 28)
INSERT [dbo].[ObjectionNotification] ([Id], [PushedByUserId], [PushedToUserId], [CreatedTime], [IsCleared], [ObjectionId]) VALUES (30, N'665d5350-cc9b-4d4f-9b70-26fbab9a6b53', N'898006f2-817c-4744-94d5-2c9edc07802f', CAST(N'2019-09-26T19:42:46.8982541' AS DateTime2), 0, 30)
INSERT [dbo].[ObjectionNotification] ([Id], [PushedByUserId], [PushedToUserId], [CreatedTime], [IsCleared], [ObjectionId]) VALUES (31, N'665d5350-cc9b-4d4f-9b70-26fbab9a6b53', N'8fd69ba2-4baa-4ca9-8011-28895fa34775', CAST(N'2019-09-26T19:42:46.9023302' AS DateTime2), 0, 30)
SET IDENTITY_INSERT [dbo].[ObjectionNotification] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Objection_UserId]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_Objection_UserId] ON [dbo].[Objection]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ObjectionLogs_AppUserId]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectionLogs_AppUserId] ON [dbo].[ObjectionLogs]
(
	[AppUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectionLogs_ObjectionId]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectionLogs_ObjectionId] ON [dbo].[ObjectionLogs]
(
	[ObjectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ObjectionNotification_ObjectionId]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_ObjectionNotification_ObjectionId] ON [dbo].[ObjectionNotification]
(
	[ObjectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_SpeechToText_AppUserId]    Script Date: 10/21/2019 7:11:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_SpeechToText_AppUserId] ON [dbo].[SpeechToText]
(
	[AppUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [LastNotifiedOn]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [LastObjectionCount]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [IsManager]
GO
ALTER TABLE [dbo].[ObjectionLogs] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [ObjectionTime]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Objection]  WITH CHECK ADD  CONSTRAINT [FK_Objection_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Objection] CHECK CONSTRAINT [FK_Objection_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[ObjectionLogs]  WITH CHECK ADD  CONSTRAINT [FK_ObjectionLogs_AspNetUsers_AppUserId] FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ObjectionLogs] CHECK CONSTRAINT [FK_ObjectionLogs_AspNetUsers_AppUserId]
GO
ALTER TABLE [dbo].[ObjectionLogs]  WITH CHECK ADD  CONSTRAINT [FK_ObjectionLogs_Objection_ObjectionId] FOREIGN KEY([ObjectionId])
REFERENCES [dbo].[Objection] ([Id])
GO
ALTER TABLE [dbo].[ObjectionLogs] CHECK CONSTRAINT [FK_ObjectionLogs_Objection_ObjectionId]
GO
ALTER TABLE [dbo].[ObjectionNotification]  WITH CHECK ADD  CONSTRAINT [FK_ObjectionNotification_Objection_ObjectionId] FOREIGN KEY([ObjectionId])
REFERENCES [dbo].[Objection] ([Id])
GO
ALTER TABLE [dbo].[ObjectionNotification] CHECK CONSTRAINT [FK_ObjectionNotification_Objection_ObjectionId]
GO
ALTER TABLE [dbo].[SpeechToText]  WITH CHECK ADD  CONSTRAINT [FK_SpeechToText_AspNetUsers_AppUserId] FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[SpeechToText] CHECK CONSTRAINT [FK_SpeechToText_AspNetUsers_AppUserId]
GO
USE [master]
GO
ALTER DATABASE [ParasaleDb] SET  READ_WRITE 
GO
