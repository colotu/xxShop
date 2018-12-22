/**  版本信息模板在安装目录下，可自行修改。
* UserDistribution.cs
*
* 功 能： N/A
* 类 名： UserDistribution
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/1/14 12:07:50   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Members
{
	/// <summary>
	/// UserDistribution:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class UserDistribution
	{
		public UserDistribution()
		{}
		#region Model
		private int _userid;
		private int _distributionid;
		/// <summary>
		/// 客户ID
		/// </summary>
		public int UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 分销商ID  (存分销商企业ID)
		/// </summary>
		public int DistributionId
		{
			set{ _distributionid=value;}
			get{return _distributionid;}
		}
		#endregion Model

	}
}

