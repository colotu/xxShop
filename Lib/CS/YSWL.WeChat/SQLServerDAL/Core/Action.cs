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
namespace YSWL.WeChat.SQLServerDAL.Core
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
		return DBHelper.DefaultDBHelper.GetMaxID("ActionId", "WeChat_Action"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ActionId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from WeChat_Action");
			strSql.Append(" where ActionId=@ActionId");
			SqlParameter[] parameters = {
					new SqlParameter("@ActionId", SqlDbType.Int,4)
			};
			parameters[0].Value = ActionId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
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
			strSql.Append("@Name,@Remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@Remark", SqlDbType.NVarChar,-1)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.Remark;

			object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(),parameters);
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
			strSql.Append("Name=@Name,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where ActionId=@ActionId");
			SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@Remark", SqlDbType.NVarChar,-1),
					new SqlParameter("@ActionId", SqlDbType.Int,4)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.Remark;
			parameters[2].Value = model.ActionId;

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
		public bool Delete(int ActionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_Action ");
			strSql.Append(" where ActionId=@ActionId");
			SqlParameter[] parameters = {
					new SqlParameter("@ActionId", SqlDbType.Int,4)
			};
			parameters[0].Value = ActionId;

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
		public bool DeleteList(string ActionIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_Action ");
			strSql.Append(" where ActionId in ("+ActionIdlist + ")  ");
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
		public YSWL.WeChat.Model.Core.Action GetModel(int ActionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ActionId,Name,Remark from WeChat_Action ");
			strSql.Append(" where ActionId=@ActionId");
			SqlParameter[] parameters = {
					new SqlParameter("@ActionId", SqlDbType.Int,4)
			};
			parameters[0].Value = ActionId;

			YSWL.WeChat.Model.Core.Action model=new YSWL.WeChat.Model.Core.Action();
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
			strSql.Append(" ActionId,Name,Remark ");
			strSql.Append(" FROM WeChat_Action ");
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
			strSql.Append("select count(1) FROM WeChat_Action ");
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
				strSql.Append("order by T.ActionId desc");
			}
			strSql.Append(")AS Row, T.*  from WeChat_Action T ");
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
          

            return DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist) > 0 ? true : false;
	    }

	    #endregion  ExtensionMethod
	}
}

