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

namespace YSWL.MALL.ViewModel.Shop
{
   public class FavoStoreModel
    {
        #region Model
        private int _favoriteid;
        private DateTime _createddate;
        private int _supplierid;
        private string _shopName;
        private int _status;
        private int _storeStatus;
       private int _salesCount;
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
        /// 店铺Id
        /// </summary>
        public int SupplierId
        {
            set { _supplierid = value; }
            get { return _supplierid; }
        }

       /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName
        {            set { _shopName = value; }
            get { return _shopName; }
        }
        /// <summary>
        /// 销量
        /// </summary>
        public int SalesCount
        {
            set { _salesCount = value; }
            get { return _salesCount; }
        }
        /// <summary>
        /// 状态 
        /// </summary>
        public int  Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 店铺状态 
        /// </summary>
        public int StoreStatus
        {
            set { _storeStatus = value; }
            get { return _storeStatus; }
        }
        #endregion Model
 
 
    }
}
