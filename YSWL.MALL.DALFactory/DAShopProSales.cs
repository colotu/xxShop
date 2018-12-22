using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
     public sealed class DAShopProSales : DataAccessBase
    {
        /// <summary>
        /// 创建CountDown数据层接口。
        /// </summary>
         public static YSWL.MALL.IDAL.Shop.PromoteSales.ICountDown CreateCountDown()
        {
            string ClassNamespace = AssemblyPath + ".Shop.PromoteSales.CountDown";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.PromoteSales.ICountDown)objType;
        }

         /// <summary>
         /// 创建GroupBuy数据层接口。
         /// </summary>
         public static YSWL.MALL.IDAL.Shop.PromoteSales.IGroupBuy CreateGroupBuy()
         {
             string ClassNamespace = AssemblyPath + ".Shop.PromoteSales.GroupBuy";
             object objType = CreateObject(AssemblyPath, ClassNamespace);
             return (YSWL.MALL.IDAL.Shop.PromoteSales.IGroupBuy)objType;
         }
    }
}
