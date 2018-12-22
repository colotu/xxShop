/**  版本信息模板在安装目录下，可自行修改。
* CommissionRule.cs
*
* 功 能： N/A
* 类 名： CommissionRule
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/2/13 13:59:35   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;

namespace YSWL.MALL.Model.Shop.Commission
{
	/// <summary>
	/// CommissionRule:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CommissionRule
	{
		public CommissionRule()
		{}
        #region Model
		private int _ruleid;
		private string _rulename;
		private int _rulemode;
		private decimal _firstvalue;
		private decimal _secondvalue;
		private decimal _thirdvalue;
		private decimal _fourthvalue;
		private decimal _fifthvalue;
		private bool _isall;
		private int _status;
		private DateTime _createddate;
		private int _createduserid;
		/// <summary>
		/// 佣金规则ID
		/// </summary>
		public int RuleId
		{
			set{ _ruleid=value;}
			get{return _ruleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RuleName
		{
			set{ _rulename=value;}
			get{return _rulename;}
		}
		/// <summary>
		/// 佣金计算方式  0：固定金额 1：百分比
		/// </summary>
		public int RuleMode
		{
			set{ _rulemode=value;}
			get{return _rulemode;}
		}
		/// <summary>
		/// 第一级佣金值
		/// </summary>
		public decimal FirstValue
		{
			set{ _firstvalue=value;}
			get{return _firstvalue;}
		}
		/// <summary>
		/// 第二级佣金值
		/// </summary>
		public decimal SecondValue
		{
			set{ _secondvalue=value;}
			get{return _secondvalue;}
		}
		/// <summary>
		/// 第三级佣金值
		/// </summary>
		public decimal ThirdValue
		{
			set{ _thirdvalue=value;}
			get{return _thirdvalue;}
		}
		/// <summary>
		/// 第四级佣金值
		/// </summary>
		public decimal FourthValue
		{
			set{ _fourthvalue=value;}
			get{return _fourthvalue;}
		}
		/// <summary>
		/// 第五级佣金值
		/// </summary>
		public decimal FifthValue
		{
			set{ _fifthvalue=value;}
			get{return _fifthvalue;}
		}
		/// <summary>
		/// 是否包括全部商品
		/// </summary>
		public bool IsAll
		{
			set{ _isall=value;}
			get{return _isall;}
		}
		/// <summary>
		/// 状态 0：不启用，1：启用
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
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
		/// 创建人
		/// </summary>
		public int CreatedUserID
		{
			set{ _createduserid=value;}
			get{return _createduserid;}
		}
		#endregion Model

        

	}
}

