/**
* IOrderService.cs
*
* 功 能： Shop模块-订单相关 含多表事务操作接口
* 类 名： IOrderService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/05/20 18:35:05  Ben    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/


using System.Data;
using YSWL.MALL.Model.Shop.Order;
using System;
using System.Collections.Generic;
namespace YSWL.MALL.IDAL.Shop.Order
{
    /// <summary>
    /// Shop模块-订单相关 含多表事务操作接口
    /// </summary>
    public interface IOrderService
    {
        long CreateOrder(Model.Shop.Order.OrderInfo orderInfo,int depotId, Accounts.Bus.User currentUser = null);

        bool CancelOrder(Model.Shop.Order.OrderInfo orderInfo, Accounts.Bus.User currentUser = null);

        bool PayForOrder(Model.Shop.Order.OrderInfo orderInfo, Accounts.Bus.User currentUser = null);
        System.Data.DataSet Stat4OrderStatus(int orderStatus);
        System.Data.DataSet StatSales(Model.Shop.Order.StatisticMode mode, System.DateTime startDate, System.DateTime endDate, int? supplierId = null);
        System.Data.DataSet ProductSales(Model.Shop.Order.StatisticMode mode, System.DateTime startDate, System.DateTime endDate, int supplierId);
        System.Data.DataSet ProductSaleInfo(DateTime startDate, DateTime endDate, int topCount);
        System.Data.DataSet ShopSale(StatisticMode mode, DateTime startDate, DateTime endDate);
        System.Data.DataSet ShopSaleInfo(StatisticMode mode, DateTime startDate, DateTime endDate);
        int GetRecordCount(StatisticMode mode, DateTime startDate, DateTime endDate, string modes, int startIndex, int endIndex,int supplierId);
        System.Data.DataSet GetListByPage( StatisticMode mode, DateTime startDate, DateTime endDate,string modes, int startIndex, int endIndex,int supplierId);
        System.Data.DataSet Stat4OrderStatus(int orderStatus, DateTime startDate, DateTime endDate, int? supplierId = null);
        bool CompleteOrder(OrderInfo orderInfo,int userId=-1,string userName="",string usertype="AA");
        /// <summary>
        /// 更新订单状态-在途审核
        /// </summary>
        /// <param name="orderInfos"></param>
        /// <param name="shippingStatus"></param>
        /// <param name="orderStatus"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        bool UpdateOrderStatus(List<OrderInfo> orderInfos, YSWL.MALL.Model.Shop.Order.EnumHelper.ShippingStatus shippingStatus, YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus orderStatus, Accounts.Bus.User currentUser);

        /// <summary>
        /// 更新订单状态-打包中
        /// </summary>
        bool PackingOrder(Model.Shop.Order.OrderInfo orderInfo, Accounts.Bus.User currentUser);
        /// <summary>
        /// 库存不足自动下架
        /// </summary>
        bool AutoSoldOut(OrderInfo orderInfo);
        /// <summary>
        /// 订单统计
        /// </summary>
        /// <param name="paymentStatus">支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单</param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        System.Data.DataSet Stat4OrderStatus(int paymentStatus, DateTime startDate, DateTime endDate, int referType, int? supplierId = null);
        /// <summary>
        /// 客户活跃统计
        /// </summary>
        /// <param name="paymentStatus">支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单</param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        System.Data.DataSet ActiveCount(int paymentStatus, DateTime startDate, DateTime endDate, int referType, int? supplierId = null, StatisticMode mode = StatisticMode.Day);
        /// <summary>
        /// 客户活跃统计--来源类型
        /// </summary>
        /// <param name="paymentStatus">支付状态 -1显示全部| 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        System.Data.DataSet ActiveCountbyType(int paymentStatus, DateTime startDate, DateTime endDate, int? supplierId = null);

        /// <summary>
        /// 客户数量
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="feferType">来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单</param>
        /// <param name="paymentStatus">支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <returns></returns>
        int GetActiveCount(DateTime startDate, DateTime endDate, int referType, int? paymentStatus = null);
  
        /// <summary>
        /// 统计订单数和销售额
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        System.Data.DataSet StatOrderCountPrice(StatisticMode mode, DateTime startDate, DateTime endDate, int? supplierId = null);
        /// <summary>
        /// 品牌排行
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="topCount">前几条</param>
        /// <returns></returns>
        System.Data.DataSet BrandSaleInfo(DateTime startDate, DateTime endDate, int topCount);


        DataSet CancleData(string startDate, string endDate, int referType);

        DataSet GetNoBindData(DateTime startDate, DateTime endDate);

        DataSet GetErrBindData(DateTime startDate, DateTime endDate);

        DataSet SalesNewCustoms(DateTime startDay, DateTime endDay);

        int GetTotalCustoms(DateTime startDay, DateTime endDay);

        DataSet SalesActiveCount(DateTime startDay, DateTime endDay);

        DataSet GetSalesCount(int SalesId, string startDay, string endDay);

        DataSet GetOrderSales(int SalesId, string startDay, string endDay, int dateType = 0);

        DataSet GetShipsCount(int modeId, string startDay, string endDay, int type = 1);

        DataSet GetOrderShips(int modeId, string startDate, string endDate, int type = 1);

        int GetItemsCount(int modeId, string startDay, string endDay, int type = 1);

        DataSet GetItemsList(int modeId, string startDay, string endDay, int type = 1);

        #region   OMS API 调用方法
        DataSet GetPackOrderList(int delayMin, int depotId, bool isOpenMultiDepot);

        bool CheckOrder(long orderId, string orderCode, string depotName, int userId, string username);

        bool PackingOrder(long orderId);

         bool ShipOrder(long orderId, decimal freightAdjusted, decimal freightActual, string shipOrderNumber, string expressCompanyName, string expressCompanyAbb, int depotId, string depotName);


        bool CancelOrder(OrderInfo orderInfo, int userId, string userName, int depotId = -1);

        bool UpdateRemark(long orderId, string remark);

        #endregion

    }
}
