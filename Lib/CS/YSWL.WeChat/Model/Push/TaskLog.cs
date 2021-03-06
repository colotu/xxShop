﻿/**  版本信息模板在安装目录下，可自行修改。
* TaskLog.cs
*
* 功 能： N/A
* 类 名： TaskLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/7 17:57:54   N/A    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.WeChat.Model.Push
{
	/// <summary>
	/// TaskLog:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class TaskLog
	{
		public TaskLog()
		{}
		#region Model
		private int _taskid;
		private string _username;
		private DateTime _createdtime;
		/// <summary>
		/// 任务ID
		/// </summary>
		public int TaskId
		{
			set{ _taskid=value;}
			get{return _taskid;}
		}
		/// <summary>
		/// 用户微信号
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 发送时间
		/// </summary>
		public DateTime CreatedTime
		{
			set{ _createdtime=value;}
			get{return _createdtime;}
		}
		#endregion Model

	}
}

