using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
    public class DAShopCoupon : DataAccessBase
    {
        /// <summary>
        /// 创建CouponInfo数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Coupon.ICouponInfo CreateCouponInfo()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Coupon.CouponInfo";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Coupon.ICouponInfo)objType;
        }

        /// <summary>
        /// 创建CouponClass数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Coupon.ICouponClass CreateCouponClass()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Coupon.CouponClass";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Coupon.ICouponClass)objType;
        }
        /// <summary>
        /// 创建CouponRule数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Coupon.ICouponRule CreateCouponRule()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Coupon.CouponRule";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Coupon.ICouponRule)objType;
        }

        /// <summary>
        /// 创建CouponHistory数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Coupon.ICouponHistory CreateCouponHistory()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Coupon.CouponHistory";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Coupon.ICouponHistory)objType;
        }

    }
}
