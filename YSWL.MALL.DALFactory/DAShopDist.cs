using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
    public class DAShopDist : DataAccessBase
    {
        /// <summary>
        /// 创建DistSuppRegion数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Distribution.ISuppDistRegion CreateSuppDistRegion()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Distribution.SuppDistRegion";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Distribution.ISuppDistRegion)objType;
        }

        /// <summary>
        /// 创建DistSuppSKU数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Distribution.ISuppDistSKU CreateSuppDistSKU()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Distribution.SuppDistSKU";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Distribution.ISuppDistSKU)objType;
        }

        /// <summary>
        /// 创建SuppDistProduct数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Distribution.ISuppDistProduct CreateSuppDistProduct()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Distribution.SuppDistProduct";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Distribution.ISuppDistProduct)objType;
        }
    }
}
