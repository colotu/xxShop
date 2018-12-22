/**
* GroupBuy.cs
*
* 功 能： N/A
* 类 名： GroupBuy
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/10/14 15:51:55   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.PromoteSales
{
	/// <summary>
	/// GroupBuy:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class GroupBuy
	{
		public GroupBuy()
		{}
        #region Model
        private int _groupbuyid;
        private long _productid;
        private int _sequence;
        private decimal _fineprice;
        private DateTime _startdate;
        private DateTime _enddate;
        private int _maxcount;
        private int _groupcount;
        private int _buycount;
        private decimal _price;
        private int _status;
        private string _description;
        private int _regionid;
	    private string _productname;
	    private string _productcategory;
	    private string _groupbuyimage;
	    private int _categoryId;
	    private string _categoryPath;
        private int _limitqty;
        /// <summary>
        /// 团购主键ID
        /// </summary>
        public int GroupBuyId
        {
            set { _groupbuyid = value; }
            get { return _groupbuyid; }
        }
	    public int RegionId
	    {
            set { _regionid = value; }
            get { return _regionid; }
	    }
        /// <summary>
        /// 团购商品ID
        /// </summary>
        public long ProductId
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Sequence
        {
            set { _sequence = value; }
            get { return _sequence; }
        }
        /// <summary>
        /// 违约金
        /// </summary>
        public decimal FinePrice
        {
            set { _fineprice = value; }
            get { return _fineprice; }
        }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime StartDate
        {
            set { _startdate = value; }
            get { return _startdate; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate
        {
            set { _enddate = value; }
            get { return _enddate; }
        }
        /// <summary>
        /// 限购数量
        /// </summary>
        public int MaxCount
        {
            set { _maxcount = value; }
            get { return _maxcount; }
        }
        /// <summary>
        /// 团购满足数量
        /// </summary>
        public int GroupCount
        {
            set { _groupcount = value; }
            get { return _groupcount; }
        }
        /// <summary>
        /// 已经购买数量
        /// </summary>
        public int BuyCount
        {
            set { _buycount = value; }
            get { return _buycount; }
        }
        /// <summary>
        /// 团购价格
        /// </summary>
        public decimal Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 活动状态 0：未审核 1 ：审核通过  2：活动结束
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 活动说明
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 商品名称
        /// </summary>
	    public  string ProductName
	    {
            set { _productname = value; }
            get { return _productname; }
	    }

        /// <summary>
        /// 商品分类
        /// </summary>
        public string ProductCategory
        {
            set { _productcategory = value; }
            get { return _productcategory; }
        }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string GroupBuyImage
        {
            set { _groupbuyimage = value; }
            get { return _groupbuyimage; }
        }
	    public int CategoryId
	    {
	        set { _categoryId = value; }
            get { return _categoryId; }
	    }
        public string  CategoryPath
        {
            set { _categoryPath = value; }
            get { return _categoryPath; }
        }
        //单个商品单次限购数量
        public int LimitQty
        {
            set { _limitqty = value; }
            get { return _limitqty; }
        }
        #endregion Model

	    /// <summary>
	    /// 原价格
	    /// </summary>
	    public decimal LowestSalePrice { set; get; }

	}
}

