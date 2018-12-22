/**
* Action.cs
*
* 功 能： N/A
* 类 名： Action
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/7/29 15:35:10   N/A    初版
*
* Copyright (c) 2012-2013 YSWL Corporation. All rights reserved.
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
using YSWL.WeChat.IDAL.Core;
using YSWL.DBUtility;//Please add references
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Core
{
	/// <summary>
	/// 数据访问类:Action
	/// </summary>
	public partial class Action:IAction
	{
		public Action()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("ActionId", "WeChat_Action"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ActionId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from WeChat_Action");
			strSql.Append(" where ActionId=?ActionId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?ActionId", MySqlDbType.Int32,4)
			};
			parameters[0].Value = ActionId;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.WeChat.Model.Core.Action model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into WeChat_Action(");
			strSql.Append("Name,Remark)");
			strSql.Append(" values (");
			strSql.Append("?Name,?Remark)");
			strSql.Append(";select last_insert_id()");
			MySqlParameter[] parameters = {
					new MySqlParameter("?Name", MySqlDbType.VarChar,200),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.Remark;

			object obj = DbHelperMySQL.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(YSWL.WeChat.Model.Core.Action model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update WeChat_Action set ");
			strSql.Append("Name=?Name,");
			strSql.Append("Remark=?Remark");
			strSql.Append(" where ActionId=?ActionId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?Name", MySqlDbType.VarChar,200),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1),
					new MySqlParameter("?ActionId", MySqlDbType.Int32,4)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.Remark;
			parameters[2].Value = model.ActionId;

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
		public bool Delete(int ActionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_Action ");
			strSql.Append(" where ActionId=?ActionId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?ActionId", MySqlDbType.Int32,4)
			};
			parameters[0].Value = ActionId;

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
		public bool DeleteList(string ActionIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_Action ");
			strSql.Append(" where ActionId in ("+ActionIdlist + ")  ");
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
		public YSWL.WeChat.Model.Core.Action GetModel(int ActionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ActionId,Name,Remark from WeChat_Action ");
            strSql.Append(" where ActionId=?ActionId LIMIT 1 ");
			MySqlParameter[] parameters = {
					new MySqlParameter("?ActionId", MySqlDbType.Int32,4)
			};
			parameters[0].Value = ActionId;

			YSWL.WeChat.Model.Core.Action model=new YSWL.WeChat.Model.Core.Action();
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
		public YSWL.WeChat.Model.Core.Action DataRowToModel(DataRow row)
		{
			YSWL.WeChat.Model.Core.Action model=new YSWL.WeChat.Model.Core.Action();
			if (row != null)
			{
				if(row["ActionId"]!=null && row["ActionId"].ToString()!="")
				{
					model.ActionId=int.Parse(row["ActionId"].ToString());
				}
				if(row["Name"]!=null)
				{
					model.Name=row["Name"].ToString();
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
			strSql.Append("select ActionId,Name,Remark ");
			strSql.Append(" FROM WeChat_Action ");
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
			
			strSql.Append(" ActionId,Name,Remark ");
			strSql.Append(" FROM WeChat_Action ");
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
			strSql.Append("select count(1) FROM WeChat_Action ");
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
            strSql.Append("SELECT T.* from WeChat_Action T ");
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
                strSql.Append(" order by T.ActionId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
		}



		#endregion  BasicMethod
		#region  ExtensionMethod

	    public bool DeleteListEx(string ActionIdlist)
	    {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_Action ");
            strSql.Append(" where ActionId in (" + ActionIdlist + ")  ");
            CommandInfo cmd = new CommandInfo(strSql.ToString(), null);
            sqllist.Add(cmd);

            //删除指令
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("delete from WeChat_Command ");
            strSql1.Append(" where ActionId in (" + ActionIdlist + ")  ");
            CommandInfo cmd1 = new CommandInfo(strSql1.ToString(), null);
            sqllist.Add(cmd1);
          

            return DbHelperMySQL.ExecuteSqlTran(sqllist) > 0 ? true : false;
	    }

	    #endregion  ExtensionMethod
	}
}

