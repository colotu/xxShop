/**  版本信息模板在安装目录下，可自行修改。
* BalanceDetails.cs
*
* 功 能： N/A
* 类 名： BalanceDetails
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/9/3 14:45:12   N/A    初版
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
using YSWL.MALL.IDAL.Pay;
using YSWL.DBUtility;//Please add references
using YSWL.MALL.Model.Pay;

namespace YSWL.MALL.SQLServerDAL.Pay
{
    /// <summary>
    /// 数据访问类:GwjfDetails
    /// </summary>
    public partial class GwjfDetails :IGwjfDetails
    {
        public GwjfDetails()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long gwjfid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Pay_gwjfDetails");
            strSql.Append(" where gwjfid=@gwjfid");
            SqlParameter[] parameters = {
                    new SqlParameter("@gwjfid", SqlDbType.BigInt)
            };
            parameters[0].Value = gwjfid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Pay.GwjfDetails model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Pay_gwjfDetails(");
            strSql.Append("UserId,TradeDate,TradeType,Income,Expenses,gwjf,remarkC,remarkCtwo,remarkCthree)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@TradeDate,@TradeType,@Income,@Expenses,@gwjf,@remarkC,@remarkCtwo,@remarkCthree)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@TradeDate", SqlDbType.DateTime),
                    new SqlParameter("@TradeType", SqlDbType.Int,4),
                    new SqlParameter("@Income", SqlDbType.Decimal,9),
                    new SqlParameter("@Expenses", SqlDbType.VarChar,200),
                    new SqlParameter("@gwjf", SqlDbType.Decimal,9),
                    new SqlParameter("@remarkC", SqlDbType.VarChar,200),
                    new SqlParameter("@remarkCtwo", SqlDbType.VarChar,200),
                    new SqlParameter("@remarkCthree", SqlDbType.VarChar,200)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.TradeDate;
            parameters[2].Value = model.TradeType;
            parameters[3].Value = model.Income;
            parameters[4].Value = model.Expenses;
            parameters[5].Value = model.gwjf;
            parameters[6].Value = model.remarkC;
            parameters[7].Value = model.remarkCtwo;
            parameters[8].Value = model.remarkCthree;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        public bool Update(YSWL.MALL.Model.Pay.GwjfDetails model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Pay_gwjfDetails set ");
            strSql.Append("UserId=@UserId,");
            strSql.Append("TradeDate=@TradeDate,");
            strSql.Append("TradeType=@TradeType,");
            strSql.Append("Income=@Income,");
            strSql.Append("Expenses=@Expenses,");
            strSql.Append("gwjf=@gwjf,");
            strSql.Append("remarkC=@remarkC,");
            strSql.Append("remarkCtwo=@remarkCtwo,");
            strSql.Append("remarkCthree=@remarkCthree");
            strSql.Append(" where gwjfid=@gwjfid");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@TradeDate", SqlDbType.DateTime),
                    new SqlParameter("@TradeType", SqlDbType.Int,4),
                    new SqlParameter("@Income", SqlDbType.Decimal,9),
                    new SqlParameter("@Expenses", SqlDbType.VarChar,200),
                    new SqlParameter("@gwjf", SqlDbType.Decimal,9),
                    new SqlParameter("@remarkC", SqlDbType.VarChar,200),
                    new SqlParameter("@remarkCtwo", SqlDbType.VarChar,200),
                    new SqlParameter("@remarkCthree", SqlDbType.VarChar,200),
                    new SqlParameter("@gwjfid", SqlDbType.BigInt,8)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.TradeDate;
            parameters[2].Value = model.TradeType;
            parameters[3].Value = model.Income;
            parameters[4].Value = model.Expenses;
            parameters[5].Value = model.gwjf;
            parameters[6].Value = model.remarkC;
            parameters[7].Value = model.remarkCtwo;
            parameters[8].Value = model.remarkCthree;
            parameters[9].Value = model.gwjfid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(long gwjfid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Pay_gwjfDetails ");
            strSql.Append(" where gwjfid=@gwjfid");
            SqlParameter[] parameters = {
                    new SqlParameter("@gwjfid", SqlDbType.BigInt)
            };
            parameters[0].Value = gwjfid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool DeleteList(string gwjfidlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Pay_gwjfDetails ");
            strSql.Append(" where gwjfid in (" + gwjfidlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
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
        public YSWL.MALL.Model.Pay.GwjfDetails GetModel(long gwjfid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gwjfid,UserId,TradeDate,TradeType,Income,Expenses,gwjf,remarkC,remarkCtwo,remarkCthree from Pay_gwjfDetails ");
            strSql.Append(" where gwjfid=@gwjfid");
            SqlParameter[] parameters = {
                    new SqlParameter("@gwjfid", SqlDbType.BigInt)
            };
            parameters[0].Value = gwjfid;

            YSWL.MALL.Model.Pay.GwjfDetails model = new YSWL.MALL.Model.Pay.GwjfDetails();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
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
        public YSWL.MALL.Model.Pay.GwjfDetails DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Pay.GwjfDetails model = new YSWL.MALL.Model.Pay.GwjfDetails();
            if (row != null)
            {
                if (row["gwjfid"] != null && row["gwjfid"].ToString() != "")
                {
                    model.gwjfid = long.Parse(row["gwjfid"].ToString());
                }
                if (row["UserId"] != null && row["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["TradeDate"] != null && row["TradeDate"].ToString() != "")
                {
                    model.TradeDate = DateTime.Parse(row["TradeDate"].ToString());
                }
                if (row["TradeType"] != null && row["TradeType"].ToString() != "")
                {
                    model.TradeType = int.Parse(row["TradeType"].ToString());
                }
                if (row["Income"] != null && row["Income"].ToString() != "")
                {
                    model.Income = decimal.Parse(row["Income"].ToString());
                }
                if (row["Expenses"] != null)
                {
                    model.Expenses = row["Expenses"].ToString();
                }
                if (row["hbje"] != null && row["hbje"].ToString() != "")
                {
                    model.gwjf = decimal.Parse(row["hbje"].ToString());
                }
                if (row["remarkC"] != null)
                {
                    model.remarkC = row["remarkC"].ToString();
                }
                if (row["remarkCtwo"] != null)
                {
                    model.remarkCtwo = row["remarkCtwo"].ToString();
                }
                if (row["remarkCthree"] != null)
                {
                    model.remarkCthree = row["remarkCthree"].ToString();
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
            strSql.Append("select gwjfid,UserId,TradeDate,TradeType,Income,Expenses,gwjf,remarkC,remarkCtwo,remarkCthree ");
            strSql.Append(" FROM Pay_gwjfDetails ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
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
            strSql.Append(" gwjfid,UserId,TradeDate,TradeType,Income,Expenses,gwjf,remarkC,remarkCtwo,remarkCthree ");
            strSql.Append(" FROM Pay_gwjfDetails ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Pay_gwjfDetails ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
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
                strSql.Append("order by T.gwjfid desc");
            }
            strSql.Append(")AS Row, T.*  from Pay_gwjfDetails T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
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
            parameters[0].Value = "Pay_gwjfDetails";
            parameters[1].Value = "gwjfid";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod

        

        Model.Pay.GwjfDetails IGwjfDetails.GetModel(long JournalNumber)
        {
            throw new NotImplementedException();
        }

        Model.Pay.GwjfDetails IGwjfDetails.DataRowToModel(DataRow row)
        {
            throw new NotImplementedException();
        }

        public bool Pay(decimal amount, int userId, string remark)
        {
            throw new NotImplementedException();
        }
        //OfflineHBJEzs
    }
}