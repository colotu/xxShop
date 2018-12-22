/**  版本信息模板在安装目录下，可自行修改。
* ReturnOrders.cs
*
* 功 能： N/A
* 类 名： ReturnOrders
* 负责人   [hhy]
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/2 11:50:36   N/A    初版   
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.ReturnOrder;
using YSWL.DBUtility;
using System.Collections.Generic;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.SQLServerDAL.Shop.Order;

//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.ReturnOrder
{
	/// <summary>
	/// 数据访问类:ReturnOrders
	/// </summary>
	public partial class ReturnOrders:IReturnOrders
	{
		public ReturnOrders()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ReturnOrderId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_ReturnOrders");
            strSql.Append(" where ReturnOrderId=@ReturnOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt)
			};
            parameters[0].Value = ReturnOrderId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_ReturnOrders(");
            strSql.Append("ReturnOrderCode,OrderId,OrderCode,ReturnUserId,ReturnUserName,CreateUserId,CreatedDate,UpdatedUserId,UpdatedDate,SupplierId,SupplierName,CouponCode,CouponName,CouponAmount,CouponValue,CouponValueType,ReturnGoodsType,ReturnCoupon,ActualSalesTotal,Amount,AmountAdjusted,AmountActual,ServiceType,Credential,Description,ImageUrl,ReturnType,PickRegionId,PickRegion,PickAddress,PickZipCode,PickName,PickTelPhone,PickCellPhone,PickEmail,ShippingModeId,ShippingModeName,ShipOrderNumber,ExpressCompanyName,ExpressCompanyAbb,ReturnTrueName,ReturnBankName,ReturnCard,ReturnCardType,ContactName,ContactPhone,Status,RefundStatus,LogisticStatus,RefuseReason,CustomerReview,Remark)");
            strSql.Append(" values (");
            strSql.Append("@ReturnOrderCode,@OrderId,@OrderCode,@ReturnUserId,@ReturnUserName,@CreateUserId,@CreatedDate,@UpdatedUserId,@UpdatedDate,@SupplierId,@SupplierName,@CouponCode,@CouponName,@CouponAmount,@CouponValue,@CouponValueType,@ReturnGoodsType,@ReturnCoupon,@ActualSalesTotal,@Amount,@AmountAdjusted,@AmountActual,@ServiceType,@Credential,@Description,@ImageUrl,@ReturnType,@PickRegionId,@PickRegion,@PickAddress,@PickZipCode,@PickName,@PickTelPhone,@PickCellPhone,@PickEmail,@ShippingModeId,@ShippingModeName,@ShipOrderNumber,@ExpressCompanyName,@ExpressCompanyAbb,@ReturnTrueName,@ReturnBankName,@ReturnCard,@ReturnCardType,@ContactName,@ContactPhone,@Status,@RefundStatus,@LogisticStatus,@RefuseReason,@CustomerReview,@Remark)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@ReturnUserId", SqlDbType.Int,4),
					new SqlParameter("@ReturnUserName", SqlDbType.NVarChar,100),
					new SqlParameter("@CreateUserId", SqlDbType.Int,4),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@UpdatedUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdatedDate", SqlDbType.DateTime),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
					new SqlParameter("@CouponCode", SqlDbType.NVarChar,50),
					new SqlParameter("@CouponName", SqlDbType.NVarChar,100),
					new SqlParameter("@CouponAmount", SqlDbType.Money,8),
					new SqlParameter("@CouponValue", SqlDbType.Money,8),
					new SqlParameter("@CouponValueType", SqlDbType.SmallInt,2),
					new SqlParameter("@ReturnGoodsType", SqlDbType.SmallInt,2),
					new SqlParameter("@ReturnCoupon", SqlDbType.SmallInt,2),
					new SqlParameter("@ActualSalesTotal", SqlDbType.Money,8),
					new SqlParameter("@Amount", SqlDbType.Money,8),
					new SqlParameter("@AmountAdjusted", SqlDbType.Money,8),
					new SqlParameter("@AmountActual", SqlDbType.Money,8),
					new SqlParameter("@ServiceType", SqlDbType.SmallInt,2),
					new SqlParameter("@Credential", SqlDbType.NVarChar,100),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@ImageUrl", SqlDbType.Text),
					new SqlParameter("@ReturnType", SqlDbType.SmallInt,2),
					new SqlParameter("@PickRegionId", SqlDbType.Int,4),
					new SqlParameter("@PickRegion", SqlDbType.NVarChar,300),
					new SqlParameter("@PickAddress", SqlDbType.NVarChar,300),
					new SqlParameter("@PickZipCode", SqlDbType.NVarChar,20),
					new SqlParameter("@PickName", SqlDbType.NVarChar,50),
					new SqlParameter("@PickTelPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@PickCellPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@PickEmail", SqlDbType.NVarChar,100),
					new SqlParameter("@ShippingModeId", SqlDbType.Int,4),
					new SqlParameter("@ShippingModeName", SqlDbType.NVarChar,100),
					new SqlParameter("@ShipOrderNumber", SqlDbType.NVarChar,50),
					new SqlParameter("@ExpressCompanyName", SqlDbType.NVarChar,500),
					new SqlParameter("@ExpressCompanyAbb", SqlDbType.NVarChar,500),
					new SqlParameter("@ReturnTrueName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReturnBankName", SqlDbType.NVarChar,200),
					new SqlParameter("@ReturnCard", SqlDbType.NVarChar,50),
					new SqlParameter("@ReturnCardType", SqlDbType.Int,4),
					new SqlParameter("@ContactName", SqlDbType.NVarChar,100),
					new SqlParameter("@ContactPhone", SqlDbType.NVarChar,100),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@RefundStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@LogisticStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@RefuseReason", SqlDbType.NVarChar,500),
					new SqlParameter("@CustomerReview", SqlDbType.SmallInt,2),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000)};
            parameters[0].Value = model.ReturnOrderCode;
            parameters[1].Value = model.OrderId;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.ReturnUserId;
            parameters[4].Value = model.ReturnUserName;
            parameters[5].Value = model.CreateUserId;
            parameters[6].Value = model.CreatedDate;
            parameters[7].Value = model.UpdatedUserId;
            parameters[8].Value = model.UpdatedDate;
            parameters[9].Value = model.SupplierId;
            parameters[10].Value = model.SupplierName;
            parameters[11].Value = model.CouponCode;
            parameters[12].Value = model.CouponName;
            parameters[13].Value = model.CouponAmount;
            parameters[14].Value = model.CouponValue;
            parameters[15].Value = model.CouponValueType;
            parameters[16].Value = model.ReturnGoodsType;
            parameters[17].Value = model.ReturnCoupon;
            parameters[18].Value = model.ActualSalesTotal;
            parameters[19].Value = model.Amount;
            parameters[20].Value = model.AmountAdjusted;
            parameters[21].Value = model.AmountActual;
            parameters[22].Value = model.ServiceType;
            parameters[23].Value = model.Credential;
            parameters[24].Value = model.Description;
            parameters[25].Value = model.ImageUrl;
            parameters[26].Value = model.ReturnType;
            parameters[27].Value = model.PickRegionId;
            parameters[28].Value = model.PickRegion;
            parameters[29].Value = model.PickAddress;
            parameters[30].Value = model.PickZipCode;
            parameters[31].Value = model.PickName;
            parameters[32].Value = model.PickTelPhone;
            parameters[33].Value = model.PickCellPhone;
            parameters[34].Value = model.PickEmail;
            parameters[35].Value = model.ShippingModeId;
            parameters[36].Value = model.ShippingModeName;
            parameters[37].Value = model.ShipOrderNumber;
            parameters[38].Value = model.ExpressCompanyName;
            parameters[39].Value = model.ExpressCompanyAbb;
            parameters[40].Value = model.ReturnTrueName;
            parameters[41].Value = model.ReturnBankName;
            parameters[42].Value = model.ReturnCard;
            parameters[43].Value = model.ReturnCardType;
            parameters[44].Value = model.ContactName;
            parameters[45].Value = model.ContactPhone;
            parameters[46].Value = model.Status;
            parameters[47].Value = model.RefundStatus;
            parameters[48].Value = model.LogisticStatus;
            parameters[49].Value = model.RefuseReason;
            parameters[50].Value = model.CustomerReview;
            parameters[51].Value = model.Remark;

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
        public bool Update(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ReturnOrders set ");
            strSql.Append("ReturnOrderCode=@ReturnOrderCode,");
            strSql.Append("OrderId=@OrderId,");
            strSql.Append("OrderCode=@OrderCode,");
            strSql.Append("ReturnUserId=@ReturnUserId,");
            strSql.Append("ReturnUserName=@ReturnUserName,");
            strSql.Append("CreateUserId=@CreateUserId,");
            strSql.Append("CreatedDate=@CreatedDate,");
            strSql.Append("UpdatedUserId=@UpdatedUserId,");
            strSql.Append("UpdatedDate=@UpdatedDate,");
            strSql.Append("SupplierId=@SupplierId,");
            strSql.Append("SupplierName=@SupplierName,");
            strSql.Append("CouponCode=@CouponCode,");
            strSql.Append("CouponName=@CouponName,");
            strSql.Append("CouponAmount=@CouponAmount,");
            strSql.Append("CouponValue=@CouponValue,");
            strSql.Append("CouponValueType=@CouponValueType,");
            strSql.Append("ReturnGoodsType=@ReturnGoodsType,");
            strSql.Append("ReturnCoupon=@ReturnCoupon,");
            strSql.Append("ActualSalesTotal=@ActualSalesTotal,");
            strSql.Append("Amount=@Amount,");
            strSql.Append("AmountAdjusted=@AmountAdjusted,");
            strSql.Append("AmountActual=@AmountActual,");
            strSql.Append("ServiceType=@ServiceType,");
            strSql.Append("Credential=@Credential,");
            strSql.Append("Description=@Description,");
            strSql.Append("ImageUrl=@ImageUrl,");
            strSql.Append("ReturnType=@ReturnType,");
            strSql.Append("PickRegionId=@PickRegionId,");
            strSql.Append("PickRegion=@PickRegion,");
            strSql.Append("PickAddress=@PickAddress,");
            strSql.Append("PickZipCode=@PickZipCode,");
            strSql.Append("PickName=@PickName,");
            strSql.Append("PickTelPhone=@PickTelPhone,");
            strSql.Append("PickCellPhone=@PickCellPhone,");
            strSql.Append("PickEmail=@PickEmail,");
            strSql.Append("ShippingModeId=@ShippingModeId,");
            strSql.Append("ShippingModeName=@ShippingModeName,");
            strSql.Append("ShipOrderNumber=@ShipOrderNumber,");
            strSql.Append("ExpressCompanyName=@ExpressCompanyName,");
            strSql.Append("ExpressCompanyAbb=@ExpressCompanyAbb,");
            strSql.Append("ReturnTrueName=@ReturnTrueName,");
            strSql.Append("ReturnBankName=@ReturnBankName,");
            strSql.Append("ReturnCard=@ReturnCard,");
            strSql.Append("ReturnCardType=@ReturnCardType,");
            strSql.Append("ContactName=@ContactName,");
            strSql.Append("ContactPhone=@ContactPhone,");
            strSql.Append("Status=@Status,");
            strSql.Append("RefundStatus=@RefundStatus,");
            strSql.Append("LogisticStatus=@LogisticStatus,");
            strSql.Append("RefuseReason=@RefuseReason,");
            strSql.Append("CustomerReview=@CustomerReview,");
            strSql.Append("Remark=@Remark");
            strSql.Append(" where ReturnOrderId=@ReturnOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@ReturnUserId", SqlDbType.Int,4),
					new SqlParameter("@ReturnUserName", SqlDbType.NVarChar,100),
					new SqlParameter("@CreateUserId", SqlDbType.Int,4),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@UpdatedUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdatedDate", SqlDbType.DateTime),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
					new SqlParameter("@CouponCode", SqlDbType.NVarChar,50),
					new SqlParameter("@CouponName", SqlDbType.NVarChar,100),
					new SqlParameter("@CouponAmount", SqlDbType.Money,8),
					new SqlParameter("@CouponValue", SqlDbType.Money,8),
					new SqlParameter("@CouponValueType", SqlDbType.SmallInt,2),
					new SqlParameter("@ReturnGoodsType", SqlDbType.SmallInt,2),
					new SqlParameter("@ReturnCoupon", SqlDbType.SmallInt,2),
					new SqlParameter("@ActualSalesTotal", SqlDbType.Money,8),
					new SqlParameter("@Amount", SqlDbType.Money,8),
					new SqlParameter("@AmountAdjusted", SqlDbType.Money,8),
					new SqlParameter("@AmountActual", SqlDbType.Money,8),
					new SqlParameter("@ServiceType", SqlDbType.SmallInt,2),
					new SqlParameter("@Credential", SqlDbType.NVarChar,100),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@ImageUrl", SqlDbType.Text),
					new SqlParameter("@ReturnType", SqlDbType.SmallInt,2),
					new SqlParameter("@PickRegionId", SqlDbType.Int,4),
					new SqlParameter("@PickRegion", SqlDbType.NVarChar,300),
					new SqlParameter("@PickAddress", SqlDbType.NVarChar,300),
					new SqlParameter("@PickZipCode", SqlDbType.NVarChar,20),
					new SqlParameter("@PickName", SqlDbType.NVarChar,50),
					new SqlParameter("@PickTelPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@PickCellPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@PickEmail", SqlDbType.NVarChar,100),
					new SqlParameter("@ShippingModeId", SqlDbType.Int,4),
					new SqlParameter("@ShippingModeName", SqlDbType.NVarChar,100),
					new SqlParameter("@ShipOrderNumber", SqlDbType.NVarChar,50),
					new SqlParameter("@ExpressCompanyName", SqlDbType.NVarChar,500),
					new SqlParameter("@ExpressCompanyAbb", SqlDbType.NVarChar,500),
					new SqlParameter("@ReturnTrueName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReturnBankName", SqlDbType.NVarChar,200),
					new SqlParameter("@ReturnCard", SqlDbType.NVarChar,50),
					new SqlParameter("@ReturnCardType", SqlDbType.Int,4),
					new SqlParameter("@ContactName", SqlDbType.NVarChar,100),
					new SqlParameter("@ContactPhone", SqlDbType.NVarChar,100),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@RefundStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@LogisticStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@RefuseReason", SqlDbType.NVarChar,500),
					new SqlParameter("@CustomerReview", SqlDbType.SmallInt,2),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.ReturnOrderCode;
            parameters[1].Value = model.OrderId;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.ReturnUserId;
            parameters[4].Value = model.ReturnUserName;
            parameters[5].Value = model.CreateUserId;
            parameters[6].Value = model.CreatedDate;
            parameters[7].Value = model.UpdatedUserId;
            parameters[8].Value = model.UpdatedDate;
            parameters[9].Value = model.SupplierId;
            parameters[10].Value = model.SupplierName;
            parameters[11].Value = model.CouponCode;
            parameters[12].Value = model.CouponName;
            parameters[13].Value = model.CouponAmount;
            parameters[14].Value = model.CouponValue;
            parameters[15].Value = model.CouponValueType;
            parameters[16].Value = model.ReturnGoodsType;
            parameters[17].Value = model.ReturnCoupon;
            parameters[18].Value = model.ActualSalesTotal;
            parameters[19].Value = model.Amount;
            parameters[20].Value = model.AmountAdjusted;
            parameters[21].Value = model.AmountActual;
            parameters[22].Value = model.ServiceType;
            parameters[23].Value = model.Credential;
            parameters[24].Value = model.Description;
            parameters[25].Value = model.ImageUrl;
            parameters[26].Value = model.ReturnType;
            parameters[27].Value = model.PickRegionId;
            parameters[28].Value = model.PickRegion;
            parameters[29].Value = model.PickAddress;
            parameters[30].Value = model.PickZipCode;
            parameters[31].Value = model.PickName;
            parameters[32].Value = model.PickTelPhone;
            parameters[33].Value = model.PickCellPhone;
            parameters[34].Value = model.PickEmail;
            parameters[35].Value = model.ShippingModeId;
            parameters[36].Value = model.ShippingModeName;
            parameters[37].Value = model.ShipOrderNumber;
            parameters[38].Value = model.ExpressCompanyName;
            parameters[39].Value = model.ExpressCompanyAbb;
            parameters[40].Value = model.ReturnTrueName;
            parameters[41].Value = model.ReturnBankName;
            parameters[42].Value = model.ReturnCard;
            parameters[43].Value = model.ReturnCardType;
            parameters[44].Value = model.ContactName;
            parameters[45].Value = model.ContactPhone;
            parameters[46].Value = model.Status;
            parameters[47].Value = model.RefundStatus;
            parameters[48].Value = model.LogisticStatus;
            parameters[49].Value = model.RefuseReason;
            parameters[50].Value = model.CustomerReview;
            parameters[51].Value = model.Remark;
            parameters[52].Value = model.ReturnOrderId;

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
        public bool Delete(long ReturnOrderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_ReturnOrders ");
            strSql.Append(" where ReturnOrderId=@ReturnOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt)
			};
            parameters[0].Value = ReturnOrderId;

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
        public bool DeleteList(string ReturnOrderIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_ReturnOrders ");
            strSql.Append(" where ReturnOrderId in (" + ReturnOrderIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders GetModel(long ReturnOrderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ReturnOrderId,ReturnOrderCode,OrderId,OrderCode,ReturnUserId,ReturnUserName,CreateUserId,CreatedDate,UpdatedUserId,UpdatedDate,SupplierId,SupplierName,CouponCode,CouponName,CouponAmount,CouponValue,CouponValueType,ReturnGoodsType,ReturnCoupon,ActualSalesTotal,Amount,AmountAdjusted,AmountActual,ServiceType,Credential,Description,ImageUrl,ReturnType,PickRegionId,PickRegion,PickAddress,PickZipCode,PickName,PickTelPhone,PickCellPhone,PickEmail,ShippingModeId,ShippingModeName,ShipOrderNumber,ExpressCompanyName,ExpressCompanyAbb,ReturnTrueName,ReturnBankName,ReturnCard,ReturnCardType,ContactName,ContactPhone,Status,RefundStatus,LogisticStatus,RefuseReason,CustomerReview,Remark from Shop_ReturnOrders ");
            strSql.Append(" where ReturnOrderId=@ReturnOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt)
			};
            parameters[0].Value = ReturnOrderId;

            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = new YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders();
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
        public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = new YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders();
            if (row != null)
            {
                if (row["ReturnOrderId"] != null && row["ReturnOrderId"].ToString() != "")
                {
                    model.ReturnOrderId = long.Parse(row["ReturnOrderId"].ToString());
                }
                if (row["ReturnOrderCode"] != null)
                {
                    model.ReturnOrderCode = row["ReturnOrderCode"].ToString();
                }
                if (row["OrderId"] != null && row["OrderId"].ToString() != "")
                {
                    model.OrderId = long.Parse(row["OrderId"].ToString());
                }
                if (row["OrderCode"] != null)
                {
                    model.OrderCode = row["OrderCode"].ToString();
                }
                if (row["ReturnUserId"] != null && row["ReturnUserId"].ToString() != "")
                {
                    model.ReturnUserId = int.Parse(row["ReturnUserId"].ToString());
                }
                if (row["ReturnUserName"] != null)
                {
                    model.ReturnUserName = row["ReturnUserName"].ToString();
                }
                if (row["CreateUserId"] != null && row["CreateUserId"].ToString() != "")
                {
                    model.CreateUserId = int.Parse(row["CreateUserId"].ToString());
                }
                if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                }
                if (row["UpdatedUserId"] != null && row["UpdatedUserId"].ToString() != "")
                {
                    model.UpdatedUserId = int.Parse(row["UpdatedUserId"].ToString());
                }
                if (row["UpdatedDate"] != null && row["UpdatedDate"].ToString() != "")
                {
                    model.UpdatedDate = DateTime.Parse(row["UpdatedDate"].ToString());
                }
                if (row["SupplierId"] != null && row["SupplierId"].ToString() != "")
                {
                    model.SupplierId = int.Parse(row["SupplierId"].ToString());
                }
                if (row["SupplierName"] != null)
                {
                    model.SupplierName = row["SupplierName"].ToString();
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
                if (row["ReturnGoodsType"] != null && row["ReturnGoodsType"].ToString() != "")
                {
                    model.ReturnGoodsType = int.Parse(row["ReturnGoodsType"].ToString());
                }
                if (row["ReturnCoupon"] != null && row["ReturnCoupon"].ToString() != "")
                {
                    model.ReturnCoupon = int.Parse(row["ReturnCoupon"].ToString());
                }
                if (row["ActualSalesTotal"] != null && row["ActualSalesTotal"].ToString() != "")
                {
                    model.ActualSalesTotal = decimal.Parse(row["ActualSalesTotal"].ToString());
                }
                if (row["Amount"] != null && row["Amount"].ToString() != "")
                {
                    model.Amount = decimal.Parse(row["Amount"].ToString());
                }
                if (row["AmountAdjusted"] != null && row["AmountAdjusted"].ToString() != "")
                {
                    model.AmountAdjusted = decimal.Parse(row["AmountAdjusted"].ToString());
                }
                if (row["AmountActual"] != null && row["AmountActual"].ToString() != "")
                {
                    model.AmountActual = decimal.Parse(row["AmountActual"].ToString());
                }
                if (row["ServiceType"] != null && row["ServiceType"].ToString() != "")
                {
                    model.ServiceType = int.Parse(row["ServiceType"].ToString());
                }
                if (row["Credential"] != null)
                {
                    model.Credential = row["Credential"].ToString();
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["ImageUrl"] != null)
                {
                    model.ImageUrl = row["ImageUrl"].ToString();
                }
                if (row["ReturnType"] != null && row["ReturnType"].ToString() != "")
                {
                    model.ReturnType = int.Parse(row["ReturnType"].ToString());
                }
                if (row["PickRegionId"] != null && row["PickRegionId"].ToString() != "")
                {
                    model.PickRegionId = int.Parse(row["PickRegionId"].ToString());
                }
                if (row["PickRegion"] != null)
                {
                    model.PickRegion = row["PickRegion"].ToString();
                }
                if (row["PickAddress"] != null)
                {
                    model.PickAddress = row["PickAddress"].ToString();
                }
                if (row["PickZipCode"] != null)
                {
                    model.PickZipCode = row["PickZipCode"].ToString();
                }
                if (row["PickName"] != null)
                {
                    model.PickName = row["PickName"].ToString();
                }
                if (row["PickTelPhone"] != null)
                {
                    model.PickTelPhone = row["PickTelPhone"].ToString();
                }
                if (row["PickCellPhone"] != null)
                {
                    model.PickCellPhone = row["PickCellPhone"].ToString();
                }
                if (row["PickEmail"] != null)
                {
                    model.PickEmail = row["PickEmail"].ToString();
                }
                if (row["ShippingModeId"] != null && row["ShippingModeId"].ToString() != "")
                {
                    model.ShippingModeId = int.Parse(row["ShippingModeId"].ToString());
                }
                if (row["ShippingModeName"] != null)
                {
                    model.ShippingModeName = row["ShippingModeName"].ToString();
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
                if (row["ReturnTrueName"] != null)
                {
                    model.ReturnTrueName = row["ReturnTrueName"].ToString();
                }
                if (row["ReturnBankName"] != null)
                {
                    model.ReturnBankName = row["ReturnBankName"].ToString();
                }
                if (row["ReturnCard"] != null)
                {
                    model.ReturnCard = row["ReturnCard"].ToString();
                }
                if (row["ReturnCardType"] != null && row["ReturnCardType"].ToString() != "")
                {
                    model.ReturnCardType = int.Parse(row["ReturnCardType"].ToString());
                }
                if (row["ContactName"] != null)
                {
                    model.ContactName = row["ContactName"].ToString();
                }
                if (row["ContactPhone"] != null)
                {
                    model.ContactPhone = row["ContactPhone"].ToString();
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["RefundStatus"] != null && row["RefundStatus"].ToString() != "")
                {
                    model.RefundStatus = int.Parse(row["RefundStatus"].ToString());
                }
                if (row["LogisticStatus"] != null && row["LogisticStatus"].ToString() != "")
                {
                    model.LogisticStatus = int.Parse(row["LogisticStatus"].ToString());
                }
                if (row["RefuseReason"] != null)
                {
                    model.RefuseReason = row["RefuseReason"].ToString();
                }
                if (row["CustomerReview"] != null && row["CustomerReview"].ToString() != "")
                {
                    model.CustomerReview = int.Parse(row["CustomerReview"].ToString());
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
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
            strSql.Append("select ReturnOrderId,ReturnOrderCode,OrderId,OrderCode,ReturnUserId,ReturnUserName,CreateUserId,CreatedDate,UpdatedUserId,UpdatedDate,SupplierId,SupplierName,CouponCode,CouponName,CouponAmount,CouponValue,CouponValueType,ReturnGoodsType,ReturnCoupon,ActualSalesTotal,Amount,AmountAdjusted,AmountActual,ServiceType,Credential,Description,ImageUrl,ReturnType,PickRegionId,PickRegion,PickAddress,PickZipCode,PickName,PickTelPhone,PickCellPhone,PickEmail,ShippingModeId,ShippingModeName,ShipOrderNumber,ExpressCompanyName,ExpressCompanyAbb,ReturnTrueName,ReturnBankName,ReturnCard,ReturnCardType,ContactName,ContactPhone,Status,RefundStatus,LogisticStatus,RefuseReason,CustomerReview,Remark ");
            strSql.Append(" FROM Shop_ReturnOrders ");
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
            strSql.Append(" ReturnOrderId,ReturnOrderCode,OrderId,OrderCode,ReturnUserId,ReturnUserName,CreateUserId,CreatedDate,UpdatedUserId,UpdatedDate,SupplierId,SupplierName,CouponCode,CouponName,CouponAmount,CouponValue,CouponValueType,ReturnGoodsType,ReturnCoupon,ActualSalesTotal,Amount,AmountAdjusted,AmountActual,ServiceType,Credential,Description,ImageUrl,ReturnType,PickRegionId,PickRegion,PickAddress,PickZipCode,PickName,PickTelPhone,PickCellPhone,PickEmail,ShippingModeId,ShippingModeName,ShipOrderNumber,ExpressCompanyName,ExpressCompanyAbb,ReturnTrueName,ReturnBankName,ReturnCard,ReturnCardType,ContactName,ContactPhone,Status,RefundStatus,LogisticStatus,RefuseReason,CustomerReview,Remark ");
            strSql.Append(" FROM Shop_ReturnOrders ");
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
            strSql.Append("select count(1) FROM Shop_ReturnOrders ");
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
                strSql.Append("order by T.ReturnOrderId desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_ReturnOrders T ");
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
            parameters[0].Value = "Shop_ReturnOrders";
            parameters[1].Value = "ReturnOrderId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod

		#region  ExtensionMethod
        /// <summary>
        /// 根据订单号获取退货记录总数 (不包含已取消的记录)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public int GetCountByOrderId(long orderId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Shop_ReturnOrders ");
            strSql.AppendFormat("  where  OrderId={0}  and  Status<>-1 and  Status<>-3 ", orderId);
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

        #region 创建
        public long CreateReturnOrder(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders returnOrders, Accounts.Bus.User currentUser)
        {
            using (SqlConnection connection =  DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    object result;
                    try
                    {
                        //DONE: 1.新增退单
                        result = DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateReturnOrderInfo(returnOrders), transaction);
                     
                        //加载主键
                        returnOrders.ReturnOrderId = Common.Globals.SafeLong(result, 0);

                        //DONE: 2.新增退单项目
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateReturnOrderItems(returnOrders), transaction);

                        //DONE: 3.新增退单创建记录
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateReturnOrderAction(returnOrders,currentUser), transaction);

                        transaction.Commit();
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return returnOrders.ReturnOrderId;
        }
      

        #region GenerateReturnOrderInfo

        public CommandInfo GenerateReturnOrderInfo(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_ReturnOrders(");
            strSql.Append("ReturnOrderCode,OrderId,OrderCode,ReturnUserId,ReturnUserName,CreateUserId,CreatedDate,UpdatedUserId,UpdatedDate,SupplierId,SupplierName,CouponCode,CouponName,CouponAmount,CouponValue,CouponValueType,ReturnGoodsType,ReturnCoupon,ActualSalesTotal,Amount,AmountAdjusted,AmountActual,ServiceType,Credential,Description,ImageUrl,ReturnType,PickRegionId,PickRegion,PickAddress,PickZipCode,PickName,PickTelPhone,PickCellPhone,PickEmail,ShippingModeId,ShippingModeName,ShipOrderNumber,ExpressCompanyName,ExpressCompanyAbb,ReturnTrueName,ReturnBankName,ReturnCard,ReturnCardType,ContactName,ContactPhone,Status,RefundStatus,LogisticStatus,RefuseReason,CustomerReview,Remark)");
            strSql.Append(" values (");
            strSql.Append("@ReturnOrderCode,@OrderId,@OrderCode,@ReturnUserId,@ReturnUserName,@CreateUserId,@CreatedDate,@UpdatedUserId,@UpdatedDate,@SupplierId,@SupplierName,@CouponCode,@CouponName,@CouponAmount,@CouponValue,@CouponValueType,@ReturnGoodsType,@ReturnCoupon,@ActualSalesTotal,@Amount,@AmountAdjusted,@AmountActual,@ServiceType,@Credential,@Description,@ImageUrl,@ReturnType,@PickRegionId,@PickRegion,@PickAddress,@PickZipCode,@PickName,@PickTelPhone,@PickCellPhone,@PickEmail,@ShippingModeId,@ShippingModeName,@ShipOrderNumber,@ExpressCompanyName,@ExpressCompanyAbb,@ReturnTrueName,@ReturnBankName,@ReturnCard,@ReturnCardType,@ContactName,@ContactPhone,@Status,@RefundStatus,@LogisticStatus,@RefuseReason,@CustomerReview,@Remark)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@ReturnUserId", SqlDbType.Int,4),
					new SqlParameter("@ReturnUserName", SqlDbType.NVarChar,100),
					new SqlParameter("@CreateUserId", SqlDbType.Int,4),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@UpdatedUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdatedDate", SqlDbType.DateTime),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
					new SqlParameter("@CouponCode", SqlDbType.NVarChar,50),
					new SqlParameter("@CouponName", SqlDbType.NVarChar,100),
					new SqlParameter("@CouponAmount", SqlDbType.Money,8),
					new SqlParameter("@CouponValue", SqlDbType.Money,8),
					new SqlParameter("@CouponValueType", SqlDbType.SmallInt,2),
					new SqlParameter("@ReturnGoodsType", SqlDbType.SmallInt,2),
					new SqlParameter("@ReturnCoupon", SqlDbType.SmallInt,2),
					new SqlParameter("@ActualSalesTotal", SqlDbType.Money,8),
					new SqlParameter("@Amount", SqlDbType.Money,8),
					new SqlParameter("@AmountAdjusted", SqlDbType.Money,8),
					new SqlParameter("@AmountActual", SqlDbType.Money,8),
					new SqlParameter("@ServiceType", SqlDbType.SmallInt,2),
					new SqlParameter("@Credential", SqlDbType.NVarChar,100),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@ImageUrl", SqlDbType.Text),
					new SqlParameter("@ReturnType", SqlDbType.SmallInt,2),
					new SqlParameter("@PickRegionId", SqlDbType.Int,4),
					new SqlParameter("@PickRegion", SqlDbType.NVarChar,300),
					new SqlParameter("@PickAddress", SqlDbType.NVarChar,300),
					new SqlParameter("@PickZipCode", SqlDbType.NVarChar,20),
					new SqlParameter("@PickName", SqlDbType.NVarChar,50),
					new SqlParameter("@PickTelPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@PickCellPhone", SqlDbType.NVarChar,50),
					new SqlParameter("@PickEmail", SqlDbType.NVarChar,100),
					new SqlParameter("@ShippingModeId", SqlDbType.Int,4),
					new SqlParameter("@ShippingModeName", SqlDbType.NVarChar,100),
					new SqlParameter("@ShipOrderNumber", SqlDbType.NVarChar,50),
					new SqlParameter("@ExpressCompanyName", SqlDbType.NVarChar,500),
					new SqlParameter("@ExpressCompanyAbb", SqlDbType.NVarChar,500),
					new SqlParameter("@ReturnTrueName", SqlDbType.NVarChar,50),
					new SqlParameter("@ReturnBankName", SqlDbType.NVarChar,200),
					new SqlParameter("@ReturnCard", SqlDbType.NVarChar,50),
					new SqlParameter("@ReturnCardType", SqlDbType.Int,4),
					new SqlParameter("@ContactName", SqlDbType.NVarChar,100),
					new SqlParameter("@ContactPhone", SqlDbType.NVarChar,100),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@RefundStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@LogisticStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@RefuseReason", SqlDbType.NVarChar,500),
					new SqlParameter("@CustomerReview", SqlDbType.SmallInt,2),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000)};
            parameters[0].Value = model.ReturnOrderCode;
            parameters[1].Value = model.OrderId;
            parameters[2].Value = model.OrderCode;
            parameters[3].Value = model.ReturnUserId;
            parameters[4].Value = model.ReturnUserName;
            parameters[5].Value = model.CreateUserId;
            parameters[6].Value = model.CreatedDate;
            parameters[7].Value = model.UpdatedUserId;
            parameters[8].Value = model.UpdatedDate;
            parameters[9].Value = model.SupplierId;
            parameters[10].Value = model.SupplierName;
            parameters[11].Value = model.CouponCode;
            parameters[12].Value = model.CouponName;
            parameters[13].Value = model.CouponAmount;
            parameters[14].Value = model.CouponValue;
            parameters[15].Value = model.CouponValueType;
            parameters[16].Value = model.ReturnGoodsType;
            parameters[17].Value = model.ReturnCoupon;
            parameters[18].Value = model.ActualSalesTotal;
            parameters[19].Value = model.Amount;
            parameters[20].Value = model.AmountAdjusted;
            parameters[21].Value = model.AmountActual;
            parameters[22].Value = model.ServiceType;
            parameters[23].Value = model.Credential;
            parameters[24].Value = model.Description;
            parameters[25].Value = model.ImageUrl;
            parameters[26].Value = model.ReturnType;
            parameters[27].Value = model.PickRegionId;
            parameters[28].Value = model.PickRegion;
            parameters[29].Value = model.PickAddress;
            parameters[30].Value = model.PickZipCode;
            parameters[31].Value = model.PickName;
            parameters[32].Value = model.PickTelPhone;
            parameters[33].Value = model.PickCellPhone;
            parameters[34].Value = model.PickEmail;
            parameters[35].Value = model.ShippingModeId;
            parameters[36].Value = model.ShippingModeName;
            parameters[37].Value = model.ShipOrderNumber;
            parameters[38].Value = model.ExpressCompanyName;
            parameters[39].Value = model.ExpressCompanyAbb;
            parameters[40].Value = model.ReturnTrueName;
            parameters[41].Value = model.ReturnBankName;
            parameters[42].Value = model.ReturnCard;
            parameters[43].Value = model.ReturnCardType;
            parameters[44].Value = model.ContactName;
            parameters[45].Value = model.ContactPhone;
            parameters[46].Value = model.Status;
            parameters[47].Value = model.RefundStatus;
            parameters[48].Value = model.LogisticStatus;
            parameters[49].Value = model.RefuseReason;
            parameters[50].Value = model.CustomerReview;
            parameters[51].Value = model.Remark;
            return new CommandInfo(strSql.ToString(), parameters);
        }

        #endregion

        #region GenerateReturnOrderItems

        private List<CommandInfo> GenerateReturnOrderItems(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders returnOrders)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            foreach (Model.Shop.ReturnOrder.ReturnOrderItems item in returnOrders.Items)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Shop_ReturnOrderItems(");
                strSql.Append("OrderItemId,ReturnOrderId,ReturnOrderCode,OrderId,OrderCode,ProductId,ProductCode,SKU,Name,ThumbnailsUrl,Description,Quantity,ShipmentQuantity,CostPrice,SellPrice,ReturnPrice,Attribute,Remark,Weight,Deduct,Points,ProductLineId,SupplierId,SupplierName,BrandId,BrandName,ProductType)");
                strSql.Append(" values (");
                strSql.Append("@OrderItemId,@ReturnOrderId,@ReturnOrderCode,@OrderId,@OrderCode,@ProductId,@ProductCode,@SKU,@Name,@ThumbnailsUrl,@Description,@Quantity,@ShipmentQuantity,@CostPrice,@SellPrice,@ReturnPrice,@Attribute,@Remark,@Weight,@Deduct,@Points,@ProductLineId,@SupplierId,@SupplierName,@BrandId,@BrandName,@ProductType)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@OrderItemId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductCode", SqlDbType.NVarChar,50),
					new SqlParameter("@SKU", SqlDbType.NVarChar,200),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@ThumbnailsUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Quantity", SqlDbType.Int,4),
					new SqlParameter("@ShipmentQuantity", SqlDbType.Decimal,9),
					new SqlParameter("@CostPrice", SqlDbType.Money,8),
					new SqlParameter("@SellPrice", SqlDbType.Money,8),
					new SqlParameter("@ReturnPrice", SqlDbType.Money,8),
					new SqlParameter("@Attribute", SqlDbType.Text),
					new SqlParameter("@Remark", SqlDbType.Text),
					new SqlParameter("@Weight", SqlDbType.Int,4),
					new SqlParameter("@Deduct", SqlDbType.Money,8),
					new SqlParameter("@Points", SqlDbType.Int,4),
					new SqlParameter("@ProductLineId", SqlDbType.Int,4),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
					new SqlParameter("@BrandId", SqlDbType.Int,4),
					new SqlParameter("@BrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@ProductType", SqlDbType.SmallInt,2)};
                parameters[0].Value = item.OrderItemId;
                parameters[1].Value = returnOrders.ReturnOrderId;
                parameters[2].Value = item.ReturnOrderCode;
                parameters[3].Value = item.OrderId;
                parameters[4].Value = item.OrderCode;
                parameters[5].Value = item.ProductId;
                parameters[6].Value = item.ProductCode;
                parameters[7].Value = item.SKU;
                parameters[8].Value = item.Name;
                parameters[9].Value = item.ThumbnailsUrl;
                parameters[10].Value = item.Description;
                parameters[11].Value = item.Quantity;
                parameters[12].Value = item.ShipmentQuantity;
                parameters[13].Value = item.CostPrice;
                parameters[14].Value = item.SellPrice;
                parameters[15].Value = item.ReturnPrice;
                parameters[16].Value = item.Attribute;
                parameters[17].Value = item.Remark;
                parameters[18].Value = item.Weight;
                parameters[19].Value = item.Deduct;
                parameters[20].Value = item.Points;
                parameters[21].Value = item.ProductLineId;
                parameters[22].Value = item.SupplierId;
                parameters[23].Value = item.SupplierName;
                parameters[24].Value = item.BrandId;
                parameters[25].Value = item.BrandName;
                parameters[26].Value = item.ProductType;
                list.Add(new CommandInfo(strSql.ToString(),parameters, EffentNextType.ExcuteEffectRows));
            }
            return list;
        }

        #endregion

        #region GenerateReturnOrderAction

        private List<CommandInfo> GenerateReturnOrderAction(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders returnOrders, Accounts.Bus.User currentUser)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_ReturnOrderAction(");
            strSql.Append("ReturnOrderId,ReturnOrderCode,UserId,UserName,ActionCode,ActionDate,Remark)");
            strSql.Append(" values (");
            strSql.Append("@ReturnOrderId,@ReturnOrderCode,@UserId,@UserName,@ActionCode,@ActionDate,@Remark)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@ActionCode", SqlDbType.NVarChar,100),
					new SqlParameter("@ActionDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000)};
            parameters[0].Value = returnOrders.ReturnOrderId;
            parameters[1].Value = returnOrders.ReturnOrderCode;
            parameters[2].Value = returnOrders.ReturnUserId;
            parameters[3].Value = "客户";
            parameters[4].Value = ((int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.CustomersCreate);
            switch (currentUser.UserType)
            {
                case "UU":
                    parameters[3].Value = "客户";
                    parameters[4].Value = ((int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.CustomersCreate);
                    break;
                case "AA":
                    parameters[3].Value = currentUser.NickName;
                    parameters[4].Value = ((int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemCreate);
                    break;     
            }
            parameters[5].Value = DateTime.Now;
            parameters[6].Value = "创建申请";
            return new List<CommandInfo>
                {
                    new CommandInfo(strSql.ToString(), parameters,
                                    EffentNextType.ExcuteEffectRows)
                };
        }

        #endregion
        #endregion

        #region 审核通过
        /// <summary>
        /// 审核通过 修改数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="IsReturnGoods">是否需要退货</param>
        /// <param name="orderInfo">原订单</param>
        /// <returns></returns>
        public bool AuditPass(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model, bool IsReturnGoods, Model.Shop.Order.OrderInfo orderInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ReturnOrders set ");
            strSql.Append("UpdatedUserId=@UpdatedUserId,");
            strSql.Append("UpdatedDate=@UpdatedDate,"); 
            strSql.Append("Status=@Status,");
            strSql.Append("RefundStatus=@RefundStatus,");
            strSql.Append("LogisticStatus=@LogisticStatus,");
            strSql.Append("Remark=@Remark");               
            strSql.Append(" where ReturnOrderId=@ReturnOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@UpdatedUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdatedDate", SqlDbType.DateTime),					
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@RefundStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@LogisticStatus", SqlDbType.SmallInt,2),
                   new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.UpdatedUserId;
            parameters[1].Value = model.UpdatedDate;
            parameters[2].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.Status.Handling;
            parameters[3].Value = model.RefundStatus;
            parameters[4].Value = model.LogisticStatus;
            parameters[5].Value = model.Remark;
            parameters[6].Value = model.ReturnOrderId;
            list.Add(new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows));


            if (!IsReturnGoods) {
                StringBuilder strSql2 = new StringBuilder();
                strSql2.Append("update OMS_Orders set ");
                strSql2.Append("RefundStatus=@RefundStatus ");
                strSql2.Append(" where OrderId=@OrderId");
                SqlParameter[] parameters2 = {
					new SqlParameter("@RefundStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
                parameters2[0].Value = model.RefundStatus;
                parameters2[1].Value = model.OrderId;
                list.Add(new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows));
            }


            #region 删除赠送的未使用的优惠劵
            if (orderInfo != null)
            {
                long mainOrderId = orderInfo.OrderId;
                if (orderInfo.OrderType == 2)//是子单  
                {
                    mainOrderId = orderInfo.ParentOrderId;
                }
                if (mainOrderId > 0)
                {
                    StringBuilder strSql4 = new StringBuilder();
                    strSql4.Append("   delete from Shop_CouponInfo   where UserId=@UserId and  Status<2  ");
                    strSql4.AppendFormat(" and  OrderId={0} ", mainOrderId);
                    SqlParameter[] parameters4 = {
                    new SqlParameter("@UserId", SqlDbType.Int, 4)
                };
                    parameters4[0].Value = orderInfo.BuyerID;
                    list.Add(new CommandInfo(strSql4.ToString(), parameters4));
                }
            }
            #endregion

            return DBHelper.DefaultDBHelper.ExecuteSqlTran(list)>0;
        }
        #endregion

        #region 审核未通过
        /// <summary>
        /// 审核未通过
        /// </summary>
        /// <param name="ReturnOrderId"></param>
        /// <param name="refuseReason">原因</param>
        /// <param name="UpdateUserId"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public bool AuditFail(long ReturnOrderId, string refuseReason, int UpdateUserId, string Remark)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ReturnOrders set ");
            strSql.Append("UpdatedUserId=@UpdatedUserId,");
            strSql.Append("UpdatedDate=@UpdatedDate,");
            strSql.Append("Status=@Status,");
            strSql.Append("RefuseReason=@RefuseReason,");
            strSql.Append("Remark=@Remark");               
            strSql.Append(" where ReturnOrderId=@ReturnOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@UpdatedUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdatedDate", SqlDbType.DateTime),	
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
                     new SqlParameter("@RefuseReason", SqlDbType.NVarChar,500),
                     new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = UpdateUserId;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.Status.Refuse;
            parameters[3].Value = refuseReason;
            parameters[4].Value = Remark;
            parameters[5].Value = ReturnOrderId;
            return DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters)>0;
        }
        #endregion

        #region  取消申请
        /// <summary>
        /// 取消退单申请
       /// </summary>
       /// <param name="ReturnOrderId"></param>
       /// <param name="ReturnOrderCode"></param>
       /// <param name="currentUser"></param>
       /// <returns></returns>
        public bool CancelReturnOrder(long ReturnOrderId, string ReturnOrderCode, int userId)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ReturnOrders set ");
            strSql.Append("UpdatedUserId=@UpdatedUserId,");
            strSql.Append("UpdatedDate=@UpdatedDate,");
            strSql.Append("Status=@Status ");
            strSql.Append(" where ReturnOrderId=@ReturnOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@UpdatedUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdatedDate", SqlDbType.DateTime),	
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = userId;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = (int)Model.Shop.ReturnOrder.EnumHelper.Status.Cancel;
            parameters[3].Value = ReturnOrderId;
            list.Add(new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows));
            
            //添加操作记录
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("insert into Shop_ReturnOrderAction(");
            strSql2.Append("ReturnOrderId,ReturnOrderCode,UserId,UserName,ActionCode,ActionDate,Remark)");
            strSql2.Append(" values (");
            strSql2.Append("@ReturnOrderId,@ReturnOrderCode,@UserId,@UserName,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] parameters2 = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@ActionCode", SqlDbType.NVarChar,100),
					new SqlParameter("@ActionDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000)};
            parameters2[0].Value = ReturnOrderId;
            parameters2[1].Value = ReturnOrderCode;
            parameters2[2].Value = userId;
            parameters2[3].Value = "客户";
            parameters2[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.CustomersCancel; 
            parameters2[5].Value = DateTime.Now;
            parameters2[6].Value = "取消申请"; 
            list.Add(new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows));
            return DBHelper.DefaultDBHelper.ExecuteSqlTran(list)>0;
        }
        #endregion

        #region 确认已取货
        public bool PackedOrder(Model.Shop.ReturnOrder.ReturnOrders info, Accounts.Bus.User currentUser)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            //更新状态
            //DONE: 更新状态为 已取货
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("UPDATE  Shop_ReturnOrders SET LogisticStatus=4, Status=1, RefundStatus=1,UpdatedDate=@UpdatedDate");
            strSql2.Append(" where ReturnOrderId=@ReturnOrderId  ");
            SqlParameter[] parameters2 =
                {
                    new SqlParameter("@ReturnOrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime)
                };
            parameters2[0].Value = info.ReturnOrderId;
            parameters2[1].Value = DateTime.Now;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            //更新订单 状态
            StringBuilder strSql4 = new StringBuilder();
            strSql4.Append("UPDATE  OMS_Orders SET RefundStatus=1,UpdatedDate=@UpdatedDate");
            strSql4.Append(" where OrderId=@OrderId  ");
            SqlParameter[] parameters4 =
                {
                    new SqlParameter("@OrderId", SqlDbType.BigInt, 8),
                    new SqlParameter("@UpdatedDate", SqlDbType.DateTime)
                };
            parameters4[0].Value = info.OrderId;
            parameters4[1].Value = DateTime.Now;
            cmd = new CommandInfo(strSql4.ToString(), parameters4, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            //添加操作记录
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("insert into Shop_ReturnOrderAction(");
            strSql3.Append("ReturnOrderId,ReturnOrderCode,UserId,UserName,ActionCode,ActionDate,Remark)");
            strSql3.Append(" values (");
            strSql3.Append("@ReturnOrderId,@ReturnOrderCode,@UserId,@UserName,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] parameters3 = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@ActionCode", SqlDbType.NVarChar,100),
					new SqlParameter("@ActionDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000)};

            parameters3[0].Value = info.ReturnOrderId;
            parameters3[1].Value = info.ReturnOrderCode;
            parameters3[2].Value = currentUser != null ? currentUser.UserID : 0;
            parameters3[3].Value = currentUser != null ? currentUser.NickName : "";
            parameters3[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemPicked;
            if (currentUser != null)  
            {
                switch (currentUser.UserType)
                {
                    case "SP":
                        parameters3[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SellerPicked;
                        break;
                    case "AA":
                        parameters3[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemPicked;
                        break;
                }
            }
            parameters3[5].Value = DateTime.Now;
            parameters3[6].Value = "确认取货";
            cmd = new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            //#region 更新库存  有可能是货物有破损不能再继续销售的

            //foreach (var item in orderInfo.Items)
            //{
            //    StringBuilder strSql = new StringBuilder();
            //    strSql.Append("update PMS_SKUs  set Stock=Stock+@Stock");
            //    strSql.Append(" where SKU=@SKU");
            //    SqlParameter[] parameters =
            //            {
            //                new SqlParameter("@SKU", SqlDbType.NVarChar, 50),
            //                new SqlParameter("@Stock", SqlDbType.Int, 4)
            //            };

            //    parameters[0].Value = item.SKU;
            //    parameters[1].Value = item.Quantity;
            //    sqllist.Add(new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows));
            //}
            //#endregion

            #region  积分出现负数的情况需要考虑

            #endregion

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            return rowsAffected > 0;
        }
        #endregion

        #region 完成退款
        /// <summary>
        /// 完成退款
       /// </summary>
        /// <param name="model"></param>
       /// <param name="currentUser">当前用户</param>
       /// <param name="IsReturnCoupon">是否退优惠劵</param>
        /// <param name="suppUserId">商家的用户Id</param>
        /// <param name="deductionSuppAmount">扣除商家的金额</param>
       /// <returns></returns>
        public bool Refunds(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model, Accounts.Bus.User currentUser, bool IsReturnCoupon, int suppUserId, decimal deductionSuppAmount)
        {
            List<CommandInfo> list = new List<CommandInfo>();

            #region 更新退货单
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ReturnOrders set ");
            strSql.Append("UpdatedUserId=@UpdatedUserId,");
            strSql.Append("UpdatedDate=@UpdatedDate,");
            strSql.Append("Status=@Status, ");
            strSql.Append("LogisticStatus=@LogisticStatus,");
            strSql.Append("RefundStatus=@RefundStatus,");
            strSql.Append("AmountActual=@AmountActual");
            strSql.Append(" where ReturnOrderId=@ReturnOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@UpdatedUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdatedDate", SqlDbType.DateTime),	
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
                    new SqlParameter("@LogisticStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@RefundStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@AmountActual", SqlDbType.Money,8),
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = currentUser.UserID;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = (int)Model.Shop.ReturnOrder.EnumHelper.Status.Complete;
            parameters[3].Value = model.LogisticStatus;
            parameters[4].Value = (int)Model.Shop.ReturnOrder.EnumHelper.RefundStatus.Refunds;
            parameters[5].Value = model.AmountActual;
            parameters[6].Value = model.ReturnOrderId;
            list.Add(new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows));
            #endregion

            #region 更新订单表中的退款状态
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("update OMS_Orders set ");
            strSql1.Append("UpdatedDate=@UpdatedDate,");
            strSql1.Append("RefundStatus=@RefundStatus ");
            strSql1.Append(" where OrderId=@OrderId");
            SqlParameter[] parameters1 = {
					new SqlParameter("@UpdatedDate", SqlDbType.DateTime),	
					new SqlParameter("@RefundStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8)};
            parameters1[0].Value = DateTime.Now;
            parameters1[1].Value = (int)Model.Shop.Order.EnumHelper.RefundStatus.Refunds;
            parameters1[2].Value = model.OrderId;
            list.Add(new CommandInfo(strSql1.ToString(), parameters1, EffentNextType.ExcuteEffectRows));
            #endregion

            #region 客户账户余额
            #region 更新用户表中的余额
            //更新用户表中的余额
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append(" update Accounts_UsersExp set ");
            strSql2.Append(" Balance=Balance+ @Balance where UserID=@UserId");
            SqlParameter[] parameters2 = {
                    new SqlParameter("@Balance", SqlDbType.Money,8),
                   new SqlParameter("@UserId", SqlDbType.Int,4)
                                        };
            parameters2[0].Value = model.AmountActual;//退回金额
            parameters2[1].Value = model.ReturnUserId;
            list.Add(new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows));
            #endregion

            #region 添加余额明细
            Members.UsersExp userBll = new Members.UsersExp();
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("insert into Pay_BalanceDetails(");
            strSql3.Append("UserId,TradeDate,TradeType,Income,Balance,Remark)");
            strSql3.Append(" values (");
            strSql3.Append("@UserId,@TradeDate,@TradeType,@Income,@Balance,@Remark)");
            strSql3.Append(";select @@IDENTITY");
            SqlParameter[] parameters3 = {
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@TradeDate", SqlDbType.DateTime),
                    new SqlParameter("@TradeType", SqlDbType.Int,4),
                    new SqlParameter("@Income", SqlDbType.Money,8),
                    new SqlParameter("@Balance", SqlDbType.Money,8),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,2000)
                 };  
            parameters3[0].Value = model.ReturnUserId;
            parameters3[1].Value = DateTime.Now;
            parameters3[2].Value = 1;
            parameters3[3].Value = model.AmountActual;//收入
            parameters3[4].Value = userBll.GetUserBalance(model.ReturnUserId) + model.AmountActual;//余额
            parameters3[5].Value = "退款["+model.ReturnOrderCode+"]"; //备注
            list.Add(new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows));
            #endregion
            #endregion

            #region 退优惠劵
            if (IsReturnCoupon)
            {
                StringBuilder strSql4 = new StringBuilder();
                strSql4.Append("update Shop_CouponInfo set ");
                strSql4.Append("Status=@Status,");
                strSql4.Append("UsedDate=@UsedDate");
                strSql4.Append(" where CouponCode=@CouponCode ");
                SqlParameter[] parameters4 = {
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@UsedDate", SqlDbType.DateTime),
					new SqlParameter("@CouponCode", SqlDbType.NVarChar,200)};
                parameters4[0].Value = 1;
                parameters4[1].Value = null;
                parameters4[2].Value = model.CouponCode;
                list.Add(new CommandInfo(strSql4.ToString(), parameters4, EffentNextType.ExcuteEffectRows));

                StringBuilder strSql5 = new StringBuilder();
                strSql5.Append("insert into Shop_ReturnOrderAction(");
                strSql5.Append("ReturnOrderId,ReturnOrderCode,UserId,UserName,ActionCode,ActionDate,Remark)");
                strSql5.Append(" values (");
                strSql5.Append("@ReturnOrderId,@ReturnOrderCode,@UserId,@UserName,@ActionCode,@ActionDate,@Remark)");
                SqlParameter[] parameters5 = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@ActionCode", SqlDbType.NVarChar,100),
					new SqlParameter("@ActionDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000)};
                parameters5[0].Value = model.ReturnOrderId;
                parameters5[1].Value = model.ReturnOrderCode;
                parameters5[2].Value = currentUser.UserID;
                parameters5[3].Value = currentUser.NickName;
                parameters5[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemReturnCoupon;
                if (currentUser != null)
                {
                    switch (currentUser.UserType)
                    {
                        case "SP":
                            parameters5[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SellerReturnCoupon;
                            break;
                        case "AA":
                            parameters5[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemReturnCoupon;
                            break;
                    }
                }
                parameters5[5].Value = DateTime.Now;
                parameters5[6].Value = "返还优惠劵 ["+model.CouponCode+"]";
                list.Add(new CommandInfo(strSql5.ToString(), parameters5, EffentNextType.ExcuteEffectRows));
            }
            #endregion

            #region 添加操作记录
            //添加操作记录
            StringBuilder strSql6 = new StringBuilder();
            strSql6.Append("insert into Shop_ReturnOrderAction(");
            strSql6.Append("ReturnOrderId,ReturnOrderCode,UserId,UserName,ActionCode,ActionDate,Remark)");
            strSql6.Append(" values (");
            strSql6.Append("@ReturnOrderId,@ReturnOrderCode,@UserId,@UserName,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] parameters6 = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@ActionCode", SqlDbType.NVarChar,100),
					new SqlParameter("@ActionDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000)};
            parameters6[0].Value = model.ReturnOrderId;
            parameters6[1].Value = model.ReturnOrderCode;
            parameters6[2].Value = currentUser.UserID;
            parameters6[3].Value = currentUser.NickName;
            parameters6[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemComplete;
            if (currentUser!=null)
            {
                switch (currentUser.UserType) { 
                    case "SP":
                          parameters6[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SellerComplete;
                          break;
                    case "AA":
                          parameters6[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemComplete;
                        break;
                }
            }
            parameters6[5].Value = DateTime.Now;
            parameters6[6].Value = "完成退款(退款金额￥" + model.AmountActual + ")";
            list.Add(new CommandInfo(strSql6.ToString(), parameters6, EffentNextType.ExcuteEffectRows));
            #endregion



            if (model.SupplierId > 0 && deductionSuppAmount>0)
            {
                #region 扣除商家账户余额

                #region 更新商家账户的余额
                //更新用户表中的余额
                StringBuilder strSql7 = new StringBuilder();
                strSql7.Append(" update Accounts_UsersExp set ");
                strSql7.Append(" Balance=Balance-@Balance where UserID=@UserId  and  Balance>=@Balance ");//确保账户余额充足
                SqlParameter[] parameters7 = {
                    new SqlParameter("@Balance", SqlDbType.Money,8),
                   new SqlParameter("@UserId", SqlDbType.Int,4)
                                        };
                parameters7[0].Value = deductionSuppAmount;//扣除商家的余额
                parameters7[1].Value = suppUserId;
                list.Add(new CommandInfo(strSql7.ToString(), parameters7, EffentNextType.ExcuteEffectRows));
                #endregion

                #region 添加余额明细
                StringBuilder strSql8 = new StringBuilder();
                strSql8.Append("insert into Pay_BalanceDetails(");
                strSql8.Append("UserId,TradeDate,TradeType,Expenses,Balance,Remark)");
                strSql8.Append(" values (");
                strSql8.Append("@UserId,@TradeDate,@TradeType,@Expenses,@Balance,@Remark)");
                strSql8.Append(";select @@IDENTITY");
                SqlParameter[] parameters8 = {
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@TradeDate", SqlDbType.DateTime),
                    new SqlParameter("@TradeType", SqlDbType.Int,4),
                    new SqlParameter("@Expenses", SqlDbType.Money,8),
                    new SqlParameter("@Balance", SqlDbType.Money,8),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,2000)
                 };
                parameters8[0].Value = suppUserId;
                parameters8[1].Value = DateTime.Now;
                parameters8[2].Value = 2;//支出
                parameters8[3].Value = deductionSuppAmount;//支出金额
                parameters8[4].Value = userBll.GetUserBalance(suppUserId) - deductionSuppAmount;//账户余额
                parameters8[5].Value = "交易支出 退单号[" + model.ReturnOrderCode + "]"; //备注
                list.Add(new CommandInfo(strSql8.ToString(), parameters8, EffentNextType.ExcuteEffectRows));
                #endregion

                #endregion
            }
         


            return DBHelper.DefaultDBHelper.ExecuteSqlTran(list)>0;
        }
        #endregion

        #region 修改应退金额
        /// <summary>
        ///  修改应退金额
     /// </summary>
     /// <param name="ReturnOrderId"></param>
     /// <param name="ReturnOrderCode"></param>
     /// <param name="oldAmountAdjusted"></param>
     /// <param name="newAmountAdjusted"></param>
     /// <param name="currentUser"></param>
     /// <returns></returns>
        public bool UpdateAmountAdjusted(long ReturnOrderId, string ReturnOrderCode, decimal oldAmountAdjusted, decimal newAmountAdjusted, Accounts.Bus.User currentUser)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ReturnOrders set ");
            strSql.Append("UpdatedUserId=@UpdatedUserId,");
            strSql.Append("UpdatedDate=@UpdatedDate,");
            strSql.Append("AmountAdjusted=@AmountAdjusted ");
            strSql.Append(" where ReturnOrderId=@ReturnOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@UpdatedUserId", SqlDbType.Int,4),
					new SqlParameter("@UpdatedDate", SqlDbType.DateTime),	
					new SqlParameter("@AmountAdjusted", SqlDbType.Money,8),	
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = currentUser.UserID;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = newAmountAdjusted;
            parameters[3].Value = ReturnOrderId;
            list.Add(new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows));

             #region 添加操作记录
            //添加操作记录
            StringBuilder strSql4 = new StringBuilder();
            strSql4.Append("insert into Shop_ReturnOrderAction(");
            strSql4.Append("ReturnOrderId,ReturnOrderCode,UserId,UserName,ActionCode,ActionDate,Remark)");
            strSql4.Append(" values (");
            strSql4.Append("@ReturnOrderId,@ReturnOrderCode,@UserId,@UserName,@ActionCode,@ActionDate,@Remark)");
            SqlParameter[] parameters4 = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@ActionCode", SqlDbType.NVarChar,100),
					new SqlParameter("@ActionDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000)};
            parameters4[0].Value =ReturnOrderId;
            parameters4[1].Value =ReturnOrderCode;
            parameters4[2].Value = currentUser.UserID;
            parameters4[3].Value = currentUser.NickName;
            switch (currentUser.UserType)
            {
                case "SP":
                    parameters4[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SellerUpdateAmountAdjusted;
                    break;
                default:
                     parameters4[4].Value = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemUpdateAmountAdjusted;
                    break;
            }
            parameters4[5].Value = DateTime.Now;
            parameters4[6].Value = "应退金额由 " + oldAmountAdjusted.ToString("F") + " 修改为 " + newAmountAdjusted.ToString("F");
            list.Add(new CommandInfo(strSql4.ToString(), parameters4, EffentNextType.ExcuteEffectRows));
            #endregion

            return DBHelper.DefaultDBHelper.ExecuteSqlTran(list) > 0;
        }
        #endregion

        /// <summary>
        /// 更新备注
        /// </summary>
        /// <param name="ReturnOrderId"></param>
        /// <param name="Remark"></param>
        /// <param name="SupplierId"></param>
        /// <returns></returns>
        public bool UpdateRemark(long ReturnOrderId, string Remark, int SupplierId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ReturnOrders set ");
            strSql.Append("Remark=@Remark ");
            strSql.Append(" where ReturnOrderId=@ReturnOrderId ");
            strSql.Append(" and SupplierId=@SupplierId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000),
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
                                        new SqlParameter("@SupplierId", SqlDbType.Int,4)};
            parameters[0].Value = Remark;
            parameters[1].Value = ReturnOrderId;
            parameters[2].Value = SupplierId;
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
 

        #endregion  ExtensionMethod
    }
}

