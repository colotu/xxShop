/**  版本信息模板在安装目录下，可自行修改。
* ActivityInfo.cs
*
* 功 能： N/A
* 类 名： ActivityInfo
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/6/10 22:26:32   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.Activity
{
	/// <summary>
	/// ActivityInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ActivityInfo
	{
		public ActivityInfo()
		{}

        #region Model
        private int _activityid;
        private int _ruleid;
        private int _buycategoryid = 0;
        private string _buycategoryname;
        private long? _buyproductid;
        private string _buyproductname;
        private string _buysku;
        private int _buycount;
        private int _cpruleid = 0;
        private string _cprulename;
        private long _productid;
        private string _productname;
        private string _sku;
        private decimal _saleprice;
        private decimal _limitprice;
        private decimal? _limitmaxprice;
         private int _count;
        private int _maxcount;
        private int _status;
        private DateTime _startdate;
        private DateTime _enddate;
        private int _createduserid;
        private DateTime _createddate;
        private int _supplierid = 0;
        /// <summary>
        /// 
        /// </summary>
        public int ActivityId
        {
            set { _activityid = value; }
            get { return _activityid; }
        }
        /// <summary>
        /// 规则Id
        /// </summary>
        public int RuleId
        {
            set { _ruleid = value; }
            get { return _ruleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BuyCategoryId
        {
            set { _buycategoryid = value; }
            get { return _buycategoryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyCategoryName
        {
            set { _buycategoryname = value; }
            get { return _buycategoryname; }
        }
        /// <summary>
        /// 购买指定的商品
        /// </summary>
        public long? BuyProductId
        {
            set { _buyproductid = value; }
            get { return _buyproductid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BuyProductName
        {
            set { _buyproductname = value; }
            get { return _buyproductname; }
        }
        /// <summary>
        /// 购买指定的商品SKU
        /// </summary>
        public string BuySKU
        {
            set { _buysku = value; }
            get { return _buysku; }
        }
        /// <summary>
        /// 购买指定商品的数量
        /// </summary>
        public int BuyCount
        {
            set { _buycount = value; }
            get { return _buycount; }
        }
        /// <summary>
        /// 指定优惠券规则
        /// </summary>
        public int CpRuleId
        {
            set { _cpruleid = value; }
            get { return _cpruleid; }
        }
        /// <summary>
        /// 优惠券 名称
        /// </summary>
        public string CpRuleName
        {
            set { _cprulename = value; }
            get { return _cprulename; }
        }
        /// <summary>
        /// 赠送商品Id
        /// </summary>
        public long ProductId
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductName
        {
            set { _productname = value; }
            get { return _productname; }
        }
        /// <summary>
        /// 商品SKU
        /// </summary>
        public string SKU
        {
            set { _sku = value; }
            get { return _sku; }
        }
        /// <summary>
        /// 促销总价
        /// </summary>
        public decimal SalePrice
        {
            set { _saleprice = value; }
            get { return _saleprice; }
        }
        /// <summary>
        /// 最低消费金额
        /// </summary>
        public decimal LimitPrice
        {
            set { _limitprice = value; }
            get { return _limitprice; }
        }
        /// <summary>
        /// 最高消费金额
        /// </summary>
        public decimal? LimitMaxPrice
        {
            set { _limitmaxprice = value; }
            get { return _limitmaxprice; }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            set { _count = value; }
            get { return _count; }
        }
        /// <summary>
        /// 限售总数量
        /// </summary>
        public int MaxCount
        {
            set { _maxcount = value; }
            get { return _maxcount; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 开始时间
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
        /// 创建人ID
        /// </summary>
        public int CreatedUserId
        {
            set { _createduserid = value; }
            get { return _createduserid; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate
        {
            set { _createddate = value; }
            get { return _createddate; }
        }
        /// <summary>
        /// 商家Id
        /// </summary>
        public int SupplierId
        {
            set { _supplierid = value; }
            get { return _supplierid; }
        }
        #endregion Model

        #region  扩展属性

	    public int Total;

	    #endregion


	}
}

