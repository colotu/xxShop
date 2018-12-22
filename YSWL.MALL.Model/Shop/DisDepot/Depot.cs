/**  版本信息模板在安装目录下，可自行修改。
* Depot.cs
*
* 功 能： N/A
* 类 名： Depot
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/7/27 17:36:53   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.DisDepot
{
	/// <summary>
	/// Depot:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Depot
	{
		public Depot()
		{}
		#region Model
		private int _depotid;
		private string _name;
		private string _code;
		private int _regionid;
		private string _address;
		private string _contactname;
		private string _phone;
		private string _email;
		private int _status;
		private string _helpcode;
		private DateTime _createddate;
		private decimal? _latitude;
		private decimal? _longitude;
		private int _type=0;
		private int _depotattr=0;
		private string _remark;
		/// <summary>
		/// 仓库ID
		/// </summary>
		public int DepotId
		{
			set{ _depotid=value;}
			get{return _depotid;}
		}
		/// <summary>
		/// 仓库名称
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 仓库编码
		/// </summary>
		public string Code
		{
			set{ _code=value;}
			get{return _code;}
		}
		/// <summary>
		/// 区域ID
		/// </summary>
		public int RegionId
		{
			set{ _regionid=value;}
			get{return _regionid;}
		}
		/// <summary>
		/// 详细地址
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 联系人
		/// </summary>
		public string ContactName
		{
			set{ _contactname=value;}
			get{return _contactname;}
		}
		/// <summary>
		/// 联系方式
		/// </summary>
		public string Phone
		{
			set{ _phone=value;}
			get{return _phone;}
		}
		/// <summary>
		/// 邮箱
		/// </summary>
		public string Email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 状态 
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 助记码
		/// </summary>
		public string HelpCode
		{
			set{ _helpcode=value;}
			get{return _helpcode;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreatedDate
		{
			set{ _createddate=value;}
			get{return _createddate;}
		}
		/// <summary>
		/// 纬度
		/// </summary>
		public decimal? Latitude
		{
			set{ _latitude=value;}
			get{return _latitude;}
		}
		/// <summary>
		/// 经度
		/// </summary>
		public decimal? Longitude
		{
			set{ _longitude=value;}
			get{return _longitude;}
		}
		/// <summary>
		/// 仓库类型 预留字段
		/// </summary>
		public int Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 仓库属性  0：自己的仓库 1：第三方
		/// </summary>
		public int DepotAttr
		{
			set{ _depotattr=value;}
			get{return _depotattr;}
		}
		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

