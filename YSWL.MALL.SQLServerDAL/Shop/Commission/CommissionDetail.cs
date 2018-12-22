/**  版本信息模板在安装目录下，可自行修改。
* CommissionDetail.cs
*
* 功 能： N/A
* 类 名： CommissionDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/2/26 16:51:35   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Commission;
using YSWL.DBUtility;
using YSWL.MALL.Model.Shop.Order;

//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Commission
{
	/// <summary>
	/// 数据访问类:CommissionDetail
	/// </summary>
	public partial class CommissionDetail:ICommissionDetail
	{
		public CommissionDetail()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long DetailId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_CommissionDetail");
            strSql.Append(" where DetailId=@DetailId");
            SqlParameter[] parameters = {
					new SqlParameter("@DetailId", SqlDbType.BigInt)
			};
            parameters[0].Value = DetailId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.Commission.CommissionDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_CommissionDetail(");
            strSql.Append("RuleId,RuleName,UserId,UserName,RuleLevel,TradeDate,TradeType,Fee,OrderId,OrderCode,BuyerID,BuyerName,OrderAmount,ReferID,ReferType,ProductId,Quantity,Name,SupplierId,SupplierName,BrandId,BrandName,Remark)");
            strSql.Append(" values (");
            strSql.Append("@RuleId,@RuleName,@UserId,@UserName,@RuleLevel,@TradeDate,@TradeType,@Fee,@OrderId,@OrderCode,@BuyerID,@BuyerName,@OrderAmount,@ReferID,@ReferType,@ProductId,@Quantity,@Name,@SupplierId,@SupplierName,@BrandId,@BrandName,@Remark)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@RuleName", SqlDbType.NVarChar,200),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@RuleLevel", SqlDbType.Int,4),
					new SqlParameter("@TradeDate", SqlDbType.DateTime),
					new SqlParameter("@TradeType", SqlDbType.Int,4),
					new SqlParameter("@Fee", SqlDbType.Money,8),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@BuyerID", SqlDbType.Int,4),
					new SqlParameter("@BuyerName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderAmount", SqlDbType.Money,8),
					new SqlParameter("@ReferID", SqlDbType.Int,4),
					new SqlParameter("@ReferType", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@Quantity", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
					new SqlParameter("@BrandId", SqlDbType.Int,4),
					new SqlParameter("@BrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000)};
            parameters[0].Value = model.RuleId;
            parameters[1].Value = model.RuleName;
            parameters[2].Value = model.UserId;
            parameters[3].Value = model.UserName;
            parameters[4].Value = model.RuleLevel;
            parameters[5].Value = model.TradeDate;
            parameters[6].Value = model.TradeType;
            parameters[7].Value = model.Fee;
            parameters[8].Value = model.OrderId;
            parameters[9].Value = model.OrderCode;
            parameters[10].Value = model.BuyerID;
            parameters[11].Value = model.BuyerName;
            parameters[12].Value = model.OrderAmount;
            parameters[13].Value = model.ReferID;
            parameters[14].Value = model.ReferType;
            parameters[15].Value = model.ProductId;
            parameters[16].Value = model.Quantity;
            parameters[17].Value = model.Name;
            parameters[18].Value = model.SupplierId;
            parameters[19].Value = model.SupplierName;
            parameters[20].Value = model.BrandId;
            parameters[21].Value = model.BrandName;
            parameters[22].Value = model.Remark;

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
        public bool Update(YSWL.MALL.Model.Shop.Commission.CommissionDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_CommissionDetail set ");
            strSql.Append("RuleId=@RuleId,");
            strSql.Append("RuleName=@RuleName,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("RuleLevel=@RuleLevel,");
            strSql.Append("TradeDate=@TradeDate,");
            strSql.Append("TradeType=@TradeType,");
            strSql.Append("Fee=@Fee,");
            strSql.Append("OrderId=@OrderId,");
            strSql.Append("OrderCode=@OrderCode,");
            strSql.Append("BuyerID=@BuyerID,");
            strSql.Append("BuyerName=@BuyerName,");
            strSql.Append("OrderAmount=@OrderAmount,");
            strSql.Append("ReferID=@ReferID,");
            strSql.Append("ReferType=@ReferType,");
            strSql.Append("ProductId=@ProductId,");
            strSql.Append("Quantity=@Quantity,");
            strSql.Append("Name=@Name,");
            strSql.Append("SupplierId=@SupplierId,");
            strSql.Append("SupplierName=@SupplierName,");
            strSql.Append("BrandId=@BrandId,");
            strSql.Append("BrandName=@BrandName,");
            strSql.Append("Remark=@Remark");
            strSql.Append(" where DetailId=@DetailId");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@RuleName", SqlDbType.NVarChar,200),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@RuleLevel", SqlDbType.Int,4),
					new SqlParameter("@TradeDate", SqlDbType.DateTime),
					new SqlParameter("@TradeType", SqlDbType.Int,4),
					new SqlParameter("@Fee", SqlDbType.Money,8),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@BuyerID", SqlDbType.Int,4),
					new SqlParameter("@BuyerName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderAmount", SqlDbType.Money,8),
					new SqlParameter("@ReferID", SqlDbType.Int,4),
					new SqlParameter("@ReferType", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@Quantity", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
					new SqlParameter("@BrandId", SqlDbType.Int,4),
					new SqlParameter("@BrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@DetailId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.RuleId;
            parameters[1].Value = model.RuleName;
            parameters[2].Value = model.UserId;
            parameters[3].Value = model.UserName;
            parameters[4].Value = model.RuleLevel;
            parameters[5].Value = model.TradeDate;
            parameters[6].Value = model.TradeType;
            parameters[7].Value = model.Fee;
            parameters[8].Value = model.OrderId;
            parameters[9].Value = model.OrderCode;
            parameters[10].Value = model.BuyerID;
            parameters[11].Value = model.BuyerName;
            parameters[12].Value = model.OrderAmount;
            parameters[13].Value = model.ReferID;
            parameters[14].Value = model.ReferType;
            parameters[15].Value = model.ProductId;
            parameters[16].Value = model.Quantity;
            parameters[17].Value = model.Name;
            parameters[18].Value = model.SupplierId;
            parameters[19].Value = model.SupplierName;
            parameters[20].Value = model.BrandId;
            parameters[21].Value = model.BrandName;
            parameters[22].Value = model.Remark;
            parameters[23].Value = model.DetailId;

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
        public bool Delete(long DetailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_CommissionDetail ");
            strSql.Append(" where DetailId=@DetailId");
            SqlParameter[] parameters = {
					new SqlParameter("@DetailId", SqlDbType.BigInt)
			};
            parameters[0].Value = DetailId;

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
        public bool DeleteList(string DetailIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_CommissionDetail ");
            strSql.Append(" where DetailId in (" + DetailIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.Commission.CommissionDetail GetModel(long DetailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 DetailId,RuleId,RuleName,UserId,UserName,RuleLevel,TradeDate,TradeType,Fee,OrderId,OrderCode,BuyerID,BuyerName,OrderAmount,ReferID,ReferType,ProductId,Quantity,Name,SupplierId,SupplierName,BrandId,BrandName,Remark from Shop_CommissionDetail ");
            strSql.Append(" where DetailId=@DetailId");
            SqlParameter[] parameters = {
					new SqlParameter("@DetailId", SqlDbType.BigInt)
			};
            parameters[0].Value = DetailId;

            YSWL.MALL.Model.Shop.Commission.CommissionDetail model = new YSWL.MALL.Model.Shop.Commission.CommissionDetail();
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
        public YSWL.MALL.Model.Shop.Commission.CommissionDetail DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Commission.CommissionDetail model = new YSWL.MALL.Model.Shop.Commission.CommissionDetail();
            if (row != null)
            {
                if (row["DetailId"] != null && row["DetailId"].ToString() != "")
                {
                    model.DetailId = long.Parse(row["DetailId"].ToString());
                }
                if (row["RuleId"] != null && row["RuleId"].ToString() != "")
                {
                    model.RuleId = int.Parse(row["RuleId"].ToString());
                }
                if (row["RuleName"] != null)
                {
                    model.RuleName = row["RuleName"].ToString();
                }
                if (row["UserId"] != null && row["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["RuleLevel"] != null && row["RuleLevel"].ToString() != "")
                {
                    model.RuleLevel = int.Parse(row["RuleLevel"].ToString());
                }
                if (row["TradeDate"] != null && row["TradeDate"].ToString() != "")
                {
                    model.TradeDate = DateTime.Parse(row["TradeDate"].ToString());
                }
                if (row["TradeType"] != null && row["TradeType"].ToString() != "")
                {
                    model.TradeType = int.Parse(row["TradeType"].ToString());
                }
                if (row["Fee"] != null && row["Fee"].ToString() != "")
                {
                    model.Fee = decimal.Parse(row["Fee"].ToString());
                }
                if (row["OrderId"] != null && row["OrderId"].ToString() != "")
                {
                    model.OrderId = long.Parse(row["OrderId"].ToString());
                }
                if (row["OrderCode"] != null)
                {
                    model.OrderCode = row["OrderCode"].ToString();
                }
                if (row["BuyerID"] != null && row["BuyerID"].ToString() != "")
                {
                    model.BuyerID = int.Parse(row["BuyerID"].ToString());
                }
                if (row["BuyerName"] != null)
                {
                    model.BuyerName = row["BuyerName"].ToString();
                }
                if (row["OrderAmount"] != null && row["OrderAmount"].ToString() != "")
                {
                    model.OrderAmount = decimal.Parse(row["OrderAmount"].ToString());
                }
                if (row["ReferID"] != null && row["ReferID"].ToString() != "")
                {
                    model.ReferID = int.Parse(row["ReferID"].ToString());
                }
                if (row["ReferType"] != null && row["ReferType"].ToString() != "")
                {
                    model.ReferType = int.Parse(row["ReferType"].ToString());
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["Quantity"] != null && row["Quantity"].ToString() != "")
                {
                    model.Quantity = int.Parse(row["Quantity"].ToString());
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["SupplierId"] != null && row["SupplierId"].ToString() != "")
                {
                    model.SupplierId = int.Parse(row["SupplierId"].ToString());
                }
                if (row["SupplierName"] != null)
                {
                    model.SupplierName = row["SupplierName"].ToString();
                }
                if (row["BrandId"] != null && row["BrandId"].ToString() != "")
                {
                    model.BrandId = int.Parse(row["BrandId"].ToString());
                }
                if (row["BrandName"] != null)
                {
                    model.BrandName = row["BrandName"].ToString();
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
            strSql.Append("select DetailId,RuleId,RuleName,UserId,UserName,RuleLevel,TradeDate,TradeType,Fee,OrderId,OrderCode,BuyerID,BuyerName,OrderAmount,ReferID,ReferType,ProductId,Quantity,Name,SupplierId,SupplierName,BrandId,BrandName,Remark ");
            strSql.Append(" FROM Shop_CommissionDetail ");
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
            strSql.Append(" DetailId,RuleId,RuleName,UserId,UserName,RuleLevel,TradeDate,TradeType,Fee,OrderId,OrderCode,BuyerID,BuyerName,OrderAmount,ReferID,ReferType,ProductId,Quantity,Name,SupplierId,SupplierName,BrandId,BrandName,Remark ");
            strSql.Append(" FROM Shop_CommissionDetail ");
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
            strSql.Append("select count(1) FROM Shop_CommissionDetail ");
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
                strSql.Append("order by T.DetailId desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_CommissionDetail T ");
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
            parameters[0].Value = "Shop_CommissionDetail";
            parameters[1].Value = "DetailId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
		#region  ExtensionMethod

        public bool AddDetail(YSWL.MALL.Model.Shop.Commission.CommissionDetail model)
	    {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_CommissionDetail(");
            strSql.Append("RuleId,RuleName,UserId,UserName,RuleLevel,TradeDate,TradeType,Fee,OrderId,OrderCode,BuyerID,BuyerName,OrderAmount,ReferID,ReferType,ProductId,Quantity,Name,SupplierId,SupplierName,BrandId,BrandName,Remark)");
            strSql.Append(" values (");
            strSql.Append("@RuleId,@RuleName,@UserId,@UserName,@RuleLevel,@TradeDate,@TradeType,@Fee,@OrderId,@OrderCode,@BuyerID,@BuyerName,@OrderAmount,@ReferID,@ReferType,@ProductId,@Quantity,@Name,@SupplierId,@SupplierName,@BrandId,@BrandName,@Remark)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@RuleName", SqlDbType.NVarChar,200),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@RuleLevel", SqlDbType.Int,4),
					new SqlParameter("@TradeDate", SqlDbType.DateTime),
					new SqlParameter("@TradeType", SqlDbType.Int,4),
					new SqlParameter("@Fee", SqlDbType.Money,8),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@BuyerID", SqlDbType.Int,4),
					new SqlParameter("@BuyerName", SqlDbType.NVarChar,100),
					new SqlParameter("@OrderAmount", SqlDbType.Money,8),
					new SqlParameter("@ReferID", SqlDbType.Int,4),
					new SqlParameter("@ReferType", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@Quantity", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@SupplierName", SqlDbType.NVarChar,100),
					new SqlParameter("@BrandId", SqlDbType.Int,4),
					new SqlParameter("@BrandName", SqlDbType.NVarChar,100),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000)};
            parameters[0].Value = model.RuleId;
            parameters[1].Value = model.RuleName;
            parameters[2].Value = model.UserId;
            parameters[3].Value = model.UserName;
            parameters[4].Value = model.RuleLevel;
            parameters[5].Value = model.TradeDate;
            parameters[6].Value = model.TradeType;
            parameters[7].Value = model.Fee;
            parameters[8].Value = model.OrderId;
            parameters[9].Value = model.OrderCode;
            parameters[10].Value = model.BuyerID;
            parameters[11].Value = model.BuyerName;
            parameters[12].Value = model.OrderAmount;
            parameters[13].Value = model.ReferID;
            parameters[14].Value = model.ReferType;
            parameters[15].Value = model.ProductId;
            parameters[16].Value = model.Quantity;
            parameters[17].Value = model.Name;
            parameters[18].Value = model.SupplierId;
            parameters[19].Value = model.SupplierName;
            parameters[20].Value = model.BrandId;
            parameters[21].Value = model.BrandName;
            parameters[22].Value = model.Remark;
            cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);


    

            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append(" Insert into  Pay_BalanceDetails(");
            strSql3.Append("UserId,TradeDate,TradeType,Income,Balance,Remark)");
            strSql3.Append(" select  ");
            strSql3.Append("@UserId,@TradeDate,@TradeType,@Income,@Balance+Balance,@Remark from Accounts_UsersExp where UserId=@UserId");
         
            SqlParameter[] parameters3 = {
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@TradeDate", SqlDbType.DateTime),
                    new SqlParameter("@TradeType", SqlDbType.Int,4),
                    new SqlParameter("@Income", SqlDbType.Money,8),
                    new SqlParameter("@Balance", SqlDbType.Money,8),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,2000)
                 };
            parameters3[0].Value = model.UserId;
            parameters3[1].Value = DateTime.Now;
            parameters3[2].Value = 1;
            parameters3[3].Value = model.Fee;//收入
            parameters3[4].Value = model.Fee;//余额
            parameters3[5].Value = "商品推广佣金，订单为["+model.OrderCode+"]"; //备注

            cmd = new CommandInfo(strSql3.ToString(), parameters3);
            sqllist.Add(cmd);

            //更新用户表中的余额
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append(" update Accounts_UsersExp set ");
            strSql2.Append(" Balance=Balance+ @Balance where UserID=@UserId");
            SqlParameter[] parameters2 = {
                    new SqlParameter("@Balance", SqlDbType.Money,8),
                   new SqlParameter("@UserId", SqlDbType.Int,4)
                                        };
            parameters2[0].Value = model.Fee;//充值金额
            parameters2[1].Value = model.UserId;
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            return DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist) > 0;
	    }

	    public decimal GetUserFees(int userId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sum(Fee) FROM Shop_CommissionDetail  where UserId=@UserId");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserId", SqlDbType.Int,4)
                 };
            parameters[0].Value = userId;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeDecimal(obj,0);
            }
	    }

	    public DataSet StatUserFee(DateTime startDate, DateTime endDate)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Select T.*,A.NickName from (  ");
            strSql.Append("SELECT UserId, UserName,SUM(Fee) TotalFee,COUNT(DISTINCT(OrderId)) OrderCount  FROM  Shop_CommissionDetail  ");
            strSql.AppendFormat(" WHERE TradeType=1 and  TradeDate >'{0}' AND TradeDate<'{1}' GROUP BY UserId,UserName ", startDate, endDate);
            strSql.Append("  )T inner join  Accounts_Users A  on t.UserId=a.UserID  ORDER BY TotalFee DESC  ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
	    }

        public DataSet StatProFee(DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ProductId, Name,SUM(Fee) TotalFee,COUNT(DISTINCT(OrderId)) OrderCount,COUNT(DISTINCT(UserId)) UserCount FROM  Shop_CommissionDetail  ");
            strSql.AppendFormat("  WHERE TradeType=1 and  TradeDate >'{0}' AND TradeDate<'{1}' GROUP BY ProductId,Name  ORDER BY TotalFee DESC ", startDate, endDate);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public DataSet StatCommission(DateTime startDate, DateTime endDate, StatisticMode mode = StatisticMode.Day)
	    {
            StringBuilder strSql = new StringBuilder();
            int length = 12;
            switch (mode)
            {
                case StatisticMode.Day:
                    length = 12;
                    break;
                case StatisticMode.Month:
                    length = 7;
                    break;
                default:
                    length = 12;
                    break;

            }
            strSql.AppendFormat("SELECT  CONVERT(VARCHAR({0}), TradeDate, 111) AS TradeDate ,  SUM(Fee) TotalFee , COUNT(DISTINCT ( OrderId )) OrderCount , COUNT(DISTINCT ( UserId )) UserCount ", length);
            strSql.Append(" FROM    Shop_CommissionDetail WHERE   TradeType = 1  ");
            strSql.AppendFormat("  and  TradeDate>'{0}' ", startDate);
            strSql.AppendFormat(" and  TradeDate<'{0}' ", endDate);
            strSql.AppendFormat(" GROUP BY CONVERT(VARCHAR({0}), TradeDate, 111) ORDER BY TradeDate ", length);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
	    }
        /// <summary>
        /// 按佣金规则统计佣金金额
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet StatRuleFee(DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT RuleId, RuleName,SUM(Fee) TotalFee,COUNT(DISTINCT(OrderId)) OrderCount  FROM  Shop_CommissionDetail  ");
            strSql.AppendFormat(" WHERE TradeType=1 and  TradeDate >'{0}' AND TradeDate<'{1}' GROUP BY RuleId,RuleName  ORDER BY TotalFee DESC ", startDate, endDate);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 按佣金规则统计佣金商品数量
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet StatRulePro(DateTime startDate, DateTime endDate)
        {
              StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT RuleId, RuleName, sum(Quantity) TotalProduct  FROM  Shop_CommissionDetail "); 
            strSql.AppendFormat(" WHERE TradeType=1 and RuleLevel = 1 and  TradeDate >'{0}' AND TradeDate<'{1}' GROUP BY RuleId,RuleName  ORDER BY TotalProduct DESC ", startDate, endDate);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 按商品统计
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="RuleLevel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet StatPro(int userId , int RuleLevel, DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ProductId, Name, sum(Quantity) TotalProduct,sum(Fee) TotalFee FROM  Shop_CommissionDetail ");
            strSql.Append(" WHERE TradeType=1 ");
            strSql.AppendFormat(" and  RuleLevel={0} ",RuleLevel);
            strSql.AppendFormat(" and  userId={0} ", userId);            
            strSql.AppendFormat("  and  TradeDate >'{0}' AND TradeDate<'{1}' ", startDate, endDate);
            strSql.AppendFormat(" GROUP BY ProductId,Name  ORDER BY TotalProduct DESC  ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 按商品统计
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="RuleLevel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet StatPro(int userId, int RuleLevel, DateTime startDate, DateTime endDate,int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.TotalFee desc");
            strSql.Append(")AS Row, T.*  from   ");
            strSql.Append(" (   ");
            strSql.Append(" SELECT ProductId, Name, sum(Quantity) TotalProduct,sum(Fee) TotalFee FROM  Shop_CommissionDetail ");
            strSql.Append(" WHERE TradeType=1 ");
            strSql.AppendFormat(" and  RuleLevel={0} ", RuleLevel);
            strSql.AppendFormat(" and  userId={0} ", userId);
            strSql.AppendFormat("  and  TradeDate >'{0}' AND TradeDate<'{1}' ", startDate, endDate);
            strSql.AppendFormat(" GROUP BY ProductId,Name ");
            strSql.Append("  ) T  ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 根据条件获取总佣金
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="RuleLevel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal GetTotalFee(int userId, int RuleLevel, DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(Fee) TotalFee FROM  Shop_CommissionDetail ");
            strSql.Append(" WHERE TradeType=1 ");
            strSql.AppendFormat(" and  RuleLevel={0} ", RuleLevel);
            strSql.AppendFormat(" and  userId={0} ", userId);
            strSql.AppendFormat("  and  TradeDate >'{0}' AND TradeDate<'{1}' ", startDate, endDate);
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeDecimal(obj,0);
            }
        }
        /// <summary>
        /// 按商品统计获取总记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="RuleLevel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int StatProRecordCount(int userId, int RuleLevel, DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  select count(ProductId) from ( ");
            strSql.Append(" SELECT ProductId FROM  Shop_CommissionDetail ");
            strSql.Append(" WHERE TradeType=1 ");
            strSql.AppendFormat(" and  RuleLevel={0} ", RuleLevel);
            strSql.AppendFormat(" and  userId={0} ", userId);
            strSql.AppendFormat("  and  TradeDate >'{0}' AND TradeDate<'{1}' ", startDate, endDate);
            strSql.AppendFormat(" GROUP BY ProductId,Name   ");
            strSql.Append(" ) T ");
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

        #region 盟友排行
        /// <summary>
        /// 盟友排行  佣金维度（分页）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataSet AllyRanking(int userId, DateTime startDate, DateTime endDate, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.TotalFee desc");
            strSql.Append(")AS Row, T.*  from   ");
            strSql.Append(" (   ");

            strSql.Append(" select C.ReferID as UserId, C.TotalProduct,C.TotalFee ,A.NickName from (   ");
            strSql.Append(" SELECT ReferID   , sum(Quantity) TotalProduct,sum(Fee) TotalFee FROM  Shop_CommissionDetail ");
            strSql.Append(" WHERE TradeType=1 ");
            strSql.Append(" and  RuleLevel=2  " );
            strSql.AppendFormat(" and  userId={0} ", userId);
            strSql.AppendFormat("  and  TradeDate >'{0}' AND TradeDate<'{1}' ", startDate, endDate);
            strSql.AppendFormat(" GROUP BY  ReferID ");
            strSql.Append(" ) C inner join  Accounts_Users A  on C.ReferID=A.UserID   ");

            strSql.Append("  ) T  ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 盟友排行总数   佣金维度
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int AllyRankingRecordCount(int userId, DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  select count(ReferID) from ( ");
            strSql.Append(" SELECT ReferID FROM  Shop_CommissionDetail ");
            strSql.Append(" WHERE TradeType=1 ");
            strSql.AppendFormat(" and  RuleLevel=2 ");
            strSql.AppendFormat(" and  userId={0} ", userId);
            strSql.AppendFormat("  and  TradeDate >'{0}' AND TradeDate<'{1}' ", startDate, endDate);
            strSql.AppendFormat(" GROUP BY  ReferID    ");
            strSql.Append(" ) T ");
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
        /// <summary>
        /// 获取订单数和商品数  
        /// </summary> 
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetOrderCount(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT   COUNT(DISTINCT(OrderId)) OrderCount ,COUNT(DISTINCT(productId)) productCount from dbo.Shop_CommissionDetail   ");
            strSql.Append(" WHERE TradeType=1  and  RuleLevel=1  ");
            strSql.AppendFormat(" and  userId={0} ", userId);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        #endregion  ExtensionMethod
    }
}

