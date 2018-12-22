/**
* Trials.cs
*
* 功 能： N/A
* 类 名： Trials
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/5/22 17:39:52   N/A    初版
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
	/// Trials:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Trials
	{
		public Trials()
		{}
		#region Model
		private int _trialid;
		private int _categoryid=-1;
		private string _trialname;
		private int _enterpriseid=-1;
		private int? _regionid;
		private string _shortdescription;
		private string _unit;
		private string _description;
		private string _meta_title;
		private string _meta_description;
		private string _meta_keywords;
		private string _linklurl;
		private int _trialstatus;
		private DateTime _startdate;
		private DateTime _enddate;
		private DateTime _createddate;
		private int? _createduserid;
		private int _visticounts=0;
		private int _trialcounts=0;
		private int _displaysequence=0;
		private decimal _marketprice;
		private decimal _lowestsaleprice=0M;
		private string _maincategorypath;
		private string _extendcategorypath;
		private decimal? _points=0M;
		private string _imageurl;
		private string _thumbnailurl;
		private int? _maxquantity=0;
		private int? _minquantity=0;
		private string _tags;
		private string _seourl;
		private string _seoimagealt;
		private string _seoimagetitle;
		/// <summary>
		/// 
		/// </summary>
		public int TrialId
		{
			set{ _trialid=value;}
			get{return _trialid;}
		}
		/// <summary>
		/// 类别ID  0:正常有分类  -1:无分类
		/// </summary>
		public int CategoryId
		{
			set{ _categoryid=value;}
			get{return _categoryid;}
		}
		/// <summary>
		/// 名称
		/// </summary>
		public string TrialName
		{
			set{ _trialname=value;}
			get{return _trialname;}
		}
		/// <summary>
		/// 企业Id
		/// </summary>
		public int EnterpriseId
		{
			set{ _enterpriseid=value;}
			get{return _enterpriseid;}
		}
		/// <summary>
		/// 地区Id
		/// </summary>
		public int? RegionId
		{
			set{ _regionid=value;}
			get{return _regionid;}
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
		/// 单位
		/// </summary>
		public string Unit
		{
			set{ _unit=value;}
			get{return _unit;}
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
		/// SEO_Title
		/// </summary>
		public string Meta_Title
		{
			set{ _meta_title=value;}
			get{return _meta_title;}
		}
		/// <summary>
		/// SEO_Description
		/// </summary>
		public string Meta_Description
		{
			set{ _meta_description=value;}
			get{return _meta_description;}
		}
		/// <summary>
		/// SEO_KeyWord
		/// </summary>
		public string Meta_Keywords
		{
			set{ _meta_keywords=value;}
			get{return _meta_keywords;}
		}
		/// <summary>
		/// 试用链接
		/// </summary>
		public string LinklUrl
		{
			set{ _linklurl=value;}
			get{return _linklurl;}
		}
		/// <summary>
		/// 状态 0:即将进行 1:进行中(立即申请) 2:已结束
		/// </summary>
		public int TrialStatus
		{
			set{ _trialstatus=value;}
			get{return _trialstatus;}
		}
		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime StartDate
		{
			set{ _startdate=value;}
			get{return _startdate;}
		}
		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndDate
		{
			set{ _enddate=value;}
			get{return _enddate;}
		}
		/// <summary>
		/// 添加日期
		/// </summary>
		public DateTime CreatedDate
		{
			set{ _createddate=value;}
			get{return _createddate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CreatedUserID
		{
			set{ _createduserid=value;}
			get{return _createduserid;}
		}
		/// <summary>
		/// 访问次数
		/// </summary>
		public int VistiCounts
		{
			set{ _visticounts=value;}
			get{return _visticounts;}
		}
		/// <summary>
		/// 试用总数
		/// </summary>
		public int TrialCounts
		{
			set{ _trialcounts=value;}
			get{return _trialcounts;}
		}
		/// <summary>
		/// 显示顺序
		/// </summary>
		public int DisplaySequence
		{
			set{ _displaysequence=value;}
			get{return _displaysequence;}
		}
		/// <summary>
		/// 市场价
		/// </summary>
		public decimal MarketPrice
		{
			set{ _marketprice=value;}
			get{return _marketprice;}
		}
		/// <summary>
		/// 最低价
		/// </summary>
		public decimal LowestSalePrice
		{
			set{ _lowestsaleprice=value;}
			get{return _lowestsaleprice;}
		}
		/// <summary>
		/// 分类路径
		/// </summary>
		public string MainCategoryPath
		{
			set{ _maincategorypath=value;}
			get{return _maincategorypath;}
		}
		/// <summary>
		/// 扩展路径
		/// </summary>
		public string ExtendCategoryPath
		{
			set{ _extendcategorypath=value;}
			get{return _extendcategorypath;}
		}
		/// <summary>
		/// 积分
		/// </summary>
		public decimal? Points
		{
			set{ _points=value;}
			get{return _points;}
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
		/// <summary>
		/// 最大购买量
		/// </summary>
		public int? MaxQuantity
		{
			set{ _maxquantity=value;}
			get{return _maxquantity;}
		}
		/// <summary>
		/// 最小购买量
		/// </summary>
		public int? MinQuantity
		{
			set{ _minquantity=value;}
			get{return _minquantity;}
		}
		/// <summary>
		/// 标签
		/// </summary>
		public string Tags
		{
			set{ _tags=value;}
			get{return _tags;}
		}
		/// <summary>
		/// SEO Url地址优化规则
		/// </summary>
		public string SeoUrl
		{
			set{ _seourl=value;}
			get{return _seourl;}
		}
		/// <summary>
		/// SEO 图片Alt信息
		/// </summary>
		public string SeoImageAlt
		{
			set{ _seoimagealt=value;}
			get{return _seoimagealt;}
		}
		/// <summary>
		/// SEO 图片Title信息
		/// </summary>
		public string SeoImageTitle
		{
			set{ _seoimagetitle=value;}
			get{return _seoimagetitle;}
		}
		#endregion Model

	}
}

