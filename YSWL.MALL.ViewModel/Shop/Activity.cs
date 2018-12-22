using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Shop
{
    //活动
    public class ActivityModel
    {
        #region 活动列表
        /// <summary>
        /// 满额送
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> Full { set; get; }
        /// <summary>
        /// 首单送
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> First { set; get; }
        /// <summary>
        /// 送优惠劵
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> Coupon { set; get; }
        /// <summary>
        /// 包邮
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Activity.ActivityInfo> FreeShipping { set; get; }
        #endregion
    }

    //活动赠送列表
    public class ActicityGiveList
    {
        /// <summary>
        /// 商家Id
        /// </summary>
        public int SupplierId { set; get; }

        #region 赠送列表
        /// <summary>
        /// 赠品列表
        /// </summary>
        public List<Model.Shop.Products.ProductInfo> ActProdList { set; get; }
        /// <summary>
        /// 赠送优惠劵列表
        /// </summary>
        public List<Model.Shop.Coupon.CouponRule> ActCouponList { set; get; }
        /// <summary>
        /// 无库存赠品列表
        /// </summary>
        public List<Model.Shop.Products.ProductInfo> NotStockActProdList { set; get; }
        #endregion
    }

    
}
