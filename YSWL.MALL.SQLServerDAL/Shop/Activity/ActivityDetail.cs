/**  版本信息模板在安装目录下，可自行修改。
* ActivityDetail.cs
*
* 功 能： N/A
* 类 名： ActivityDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/12/11 11:02:05   N/A    初版
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
	/// 数据访问类:ActivityDetail
	/// </summary>
	public partial class ActivityDetail:IActivityDetail
	{
		public ActivityDetail()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long DetailId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_ActivityDetail");
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
        public long Add(YSWL.MALL.Model.Shop.Activity.ActivityDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_ActivityDetail(");
            strSql.Append("UserId,UserName,ActivityId,RuleId,CreateDate,OrderId,OrderCode,Remark,SupplierId)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@UserName,@ActivityId,@RuleId,@CreateDate,@OrderId,@OrderCode,@Remark,@SupplierId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@ActivityId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@Remark", SqlDbType.NVarChar,-1),
					new SqlParameter("@SupplierId", SqlDbType.Int,4)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.UserName;
            parameters[2].Value = model.ActivityId;
            parameters[3].Value = model.RuleId;
            parameters[4].Value = model.CreateDate;
            parameters[5].Value = model.OrderId;
            parameters[6].Value = model.OrderCode;
            parameters[7].Value = model.Remark;
            parameters[8].Value = model.SupplierId;

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
        public bool Update(YSWL.MALL.Model.Shop.Activity.ActivityDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_ActivityDetail set ");
            strSql.Append("UserId=@UserId,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("ActivityId=@ActivityId,");
            strSql.Append("RuleId=@RuleId,");
            strSql.Append("CreateDate=@CreateDate,");
            strSql.Append("OrderId=@OrderId,");
            strSql.Append("OrderCode=@OrderCode,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("SupplierId=@SupplierId");
            strSql.Append(" where DetailId=@DetailId");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@ActivityId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@Remark", SqlDbType.NVarChar,-1),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@DetailId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.UserName;
            parameters[2].Value = model.ActivityId;
            parameters[3].Value = model.RuleId;
            parameters[4].Value = model.CreateDate;
            parameters[5].Value = model.OrderId;
            parameters[6].Value = model.OrderCode;
            parameters[7].Value = model.Remark;
            parameters[8].Value = model.SupplierId;
            parameters[9].Value = model.DetailId;

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
            strSql.Append("delete from Shop_ActivityDetail ");
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
            strSql.Append("delete from Shop_ActivityDetail ");
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
        public YSWL.MALL.Model.Shop.Activity.ActivityDetail GetModel(long DetailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 DetailId,UserId,UserName,ActivityId,RuleId,CreateDate,OrderId,OrderCode,Remark,SupplierId from Shop_ActivityDetail ");
            strSql.Append(" where DetailId=@DetailId");
            SqlParameter[] parameters = {
					new SqlParameter("@DetailId", SqlDbType.BigInt)
			};
            parameters[0].Value = DetailId;

            YSWL.MALL.Model.Shop.Activity.ActivityDetail model = new YSWL.MALL.Model.Shop.Activity.ActivityDetail();
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
        public YSWL.MALL.Model.Shop.Activity.ActivityDetail DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Activity.ActivityDetail model = new YSWL.MALL.Model.Shop.Activity.ActivityDetail();
            if (row != null)
            {
                if (row["DetailId"] != null && row["DetailId"].ToString() != "")
                {
                    model.DetailId = long.Parse(row["DetailId"].ToString());
                }
                if (row["UserId"] != null && row["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["ActivityId"] != null && row["ActivityId"].ToString() != "")
                {
                    model.ActivityId = int.Parse(row["ActivityId"].ToString());
                }
                if (row["RuleId"] != null && row["RuleId"].ToString() != "")
                {
                    model.RuleId = int.Parse(row["RuleId"].ToString());
                }
                if (row["CreateDate"] != null && row["CreateDate"].ToString() != "")
                {
                    model.CreateDate = DateTime.Parse(row["CreateDate"].ToString());
                }
                if (row["OrderId"] != null && row["OrderId"].ToString() != "")
                {
                    model.OrderId = long.Parse(row["OrderId"].ToString());
                }
                if (row["OrderCode"] != null)
                {
                    model.OrderCode = row["OrderCode"].ToString();
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
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
            strSql.Append("select DetailId,UserId,UserName,ActivityId,RuleId,CreateDate,OrderId,OrderCode,Remark,SupplierId ");
            strSql.Append(" FROM Shop_ActivityDetail ");
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
            strSql.Append(" DetailId,UserId,UserName,ActivityId,RuleId,CreateDate,OrderId,OrderCode,Remark,SupplierId ");
            strSql.Append(" FROM Shop_ActivityDetail ");
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
            strSql.Append("select count(1) FROM Shop_ActivityDetail ");
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
            strSql.Append(")AS Row, T.*  from Shop_ActivityDetail T ");
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

		#endregion  ExtensionMethod
	}
}

