/**  版本信息模板在安装目录下，可自行修改。
* Location.cs
*
* 功 能： N/A
* 类 名： Location
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/3/24 14:00:02   N/A    初版
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
using YSWL.WeChat.IDAL.Core;
using YSWL.DBUtility;//Please add references
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Core
{
	/// <summary>
	/// 数据访问类:Location
	/// </summary>
	public partial class Location:ILocation
	{
		public Location()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long LocationId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from WeChat_Location");
			strSql.Append(" where LocationId=?LocationId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?LocationId", MySqlDbType.Int64)
			};
			parameters[0].Value = LocationId;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(YSWL.WeChat.Model.Core.Location model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into WeChat_Location(");
			strSql.Append("OpenId,UserName,Latitude,Longitude,Precision,CreateTime)");
			strSql.Append(" values (");
			strSql.Append("?OpenId,?UserName,?Latitude,?Longitude,?Precision,?CreateTime)");
			strSql.Append(";select last_insert_id()");
			MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Latitude", MySqlDbType.Decimal,9),
					new MySqlParameter("?Longitude", MySqlDbType.Decimal,9),
					new MySqlParameter("?Precision", MySqlDbType.Decimal,9),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime)};
			parameters[0].Value = model.OpenId;
			parameters[1].Value = model.UserName;
			parameters[2].Value = model.Latitude;
			parameters[3].Value = model.Longitude;
			parameters[4].Value = model.Precision;
			parameters[5].Value = model.CreateTime;

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
		public bool Update(YSWL.WeChat.Model.Core.Location model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update WeChat_Location set ");
			strSql.Append("OpenId=?OpenId,");
			strSql.Append("UserName=?UserName,");
			strSql.Append("Latitude=?Latitude,");
			strSql.Append("Longitude=?Longitude,");
			strSql.Append("Precision=?Precision,");
			strSql.Append("CreateTime=?CreateTime");
			strSql.Append(" where LocationId=?LocationId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Latitude", MySqlDbType.Decimal,9),
					new MySqlParameter("?Longitude", MySqlDbType.Decimal,9),
					new MySqlParameter("?Precision", MySqlDbType.Decimal,9),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?LocationId", MySqlDbType.Int64,8)};
			parameters[0].Value = model.OpenId;
			parameters[1].Value = model.UserName;
			parameters[2].Value = model.Latitude;
			parameters[3].Value = model.Longitude;
			parameters[4].Value = model.Precision;
			parameters[5].Value = model.CreateTime;
			parameters[6].Value = model.LocationId;

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
		public bool Delete(long LocationId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_Location ");
			strSql.Append(" where LocationId=?LocationId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?LocationId", MySqlDbType.Int64)
			};
			parameters[0].Value = LocationId;

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
		public bool DeleteList(string LocationIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_Location ");
			strSql.Append(" where LocationId in ("+LocationIdlist + ")  ");
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
		public YSWL.WeChat.Model.Core.Location GetModel(long LocationId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select LocationId,OpenId,UserName,Latitude,Longitude,Precision,CreateTime from WeChat_Location ");
            strSql.Append(" where LocationId=?LocationId LIMIT 1 ");
			MySqlParameter[] parameters = {
					new MySqlParameter("?LocationId", MySqlDbType.Int64)
			};
			parameters[0].Value = LocationId;

			YSWL.WeChat.Model.Core.Location model=new YSWL.WeChat.Model.Core.Location();
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
		public YSWL.WeChat.Model.Core.Location DataRowToModel(DataRow row)
		{
			YSWL.WeChat.Model.Core.Location model=new YSWL.WeChat.Model.Core.Location();
			if (row != null)
			{
				if(row["LocationId"]!=null && row["LocationId"].ToString()!="")
				{
					model.LocationId=long.Parse(row["LocationId"].ToString());
				}
				if(row["OpenId"]!=null)
				{
					model.OpenId=row["OpenId"].ToString();
				}
				if(row["UserName"]!=null)
				{
					model.UserName=row["UserName"].ToString();
				}
				if(row["Latitude"]!=null && row["Latitude"].ToString()!="")
				{
					model.Latitude=decimal.Parse(row["Latitude"].ToString());
				}
				if(row["Longitude"]!=null && row["Longitude"].ToString()!="")
				{
					model.Longitude=decimal.Parse(row["Longitude"].ToString());
				}
				if(row["Precision"]!=null && row["Precision"].ToString()!="")
				{
					model.Precision=decimal.Parse(row["Precision"].ToString());
				}
				if(row["CreateTime"]!=null && row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
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
			strSql.Append("select LocationId,OpenId,UserName,Latitude,Longitude,Precision,CreateTime ");
			strSql.Append(" FROM WeChat_Location ");
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
			
			strSql.Append(" LocationId,OpenId,UserName,Latitude,Longitude,Precision,CreateTime ");
			strSql.Append(" FROM WeChat_Location ");
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
			strSql.Append("select count(1) FROM WeChat_Location ");
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
            strSql.Append("SELECT T.* from WeChat_Location T ");
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
                strSql.Append(" order by T.LocationId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
		}




		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

