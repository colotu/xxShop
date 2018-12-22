/**
* TrialReports.cs
*
* 功 能： N/A
* 类 名： TrialReports
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/5/22 18:12:41   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.Trial
{
	/// <summary>
	/// TrialReports:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class TrialReports
	{
		public TrialReports()
		{}
		#region Model
		private int _reportid;
		private string _title;
		private string _linkurl;
		private string _shortdescription;
		private int _createduserid=-1;
		private string _createdusername;
		private string _description;
		private string _imageurl;
		private string _thumbnailurl;
		/// <summary>
		/// 
		/// </summary>
		public int ReportId
		{
			set{ _reportid=value;}
			get{return _reportid;}
		}
		/// <summary>
		/// 名称
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LinkUrl
		{
			set{ _linkurl=value;}
			get{return _linkurl;}
		}
		/// <summary>
		/// 介绍
		/// </summary>
		public string ShortDescription
		{
			set{ _shortdescription=value;}
			get{return _shortdescription;}
		}
		/// <summary>
		/// 发表者
		/// </summary>
		public int CreatedUserID
		{
			set{ _createduserid=value;}
			get{return _createduserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CreatedUserName
		{
			set{ _createdusername=value;}
			get{return _createdusername;}
		}
		/// <summary>
		/// 描述
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 图片路径
		/// </summary>
		public string ImageUrl
		{
			set{ _imageurl=value;}
			get{return _imageurl;}
		}
		/// <summary>
		/// 图片路径1
		/// </summary>
		public string ThumbnailUrl
		{
			set{ _thumbnailurl=value;}
			get{return _thumbnailurl;}
		}
		#endregion Model

	}
}

