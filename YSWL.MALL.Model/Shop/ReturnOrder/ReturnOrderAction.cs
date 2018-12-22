/**  版本信息模板在安装目录下，可自行修改。
* ReturnOrderAction.cs
*
* 功 能： N/A
* 类 名： ReturnOrderAction
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/2 11:50:35   N/A    初版
* 负责人    hhy
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.ReturnOrder
{
	/// <summary>
	/// ReturnOrderAction:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ReturnOrderAction
	{
		public ReturnOrderAction()
		{}
		#region Model
		private long _actionid;
		private long _returnorderid;
		private string _returnordercode;
		private int _userid;
		private string _username;
		private string _actioncode;
		private DateTime _actiondate;
		private string _remark;
		/// <summary>
		/// 
		/// </summary>
		public long ActionId
		{
			set{ _actionid=value;}
			get{return _actionid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long ReturnOrderId
		{
			set{ _returnorderid=value;}
			get{return _returnorderid;}
		}
		/// <summary>
		/// 订单ID
		/// </summary>
		public string ReturnOrderCode
		{
			set{ _returnordercode=value;}
			get{return _returnordercode;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UserId
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 用户名称
		/// </summary>
		public string UserName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 操作名称
		/// </summary>
		public string ActionCode
		{
			set{ _actioncode=value;}
			get{return _actioncode;}
		}
		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime ActionDate
		{
			set{ _actiondate=value;}
			get{return _actiondate;}
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

