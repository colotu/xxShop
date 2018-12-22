/**
* OrderService.cs
*
* 功 能： Shop模块-订单相关 多表事务操作类
* 类 名： OrderService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/6/22 10:46:33  Ben    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using YSWL.Common;
using YSWL.DBUtility;
using YSWL.MALL.IDAL.Shop.Order;
using YSWL.MALL.Model.Shop.Order;
using System.Data;
using System.Collections.Generic;
using System;

namespace YSWL.MALL.SQLServerDAL.Shop.Order
{
    /// <summary>
    /// Shop模块-订单相关 多表事务操作类
    /// </summary>
    public class OrderService : IOrderService
    {
        #region 创建订单

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <returns>主订单Id</returns>
        public long CreateOrder(OrderInfo orderInfo,int depotId=-1, Accounts.Bus.User currentUser = null)
        {
            using (SqlConnection connection =  DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    object result;
                    try
                    {
                        //DONE: 1.新增订单
                        result = DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateOrderInfo(orderInfo), transaction);

                        //加载订单主键
                        orderInfo.OrderId = Globals.SafeLong(result, -1);

                        //DONE: 2.新增订单项目
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateOrderItems(orderInfo), transaction);

                        //DONE: 3.新增订单创建记录
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateOrderAction(orderInfo, currentUser), transaction);

                        //DONE: 4.减少商品SKU库存
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(CutSKUStock(orderInfo, depotId), transaction);

                        //TODO: 5.增加Shop用户扩展表的订单数 Count+1

                        //DONE: 6.新增已拆单的子订单数据
                        if (orderInfo.SubOrders != null &&
                            orderInfo.SubOrders.Count > 0)
                        {
                            foreach (OrderInfo subOrder in orderInfo.SubOrders)
                            {
                                //加载主订单Id
                                subOrder.ParentOrderId = orderInfo.OrderId;
                                if (!subOrder.SupplierId.HasValue || subOrder.SupplierId.Value <= 0) {
                                    subOrder.DepotId = orderInfo.DepotId;
                                    subOrder.DepotName = orderInfo.DepotName;
                                }
                                
                                CreateSubOrder(subOrder, transaction);
                            }
                            //TODO: 7.或增加 主订单日志 拆单记录
                        }

                        //DONE: 8.提交发票信息
                        if (orderInfo.HasChildren && orderInfo.SubOrders != null)
                        {
                          
                            foreach (OrderInfo subOrder in orderInfo.SubOrders)
                            {
                                if (subOrder.OrderOptions != null && subOrder.OrderOptions.Count > 0)
                                {
                                    DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(AddOrderOptions(subOrder), transaction);
                                }
                            }
                        }
                        else
                        {
                            if (orderInfo.OrderOptions != null && orderInfo.OrderOptions.Count > 0)
                            {
                                DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(AddOrderOptions(orderInfo), transaction);
                            }
                        }
                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        Log.LogHelper.AddErrorLog("客户代下单失败", ex.Message + "------" + ex.StackTrace);
                        transaction.Rollback();
                        //回滚之后把Id置为0,避免出现返回orderId有值，数据被回滚的情况
                        orderInfo.OrderId = 0;
                        throw;
                    }
                }
            }
            return orderInfo.OrderId;
        }

        #region 创建子订单(拆单)
        /// <summary>
        /// 创建子订单(拆单)
        /// </summary>
        /// <param name="subInfo">子订单信息</param>
        /// <param name="transaction">主订单事务</param>
        /// <returns>子订单Id</returns>
        public long CreateSubOrder(OrderInfo subInfo, SqlTransaction transaction)
        {
            object result;

            //DONE: 1.新增订单
            result = DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateOrderInfo(subInfo), transaction);

            //加载子订单主键
            subInfo.OrderId = Globals.SafeLong(result.ToString(), -1);

            //DONE: 2.新增订单项目
            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateOrderItems(subInfo), transaction);

            //DONE: 3.新增订单拆单记录
            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateOrderAction(subInfo), transaction);

            return subInfo.OrderId;
        }
        #endregion

        #region UpdateProductStock

        private List<CommandInfo> CutSKUStock(OrderInfo orderInfo,int depotId=-1)
        {
            string tableName = "PMS_SKUs";
            if (depotId > -1)
            {
                tableName = "Shop_DepotProSKUs_" + depotId;
            }
            List<CommandInfo> listComand = new List<CommandInfo>();
            foreach (Model.Shop.Order.OrderItems item in orderInfo.OrderItems)
            {
                if (depotId>0 )
                {
                    if (item.SupplierId > 0)//商家商品不走分仓库存
                    {
                        tableName = "PMS_SKUs";
                    }
                    else {
                        tableName = "Shop_DepotProSKUs_" + depotId;
                    }
                }

                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("update {0}  set Stock=Stock-@Stock", tableName);
                strSql.Append(" where SKU=@SKU");
                SqlParameter[] parameters =
                        {
                            new SqlParameter("@SKU", SqlDbType.NVarChar, 50),
                            new SqlParameter("@Stock", SqlDbType.Int, 4)
                        };
                parameters[0].Value = item.SKU;
                parameters[1].Value = item.Quantity;
                listComand.Add(new CommandInfo(strSql.ToString(), parameters));
            }
            return listComand;
        }

        #endregion

        #region GenerateOrderAction

        private List<CommandInfo> GenerateOrderAction(OrderInfo orderInfo,Accounts.Bus.User currentUser = null)
        {
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            strSql.Append("insert into OMS_OrderAction(");
            strSql.Append("OrderId,OrderCode,UserId,Username,ActionCode,ActionDate,Remark)");
            strSql.Append(" values (");
            strSql.Append("@OrderId,@OrderCode,@UserId,@Username,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] parameters =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                    new SqlParameter("@UserId", SqlDbType.Int, 4),
                    new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                    new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                    new SqlParameter("@ActionDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                };
            parameters[0].Value = orderInfo.OrderId;
            parameters[1].Value = orderInfo.OrderCode;
          
            //拆单日志处理
            //if (orderInfo.ParentOrderId == -1)
            //{
            if (currentUser != null)
            {
                parameters[2].Value = currentUser.UserID;
                parameters[3].Value =String.IsNullOrWhiteSpace(currentUser.TrueName)? currentUser.UserName:currentUser.TrueName ;
                parameters[4].Value = ((int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.AdminCreate); //orderInfo.ActionCode;
            }
            else {
                parameters[2].Value = orderInfo.BuyerID;
                parameters[3].Value = "客户";
                parameters[4].Value = ((int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.Create); //orderInfo.ActionCode;
            }
            parameters[5].Value = DateTime.Now;
            parameters[6].Value = "创建订单";
            //}
            //else
            //{
            //    parameters[2].Value = -1;
            //    parameters[3].Value = "系统";
            //    parameters[6].Value = "系统自动拆单";
            //}
            return new List<CommandInfo>
                {
                    new CommandInfo(strSql.ToString(), parameters,
                                    EffentNextType.ExcuteEffectRows)
                };
        }

        #endregion

        #region GenerateOrderItems

        private List<CommandInfo> GenerateOrderItems(OrderInfo orderInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            foreach (Model.Shop.Order.OrderItems model in orderInfo.OrderItems)
            {
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append("insert into OMS_OrderItems(");
                strSql.Append(
                    "OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,AdjustedPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType,ReferId,ReferType)");
                strSql.Append(" values (");
                strSql.Append(
                    "@OrderId,@OrderCode,@ProductId,@ProductCode,@SKU,@Name,@ThumbnailsUrl,@Description,@Quantity,@ShipmentQuantity,@CostPrice,@SellPrice,@AdjustedPrice,@Attribute,@Remark,@Weight,@Deduct,@Points,@ProductLineId,@SupplierId,@SupplierName,@BrandId,@BrandName,@ProductType,@ReferId,@ReferType)");
                strSql.Append(";select @@IDENTITY");

                #region SqlParameter
                SqlParameter[] parameters =
                    {
                        new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                        new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                        new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                        new SqlParameter("@ProductCode", SqlDbType.NVarChar, 50),
                        new SqlParameter("@SKU", SqlDbType.NVarChar, 200),
                        new SqlParameter("@Name", SqlDbType.NVarChar, 200),
                        new SqlParameter("@ThumbnailsUrl", SqlDbType.NVarChar, 300),
                        new SqlParameter("@Description", SqlDbType.NVarChar, 500),
                        new SqlParameter("@Quantity", SqlDbType.Int, 4),
                        new SqlParameter("@ShipmentQuantity", SqlDbType.Int, 4),
                        new SqlParameter("@CostPrice", SqlDbType.Money, 8),
                        new SqlParameter("@SellPrice", SqlDbType.Money, 8),
                        new SqlParameter("@AdjustedPrice", SqlDbType.Money, 8),
                        new SqlParameter("@Attribute", SqlDbType.Text),
                        new SqlParameter("@Remark", SqlDbType.Text),
                        new SqlParameter("@Weight", SqlDbType.Int, 4),
                        new SqlParameter("@Deduct", SqlDbType.Money, 8),
                        new SqlParameter("@Points", SqlDbType.Int, 4),
                        new SqlParameter("@ProductLineId", SqlDbType.Int, 4),
                        new SqlParameter("@SupplierId", SqlDbType.Int, 4),
                        new SqlParameter("@SupplierName", SqlDbType.NVarChar, 100),
                                                new SqlParameter("@BrandId", SqlDbType.Int, 4),
                        new SqlParameter("@BrandName", SqlDbType.NVarChar, 100),
                        	new SqlParameter("@ProductType", SqlDbType.SmallInt,2) ,
                            new SqlParameter("@ReferId", SqlDbType.Int,4) ,
                            new SqlParameter("@ReferType", SqlDbType.Int,4) 
                    };
                parameters[0].Value = orderInfo.OrderId;
                parameters[1].Value = orderInfo.OrderCode;
                parameters[2].Value = model.ProductId;
                parameters[3].Value = model.ProductCode;
                parameters[4].Value = model.SKU;
                parameters[5].Value = model.Name;
                parameters[6].Value = model.ThumbnailsUrl;
                parameters[7].Value = model.Description;
                parameters[8].Value = model.Quantity;
                parameters[9].Value = model.ShipmentQuantity;
                parameters[10].Value = model.CostPrice;
                parameters[11].Value = model.SellPrice;
                parameters[12].Value = model.AdjustedPrice;
                parameters[13].Value = model.Attribute;
                parameters[14].Value = model.Remark;
                parameters[15].Value = model.Weight;
                parameters[16].Value = model.Deduct;
                parameters[17].Value = model.Points;
                parameters[18].Value = model.ProductLineId;
                parameters[19].Value = model.SupplierId;
                parameters[20].Value = model.SupplierName;
                parameters[21].Value = model.BrandId;
                parameters[22].Value = model.BrandName;
                parameters[23].Value = model.ProductType;
                parameters[24].Value = model.ReferId;
                parameters[25].Value = model.ReferType;
                #endregion

                list.Add(new CommandInfo(strSql.ToString(),
                                         parameters, EffentNextType.ExcuteEffectRows));
            }
            return list;
        }

        #endregion

        #region GenerateOrderInfo

        public CommandInfo GenerateOrderInfo(OrderInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OMS_Orders(");
            strSql.Append("OrderCode,ParentOrderId,SourceOrderId,SourceType,OriginalId,CreateUserId,CreatedDate,UpdatedDate,BuyerID,BuyerName,BuyerEmail,BuyerCellPhone,RegionId,ShipRegion,ShipAddress,ShipZipCode,ShipName,ShipTelPhone,ShipCellPhone,ShipEmail,ShippingModeId,ShippingModeName,RealShippingModeId,RealShippingModeName,ShipperId,ShipperName,ShipperAddress,ShipperCellPhone,Freight,FreightAdjusted,FreightActual,Weight,ShippingStatus,ShipOrderNumber,ExpressCompanyName,ExpressCompanyAbb,PaymentTypeId,PaymentTypeName,PaymentGateway,PaymentStatus,RefundStatus,PayCurrencyCode,PayCurrencyName,PaymentFee,PaymentFeeAdjusted,GatewayOrderId,OrderTotal,OrderPoint,OrderCostPrice,OrderProfit,OrderOtherCost,OrderOptionPrice,DiscountName,DiscountAmount,DiscountAdjusted,DiscountValue,DiscountValueType,CouponCode,CouponName,CouponAmount,CouponValue,CouponValueType,ActivityName,ActivityFreeAmount,ActivityStatus,GroupBuyId,GroupBuyPrice,GroupBuyStatus,Amount,OrderType,OrderTypeSub,OrderStatus,SellerID,SellerName,SellerEmail,SellerCellPhone,CommentStatus,SupplierId,SupplierName,ReferID,ReferURL,ReferType,OrderIP,Remark,ProductTotal,HasChildren,IsReviews,IsFreeShipping,DepotId,DepotName,AssignUserId,AssignName,AssignDate,WaveId,WaveNumber,OrderSort,WaveStatus,TaskBatchId,BatchNumber,DistributionId,Gwjf,Wdbh,RemarkOne,RemarkTwo)");
            strSql.Append(" values (");
            strSql.Append("@OrderCode,@ParentOrderId,@SourceOrderId,@SourceType,@OriginalId,@CreateUserId,@CreatedDate,@UpdatedDate,@BuyerID,@BuyerName,@BuyerEmail,@BuyerCellPhone,@RegionId,@ShipRegion,@ShipAddress,@ShipZipCode,@ShipName,@ShipTelPhone,@ShipCellPhone,@ShipEmail,@ShippingModeId,@ShippingModeName,@RealShippingModeId,@RealShippingModeName,@ShipperId,@ShipperName,@ShipperAddress,@ShipperCellPhone,@Freight,@FreightAdjusted,@FreightActual,@Weight,@ShippingStatus,@ShipOrderNumber,@ExpressCompanyName,@ExpressCompanyAbb,@PaymentTypeId,@PaymentTypeName,@PaymentGateway,@PaymentStatus,@RefundStatus,@PayCurrencyCode,@PayCurrencyName,@PaymentFee,@PaymentFeeAdjusted,@GatewayOrderId,@OrderTotal,@OrderPoint,@OrderCostPrice,@OrderProfit,@OrderOtherCost,@OrderOptionPrice,@DiscountName,@DiscountAmount,@DiscountAdjusted,@DiscountValue,@DiscountValueType,@CouponCode,@CouponName,@CouponAmount,@CouponValue,@CouponValueType,@ActivityName,@ActivityFreeAmount,@ActivityStatus,@GroupBuyId,@GroupBuyPrice,@GroupBuyStatus,@Amount,@OrderType,@OrderTypeSub,@OrderStatus,@SellerID,@SellerName,@SellerEmail,@SellerCellPhone,@CommentStatus,@SupplierId,@SupplierName,@ReferID,@ReferURL,@ReferType,@OrderIP,@Remark,@ProductTotal,@HasChildren,@IsReviews,@IsFreeShipping,@DepotId,@DepotName,@AssignUserId,@AssignName,@AssignDate,@WaveId,@WaveNumber,@OrderSort,@WaveStatus,@TaskBatchId,@BatchNumber,@DistributionId,@Gwjf,@Wdbh,@RemarkOne,@RemarkTwo)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@ParentOrderId", SqlDbType.BigInt,8),
                    new SqlParameter("@SourceOrderId", SqlDbType.BigInt,8),
                    new SqlParameter("@SourceType", SqlDbType.Int,4),
                    new SqlParameter("@OriginalId", SqlDbType.BigInt,8),
                    new SqlParameter("@CreateUserId", SqlDbType.Int,4),
                    new SqlParameter("@CreatedDate", SqlDbType.DateTime),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime),
                    new SqlParameter("@BuyerID", SqlDbType.Int,4),
                    new SqlParameter("@BuyerName", SqlDbType.NVarChar,100),
                    new SqlParameter("@BuyerEmail", SqlDbType.NVarChar,100),
                    new SqlParameter("@BuyerCellPhone", SqlDbType.NVarChar,50),
                    new SqlParameter("@RegionId", SqlDbType.Int,4),
                    new SqlParameter("@ShipRegion", SqlDbType.NVarChar,300),
                    new SqlParameter("@ShipAddress", SqlDbType.NVarChar,300),
                    new SqlParameter("@ShipZipCode", SqlDbType.NVarChar,20),
                    new SqlParameter("@ShipName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ShipTelPhone", SqlDbType.NVarChar,50),
                    new SqlParameter("@ShipCellPhone", SqlDbType.NVarChar,50),
                    new SqlParameter("@ShipEmail", SqlDbType.NVarChar,100),
                    new SqlParameter("@ShippingModeId", SqlDbType.Int,4),
                    new SqlParameter("@ShippingModeName", SqlDbType.NVarChar,100),
                    new SqlParameter("@RealShippingModeId", SqlDbType.Int,4),
                    new SqlParameter("@RealShippingModeName", SqlDbType.NVarChar,100),
                    new SqlParameter("@ShipperId", SqlDbType.Int,4),
                    new SqlParameter("@ShipperName", SqlDbType.NVarChar,100),
                    new SqlParameter("@ShipperAddress", SqlDbType.NVarChar,300),
                    new SqlParameter("@ShipperCellPhone", SqlDbType.NVarChar,20),
                    new SqlParameter("@Freight", SqlDbType.Money,8),
                    new SqlParameter("@FreightAdjusted", SqlDbType.Money,8),
                    new SqlParameter("@FreightActual", SqlDbType.Money,8),
                    new SqlParameter("@Weight", SqlDbType.Int,4),
                    new SqlParameter("@ShippingStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@ShipOrderNumber", SqlDbType.NVarChar,50),
                    new SqlParameter("@ExpressCompanyName", SqlDbType.NVarChar,500),
                    new SqlParameter("@ExpressCompanyAbb", SqlDbType.NVarChar,500),
                    new SqlParameter("@PaymentTypeId", SqlDbType.Int,4),
                    new SqlParameter("@PaymentTypeName", SqlDbType.NVarChar,100),
                    new SqlParameter("@PaymentGateway", SqlDbType.NVarChar,50),
                    new SqlParameter("@PaymentStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@RefundStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@PayCurrencyCode", SqlDbType.NVarChar,20),
                    new SqlParameter("@PayCurrencyName", SqlDbType.NVarChar,20),
                    new SqlParameter("@PaymentFee", SqlDbType.Money,8),
                    new SqlParameter("@PaymentFeeAdjusted", SqlDbType.Money,8),
                    new SqlParameter("@GatewayOrderId", SqlDbType.NVarChar,100),
                    new SqlParameter("@OrderTotal", SqlDbType.Money,8),
                    new SqlParameter("@OrderPoint", SqlDbType.Int,4),
                    new SqlParameter("@OrderCostPrice", SqlDbType.Money,8),
                    new SqlParameter("@OrderProfit", SqlDbType.Money,8),
                    new SqlParameter("@OrderOtherCost", SqlDbType.Money,8),
                    new SqlParameter("@OrderOptionPrice", SqlDbType.Money,8),
                    new SqlParameter("@DiscountName", SqlDbType.NVarChar,200),
                    new SqlParameter("@DiscountAmount", SqlDbType.Money,8),
                    new SqlParameter("@DiscountAdjusted", SqlDbType.Money,8),
                    new SqlParameter("@DiscountValue", SqlDbType.Money,8),
                    new SqlParameter("@DiscountValueType", SqlDbType.SmallInt,2),
                    new SqlParameter("@CouponCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@CouponName", SqlDbType.NVarChar,100),
                    new SqlParameter("@CouponAmount", SqlDbType.Money,8),
                    new SqlParameter("@CouponValue", SqlDbType.Money,8),
                    new SqlParameter("@CouponValueType", SqlDbType.SmallInt,2),
                    new SqlParameter("@ActivityName", SqlDbType.NVarChar,200),
                    new SqlParameter("@ActivityFreeAmount", SqlDbType.Money,8),
                    new SqlParameter("@ActivityStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@GroupBuyId", SqlDbType.Int,4),
                    new SqlParameter("@GroupBuyPrice", SqlDbType.Money,8),
                    new SqlParameter("@GroupBuyStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@Amount", SqlDbType.Money,8),
                    new SqlParameter("@OrderType", SqlDbType.SmallInt,2),
                    new SqlParameter("@OrderTypeSub", SqlDbType.SmallInt,2),
                    new SqlParameter("@OrderStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@SellerID", SqlDbType.Int,4),
                    new SqlParameter("@SellerName", SqlDbType.NVarChar,100),
                    new SqlParameter("@SellerEmail", SqlDbType.NVarChar,100),
                    new SqlParameter("@SellerCellPhone", SqlDbType.NVarChar,50),
                    new SqlParameter("@CommentStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@SupplierId", SqlDbType.Int,4),
                    new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
                    new SqlParameter("@ReferID", SqlDbType.NVarChar,50),
                    new SqlParameter("@ReferURL", SqlDbType.NVarChar,200),
                    new SqlParameter("@ReferType", SqlDbType.Int,4),
                    new SqlParameter("@OrderIP", SqlDbType.NVarChar,50),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
                    new SqlParameter("@ProductTotal", SqlDbType.Money,8),
                    new SqlParameter("@HasChildren", SqlDbType.Bit,1),
                    new SqlParameter("@IsReviews", SqlDbType.Bit,1),
                    new SqlParameter("@IsFreeShipping", SqlDbType.Bit,1),
                    new SqlParameter("@DepotId", SqlDbType.Int,4),
                    new SqlParameter("@DepotName", SqlDbType.NVarChar,200),
                    new SqlParameter("@AssignUserId", SqlDbType.Int,4),
                    new SqlParameter("@AssignName", SqlDbType.NVarChar,50),
                    new SqlParameter("@AssignDate", SqlDbType.DateTime),
                    new SqlParameter("@WaveId", SqlDbType.BigInt,8),
                    new SqlParameter("@WaveNumber", SqlDbType.NVarChar,50),
                    new SqlParameter("@OrderSort", SqlDbType.Int,4),
                    new SqlParameter("@WaveStatus", SqlDbType.Int,4),
                    new SqlParameter("@TaskBatchId", SqlDbType.BigInt,8),
                    new SqlParameter("@BatchNumber", SqlDbType.NVarChar,50),
                    new SqlParameter("@DistributionId", SqlDbType.Int,4),
                    new SqlParameter("@Gwjf", SqlDbType.Decimal,9),
                    new SqlParameter("@Wdbh", SqlDbType.NVarChar,50),
                    new SqlParameter("@RemarkOne", SqlDbType.NVarChar,2000),
                    new SqlParameter("@RemarkTwo", SqlDbType.NVarChar,2000)};
            parameters[0].Value = model.OrderCode;
            parameters[1].Value = model.ParentOrderId;
            parameters[2].Value = model.SourceOrderId;
            parameters[3].Value = model.SourceType;
            parameters[4].Value = model.OriginalId;
            parameters[5].Value = model.CreateUserId;
            parameters[6].Value = model.CreatedDate;
            parameters[7].Value = model.UpdatedDate;
            parameters[8].Value = model.BuyerID;
            parameters[9].Value = model.BuyerName;
            parameters[10].Value = model.BuyerEmail;
            parameters[11].Value = model.BuyerCellPhone;
            parameters[12].Value = model.RegionId;
            parameters[13].Value = model.ShipRegion;
            parameters[14].Value = model.ShipAddress;
            parameters[15].Value = model.ShipZipCode;
            parameters[16].Value = model.ShipName;
            parameters[17].Value = model.ShipTelPhone;
            parameters[18].Value = model.ShipCellPhone;
            parameters[19].Value = model.ShipEmail;
            parameters[20].Value = model.ShippingModeId;
            parameters[21].Value = model.ShippingModeName;
            parameters[22].Value = model.RealShippingModeId;
            parameters[23].Value = model.RealShippingModeName;
            parameters[24].Value = model.ShipperId;
            parameters[25].Value = model.ShipperName;
            parameters[26].Value = model.ShipperAddress;
            parameters[27].Value = model.ShipperCellPhone;
            parameters[28].Value = model.Freight;
            parameters[29].Value = model.FreightAdjusted;
            parameters[30].Value = model.FreightActual;
            parameters[31].Value = model.Weight;
            parameters[32].Value = model.ShippingStatus;
            parameters[33].Value = model.ShipOrderNumber;
            parameters[34].Value = model.ExpressCompanyName;
            parameters[35].Value = model.ExpressCompanyAbb;
            parameters[36].Value = model.PaymentTypeId;
            parameters[37].Value = model.PaymentTypeName;
            parameters[38].Value = model.PaymentGateway;
            parameters[39].Value = model.PaymentStatus;
            parameters[40].Value = model.RefundStatus;
            parameters[41].Value = model.PayCurrencyCode;
            parameters[42].Value = model.PayCurrencyName;
            parameters[43].Value = model.PaymentFee;
            parameters[44].Value = model.PaymentFeeAdjusted;
            parameters[45].Value = model.GatewayOrderId;
            parameters[46].Value = model.OrderTotal;
            parameters[47].Value = model.OrderPoint;
            parameters[48].Value = model.OrderCostPrice;
            parameters[49].Value = model.OrderProfit;
            parameters[50].Value = model.OrderOtherCost;
            parameters[51].Value = model.OrderOptionPrice;
            parameters[52].Value = model.DiscountName;
            parameters[53].Value = model.DiscountAmount;
            parameters[54].Value = model.DiscountAdjusted;
            parameters[55].Value = model.DiscountValue;
            parameters[56].Value = model.DiscountValueType;
            parameters[57].Value = model.CouponCode;
            parameters[58].Value = model.CouponName;
            parameters[59].Value = model.CouponAmount;
            parameters[60].Value = model.CouponValue;
            parameters[61].Value = model.CouponValueType;
            parameters[62].Value = model.ActivityName;
            parameters[63].Value = model.ActivityFreeAmount;
            parameters[64].Value = model.ActivityStatus;
            parameters[65].Value = model.GroupBuyId;
            parameters[66].Value = model.GroupBuyPrice;
            parameters[67].Value = model.GroupBuyStatus;
            parameters[68].Value = model.Amount;
            parameters[69].Value = model.OrderType;
            parameters[70].Value = model.OrderTypeSub;
            parameters[71].Value = model.OrderStatus;
            parameters[72].Value = model.SellerID;
            parameters[73].Value = model.SellerName;
            parameters[74].Value = model.SellerEmail;
            parameters[75].Value = model.SellerCellPhone;
            parameters[76].Value = model.CommentStatus;
            parameters[77].Value = model.SupplierId;
            parameters[78].Value = model.SupplierName;
            parameters[79].Value = model.ReferID;
            parameters[80].Value = model.ReferURL;
            parameters[81].Value = model.ReferType;
            parameters[82].Value = model.OrderIP;
            parameters[83].Value = model.Remark;
            parameters[84].Value = model.ProductTotal;
            parameters[85].Value = model.HasChildren;
            parameters[86].Value = model.IsReviews;
            parameters[87].Value = model.IsFreeShipping;
            parameters[88].Value = model.DepotId;
            parameters[89].Value = model.DepotName;
            parameters[90].Value = model.AssignUserId;
            parameters[91].Value = model.AssignName;
            parameters[92].Value = DateTime.Now; ;
            parameters[93].Value = model.WaveId;
            parameters[94].Value = model.WaveNumber;
            parameters[95].Value = model.OrderSort;
            parameters[96].Value = model.WaveStatus;
            parameters[97].Value = model.TaskBatchId;
            parameters[98].Value = model.BatchNumber;
            parameters[99].Value = model.DistributionId;
            parameters[100].Value = model.Gwjf;

            parameters[101].Value = model.Wdbh;
            parameters[102].Value = model.RemrkOne;
            parameters[103].Value = model.RemrkTwo;
            return new CommandInfo(strSql.ToString(), parameters);
        }

        #endregion

        #region 自动下架
        public bool AutoSoldOut(OrderInfo orderInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"
 UPDATE P
 SET    P.SaleStatus = 0
 FROM   PMS_Products P
 WHERE  P.HasSKU = 0
        AND P.SaleStatus = 1
        AND EXISTS ( SELECT S.ProductId
                     FROM   PMS_SKUs S
                          , OMS_OrderItems O
                     WHERE  O.OrderId = @OrderId
                            AND S.SKU = O.SKU
                            AND S.ProductId = P.ProductId
                            AND Stock < 1 )
");
            SqlParameter[] parameters =
            {
                new SqlParameter("@OrderId", SqlDbType.BigInt)
            };
            parameters[0].Value = orderInfo.OrderId;
            return DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters) > 0;
        }
        #endregion

        #endregion

        #region 配货中

        public bool PackingOrder(Model.Shop.Order.OrderInfo orderInfo, Accounts.Bus.User currentUser)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            //更新订单状态
            //DONE: 更新子订单的状态为 已配货
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("UPDATE  OMS_Orders SET ShippingStatus=1, OrderStatus=1, UpdatedDate=@UpdatedDate");
            strSql2.Append(" where OrderId=@OrderId OR ParentOrderId=@OrderId");
            SqlParameter[] parameters2 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime)
                };
            parameters2[0].Value = orderInfo.OrderId;
            parameters2[1].Value = DateTime.Now;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            //添加操作记录
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("insert into OMS_OrderAction(");
            strSql3.Append("OrderId,OrderCode,UserId,Username,ActionCode,ActionDate,Remark)");
            strSql3.Append(" values (");
            strSql3.Append("@OrderId,@OrderCode,@UserId,@Username,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] parameters3 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                    new SqlParameter("@UserId", SqlDbType.Int, 4),
                    new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                    new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                    new SqlParameter("@ActionDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                };
            parameters3[0].Value = orderInfo.OrderId;
            parameters3[1].Value = orderInfo.OrderCode;
            parameters3[2].Value = currentUser != null ? currentUser.UserID : (orderInfo.SellerID.HasValue ? orderInfo.SellerID.Value : orderInfo.BuyerID);
            parameters3[3].Value = currentUser != null ? GetName(currentUser) : (orderInfo.SellerID.HasValue ? orderInfo.SellerName : "系统");
            parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemPacking;
            parameters3[5].Value = DateTime.Now;

            //DONE: 要区分 user/admin 取消
            if (currentUser != null)  //TODO: 如果出现另外一种可以操作订单的角色 那么此处就要多加一层判断
            {
                switch (currentUser.UserType)
                {
                    case "AA":
                        parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemPacking;
                        break;
                    case "SP":
                        parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SellerPacking;
                        break;
                    case "AG":
                        parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.AgentPacking;
                        break;
                }

            }
            parameters3[6].Value = "配货操作";
            cmd = new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            return rowsAffected > 0;
        }

        #endregion

        #region 取消订单

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool CancelOrder(OrderInfo orderInfo, Accounts.Bus.User currentUser = null)
        {
            int depotId = orderInfo.DepotId;
            string tableName = "PMS_SKUs";
            bool isExists = true;
            if (depotId > 0)
            {
                tableName = "Shop_DepotProSKUs_" + depotId;

                //检查分仓商品表是否存在
                Shop.DisDepot.DepotProSKUs depskus=new DisDepot.DepotProSKUs ();
                if (!depskus.TabExists(tableName))
                {
                    isExists = false;
                }
            }
            
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //返回SKU库存
            if (orderInfo.OrderItems != null && orderInfo.OrderItems.Count > 0)
            {
                foreach (Model.Shop.Order.OrderItems item in orderInfo.OrderItems)
                {
                    if (depotId > 0)
                    {
                        if (item.SupplierId > 0)//商家商品不走分仓库存
                        {
                            tableName = "PMS_SKUs";
                        }
                        else
                        {
                            tableName = "Shop_DepotProSKUs_" + depotId;
                        }
                    }

                    if (tableName == string.Format("Shop_DepotProSKUs_{0}", depotId) && !isExists)
                    {
                        continue;
                    }
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendFormat("update {0}  set Stock=Stock+@Stock",tableName);
                    strSql.Append(" where SKU=@SKU");
                    SqlParameter[] parameters =
                        {
                            new SqlParameter("@SKU", SqlDbType.NVarChar, 50),
                            new SqlParameter("@Stock", SqlDbType.Int, 4)
                        };
                    parameters[0].Value = item.SKU;
                    parameters[1].Value = item.Quantity;
                    sqllist.Add(new CommandInfo(strSql.ToString(), parameters));
                }
            }

            //返回团购库存
            if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus == 1)
            {
                foreach (Model.Shop.Order.OrderItems item in orderInfo.OrderItems)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update Shop_GroupBuy set ");
                    strSql.Append("BuyCount=BuyCount-@BuyCount");
                    strSql.Append(" where GroupBuyId =@GroupBuyId ");
                    SqlParameter[] parameters = {
                    new SqlParameter("@BuyCount", SqlDbType.Int,4),
                        new SqlParameter("@GroupBuyId", SqlDbType.Int,4)
                                        };

                    parameters[0].Value = item.Quantity;
                    parameters[1].Value = orderInfo.GroupBuyId;
                    sqllist.Add(new CommandInfo(strSql.ToString(), parameters));
                }
            }
            //返回商品销售数
            //if (orderInfo.OrderStatus == 已支付)
            //{

            //}

            //更新订单状态
            //DONE: 更新子订单的状态为 已取消
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("UPDATE  OMS_Orders SET OrderStatus=-1, UpdatedDate=@UpdatedDate");
            strSql2.Append(" where OrderId=@OrderId OR ParentOrderId=@OrderId");
            SqlParameter[] parameters2 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime)
                };
            parameters2[0].Value = orderInfo.OrderId;
            parameters2[1].Value = DateTime.Now;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            //添加操作记录
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("insert into OMS_OrderAction(");
            strSql3.Append("OrderId,OrderCode,UserId,Username,ActionCode,ActionDate,Remark)");
            strSql3.Append(" values (");
            strSql3.Append("@OrderId,@OrderCode,@UserId,@Username,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] parameters3 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                    new SqlParameter("@UserId", SqlDbType.Int, 4),
                    new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                    new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                    new SqlParameter("@ActionDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                };
            parameters3[0].Value = orderInfo.OrderId;
            parameters3[1].Value = orderInfo.OrderCode;
            parameters3[2].Value = currentUser != null ? currentUser.UserID : orderInfo.BuyerID;
            parameters3[3].Value = currentUser != null ? (String.IsNullOrWhiteSpace(currentUser.NickName) ? currentUser.TrueName : currentUser.NickName) : orderInfo.BuyerName;
            parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemCancel; //orderInfo.ActionCode;
            parameters3[5].Value = DateTime.Now;

            //DONE: 要区分 user/admin 取消
            if (currentUser != null)  //TODO: 如果出现另外一种可以操作订单的角色 那么此处就要多加一层判断
            {
                switch (currentUser.UserType)
                {
                    case "AA":
                        parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemCancel;
                        break;
                    case "SP":
                        parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SellerCancel;
                        break;
                    case "UU":
                        parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.CustomersCancel;
                        break;
                    case "AG":
                        parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.AgentCancel;
                        break;
                }

            }
            parameters3[6].Value = "取消订单";
            cmd = new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

 
            #region 删除已赠送优惠劵
            long mainOrderId = orderInfo.OrderId;
            if (orderInfo.OrderType == 2)//是子单  
            {
                mainOrderId = orderInfo.ParentOrderId;
            }
            if (mainOrderId > 0)
            {
                #region 删除优惠劵
                StringBuilder strSql4 = new StringBuilder();
                strSql4.Append("   delete from Shop_CouponInfo   where UserId=@UserId");
                strSql4.AppendFormat(" and  OrderId={0} ", mainOrderId);
                SqlParameter[] parameters4 = {
                    new SqlParameter("@UserId", SqlDbType.Int, 4)
                };
                parameters4[0].Value = orderInfo.BuyerID;
                cmd = new CommandInfo(strSql4.ToString(), parameters4);
                sqllist.Add(cmd);
                #endregion

                #region 删除活动详情
                StringBuilder strSql5 = new StringBuilder();
                strSql5.Append("   delete from Shop_ActivityDetail   where RuleId=3  and  UserId=@UserId");
                strSql5.AppendFormat(" and  OrderId={0} ", mainOrderId);
                SqlParameter[] parameters5 = {
                    new SqlParameter("@UserId", SqlDbType.Int, 4)
                };
                parameters5[0].Value = orderInfo.BuyerID;
                cmd = new CommandInfo(strSql5.ToString(), parameters5);
                sqllist.Add(cmd);
                #endregion
            }
            #endregion

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            return rowsAffected > 0;
        }

        #endregion

        #region 支付订单

        /// <summary>
        /// 支付订单
        /// </summary>
        public bool PayForOrder(OrderInfo orderInfo, Accounts.Bus.User currentUser = null)
        {
            List<CommandInfo> listCommand = new List<CommandInfo>();
            DateTime updatedDate = DateTime.Now;

            #region 1.更新订单状态为 进行中 - 已支付

            //DONE: 1.更新订单状态为 进行中 - 已支付
            //DONE: 更新子订单的状态为 已支付
            StringBuilder sqlOrders = new StringBuilder();
            sqlOrders.Append("UPDATE  OMS_Orders SET  PaymentStatus=2, UpdatedDate=@UpdatedDate");
            sqlOrders.Append(" WHERE OrderId=@OrderId OR ParentOrderId=@OrderId");
            SqlParameter[] paramOrders =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime)
                };
            paramOrders[0].Value = orderInfo.OrderId;
            paramOrders[1].Value = updatedDate;
            listCommand.Add(new CommandInfo(sqlOrders.ToString(), paramOrders, EffentNextType.ExcuteEffectRows));

            #endregion

            #region 2.新增订单操作记录

            //DONE: 2.新增订单操作记录
            StringBuilder sqlOrderAction = new StringBuilder();
            sqlOrderAction.Append("insert into OMS_OrderAction(");
            sqlOrderAction.Append("OrderId,OrderCode,UserId,Username,ActionCode,ActionDate,Remark)");
            sqlOrderAction.Append(" values (");
            sqlOrderAction.Append("@OrderId,@OrderCode,@UserId,@Username,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] paramOrderAction =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                    new SqlParameter("@UserId", SqlDbType.Int, 4),
                    new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                    new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                    new SqlParameter("@ActionDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                };
            paramOrderAction[0].Value = orderInfo.OrderId;
            paramOrderAction[1].Value = orderInfo.OrderCode;
            paramOrderAction[2].Value = currentUser != null ? currentUser.UserID : orderInfo.BuyerID;
            paramOrderAction[3].Value = "系统";
            paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemPay;
            //DONE: 要区分 user/admin 支付
            if (currentUser != null)
            {
                switch (currentUser.UserType)
                {
                    case "AA":
                        paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemPay;
                        break;
                    case "UU":
                        paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.CustomersPay;
                        break;
                    default:
                        paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemPay;
                        break;
                }

            }
            paramOrderAction[5].Value = updatedDate;
            paramOrderAction[6].Value = "支付订单"; //TODO: 需要记录实际操作人
            listCommand.Add(new CommandInfo(sqlOrderAction.ToString(), paramOrderAction, EffentNextType.ExcuteEffectRows));

            #endregion

            //DONE: 3.增加用户扩展表 积分 禁止执行 *)此功能移动到[完成订单]时

            //DONE: 4.新增积分记录 禁止执行 *)此功能移动到[完成订单]时

            #region 5.增加商品销售数 未启用 已在完成流程调用 禁止二次调用
            ////DONE: 5.增加商品销售数
            //if (orderInfo.OrderItems != null && orderInfo.OrderItems.Count > 0)
            //{
            //    foreach (Model.Shop.Order.OrderItems item in orderInfo.OrderItems)
            //    {
            //        StringBuilder strSql = new StringBuilder();
            //        strSql.Append("update PMS_Products SET SaleCounts=SaleCounts+@Stock");
            //        strSql.Append(" where ProductId=@ProductId");
            //        SqlParameter[] parameters =
            //            {
            //                new SqlParameter("@ProductId", SqlDbType.BigInt),
            //                new SqlParameter("@Stock", SqlDbType.Int, 4)
            //            };
            //        parameters[0].Value = item.ProductId;
            //        parameters[1].Value = item.Quantity;
            //        listCommand.Add(new CommandInfo(strSql.ToString(), parameters));
            //    }
            //}
            #endregion

            #region 子单操作
            if (orderInfo.HasChildren && orderInfo.SubOrders.Count > 0)
            {
                foreach (OrderInfo subOrder in orderInfo.SubOrders)
                {
                    #region 子单日志
                    paramOrderAction = new SqlParameter[]
                    {
                        new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                        new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                        new SqlParameter("@UserId", SqlDbType.Int, 4),
                        new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                        new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                        new SqlParameter("@ActionDate", SqlDbType.DateTime),
                        new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                    };
                    paramOrderAction[0].Value = subOrder.OrderId;
                    paramOrderAction[1].Value = subOrder.OrderCode;
                    paramOrderAction[2].Value = currentUser != null ? currentUser.UserID : orderInfo.BuyerID;
                    paramOrderAction[3].Value = "系统";
                    paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemPay;
                    //DONE: 要区分 user/admin 支付
                    if (currentUser != null)
                    {
                        switch (currentUser.UserType)
                        {
                            case "AA":
                                paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemPay;
                                break;
                            case "UU":
                                paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.CustomersPay;
                                break;
                            default:
                                paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemPay;
                                break;
                        }

                    }
                    paramOrderAction[5].Value = updatedDate;
                    paramOrderAction[6].Value = "支付订单"; //TODO: 需要记录实际操作人
                    listCommand.Add(new CommandInfo(sqlOrderAction.ToString(), paramOrderAction,
                        EffentNextType.ExcuteEffectRows));
                    #endregion

                    #region 子订单卖家操作
                    PayToSupplier(listCommand, updatedDate, subOrder);
                    #endregion
                }
            }
            #endregion
            else
            {
                #region 主订单卖家操作
                PayToSupplier(listCommand, updatedDate, orderInfo);
                #endregion
            }


            return DBHelper.DefaultDBHelper.ExecuteSqlTran(listCommand) > 0;
        }

        #region 卖家操作
        private static void PayToSupplier(List<CommandInfo> listCommand, DateTime updatedDate, OrderInfo orderInfo)
        {
            SysManage.ConfigSystem conf = new SysManage.ConfigSystem();
            int  supplierSettleType =Common.Globals.SafeInt(conf.GetValue("Shop_Supplier_SettleType"),0);
            // 0 订单支付金额    1 成本价
            StringBuilder sqlOrders;
            SqlParameter[] paramOrders;
            decimal supplierSettlementAmount ;
            if (supplierSettleType == 1)
            {
                supplierSettlementAmount = orderInfo.OrderCostPrice.HasValue ? orderInfo.OrderCostPrice.Value : 0;
            }else {
                supplierSettlementAmount = orderInfo.Amount;
            }

            if (orderInfo.SellerID.HasValue &&
                orderInfo.SellerID.Value > 0 &&
                supplierSettlementAmount > 0)
            {
                #region 增加卖家余额

                sqlOrders = new StringBuilder();
                sqlOrders.Append("UPDATE  Accounts_UsersExp SET Balance=Balance+@Balance");
                sqlOrders.Append(" WHERE UserID=@UserID");
                paramOrders = new SqlParameter[]
                        {
                            new SqlParameter("@Balance", SqlDbType.Money, 8),
                            new SqlParameter("@UserID", SqlDbType.Int, 4)
                        };
                paramOrders[0].Value = supplierSettlementAmount;
                paramOrders[1].Value = orderInfo.SellerID.Value;
                listCommand.Add(new CommandInfo(sqlOrders.ToString(), paramOrders,
                    EffentNextType.ExcuteEffectRows));

                #endregion

                #region 增加商家余额 暂未使用

                /**
                         * 商家余额保存到  Accounts_UsersExp 表, 仅商家所有者可以提现.
                         * 不保存到 Shop_Suppliers 表 原因:
                         * 1. 交易记录 UserId 共通模块 无商家Id
                         * 2. 提现流程 UserId 共通模块 无商家Id
                         * 3. Shop v1.9.5 基础上执行最小改动
                         */

                #endregion

                #region 卖家交易(收入)记录
                
                sqlOrders = new StringBuilder();
                sqlOrders.Append("insert into Pay_BalanceDetails(");
                sqlOrders.Append("UserId,TradeDate,TradeType,Income,Balance,Remark) ");
                //  Insert into Table2(a, c, d) select a,c,5 from Table1

                sqlOrders.Append("  ");
                //TODO:Sql2005语法兼容性Check BEN ADD 20131202
                sqlOrders.Append(
                    "select @UserId,@TradeDate,@TradeType,@Income, Balance ,@Remark FROM Accounts_UsersExp WITH (NOLOCK) WHERE UserID = @UserId");
                paramOrders = new SqlParameter[]
                        {
                            new SqlParameter("@UserId", SqlDbType.Int, 4),
                            new SqlParameter("@TradeDate", SqlDbType.DateTime),
                            new SqlParameter("@TradeType", SqlDbType.Int, 4),
                            new SqlParameter("@Income", SqlDbType.Money, 8),
                            new SqlParameter("@Remark", SqlDbType.NVarChar, 2000)
                        };
                paramOrders[0].Value = orderInfo.SellerID.Value;
                paramOrders[1].Value = updatedDate;
                paramOrders[2].Value = 1;
                paramOrders[3].Value = supplierSettlementAmount; //收入
                paramOrders[4].Value = string.Format("交易收入 订单号[{0}]", orderInfo.OrderCode); //备注
                listCommand.Add(new CommandInfo(sqlOrders.ToString(), paramOrders,
                    EffentNextType.ExcuteEffectRows));

                #endregion
            }
        }
        #endregion

        #endregion
        private CommandInfo AddSuppSalesCount(int quantity, int supplierId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql = new StringBuilder();
            strSql.AppendFormat(" update Shop_Suppliers set SalesCount=SalesCount+{0} where SupplierId = {1}  ", quantity, supplierId);
            return new CommandInfo(strSql.ToString(), null, EffentNextType.ExcuteEffectRows);
        }

        #region 完成订单
        public bool CompleteOrder(OrderInfo orderInfo, int userId=-1,string userName="",string userType="AA")
        {

            List<CommandInfo> listCommand = new List<CommandInfo>();
            DateTime updatedDate = DateTime.Now;

            #region 1.更新订单状态为 进行中 - 已支付

            //DONE: 1.更新订单状态为  已完成 - 已支付 - 已确认收货
            StringBuilder sqlOrders = new StringBuilder();
            sqlOrders.Append("UPDATE  OMS_Orders SET OrderStatus=2,PaymentStatus=2,ShippingStatus=3, UpdatedDate=@UpdatedDate");
            sqlOrders.Append(" WHERE OrderId=@OrderId OR ParentOrderId=@OrderId");
            SqlParameter[] paramOrders =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime)
                };
            paramOrders[0].Value = orderInfo.OrderId;
            paramOrders[1].Value = updatedDate;
            listCommand.Add(new CommandInfo(sqlOrders.ToString(), paramOrders, EffentNextType.ExcuteEffectRows));

            #endregion

            #region 2.新增订单操作记录

            //DONE: 2.新增订单操作记录
            StringBuilder sqlOrderAction = new StringBuilder();
            sqlOrderAction.Append("insert into OMS_OrderAction(");
            sqlOrderAction.Append("OrderId,OrderCode,UserId,Username,ActionCode,ActionDate,Remark)");
            sqlOrderAction.Append(" values (");
            sqlOrderAction.Append("@OrderId,@OrderCode,@UserId,@Username,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] paramOrderAction =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                    new SqlParameter("@UserId", SqlDbType.Int, 4),
                    new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                    new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                    new SqlParameter("@ActionDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                };
            paramOrderAction[0].Value = orderInfo.OrderId;
            paramOrderAction[1].Value = orderInfo.OrderCode;
            paramOrderAction[2].Value = userId;
            paramOrderAction[3].Value = string.IsNullOrWhiteSpace(userName)?"System":userName;
            paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SysComplete;
            //DONE: 要区分 user/admin 支付
            if (userId != -1)
            {
                switch (userType)
                {
                    case "AA":
                        paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SysComplete;
                        break;
                    case "AG":
                        paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.AgentComplete;
                        break;
                    case "SP":
                        paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SellerComplete;
                        break;
                    case "UU":
                        paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.Complete;
                        break;
                    default:
                        paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SysComplete;
                        break;
                }
            }
            paramOrderAction[5].Value = updatedDate;
            paramOrderAction[6].Value = "完成订单"; //TODO: 需要记录实际操作人
            listCommand.Add(new CommandInfo(sqlOrderAction.ToString(), paramOrderAction, EffentNextType.ExcuteEffectRows));

            #endregion

            #region 增加积分
            if (orderInfo.OrderPoint > 0)
            {
                #region 3.增加用户扩展表 积分
                //DONE: 3.增加用户扩展表 积分
                //TODO: 如果要[实现购买某件商品获得与商品价格一定比率的积分，比率可以通过配置表配置]功能
                //TODO: 请将[3][4]提取到BLL层执行, 并作积分比率计算
                StringBuilder sqlOrderPoint = new StringBuilder();
                sqlOrderPoint.Append("update Accounts_UsersExp SET ");
                sqlOrderPoint.Append(" Points=Points+@Points ");
                sqlOrderPoint.Append(" WHERE UserID=@UserID ");
                SqlParameter[] paramOrderPoint =
            {
                new SqlParameter("@Points", SqlDbType.Int, 4),
                new SqlParameter("@UserID", SqlDbType.Int, 4)
            };
                paramOrderPoint[0].Value = orderInfo.OrderPoint;
                paramOrderPoint[1].Value = orderInfo.BuyerID;
                listCommand.Add(new CommandInfo(sqlOrderPoint.ToString(), paramOrderPoint));
                #endregion

                #region 4.新增积分记录
                //DONE: 4.新增积分记录

                StringBuilder sqlPointDetail = new StringBuilder();
                sqlPointDetail.Append("insert into Accounts_PointsDetail(");
                sqlPointDetail.Append("RuleID,UserID,Score,ExtData,CurrentPoints,Description,CreatedDate,Type)");
                sqlPointDetail.Append(" values (");
                sqlPointDetail.Append("@RuleID,@UserID,@Score,@ExtData,0,@Description,@CreatedDate,@Type)");
                SqlParameter[] paramPointDetail =
            {
                new SqlParameter("@RuleID", SqlDbType.Int, 4),
                new SqlParameter("@UserID", SqlDbType.Int, 4),
                new SqlParameter("@Score", SqlDbType.Int, 4),
                new SqlParameter("@ExtData", SqlDbType.NVarChar),
                new SqlParameter("@Description", SqlDbType.NVarChar),
                new SqlParameter("@CreatedDate", SqlDbType.DateTime),
                new SqlParameter("@Type", SqlDbType.Int, 4)
            };
                paramPointDetail[0].Value = (int)YSWL.MALL.Model.Members.Enum.PointRule.Order;
                paramPointDetail[1].Value = orderInfo.BuyerID;
                paramPointDetail[2].Value = orderInfo.OrderPoint;
                paramPointDetail[3].Value = string.Empty;
                paramPointDetail[4].Value = string.Format("[订单完成] 订单号:{0}", orderInfo.OrderCode);
                paramPointDetail[5].Value = updatedDate;
                paramPointDetail[6].Value = 0;
                listCommand.Add(new CommandInfo(sqlPointDetail.ToString(), paramPointDetail));

                #endregion
            }
            #endregion

            #region 5.增加商品销售数
            //DONE: 5.增加商品销售数
            if (orderInfo.OrderItems != null && orderInfo.OrderItems.Count > 0)
            {
                foreach (Model.Shop.Order.OrderItems item in orderInfo.OrderItems)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update PMS_Products SET SaleCounts=SaleCounts+@Stock");
                    strSql.Append(" where ProductId=@ProductId");
                    SqlParameter[] parameters =
                        {
                            new SqlParameter("@ProductId", SqlDbType.BigInt),
                            new SqlParameter("@Stock", SqlDbType.Int, 4)
                        };
                    parameters[0].Value = item.ProductId;
                    parameters[1].Value = item.Quantity;
                    listCommand.Add(new CommandInfo(strSql.ToString(), parameters));
                }
            }
            #endregion


            long mainOrderId = 0;
            if (orderInfo.OrderType == 1)
            {//1.是主单  
                mainOrderId = orderInfo.OrderId;
            }else if (orderInfo.OrderType == 2 && !HasUnCompleteOrder(orderInfo.OrderId, orderInfo.ParentOrderId))
            {
                //是子单 并且  同级子单都已完成

                //1. 修改主单的状态为已完成
                #region 更改主单状态
                StringBuilder sqlMainOrders = new StringBuilder();
                sqlMainOrders.Append("UPDATE  OMS_Orders SET OrderStatus=2,PaymentStatus=2,ShippingStatus=3, UpdatedDate=@UpdatedDate");
                sqlMainOrders.Append(" WHERE OrderId=@OrderId OR ParentOrderId=@OrderId");
                SqlParameter[] paramMainOrders =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime)
                };
                paramMainOrders[0].Value = orderInfo.ParentOrderId;
                paramMainOrders[1].Value = updatedDate;
                listCommand.Add(new CommandInfo(sqlMainOrders.ToString(), paramMainOrders, EffentNextType.ExcuteEffectRows));
                #endregion

                //2.更改优惠劵的状态
                mainOrderId = orderInfo.ParentOrderId;
            }

            #region 更改优惠劵的状态   
            if (mainOrderId > 0)
            {
                StringBuilder strSql5 = new StringBuilder();
                strSql5.Append(" update Shop_CouponInfo set ");
                strSql5.Append(" Status=1 ");
                strSql5.AppendFormat(" where OrderId={0} ", mainOrderId);
                listCommand.Add(new CommandInfo(strSql5.ToString(), null));     
            } 
            #endregion


            #region 主子单操作
            int quantity = 0;
            if (orderInfo.HasChildren && orderInfo.SubOrders.Count > 0)
            {
                foreach (OrderInfo subOrder in orderInfo.SubOrders)
                {
                    #region 子单日志
                    paramOrderAction = new SqlParameter[]
                    {
                        new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                        new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                        new SqlParameter("@UserId", SqlDbType.Int, 4),
                        new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                        new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                        new SqlParameter("@ActionDate", SqlDbType.DateTime),
                        new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                    };
                    paramOrderAction[0].Value = subOrder.OrderId;
                    paramOrderAction[1].Value = subOrder.OrderCode;
                    paramOrderAction[2].Value = userId;
                    paramOrderAction[3].Value = string.IsNullOrWhiteSpace(userName) ? "System" : userName;
                    paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SysComplete;
                    //DONE: 要区分 user/admin 支付
                    if (userId != -1)
                    {
                        switch (userType)
                        {
                            case "AA":
                                paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SysComplete;
                                break;
                            case "AG":
                                paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.AgentComplete;
                                break;
                            case "SP":
                                paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SellerComplete;
                                break;
                            case "UU":
                                paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.Complete;
                                break;
                            default:
                                paramOrderAction[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SysComplete;
                                break;
                        }
                    }
                    paramOrderAction[5].Value = updatedDate;
                    paramOrderAction[6].Value = "完成订单"; //TODO: 需要记录实际操作人
                    listCommand.Add(new CommandInfo(sqlOrderAction.ToString(), paramOrderAction,
                        EffentNextType.ExcuteEffectRows));
                    #endregion

                    #region 子订单卖家操作 未启用 已在支付流程调用 禁止二次调用
                    //PayToSupplier(listCommand, updatedDate, subOrder);
                    #endregion

                    #region 增加商家售出商品数
                    if (subOrder.SupplierId.HasValue && subOrder.SupplierId.Value > 0 && subOrder.OrderItems != null)
                    {
                        quantity = 0;
                        subOrder.OrderItems.ForEach(o => quantity += o.Quantity);
                        listCommand.Add(AddSuppSalesCount(quantity, subOrder.SupplierId.Value));
                    }
                    #endregion
                }
            }
            #endregion
            else
            {
                #region 增加商家售出商品数
                if (orderInfo.SupplierId.HasValue && orderInfo.SupplierId.Value > 0 && orderInfo.OrderItems!=null)
                {
                    quantity = 0;
                    orderInfo.OrderItems.ForEach(o => quantity += o.Quantity);
                    listCommand.Add(AddSuppSalesCount(quantity, orderInfo.SupplierId.Value));
                }
                #endregion

                #region 主订单卖家操作 未启用 已在支付流程调用 禁止二次调用
                //PayToSupplier(listCommand, updatedDate, orderInfo);
                #endregion
            }

            return DBHelper.DefaultDBHelper.ExecuteSqlTran(listCommand) > 0;
        }
        #endregion

        #region 订单统计
        public DataSet Stat4OrderStatus(int orderStatus)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
                @"
