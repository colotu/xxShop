/**  版本信息模板在安装目录下，可自行修改。
* DepotRegion.cs
*
* 功 能： N/A
* 类 名： DepotRegion
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/7/27 17:36:56   N/A    初版
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
	/// DepotRegion:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class DepotRegion
	{
		public DepotRegion()
		{}
		#region Model
		private int _depotid;
		private int _regionid;
		private string _regionname;
		private bool _status= false;
		private string _path;
		private int _depth;
		/// <summary>
		/// 分销商
		/// </summary>
		public int DepotId
		{
			set{ _depotid=value;}
			get{return _depotid;}
		}
		/// <summary>
		/// 地区Id
		/// </summary>
		public int RegionId
		{
			set{ _regionid=value;}
			get{return _regionid;}
		}
		/// <summary>
		/// 名称
		/// </summary>
		public string RegionName
		{
			set{ _regionname=value;}
			get{return _regionname;}
		}
		/// <summary>
		/// 状态    0：启用 1：不启用
		/// </summary>
		public bool Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Path
		{
			set{ _path=value;}
			get{return _path;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Depth
		{
			set{ _depth=value;}
			get{return _depth;}
		}
		#endregion Model

	}
}

