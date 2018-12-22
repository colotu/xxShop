/**  版本信息模板在安装目录下，可自行修改。
* PreOrder.cs
*
* 功 能： N/A
* 类 名： PreOrder
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/8/24 16:08:39   N/A    初版
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
using YSWL.MALL.IDAL.Shop.PrePro;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.PrePro
{
	/// <summary>
	/// 数据访问类:PreOrder
	/// </summary>
	public partial class PreOrder:IPreOrder
	{
		public PreOrder()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long PreOrderId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_PreOrder");
            strSql.Append(" where PreOrderId=@PreOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@PreOrderId", SqlDbType.BigInt)
			};
            parameters[0].Value = PreOrderId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.PrePro.PreOrder model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_PreOrder(");
            strSql.Append("PreCode,PreProId,ProductId,ProductName,Count,SKU,PaymentTypeId,PaymentTypeName,PaymentGateway,PaymentStatus,Amount,Phone,UserId,UserName,CreatedDate,HandleUserId,HandleDate,DeliveryTip,Status,Remark)");
            strSql.Append(" values (");
            strSql.Append("@PreCode,@PreProId,@ProductId,@ProductName,@Count,@SKU,@PaymentTypeId,@PaymentTypeName,@PaymentGateway,@PaymentStatus,@Amount,@Phone,@UserId,@UserName,@CreatedDate,@HandleUserId,@HandleDate,@DeliveryTip,@Status,@Remark)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@PreCode", SqlDbType.NVarChar,50),
					new SqlParameter("@PreProId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@Count", SqlDbType.Int,4),
					new SqlParameter("@SKU", SqlDbType.NVarChar,50),
					new SqlParameter("@PaymentTypeId", SqlDbType.Int,4),
					new SqlParameter("@PaymentTypeName", SqlDbType.NVarChar,100),
					new SqlParameter("@PaymentGateway", SqlDbType.NVarChar,50),
					new SqlParameter("@PaymentStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@Amount", SqlDbType.Money,8),
					new SqlParameter("@Phone", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,100),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@HandleUserId", SqlDbType.Int,4),
					new SqlParameter("@HandleDate", SqlDbType.DateTime),
					new SqlParameter("@DeliveryTip", SqlDbType.NVarChar,100),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@Remark", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.PreCode;
            parameters[1].Value = model.PreProId;
            parameters[2].Value = model.ProductId;
            parameters[3].Value = model.ProductName;
            parameters[4].Value = model.Count;
            parameters[5].Value = model.SKU;
            parameters[6].Value = model.PaymentTypeId;
            parameters[7].Value = model.PaymentTypeName;
            parameters[8].Value = model.PaymentGateway;
            parameters[9].Value = model.PaymentStatus;
            parameters[10].Value = model.Amount;
            parameters[11].Value = model.Phone;
            parameters[12].Value = model.UserId;
            parameters[13].Value = model.UserName;
            parameters[14].Value = model.CreatedDate;
            parameters[15].Value = model.HandleUserId;
            parameters[16].Value = model.HandleDate;
            parameters[17].Value = model.DeliveryTip;
            parameters[18].Value = model.Status;
            parameters[19].Value = model.Remark;

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
        public bool Update(YSWL.MALL.Model.Shop.PrePro.PreOrder model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_PreOrder set ");
            strSql.Append("PreCode=@PreCode,");
            strSql.Append("PreProId=@PreProId,");
            strSql.Append("ProductId=@ProductId,");
            strSql.Append("ProductName=@ProductName,");
            strSql.Append("Count=@Count,");
            strSql.Append("SKU=@SKU,");
            strSql.Append("PaymentTypeId=@PaymentTypeId,");
            strSql.Append("PaymentTypeName=@PaymentTypeName,");
            strSql.Append("PaymentGateway=@PaymentGateway,");
            strSql.Append("PaymentStatus=@PaymentStatus,");
            strSql.Append("Amount=@Amount,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("CreatedDate=@CreatedDate,");
            strSql.Append("HandleUserId=@HandleUserId,");
            strSql.Append("HandleDate=@HandleDate,");
            strSql.Append("DeliveryTip=@DeliveryTip,");
            strSql.Append("Status=@Status,");
            strSql.Append("Remark=@Remark");
            strSql.Append(" where PreOrderId=@PreOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@PreCode", SqlDbType.NVarChar,50),
					new SqlParameter("@PreProId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@Count", SqlDbType.Int,4),
					new SqlParameter("@SKU", SqlDbType.NVarChar,50),
					new SqlParameter("@PaymentTypeId", SqlDbType.Int,4),
					new SqlParameter("@PaymentTypeName", SqlDbType.NVarChar,100),
					new SqlParameter("@PaymentGateway", SqlDbType.NVarChar,50),
					new SqlParameter("@PaymentStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@Amount", SqlDbType.Money,8),
					new SqlParameter("@Phone", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,100),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@HandleUserId", SqlDbType.Int,4),
					new SqlParameter("@HandleDate", SqlDbType.DateTime),
					new SqlParameter("@DeliveryTip", SqlDbType.NVarChar,100),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@Remark", SqlDbType.NVarChar,500),
					new SqlParameter("@PreOrderId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.PreCode;
            parameters[1].Value = model.PreProId;
            parameters[2].Value = model.ProductId;
            parameters[3].Value = model.ProductName;
            parameters[4].Value = model.Count;
            parameters[5].Value = model.SKU;
            parameters[6].Value = model.PaymentTypeId;
            parameters[7].Value = model.PaymentTypeName;
            parameters[8].Value = model.PaymentGateway;
            parameters[9].Value = model.PaymentStatus;
            parameters[10].Value = model.Amount;
            parameters[11].Value = model.Phone;
            parameters[12].Value = model.UserId;
            parameters[13].Value = model.UserName;
            parameters[14].Value = model.CreatedDate;
            parameters[15].Value = model.HandleUserId;
            parameters[16].Value = model.HandleDate;
            parameters[17].Value = model.DeliveryTip;
            parameters[18].Value = model.Status;
            parameters[19].Value = model.Remark;
            parameters[20].Value = model.PreOrderId;

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
        public bool Delete(long PreOrderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_PreOrder ");
            strSql.Append(" where PreOrderId=@PreOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@PreOrderId", SqlDbType.BigInt)
			};
            parameters[0].Value = PreOrderId;

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
        public bool DeleteList(string PreOrderIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_PreOrder ");
            strSql.Append(" where PreOrderId in (" + PreOrderIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.PrePro.PreOrder GetModel(long PreOrderId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 PreOrderId,PreCode,PreProId,ProductId,ProductName,Count,SKU,PaymentTypeId,PaymentTypeName,PaymentGateway,PaymentStatus,Amount,Phone,UserId,UserName,CreatedDate,HandleUserId,HandleDate,DeliveryTip,Status,Remark from Shop_PreOrder ");
            strSql.Append(" where PreOrderId=@PreOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@PreOrderId", SqlDbType.BigInt)
			};
            parameters[0].Value = PreOrderId;

            YSWL.MALL.Model.Shop.PrePro.PreOrder model = new YSWL.MALL.Model.Shop.PrePro.PreOrder();
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
        public YSWL.MALL.Model.Shop.PrePro.PreOrder DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.PrePro.PreOrder model = new YSWL.MALL.Model.Shop.PrePro.PreOrder();
            if (row != null)
            {
                if (row["PreOrderId"] != null && row["PreOrderId"].ToString() != "")
                {
                    model.PreOrderId = long.Parse(row["PreOrderId"].ToString());
                }
                if (row["PreCode"] != null)
                {
                    model.PreCode = row["PreCode"].ToString();
                }
                if (row["PreProId"] != null && row["PreProId"].ToString() != "")
                {
                    model.PreProId = int.Parse(row["PreProId"].ToString());
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["ProductName"] != null)
                {
                    model.ProductName = row["ProductName"].ToString();
                }
                if (row["Count"] != null && row["Count"].ToString() != "")
                {
                    model.Count = int.Parse(row["Count"].ToString());
                }
                if (row["SKU"] != null)
                {
                    model.SKU = row["SKU"].ToString();
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
                if (row["Amount"] != null && row["Amount"].ToString() != "")
                {
                    model.Amount = decimal.Parse(row["Amount"].ToString());
                }
                if (row["Phone"] != null)
                {
                    model.Phone = row["Phone"].ToString();
                }
                if (row["UserId"] != null && row["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                }
                if (row["HandleUserId"] != null && row["HandleUserId"].ToString() != "")
                {
                    model.HandleUserId = int.Parse(row["HandleUserId"].ToString());
                }
                if (row["HandleDate"] != null && row["HandleDate"].ToString() != "")
                {
                    model.HandleDate = DateTime.Parse(row["HandleDate"].ToString());
                }
                if (row["DeliveryTip"] != null)
                {
                    model.DeliveryTip = row["DeliveryTip"].ToString();
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
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
            strSql.Append("select PreOrderId,PreCode,PreProId,ProductId,ProductName,Count,SKU,PaymentTypeId,PaymentTypeName,PaymentGateway,PaymentStatus,Amount,Phone,UserId,UserName,CreatedDate,HandleUserId,HandleDate,DeliveryTip,Status,Remark ");
            strSql.Append(" FROM Shop_PreOrder ");
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
            strSql.Append(" PreOrderId,PreCode,PreProId,ProductId,ProductName,Count,SKU,PaymentTypeId,PaymentTypeName,PaymentGateway,PaymentStatus,Amount,Phone,UserId,UserName,CreatedDate,HandleUserId,HandleDate,DeliveryTip,Status,Remark ");
            strSql.Append(" FROM Shop_PreOrder ");
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
            strSql.Append("select count(1) FROM Shop_PreOrder ");
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
                strSql.Append("order by T.PreOrderId desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_PreOrder T ");
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
            parameters[0].Value = "Shop_PreOrder";
            parameters[1].Value = "PreOrderId";
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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int userId, string sku, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_PreOrder  ");
            strSql.Append(" where SKU=@SKU and UserId=@UserId  and Status=@Status ");
            SqlParameter[] parameters = {
                    	new SqlParameter("@SKU", SqlDbType.NVarChar,50),
                        new SqlParameter("@UserId", SqlDbType.Int,4),
                         new SqlParameter("@Status", SqlDbType.SmallInt,2)
			};
            parameters[0].Value = sku;
            parameters[1].Value = userId;
            parameters[2].Value = status;
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.PrePro.PreOrder GetModel(int userId, string sku, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  * from Shop_PreOrder ");
            strSql.Append(" where  SKU=@SKU and UserId=@UserId    and Status=@Status  ");
            SqlParameter[] parameters = {
                    	new SqlParameter("@SKU", SqlDbType.NVarChar,50),
                        new SqlParameter("@UserId", SqlDbType.Int,4),
                             new SqlParameter("@Status", SqlDbType.SmallInt,2)
			};
            parameters[0].Value = sku;
            parameters[1].Value = userId;
            parameters[2].Value = status;
            YSWL.MALL.Model.Shop.PrePro.PreOrder model = new YSWL.MALL.Model.Shop.PrePro.PreOrder();
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
        /// 更新一条数据
        /// </summary>
        public bool Update(long PreOrderId, int count, string deliveryTip)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_PreOrder set ");
            strSql.Append("Count=Count+@Count,");
            strSql.Append("DeliveryTip=@DeliveryTip ");
            strSql.Append(" where PreOrderId=@PreOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@Count", SqlDbType.Int,4),	
				new SqlParameter("@DeliveryTip", SqlDbType.NVarChar,100),
					new SqlParameter("@PreOrderId", SqlDbType.Int,4)};
            parameters[0].Value = count;
            parameters[1].Value = deliveryTip;
            parameters[2].Value = PreOrderId;
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
        ///  更新状态 
        /// </summary>
        /// <param name="PreOrderId">Id</param>
        /// <param name="Status">状态</param>
        /// <param name="HandleUserId">处理人Id</param>
        /// <returns></returns>
        public bool UpdateStatus(long PreOrderId, int Status, int HandleUserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_PreOrder set ");
            strSql.Append("Status=@Status,");
            strSql.Append("HandleUserId=@HandleUserId,");
            strSql.Append("HandleDate=@HandleDate");
            strSql.Append(" where PreOrderId=@PreOrderId");
            SqlParameter[] parameters = {
					new SqlParameter("@HandleUserId", SqlDbType.Int,4),
					new SqlParameter("@HandleDate", SqlDbType.DateTime),
                    new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@PreOrderId", SqlDbType.Int,4)};
            parameters[0].Value = HandleUserId;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = Status;
            parameters[3].Value = PreOrderId;
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
        ///  批量修改状态
        /// </summary>
        /// <param name="IDlist"></param>
        /// <param name="Status"></param>
        /// <param name="HandleUserId"></param>
        /// <returns></returns>
        public bool UpdateList(string IDlist, int Status, int HandleUserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_PreOrder set ");
            strSql.Append("Status=@Status,");
            strSql.Append("HandleUserId=@HandleUserId,");
            strSql.Append("HandleDate=@HandleDate");
            strSql.Append(" where PreOrderId in(" + IDlist + ")  ");
            SqlParameter[] parameters = {
					new SqlParameter("@HandleUserId", SqlDbType.Int,4),
					new SqlParameter("@HandleDate", SqlDbType.DateTime),
                    new SqlParameter("@Status", SqlDbType.SmallInt,2)};
            parameters[0].Value = HandleUserId;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = Status;

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

