/**  版本信息模板在安装目录下，可自行修改。
* CommissionPro.cs
*
* 功 能： N/A
* 类 名： CommissionPro
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/2/13 13:59:34   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.Commission
{
	/// <summary>
	/// CommissionPro:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CommissionPro
	{
		public CommissionPro()
		{}
		#region Model
		private int _ruleid;
		private long _productid;
		/// <summary>
		/// 佣金规则ID
		/// </summary>
		public int RuleId
		{
			set{ _ruleid=value;}
			get{return _ruleid;}
		}
		/// <summary>
		/// 参与佣金活动的商品
		/// </summary>
		public long ProductId
		{
			set{ _productid=value;}
			get{return _productid;}
		}
		#endregion Model

	}
}

