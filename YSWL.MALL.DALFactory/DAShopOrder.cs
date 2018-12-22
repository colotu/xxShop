using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
    public sealed class DAShopOrder : DataAccessBase
    {
        /// <summary>
        /// 创建OrderAction数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Order.IOrderAction CreateOrderAction()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Order.OrderAction";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Order.IOrderAction)objType;
        }

        /// <summary>
        /// 创建OrderItem数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Order.IOrderItems CreateOrderItem()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Order.OrderItems";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Order.IOrderItems)objType;
        }

        /// <summary>
        /// 创建OrderLookupItems数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Order.IOrderLookupItems CreateOrderLookupItems()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Order.OrderLookupItems";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Order.IOrderLookupItems)objType;
        }


        /// <summary>
        /// 创建OrderLookupList数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Order.IOrderLookupList CreateOrderLookupList()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Order.OrderLookupList";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Order.IOrderLookupList)objType;
        }

        /// <summary>
        /// 创建OrderOptions数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Order.IOrderOptions CreateOrderOptions()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Order.OrderOptions";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Order.IOrderOptions)objType;
        }

        /// <summary>
        /// 创建OrderRemark数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Order.IOrderRemark CreateOrderRemark()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Order.OrderRemark";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Order.IOrderRemark)objType;
        }

        /// <summary>
        /// 创建OrderRemark数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Order.IOrders CreateOrders()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Order.Orders";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Order.IOrders)objType;
        }

        /// <summary>
        /// 创建OrderService数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Order.IOrderService CreateOrderService()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Order.OrderService";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Order.IOrderService)objType;
        }

        /// <summary>
        /// 创建OrdersHistory数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Order.IOrdersHistory CreateOrdersHistory()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Order.OrdersHistory";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Order.IOrdersHistory)objType;
        }

    }
}
