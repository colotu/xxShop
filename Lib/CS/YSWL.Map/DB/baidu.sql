GO

/****** Object:  Table [dbo].[Ms_Maps]    Script Date: 05/30/2012 11:25:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ms_Maps](
	[MapId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[PointerLongitude] [nvarchar](100) NULL,
	[PointDimension] [nvarchar](100) NULL,
	[PointClass] [nvarchar](100) NULL,
	[PointerType] [nvarchar](100) NULL,
	[PointerTitle] [nvarchar](100) NULL,
	[PointImg] [nvarchar](100) NULL,
	[PointerContent] [nvarchar](300) NULL,
	[SearchCity] [nvarchar](50) NULL,
	[searchArea] [nvarchar](50) NULL,
	[Level] [int] NULL,
	[enableKeyboard] [bit] NULL,
	[enableScrollWheelZoom] [bit] NULL,
	[NavigationControl] [bit] NULL,
	[ScaleControl] [bit] NULL,
	[MapTypeControl] [bit] NULL,
	[MarkersLongitude] [nvarchar](100) NULL,
	[Markersdimension] [nvarchar](100) NULL,
	[setAnimation] [nvarchar](100) NULL,
	[LoadEvent] [nvarchar](50) NULL,
	[MenuItemzoomIn] [bit] NULL,
	[MenuItemzoomOut] [bit] NULL,
	[MenuItemsetZoomTop] [bit] NULL,
	[MenuItemsetPoint] [bit] NULL,
	[MapType] [smallint] NOT NULL,
	[Other1] [nvarchar](100) NULL,
	[Other2] [nvarchar](100) NULL,
	[Other3] [nvarchar](50) NULL,
 CONSTRAINT [PK_BaiduMap] PRIMARY KEY CLUSTERED 
(
	[MapId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地图类型 0:百度地图 1:Google地图' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ms_Maps', @level2type=N'COLUMN',@level2name=N'MapType'
GO