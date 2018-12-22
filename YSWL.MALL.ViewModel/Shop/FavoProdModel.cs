/**
* FavoriteProduct.cs
*
* 功 能： [N/A]
* 类 名： FavoriteProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/22 17:10:53  Rock    初版
*
* Copyright (c) 2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Shop
{
   public class FavoProdModel
    {
        #region Model
        private int _favoriteid;
        private DateTime _createddate;
        private long _productId;
        private string _productName;
        private int _saleStatus;
        private string _thumbnailUrl1;
        private decimal _lowestsaleprice;
        /// <summary>
        /// 
        /// </summary>
        public int FavoriteId
        {
            set { _favoriteid = value; }
            get { return _favoriteid; }
        }
         /// <summary>
        /// 收藏时间  默认值为当前日期
        /// </summary>
        public DateTime CreatedDate
        {
            set { _createddate = value; }
            get { return _createddate; }
        }
        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId
        {
            set { _productId = value; }
            get { return _productId; }
        }

       /// <summary>
        /// 产品名
        /// </summary>
        public string  ProductName
        {
            set { _productName = value; }
            get { return _productName; }
        }
       /// <summary>
        /// 状态 上下架货  删除 
        /// </summary>
        public int  SaleStatus
        {
            set { _saleStatus = value; }
            get { return _saleStatus; }
        }
       /// <summary>
        /// Image
        /// </summary>
        public string  ThumbnailUrl1
        {
            set { _thumbnailUrl1 = value; }
            get { return _thumbnailUrl1; }
        }
        #endregion Model

        /// <summary>
        /// 最低价
        /// </summary>
        public decimal LowestSalePrice
        {
            set { _lowestsaleprice = value; }
            get { return _lowestsaleprice; }
        }
        /// <summary>
        /// 市场价
        /// </summary>
        public decimal? MarketPrice;

        /// <summary>
        /// 供应商Id
        /// </summary>
        public int SupplierId;

        #region 订货促销规则形式
        public int ruleType = -1;//促销规则形式： -1：没有参与促销 0：打折  1：减价  2：直降
        #endregion

        /// <summary>
        /// 是否有SKU
        /// </summary>
        public bool HasSKU=false;
    }
}
