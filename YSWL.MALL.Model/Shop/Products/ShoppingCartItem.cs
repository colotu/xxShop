/**
* ShoppingCarts.cs
*
* 功 能： N/A
* 类 名： ShoppingCarts
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/4/27 11:18:08   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.Products
{
    /// <summary>
    /// ShoppingCarts:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ShoppingCartItem : ShoppingCart.Model.CartItemInfo
    {
        public ShoppingCartItem()
        { }
        #region Model

        private string _unit;
        private string _saleDes;
        private string _description;

        private string[] _skuValues;
        private string _skuImageUrl;

        private decimal _adjustedprice;
        private string _attributes;
        private int _weight;
        private decimal? _deduct;
        private int _points;
        private int? _productlineid;
        private int? _supplierid;
        private string _suppliername;
        /// <summary>
        /// SKU值集合
        /// </summary>
        public string[] SkuValues
        {
            get { return _skuValues; }
            set { _skuValues = value; }
        }

        /// <summary>
        /// SKU图片URL
        /// </summary>
        public string SkuImageUrl
        {
            get { return _skuImageUrl; }
            set { _skuImageUrl = value; }
        }

        /// <summary>
        /// 促销说明
        /// </summary>
        public string SaleDes
        {
            get { return _saleDes; }
            set { _saleDes = value; }
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
        /// 调整后的价格
        /// </summary>
        public decimal AdjustedPrice
        {
            set { _adjustedprice = value; }
            get { return _adjustedprice; }
        }

        /// <summary>
        /// 属性
        /// </summary>
        public string Attributes
        {
            set { _attributes = value; }
            get { return _attributes; }
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

        # region 扩展属性

        private int _brandid;
        private string _brandName;

        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
        }

        public decimal SubTotal
        {
            get
            {
                return Quantity * SellPrice;
            }
        }
        public int BrandId
        {
            set { _brandid = value; }
            get { return _brandid; }
        }

        public string BrandName
        {
            set { _brandName = value; }
            get { return _brandName; }
        }

        int _restrictioncount;
        /// <summary>
        /// 限购数
        /// </summary>
        public int RestrictionCount
        {
            set { _restrictioncount = value; }
            get { return _restrictioncount; }
        }
        /// <summary>
        /// 推广ID
        /// </summary>
        private int _referId;

       public int ReferId
        {
            set { _referId = value; }
            get { return _referId; }
        }

       /// <summary>
       /// 推广类型
       /// </summary>
       private int _referType;

       public int ReferType
       {
           set { _referType = value; }
           get { return _referType; }
       }

       int _stock;
       /// <summary>
       /// 库存
       /// </summary>
       public int Stock
       {
           set { _stock = value; }
           get { return _stock; }
       }

       string _shopname;
       /// <summary>
       /// 店铺名称
       /// </summary>
       public string ShopName
       {
           set { _shopname = value; }
           get { return _shopname; }
       }

       int _salestatus;
       /// <summary>
       /// 状态  0:下架(仓库中)  1:上架 2:已删除
       /// </summary>
       public int SaleStatus
       {
           set { _salestatus = value; }
           get { return _salestatus; }
       }

        int _alertStock;
        /// <summary>
        ///警戒库存
        /// </summary>
        public int AlertStock
        {
            set { _alertStock = value; }
            get { return _alertStock; }
        }

        #endregion
    }
}

