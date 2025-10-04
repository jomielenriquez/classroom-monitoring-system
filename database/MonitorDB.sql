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
/****** Object:  Table [dbo].[Users]    Script Date: 04/10/2025 10:42:10 am ******/
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
INSERT [dbo].[UserRoles] ([UserRoleId], [RoleName], [RoleDescription], [CreatedDate]) VALUES (N'ba0ad894-7c4c-45be-83fb-72307b2f34a0', N'Checker', N'Checker', CAST(N'2025-10-04T10:41:31.523' AS DateTime))
GO
INSERT [dbo].[UserRoles] ([UserRoleId], [RoleName], [RoleDescription], [CreatedDate]) VALUES (N'7a8ceb9d-7c7d-4f77-a063-de54ba4f1d9f', N'Professor', N'Professor', CAST(N'2025-10-04T10:41:31.523' AS DateTime))
GO
INSERT [dbo].[UserRoles] ([UserRoleId], [RoleName], [RoleDescription], [CreatedDate]) VALUES (N'f504246a-1206-4a78-9d3f-ff0b0346d308', N'Admin', N'Administrator', CAST(N'2025-10-03T20:54:10.630' AS DateTime))
GO
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [UserName], [Password], [UserRoleId], [CreatedDate], [Email]) VALUES (N'ca5818fd-be56-481a-a870-a9343672bcc9', N'System', N'Administrator', N'admin', N'0192023A7BBD73250516F069DF18B500', N'f504246a-1206-4a78-9d3f-ff0b0346d308', CAST(N'2025-10-03T21:45:18.067' AS DateTime), N'classroommonitoring2@gmail.com')
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT (newid()) FOR [UserRoleId]
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [UserId]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRoleId] FOREIGN KEY([UserRoleId])
REFERENCES [dbo].[UserRoles] ([UserRoleId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserRoleId]
GO