SELECT COUNT(*) ToalQuantity
, SUM(O.Amount) ToalPrice
FROM    OMS_Orders O
WHERE   O.OrderStatus = {0} ", orderStatus);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public DataSet Stat4OrderStatus(int orderStatus, DateTime startDate, DateTime endDate, int? supplierId = null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select B.ToalPrice,B.ToalQuantity
                                from (select COUNT(*) ToalQuantity,SUM(O.Amount) ToalPrice
                                from OMS_Orders O
                                where O.OrderStatus ={0} and O.CreatedDate BETWEEN '{1}' AND '{2}' 
                                ", orderStatus, startDate, endDate);
            if (supplierId.HasValue && supplierId.Value > 0)
            {
                strSql.AppendFormat("  AND O.SupplierId = {0}", supplierId);
            }
            strSql.AppendFormat(@") B ");
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime)
            };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public DataSet StatSales(StatisticMode mode, DateTime startDate, DateTime endDate, int? supplierId = null)
        {
            int subLength = 8;
            string method;
            switch (mode)
            {
                case StatisticMode.Year:
                    subLength = 4;
                    method = "GET_GeneratedYear";
                    break;
                case StatisticMode.Month:
                    subLength = 6;
                    method = "GET_GeneratedMonth";
                    break;
                case StatisticMode.Day:
                    subLength = 8;
                    method = "GET_GeneratedDay";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
            @"
--销量/业绩走势图
SELECT  A.GeneratedDate AS GeneratedDate
      , CASE WHEN B.ToalQuantity IS NULL THEN 0
             ELSE B.ToalQuantity
        END AS ToalQuantity
      , CASE WHEN C.ToalPrice IS NULL THEN 0.00
             ELSE C.ToalPrice
        END AS ToalPrice
FROM    ( SELECT    *
          FROM      {0}(@StartDate, @EndDate)
        ) A
        LEFT JOIN ( SELECT  CONVERT(varchar({1}) , O.CreatedDate, 112 ) GeneratedDate
                          , SUM(I.Quantity) ToalQuantity
                    FROM    OMS_OrderItems I
                          , OMS_Orders O
                    WHERE   I.OrderId = O.OrderId ", method, subLength);
            if (supplierId.HasValue && supplierId.Value > 0)
            {
                strSql.AppendFormat("  AND O.SupplierId = {0}", supplierId);
            }
            strSql.AppendFormat(@" 
                            AND O.PaymentStatus = 2
                            AND O.OrderType = 1
                            AND O.CreatedDate BETWEEN @StartDate AND @EndDate 
                    GROUP BY CONVERT(varchar({0}) , O.CreatedDate, 112 )
                  ) B 
ON CONVERT(varchar({0}) , A.GeneratedDate, 112 ) = B.GeneratedDate
", subLength);
            strSql.AppendFormat(@"LEFT JOIN ( SELECT  CONVERT(varchar({0}) , O.CreatedDate, 112 ) GeneratedDate
                          , SUM(Amount) ToalPrice
                    FROM  OMS_Orders O ", subLength);
            strSql.Append(@" WHERE O.PaymentStatus = 2
                            AND O.OrderType = 1");
            if (supplierId.HasValue && supplierId.Value > 0)
            {
                strSql.AppendFormat("  AND O.SupplierId = {0}", supplierId);
            }
            strSql.AppendFormat(@" AND O.CreatedDate BETWEEN @StartDate AND @EndDate 
                    GROUP BY CONVERT(varchar({0}) , O.CreatedDate, 112 )
                  ) C
ON CONVERT(varchar({0}) , A.GeneratedDate, 112 ) =C.GeneratedDate 
", subLength);
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime)
            };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 统计订单数和销售额
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public DataSet StatOrderCountPrice(StatisticMode mode, DateTime startDate, DateTime endDate, int? supplierId = null)
        {
            int subLength = 8;
            string method;
            switch (mode)
            {
                case StatisticMode.Year:
                    subLength = 4;
                    method = "GET_GeneratedYear";
                    break;
                case StatisticMode.Month:
                    subLength = 6;
                    method = "GET_GeneratedMonth";
                    break;
                case StatisticMode.Day:
                    subLength = 8;
                    method = "GET_GeneratedDay";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
            @"
--订单数和销售额
SELECT  A.GeneratedDate AS GeneratedDate
      , CASE WHEN B.c IS NULL THEN 0
             ELSE B.c
        END AS OrderCount
      , CASE WHEN C.ToalAmount IS NULL THEN 0.00
             ELSE C.ToalAmount
        END AS ToalAmount
FROM    ( SELECT    *
          FROM      {0}(@StartDate, @EndDate)
        ) A
        LEFT JOIN (  SELECT  CONVERT(varchar({1}) , O.CreatedDate, 112 ) GeneratedDate
                          ,COUNT(1) c
                    FROM  OMS_Orders O
                    WHERE  O.OrderType = 1 and O.OrderStatus<>-1 ", method, subLength);
            if (supplierId.HasValue && supplierId.Value > 0)
            {
                strSql.AppendFormat("  AND O.SupplierId = {0}", supplierId);
            }
            strSql.AppendFormat(@" AND O.CreatedDate BETWEEN @StartDate AND @EndDate 
                    GROUP BY CONVERT(varchar({0}) , O.CreatedDate, 112 )
                  ) B 
ON CONVERT(varchar({0}) , A.GeneratedDate, 112 ) = B.GeneratedDate
", subLength);
            strSql.AppendFormat(@"LEFT JOIN ( SELECT  CONVERT(varchar({0}) , O.CreatedDate, 112 ) GeneratedDate
                          , SUM(Amount) ToalAmount
                    FROM  OMS_Orders O ", subLength);
            strSql.Append(@" WHERE O.OrderType = 1 and O.OrderStatus<>-1");
            if (supplierId.HasValue && supplierId.Value > 0)
            {
                strSql.AppendFormat("  AND O.SupplierId = {0}", supplierId);
            }
            strSql.AppendFormat(@" AND O.CreatedDate BETWEEN @StartDate AND @EndDate 
                    GROUP BY CONVERT(varchar({0}) , O.CreatedDate, 112 )
                  ) C
ON CONVERT(varchar({0}) , A.GeneratedDate, 112 ) =C.GeneratedDate 
", subLength);
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime)
            };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;

            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 订单统计
        /// </summary>
        /// <param name="paymentStatus">支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单</param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public DataSet Stat4OrderStatus(int paymentStatus, DateTime startDate, DateTime endDate, int referType, int? supplierId = null)
        {
            StringBuilder strSql = new StringBuilder();
            string date = "O.CreatedDate";
            //if(paymentStatus==0)
            //{
            //    date = "O.UpdatedDate";
            //}
            strSql.Append("select COUNT(*) ToalQuantity,SUM(O.Amount) ToalPrice  from OMS_Orders O  ");
            strSql.AppendFormat("  where  O.OrderType = 1 and O.ReferType ={0} and {1} BETWEEN '{2}' AND '{3}'  and O.OrderType=1 and O.OrderStatus<>-1", referType, date, startDate,
                endDate);
            if (paymentStatus >= 0)
            {
                strSql.AppendFormat("  and  O.PaymentStatus ={0}", paymentStatus);
            }
            if (supplierId.HasValue && supplierId.Value > 0)
            {
                strSql.AppendFormat("  AND O.SupplierId = {0}", supplierId);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 客户活跃统计
        /// </summary>
        /// <param name="paymentStatus">支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单</param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public DataSet ActiveCount(int paymentStatus, DateTime startDate, DateTime endDate, int referType, int? supplierId = null, StatisticMode mode = StatisticMode.Day)
        {
            StringBuilder strSql = new StringBuilder();
            string date = "CreatedDate";
            //if(paymentStatus==0)
            //{
            //    date = "UpdatedDate";
            //}

            switch (mode)
            {
                case StatisticMode.Month:
                    var startMonth = new DateTime(startDate.Year, startDate.Month, 1);
                    var endMonth = new DateTime(startDate.Year, endDate.Month, 1).AddMonths(1);
                    strSql.AppendFormat(@"select D, COUNT(BuyerID) BuyerCount from(    select distinct CONVERT(varchar(7),{0},111) D,BuyerID from OMS_Orders ", date);
                    strSql.AppendFormat(" where  OrderType = 1 and OrderStatus<>-1  and {0} BETWEEN '{1}' AND '{2}'", date, startMonth, endMonth);
                    break;
                case StatisticMode.Day:
                    strSql.AppendFormat(@"select D, COUNT(BuyerID) BuyerCount from(    
              select distinct CONVERT(varchar(12),{0},111) D,BuyerID from OMS_Orders ", date);
                    strSql.AppendFormat(" where  OrderType = 1 and OrderStatus<>-1  and {0} BETWEEN '{1}' AND '{2}'", date, startDate,
               endDate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }

            if (referType >= 0)
            {
                strSql.AppendFormat("  and  ReferType ={0}", paymentStatus);
            }
            if (paymentStatus >= 0)
            {
                strSql.AppendFormat("  and  PaymentStatus ={0}", paymentStatus);
            }
            if (supplierId.HasValue && supplierId.Value > 0)
            {
                strSql.AppendFormat("  and SupplierId = {0}", supplierId);
            }
            strSql.AppendFormat(" )t group by D order by D ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 客户活跃统计--来源类型
        /// </summary>
        /// <param name="paymentStatus">支付状态 -1显示全部| 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public DataSet ActiveCountbyType(int paymentStatus, DateTime startDate, DateTime endDate, int? supplierId = null)
        {
            StringBuilder strSql = new StringBuilder();
            string date = "CreatedDate";
            //if(paymentStatus==0)
            //{
            //    date = "UpdatedDate";
            //}
            strSql.Append(@"select  ReferType ,COUNT(BuyerID) BuyerCount from(
select distinct ReferType,BuyerID from OMS_Orders ");
            strSql.AppendFormat(" where OrderType = 1 and {0} BETWEEN '{1}' AND '{2}'", date, startDate,
                endDate);
            if (paymentStatus >= 0)
            {
                strSql.AppendFormat("  and  PaymentStatus ={0}", paymentStatus);
            }
            if (supplierId.HasValue && supplierId.Value > 0)
            {
                strSql.AppendFormat("  and SupplierId = {0}", supplierId);
            }
            strSql.AppendFormat(" )t group by ReferType");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }


        #endregion

        #region 商品销量
        public DataSet ProductSales(StatisticMode mode, DateTime startDate, DateTime endDate, int supplierId)
        {
            int subLength = 8;
            string method;
            switch (mode)
            {
                case StatisticMode.Year:
                    subLength = 4;
                    method = "GET_GeneratedYear";
                    break;
                case StatisticMode.Month:
                    subLength = 6;
                    method = "GET_GeneratedMonth";
                    break;
                case StatisticMode.Day:
                    subLength = 8;
                    method = "GET_GeneratedDay";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
            @"
--商品销量
SELECT  A.GeneratedDate AS GeneratedDate
      , CASE WHEN B.ToalQuantity IS NULL THEN 0
             ELSE B.ToalQuantity
        END AS ToalQuantity
FROM    ( SELECT    *
          FROM      {0}(@StartDate, @EndDate)
        ) A
        LEFT JOIN ( SELECT  CONVERT(varchar({1}) , O.CreatedDate, 112 ) GeneratedDate
                          , SUM(I.Quantity) ToalQuantity
                    FROM    OMS_OrderItems I
                          , OMS_Orders O
                    WHERE   I.OrderId = O.OrderId ", method, subLength);

            if (supplierId > 0)
            {
                strSql.AppendFormat(" and O.SupplierId={0}  ", supplierId);
            }
            else {
                strSql.Append(" AND O.OrderType = 1 ");
            }
            strSql.AppendFormat(@" 
                         AND  O.PaymentStatus =2  AND O.OrderStatus>=0  AND  O.CreatedDate BETWEEN @StartDate AND @EndDate 
                    GROUP BY CONVERT(varchar({0}) , O.CreatedDate, 112 )
                  ) B 
ON CONVERT(varchar({0}) , A.GeneratedDate, 112 ) = CONVERT(varchar({0}) , B.GeneratedDate, 112 ) 
", subLength);
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime)
            };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;

            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        #endregion

        #region 每种商品销量统计
        public DataSet ProductSaleInfo(DateTime startDate, DateTime endDate, int topCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
            @"
--商品销量排行统计列表
SELECT top {0}  CASE WHEN B.ProductID IS NULL THEN 0
             ELSE B.ProductID
        END AS Product
      , CASE WHEN B.ToalQuantity IS NULL THEN 0
             ELSE B.ToalQuantity
        END AS ToalQuantity
        ,P.ProductName
FROM    ( SELECT  I.ProductId ProductID
                          , SUM(I.Quantity) ToalQuantity
                    FROM    OMS_OrderItems I
                          , OMS_Orders O
                    WHERE   I.OrderId = O.OrderId ", topCount);

            strSql.AppendFormat(@" 
                           AND O.OrderType = 1  AND  O.PaymentStatus =2  AND O.CreatedDate BETWEEN @StartDate AND @EndDate 
                    GROUP BY I.ProductId
                  ) B 
 INNER JOIN PMS_Products P on P.ProductId = B.ProductID order by ToalQuantity desc
");
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime)
            };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;

            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }
        #endregion

        #region 店铺排行统计
        public DataSet ShopSale(StatisticMode mode, DateTime startDate, DateTime endDate)
        {
            int subLength = 8;
            string method;
            switch (mode)
            {
                case StatisticMode.Year:
                    subLength = 4;
                    method = "GET_GeneratedYear";
                    break;
                case StatisticMode.Month:
                    subLength = 6;
                    method = "GET_GeneratedMonth";
                    break;
                case StatisticMode.Day:
                    subLength = 8;
                    method = "GET_GeneratedDay";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
           @"
--店铺排行走势图
SELECT  A.GeneratedDate AS GeneratedDate
       ,B.SupplierID
       ,CASE WHEN Amount IS NULL THEN 0
             ELSE Amount
        END AS Amount
       ,P.Name
       ,P.ShopName
FROM    ( SELECT    *
          FROM      {0}(@StartDate, @EndDate)
        ) A
        LEFT JOIN ( SELECT  CONVERT(varchar({1}) , U.CreatedDate, 112 ) GeneratedDate
                          ,sum(SupplierId) SupplierId
                          ,sum(Amount) Amount
                    FROM    OMS_Orders U
                    ", method, subLength);
            strSql.AppendFormat(@" 
                            where U.CreatedDate BETWEEN @StartDate AND @EndDate 
                            and SupplierId!=-1
                            GROUP BY CONVERT(varchar({0}) , U.CreatedDate, 112 )
                  ) B 
ON CONVERT(varchar({0}) , A.GeneratedDate, 112 ) = CONVERT(varchar({0}) , B.GeneratedDate, 112 ) 
left join Shop_Suppliers P on P.SupplierId = B.SupplierId ", subLength);
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime)
            };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;

            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }
        #endregion

        #region 每种店铺统计
        public DataSet ShopSaleInfo(StatisticMode mode, DateTime startDate, DateTime endDate)
        {
            int subLength = 8;
            string method;
            switch (mode)
            {
                case StatisticMode.Year:
                    subLength = 4;
                    method = "GET_GeneratedYear";
                    break;
                case StatisticMode.Month:
                    subLength = 6;
                    method = "GET_GeneratedMonth";
                    break;
                case StatisticMode.Day:
                    subLength = 8;
                    method = "GET_GeneratedDay";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
           @"
--店铺排行走势图
SELECT B.SupplierID
       ,Amount as Amount
       ,P.Name
       ,P.ShopName 

 FROM  (SELECT  SupplierId ,sum(Amount) Amount
                    FROM    OMS_Orders U
                    ", method, subLength);
            strSql.AppendFormat(@" 
                            where    OrderStatus<>-1  and  U.CreatedDate BETWEEN @StartDate AND @EndDate 
                            and SupplierId>0
                            GROUP BY SupplierId
                  ) B 
left join Shop_Suppliers P on P.SupplierId = B.SupplierId order by Amount desc", subLength);
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime)
            };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;

            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }
        #endregion

        #region 商品销量排行统计

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int GetRecordCount(StatisticMode mode, DateTime startDate, DateTime endDate, string modes, int startIndex, int endIndex, int supplierId)
        {
            int subLength = 8;
            string method;
            switch (mode)
            {
                case StatisticMode.Year:
                    subLength = 4;
                    method = "GET_GeneratedYear";
                    break;
                case StatisticMode.Month:
                    subLength = 6;
                    method = "GET_GeneratedMonth";
                    break;
                case StatisticMode.Day:
                    subLength = 8;
                    method = "GET_GeneratedDay";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
            @"
--商品销量排行统计列表
select COUNT(*) from (
select * from (select ROW_NUMBER() over (order by T.ToalQuantity desc,T.GeneratedDate desc) as num,* from (
SELECT  A.GeneratedDate AS GeneratedDate
      , CASE WHEN B.ProductID IS NULL THEN 0
             ELSE B.ProductID
        END AS Product
      , CASE WHEN B.ToalQuantity IS NULL THEN 0
             ELSE B.ToalQuantity
        END AS ToalQuantity
        ,P.ProductName
FROM    ( SELECT    *
          FROM      {0}(@StartDate, @EndDate)
        ) A
        INNER JOIN ( SELECT  CONVERT(varchar({1}) , O.CreatedDate, 112 ) GeneratedDate
                          , I.ProductId ProductID
                          , SUM(I.Quantity) ToalQuantity
                    FROM    OMS_OrderItems I
                          , OMS_Orders O
                    WHERE   I.OrderId = O.OrderId  ", method, subLength);
            if (supplierId > 0)
            {
                strSql.AppendFormat(" and O.SupplierId={0}  ", supplierId);
            } else {
                strSql.Append(" AND O.OrderType = 1 ");
            }

            strSql.AppendFormat(@"  
                            AND  O.PaymentStatus =2 and O.OrderStatus>=0   AND O.CreatedDate BETWEEN @StartDate AND @EndDate 
                    GROUP BY CONVERT(varchar({0}) , O.CreatedDate, 112 ),I.ProductId
                  ) B 
ON CONVERT(varchar({0}) , A.GeneratedDate, 112 ) = CONVERT(varchar({0}) , B.GeneratedDate, 112 ) 
 INNER JOIN PMS_Products P on P.ProductId = B.ProductID  ) as T )as M)as N 
", subLength);
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime)
            };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }

        }
        
        public DataSet GetListByPage(StatisticMode mode, DateTime startDate, DateTime endDate, string modes, int startIndex, int endIndex, int supplierId)
        {
            int subLength = 8;
            string method;
            switch (mode)
            {
                case StatisticMode.Year:
                    subLength = 4;
                    method = "GET_GeneratedYear";
                    break;
                case StatisticMode.Month:
                    subLength = 6;
                    method = "GET_GeneratedMonth";
                    break;
                case StatisticMode.Day:
                    subLength = 8;
                    method = "GET_GeneratedDay";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
            @"
--商品销量排行统计列表

select * from (select ROW_NUMBER() over (order by T.ToalQuantity desc,T.GeneratedDate desc ) as num,* from (
SELECT  A.GeneratedDate AS GeneratedDate
      , CASE WHEN B.ProductID IS NULL THEN 0
             ELSE B.ProductID
        END AS Product
      , CASE WHEN B.ToalQuantity IS NULL THEN 0
             ELSE B.ToalQuantity
        END AS ToalQuantity
        ,P.ProductName
FROM    ( SELECT    *
          FROM      {0}(@StartDate, @EndDate)
        ) A
        INNER JOIN ( SELECT  CONVERT(varchar({1}) , O.CreatedDate, 112 ) GeneratedDate
                          , I.ProductId ProductID
                          , SUM(I.Quantity) ToalQuantity
                    FROM    OMS_OrderItems I
                          , OMS_Orders O
                    WHERE   I.OrderId = O.OrderId ", method, subLength);
            if (supplierId > 0)
            {
                strSql.AppendFormat(" and O.SupplierId={0}  ", supplierId);
            } else {
                strSql.Append(" AND O.OrderType = 1 ");
            }

            strSql.AppendFormat(@"  
                           AND  O.PaymentStatus =2 and O.OrderStatus>=0    AND O.CreatedDate BETWEEN @StartDate AND @EndDate 
                    GROUP BY CONVERT(varchar({0}) , O.CreatedDate, 112 ),I.ProductId
                  ) B 
ON CONVERT(varchar({0}) , A.GeneratedDate, 112 ) = CONVERT(varchar({0}) , B.GeneratedDate, 112 ) 
 INNER JOIN PMS_Products P on P.ProductId = B.ProductID  ) as T )as M where M.num between {1} and {2} 
", subLength, startIndex, endIndex);
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime)
            };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;

            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 品牌排行
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="topCount">前几条</param>
        /// <returns></returns>
        public DataSet BrandSaleInfo(DateTime startDate, DateTime endDate, int topCount)
        {
            StringBuilder strSql = new StringBuilder();


            strSql.AppendFormat(
            @"SELECT top {2} Br.BrandId,br.BrandName
                   ,CASE WHEN B.AdjustedPrice IS NULL THEN 0
                         ELSE B.AdjustedPrice
                    END AS AdjustedPrice
             FROM  (
             SELECT OI.BrandId ,sum(OI.AdjustedPrice) AdjustedPrice
                            FROM OMS_Orders O,OMS_OrderItems OI
                            where O.OrderId=OI.OrderId and O.OrderType = 1 and O.OrderStatus<>-1 and O.CreatedDate BETWEEN '{0}' AND '{1}' 
                            GROUP BY OI.BrandId
                              ) B 
            inner join dbo.PMS_Brands Br on Br.BrandId=B.BrandId
            order by AdjustedPrice desc", startDate, endDate, topCount);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        #endregion


        #region IOrderService 成员

        public bool UpdateOrderStatus(List<OrderInfo> orderInfos, EnumHelper.ShippingStatus shippingStatus, EnumHelper.OrderStatus orderStatus, Accounts.Bus.User currentUser)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region   未绑定订单数

        public DataSet GetNoBindData(DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(*) as   ToalQuantity,SUM(Amount)  as ToalPrice  from OMS_Orders    ");
            strSql.Append(" where   OrderType=1 and  OrderStatus<>-1 and  ReferID<=0   ");
            strSql.AppendFormat("  and  CreatedDate>'{0}' ", startDate);
            strSql.AppendFormat(" and  CreatedDate<'{0}' ", endDate);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public DataSet GetErrBindData(DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(*) as   ToalQuantity,SUM(Amount)  as ToalPrice  from OMS_Orders    ");
            strSql.Append(" where   OrderType=1 and  OrderStatus<>-1 and  ReferID>0   ");
            strSql.AppendFormat("  and  CreatedDate>'{0}' ", startDate);
            strSql.AppendFormat(" and  CreatedDate<'{0}' ", endDate);
            strSql.Append(" and not  Exists(select userId from Accounts_Users where UserType='SS' and ReferID=Accounts_Users.UserID)   ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        #endregion

        #region  新增客户数

        public DataSet SalesNewCustoms(DateTime startDay, DateTime endDay)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT   k.ReferID ,COUNT(1) AS Count     ");
            strSql.Append(" FROM     ( SELECT DISTINCT BuyerID , ReferID FROM      OMS_Orders N  WHERE     NOT EXISTS (     ");
            strSql.AppendFormat("  SELECT * FROM   OMS_Orders O WHERE  CreatedDate < '{0}'  AND OrderStatus <> -1 AND N.BuyerID = O.BuyerID ) ", startDay);
            strSql.AppendFormat("  AND OrderStatus <> -1  AND CreatedDate > '{0}' AND CreatedDate <= '{1}' AND ReferID > 0 ) k  GROUP BY k.ReferID ", startDay, endDay);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        #endregion

        #region 新增客户总数

        public int GetTotalCustoms(DateTime startDay, DateTime endDay)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  COUNT(1) AS Count     ");
            strSql.Append(" FROM     ( SELECT DISTINCT BuyerID , ReferID FROM      OMS_Orders N  WHERE    NOT EXISTS (     ");
            strSql.AppendFormat("  SELECT * FROM   OMS_Orders O WHERE  CreatedDate < '{0}'  AND OrderStatus <> -1 AND N.BuyerID = O.BuyerID ) ", startDay);
            strSql.AppendFormat("  AND OrderStatus <> -1  AND CreatedDate > '{0}' AND CreatedDate <= '{1}' AND ReferID > 0 ) K ", startDay, endDay);
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        #endregion


        #region  取消订单数
        public DataSet CancleData(string startDate, string endDate, int referType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(*) as   ToalQuantity,SUM(Amount)  as ToalPrice  from OMS_Orders    ");
            strSql.Append(" where   OrderType=1 and  OrderStatus=-1  ");
            if (referType > -1)
            {
                strSql.AppendFormat("  and  ReferType={0} ", referType);
            }
            if (!String.IsNullOrWhiteSpace(startDate))
            {
                strSql.AppendFormat("  and  UpdatedDate>'{0}' ", startDate);
            }
            if (!String.IsNullOrWhiteSpace(endDate))
            {
                strSql.AppendFormat(" and  UpdatedDate<'{0}' ", endDate);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        #endregion

        public DataSet SalesActiveCount(DateTime startDay, DateTime endDay)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT   k.ReferID ,COUNT(1) AS Count     ");
            strSql.Append(" FROM     ( SELECT  DISTINCT BuyerID,ReferID  FROM OMS_Orders     ");
            strSql.AppendFormat("  WHERE   OrderStatus <> -1 AND OrderType =1 AND CreatedDate >= '{0}' and CreatedDate <='{1}'  ", startDay, endDay);
            strSql.AppendFormat("  GROUP BY BuyerID ,ReferID ) k  GROUP BY k.ReferID ", startDay, endDay);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }


        /// <summary>
        /// 客户数量
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="feferType">来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单</param>
        /// <param name="paymentStatus">支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <returns></returns>
        public int GetActiveCount(DateTime startDate, DateTime endDate, int referType, int? paymentStatus = null)
        {
            StringBuilder strSql = new StringBuilder();
            //暂时全部根据订单创建时间统计。
            string date = "CreatedDate";
            //if (paymentStatus > 0)
            //{
            //    date = "UpdatedDate";
            //}
            strSql.AppendFormat(@"select COUNT(1) as Count from (select  DISTINCT  BuyerID from OMS_Orders where  {0} BETWEEN '{1}' AND '{2}'  and OrderType=1 and OrderStatus<>-1 ", date, startDate, endDate);
            if (referType > 0)
            {
                strSql.AppendFormat(" and referType = {0}", referType);
            }
            if (paymentStatus.HasValue && paymentStatus.Value >= 0)
            {
                strSql.AppendFormat(" and PaymentStatus = {0}", paymentStatus);
            }
            strSql.Append(" group by BuyerID) temp");

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

      

        /// <summary>
        /// 业务员业绩 （订单和销售额）
        /// </summary>
        /// <param name="SalesId"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        public DataSet GetSalesCount(int SalesId, string startDay, string endDay)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  COUNT(1) AS Count ,Sum(Amount)  As Amount   ");
            strSql.Append("     FROM OMS_Orders     ");
            strSql.AppendFormat("  WHERE   OrderStatus <> -1 AND OrderType =1 AND CreatedDate >= '{0}' and CreatedDate <='{1}'  ", startDay, endDay);
            strSql.AppendFormat("  And ReferId={0} ", SalesId);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public DataSet GetOrderSales(int SalesId, string startDate, string endDate, int dateType = 0)
        {
            int length = 12;
            switch (dateType)
            {
                case 0:
                    length = 12;
                    break;
                case 1:
                    length = 7;
                    break;
                default:
                    length = 12;
                    break;

            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select CONVERT(varchar({0}),CreatedDate,111) As D,COUNT(*)  As Count,Sum(Amount) As Amount from OMS_Orders ", length);
            strSql.Append(" where  OrderStatus <> -1 AND OrderType =1  ");
            if (!String.IsNullOrWhiteSpace(startDate) && YSWL.Common.PageValidate.IsDateTime(startDate))
            {
                strSql.AppendFormat("   AND CreatedDate >= '{0}'  ", startDate);
            }
            if (!String.IsNullOrWhiteSpace(endDate) && YSWL.Common.PageValidate.IsDateTime(endDate))
            {
                strSql.AppendFormat("  AND CreatedDate <= '{0}'  ", endDate);
            }

            strSql.AppendFormat("  And ReferId={0} ", SalesId);
            strSql.AppendFormat(" group by CONVERT(varchar({0}),CreatedDate,111) order by D desc", length);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        #region 加盟商业绩
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <param name="type">0:待配送，1：已配送，2：总计 </param>
        /// <returns></returns>
        public DataSet GetShipsCount(int modeId, string startDay, string endDay, int type = 1)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT  COUNT(1) AS Count ,Sum(Amount)  As Amount   ");
            strSql.Append("     FROM OMS_Orders    WHERE ");
            switch (type)
            {
                case 0:
                    strSql.Append("   ShippingStatus=2 and  OrderStatus<>-1 ");
                    break;
                case 1:
                    strSql.Append("     OrderStatus=2 ");
                    break;
                case 2:
                    strSql.Append("    ShippingStatus>=2  and  OrderStatus<>-1 ");
                    break;
                default:
                    break;
            }
            strSql.AppendFormat("       AND OrderType =1 AND UpdatedDate >= '{0}' and UpdatedDate <='{1}'  ", startDay, endDay);
            strSql.AppendFormat("  And ShippingModeId={0} ", modeId);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="type">0:待配送，1：已配送，2：总计 </param>
        /// <returns></returns>
        public DataSet GetOrderShips(int modeId, string startDate, string endDate, int type = 1)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select CONVERT(varchar(12),UpdatedDate,111) As D,COUNT(*)  As Count,Sum(Amount) As Amount from OMS_Orders ");
            strSql.Append("     where ");
            switch (type)
            {
                case 0:
                    strSql.Append("   ShippingStatus=2 and  OrderStatus<>-1 ");
                    break;
                case 1:
                    strSql.Append("     OrderStatus=2 ");
                    break;
                case 2:
                    strSql.Append("    ShippingStatus>=2  and  OrderStatus<>-1 ");
                    break; 
                default:
                    break;
            }
            strSql.AppendFormat("   AND OrderType =1 AND UpdatedDate >= '{0}' and UpdatedDate <='{1}' ", startDate, endDate);
            strSql.AppendFormat("  And ShippingModeId={0} ", modeId);
            strSql.AppendFormat(" group by CONVERT(varchar(12),UpdatedDate,111) order by D desc");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <param name="type">0:待配送，1：已配送，2：总计</param>
        /// <returns></returns>
        public int GetItemsCount(int modeId, string startDay, string endDay, int type = 1)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT SUM(Quantity)  FROM   OMS_OrderItems ");
            strSql.Append("  WHERE  EXISTS (  SELECT * FROM   OMS_Orders  WHERE    OrderType = 1  and ");
            switch (type)
            {
                case 0:
                    strSql.Append("   ShippingStatus=2 and  OrderStatus<>-1 ");
                    break;
                case 1:
                    strSql.Append("     OrderStatus=2 ");
                    break;
                case 2:
                    strSql.Append("    ShippingStatus>=2  and  OrderStatus<>-1 ");
                    break;
                default:
                    break;
            }
            strSql.AppendFormat("   AND UpdatedDate >= '{0}'  AND UpdatedDate <= '{1}' AND ShippingModeId = {2} ", startDay, endDay, modeId);
            strSql.Append("    AND OMS_OrderItems.OrderId = OMS_Orders.OrderId ) ");
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <param name="type">0:待配送，1：已配送，2：总计</param>
        /// <returns></returns>
        public DataSet GetItemsList(int modeId, string startDay, string endDay, int type = 1)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  SELECT CONVERT(VARCHAR(12), O.UpdatedDate, 111) AS D ,  SUM(Quantity) AS Count ");
            strSql.Append("   FROM   OMS_OrderItems I JOIN OMS_Orders O ON O.OrderId = I.OrderId   AND OrderType = 1  And ");
            switch (type)
            {
                case 0:
                    strSql.Append("   ShippingStatus=2 and  OrderStatus<>-1 ");
                    break;
                case 1:
                    strSql.Append("     OrderStatus=2 ");
                    break;
                case 2:
                    strSql.Append("    ShippingStatus>=2  and  OrderStatus<>-1 ");
                    break;
                default:
                    break;
            }
            strSql.AppendFormat("   AND UpdatedDate >= '{0}'  AND UpdatedDate <= '{1}' AND ShippingModeId = {2} ", startDay, endDay, modeId);
            strSql.Append("     GROUP BY CONVERT(VARCHAR(12), UpdatedDate, 111) ORDER BY D DESC ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        #endregion

        /// <summary>
        /// 根据子单id获取其他同级订单是否有未完成的（排除自己)   返回true:有未完成的   false:没有未完成的
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="parentOrderId">父订单Id</param>
        /// <returns></returns>
        private bool HasUnCompleteOrder(long orderId, long parentOrderId)
        {
              StringBuilder strSql = new StringBuilder();
              strSql.Append("select count(1)   from OMS_Orders  ");
              strSql.AppendFormat(" where  OrderStatus<2  and  ParentOrderId={0}  and orderid<> {1} ", parentOrderId,orderId);
              return DBHelper.DefaultDBHelper.Exists(strSql.ToString());
        }

        #region  OMS API  方法
        /// <summary>
        /// 获取备货数据方法
        /// </summary>
        /// <param name="delayMin">延迟多少分钟</param>
        /// <param name="depotId"></param>
        /// <param name="isOpenMultiDepot"></param>
        /// <returns></returns>
        public DataSet GetPackOrderList(int  delayMin, int depotId, bool isOpenMultiDepot)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  from OMS_Orders    ");
            strSql.AppendFormat("   WHERE OrderStatus<>-1  AND ShippingStatus=0  AND OrderTypeSub=1 and  CreatedDate<'{0}'", DateTime.Now.AddMinutes(-delayMin));
            if (isOpenMultiDepot)//如果开启了多仓库对应 需要根据收货地址自动过滤订单
            {

            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 审核订单
        /// </summary>
        /// <param name="orderId"></param>
        ///  <param name="orderCode"></param>
        /// <param name="depotName"></param>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool CheckOrder(long orderId,string orderCode, string depotName, int userId, string username)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //更新订单状态
            //DONE: 更新子订单的状态为 已配货
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("UPDATE  OMS_Orders SET ShippingStatus=0, OrderStatus=1, UpdatedDate=@UpdatedDate");
            strSql2.Append(" where OrderId=@OrderId OR ParentOrderId=@OrderId");
            SqlParameter[] parameters2 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime)
                };
            parameters2[0].Value = orderId;
            parameters2[1].Value = DateTime.Now;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            //添加操作记录
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("insert into OMS_OrderAction(");
            strSql3.Append("OrderId,OrderCode,UserId,Username,ActionCode,ActionDate,Remark)");
            strSql3.Append(" values (");
            strSql3.Append("@OrderId,@OrderCode,@UserId,@Username,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] parameters3 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                    new SqlParameter("@UserId", SqlDbType.Int, 4),
                    new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                    new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                    new SqlParameter("@ActionDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                };
            parameters3[0].Value = orderId;
            parameters3[1].Value = orderCode;
            parameters3[2].Value = userId;
            parameters3[3].Value = username;
            parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.Audited;
            parameters3[5].Value = DateTime.Now;
            parameters3[6].Value = "您的订单已经审核，被分配到【" + depotName + "】仓库";
            cmd = new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            return rowsAffected > 0;
        }
        /// <summary>
        /// OMS 接口返回备货操作
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool PackingOrder(long orderId)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            //更新订单状态
            //DONE: 更新子订单的状态为 已配货
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("UPDATE  OMS_Orders SET ShippingStatus=1, OrderStatus=1, UpdatedDate=@UpdatedDate");
            strSql2.Append(" where OrderId=@OrderId OR ParentOrderId=@OrderId");
            SqlParameter[] parameters2 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime)
                };
            parameters2[0].Value = orderId;
            parameters2[1].Value = DateTime.Now;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            //添加操作记录
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("delete from  OMS_OrderAction where OrderId=@OrderId and ActionCode=@ActionCode ;");
            strSql3.Append("insert into OMS_OrderAction(");
            strSql3.Append("OrderId,OrderCode,UserId,Username,ActionCode,ActionDate,Remark)");
            strSql3.Append(" select ");
            strSql3.Append("@OrderId,OrderCode,@UserId,@Username,@ActionCode,@ActionDate,@Remark from OMS_Orders where  OrderId=@OrderId");
            SqlParameter[] parameters3 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UserId", SqlDbType.Int, 4),
                    new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                    new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                    new SqlParameter("@ActionDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                };
            parameters3[0].Value = orderId;
            parameters3[1].Value = -1;
            parameters3[2].Value = "System";
            parameters3[3].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemPacking;
            parameters3[4].Value = DateTime.Now;
            parameters3[5].Value = "您的订单正在仓库配货分拣";
            cmd = new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            return rowsAffected > 0;
        }
        /// <summary>
        /// OMS 接口返回发货操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ShipOrder(long orderId, decimal freightAdjusted, decimal freightActual, string shipOrderNumber, string expressCompanyName, string expressCompanyAbb, int depotId, string depotName)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            #region 更新动作

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update OMS_Orders set ");
            strSql.Append("UpdatedDate=@UpdatedDate,");
            strSql.Append("FreightAdjusted=@FreightAdjusted,");
            strSql.Append("FreightActual=@FreightActual,");
            strSql.Append("ShippingStatus=@ShippingStatus,");
            strSql.Append("OrderStatus=@OrderStatus,");
            strSql.Append("ShipOrderNumber=@ShipOrderNumber,");
            strSql.Append("ExpressCompanyName=@ExpressCompanyName,");
            strSql.Append("ExpressCompanyAbb=@ExpressCompanyAbb,");
            strSql.Append("DepotId=@DepotId,");
            strSql.Append("DepotName=@DepotName");
            strSql.Append(" where OrderId=@OrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@UpdatedDate", SqlDbType.DateTime),
					new SqlParameter("@FreightAdjusted", SqlDbType.Money,8),
					new SqlParameter("@FreightActual", SqlDbType.Money,8),
					new SqlParameter("@ShippingStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@OrderStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@ShipOrderNumber", SqlDbType.NVarChar,50),
					new SqlParameter("@ExpressCompanyName", SqlDbType.NVarChar,500),
					new SqlParameter("@ExpressCompanyAbb", SqlDbType.NVarChar,500),
                    	new SqlParameter("@DepotId", SqlDbType.Int,4),
					new SqlParameter("@DepotName", SqlDbType.NVarChar,200),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = DateTime.Now;
            parameters[1].Value =freightAdjusted;
            parameters[2].Value =freightActual;
            parameters[3].Value =(int)YSWL.MALL.Model.Shop.Order.EnumHelper.ShippingStatus.Shipped;
            parameters[4].Value =(int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.Handling;
            parameters[5].Value =shipOrderNumber;
            parameters[6].Value =expressCompanyName;
            parameters[7].Value =expressCompanyAbb;
            parameters[8].Value =depotId;
            parameters[9].Value = depotName;
            parameters[10].Value =orderId;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            StringBuilder strSql6 = new StringBuilder();
            strSql6.Append("UPDATE OMS_OrderItems SET ShipmentQuantity=Quantity WHERE OrderId =@OrderId ");
            SqlParameter[] parameters6 = {
					new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
            parameters6[0].Value = orderId;
            cmd = new CommandInfo(strSql6.ToString(), parameters6);
            sqllist.Add(cmd);

            #endregion 更新动作

            //添加操作记录
            StringBuilder strSql3 = new StringBuilder();
            //删除重复操作的记录
            strSql3.Append("delete from  OMS_OrderAction where OrderId=@OrderId and ActionCode=@ActionCode ;");

            strSql3.Append("insert into OMS_OrderAction(");
            strSql3.Append("OrderId,OrderCode,UserId,Username,ActionCode,ActionDate,Remark)");
            strSql3.Append("   select @OrderId,OrderCode,@UserId,@Username,@ActionCode,@ActionDate,@Remark from OMS_Orders where  OrderId=@OrderId ");
            SqlParameter[] parameters3 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UserId", SqlDbType.Int, 4),
                    new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                    new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                    new SqlParameter("@ActionDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                };
            parameters3[0].Value = orderId;
            parameters3[1].Value = -1;
            parameters3[2].Value = "System";
            parameters3[3].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.Shipped;
            parameters3[4].Value = DateTime.Now;
            parameters3[5].Value = "您的订单在仓库【" + depotName + "】已经出库交付【" + expressCompanyName + "】，运单号为【" + shipOrderNumber + "】";
            cmd = new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);


            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        public bool CancelOrder(OrderInfo orderInfo, int userId,string userName,int depotId=-1)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            string tableName = "PMS_SKUs";
            if (depotId > -1)
            {
                tableName = "Shop_DepotProSKUs_" + depotId;
            }
            //返回SKU库存
            if (orderInfo.OrderItems != null && orderInfo.OrderItems.Count > 0)
            {
                
                foreach (Model.Shop.Order.OrderItems item in orderInfo.OrderItems)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.AppendFormat("update {0}  set Stock=Stock+@Stock", tableName);
                    strSql.Append(" where SKU=@SKU");
                    SqlParameter[] parameters =
                        {
                            new SqlParameter("@SKU", SqlDbType.NVarChar, 50),
                            new SqlParameter("@Stock", SqlDbType.Int, 4)
                        };
                    parameters[0].Value = item.SKU;
                    parameters[1].Value = item.Quantity;
                    sqllist.Add(new CommandInfo(strSql.ToString(), parameters));
                }
            }

            //返回团购库存
            if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus == 1)
            {
                foreach (Model.Shop.Order.OrderItems item in orderInfo.OrderItems)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update Shop_GroupBuy set ");
                    strSql.Append("BuyCount=BuyCount-@BuyCount");
                    strSql.Append(" where GroupBuyId =@GroupBuyId ");
                    SqlParameter[] parameters = {
                    new SqlParameter("@BuyCount", SqlDbType.Int,4),
                        new SqlParameter("@GroupBuyId", SqlDbType.Int,4)
                                        };

                    parameters[0].Value = item.Quantity;
                    parameters[1].Value = orderInfo.GroupBuyId;
                    sqllist.Add(new CommandInfo(strSql.ToString(), parameters));
                }
            }

            //更新订单状态
            //DONE: 更新子订单的状态为 已取消
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("UPDATE  OMS_Orders SET OrderStatus=-1, UpdatedDate=@UpdatedDate");
            strSql2.Append(" where OrderId=@OrderId OR ParentOrderId=@OrderId");
            SqlParameter[] parameters2 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime)
                };
            parameters2[0].Value = orderInfo.OrderId;
            parameters2[1].Value = DateTime.Now;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            //添加操作记录
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("insert into OMS_OrderAction(");
            strSql3.Append("OrderId,OrderCode,UserId,Username,ActionCode,ActionDate,Remark)");
            strSql3.Append(" values (");
            strSql3.Append("@OrderId,@OrderCode,@UserId,@Username,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] parameters3 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                    new SqlParameter("@UserId", SqlDbType.Int, 4),
                    new SqlParameter("@Username", SqlDbType.NVarChar, 200),
                    new SqlParameter("@ActionCode", SqlDbType.NVarChar, 100),
                    new SqlParameter("@ActionDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 1000)
                };
            parameters3[0].Value = orderInfo.OrderId;
            parameters3[1].Value = orderInfo.OrderCode;
            parameters3[2].Value = userId;
            parameters3[3].Value = userName;
            parameters3[4].Value = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SystemCancel; //orderInfo.ActionCode;
            parameters3[5].Value = DateTime.Now;
            parameters3[6].Value = "系统取消订单";
            cmd = new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);


            #region 删除已赠送优惠劵
            long mainOrderId = orderInfo.OrderId;
            if (orderInfo.OrderType == 2)//是子单  
            {
                mainOrderId = orderInfo.ParentOrderId;
            }
            if (mainOrderId > 0)
            {
                #region 删除优惠劵
                StringBuilder strSql4 = new StringBuilder();
                strSql4.Append("   delete from Shop_CouponInfo   where UserId=@UserId");
                strSql4.AppendFormat(" and  OrderId={0} ", mainOrderId);
                SqlParameter[] parameters4 = {
                    new SqlParameter("@UserId", SqlDbType.Int, 4)
                };
                parameters4[0].Value = orderInfo.BuyerID;
                cmd = new CommandInfo(strSql4.ToString(), parameters4);
                sqllist.Add(cmd);
                #endregion

                #region 删除活动详情
                StringBuilder strSql5 = new StringBuilder();
                strSql5.Append("   delete from Shop_ActivityDetail   where RuleId=3  and  UserId=@UserId");
                strSql5.AppendFormat(" and  OrderId={0} ", mainOrderId);
                SqlParameter[] parameters5 = {
                    new SqlParameter("@UserId", SqlDbType.Int, 4)
                };
                parameters5[0].Value = orderInfo.BuyerID;
                cmd = new CommandInfo(strSql5.ToString(), parameters5);
                sqllist.Add(cmd);
                #endregion
            }
            #endregion

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            return rowsAffected > 0;
        }

        /// <summary>
        /// 更新备注信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool UpdateRemark(long orderId, string remark)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update OMS_Orders set ");
            strSql.Append("Remark=@Remark ");
            strSql.Append(" where OrderId=@OrderId  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = remark;
            parameters[1].Value = orderId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string GetName(Accounts.Bus.User currentUser) {
            if (currentUser == null) {
                return "";
            }
            if (!String.IsNullOrWhiteSpace(currentUser.NickName)) {
                return currentUser.NickName;
            }
            if (!String.IsNullOrWhiteSpace(currentUser.TrueName))
            {
                return currentUser.TrueName;
            }
            return currentUser.UserName;
        }
        #endregion 


        private List<CommandInfo> AddOrderOptions(OrderInfo orderInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            foreach (Model.Shop.Order.OrderOptions model in orderInfo.OrderOptions)
            {
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append("insert into Shop_OrderOptions(");
                strSql.Append("LookupListId,LookupItemId,OrderId,OrderCode,ListDescription,ItemDescription,AdjustedPrice,CustomerTitle,CustomerDescription)");
                strSql.Append(" values (");
                strSql.Append("@LookupListId,@LookupItemId,@OrderId,@OrderCode,@ListDescription,@ItemDescription,@AdjustedPrice,@CustomerTitle,@CustomerDescription)");

                #region SqlParameter
                SqlParameter[] parameters =
                    {
                        new SqlParameter("@LookupListId", SqlDbType.Int, 4),
                        new SqlParameter("@LookupItemId", SqlDbType.Int, 4),
                        new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                        new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50),
                        new SqlParameter("@ListDescription", SqlDbType.NVarChar, 500),
                         new SqlParameter("@ItemDescription", SqlDbType.NVarChar, 500),
                        new SqlParameter("@AdjustedPrice", SqlDbType.Money, 8),
                        new SqlParameter("@CustomerTitle", SqlDbType.NVarChar, 500),
                        new SqlParameter("@CustomerDescription", SqlDbType.NVarChar, 500),

                    };
                parameters[0].Value = model.LookupListId;
                parameters[1].Value = model.LookupItemId;
                parameters[2].Value = orderInfo.OrderId;
                parameters[3].Value = orderInfo.OrderCode;
                parameters[4].Value = model.ListDescription;
                parameters[5].Value = model.ItemDescription;
                parameters[6].Value = model.AdjustedPrice;
                parameters[7].Value = model.CustomerTitle;
                parameters[8].Value = model.CustomerDescription;
                #endregion

                list.Add(new CommandInfo(strSql.ToString(),
                                         parameters, EffentNextType.ExcuteEffectRows));
            }
            return list;
        }

    }
}