/**  版本信息模板在安装目录下，可自行修改。
* RankLimit.cs
*
* 功 能： N/A
* 类 名： RankLimit
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/12/17 16:54:41   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Members
{
	/// <summary>
	/// RankLimit:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class RankLimit
	{
		public RankLimit()
		{}
		#region Model
		private int _limitid;
		private string _name;
		private int _cycle;
		private string _cycleunit;
		private int _maxtimes;
		private int _targetid=0;
		private int _targettype=0;
		/// <summary>
		/// 
		/// </summary>
		public int LimitID
		{
			set{ _limitid=value;}
			get{return _limitid;}
		}
		/// <summary>
		/// 条件限制名称
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 周期
		/// </summary>
		public int Cycle
		{
			set{ _cycle=value;}
			get{return _cycle;}
		}
		/// <summary>
		/// 周期单位 如：day,month,year
		/// </summary>
		public string CycleUnit
		{
			set{ _cycleunit=value;}
			get{return _cycleunit;}
		}
		/// <summary>
		/// 限制次数
		/// </summary>
		public int MaxTimes
		{
			set{ _maxtimes=value;}
			get{return _maxtimes;}
		}
		/// <summary>
		/// 目标ID
		/// </summary>
		public int TargetId
		{
			set{ _targetid=value;}
			get{return _targetid;}
		}
		/// <summary>
		/// 目标类型 0：表示商家，1：表示代理商
		/// </summary>
		public int TargetType
		{
			set{ _targettype=value;}
			get{return _targettype;}
		}
		#endregion Model

	}
}

