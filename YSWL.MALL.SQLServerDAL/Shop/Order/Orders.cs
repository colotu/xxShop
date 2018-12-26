using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Order;
using YSWL.DBUtility;
using YSWL.MALL.Model.Shop.Order;

//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Order
{
    /// <summary>
    /// 数据访问类:Orders
    /// </summary>
    public partial class Orders : IOrders
    {
        public Orders()
        { }
        private OrderService orderService = new OrderService();

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long OrderId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from OMS_Orders");
            strSql.Append(" where OrderId=@OrderId");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderId", SqlDbType.BigInt)
            };
            parameters[0].Value = OrderId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.Order.OrderInfo  model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into OMS_Orders(");
            strSql.Append("OrderCode,ParentOrderId,SourceOrderId,SourceType,OriginalId,CreateUserId,CreatedDate,UpdatedDate,BuyerID,BuyerName,BuyerEmail,BuyerCellPhone,RegionId,ShipRegion,ShipAddress,ShipZipCode,ShipName,ShipTelPhone,ShipCellPhone,ShipEmail,ShippingModeId,ShippingModeName,RealShippingModeId,RealShippingModeName,ShipperId,ShipperName,ShipperAddress,ShipperCellPhone,Freight,FreightAdjusted,FreightActual,Weight,ShippingStatus,ShipOrderNumber,ExpressCompanyName,ExpressCompanyAbb,PaymentTypeId,PaymentTypeName,PaymentGateway,PaymentStatus,RefundStatus,PayCurrencyCode,PayCurrencyName,PaymentFee,PaymentFeeAdjusted,GatewayOrderId,OrderTotal,OrderPoint,OrderCostPrice,OrderProfit,OrderOtherCost,OrderOptionPrice,DiscountName,DiscountAmount,DiscountAdjusted,DiscountValue,DiscountValueType,CouponCode,CouponName,CouponAmount,CouponValue,CouponValueType,ActivityName,ActivityFreeAmount,ActivityStatus,GroupBuyId,GroupBuyPrice,GroupBuyStatus,Amount,OrderType,OrderTypeSub,OrderStatus,SellerID,SellerName,SellerEmail,SellerCellPhone,CommentStatus,SupplierId,SupplierName,ReferID,ReferURL,ReferType,OrderIP,Remark,ProductTotal,HasChildren,IsReviews,IsFreeShipping,DepotId,DepotName,AssignUserId,AssignName,AssignDate,WaveId,WaveNumber,OrderSort,WaveStatus,TaskBatchId,BatchNumber,DistributionId,Gwjf,Wdbh,RemarkOne,RemarkTwo,Dpxfjf)");
            strSql.Append(" values (");
            strSql.Append("@OrderCode,@ParentOrderId,@SourceOrderId,@SourceType,@OriginalId,@CreateUserId,@CreatedDate,@UpdatedDate,@BuyerID,@BuyerName,@BuyerEmail,@BuyerCellPhone,@RegionId,@ShipRegion,@ShipAddress,@ShipZipCode,@ShipName,@ShipTelPhone,@ShipCellPhone,@ShipEmail,@ShippingModeId,@ShippingModeName,@RealShippingModeId,@RealShippingModeName,@ShipperId,@ShipperName,@ShipperAddress,@ShipperCellPhone,@Freight,@FreightAdjusted,@FreightActual,@Weight,@ShippingStatus,@ShipOrderNumber,@ExpressCompanyName,@ExpressCompanyAbb,@PaymentTypeId,@PaymentTypeName,@PaymentGateway,@PaymentStatus,@RefundStatus,@PayCurrencyCode,@PayCurrencyName,@PaymentFee,@PaymentFeeAdjusted,@GatewayOrderId,@OrderTotal,@OrderPoint,@OrderCostPrice,@OrderProfit,@OrderOtherCost,@OrderOptionPrice,@DiscountName,@DiscountAmount,@DiscountAdjusted,@DiscountValue,@DiscountValueType,@CouponCode,@CouponName,@CouponAmount,@CouponValue,@CouponValueType,@ActivityName,@ActivityFreeAmount,@ActivityStatus,@GroupBuyId,@GroupBuyPrice,@GroupBuyStatus,@Amount,@OrderType,@OrderTypeSub,@OrderStatus,@SellerID,@SellerName,@SellerEmail,@SellerCellPhone,@CommentStatus,@SupplierId,@SupplierName,@ReferID,@ReferURL,@ReferType,@OrderIP,@Remark,@ProductTotal,@HasChildren,@IsReviews,@IsFreeShipping,@DepotId,@DepotName,@AssignUserId,@AssignName,@AssignDate,@WaveId,@WaveNumber,@OrderSort,@WaveStatus,@TaskBatchId,@BatchNumber,@DistributionId,@Gwjf,@Wdbh,@RemarkOne,@RemarkTwo,@Dpxfjf)");
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
                    new SqlParameter("@Gwjf", SqlDbType.Decimal,8),
                    new SqlParameter("@Wdbh", SqlDbType.NVarChar,50),
                    new SqlParameter("@RemarkOne", SqlDbType.NVarChar,2000),
                    new SqlParameter("@RemarkTwo", SqlDbType.NVarChar,2000),
                    new SqlParameter("@Dpxfjf", SqlDbType.Decimal,8)};
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
            parameters[92].Value = model.AssignDate;
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
            parameters[104].Value = model.Dpxfjf;
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Order.OrderInfo  model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update OMS_Orders set ");
            strSql.Append("OrderCode=@OrderCode,");
            strSql.Append("ParentOrderId=@ParentOrderId,");
            strSql.Append("SourceOrderId=@SourceOrderId,");
            strSql.Append("SourceType=@SourceType,");
            strSql.Append("OriginalId=@OriginalId,");
            strSql.Append("CreateUserId=@CreateUserId,");
            strSql.Append("CreatedDate=@CreatedDate,");
            strSql.Append("UpdatedDate=@UpdatedDate,");
            strSql.Append("BuyerID=@BuyerID,");
            strSql.Append("BuyerName=@BuyerName,");
            strSql.Append("BuyerEmail=@BuyerEmail,");
            strSql.Append("BuyerCellPhone=@BuyerCellPhone,");
            strSql.Append("RegionId=@RegionId,");
            strSql.Append("ShipRegion=@ShipRegion,");
            strSql.Append("ShipAddress=@ShipAddress,");
            strSql.Append("ShipZipCode=@ShipZipCode,");
            strSql.Append("ShipName=@ShipName,");
            strSql.Append("ShipTelPhone=@ShipTelPhone,");
            strSql.Append("ShipCellPhone=@ShipCellPhone,");
            strSql.Append("ShipEmail=@ShipEmail,");
            strSql.Append("ShippingModeId=@ShippingModeId,");
            strSql.Append("ShippingModeName=@ShippingModeName,");
            strSql.Append("RealShippingModeId=@RealShippingModeId,");
            strSql.Append("RealShippingModeName=@RealShippingModeName,");
            strSql.Append("ShipperId=@ShipperId,");
            strSql.Append("ShipperName=@ShipperName,");
            strSql.Append("ShipperAddress=@ShipperAddress,");
            strSql.Append("ShipperCellPhone=@ShipperCellPhone,");
            strSql.Append("Freight=@Freight,");
            strSql.Append("FreightAdjusted=@FreightAdjusted,");
            strSql.Append("FreightActual=@FreightActual,");
            strSql.Append("Weight=@Weight,");
            strSql.Append("ShippingStatus=@ShippingStatus,");
            strSql.Append("ShipOrderNumber=@ShipOrderNumber,");
            strSql.Append("ExpressCompanyName=@ExpressCompanyName,");
            strSql.Append("ExpressCompanyAbb=@ExpressCompanyAbb,");
            strSql.Append("PaymentTypeId=@PaymentTypeId,");
            strSql.Append("PaymentTypeName=@PaymentTypeName,");
            strSql.Append("PaymentGateway=@PaymentGateway,");
            strSql.Append("PaymentStatus=@PaymentStatus,");
            strSql.Append("RefundStatus=@RefundStatus,");
            strSql.Append("PayCurrencyCode=@PayCurrencyCode,");
            strSql.Append("PayCurrencyName=@PayCurrencyName,");
            strSql.Append("PaymentFee=@PaymentFee,");
            strSql.Append("PaymentFeeAdjusted=@PaymentFeeAdjusted,");
            strSql.Append("GatewayOrderId=@GatewayOrderId,");
            strSql.Append("OrderTotal=@OrderTotal,");
            strSql.Append("OrderPoint=@OrderPoint,");
            strSql.Append("OrderCostPrice=@OrderCostPrice,");
            strSql.Append("OrderProfit=@OrderProfit,");
            strSql.Append("OrderOtherCost=@OrderOtherCost,");
            strSql.Append("OrderOptionPrice=@OrderOptionPrice,");
            strSql.Append("DiscountName=@DiscountName,");
            strSql.Append("DiscountAmount=@DiscountAmount,");
            strSql.Append("DiscountAdjusted=@DiscountAdjusted,");
            strSql.Append("DiscountValue=@DiscountValue,");
            strSql.Append("DiscountValueType=@DiscountValueType,");
            strSql.Append("CouponCode=@CouponCode,");
            strSql.Append("CouponName=@CouponName,");
            strSql.Append("CouponAmount=@CouponAmount,");
            strSql.Append("CouponValue=@CouponValue,");
            strSql.Append("CouponValueType=@CouponValueType,");
            strSql.Append("ActivityName=@ActivityName,");
            strSql.Append("ActivityFreeAmount=@ActivityFreeAmount,");
            strSql.Append("ActivityStatus=@ActivityStatus,");
            strSql.Append("GroupBuyId=@GroupBuyId,");
            strSql.Append("GroupBuyPrice=@GroupBuyPrice,");
            strSql.Append("GroupBuyStatus=@GroupBuyStatus,");
            strSql.Append("Amount=@Amount,");
            strSql.Append("OrderType=@OrderType,");
            strSql.Append("OrderTypeSub=@OrderTypeSub,");
            strSql.Append("OrderStatus=@OrderStatus,");
            strSql.Append("SellerID=@SellerID,");
            strSql.Append("SellerName=@SellerName,");
            strSql.Append("SellerEmail=@SellerEmail,");
            strSql.Append("SellerCellPhone=@SellerCellPhone,");
            strSql.Append("CommentStatus=@CommentStatus,");
            strSql.Append("SupplierId=@SupplierId,");
            strSql.Append("SupplierName=@SupplierName,");
            strSql.Append("ReferID=@ReferID,");
            strSql.Append("ReferURL=@ReferURL,");
            strSql.Append("ReferType=@ReferType,");
            strSql.Append("OrderIP=@OrderIP,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("ProductTotal=@ProductTotal,");
            strSql.Append("HasChildren=@HasChildren,");
            strSql.Append("IsReviews=@IsReviews,");
            strSql.Append("IsFreeShipping=@IsFreeShipping,");
            strSql.Append("DepotId=@DepotId,");
            strSql.Append("DepotName=@DepotName,");
            strSql.Append("AssignUserId=@AssignUserId,");
            strSql.Append("AssignName=@AssignName,");
            strSql.Append("AssignDate=@AssignDate,");
            strSql.Append("WaveId=@WaveId,");
            strSql.Append("WaveNumber=@WaveNumber,");
            strSql.Append("OrderSort=@OrderSort,");
            strSql.Append("WaveStatus=@WaveStatus,");
            strSql.Append("TaskBatchId=@TaskBatchId,");
            strSql.Append("BatchNumber=@BatchNumber,");
            strSql.Append("DistributionId=@DistributionId,");
            strSql.Append("Gwjf=@Gwjf,");
            strSql.Append("Wdbh=@Wdbh,");
            strSql.Append("RemarkOne=@RemarkOne,");
            strSql.Append("RemarkTwo=@RemarkTwo,");
            strSql.Append("Dpxfjf=@Dpxfjf");
            strSql.Append(" where OrderId=@OrderId");
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
                    new SqlParameter("@RemarkTwo", SqlDbType.NVarChar,2000),
                    new SqlParameter("@Dpxfjf", SqlDbType.Decimal,8),
                    new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
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
            parameters[92].Value = model.AssignDate;
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
            parameters[104].Value = model.Dpxfjf;
            parameters[105].Value = model.OrderId;

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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(long OrderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from OMS_Orders ");
            strSql.Append(" where OrderId=@OrderId");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderId", SqlDbType.BigInt)
            };
            parameters[0].Value = OrderId;

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
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string OrderIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from OMS_Orders ");
            strSql.Append(" where OrderId in (" + OrderIdlist + ")  ");
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Order.OrderInfo  GetModel(long OrderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 OrderId,OrderCode,ParentOrderId,SourceOrderId,SourceType,OriginalId,CreateUserId,CreatedDate,UpdatedDate,BuyerID,BuyerName,BuyerEmail,BuyerCellPhone,RegionId,ShipRegion,ShipAddress,ShipZipCode,ShipName,ShipTelPhone,ShipCellPhone,ShipEmail,ShippingModeId,ShippingModeName,RealShippingModeId,RealShippingModeName,ShipperId,ShipperName,ShipperAddress,ShipperCellPhone,Freight,FreightAdjusted,FreightActual,Weight,ShippingStatus,ShipOrderNumber,ExpressCompanyName,ExpressCompanyAbb,PaymentTypeId,PaymentTypeName,PaymentGateway,PaymentStatus,RefundStatus,PayCurrencyCode,PayCurrencyName,PaymentFee,PaymentFeeAdjusted,GatewayOrderId,OrderTotal,OrderPoint,OrderCostPrice,OrderProfit,OrderOtherCost,OrderOptionPrice,DiscountName,DiscountAmount,DiscountAdjusted,DiscountValue,DiscountValueType,CouponCode,CouponName,CouponAmount,CouponValue,CouponValueType,ActivityName,ActivityFreeAmount,ActivityStatus,GroupBuyId,GroupBuyPrice,GroupBuyStatus,Amount,OrderType,OrderTypeSub,OrderStatus,SellerID,SellerName,SellerEmail,SellerCellPhone,CommentStatus,SupplierId,SupplierName,ReferID,ReferURL,ReferType,OrderIP,Remark,ProductTotal,HasChildren,IsReviews,IsFreeShipping,DepotId,DepotName,AssignUserId,AssignName,AssignDate,WaveId,WaveNumber,OrderSort,WaveStatus,TaskBatchId,BatchNumber,DistributionId,Gwjf,Wdbh,RemarkOne,RemarkTwo,Dpxfjf from OMS_Orders ");
            strSql.Append(" where OrderId=@OrderId");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderId", SqlDbType.BigInt)
            };
            parameters[0].Value = OrderId;

            YSWL.MALL.Model.Shop.Order.OrderInfo  model = new YSWL.MALL.Model.Shop.Order.OrderInfo ();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Order.OrderInfo DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Order.OrderInfo  model = new YSWL.MALL.Model.Shop.Order.OrderInfo ();
            if (row != null)
            {
                if (row["OrderId"] != null && row["OrderId"].ToString() != "")
                {
                    model.OrderId = long.Parse(row["OrderId"].ToString());
                }
                if (row["OrderCode"] != null)
                {
                    model.OrderCode = row["OrderCode"].ToString();
                }
                if (row["ParentOrderId"] != null && row["ParentOrderId"].ToString() != "")
                {
                    model.ParentOrderId = long.Parse(row["ParentOrderId"].ToString());
                }
                if (row["SourceOrderId"] != null && row["SourceOrderId"].ToString() != "")
                {
                    model.SourceOrderId = long.Parse(row["SourceOrderId"].ToString());
                }
                if (row["SourceType"] != null && row["SourceType"].ToString() != "")
                {
                    model.SourceType = int.Parse(row["SourceType"].ToString());
                }
                if (row["OriginalId"] != null && row["OriginalId"].ToString() != "")
                {
                    model.OriginalId = long.Parse(row["OriginalId"].ToString());
                }
                if (row["CreateUserId"] != null && row["CreateUserId"].ToString() != "")
                {
                    model.CreateUserId = int.Parse(row["CreateUserId"].ToString());
                }
                if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                }
                if (row["UpdatedDate"] != null && row["UpdatedDate"].ToString() != "")
                {
                    model.UpdatedDate = DateTime.Parse(row["UpdatedDate"].ToString());
                }
                if (row["BuyerID"] != null && row["BuyerID"].ToString() != "")
                {
                    model.BuyerID = int.Parse(row["BuyerID"].ToString());
                }
                if (row["BuyerName"] != null)
                {
                    model.BuyerName = row["BuyerName"].ToString();
                }
                if (row["BuyerEmail"] != null)
                {
                    model.BuyerEmail = row["BuyerEmail"].ToString();
                }
                if (row["BuyerCellPhone"] != null)
                {
                    model.BuyerCellPhone = row["BuyerCellPhone"].ToString();
                }
                if (row["RegionId"] != null && row["RegionId"].ToString() != "")
                {
                    model.RegionId = int.Parse(row["RegionId"].ToString());
                }
                if (row["ShipRegion"] != null)
                {
                    model.ShipRegion = row["ShipRegion"].ToString();
                }
                if (row["ShipAddress"] != null)
                {
                    model.ShipAddress = row["ShipAddress"].ToString();
                }
                if (row["ShipZipCode"] != null)
                {
                    model.ShipZipCode = row["ShipZipCode"].ToString();
                }
                if (row["ShipName"] != null)
                {
                    model.ShipName = row["ShipName"].ToString();
                }
                if (row["ShipTelPhone"] != null)
                {
                    model.ShipTelPhone = row["ShipTelPhone"].ToString();
                }
                if (row["ShipCellPhone"] != null)
                {
                    model.ShipCellPhone = row["ShipCellPhone"].ToString();
                }
                if (row["ShipEmail"] != null)
                {
                    model.ShipEmail = row["ShipEmail"].ToString();
                }
                if (row["ShippingModeId"] != null && row["ShippingModeId"].ToString() != "")
                {
                    model.ShippingModeId = int.Parse(row["ShippingModeId"].ToString());
                }
                if (row["ShippingModeName"] != null)
                {
                    model.ShippingModeName = row["ShippingModeName"].ToString();
                }
                if (row["RealShippingModeId"] != null && row["RealShippingModeId"].ToString() != "")
                {
                    model.RealShippingModeId = int.Parse(row["RealShippingModeId"].ToString());
                }
                if (row["RealShippingModeName"] != null)
                {
                    model.RealShippingModeName = row["RealShippingModeName"].ToString();
                }
                if (row["ShipperId"] != null && row["ShipperId"].ToString() != "")
                {
                    model.ShipperId = int.Parse(row["ShipperId"].ToString());
                }
                if (row["ShipperName"] != null)
                {
                    model.ShipperName = row["ShipperName"].ToString();
                }
                if (row["ShipperAddress"] != null)
                {
                    model.ShipperAddress = row["ShipperAddress"].ToString();
                }
                if (row["ShipperCellPhone"] != null)
                {
                    model.ShipperCellPhone = row["ShipperCellPhone"].ToString();
                }
                if (row["Freight"] != null && row["Freight"].ToString() != "")
                {
                    model.Freight = decimal.Parse(row["Freight"].ToString());
                }
                if (row["FreightAdjusted"] != null && row["FreightAdjusted"].ToString() != "")
                {
                    model.FreightAdjusted = decimal.Parse(row["FreightAdjusted"].ToString());
                }
                if (row["FreightActual"] != null && row["FreightActual"].ToString() != "")
                {
                    model.FreightActual = decimal.Parse(row["FreightActual"].ToString());
                }
                if (row["Weight"] != null && row["Weight"].ToString() != "")
                {
                    model.Weight = int.Parse(row["Weight"].ToString());
                }
                if (row["ShippingStatus"] != null && row["ShippingStatus"].ToString() != "")
                {
                    model.ShippingStatus = int.Parse(row["ShippingStatus"].ToString());
                }
                if (row["ShipOrderNumber"] != null)
                {
                    model.ShipOrderNumber = row["ShipOrderNumber"].ToString();
                }
                if (row["ExpressCompanyName"] != null)
                {
                    model.ExpressCompanyName = row["ExpressCompanyName"].ToString();
                }
                if (row["ExpressCompanyAbb"] != null)
                {
                    model.ExpressCompanyAbb = row["ExpressCompanyAbb"].ToString();
                }
                if (row["PaymentTypeId"] != null && row["PaymentTypeId"].ToString() != "")
                {
                    model.PaymentTypeId = int.Parse(row["PaymentTypeId"].ToString());
                }
                if (row["PaymentTypeName"] != null)
                {
                    model.PaymentTypeName = row["PaymentTypeName"].ToString();
                }
                if (row["PaymentGateway"] != null)
                {
                    model.PaymentGateway = row["PaymentGateway"].ToString();
                }
                if (row["PaymentStatus"] != null && row["PaymentStatus"].ToString() != "")
                {
                    model.PaymentStatus = int.Parse(row["PaymentStatus"].ToString());
                }
                if (row["RefundStatus"] != null && row["RefundStatus"].ToString() != "")
                {
                    model.RefundStatus = int.Parse(row["RefundStatus"].ToString());
                }
                if (row["PayCurrencyCode"] != null)
                {
                    model.PayCurrencyCode = row["PayCurrencyCode"].ToString();
                }
                if (row["PayCurrencyName"] != null)
                {
                    model.PayCurrencyName = row["PayCurrencyName"].ToString();
                }
                if (row["PaymentFee"] != null && row["PaymentFee"].ToString() != "")
                {
                    model.PaymentFee = decimal.Parse(row["PaymentFee"].ToString());
                }
                if (row["PaymentFeeAdjusted"] != null && row["PaymentFeeAdjusted"].ToString() != "")
                {
                    model.PaymentFeeAdjusted = decimal.Parse(row["PaymentFeeAdjusted"].ToString());
                }
                if (row["GatewayOrderId"] != null)
                {
                    model.GatewayOrderId = row["GatewayOrderId"].ToString();
                }
                if (row["OrderTotal"] != null && row["OrderTotal"].ToString() != "")
                {
                    model.OrderTotal = decimal.Parse(row["OrderTotal"].ToString());
                }
                if (row["OrderPoint"] != null && row["OrderPoint"].ToString() != "")
                {
                    model.OrderPoint = int.Parse(row["OrderPoint"].ToString());
                }
                if (row["OrderCostPrice"] != null && row["OrderCostPrice"].ToString() != "")
                {
                    model.OrderCostPrice = decimal.Parse(row["OrderCostPrice"].ToString());
                }
                if (row["OrderProfit"] != null && row["OrderProfit"].ToString() != "")
                {
                    model.OrderProfit = decimal.Parse(row["OrderProfit"].ToString());
                }
                if (row["OrderOtherCost"] != null && row["OrderOtherCost"].ToString() != "")
                {
                    model.OrderOtherCost = decimal.Parse(row["OrderOtherCost"].ToString());
                }
                if (row["OrderOptionPrice"] != null && row["OrderOptionPrice"].ToString() != "")
                {
                    model.OrderOptionPrice = decimal.Parse(row["OrderOptionPrice"].ToString());
                }
                if (row["DiscountName"] != null)
                {
                    model.DiscountName = row["DiscountName"].ToString();
                }
                if (row["DiscountAmount"] != null && row["DiscountAmount"].ToString() != "")
                {
                    model.DiscountAmount = decimal.Parse(row["DiscountAmount"].ToString());
                }
                if (row["DiscountAdjusted"] != null && row["DiscountAdjusted"].ToString() != "")
                {
                    model.DiscountAdjusted = decimal.Parse(row["DiscountAdjusted"].ToString());
                }
                if (row["DiscountValue"] != null && row["DiscountValue"].ToString() != "")
                {
                    model.DiscountValue = decimal.Parse(row["DiscountValue"].ToString());
                }
                if (row["DiscountValueType"] != null && row["DiscountValueType"].ToString() != "")
                {
                    model.DiscountValueType = int.Parse(row["DiscountValueType"].ToString());
                }
                if (row["CouponCode"] != null)
                {
                    model.CouponCode = row["CouponCode"].ToString();
                }
                if (row["CouponName"] != null)
                {
                    model.CouponName = row["CouponName"].ToString();
                }
                if (row["CouponAmount"] != null && row["CouponAmount"].ToString() != "")
                {
                    model.CouponAmount = decimal.Parse(row["CouponAmount"].ToString());
                }
                if (row["CouponValue"] != null && row["CouponValue"].ToString() != "")
                {
                    model.CouponValue = decimal.Parse(row["CouponValue"].ToString());
                }
                if (row["CouponValueType"] != null && row["CouponValueType"].ToString() != "")
                {
                    model.CouponValueType = int.Parse(row["CouponValueType"].ToString());
                }
                if (row["ActivityName"] != null)
                {
                    model.ActivityName = row["ActivityName"].ToString();
                }
                if (row["ActivityFreeAmount"] != null && row["ActivityFreeAmount"].ToString() != "")
                {
                    model.ActivityFreeAmount = decimal.Parse(row["ActivityFreeAmount"].ToString());
                }
                if (row["ActivityStatus"] != null && row["ActivityStatus"].ToString() != "")
                {
                    model.ActivityStatus = int.Parse(row["ActivityStatus"].ToString());
                }
                if (row["GroupBuyId"] != null && row["GroupBuyId"].ToString() != "")
                {
                    model.GroupBuyId = int.Parse(row["GroupBuyId"].ToString());
                }
                if (row["GroupBuyPrice"] != null && row["GroupBuyPrice"].ToString() != "")
                {
                    model.GroupBuyPrice = decimal.Parse(row["GroupBuyPrice"].ToString());
                }
                if (row["GroupBuyStatus"] != null && row["GroupBuyStatus"].ToString() != "")
                {
                    model.GroupBuyStatus = int.Parse(row["GroupBuyStatus"].ToString());
                }
                if (row["Amount"] != null && row["Amount"].ToString() != "")
                {
                    model.Amount = decimal.Parse(row["Amount"].ToString());
                }
                if (row["OrderType"] != null && row["OrderType"].ToString() != "")
                {
                    model.OrderType = int.Parse(row["OrderType"].ToString());
                }
                if (row["OrderTypeSub"] != null && row["OrderTypeSub"].ToString() != "")
                {
                    model.OrderTypeSub = int.Parse(row["OrderTypeSub"].ToString());
                }
                if (row["OrderStatus"] != null && row["OrderStatus"].ToString() != "")
                {
                    model.OrderStatus = int.Parse(row["OrderStatus"].ToString());
                }
                if (row["SellerID"] != null && row["SellerID"].ToString() != "")
                {
                    model.SellerID = int.Parse(row["SellerID"].ToString());
                }
                if (row["SellerName"] != null)
                {
                    model.SellerName = row["SellerName"].ToString();
                }
                if (row["SellerEmail"] != null)
                {
                    model.SellerEmail = row["SellerEmail"].ToString();
                }
                if (row["SellerCellPhone"] != null)
                {
                    model.SellerCellPhone = row["SellerCellPhone"].ToString();
                }
                if (row["CommentStatus"] != null && row["CommentStatus"].ToString() != "")
                {
                    model.CommentStatus = int.Parse(row["CommentStatus"].ToString());
                }
                if (row["SupplierId"] != null && row["SupplierId"].ToString() != "")
                {
                    model.SupplierId = int.Parse(row["SupplierId"].ToString());
                }
                if (row["SupplierName"] != null)
                {
                    model.SupplierName = row["SupplierName"].ToString();
                }
                if (row["ReferID"] != null)
                {
                    model.ReferID = row["ReferID"].ToString();
                }
                if (row["ReferURL"] != null)
                {
                    model.ReferURL = row["ReferURL"].ToString();
                }
                if (row["ReferType"] != null && row["ReferType"].ToString() != "")
                {
                    model.ReferType = int.Parse(row["ReferType"].ToString());
                }
                if (row["OrderIP"] != null)
                {
                    model.OrderIP = row["OrderIP"].ToString();
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["ProductTotal"] != null && row["ProductTotal"].ToString() != "")
                {
                    model.ProductTotal = decimal.Parse(row["ProductTotal"].ToString());
                }
                if (row["HasChildren"] != null && row["HasChildren"].ToString() != "")
                {
                    if ((row["HasChildren"].ToString() == "1") || (row["HasChildren"].ToString().ToLower() == "true"))
                    {
                        model.HasChildren = true;
                    }
                    else
                    {
                        model.HasChildren = false;
                    }
                }
                if (row["IsReviews"] != null && row["IsReviews"].ToString() != "")
                {
                    if ((row["IsReviews"].ToString() == "1") || (row["IsReviews"].ToString().ToLower() == "true"))
                    {
                        model.IsReviews = true;
                    }
                    else
                    {
                        model.IsReviews = false;
                    }
                }
                if (row["IsFreeShipping"] != null && row["IsFreeShipping"].ToString() != "")
                {
                    if ((row["IsFreeShipping"].ToString() == "1") || (row["IsFreeShipping"].ToString().ToLower() == "true"))
                    {
                        model.IsFreeShipping = true;
                    }
                    else
                    {
                        model.IsFreeShipping = false;
                    }
                }
                if (row["DepotId"] != null && row["DepotId"].ToString() != "")
                {
                    model.DepotId = int.Parse(row["DepotId"].ToString());
                }
                if (row["DepotName"] != null)
                {
                    model.DepotName = row["DepotName"].ToString();
                }
                if (row["AssignUserId"] != null && row["AssignUserId"].ToString() != "")
                {
                    model.AssignUserId = int.Parse(row["AssignUserId"].ToString());
                }
                if (row["AssignName"] != null)
                {
                    model.AssignName = row["AssignName"].ToString();
                }
                if (row["AssignDate"] != null && row["AssignDate"].ToString() != "")
                {
                    model.AssignDate = DateTime.Parse(row["AssignDate"].ToString());
                }
                if (row["WaveId"] != null && row["WaveId"].ToString() != "")
                {
                    model.WaveId = long.Parse(row["WaveId"].ToString());
                }
                if (row["WaveNumber"] != null)
                {
                    model.WaveNumber = row["WaveNumber"].ToString();
                }
                if (row["OrderSort"] != null && row["OrderSort"].ToString() != "")
                {
                    model.OrderSort = int.Parse(row["OrderSort"].ToString());
                }
                if (row["WaveStatus"] != null && row["WaveStatus"].ToString() != "")
                {
                    model.WaveStatus = int.Parse(row["WaveStatus"].ToString());
                }
                if (row["TaskBatchId"] != null && row["TaskBatchId"].ToString() != "")
                {
                    model.TaskBatchId = long.Parse(row["TaskBatchId"].ToString());
                }
                if (row["BatchNumber"] != null)
                {
                    model.BatchNumber = row["BatchNumber"].ToString();
                }
                if (row["DistributionId"] != null && row["DistributionId"].ToString() != "")
                {
                    model.DistributionId = int.Parse(row["DistributionId"].ToString());
                }
                if (row["Gwjf"] != null && row["Gwjf"].ToString() != "")
                {
                    model.Gwjf = decimal.Parse(row["Gwjf"].ToString());
                }

                if (row["Wdbh"] != null)
                {
                    model.Wdbh = row["Wdbh"].ToString();
                }

                if (row["RemarkOne"] != null)
                {
                    model.RemrkOne = row["RemarkOne"].ToString();
                }

                if (row["RemarkTwo"] != null)
                {
                    model.RemrkTwo = row["RemarkTwo"].ToString();
                }

                if (row["Dpxfjf"] != null && row["Dpxfjf"].ToString() != "")
                {
                    model.Dpxfjf = decimal.Parse(row["Dpxfjf"].ToString());
                }

            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select OrderId,OrderCode,ParentOrderId,SourceOrderId,SourceType,OriginalId,CreateUserId,CreatedDate,UpdatedDate,BuyerID,BuyerName,BuyerEmail,BuyerCellPhone,RegionId,ShipRegion,ShipAddress,ShipZipCode,ShipName,ShipTelPhone,ShipCellPhone,ShipEmail,ShippingModeId,ShippingModeName,RealShippingModeId,RealShippingModeName,ShipperId,ShipperName,ShipperAddress,ShipperCellPhone,Freight,FreightAdjusted,FreightActual,Weight,ShippingStatus,ShipOrderNumber,ExpressCompanyName,ExpressCompanyAbb,PaymentTypeId,PaymentTypeName,PaymentGateway,PaymentStatus,RefundStatus,PayCurrencyCode,PayCurrencyName,PaymentFee,PaymentFeeAdjusted,GatewayOrderId,OrderTotal,OrderPoint,OrderCostPrice,OrderProfit,OrderOtherCost,OrderOptionPrice,DiscountName,DiscountAmount,DiscountAdjusted,DiscountValue,DiscountValueType,CouponCode,CouponName,CouponAmount,CouponValue,CouponValueType,ActivityName,ActivityFreeAmount,ActivityStatus,GroupBuyId,GroupBuyPrice,GroupBuyStatus,Amount,OrderType,OrderTypeSub,OrderStatus,SellerID,SellerName,SellerEmail,SellerCellPhone,CommentStatus,SupplierId,SupplierName,ReferID,ReferURL,ReferType,OrderIP,Remark,ProductTotal,HasChildren,IsReviews,IsFreeShipping,DepotId,DepotName,AssignUserId,AssignName,AssignDate,WaveId,WaveNumber,OrderSort,WaveStatus,TaskBatchId,BatchNumber,DistributionId,Gwjf,Wdbh,RemarkOne,RemarkTwo,Dpxfjf ");
            strSql.Append(" FROM OMS_Orders ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" OrderId,OrderCode,ParentOrderId,SourceOrderId,SourceType,OriginalId,CreateUserId,CreatedDate,UpdatedDate,BuyerID,BuyerName,BuyerEmail,BuyerCellPhone,RegionId,ShipRegion,ShipAddress,ShipZipCode,ShipName,ShipTelPhone,ShipCellPhone,ShipEmail,ShippingModeId,ShippingModeName,RealShippingModeId,RealShippingModeName,ShipperId,ShipperName,ShipperAddress,ShipperCellPhone,Freight,FreightAdjusted,FreightActual,Weight,ShippingStatus,ShipOrderNumber,ExpressCompanyName,ExpressCompanyAbb,PaymentTypeId,PaymentTypeName,PaymentGateway,PaymentStatus,RefundStatus,PayCurrencyCode,PayCurrencyName,PaymentFee,PaymentFeeAdjusted,GatewayOrderId,OrderTotal,OrderPoint,OrderCostPrice,OrderProfit,OrderOtherCost,OrderOptionPrice,DiscountName,DiscountAmount,DiscountAdjusted,DiscountValue,DiscountValueType,CouponCode,CouponName,CouponAmount,CouponValue,CouponValueType,ActivityName,ActivityFreeAmount,ActivityStatus,GroupBuyId,GroupBuyPrice,GroupBuyStatus,Amount,OrderType,OrderTypeSub,OrderStatus,SellerID,SellerName,SellerEmail,SellerCellPhone,CommentStatus,SupplierId,SupplierName,ReferID,ReferURL,ReferType,OrderIP,Remark,ProductTotal,HasChildren,IsReviews,IsFreeShipping,DepotId,DepotName,AssignUserId,AssignName,AssignDate,WaveId,WaveNumber,OrderSort,WaveStatus,TaskBatchId,BatchNumber,DistributionId,Gwjf,Wdbh,RemarkOne,RemarkTwo,Dpxfjf ");
            strSql.Append(" FROM OMS_Orders ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM OMS_Orders ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.OrderId desc");
            }
            strSql.Append(")AS Row, T.*  from OMS_Orders T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "OMS_Orders";
			parameters[1].Value = "OrderId";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod

        #region  ExtensionMethod

        //退货操作
        public bool ReturnStatus(long orderId)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //返回库存

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
        /// 更新订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateOrderStatus(long orderId, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update OMS_Orders set ");
            strSql.Append("OrderStatus=@OrderStatus");
            strSql.Append(" where OrderId=@OrderId");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = status;
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

        public bool UpdateShipped(YSWL.MALL.Model.Shop.Order.OrderInfo model)
        {

            List<CommandInfo> sqllist = new List<CommandInfo>();
            #region 更新动作

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update OMS_Orders set ");
            strSql.Append("OrderCode=@OrderCode,");
            strSql.Append("ParentOrderId=@ParentOrderId,");
            strSql.Append("CreatedDate=@CreatedDate,");
            strSql.Append("UpdatedDate=@UpdatedDate,");
            strSql.Append("BuyerID=@BuyerID,");
            strSql.Append("BuyerName=@BuyerName,");
            strSql.Append("BuyerEmail=@BuyerEmail,");
            strSql.Append("BuyerCellPhone=@BuyerCellPhone,");
            strSql.Append("RegionId=@RegionId,");
            strSql.Append("ShipRegion=@ShipRegion,");
            strSql.Append("ShipAddress=@ShipAddress,");
            strSql.Append("ShipZipCode=@ShipZipCode,");
            strSql.Append("ShipName=@ShipName,");
            strSql.Append("ShipTelPhone=@ShipTelPhone,");
            strSql.Append("ShipCellPhone=@ShipCellPhone,");
            strSql.Append("ShipEmail=@ShipEmail,");
            strSql.Append("ShippingModeId=@ShippingModeId,");
            strSql.Append("ShippingModeName=@ShippingModeName,");
            strSql.Append("RealShippingModeId=@RealShippingModeId,");
            strSql.Append("RealShippingModeName=@RealShippingModeName,");
            strSql.Append("ShipperId=@ShipperId,");
            strSql.Append("ShipperName=@ShipperName,");
            strSql.Append("ShipperAddress=@ShipperAddress,");
            strSql.Append("ShipperCellPhone=@ShipperCellPhone,");
            strSql.Append("Freight=@Freight,");
            strSql.Append("FreightAdjusted=@FreightAdjusted,");
            strSql.Append("FreightActual=@FreightActual,");
            strSql.Append("Weight=@Weight,");
            strSql.Append("ShippingStatus=@ShippingStatus,");
            strSql.Append("ShipOrderNumber=@ShipOrderNumber,");
            strSql.Append("ExpressCompanyName=@ExpressCompanyName,");
            strSql.Append("ExpressCompanyAbb=@ExpressCompanyAbb,");
            strSql.Append("PaymentTypeId=@PaymentTypeId,");
            strSql.Append("PaymentTypeName=@PaymentTypeName,");
            strSql.Append("PaymentGateway=@PaymentGateway,");
            strSql.Append("PaymentStatus=@PaymentStatus,");
            strSql.Append("RefundStatus=@RefundStatus,");
            strSql.Append("PayCurrencyCode=@PayCurrencyCode,");
            strSql.Append("PayCurrencyName=@PayCurrencyName,");
            strSql.Append("PaymentFee=@PaymentFee,");
            strSql.Append("PaymentFeeAdjusted=@PaymentFeeAdjusted,");
            strSql.Append("GatewayOrderId=@GatewayOrderId,");
            strSql.Append("OrderTotal=@OrderTotal,");
            strSql.Append("OrderPoint=@OrderPoint,");
            strSql.Append("OrderCostPrice=@OrderCostPrice,");
            strSql.Append("OrderProfit=@OrderProfit,");
            strSql.Append("OrderOtherCost=@OrderOtherCost,");
            strSql.Append("OrderOptionPrice=@OrderOptionPrice,");
            strSql.Append("DiscountName=@DiscountName,");
            strSql.Append("DiscountAmount=@DiscountAmount,");
            strSql.Append("DiscountAdjusted=@DiscountAdjusted,");
            strSql.Append("DiscountValue=@DiscountValue,");
            strSql.Append("DiscountValueType=@DiscountValueType,");
            strSql.Append("CouponCode=@CouponCode,");
            strSql.Append("CouponName=@CouponName,");
            strSql.Append("CouponAmount=@CouponAmount,");
            strSql.Append("CouponValue=@CouponValue,");
            strSql.Append("CouponValueType=@CouponValueType,");
            strSql.Append("ActivityName=@ActivityName,");
            strSql.Append("ActivityFreeAmount=@ActivityFreeAmount,");
            strSql.Append("ActivityStatus=@ActivityStatus,");
            strSql.Append("GroupBuyId=@GroupBuyId,");
            strSql.Append("GroupBuyPrice=@GroupBuyPrice,");
            strSql.Append("GroupBuyStatus=@GroupBuyStatus,");
            strSql.Append("Amount=@Amount,");
            strSql.Append("OrderType=@OrderType,");
            strSql.Append("OrderStatus=@OrderStatus,");
            strSql.Append("SellerID=@SellerID,");
            strSql.Append("SellerName=@SellerName,");
            strSql.Append("SellerEmail=@SellerEmail,");
            strSql.Append("SellerCellPhone=@SellerCellPhone,");
            strSql.Append("SupplierId=@SupplierId,");
            strSql.Append("SupplierName=@SupplierName,");
            strSql.Append("ReferID=@ReferID,");
            strSql.Append("ReferURL=@ReferURL,");
            strSql.Append("OrderIP=@OrderIP,");
            strSql.Append("Wdbh=@Wdbh,");
            strSql.Append("RemarkOne=@RemarkOne,");
            strSql.Append("RemarkTwo=@RemarkTwo,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Dpxfjf=@Dpxfjf,");
            strSql.Append("CommentStatus=@CommentStatus");
            strSql.Append(" where OrderId=@OrderId");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@ParentOrderId", SqlDbType.BigInt,8),
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
                    new SqlParameter("@OrderStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@SellerID", SqlDbType.Int,4),
                    new SqlParameter("@SellerName", SqlDbType.NVarChar,100),
                    new SqlParameter("@SellerEmail", SqlDbType.NVarChar,100),
                    new SqlParameter("@SellerCellPhone", SqlDbType.NVarChar,50),
                    new SqlParameter("@SupplierId", SqlDbType.Int,4),
                    new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
                    new SqlParameter("@ReferID", SqlDbType.NVarChar,50),
                    new SqlParameter("@ReferURL", SqlDbType.NVarChar,200),
                    new SqlParameter("@OrderIP", SqlDbType.NVarChar,50),
                    new SqlParameter("@Wdbh", SqlDbType.NVarChar,50),
                    new SqlParameter("@RemarkOne", SqlDbType.NVarChar,2000),
                    new SqlParameter("@RemarkTwo", SqlDbType.NVarChar,2000),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
                    new SqlParameter("@Dpxfjf", SqlDbType.Decimal,8),
                    new SqlParameter("@CommentStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.OrderCode;
            parameters[1].Value = model.ParentOrderId;
            parameters[2].Value = model.CreatedDate;
            parameters[3].Value = model.UpdatedDate;
            parameters[4].Value = model.BuyerID;
            parameters[5].Value = model.BuyerName;
            parameters[6].Value = model.BuyerEmail;
            parameters[7].Value = model.BuyerCellPhone;
            parameters[8].Value = model.RegionId;
            parameters[9].Value = model.ShipRegion;
            parameters[10].Value = model.ShipAddress;
            parameters[11].Value = model.ShipZipCode;
            parameters[12].Value = model.ShipName;
            parameters[13].Value = model.ShipTelPhone;
            parameters[14].Value = model.ShipCellPhone;
            parameters[15].Value = model.ShipEmail;
            parameters[16].Value = model.ShippingModeId;
            parameters[17].Value = model.ShippingModeName;
            parameters[18].Value = model.RealShippingModeId;
            parameters[19].Value = model.RealShippingModeName;
            parameters[20].Value = model.ShipperId;
            parameters[21].Value = model.ShipperName;
            parameters[22].Value = model.ShipperAddress;
            parameters[23].Value = model.ShipperCellPhone;
            parameters[24].Value = model.Freight;
            parameters[25].Value = model.FreightAdjusted;
            parameters[26].Value = model.FreightActual;
            parameters[27].Value = model.Weight;
            parameters[28].Value = model.ShippingStatus;
            parameters[29].Value = model.ShipOrderNumber;
            parameters[30].Value = model.ExpressCompanyName;
            parameters[31].Value = model.ExpressCompanyAbb;
            parameters[32].Value = model.PaymentTypeId;
            parameters[33].Value = model.PaymentTypeName;
            parameters[34].Value = model.PaymentGateway;
            parameters[35].Value = model.PaymentStatus;
            parameters[36].Value = model.RefundStatus;
            parameters[37].Value = model.PayCurrencyCode;
            parameters[38].Value = model.PayCurrencyName;
            parameters[39].Value = model.PaymentFee;
            parameters[40].Value = model.PaymentFeeAdjusted;
            parameters[41].Value = model.GatewayOrderId;
            parameters[42].Value = model.OrderTotal;
            parameters[43].Value = model.OrderPoint;
            parameters[44].Value = model.OrderCostPrice;
            parameters[45].Value = model.OrderProfit;
            parameters[46].Value = model.OrderOtherCost;
            parameters[47].Value = model.OrderOptionPrice;
            parameters[48].Value = model.DiscountName;
            parameters[49].Value = model.DiscountAmount;
            parameters[50].Value = model.DiscountAdjusted;
            parameters[51].Value = model.DiscountValue;
            parameters[52].Value = model.DiscountValueType;
            parameters[53].Value = model.CouponCode;
            parameters[54].Value = model.CouponName;
            parameters[55].Value = model.CouponAmount;
            parameters[56].Value = model.CouponValue;
            parameters[57].Value = model.CouponValueType;
            parameters[58].Value = model.ActivityName;
            parameters[59].Value = model.ActivityFreeAmount;
            parameters[60].Value = model.ActivityStatus;
            parameters[61].Value = model.GroupBuyId;
            parameters[62].Value = model.GroupBuyPrice;
            parameters[63].Value = model.GroupBuyStatus;
            parameters[64].Value = model.Amount;
            parameters[65].Value = model.OrderType;
            parameters[66].Value = model.OrderStatus;
            parameters[67].Value = model.SellerID;
            parameters[68].Value = model.SellerName;
            parameters[69].Value = model.SellerEmail;
            parameters[70].Value = model.SellerCellPhone;
            parameters[71].Value = model.SupplierId;
            parameters[72].Value = model.SupplierName;
            parameters[73].Value = model.ReferID;
            parameters[74].Value = model.ReferURL;
            parameters[75].Value = model.OrderIP;
            parameters[76].Value = model.Wdbh;
            parameters[77].Value = model.RemrkOne;
            parameters[78].Value = model.RemrkTwo;
            parameters[79].Value = model.Remark;
            parameters[80].Value = model.Dpxfjf;
            parameters[81].Value = model.CommentStatus;
            parameters[82].Value = model.OrderId;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            StringBuilder strSql6 = new StringBuilder();
            strSql6.Append("UPDATE OMS_OrderItems SET ShipmentQuantity=Quantity WHERE OrderId =@OrderId ");
            SqlParameter[] parameters6 = {
                    new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
            parameters6[0].Value = model.OrderId;
            cmd = new CommandInfo(strSql6.ToString(), parameters6);
            sqllist.Add(cmd);

            #endregion 更新动作

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
        /// 根据条件获取对应的订单状态的数量
        /// </summary>
        /// <param name="userid">下单人 ID</param>
        /// <param name="PaymentStatus">支付状态</param>
        /// <param name="OrderStatusCancel">订单的取消状态</param>
        /// <returns></returns>
        public int GetPaymentStatusCounts(int userid, int PaymentStatus, int OrderStatusCancel)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT (*) FROM  OMS_Orders WHERE BuyerID=@BuyerID AND PaymentStatus=@PaymentStatus AND OrderStatus!=@OrderStatus");
            SqlParameter[] parameters =
                {
                    new  SqlParameter("@BuyerID",userid),
                    new SqlParameter("@PaymentStatus",PaymentStatus),
                    new SqlParameter("@OrderStatus",OrderStatusCancel)
                };
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

        /// <summary>
        /// 更新订单备注
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateOrderRemark(long orderId, string Remark, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update OMS_Orders set ");
            strSql.Append("Remark=@Remark ");
            strSql.Append(" where OrderId=@OrderId ");
            if (!String.IsNullOrWhiteSpace(strWhere))
            {
                strSql.Append(strWhere);
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
                    new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = Remark;
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


        public YSWL.MALL.Model.Shop.Order.OrderInfo GetOrderInfo(string ordercode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from OMS_Orders ");
            strSql.Append(" where OrderCode=@OrderCode");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar,200)
            };
            parameters[0].Value = ordercode;

            YSWL.MALL.Model.Shop.Order.OrderInfo model = new YSWL.MALL.Model.Shop.Order.OrderInfo();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 统计业务员的销售以及订单数
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetSalesStatis(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ReferID, SUM(OrderTotal)AS OrderTotal,COUNT(1) AS OrderCount  ");
            strSql.Append(" FROM OMS_Orders T ");
            strSql.Append(" where ReferID>0  and OrderStatus <> -1 AND OrderType =1 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append("  and " + strWhere);
            }
            strSql.Append(" GROUP BY ReferID ORDER BY OrderTotal Desc");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }


        public Decimal GetOrderTotal(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select    SUM(OrderTotal)   ");
            strSql.Append(" FROM OMS_Orders T ");
            strSql.Append(" where ReferID>0 and OrderStatus <> -1 AND OrderType =1");
            if (strWhere.Trim() != "")
            {
                strSql.Append("  and " + strWhere);
            }
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeDecimal(obj, 0);
            }

        }

        /// <summary>
        /// 获取所有的消费积分
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public Decimal GetOrderDpxfjf(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select    SUM(Dpxfjf)   ");
            strSql.Append(" FROM OMS_Orders T ");
            strSql.Append(" where PaymentStatus>=2 and OrderStatus <> -1 AND OrderType =1");
            if (strWhere.Trim() != "")
            {
                strSql.Append("  and " + strWhere);
            }
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeDecimal(obj, 0);
            }

        }

        public int GetOrderCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select   Count(1)   ");
            strSql.Append(" FROM OMS_Orders T ");
            strSql.Append(" where ReferID>0 and  OrderStatus <> -1 AND OrderType =1");
            if (strWhere.Trim() != "")
            {
                strSql.Append("  and " + strWhere);
            }
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeInt(obj, 0);
            }
        }


        public DataSet GetMySalesStatis(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CONVERT(varchar(12) , CreatedDate, 111 )  AS Date  , SUM(OrderTotal)AS OrderTotal,COUNT(1) AS OrderCount  ");
            strSql.Append(" FROM OMS_Orders T ");
            strSql.AppendFormat(" where  OrderStatus <> -1 AND OrderType =1");
            if (strWhere.Trim() != "")
            {
                strSql.Append("  and " + strWhere);
            }
            strSql.Append(" GROUP BY CONVERT(varchar(12) , CreatedDate, 111 )  ORDER BY Date ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }


        public int GetCustomCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM (  ");
            strSql.Append("select distinct  BuyerID  FROM  OMS_Orders  ");
            strSql.Append(" where   OrderStatus <> -1 AND OrderType =1 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append("  and " + strWhere);
            }
            strSql.Append(" ) Temp");
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeInt(obj, 0);
            }
        }

        /// <summary>
        /// 拆单操作
        /// </summary>
        /// <param name="mainOrder">主单</param>
        /// <returns></returns>
        public long PakingMainOrder(YSWL.MALL.Model.Shop.Order.OrderInfo mainOrder)
        {

            long result = 0;
            //循环添加
            string updateSql =
            "Update OMS_Orders set OrderType=@OrderType,HasChildren=@HasChildren where OrderId=@OrderId";
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    object subOrderIdObj;
                    try
                    {//平台的单不需要再拆 所以返回的Id只有一个 
                        foreach (var orderInfo in mainOrder.SubOrders)
                        {
                            subOrderIdObj =
                                DBHelper.DefaultDBHelper.GetSingle4Trans(orderService.GenerateOrderInfo(orderInfo), transaction);
                            if (!(orderInfo.SupplierId.HasValue && orderInfo.SupplierId.Value > 0)) //商家为平台
                            {
                                result = Convert.ToInt64(subOrderIdObj);
                            }
                            orderInfo.OrderId = Common.Globals.SafeLong(subOrderIdObj, 0);
                            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateOrderItems(orderInfo), transaction);
                            //foreach (var orderItem in orderInfo.OrderItems)
                            //{
                            //    #region Add Order Item
                            //    StringBuilder stringAddItem = new StringBuilder();
                            //    stringAddItem.Append(" insert into OMS_OrderItems(OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl ,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice      ,AdjustedPrice,Attribute,Remark,Weight,Deduct ,Points,ProductLineId,SupplierId,SupplierName ,BrandId,BrandName)");
                            //    stringAddItem.Append(" select s.OrderId,s.OrderCode  ,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,AdjustedPrice,Attribute,t.Remark,t.Weight,Deduct,Points,ProductLineId,t.SupplierId,t.SupplierName,BrandId ,BrandName from OMS_Orders s, OMS_OrderItems t");
                            //    stringAddItem.AppendFormat(
                            //            " where s.OrderId={0} and s.ParentOrderId=t.OrderId and t.ProductId={1} ",
                            //            subOrderIdObj, orderItem.ProductId);
                            //    #endregion
                            //    DBHelper.DefaultDBHelper.GetSingle4Trans(new CommandInfo(stringAddItem.ToString(), new SqlParameter[0]), transaction);
                            //}
                        }
                        DBHelper.DefaultDBHelper.GetSingle4Trans(
                            new CommandInfo(updateSql, UpdateOrderParams(mainOrder)), transaction);
                        transaction.Commit();
                    }
                    catch (ArgumentNullException)
                    {
                        transaction.Rollback();
                        return -1;
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }

        public SqlParameter[] UpdateOrderParams(YSWL.MALL.Model.Shop.Order.OrderInfo model)
        {

            SqlParameter[] parameters =
                {
                    new SqlParameter("@OrderType", SqlDbType.Int),
                    new SqlParameter("@HasChildren", SqlDbType.Bit),
                    new SqlParameter("@OrderId", SqlDbType.Int)
                };
            parameters[0].Value = model.OrderType;
            parameters[1].Value = model.HasChildren;
            parameters[2].Value = model.OrderId;
            return parameters;
        }

        public DataSet GetBrandList(int supplierId)
        {
            string strSql = "select * from Shop_SupplierBrands  where  SupplierId=@SupplierId";
            SqlParameter[] parameters = { new SqlParameter("@SupplierId", SqlDbType.Int, 4) };
            parameters[0].Value = supplierId;
            return DBHelper.DefaultDBHelper.Query(strSql, parameters);
        }

        public bool UpdateSupplier(long orderId, int supplierId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update OMS_Orders set ");
            strSql.Append("SupplierId=@SupplierId");
            strSql.Append(" where OrderId=@OrderId");
            SqlParameter[] parameters = {
                    new SqlParameter("@SupplierId", SqlDbType.Int,4),
                    new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = supplierId;
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


        public DataSet GetSupplierCount(string startDate, string endDate, int paymentStatus = -1)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  select  COUNT(1) as Count,SUM(Amount) As  Amount,SupplierName,SupplierId  from OMS_Orders where SupplierId>0  ");
            strSql.AppendFormat(" and CreatedDate>{0} and  CreatedDate<'{1}'  ", startDate, endDate);
            if (paymentStatus > -1)
            {
                strSql.AppendFormat(" and   PaymentStatus={0}", paymentStatus);
            }
            strSql.Append(" GROUP BY SupplierId,SupplierName ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());

        }
        /// <summary>
        /// 根据购买用户Id获取是否是首单 排除已取消订单 （返回  true 为首单  false  为否）
        /// </summary>
        /// <param name="buyerID">购买用户Id</param>
        /// <returns></returns>
        public bool IsFirstOrder(int buyerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" select count(1)  from OMS_Orders  where  BuyerID = {0}  and OrderStatus!=-1   ", buyerID);
            return !DBHelper.DefaultDBHelper.Exists(strSql.ToString());
        }

        public YSWL.MALL.Model.Shop.Order.OrderInfo GetOrderByCoupon(string coupon)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from OMS_Orders ");
            strSql.Append(" where CouponCode=@CouponCode");
            SqlParameter[] parameters = {
                    new SqlParameter("@CouponCode", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = coupon;

            YSWL.MALL.Model.Shop.Order.OrderInfo model = new YSWL.MALL.Model.Shop.Order.OrderInfo();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        #region GenerateOrderItems

        private List<CommandInfo> GenerateOrderItems(OrderInfo orderInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            foreach (Model.Shop.Order.OrderItems model in orderInfo.OrderItems)
            {
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append("insert into OMS_OrderItems(");
                strSql.Append(
                    "OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,AdjustedPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType)");
                strSql.Append(" values (");
                strSql.Append(
                    "@OrderId,@OrderCode,@ProductId,@ProductCode,@SKU,@Name,@ThumbnailsUrl,@Description,@Quantity,@ShipmentQuantity,@CostPrice,@SellPrice,@AdjustedPrice,@Attribute,@Remark,@Weight,@Deduct,@Points,@ProductLineId,@SupplierId,@SupplierName,@BrandId,@BrandName,@ProductType)");
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
                            new SqlParameter("@ProductType", SqlDbType.SmallInt,2)
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
                parameters[22].Value = null;
                parameters[23].Value = model.ProductType;
                #endregion

                list.Add(new CommandInfo(strSql.ToString(),
                                         parameters, EffentNextType.ExcuteEffectRows));
            }
            return list;
        }

        #endregion

        public bool UpdateOrderTypeSub(long orderId, int orderTypeSub)
        {
            string strSql = string.Format("update OMS_Orders set orderTypeSub={0} where orderId={1}", orderTypeSub, orderId);
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否允许修改当前订单的状态  true 允许  false 不允许
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="ShippingStatus"></param>
        /// <param name="OrderStatus"></param>
        /// <returns></returns>
        public bool IsAllowModify(long OrderId, int ShippingStatus, int OrderStatus)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from OMS_Orders");
            strSql.Append(" where OrderId=@OrderId ");
            strSql.AppendFormat("  and  ShippingStatus={0}  and  OrderStatus={1} ", ShippingStatus, OrderStatus);
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderId", SqlDbType.BigInt)
            };
            parameters[0].Value = OrderId;
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

        public int GetUnPaidCounts(int userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT (1)  FROM  OMS_Orders WHERE BuyerID=@BuyerID AND PaymentStatus=0 AND OrderStatus!=-1 and OrderType=1  and PaymentGateway<>'cod' ");
            SqlParameter[] parameters =
                {
                    new  SqlParameter("@BuyerID",userid)
                };
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

        public YSWL.MALL.Model.Shop.Order.OrderInfo GetModel(string orderCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 *  from OMS_Orders ");
            strSql.Append(" where OrderCode=@OrderCode");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = orderCode;

            YSWL.MALL.Model.Shop.Order.OrderInfo model = new YSWL.MALL.Model.Shop.Order.OrderInfo();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        ///  根据日期获取订单数及总支付金额
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet GetOrderCountAmount(DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(orderid) as OrderCount, SUM(Amount) as TotalAmount  ");
            strSql.Append(" FROM OMS_Orders T ");
            strSql.Append(" where   OrderStatus <> -1 AND OrderType =1 ");
            strSql.AppendFormat("  and CreatedDate >='{0}'  and CreatedDate < '{1}' ", startDate, endDate);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 按天统计订单金额
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet StatOrderAmount(DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select convert(varchar(12), CreatedDate, 111) as CreatedDate , SUM(Amount) as TotalAmount , COUNT(OrderId) as OrderCount from OMS_Orders ");
            strSql.AppendFormat(" where   CreatedDate>='{0}' ", startDate);
            strSql.AppendFormat(" and  CreatedDate<'{0}' ", endDate);
            strSql.Append("  group by  convert(varchar(12), CreatedDate, 111) ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        ///   统计用户订单数及金额
        /// </summary>
        /// <param name="top"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet StatBuyerOrderCountAmount(int top, DateTime startDate, DateTime endDate)
        {
            //select top 10 BuyerID,BuyerName,COUNT(orderid) as orderCount,SUM(Amount) as TotalAmount from OMS_Orders where OrderStatus <> -1--and CreatedDate>='{0}' and CreatedDate< '{1}'  group by BuyerID, BuyerName order by TotalAmount desc
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ");
            if (top > 0)
            {
                strSql.AppendFormat(" top {0} ", top);
            }
            strSql.Append(" BuyerID,BuyerName,COUNT(orderid) as orderCount,SUM(Amount) as TotalAmount ");
            strSql.Append(" FROM OMS_Orders T ");
            strSql.Append(" where   OrderStatus <> -1 AND OrderType =1 ");
            strSql.AppendFormat("  and CreatedDate >='{0}'  and CreatedDate < '{1}' ", startDate, endDate);
            strSql.Append("  group by BuyerID, BuyerName order by TotalAmount desc ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 获取未付款金额 (不排除货到付款)
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetUnPaidAmount(int userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT SUM(Amount)  FROM  OMS_Orders WHERE BuyerID=@BuyerID AND PaymentStatus=0 AND OrderStatus!=-1 and OrderType=1  ");
            SqlParameter[] parameters =
                {
                    new  SqlParameter("@BuyerID",userid)
                };
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeDecimal(obj, 0);
            }
        }
        /// <summary>
        /// 获取已付款金额 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetPaidAmount(int userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT SUM(Amount)  FROM  OMS_Orders WHERE BuyerID=@BuyerID AND PaymentStatus=2 AND OrderStatus!=-1 and OrderType=1  ");
            SqlParameter[] parameters =
                {
                    new  SqlParameter("@BuyerID",userid)
                };
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeDecimal(obj, 0);
            }
        }
        #endregion  ExtensionMethod

        #region pos统计接口

        /// <summary>
        ///  根据日期获取订单数及总支付金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">订单来源</param>
        /// <returns></returns>
        public DataSet GetOrderCountAmountByUser(int userId, DateTime startDate, DateTime endDate, int referType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(orderid) as OrderCount, SUM(Amount) as TotalAmount  ");
            strSql.Append(" FROM OMS_Orders T ");
            strSql.AppendFormat(" where referType={0} and OrderStatus <> -1 AND OrderType =1 ", referType);
            strSql.AppendFormat("  and CreatedDate >='{0}'  and CreatedDate < '{1}' ", startDate, endDate);
            strSql.AppendFormat("  and CreateUserId ={0}", userId);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取未付款金额 (不排除货到付款)
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">订单来源</param>
        /// <returns></returns>
        public decimal GetUnPaidAmountByUser(int userid, DateTime startDate, DateTime endDate, int referType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" SELECT SUM(Amount)  FROM  OMS_Orders WHERE referType={0} and CreateUserId=@CreateUserId AND PaymentStatus=0 AND OrderStatus!=-1 and OrderType=1  ", referType);
            strSql.AppendFormat("  and CreatedDate >='{0}' and CreatedDate < '{1}' ", startDate, endDate);
            SqlParameter[] parameters =
            {
                new  SqlParameter("@CreateUserId",userid)
            };
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeDecimal(obj, 0);
            }
        }

        /// <summary>
        /// 获取已付款金额 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">订单来源</param>
        /// <returns></returns>
        public decimal GetPaidAmountByUser(int userid, DateTime startDate, DateTime endDate, int referType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(" SELECT SUM(Amount)  FROM  OMS_Orders WHERE referType={0} and CreateUserId=@CreateUserId AND PaymentStatus=2 AND OrderStatus!=-1 and OrderType=1  ", referType);
            strSql.AppendFormat("  and CreatedDate >='{0}' and CreatedDate < '{1}' ", startDate, endDate);
            SqlParameter[] parameters =
            {
                new  SqlParameter("@CreateUserId",userid)
            };
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeDecimal(obj, 0);
            }
        }

        /// <summary>
        /// 获取支付类型方式
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">订单来源</param>
        /// <returns></returns>
        public DataSet GetPaymentByUser(int userId, DateTime startDate, DateTime endDate, int referType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PaymentTypeName,sum(Amount) as Amount  ");
            strSql.Append(" FROM OMS_Orders T ");
            strSql.AppendFormat(" where referType={0} and OrderStatus <> -1 AND OrderType =1 and PaymentStatus=2", referType);
            strSql.AppendFormat("  and CreatedDate >='{0}'  and CreatedDate < '{1}' ", startDate, endDate);
            strSql.AppendFormat("  and CreateUserId ={0}", userId);
            strSql.Append("  group by PaymentTypeName");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取当前用户的订单项
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">订单来源</param>
        /// <returns></returns>
        public DataSet GetOrderItemByUser(int userId, DateTime startDate, DateTime endDate, int referType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from OMS_OrderItems");
            strSql.AppendFormat(" where exists (select OrderId from OMS_Orders where OMS_Orders.OrderId=OMS_OrderItems.OrderId and referType={0} and OrderStatus <> -1 AND OrderType =1", referType);
            strSql.AppendFormat("  and CreatedDate >='{0}' and CreatedDate < '{1}'", startDate, endDate);
            strSql.AppendFormat("  and CreateUserId ={0})", userId);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        #endregion
    }
}

