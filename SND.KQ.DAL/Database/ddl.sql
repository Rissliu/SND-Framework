if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Subscription_Constraint2]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Subscription] DROP CONSTRAINT Subscription_Constraint2
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Subscription_Constraint3]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Subscription] DROP CONSTRAINT Subscription_Constraint3
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FilterNamespace_Constraint1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[FilterNamespace] DROP CONSTRAINT FilterNamespace_Constraint1
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Subscription_Constraint4]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Subscription] DROP CONSTRAINT Subscription_Constraint4
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EndpointReference_Constraint3]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[EndpointReference] DROP CONSTRAINT EndpointReference_Constraint3
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EndpointReference_Constraint2]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[EndpointReference] DROP CONSTRAINT EndpointReference_Constraint2
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EndpointReference]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[EndpointReference]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Filter]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Filter]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FilterNamespace]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[FilterNamespace]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PortType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PortType]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReferenceProperties]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ReferenceProperties]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Subscription]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Subscription]
GO

CREATE TABLE [dbo].[EndpointReference] (
	[EndpointReferenceId] [bigint] IDENTITY (1, 1) NOT NULL ,
	[EndpointReferenceAddress] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ReferencePropertiesId] [bigint] NULL ,
	[PortTypeId] [bigint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Filter] (
	[FilterId] [bigint] IDENTITY (1, 1) NOT NULL ,
	[FilterDialect] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[FilterValue] [nvarchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[FilterBodyElementName] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[FilterBodyElementNamespace] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[FilterNamespace] (
	[FilterId] [bigint] NOT NULL ,
	[FilterNamespacePrefix] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[FilterNamespaceNamespace] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PortType] (
	[PortTypeId] [bigint] IDENTITY (1, 1) NOT NULL ,
	[PortTypeName] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[PortTypeNamespace] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ReferenceProperties] (
	[ReferencePropertiesId] [bigint] IDENTITY (1, 1) NOT NULL ,
	[ReferencePropertiesFragment] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Subscription] (
	[SubscriptionId] [uniqueidentifier] NOT NULL ,
	[SubscriptionExpires] [datetime] NULL ,
	[NotifyToEndpointReferenceId] [bigint] NOT NULL ,
	[EndToEndpointReferenceId] [bigint] NULL ,
	[FilterId] [bigint] NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EndpointReference] ADD 
	CONSTRAINT [EndpointReference_Constraint1] PRIMARY KEY  NONCLUSTERED 
	(
		[EndpointReferenceId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Filter] ADD 
	CONSTRAINT [Filter_Constraint1] PRIMARY KEY  NONCLUSTERED 
	(
		[FilterId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[FilterNamespace] ADD 
	CONSTRAINT [FilterNamespace_Constraint2] PRIMARY KEY  NONCLUSTERED 
	(
		[FilterId],
		[FilterNamespacePrefix]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[PortType] ADD 
	CONSTRAINT [PortType_Constraint1] PRIMARY KEY  NONCLUSTERED 
	(
		[PortTypeId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ReferenceProperties] ADD 
	CONSTRAINT [ReferenceProperties_Constraint1] PRIMARY KEY  NONCLUSTERED 
	(
		[ReferencePropertiesId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Subscription] ADD 
	CONSTRAINT [Subscription_Constraint1] PRIMARY KEY  NONCLUSTERED 
	(
		[SubscriptionId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[EndpointReference] ADD 
	CONSTRAINT [EndpointReference_Constraint2] FOREIGN KEY 
	(
		[ReferencePropertiesId]
	) REFERENCES [dbo].[ReferenceProperties] (
		[ReferencePropertiesId]
	),
	CONSTRAINT [EndpointReference_Constraint3] FOREIGN KEY 
	(
		[PortTypeId]
	) REFERENCES [dbo].[PortType] (
		[PortTypeId]
	)
GO

ALTER TABLE [dbo].[FilterNamespace] ADD 
	CONSTRAINT [FilterNamespace_Constraint1] FOREIGN KEY 
	(
		[FilterId]
	) REFERENCES [dbo].[Filter] (
		[FilterId]
	) ON DELETE CASCADE 
GO

ALTER TABLE [dbo].[Subscription] ADD 
	CONSTRAINT [Subscription_Constraint2] FOREIGN KEY 
	(
		[NotifyToEndpointReferenceId]
	) REFERENCES [dbo].[EndpointReference] (
		[EndpointReferenceId]
	),
	CONSTRAINT [Subscription_Constraint3] FOREIGN KEY 
	(
		[EndToEndpointReferenceId]
	) REFERENCES [dbo].[EndpointReference] (
		[EndpointReferenceId]
	),
	CONSTRAINT [Subscription_Constraint4] FOREIGN KEY 
	(
		[FilterId]
	) REFERENCES [dbo].[Filter] (
		[FilterId]
	)
GO

