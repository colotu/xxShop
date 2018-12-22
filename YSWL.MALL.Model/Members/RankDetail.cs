/**  版本信息模板在安装目录下，可自行修改。
* RankDetail.cs
*
* 功 能： N/A
* 类 名： RankDetail
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
	/// RankDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class RankDetail
	{
		public RankDetail()
		{}
		#region Model
		private int _detailid;
		private int _ruleid;
		private int _userid;
		private int _score;
		private string _extdata;
		private string _description;
		private DateTime _createddate;
		private int _type;
		/// <summary>
		/// 流水帐号 ID
		/// </summary>
		public int DetailID
		{
			set{ _detailid=value;}
			get{return _detailid;}
		}
		/// <summary>
		/// 成长值操作ID
		/// </summary>
		public int RuleId
		{
			set{ _ruleid=value;}
			get{return _ruleid;}
		}
		/// <summary>
		/// 用户Id
		/// </summary>
		public int UserID
		{
			set{ _userid=value;}
			get{return _userid;}
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
		/// 附加数据 如：用户上传了附件的地址
		/// </summary>
		public string ExtData
		{
			set{ _extdata=value;}
			get{return _extdata;}
		}
		/// <summary>
		/// 积分详细
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
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
		/// 0:表示成长值的获得，1：表示成长值的消费
		/// </summary>
		public int Type
		{
			set{ _type=value;}
			get{return _type;}
		}
		#endregion Model

	}
}

