/****** Object:  Table [dbo].[Room]    Script Date: 05/10/2025 12:55:52 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[RoomId] [uniqueidentifier] NOT NULL,
	[RoomName] [nvarchar](100) NOT NULL,
	[RoomDescription] [nvarchar](max) NOT NULL,
	[RoomTypeId] [uniqueidentifier] NOT NULL,
	[RoomCode] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoomSchedule]    Script Date: 05/10/2025 12:55:52 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoomSchedule](
	[RoomScheduleId] [uniqueidentifier] NOT NULL,
	[ProfessorUserId] [uniqueidentifier] NOT NULL,
	[RoomId] [uniqueidentifier] NOT NULL,
	[DateOfUse] [date] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Note] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoomType]    Script Date: 05/10/2025 12:55:52 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoomType](
	[RoomTypeId] [uniqueidentifier] NOT NULL,
	[RoomTypeName] [nvarchar](100) NOT NULL,
	[RoomTypeDescription] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 05/10/2025 12:55:52 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserRoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[RoleDescription] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 05/10/2025 12:55:52 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[UserRoleId] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Room] ([RoomId], [RoomName], [RoomDescription], [RoomTypeId], [RoomCode], [CreatedDate]) VALUES (N'fc157fb7-d11a-48cd-a5c4-282010bb9bb1', N'Room 101', N'Room 101', N'58cf9eee-39aa-46c3-9c06-bcffaae4cff7', N'101', CAST(N'2025-10-04T17:48:02.823' AS DateTime))
GO
INSERT [dbo].[RoomSchedule] ([RoomScheduleId], [ProfessorUserId], [RoomId], [DateOfUse], [StartTime], [EndTime], [CreatedDate], [Note]) VALUES (N'de34acbd-5b2a-446d-bc14-078488f5076e', N'75148f69-e6d4-46ce-8e2c-d17b47e208d9', N'fc157fb7-d11a-48cd-a5c4-282010bb9bb1', CAST(N'2025-10-06' AS Date), CAST(N'13:52:00' AS Time), CAST(N'14:52:00' AS Time), CAST(N'2025-10-04T23:52:50.440' AS DateTime), NULL)
GO
INSERT [dbo].[RoomSchedule] ([RoomScheduleId], [ProfessorUserId], [RoomId], [DateOfUse], [StartTime], [EndTime], [CreatedDate], [Note]) VALUES (N'61c96a7e-a197-4b71-9e59-7ae65b76b6be', N'75148f69-e6d4-46ce-8e2c-d17b47e208d9', N'fc157fb7-d11a-48cd-a5c4-282010bb9bb1', CAST(N'2025-10-06' AS Date), CAST(N'15:00:00' AS Time), CAST(N'16:00:00' AS Time), CAST(N'2025-10-05T00:45:13.313' AS DateTime), NULL)
GO
INSERT [dbo].[RoomType] ([RoomTypeId], [RoomTypeName], [RoomTypeDescription], [CreatedDate]) VALUES (N'58cf9eee-39aa-46c3-9c06-bcffaae4cff7', N'Classroom', N'Normal Classroom', CAST(N'2025-10-04T17:47:22.937' AS DateTime))
GO
INSERT [dbo].[UserRoles] ([UserRoleId], [RoleName], [RoleDescription], [CreatedDate]) VALUES (N'ba0ad894-7c4c-45be-83fb-72307b2f34a0', N'Checker', N'Checker', CAST(N'2025-10-04T10:41:31.523' AS DateTime))
GO
INSERT [dbo].[UserRoles] ([UserRoleId], [RoleName], [RoleDescription], [CreatedDate]) VALUES (N'7a8ceb9d-7c7d-4f77-a063-de54ba4f1d9f', N'Professor', N'Professor', CAST(N'2025-10-04T10:41:31.523' AS DateTime))
GO
INSERT [dbo].[UserRoles] ([UserRoleId], [RoleName], [RoleDescription], [CreatedDate]) VALUES (N'f504246a-1206-4a78-9d3f-ff0b0346d308', N'Admin', N'Administrator', CAST(N'2025-10-03T20:54:10.630' AS DateTime))
GO
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [UserName], [Password], [UserRoleId], [CreatedDate], [Email]) VALUES (N'ca5818fd-be56-481a-a870-a9343672bcc9', N'System', N'Administrator', N'admin', N'0192023A7BBD73250516F069DF18B500', N'f504246a-1206-4a78-9d3f-ff0b0346d308', CAST(N'2025-10-03T21:45:18.067' AS DateTime), N'classroommonitoring2@gmail.com')
GO
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [UserName], [Password], [UserRoleId], [CreatedDate], [Email]) VALUES (N'75148f69-e6d4-46ce-8e2c-d17b47e208d9', N'Juan', N'Dela Cruz', N'test', N'098F6BCD4621D373CADE4E832627B4F6', N'7a8ceb9d-7c7d-4f77-a063-de54ba4f1d9f', CAST(N'2025-10-04T23:37:56.700' AS DateTime), N'jom72056@gmail.com')
GO
ALTER TABLE [dbo].[Room] ADD  DEFAULT (newid()) FOR [RoomId]
GO
ALTER TABLE [dbo].[Room] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RoomSchedule] ADD  DEFAULT (newid()) FOR [RoomScheduleId]
GO
ALTER TABLE [dbo].[RoomSchedule] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RoomType] ADD  DEFAULT (newid()) FOR [RoomTypeId]
GO
ALTER TABLE [dbo].[RoomType] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT (newid()) FOR [UserRoleId]
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [UserId]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK_Room_RoomTypeId] FOREIGN KEY([RoomTypeId])
REFERENCES [dbo].[RoomType] ([RoomTypeId])
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [FK_Room_RoomTypeId]
GO
ALTER TABLE [dbo].[RoomSchedule]  WITH CHECK ADD  CONSTRAINT [FK_RoomSchedule_ProfessorUserId] FOREIGN KEY([ProfessorUserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[RoomSchedule] CHECK CONSTRAINT [FK_RoomSchedule_ProfessorUserId]
GO
ALTER TABLE [dbo].[RoomSchedule]  WITH CHECK ADD  CONSTRAINT [FK_RoomShcedule_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([RoomId])
GO
ALTER TABLE [dbo].[RoomSchedule] CHECK CONSTRAINT [FK_RoomShcedule_RoomId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRoleId] FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UserRoles] ([UserRoleId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserRoleId]
GO

