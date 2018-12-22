using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
    public sealed class DAShopDisDepot : DataAccessBase
    {
        /// <summary>
        /// 创建Depot数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.DisDepot.IDepot CreateDepot()
        {
            string ClassNamespace = AssemblyPath + ".Shop.DisDepot.Depot";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.DisDepot.IDepot)objType;
        }


        /// <summary>
        /// 创建DepotProduct数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.DisDepot.IDepotProduct CreateDepotProduct()
        {
            string ClassNamespace = AssemblyPath + ".Shop.DisDepot.DepotProduct";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.DisDepot.IDepotProduct)objType;
        }


        /// <summary>
        /// 创建DepotProSKUs数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.DisDepot.IDepotProSKUs CreateDepotProSKUs()
        {
            string ClassNamespace = AssemblyPath + ".Shop.DisDepot.DepotProSKUs";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.DisDepot.IDepotProSKUs)objType;
        }


        /// <summary>
        /// 创建DepotRegion数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.DisDepot.IDepotRegion CreateDepotRegion()
        {
            string ClassNamespace = AssemblyPath + ".Shop.DisDepot.DepotRegion";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.DisDepot.IDepotRegion)objType;
        }

    }
}
