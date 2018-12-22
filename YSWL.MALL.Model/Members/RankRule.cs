/**  版本信息模板在安装目录下，可自行修改。
* RankRule.cs
*
* 功 能： N/A
* 类 名： RankRule
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
	/// RankRule:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class RankRule
	{
		public RankRule()
		{}
		#region Model
		private int _ruleid;
		private int _actionid;
		private int _limitid;
		private string _name;
		private int _score;
		private string _description;
		private int _targetid=0;
		private int _targettype=0;
		/// <summary>
		/// 规则ID 
		/// </summary>
		public int RuleId
		{
			set{ _ruleid=value;}
			get{return _ruleid;}
		}
		/// <summary>
		/// 对应Action
		/// </summary>
		public int ActionId
		{
			set{ _actionid=value;}
			get{return _actionid;}
		}
		/// <summary>
		/// 限制条件ID
		/// </summary>
		public int LimitID
		{
			set{ _limitid=value;}
			get{return _limitid;}
		}
		/// <summary>
		/// 规则名称 如：登录
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 影响成长值
		/// </summary>
		public int Score
		{
			set{ _score=value;}
			get{return _score;}
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
		/// 目标ID 商家或者代理商ID
		/// </summary>
		public int TargetId
		{
			set{ _targetid=value;}
			get{return _targetid;}
		}
		/// <summary>
		/// 目标类型 1：表示商家，2：表示代理商
		/// </summary>
		public int TargetType
		{
			set{ _targettype=value;}
			get{return _targettype;}
		}
		#endregion Model

	}
}

