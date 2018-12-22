/**  版本信息模板在安装目录下，可自行修改。
* ActivityInfo.cs
*
* 功 能： N/A
* 类 名： ActivityInfo
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/6/10 22:26:32   N/A    初版
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
using YSWL.MALL.IDAL.Shop.Activity;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Activity
{
	/// <summary>
	/// 数据访问类:ActivityInfo
	/// </summary>
	public partial class ActivityInfo:IActivityInfo
	{
		public ActivityInfo()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("ActivityId", "Shop_ActivityInfo");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ActivityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_ActivityInfo");
            strSql.Append(" where ActivityId=@ActivityId");
            SqlParameter[] parameters = {
					new SqlParameter("@ActivityId", SqlDbType.Int,4)
			};
            parameters[0].Value = ActivityId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Shop.Activity.ActivityInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_ActivityInfo(");
            strSql.Append("RuleId,BuyCategoryId,BuyCategoryName,BuyProductId,BuyProductName,BuySKU,BuyCount,CpRuleId,CpRuleName,ProductId,ProductName,SKU,SalePrice,LimitPrice,LimitMaxPrice,Count,MaxCount,Status,StartDate,EndDate,CreatedUserId,CreatedDate,SupplierId)");
            strSql.Append(" values (");
            strSql.Append("@RuleId,@BuyCategoryId,@BuyCategoryName,@BuyProductId,@BuyProductName,@BuySKU,@BuyCount,@CpRuleId,@CpRuleName,@ProductId,@ProductName,@SKU,@SalePrice,@LimitPrice,@LimitMaxPrice,@Count,@MaxCount,@Status,@StartDate,@EndDate,@CreatedUserId,@CreatedDate,@SupplierId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@BuyCategoryId", SqlDbType.Int,4),
					new SqlParameter("@BuyCategoryName", SqlDbType.NVarChar,100),
					new SqlParameter("@BuyProductId", SqlDbType.BigInt,8),
					new SqlParameter("@BuyProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@BuySKU", SqlDbType.NVarChar,200),
					new SqlParameter("@BuyCount", SqlDbType.Int,4),
					new SqlParameter("@CpRuleId", SqlDbType.Int,4),
					new SqlParameter("@CpRuleName", SqlDbType.NVarChar,200),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@SKU", SqlDbType.NVarChar,200),
					new SqlParameter("@SalePrice", SqlDbType.Money,8),
					new SqlParameter("@LimitPrice", SqlDbType.Money,8),
					new SqlParameter("@LimitMaxPrice", SqlDbType.Money,8),
					new SqlParameter("@Count", SqlDbType.Int,4),
					new SqlParameter("@MaxCount", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@StartDate", SqlDbType.DateTime),
					new SqlParameter("@EndDate", SqlDbType.DateTime),
					new SqlParameter("@CreatedUserId", SqlDbType.Int,4),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@SupplierId", SqlDbType.Int,4)};
            parameters[0].Value = model.RuleId;
            parameters[1].Value = model.BuyCategoryId;
            parameters[2].Value = model.BuyCategoryName;
            parameters[3].Value = model.BuyProductId;
            parameters[4].Value = model.BuyProductName;
            parameters[5].Value = model.BuySKU;
            parameters[6].Value = model.BuyCount;
            parameters[7].Value = model.CpRuleId;
            parameters[8].Value = model.CpRuleName;
            parameters[9].Value = model.ProductId;
            parameters[10].Value = model.ProductName;
            parameters[11].Value = model.SKU;
            parameters[12].Value = model.SalePrice;
            parameters[13].Value = model.LimitPrice;
            parameters[14].Value = model.LimitMaxPrice;
            parameters[15].Value = model.Count;
            parameters[16].Value = model.MaxCount;
            parameters[17].Value = model.Status;
            parameters[18].Value = model.StartDate;
            parameters[19].Value = model.EndDate;
            parameters[20].Value = model.CreatedUserId;
            parameters[21].Value = model.CreatedDate;
            parameters[22].Value = model.SupplierId;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Activity.ActivityInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ActivityInfo set ");
            strSql.Append("RuleId=@RuleId,");
            strSql.Append("BuyCategoryId=@BuyCategoryId,");
            strSql.Append("BuyCategoryName=@BuyCategoryName,");
            strSql.Append("BuyProductId=@BuyProductId,");
            strSql.Append("BuyProductName=@BuyProductName,");
            strSql.Append("BuySKU=@BuySKU,");
            strSql.Append("BuyCount=@BuyCount,");
            strSql.Append("CpRuleId=@CpRuleId,");
            strSql.Append("CpRuleName=@CpRuleName,");
            strSql.Append("ProductId=@ProductId,");
            strSql.Append("ProductName=@ProductName,");
            strSql.Append("SKU=@SKU,");
            strSql.Append("SalePrice=@SalePrice,");
            strSql.Append("LimitPrice=@LimitPrice,");
            strSql.Append("LimitMaxPrice=@LimitMaxPrice,");
            strSql.Append("Count=@Count,");
            strSql.Append("MaxCount=@MaxCount,");
            strSql.Append("Status=@Status,");
            strSql.Append("StartDate=@StartDate,");
            strSql.Append("EndDate=@EndDate,");
            strSql.Append("CreatedUserId=@CreatedUserId,");
            strSql.Append("CreatedDate=@CreatedDate,");
            strSql.Append("SupplierId=@SupplierId");
            strSql.Append(" where ActivityId=@ActivityId");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@BuyCategoryId", SqlDbType.Int,4),
					new SqlParameter("@BuyCategoryName", SqlDbType.NVarChar,100),
					new SqlParameter("@BuyProductId", SqlDbType.BigInt,8),
					new SqlParameter("@BuyProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@BuySKU", SqlDbType.NVarChar,200),
					new SqlParameter("@BuyCount", SqlDbType.Int,4),
					new SqlParameter("@CpRuleId", SqlDbType.Int,4),
					new SqlParameter("@CpRuleName", SqlDbType.NVarChar,200),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@SKU", SqlDbType.NVarChar,200),
					new SqlParameter("@SalePrice", SqlDbType.Money,8),
					new SqlParameter("@LimitPrice", SqlDbType.Money,8),
					new SqlParameter("@LimitMaxPrice", SqlDbType.Money,8),
					new SqlParameter("@Count", SqlDbType.Int,4),
					new SqlParameter("@MaxCount", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@StartDate", SqlDbType.DateTime),
					new SqlParameter("@EndDate", SqlDbType.DateTime),
					new SqlParameter("@CreatedUserId", SqlDbType.Int,4),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@ActivityId", SqlDbType.Int,4)};
            parameters[0].Value = model.RuleId;
            parameters[1].Value = model.BuyCategoryId;
            parameters[2].Value = model.BuyCategoryName;
            parameters[3].Value = model.BuyProductId;
            parameters[4].Value = model.BuyProductName;
            parameters[5].Value = model.BuySKU;
            parameters[6].Value = model.BuyCount;
            parameters[7].Value = model.CpRuleId;
            parameters[8].Value = model.CpRuleName;
            parameters[9].Value = model.ProductId;
            parameters[10].Value = model.ProductName;
            parameters[11].Value = model.SKU;
            parameters[12].Value = model.SalePrice;
            parameters[13].Value = model.LimitPrice;
            parameters[14].Value = model.LimitMaxPrice;
            parameters[15].Value = model.Count;
            parameters[16].Value = model.MaxCount;
            parameters[17].Value = model.Status;
            parameters[18].Value = model.StartDate;
            parameters[19].Value = model.EndDate;
            parameters[20].Value = model.CreatedUserId;
            parameters[21].Value = model.CreatedDate;
            parameters[22].Value = model.SupplierId;
            parameters[23].Value = model.ActivityId;

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
        public bool Delete(int ActivityId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_ActivityInfo ");
            strSql.Append(" where ActivityId=@ActivityId");
            SqlParameter[] parameters = {
					new SqlParameter("@ActivityId", SqlDbType.Int,4)
			};
            parameters[0].Value = ActivityId;

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
        public bool DeleteList(string ActivityIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_ActivityInfo ");
            strSql.Append(" where ActivityId in (" + ActivityIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.Activity.ActivityInfo GetModel(int ActivityId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ActivityId,RuleId,BuyCategoryId,BuyCategoryName,BuyProductId,BuyProductName,BuySKU,BuyCount,CpRuleId,CpRuleName,ProductId,ProductName,SKU,SalePrice,LimitPrice,LimitMaxPrice,Count,MaxCount,Status,StartDate,EndDate,CreatedUserId,CreatedDate,SupplierId from Shop_ActivityInfo ");
            strSql.Append(" where ActivityId=@ActivityId");
            SqlParameter[] parameters = {
					new SqlParameter("@ActivityId", SqlDbType.Int,4)
			};
            parameters[0].Value = ActivityId;

            YSWL.MALL.Model.Shop.Activity.ActivityInfo model = new YSWL.MALL.Model.Shop.Activity.ActivityInfo();
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
        public YSWL.MALL.Model.Shop.Activity.ActivityInfo DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Activity.ActivityInfo model = new YSWL.MALL.Model.Shop.Activity.ActivityInfo();
            if (row != null)
            {
                if (row["ActivityId"] != null && row["ActivityId"].ToString() != "")
                {
                    model.ActivityId = int.Parse(row["ActivityId"].ToString());
                }
                if (row["RuleId"] != null && row["RuleId"].ToString() != "")
                {
                    model.RuleId = int.Parse(row["RuleId"].ToString());
                }
                if (row["BuyCategoryId"] != null && row["BuyCategoryId"].ToString() != "")
                {
                    model.BuyCategoryId = int.Parse(row["BuyCategoryId"].ToString());
                }
                if (row["BuyCategoryName"] != null)
                {
                    model.BuyCategoryName = row["BuyCategoryName"].ToString();
                }
                if (row["BuyProductId"] != null && row["BuyProductId"].ToString() != "")
                {
                    model.BuyProductId = long.Parse(row["BuyProductId"].ToString());
                }
                if (row["BuyProductName"] != null)
                {
                    model.BuyProductName = row["BuyProductName"].ToString();
                }
                if (row["BuySKU"] != null)
                {
                    model.BuySKU = row["BuySKU"].ToString();
                }
                if (row["BuyCount"] != null && row["BuyCount"].ToString() != "")
                {
                    model.BuyCount = int.Parse(row["BuyCount"].ToString());
                }
                if (row["CpRuleId"] != null && row["CpRuleId"].ToString() != "")
                {
                    model.CpRuleId = int.Parse(row["CpRuleId"].ToString());
                }
                if (row["CpRuleName"] != null)
                {
                    model.CpRuleName = row["CpRuleName"].ToString();
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["ProductName"] != null)
                {
                    model.ProductName = row["ProductName"].ToString();
                }
                if (row["SKU"] != null)
                {
                    model.SKU = row["SKU"].ToString();
                }
                if (row["SalePrice"] != null && row["SalePrice"].ToString() != "")
                {
                    model.SalePrice = decimal.Parse(row["SalePrice"].ToString());
                }
                if (row["LimitPrice"] != null && row["LimitPrice"].ToString() != "")
                {
                    model.LimitPrice = decimal.Parse(row["LimitPrice"].ToString());
                }
                if (row["LimitMaxPrice"] != null && row["LimitMaxPrice"].ToString() != "")
                {
                    model.LimitMaxPrice = decimal.Parse(row["LimitMaxPrice"].ToString());
                }
                if (row["Count"] != null && row["Count"].ToString() != "")
                {
                    model.Count = int.Parse(row["Count"].ToString());
                }
                if (row["MaxCount"] != null && row["MaxCount"].ToString() != "")
                {
                    model.MaxCount = int.Parse(row["MaxCount"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["StartDate"] != null && row["StartDate"].ToString() != "")
                {
                    model.StartDate = DateTime.Parse(row["StartDate"].ToString());
                }
                if (row["EndDate"] != null && row["EndDate"].ToString() != "")
                {
                    model.EndDate = DateTime.Parse(row["EndDate"].ToString());
                }
                if (row["CreatedUserId"] != null && row["CreatedUserId"].ToString() != "")
                {
                    model.CreatedUserId = int.Parse(row["CreatedUserId"].ToString());
                }
                if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                }
                if (row["SupplierId"] != null && row["SupplierId"].ToString() != "")
                {
                    model.SupplierId = int.Parse(row["SupplierId"].ToString());
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
            strSql.Append("select ActivityId,RuleId,BuyCategoryId,BuyCategoryName,BuyProductId,BuyProductName,BuySKU,BuyCount,CpRuleId,CpRuleName,ProductId,ProductName,SKU,SalePrice,LimitPrice,LimitMaxPrice,Count,MaxCount,Status,StartDate,EndDate,CreatedUserId,CreatedDate,SupplierId ");
            strSql.Append(" FROM Shop_ActivityInfo ");
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
            strSql.Append(" ActivityId,RuleId,BuyCategoryId,BuyCategoryName,BuyProductId,BuyProductName,BuySKU,BuyCount,CpRuleId,CpRuleName,ProductId,ProductName,SKU,SalePrice,LimitPrice,LimitMaxPrice,Count,MaxCount,Status,StartDate,EndDate,CreatedUserId,CreatedDate,SupplierId ");
            strSql.Append(" FROM Shop_ActivityInfo ");
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
            strSql.Append("select count(1) FROM Shop_ActivityInfo ");
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
                strSql.Append("order by T.ActivityId desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_ActivityInfo T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        #endregion  BasicMethod
		#region  ExtensionMethod
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetActInfos(long ProductId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append("  * ");
            strSql.Append(" FROM Shop_ActivityInfo ");
            strSql.Append(" where ");
            strSql.Append(" order by   ActivityId  desc ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long ProductId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_ActivityInfo");
            strSql.Append(" where ProductId=@ProductId");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.Int,4)
			};
            parameters[0].Value = ProductId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }
		#endregion  ExtensionMethod
	}
}

