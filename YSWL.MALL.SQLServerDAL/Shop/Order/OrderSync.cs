/**
* OrderSync.cs
*
* 功 能： Shop模块-订单同步 跨库操作类
* 类 名： OrderSync
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/04/10 16:25:45  Ben    初版
*
* Copyright (c) 2014 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using YSWL.Common;
using YSWL.DBUtility;
using YSWL.MALL.Model.Shop.Order;

namespace YSWL.MALL.SQLServerDAL.Shop.Order
{
    /// <summary>
    /// Shop模块-订单同步 跨库操作类
    /// </summary>
    public class OrderSync : IDAL.Shop.Order.IOrderSync
    {
        #region 创建订单

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns>主订单Id</returns>
        public long CreateOrder(OrderInfo orderInfo, bool borrowEnable)
        {
            //using (SqlConnection connection =  DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            //{
            //    connection.Open();
            //    using (SqlTransaction transaction = connection.BeginTransaction())
            //    {
            //        object result;
            //        try
            //        {
            //            //DONE: 1.新增订单
            //            result = DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateOrderInfo(orderInfo), transaction);

            //            //加载订单主键
            //            orderInfo.OrderId = Globals.SafeLong(result.ToString(), -1);

            //            //DONE: 2.新增订单项目
            //            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateOrderItems(orderInfo), transaction);

            //            //DONE: 3.新增订单创建记录
            //            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateOrderAction(orderInfo), transaction);

            //            //DONE: 4.减少商品SKU库存
            //            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(CutSKUStock(orderInfo), transaction);

            //            //TODO: 5.增加Shop用户扩展表的订单数 Count+1

            //            //DONE: 6.新增已拆单的子订单数据
            //            if (orderInfo.SubOrders != null &&
            //                orderInfo.SubOrders.Count > 0)
            //            {
            //                foreach (OrderInfo subOrder in orderInfo.SubOrders)
            //                {
            //                    //加载主订单Id
            //                    subOrder.ParentOrderId = orderInfo.OrderId;
            //                    CreateSubOrder(subOrder, transaction);
            //                }
            //                //TODO: 7.或增加 主订单日志 拆单记录
            //            }
            //            transaction.Commit();
            //        }
            //        catch (SqlException)
            //        {
            //            transaction.Rollback();
            //            throw;
            //        }
            //    }
            //}
            return orderInfo.OrderId;
        }

        #endregion

    }
}