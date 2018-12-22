/**  版本信息模板在安装目录下，可自行修改。
* ActivityDetail.cs
*
* 功 能： N/A
* 类 名： ActivityDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/12/25 19:04:16   N/A    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.WeChat.IDAL.Activity;
using YSWL.DBUtility;//Please add references
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Activity
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from WeChat_ActivityDetail");
			strSql.Append(" where DetailId=?DetailId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?DetailId", MySqlDbType.Int64)
			};
			parameters[0].Value = DetailId;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(YSWL.WeChat.Model.Activity.ActivityDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into WeChat_ActivityDetail(");
			strSql.Append("UserName,ActivityId,ActivityName,CreateDate,Remark)");
			strSql.Append(" values (");
			strSql.Append("?UserName,?ActivityId,?ActivityName,?CreateDate,?Remark)");
			strSql.Append(";select last_insert_id()");
			MySqlParameter[] parameters = {
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ActivityId", MySqlDbType.Int32,4),
					new MySqlParameter("?ActivityName", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreateDate", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1)};
			parameters[0].Value = model.UserName;
			parameters[1].Value = model.ActivityId;
			parameters[2].Value = model.ActivityName;
			parameters[3].Value = model.CreateDate;
			parameters[4].Value = model.Remark;

			object obj = DbHelperMySQL.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(YSWL.WeChat.Model.Activity.ActivityDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update WeChat_ActivityDetail set ");
			strSql.Append("UserName=?UserName,");
			strSql.Append("ActivityId=?ActivityId,");
			strSql.Append("ActivityName=?ActivityName,");
			strSql.Append("CreateDate=?CreateDate,");
			strSql.Append("Remark=?Remark");
			strSql.Append(" where DetailId=?DetailId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ActivityId", MySqlDbType.Int32,4),
					new MySqlParameter("?ActivityName", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreateDate", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1),
					new MySqlParameter("?DetailId", MySqlDbType.Int64,8)};
			parameters[0].Value = model.UserName;
			parameters[1].Value = model.ActivityId;
			parameters[2].Value = model.ActivityName;
			parameters[3].Value = model.CreateDate;
			parameters[4].Value = model.Remark;
			parameters[5].Value = model.DetailId;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
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
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_ActivityDetail ");
			strSql.Append(" where DetailId=?DetailId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?DetailId", MySqlDbType.Int64)
			};
			parameters[0].Value = DetailId;

			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string DetailIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_ActivityDetail ");
			strSql.Append(" where DetailId in ("+DetailIdlist + ")  ");
			int rows=DbHelperMySQL.ExecuteSql(strSql.ToString());
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
		public YSWL.WeChat.Model.Activity.ActivityDetail GetModel(long DetailId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select DetailId,UserName,ActivityId,ActivityName,CreateDate,Remark from WeChat_ActivityDetail ");
            strSql.Append(" where DetailId=?DetailId LIMIT 1 ");
			MySqlParameter[] parameters = {
					new MySqlParameter("?DetailId", MySqlDbType.Int64)
			};
			parameters[0].Value = DetailId;

			YSWL.WeChat.Model.Activity.ActivityDetail model=new YSWL.WeChat.Model.Activity.ActivityDetail();
			DataSet ds=DbHelperMySQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
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
		public YSWL.WeChat.Model.Activity.ActivityDetail DataRowToModel(DataRow row)
		{
			YSWL.WeChat.Model.Activity.ActivityDetail model=new YSWL.WeChat.Model.Activity.ActivityDetail();
			if (row != null)
			{
				if(row["DetailId"]!=null && row["DetailId"].ToString()!="")
				{
					model.DetailId=long.Parse(row["DetailId"].ToString());
				}
				if(row["UserName"]!=null)
				{
					model.UserName=row["UserName"].ToString();
				}
				if(row["ActivityId"]!=null && row["ActivityId"].ToString()!="")
				{
					model.ActivityId=int.Parse(row["ActivityId"].ToString());
				}
				if(row["ActivityName"]!=null)
				{
					model.ActivityName=row["ActivityName"].ToString();
				}
				if(row["CreateDate"]!=null && row["CreateDate"].ToString()!="")
				{
					model.CreateDate=DateTime.Parse(row["CreateDate"].ToString());
				}
				if(row["Remark"]!=null)
				{
					model.Remark=row["Remark"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select DetailId,UserName,ActivityId,ActivityName,CreateDate,Remark ");
			strSql.Append(" FROM WeChat_ActivityDetail ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperMySQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			
			strSql.Append(" DetailId,UserName,ActivityId,ActivityName,CreateDate,Remark ");
			strSql.Append(" FROM WeChat_ActivityDetail ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
            if (Top > 0)
            {
                strSql.Append(" LIMIT " + Top.ToString());
            }
			return DbHelperMySQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM WeChat_ActivityDetail ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperMySQL.GetSingle(strSql.ToString());
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
            strSql.Append("SELECT T.* from WeChat_ActivityDetail T ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            if (!string.IsNullOrWhiteSpace(orderby.Trim()))
            {
                strSql.Append(" order by T." + orderby);
            }
            else
            {
                strSql.Append(" order by T.DetailId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
		}




		#endregion  BasicMethod
		#region  ExtensionMethod
        public int GetEachCount(int activityId, string userName)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("SELECT Count(1)  FROM WeChat_ActivityDetail WHERE ActivityId=?ActivityId and UserName=?UserName and  datediff( day, CreateDate, now())<1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ActivityId", MySqlDbType.Int32,4),
                    new MySqlParameter("?UserName", MySqlDbType.VarChar,200)
			};
            parameters[0].Value = activityId;
            parameters[1].Value = userName;
            object obj = DbHelperMySQL.GetSingle(strSql.ToString(), parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
        }


        public int GetDayTotal(int activityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(1)  FROM WeChat_ActivityDetail WHERE ActivityId=?ActivityId   and  datediff( day, CreateDate, now())<1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ActivityId", MySqlDbType.Int32,4)
                     
			};
            parameters[0].Value = activityId;
            object obj = DbHelperMySQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        
		#endregion  ExtensionMethod
	}
}

