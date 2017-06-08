USE [iothubtelemetry]
GO

/****** Object:  Table [dbo].[IotHubSensorReadings]    Script Date: 3/26/2017 3:05:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IotHubSensorReadings](
	[UserId] [char](256) NOT NULL,
	[Age] [float] NOT NULL,
	[Height] [float] NOT NULL,
	[Weight] [float] NOT NULL,
	[HeartRateBPM] [float] NOT NULL,
	[BreathingRate] [float] NOT NULL,
	[Temperature] [float] NOT NULL,
	[Steps] [float] NOT NULL,
	[Velocity] [float] NOT NULL,
	[Altitude] [float] NOT NULL,
	[Ventilization] [float] NOT NULL,
	[Activity] [float] NOT NULL,
	[Cadence] [float] NOT NULL,
	[Speed] [float] NOT NULL,
	[HIB] [float] NOT NULL,
	[HeartRateRedZone] [float] NOT NULL,
	[HeartrateVariability] [float] NOT NULL,
	[Status] [int] NOT NULL,
	[Id] [char](256) NOT NULL,
	[DeviceId] [char](256) NOT NULL,
	[MessageType] [int] NOT NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
	[Timestamp] [datetime2](7) NOT NULL,
	[EventProcessedUtcTime] [datetime2](7) NOT NULL,
	[PartitionId] [int] NOT NULL,
	[EventEnqueuedUtcTime] [datetime2](7) NOT NULL,
	[companyname] [char](256) NOT NULL,
	[imageUrl] [char](256) NOT NULL,
	[firstname] [char](256) NOT NULL,
	[lastname] [char](256) NOT NULL,
	[username] [char](256) NOT NULL,
	[type] [char](256) NOT NULL,
	[phone] [char](256) NOT NULL,
	[email] [char](256) NOT NULL,
	[gender] [char](256) NOT NULL,
	[race] [char](256) NOT NULL
)

GO


