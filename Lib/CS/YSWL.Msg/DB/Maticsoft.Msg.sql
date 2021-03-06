USE [MaticsoftFrameWork]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTableInfo]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetTableInfo]
@table NVARCHAR(200)
as
SELECT 
    表名       = case when a.colorder=1 then d.name else '' end,
    表说明     = case when a.colorder=1 then isnull(f.value,'') else '' end,
    字段序号   = a.colorder,
    字段名     = a.name,
    标识       = case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end,
    主键       = case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (
                     SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then '√' else '' end,
    类型       = b.name,
    占用字节数 = a.length,
    长度       = COLUMNPROPERTY(a.id,a.name,'PRECISION'),
    小数位数   = isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),
    允许空     = case when a.isnullable=1 then '√'else '' end,
    默认值     = isnull(e.text,''),
    字段说明   = isnull(g.[value],'')
FROM 
    syscolumns a
left join 
    systypes b 
on 
    a.xusertype=b.xusertype
inner join 
    sysobjects d 
on 
    a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
left join 
    syscomments e 
on 
    a.cdefault=e.id
left join 
sys.extended_properties   g 
on 
    a.id=G.major_id and a.colid=g.minor_id  
left join 

sys.extended_properties f
on 
    d.id=f.major_id and f.minor_id=0
where 
    d.name='ReceiveMsgBox'    --如果只查询指定表,加上此条件
order by 
    a.id,a.colorder
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllTableInfoByName]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[sp_GetAllTableInfoByName]
(@TableName NVARCHAR(50))
AS
SELECT 
    表名       = case when a.colorder=1 then d.name else '' end,
    表说明     = case when a.colorder=1 then isnull(f.value,'') else '' end,
    字段序号   = a.colorder,
    字段名     = a.name,
    标识       = case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end,
    主键       = case when exists(SELECT 1 FROM sysobjects where xtype='PK' and parent_obj=a.id and name in (
                     SELECT name FROM sysindexes WHERE indid in( SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid))) then '√' else '' end,
    类型       = b.name,
    占用字节数 = a.length,
    长度       = COLUMNPROPERTY(a.id,a.name,'PRECISION'),
    小数位数   = isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),
    允许空     = case when a.isnullable=1 then '√'else '' end,
    默认值     = isnull(e.text,''),
    字段说明   = isnull(g.[value],'')
FROM 
    syscolumns a
left join 
    systypes b 
on 
    a.xusertype=b.xusertype
inner join 
    sysobjects d 
on 
    a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
left join 
    syscomments e 
on 
    a.cdefault=e.id
left join 
sys.extended_properties   g 
on 
    a.id=G.major_id and a.colid=g.minor_id  
left join 

sys.extended_properties f
on 
    d.id=f.major_id and f.minor_id=0
where 
    d.name=@TableName    --如果只查询指定表,加上此条件
order by 
    a.id,a.colorder
