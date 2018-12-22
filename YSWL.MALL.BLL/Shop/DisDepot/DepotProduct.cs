/**  版本信息模板在安装目录下，可自行修改。
* DepotProduct.cs
*
* 功 能： N/A
* 类 名： DepotProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/7/27 17:36:55   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.MALL.Model.Shop.DisDepot;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.DisDepot;
namespace YSWL.MALL.BLL.Shop.DisDepot
{
	/// <summary>
	/// DepotProduct
	/// </summary>
	public partial class DepotProduct
	{
        private readonly IDepotProduct dal = DAShopDisDepot.CreateDepotProduct();
		public DepotProduct()
		{}
		
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

