using System;
namespace YSWL.MALL.Model.Shop.Order
{
	/// <summary>
	/// OrderItem:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class OrderItems
	{
		public OrderItems()
		{}
        #region Model
        private long _itemid;
        private long _orderid;
        private string _ordercode;
        private long _productid;
        private string _productcode;
        private string _sku;
        private string _name;
        private string _thumbnailsurl;
        private string _description;
        private int _quantity;
        private int _shipmentquantity;
        private decimal _costprice;
        private decimal _sellprice;
        private decimal _adjustedprice;
        private string _attribute;
        private string _remark;
        private int _weight;
        private decimal? _deduct;
        private int _points;
        private int? _productlineid;
        private int? _supplierid;
        private string _suppliername;
        private int? _brandid;
        private string _brandname;
        private int _producttype = 1;
        private int _referid = 0;
        private int _refertype = 0;
        /// <summary>
        /// 订单项目ID
        /// </summary>
        public long ItemId
        {
            set { _itemid = value; }
            get { return _itemid; }
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderId
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 订单自定义单号
        /// </summary>
        public string OrderCode
        {
            set { _ordercode = value; }
            get { return _ordercode; }
        }
        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 商品条码
        /// </summary>
        public string ProductCode
        {
            set { _productcode = value; }
            get { return _productcode; }
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
        /// 商品名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 商品缩略图
        /// </summary>
        public string ThumbnailsUrl
        {
            set { _thumbnailsurl = value; }
            get { return _thumbnailsurl; }
        }
        /// <summary>
        /// 商品说明
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int Quantity
        {
            set { _quantity = value; }
            get { return _quantity; }
        }
        /// <summary>
        /// 实际出货数量
        /// </summary>
        public int ShipmentQuantity
        {
            set { _shipmentquantity = value; }
            get { return _shipmentquantity; }
        }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice
        {
            set { _costprice = value; }
            get { return _costprice; }
        }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal SellPrice
        {
            set { _sellprice = value; }
            get { return _sellprice; }
        }
        /// <summary>
        /// 调整后的价格
        /// </summary>
        public decimal AdjustedPrice
        {
            set { _adjustedprice = value; }
            get { return _adjustedprice; }
        }
        /// <summary>
        /// 商品属性
        /// </summary>
        public string Attribute
        {
            set { _attribute = value; }
            get { return _attribute; }
        }
        /// <summary>
        /// 项目备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 商品重量
        /// </summary>
        public int Weight
        {
            set { _weight = value; }
            get { return _weight; }
        }
        /// <summary>
        /// 扣除金额
        /// </summary>
        public decimal? Deduct
        {
            set { _deduct = value; }
            get { return _deduct; }
        }
        /// <summary>
        /// 商品所赠的积分
        /// </summary>
        public int Points
        {
            set { _points = value; }
            get { return _points; }
        }
        /// <summary>
        /// 商品线ID
        /// </summary>
        public int? ProductLineId
        {
            set { _productlineid = value; }
            get { return _productlineid; }
        }
        /// <summary>
        /// 供货商ID
        /// </summary>
        public int? SupplierId
        {
            set { _supplierid = value; }
            get { return _supplierid; }
        }
        /// <summary>
        /// 供货商名称
        /// </summary>
        public string SupplierName
        {
            set { _suppliername = value; }
            get { return _suppliername; }
        }
        /// <summary>
        /// 品牌Id
        /// </summary>
        public int? BrandId
        {
            set { _brandid = value; }
            get { return _brandid; }
        }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName
        {
            set { _brandname = value; }
            get { return _brandname; }
        }
        /// <summary>
        /// 商品类型  1 正常购买    2赠送商品
        /// </summary>
        public int ProductType
        {
            set { _producttype = value; }
            get { return _producttype; }
        }
        /// <summary>
        /// 推广来源ID
        /// </summary>
        public int ReferId
        {
            set { _referid = value; }
            get { return _referid; }
        }
        /// <summary>
        /// 推广类型
        /// </summary>
        public int ReferType
        {
            set { _refertype = value; }
            get { return _refertype; }
        }

        private decimal _gwjf;
        /// <summary>
        /// 抵扣积分金额
        /// </summary>
        public decimal Gwjf
        {
            set { _gwjf = value; }
            get { return _gwjf; }
        }

        #endregion Model
    }
}

