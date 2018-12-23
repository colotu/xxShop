﻿/**  版本信息模板在安装目录下，可自行修改。
* LinkLog.cs
*
* 功 能： N/A
* 类 名： LinkLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/9 18:22:16   N/A    初版
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
namespace YSWL.WeChat.SQLServerDAL.Core
{
	/// <summary>
	/// 数据访问类:LinkLog
	/// </summary>
	public partial class LinkLog:ILinkLog
	{
		public LinkLog()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string WeChatLink)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from WeChat_LinkLog");
			strSql.Append(" where WeChatLink=@WeChatLink ");
			SqlParameter[] parameters = {
					new SqlParameter("@WeChatLink", SqlDbType.NVarChar,400)			};
			parameters[0].Value = WeChatLink;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.WeChat.Model.Core.LinkLog model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into WeChat_LinkLog(");
			strSql.Append("WeChatLink,CreatedDate)");
			strSql.Append(" values (");
			strSql.Append("@WeChatLink,@CreatedDate)");
			SqlParameter[] parameters = {
					new SqlParameter("@WeChatLink", SqlDbType.NVarChar,400),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime)};
			parameters[0].Value = model.WeChatLink;
			parameters[1].Value = model.CreatedDate;

			int rows=DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(),parameters);
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
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.WeChat.Model.Core.LinkLog model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update WeChat_LinkLog set ");
			strSql.Append("CreatedDate=@CreatedDate");
			strSql.Append(" where WeChatLink=@WeChatLink ");
			SqlParameter[] parameters = {
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@WeChatLink", SqlDbType.NVarChar,400)};
			parameters[0].Value = model.CreatedDate;
			parameters[1].Value = model.WeChatLink;

			int rows=DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Delete(string WeChatLink)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_LinkLog ");
			strSql.Append(" where WeChatLink=@WeChatLink ");
			SqlParameter[] parameters = {
					new SqlParameter("@WeChatLink", SqlDbType.NVarChar,400)			};
			parameters[0].Value = WeChatLink;

			int rows=DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string WeChatLinklist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_LinkLog ");
			strSql.Append(" where WeChatLink in ("+WeChatLinklist + ")  ");
			int rows=DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
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
		public YSWL.WeChat.Model.Core.LinkLog GetModel(string WeChatLink)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 WeChatLink,CreatedDate from WeChat_LinkLog ");
			strSql.Append(" where WeChatLink=@WeChatLink ");
			SqlParameter[] parameters = {
					new SqlParameter("@WeChatLink", SqlDbType.NVarChar,400)			};
			parameters[0].Value = WeChatLink;

			YSWL.WeChat.Model.Core.LinkLog model=new YSWL.WeChat.Model.Core.LinkLog();
			DataSet ds=DBHelper.DefaultDBHelper.Query(strSql.ToString(),parameters);
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
		public YSWL.WeChat.Model.Core.LinkLog DataRowToModel(DataRow row)
		{
			YSWL.WeChat.Model.Core.LinkLog model=new YSWL.WeChat.Model.Core.LinkLog();
			if (row != null)
			{
				if(row["WeChatLink"]!=null)
				{
					model.WeChatLink=row["WeChatLink"].ToString();
				}
				if(row["CreatedDate"]!=null && row["CreatedDate"].ToString()!="")
				{
					model.CreatedDate=DateTime.Parse(row["CreatedDate"].ToString());
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
			strSql.Append("select WeChatLink,CreatedDate ");
			strSql.Append(" FROM WeChat_LinkLog ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DBHelper.DefaultDBHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" WeChatLink,CreatedDate ");
			strSql.Append(" FROM WeChat_LinkLog ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DBHelper.DefaultDBHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM WeChat_LinkLog ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.WeChatLink desc");
			}
			strSql.Append(")AS Row, T.*  from WeChat_LinkLog T ");
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
