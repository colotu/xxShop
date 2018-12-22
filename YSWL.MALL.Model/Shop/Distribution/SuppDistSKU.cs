/**  版本信息模板在安装目录下，可自行修改。
* SuppDistSKU.cs
*
* 功 能： N/A
* 类 名： SuppDistSKU
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/26 18:31:56   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace YSWL.MALL.Model.Shop.Distribution
{
	/// <summary>
	/// SuppDistSKU:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class SuppDistSKU
	{
		public SuppDistSKU()
		{}
        #region Model
        private string _sku;
        private long _productid;
        private int _weight = 0;
        private int _stock;
        private int _alertstock;
        private decimal _costprice = 0M;
        private decimal _saleprice;
        private bool _upselling;
        /// <summary>
        /// SKU值
        /// </summary>
        public string SKU
        {
            set { _sku = value; }
            get { return _sku; }
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
        /// 商品重量
        /// </summary>
        public int Weight
        {
            set { _weight = value; }
            get { return _weight; }
        }
        /// <summary>
        /// 库存
        /// </summary>
        public int Stock
        {
            set { _stock = value; }
            get { return _stock; }
        }
        /// <summary>
        /// 警戒库存
        /// </summary>
        public int AlertStock
        {
            set { _alertstock = value; }
            get { return _alertstock; }
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
        /// 销售价
        /// </summary>
        public decimal SalePrice
        {
            set { _saleprice = value; }
            get { return _saleprice; }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Upselling
        {
            set { _upselling = value; }
            get { return _upselling; }
        }
        #endregion Model

	}
}

