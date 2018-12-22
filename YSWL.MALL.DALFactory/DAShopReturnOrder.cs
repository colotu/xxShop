using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
    public sealed class DAShopReturnOrder : DataAccessBase
    {
        /// <summary>
        /// 创建ReturnOrderAction数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.ReturnOrder.IReturnOrderAction CreateReturnOrderAction()
        {

            string ClassNamespace = AssemblyPath + ".Shop.ReturnOrder.ReturnOrderAction";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.ReturnOrder.IReturnOrderAction)objType;
        }


        /// <summary>
        /// 创建ReturnOrderItems数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.ReturnOrder.IReturnOrderItems CreateReturnOrderItems()
        {

            string ClassNamespace = AssemblyPath + ".Shop.ReturnOrder.ReturnOrderItems";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.ReturnOrder.IReturnOrderItems)objType;
        }


        /// <summary>
        /// 创建ReturnOrders数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.ReturnOrder.IReturnOrders CreateReturnOrders()
        {

            string ClassNamespace = AssemblyPath + ".Shop.ReturnOrder.ReturnOrders";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.ReturnOrder.IReturnOrders)objType;
        }


    }
}