GO
/****** Object:  Table [dbo].[MsgType]    Script Date: 04/15/2012 21:31:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MsgType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NULL,
	[UserType] [nvarchar](50) NULL,
	[Remark] [nvarchar](100) NULL,
	[Other] [nvarchar](100) NULL,
 CONSTRAINT [PK_MessageType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MsgRecord]    Script Date: 04/15/2012 21:31:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MsgRecord](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MsgBoxId] [int] NULL,
	[MsgType] [nvarchar](50) NULL,
	[Other] [nvarchar](100) NULL,
	[MsgState] [nvarchar](50) NULL,
	[ReceiverID] [nvarchar](50) NULL,
 CONSTRAINT [PK_SystemMsgRecord] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgRecord', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统消息的ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgRecord', @level2type=N'COLUMN',@level2name=N'MsgBoxId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型 0：已读 1：已删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgRecord', @level2type=N'COLUMN',@level2name=N'MsgType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预留字段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgRecord', @level2type=N'COLUMN',@level2name=N'Other'
GO
/****** Object:  Table [dbo].[MsgBox]    Script Date: 04/15/2012 21:31:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MsgBox](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SenderID] [nvarchar](100) NULL,
	[ReceiverID] [nvarchar](100) NULL,
	[Title] [nvarchar](100) NULL,
	[Content] [nvarchar](4000) NULL,
	[MsgType] [nvarchar](50) NULL,
	[SendTime] [datetime] NULL,
	[ReadTime] [datetime] NULL,
	[ReMark] [nvarchar](100) NULL,
	[Other] [nvarchar](100) NULL,
	[ReceiverIsRead] [bit] NULL,
	[SenderIsDel] [bit] NULL,
	[ReaderIsDel] [bit] NULL,
 CONSTRAINT [PK_SendMsgBox] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgBox', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgBox', @level2type=N'COLUMN',@level2name=N'SenderID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接受者ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgBox', @level2type=N'COLUMN',@level2name=N'ReceiverID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgBox', @level2type=N'COLUMN',@level2name=N'Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgBox', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'msg类型，0为不同用户发的信息，1为系统消息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgBox', @level2type=N'COLUMN',@level2name=N'MsgType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgBox', @level2type=N'COLUMN',@level2name=N'SendTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgBox', @level2type=N'COLUMN',@level2name=N'ReMark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'预留字段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MsgBox', @level2type=N'COLUMN',@level2name=N'Other'
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_SetSystemMsgStateToDel]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[Sp_MsgBox_SetSystemMsgStateToDel]
    @ID int ,
    @ReceiverID NVARCHAR(50) ,
    @AdminId NVARCHAR(50) ,
    @UserType NVARCHAR(50) 
    
  AS 
   declare @count int 
   BEGIN 
     
     select @count=COUNT(1) from dbo.MsgBox where  SenderID=@AdminId
     and ReceiverID=@ReceiverID and ID=@ID
     if(@count>0)
     BEGIN
     UPDATE dbo.MsgBox SET ReaderIsDel='TRUE' WHERE ID=@ID
     END
     ELSE
     BEGIN
     SELECT @count=COUNT(1) FROM MsgRecord WHERE MsgBoxId=@ID AND  ReceiverID=@ReceiverID
     IF(@count>0)
       UPDATE dbo.MsgRecord SET  MsgState='-1'  WHERE MsgBoxId=@ID AND ReceiverID=@ReceiverID
     ELSE
     
      INSERT INTO MsgRecord
           ([MsgBoxId]
           ,[MsgType]
           ,[MsgState]
           ,[ReceiverID])
     VALUES
           (@ID,@UserType,'-1',@ReceiverID)
     
   
     END
      
   END
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_SetSystemMsgStateToAlreadyRead]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[Sp_MsgBox_SetSystemMsgStateToAlreadyRead]
    @ID  int,
    @ReceiverID NVARCHAR(50) ,
    @AdminId NVARCHAR(50) ,
    @UserType NVARCHAR(50) 
   AS 
   declare @count int
   select @count=COUNT(1) from dbo.MsgBox where ReceiverID=@ReceiverID and
   ID=@ID and SenderID=@AdminId AND SenderIsDel='FALSE'
   if(@count>0)
   begin
   update  MsgBox set ReceiverIsRead='True',ReadTime=GETDATE() where ID=@ID
   end 
   else
   begin
   INSERT INTO MsgRecord
           ([MsgBoxId]
           ,[MsgType]
           ,[MsgState]
           ,[ReceiverID]
           ,Other)
     VALUES
           (@ID,@UserType,'1',@ReceiverID,GETDATE())

   end
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_SetSendMsgToDelById]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_SetSendMsgToDelById]
@ID int

as 
update dbo.MsgBox set SenderIsDel='True' where  ID=@ID
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_SetMsgStateToAlreadyRead]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[Sp_MsgBox_SetMsgStateToAlreadyRead]
    @ID  int
   AS 
update dbo.MsgBox set ReceiverIsRead='True' where ID=@ID
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_SetAdminMsgToDelById]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_SetAdminMsgToDelById]
@ID int,
@AdminId nvarchar(50)

as 
update dbo.MsgBox set SenderIsDel='True' where SenderID=@AdminId and ID=@ID
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetSystemMsgNotReadListByPage]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[Sp_MsgBox_GetSystemMsgNotReadListByPage]
  @ReceiverID NVARCHAR(50) ,
    @AdminId NVARCHAR(50) ,
    @UserType NVARCHAR(50) ,
    @StartIndex int,
    @EndIndex int
  
   AS 
   select * from (
          select ROW_NUMBER() OVER(ORDER BY ReadTime)AS row , * from (
            SELECT  *
            FROM    dbo.MsgBox
            WHERE   ReceiverID = @ReceiverID
                    AND SenderID = @AdminId
                    AND ReceiverIsRead = 'False'
                    AND ReaderIsDel = 'False'
            UNION
            SELECT    *    
            FROM    dbo.MsgBox
            WHERE   ID NOT IN ( SELECT  MsgBoxId
                                FROM    dbo.MsgRecord
                                WHERE   ReceiverID = @ReceiverID )
                                AND MsgType=@UserType
 ) as temp1
 ) as temp2 where row between @StartIndex and @EndIndex
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetSystemMsgNotReadList]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[Sp_MsgBox_GetSystemMsgNotReadList]
  @ReceiverID NVARCHAR(50) ,
    @AdminId NVARCHAR(50) ,
    @UserType NVARCHAR(50) 
  
   AS 
   
            SELECT  *
            FROM    dbo.MsgBox
            WHERE   ReceiverID = @ReceiverID
                    AND SenderID = @AdminId
                    AND ReceiverIsRead = 'False'
                    AND ReaderIsDel = 'False'
            UNION
            SELECT    *    
            FROM    dbo.MsgBox
            WHERE   ID NOT IN ( SELECT  MsgBoxId
                                FROM    dbo.MsgRecord
                                WHERE   ReceiverID = @ReceiverID )
                                AND MsgType=@UserType
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetSystemMsgNotReadCount]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_MsgBox_GetSystemMsgNotReadCount]
    @ReceiverID NVARCHAR(50) ,
    @AdminId NVARCHAR(50) ,
    @UserType NVARCHAR(50) 
  
AS       --第一种是一对一的发，这时候如果用户的id和系统管理员的id相配并且用户没有读该消息 
    BEGIN
        DECLARE @count1 INT 
        DECLARE @count2 INT 
        DECLARE @ReturnCount INT 
        BEGIN
            SELECT  @count1 = COUNT(1)
            FROM    dbo.MsgBox
            WHERE   ReceiverID = @ReceiverID
                    AND SenderID = @AdminId
                    AND ReceiverIsRead = 'False' 
                    AND ReaderIsDel='False'
   
        END
   --下面是一对多 这种情况下 如果在msgrecord表中存在receiveid的相关信息 说明这条数据已经读了 所以找的是在msgbox中存在但是在msgrecord表中不存的数据 并且用户的类型要相配
        BEGIN
            SELECT  @count2 = COUNT(1)
            FROM    dbo.MsgBox
            WHERE   ID NOT IN ( SELECT  MsgBoxId
                                FROM    dbo.MsgRecord
                                WHERE   ReceiverID = @ReceiverID )
                                AND MsgType=@UserType
        END
set @ReturnCount =@count1 + @count2
    END
 return    @ReturnCount
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetSystemMsgAlreadyReadListByPage]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[Sp_MsgBox_GetSystemMsgAlreadyReadListByPage]
  @ReceiverID NVARCHAR(50) ,
    @AdminId NVARCHAR(50) ,
    @UserType NVARCHAR(50) ,
    @StartIndex int ,
    @EndIndex int
  
   AS 
   select * from (
   select *,ROW_NUMBER() over(order by ReadTime) as row from 
   
        (    SELECT  ID,Title,Content,ReadTime
            FROM    dbo.MsgBox
            WHERE   ReceiverID = @ReceiverID
                    AND SenderID = @AdminId
                    AND ReceiverIsRead = 'True'
                    AND ReaderIsDel = 'False'
            UNION
            SELECT    ID,Title,Content,ReadTime 
            FROM    dbo.MsgBox
            WHERE   ID  IN ( SELECT  MsgBoxId
                                FROM    dbo.MsgRecord
                                WHERE   ReceiverID = @ReceiverID 
                                and
                                MsgState='1'
                                )
                                AND MsgType=@UserType
                                ) as temp1
 
 ) as temp2 where row between @StartIndex and @EndIndex
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetSystemMsgAlreadyReadList]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[Sp_MsgBox_GetSystemMsgAlreadyReadList]
  @ReceiverID NVARCHAR(50) ,
    @AdminId NVARCHAR(50) ,
    @UserType NVARCHAR(50) 
  
   AS 
   
            SELECT  *
            FROM    dbo.MsgBox
            WHERE   ReceiverID = @ReceiverID
                    AND SenderID = @AdminId
                    AND ReceiverIsRead = 'False'
                    AND ReaderIsDel = 'False'
            UNION
            SELECT    *    
            FROM    dbo.MsgBox
            WHERE   ID  IN ( SELECT  MsgBoxId
                                FROM    dbo.MsgRecord
                                WHERE   ReceiverID = @ReceiverID 
                                and
                                MsgState='-1'
                                )
                                AND MsgType=@UserType
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetSystemMsgAlreadyReadCount]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Sp_MsgBox_GetSystemMsgAlreadyReadCount]
    @ReceiverID NVARCHAR(50) ,
    @AdminId NVARCHAR(50) ,
    @UserType NVARCHAR(50) 
  
AS       --第一种是一对一的发，这时候如果用户的id和系统管理员的id相配并且用户没有读该消息 
    BEGIN
        DECLARE @count1 INT 
        DECLARE @count2 INT 
        DECLARE @ReturnCount INT 
        BEGIN
            SELECT  @count1 = COUNT(1)
            FROM    dbo.MsgBox
            WHERE   ReceiverID = @ReceiverID
                    AND SenderID = @AdminId
                    AND ReceiverIsRead = 'True' 
                    AND ReaderIsDel='False'
   
        END
   --下面是一对多 这种情况下 如果在msgrecord表中存在receiveid的相关信息 说明这条数据已经读了 所以找的是在msgbox中存在但是在msgrecord表中不存的数据 并且用户的类型要相配
        BEGIN
            SELECT  @count2 = COUNT(1)
            FROM    dbo.MsgBox
            WHERE   ID  IN ( SELECT  MsgBoxId
                                FROM    dbo.MsgRecord
                                WHERE   ReceiverID = @ReceiverID and
                                MsgState='1'
                               )
                                AND MsgType=@UserType
        END
set @ReturnCount =@count1 + @count2
    END
 return    @ReturnCount
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetSendMsgCount]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetSendMsgCount]
@SenderID nvarchar(50)
as 
declare @count int 
select @count=COUNT(1) from dbo.MsgBox
where SenderID=@SenderID
and SenderIsDel='False'


return @count
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetReceiveMsgNotReadListByPage]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetReceiveMsgNotReadListByPage]
@ReceiverID nvarchar(50),
@AdminId nvarchar(50),
@StartIndex int,
@EndIndex int
as 
select * from (
select *,ROW_NUMBER() over(order by SendTime desc) as row from 
(
select ID,SenderID,Title,Content,SendTime from dbo.MsgBox
where ReceiverID=@ReceiverID
and ReaderIsDel='False'
and ReceiverIsRead='False'
and SenderID<>@AdminId
) as temp 
)  as temp1 where row between 
@StartIndex and @EndIndex
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetReceiveMsgNotReadList]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Sp_MsgBox_GetReceiveMsgNotReadList]
@ReceiverID nvarchar(50),
@AdminId nvarchar(50)
as 

select ID,SenderID,Title,Content,SendTime from dbo.MsgBox
where ReceiverID=@ReceiverID
and ReaderIsDel='False'
and ReceiverIsRead='False'
and SenderID<>@AdminId
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetReceiveMsgNotReadCount]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetReceiveMsgNotReadCount]
@ReceiverID nvarchar(50),
@AdminId nvarchar(50)
as 
declare @count int 
select @count=COUNT(1) from dbo.MsgBox
where ReceiverID=@ReceiverID
and ReaderIsDel='False'
and ReceiverIsRead='False'
and SenderID<>@AdminId

return @count
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetReceiveMsgAreadyReadCount]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetReceiveMsgAreadyReadCount]
@ReceiverID nvarchar(50),
@AdminId nvarchar(50)
as 
declare @count int 
select @count=COUNT(1) from dbo.MsgBox
where ReceiverID=@ReceiverID
and ReaderIsDel='False'
and ReceiverIsRead='True'
and SenderID<>@AdminId

return @count
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetReceiveMsgAlreadyReadListByPage]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetReceiveMsgAlreadyReadListByPage]
@ReceiverID nvarchar(50),
@AdminId nvarchar(50),
@StartIndex int,
@EndIndex int
as 
select * from (
select *,ROW_NUMBER() over(order by SendTime desc) as row from 
(
select ID,SenderID,Title,Content,SendTime from dbo.MsgBox
where ReceiverID=@ReceiverID
and ReaderIsDel='True'
and ReceiverIsRead='False'
and SenderID<>@AdminId
) as temp 
)  as temp1 where row between 
@StartIndex and @EndIndex
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetReceiveMsgAlreadyReadList]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetReceiveMsgAlreadyReadList]
@ReceiverID nvarchar(50),
@AdminId nvarchar(50)
as 

select ID,SenderID,Title,Content,SendTime from dbo.MsgBox
where ReceiverID=@ReceiverID
and ReaderIsDel='False'
and ReceiverIsRead='True'
and SenderID<>@AdminId
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAllSystemMsgListByPage]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  PROCEDURE [dbo].[Sp_MsgBox_GetAllSystemMsgListByPage]
  @ReceiverID NVARCHAR(50) ,
  @AdminId NVARCHAR(50) ,
  @UserType NVARCHAR(50),
  @StartIndex int,
  @EndIndex int
  AS
      begin
      
      select * from(
 select *,Row_Number() over(order by ID desc) as row from (
        ( SELECT ID,Title,Content,ReceiverIsRead
         FROM dbo.MsgBox WHERE ReceiverID=@ReceiverID
         AND SenderID=@AdminId 
         AND ReaderIsDel='False')
         union
 
      
      --SELECT * FROM MsgBox  WHERE  ID NOT IN (SELECT MsgBoxId FROM 
      --dbo.MsgRecord WHERE  MsgState=-1 AND ReceiverID=@ReceiverID )
      --AND MsgType=@UserType and SenderID=@AdminId 
      
      (
      SELECT ID,Title,Content,ReceiverIsRead='False' FROM MsgBox  WHERE  ID NOT IN (SELECT MsgBoxId FROM 
      dbo.MsgRecord WHERE   ReceiverID=@ReceiverID )
      AND MsgType=@UserType and SenderID=@AdminId 
      )
      union
      (
    
      SELECT ID,Title,Content,ReceiverIsRead='True' FROM MsgBox  WHERE  ID  IN (SELECT MsgBoxId FROM 
      dbo.MsgRecord WHERE  MsgState=1 and ReceiverID=@ReceiverID )
      AND MsgType=@UserType and SenderID=@AdminId 
    )) as temp
    ) as temp1
    where row between @StartIndex and @EndIndex
  end
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAllSystemMsgList]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_MsgBox_GetAllSystemMsgList]
  @ReceiverID NVARCHAR(50) ,
  @AdminId NVARCHAR(50) ,
  @UserType NVARCHAR(50) 
  AS
      begin
 
        ( SELECT ID,Title,Content,ReceiverIsRead
         FROM dbo.MsgBox WHERE ReceiverID=@ReceiverID
         AND SenderID=@AdminId 
         AND ReaderIsDel='False')
         union
 
      
      --SELECT * FROM MsgBox  WHERE  ID NOT IN (SELECT MsgBoxId FROM 
      --dbo.MsgRecord WHERE  MsgState=-1 AND ReceiverID=@ReceiverID )
      --AND MsgType=@UserType and SenderID=@AdminId 
      
      (
      SELECT ID,Title,Content,ReceiverIsRead='False' FROM MsgBox  WHERE  ID NOT IN (SELECT MsgBoxId FROM 
      dbo.MsgRecord WHERE   ReceiverID=@ReceiverID )
      AND MsgType=@UserType and SenderID=@AdminId 
      )
      union
      (
    
      SELECT ID,Title,Content,ReceiverIsRead='True' FROM MsgBox  WHERE  ID  IN (SELECT MsgBoxId FROM 
      dbo.MsgRecord WHERE  MsgState=1 and ReceiverID=@ReceiverID )
      AND MsgType=@UserType and SenderID=@AdminId 
    )
  end
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAllSystemMsgCount]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_MsgBox_GetAllSystemMsgCount]
  @ReceiverID NVARCHAR(50) ,
  @AdminId NVARCHAR(50) ,
  @UserType NVARCHAR(50) 
  AS
 
      DECLARE @count1 INT 
      DECLARE @count2 INT 
      DECLARE @ReturnCount INT  
      begin
 
         SELECT @count1=COUNT(1)
         FROM dbo.MsgBox WHERE ReceiverID=@ReceiverID
         AND SenderID=@AdminId 
         AND ReaderIsDel='False'
  
 
      
      SELECT @count2=COUNT(1) FROM MsgBox  WHERE  ID NOT IN (SELECT MsgBoxId FROM 
      dbo.MsgRecord WHERE  MsgState=-1 AND ReceiverID=@ReceiverID )
      AND MsgType=@UserType and SenderID=@AdminId 
    
  
        
        set @ReturnCount=@count1+@count2
        end 
        
   RETURN @ReturnCount
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAllSendMsgListByPage]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetAllSendMsgListByPage]
@SenderID nvarchar(50),
@StartIndex int,
@EndIndex int
as 
declare @count int 
select * from (
select *,ROW_NUMBER() over(order by SendTime desc) as row from
(
select SenderID,Title,Content,ReceiverIsRead,SendTime from dbo.MsgBox
where SenderID=@SenderID
and SenderIsDel='False'

) as temp
)  as temp2  where row between @StartIndex and @EndIndex
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAllSendMsgList]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetAllSendMsgList]
@SenderID nvarchar(50)
as select * from dbo.MsgBox
where SenderID=@SenderID
and SenderIsDel='False'
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAllReceiveMsgListByPage]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetAllReceiveMsgListByPage]
@ReceiverID nvarchar(50),
@AdminId nvarchar(50),
@StartIndex int,
@EndIndex int
as 
declare @count int 
select * from (
select *,ROW_NUMBER() over(order by SendTime desc) as row from
(
select SenderID,Title,Content,ReceiverIsRead,SendTime from dbo.MsgBox
where ReceiverID=@ReceiverID
and ReaderIsDel='False'
and SenderID<>@AdminId
) as temp
)  as temp2  where row between @StartIndex and @EndIndex
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAllReceiveMsgList]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetAllReceiveMsgList]
@ReceiverID nvarchar(50),
@AdminId nvarchar(50)
as select * from dbo.MsgBox
where ReceiverID=@ReceiverID
and ReaderIsDel='False'
and SenderID<>@AdminId
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAllReceiveMsgCount]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetAllReceiveMsgCount]
@ReceiverID nvarchar(50),
@AdminId nvarchar(50)
as 
declare @count int 
select @count=COUNT(1) from dbo.MsgBox
where ReceiverID=@ReceiverID
and ReaderIsDel='False'
and SenderID<>@AdminId

return @count
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAdminSendMsgListByPage]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--create procedure Sp_MsgBox_SetAdminMsgToDelById
--@ID int,
--@AdminId nvarchar(50)

--as 
--update dbo.MsgBox set SenderIsDel='True' where SenderID=@AdminId and ID=@ID

create procedure [dbo].[Sp_MsgBox_GetAdminSendMsgListByPage]
@AdminId nvarchar(50),
@StartIndex int,
@EndIndex int 

as  
select * from (
select *,ROW_NUMBER()over(order by SendTime desc) as row from 
(
select * from  dbo.MsgBox where SenderIsDel='False' and SenderID=@AdminId
)
as temp
)
as temp2 where row between @StartIndex and @EndIndex
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAdminSendMsgList]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetAdminSendMsgList]
@AdminId nvarchar(50)

as  select * from  dbo.MsgBox where SenderIsDel='False' and SenderID=@AdminId
GO
/****** Object:  StoredProcedure [dbo].[Sp_MsgBox_GetAdminSendMsgCount]    Script Date: 04/15/2012 21:31:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sp_MsgBox_GetAdminSendMsgCount]
@AdminId nvarchar(50)

as  
declare @count int 
select @count=COUNT(1) from  dbo.MsgBox where SenderIsDel='False' and SenderID=@AdminId

return @count
GO
/****** Object:  Default [DF_MsgBox_ReMark]    Script Date: 04/15/2012 21:31:40 ******/
ALTER TABLE [dbo].[MsgBox] ADD  CONSTRAINT [DF_MsgBox_ReMark]  DEFAULT ((0)) FOR [ReMark]
GO
