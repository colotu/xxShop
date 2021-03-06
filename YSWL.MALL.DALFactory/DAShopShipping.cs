﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.DALFactory
{
    public sealed class DAShopShipping : DataAccessBase
    {
        /// <summary>
        /// 创建ShippingAddress数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Shipping.IShippingAddress CreateShippingAddress()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Shipping.ShippingAddress";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Shipping.IShippingAddress)objType;
        }


        /// <summary>
        /// 创建ShippingPayment数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Shipping.IShippingPayment CreateShippingPayment()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Shipping.ShippingPayment";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Shipping.IShippingPayment)objType;
        }


        /// <summary>
        /// 创建ShippingType数据层接口。
        /// </summary>
        public static YSWL.MALL.IDAL.Shop.Shipping.IShippingType CreateShippingType()
        {
            string ClassNamespace = AssemblyPath + ".Shop.Shipping.ShippingType";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (YSWL.MALL.IDAL.Shop.Shipping.IShippingType)objType;
        }


        /// <summary>
        /// 创建ShippingRegions数据层接口。
        /// </summary>
        public static IDAL.Shop.Shipping.IShippingRegions CreateShippingRegions()
        {

            string ClassNamespace = AssemblyPath + ".Shop.Shipping.ShippingRegions";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (IDAL.Shop.Shipping.IShippingRegions)objType;
        }


        /// <summary>
        /// 创建ShippingRegionGroups数据层接口。
        /// </summary>
        public static IDAL.Shop.Shipping.IShippingRegionGroups CreateShippingRegionGroups()
        {

            string ClassNamespace = AssemblyPath + ".Shop.Shipping.ShippingRegionGroups";
            object objType = CreateObject(AssemblyPath, ClassNamespace);
            return (IDAL.Shop.Shipping.IShippingRegionGroups)objType;
        }
    }
}
