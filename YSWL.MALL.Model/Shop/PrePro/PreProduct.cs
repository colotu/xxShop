/**  版本信息模板在安装目录下，可自行修改。
* PreProduct.cs
*
* 功 能： N/A
* 类 名： PreProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/8/24 16:08:40   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.PrePro
{
	/// <summary>
	/// 预定
	/// </summary>
	[Serializable]
	public partial class PreProduct
	{
		public PreProduct()
		{}
		#region Model
		private int _preproid;
		private long _productid;
		private decimal _preamount;
		private DateTime _prestartdate;
		private DateTime _preenddate;
		private DateTime _buystartdate;
		private DateTime _buyenddate;
		private int _buycount;
		private int _limitqty=0;
		private int _regionid=-1;
		private int _status;
		private string _description;
		/// <summary>
		/// 预订ID
		/// </summary>
		public int PreProId
		{
			set{ _preproid=value;}
			get{return _preproid;}
		}
		/// <summary>
		/// 预订商品ID
		/// </summary>
		public long ProductId
		{
			set{ _productid=value;}
			get{return _productid;}
		}
		/// <summary>
		/// 订金
		/// </summary>
		public decimal PreAmount
		{
			set{ _preamount=value;}
			get{return _preamount;}
		}
		/// <summary>
		/// 预订开始时间
		/// </summary>
		public DateTime PreStartDate
		{
			set{ _prestartdate=value;}
			get{return _prestartdate;}
		}
		/// <summary>
		/// 预订结束时间
		/// </summary>
		public DateTime PreEndDate
		{
			set{ _preenddate=value;}
			get{return _preenddate;}
		}
		/// <summary>
		/// 购买开始时间
		/// </summary>
		public DateTime BuyStartDate
		{
			set{ _buystartdate=value;}
			get{return _buystartdate;}
		}
		/// <summary>
		/// 购买结束时间
		/// </summary>
		public DateTime BuyEndDate
		{
			set{ _buyenddate=value;}
			get{return _buyenddate;}
		}
		/// <summary>
		/// 已经预订数量
		/// </summary>
		public int BuyCount
		{
			set{ _buycount=value;}
			get{return _buycount;}
		}
		/// <summary>
		/// 单个商品限订最大数量
		/// </summary>
		public int LimitQty
		{
			set{ _limitqty=value;}
			get{return _limitqty;}
		}
		/// <summary>
		/// 支持预订地区ID -1 表示全地区
		/// </summary>
		public int RegionId
		{
			set{ _regionid=value;}
			get{return _regionid;}
		}
		/// <summary>
		/// 是否启用
		/// </summary>
		public int Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 活动说明
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		#endregion Model

        #region 扩展属性

	    public string ProductName;//产品名

	    public decimal SalePrice;//销售价

        public decimal MarketPrice;//市场价

	    public string ImageUrl;//图片

        public string ThumbnailUrl;//图片

	    #endregion
	}
}

