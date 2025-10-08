
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendance](
	[AttendanceId] [uniqueidentifier] NOT NULL,
	[ProfessorId] [uniqueidentifier] NOT NULL,
	[RoomScheduleId] [uniqueidentifier] NULL,
	[IsCorrectRoom] [bit] NOT NULL,
	[RoomReassignmentId] [uniqueidentifier] NULL,
	[DateOfUse] [date] NULL,
	[StartTime] [time](7) NULL,
	[EndTime] [time](7) NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AttendanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 08/10/2025 1:11:41 am ******/
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
/****** Object:  Table [dbo].[RoomSchedule]    Script Date: 08/10/2025 1:11:41 am ******/
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
	[SubjectId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoomType]    Script Date: 08/10/2025 1:11:41 am ******/
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
/****** Object:  Table [dbo].[Subject]    Script Date: 08/10/2025 1:11:41 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[SubjectId] [uniqueidentifier] NOT NULL,
	[SubjectName] [nvarchar](50) NOT NULL,
	[SubjectDescription] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SubjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserFingerprints]    Script Date: 08/10/2025 1:11:41 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserFingerprints](
	[UserFingerprintId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[PositionNumber] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_UserFingerprint_UserFingerprintId] PRIMARY KEY CLUSTERED 
(
	[UserFingerprintId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 08/10/2025 1:11:41 am ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 08/10/2025 1:11:41 am ******/
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
INSERT [dbo].[Attendance] ([AttendanceId], [ProfessorId], [RoomScheduleId], [IsCorrectRoom], [RoomReassignmentId], [DateOfUse], [StartTime], [EndTime], [CreatedDate]) VALUES (N'521e4c55-ee6f-40a6-950d-205d772d2bd8', N'06ed4b48-6565-4698-9069-4b0ae9d189a3', NULL, 0, NULL, CAST(N'2025-10-08' AS Date), CAST(N'01:03:01.5103843' AS Time), NULL, CAST(N'2025-10-08T01:03:01.530' AS DateTime))
GO
INSERT [dbo].[Attendance] ([AttendanceId], [ProfessorId], [RoomScheduleId], [IsCorrectRoom], [RoomReassignmentId], [DateOfUse], [StartTime], [EndTime], [CreatedDate]) VALUES (N'45c1a4ef-e042-476f-91d5-f40c29cae2e8', N'06ed4b48-6565-4698-9069-4b0ae9d189a3', NULL, 0, NULL, CAST(N'2025-10-08' AS Date), CAST(N'01:01:14.6534746' AS Time), NULL, CAST(N'2025-10-08T01:01:14.817' AS DateTime))
GO
INSERT [dbo].[Room] ([RoomId], [RoomName], [RoomDescription], [RoomTypeId], [RoomCode], [CreatedDate]) VALUES (N'fc157fb7-d11a-48cd-a5c4-282010bb9bb1', N'Room 101', N'Room 101', N'58cf9eee-39aa-46c3-9c06-bcffaae4cff7', N'101', CAST(N'2025-10-04T17:48:02.823' AS DateTime))
GO
INSERT [dbo].[RoomSchedule] ([RoomScheduleId], [ProfessorUserId], [RoomId], [DateOfUse], [StartTime], [EndTime], [CreatedDate], [Note], [SubjectId]) VALUES (N'a969c564-5815-46cb-b389-36f302559429', N'75148f69-e6d4-46ce-8e2c-d17b47e208d9', N'fc157fb7-d11a-48cd-a5c4-282010bb9bb1', CAST(N'2025-10-08' AS Date), CAST(N'12:00:00' AS Time), CAST(N'15:00:00' AS Time), CAST(N'2025-10-07T23:00:02.167' AS DateTime), NULL, N'5e1379a8-ea07-4d52-b713-f8a66d4ee1d1')
GO
INSERT [dbo].[RoomType] ([RoomTypeId], [RoomTypeName], [RoomTypeDescription], [CreatedDate]) VALUES (N'58cf9eee-39aa-46c3-9c06-bcffaae4cff7', N'Classroom', N'Normal Classroom', CAST(N'2025-10-04T17:47:22.937' AS DateTime))
GO
INSERT [dbo].[Subject] ([SubjectId], [SubjectName], [SubjectDescription], [CreatedDate]) VALUES (N'5e1379a8-ea07-4d52-b713-f8a66d4ee1d1', N'MATH1 Algebra', N'Algebra', CAST(N'2025-10-07T22:59:17.823' AS DateTime))
GO
INSERT [dbo].[UserFingerprints] ([UserFingerprintId], [UserId], [PositionNumber], [CreatedDate]) VALUES (N'87cafc15-9400-4d5b-9e50-15f48b6aff23', N'ca5818fd-be56-481a-a870-a9343672bcc9', 8, CAST(N'2025-10-07T00:04:21.013' AS DateTime))
GO
INSERT [dbo].[UserFingerprints] ([UserFingerprintId], [UserId], [PositionNumber], [CreatedDate]) VALUES (N'389d21e8-f2aa-4f3d-bcde-2b654c2dfed3', N'ca5818fd-be56-481a-a870-a9343672bcc9', 5, CAST(N'2025-10-06T23:07:49.790' AS DateTime))
GO
INSERT [dbo].[UserFingerprints] ([UserFingerprintId], [UserId], [PositionNumber], [CreatedDate]) VALUES (N'da9ade89-53bd-4408-8be4-2f5fd33cc93e', N'75148f69-e6d4-46ce-8e2c-d17b47e208d9', 3, CAST(N'2025-10-07T19:19:11.427' AS DateTime))
GO
INSERT [dbo].[UserFingerprints] ([UserFingerprintId], [UserId], [PositionNumber], [CreatedDate]) VALUES (N'c0159ef5-44d0-4c8c-ad82-a9f21325cab1', N'ca5818fd-be56-481a-a870-a9343672bcc9', 0, CAST(N'2025-10-06T22:28:32.340' AS DateTime))
GO
INSERT [dbo].[UserFingerprints] ([UserFingerprintId], [UserId], [PositionNumber], [CreatedDate]) VALUES (N'b89f40e8-f9e3-4a43-9561-d87fd89eed85', N'ca5818fd-be56-481a-a870-a9343672bcc9', 1, CAST(N'2025-10-06T21:59:23.507' AS DateTime))
GO
INSERT [dbo].[UserFingerprints] ([UserFingerprintId], [UserId], [PositionNumber], [CreatedDate]) VALUES (N'7a4a0955-e942-4cd9-85b0-da4a46a4110d', N'06ed4b48-6565-4698-9069-4b0ae9d189a3', 7, CAST(N'2025-10-07T19:32:22.320' AS DateTime))
GO
INSERT [dbo].[UserRoles] ([UserRoleId], [RoleName], [RoleDescription], [CreatedDate]) VALUES (N'ba0ad894-7c4c-45be-83fb-72307b2f34a0', N'Checker', N'Checker', CAST(N'2025-10-04T10:41:31.523' AS DateTime))
GO
INSERT [dbo].[UserRoles] ([UserRoleId], [RoleName], [RoleDescription], [CreatedDate]) VALUES (N'7a8ceb9d-7c7d-4f77-a063-de54ba4f1d9f', N'Professor', N'Professor', CAST(N'2025-10-04T10:41:31.523' AS DateTime))
GO
INSERT [dbo].[UserRoles] ([UserRoleId], [RoleName], [RoleDescription], [CreatedDate]) VALUES (N'f504246a-1206-4a78-9d3f-ff0b0346d308', N'Admin', N'Administrator', CAST(N'2025-10-03T20:54:10.630' AS DateTime))
GO
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [UserName], [Password], [UserRoleId], [CreatedDate], [Email]) VALUES (N'06ed4b48-6565-4698-9069-4b0ae9d189a3', N'Checker', N'One', N'test', N'098F6BCD4621D373CADE4E832627B4F6', N'ba0ad894-7c4c-45be-83fb-72307b2f34a0', CAST(N'2025-10-07T19:23:35.857' AS DateTime), N'test@mail.com')
GO
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [UserName], [Password], [UserRoleId], [CreatedDate], [Email]) VALUES (N'ca5818fd-be56-481a-a870-a9343672bcc9', N'System', N'Administrator', N'admin', N'0192023A7BBD73250516F069DF18B500', N'f504246a-1206-4a78-9d3f-ff0b0346d308', CAST(N'2025-10-03T21:45:18.067' AS DateTime), N'classroommonitoring2@gmail.com')
GO
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [UserName], [Password], [UserRoleId], [CreatedDate], [Email]) VALUES (N'75148f69-e6d4-46ce-8e2c-d17b47e208d9', N'Juan', N'Dela Cruz', N'test', N'098F6BCD4621D373CADE4E832627B4F6', N'7a8ceb9d-7c7d-4f77-a063-de54ba4f1d9f', CAST(N'2025-10-04T23:37:56.700' AS DateTime), N'jom72056@gmail.com')
GO
ALTER TABLE [dbo].[Attendance] ADD  DEFAULT (newid()) FOR [AttendanceId]
GO
ALTER TABLE [dbo].[Attendance] ADD  DEFAULT (getdate()) FOR [CreatedDate]
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
ALTER TABLE [dbo].[Subject] ADD  DEFAULT (newid()) FOR [SubjectId]
GO
ALTER TABLE [dbo].[Subject] ADD  CONSTRAINT [DF_Subject_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[UserFingerprints] ADD  DEFAULT (newid()) FOR [UserFingerprintId]
GO
ALTER TABLE [dbo].[UserFingerprints] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT (newid()) FOR [UserRoleId]
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [UserId]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_ProfessorID] FOREIGN KEY([ProfessorId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_ProfessorID]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_RoomReassignmentId] FOREIGN KEY([RoomReassignmentId])
REFERENCES [dbo].[Room] ([RoomId])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_RoomReassignmentId]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [FK_Attendance_RoomScheduleId] FOREIGN KEY([RoomScheduleId])
REFERENCES [dbo].[RoomSchedule] ([RoomScheduleId])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [FK_Attendance_RoomScheduleId]
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
ALTER TABLE [dbo].[RoomSchedule]  WITH CHECK ADD  CONSTRAINT [FK_RoomSchedule_SubjectId] FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subject] ([SubjectId])
GO
ALTER TABLE [dbo].[RoomSchedule] CHECK CONSTRAINT [FK_RoomSchedule_SubjectId]
GO
ALTER TABLE [dbo].[RoomSchedule]  WITH CHECK ADD  CONSTRAINT [FK_RoomShcedule_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([RoomId])
GO
ALTER TABLE [dbo].[RoomSchedule] CHECK CONSTRAINT [FK_RoomShcedule_RoomId]
GO
ALTER TABLE [dbo].[UserFingerprints]  WITH CHECK ADD  CONSTRAINT [FK_UserFingerprint_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UserFingerprints] CHECK CONSTRAINT [FK_UserFingerprint_UserId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRoleId] FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UserRoles] ([UserRoleId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserRoleId]
GO
