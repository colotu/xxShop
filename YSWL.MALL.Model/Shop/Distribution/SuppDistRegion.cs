/**  版本信息模板在安装目录下，可自行修改。
* SuppDistRegion.cs
*
* 功 能： N/A
* 类 名： SuppDistRegion
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/26 18:31:53   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.Distribution
{
	/// <summary>
	/// SuppDistRegion:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SuppDistRegion
	{
		public SuppDistRegion()
		{}
		#region Model
		private int _supplierid;
		private int _regionid;
		/// <summary>
		/// 分销商
		/// </summary>
		public int SupplierId
		{
			set{ _supplierid=value;}
			get{return _supplierid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int RegionId
		{
			set{ _regionid=value;}
			get{return _regionid;}
		}
		#endregion Model

	}
}

