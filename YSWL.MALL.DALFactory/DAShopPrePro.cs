using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
    public sealed class DAShopPrePro : DataAccessBase
    {

        /// <summary>
        /// 创建PreOrder数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.PrePro.IPreOrder CreatePreOrder()
        {
            string ClassNamespace = AssemblyPath + ".Shop.PrePro.PreOrder";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.PrePro.IPreOrder)objType;
        }

        /// <summary>
        /// 创建PreProduct数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.PrePro.IPreProduct CreatePreProduct()
        {
            string ClassNamespace = AssemblyPath + ".Shop.PrePro.PreProduct";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.PrePro.IPreProduct)objType;
        }

    }

}
