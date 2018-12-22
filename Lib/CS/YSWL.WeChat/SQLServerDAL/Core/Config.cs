/**  版本信息模板在安装目录下，可自行修改。
* Config.cs
*
* 功 能： N/A
* 类 名： Config
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/11/21 18:25:39   N/A    初版
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
	/// 数据访问类:Config
	/// </summary>
	public partial class Config:IConfig
	{
		public Config()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("ID", "WeChat_Config"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from WeChat_Config");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.WeChat.Model.Core.Config model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into WeChat_Config(");
			strSql.Append("KeyName,Value,Description,TargetId,UserType)");
			strSql.Append(" values (");
			strSql.Append("@KeyName,@Value,@Description,@TargetId,@UserType)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@KeyName", SqlDbType.NVarChar,50),
					new SqlParameter("@Value", SqlDbType.NVarChar,-1),
					new SqlParameter("@Description", SqlDbType.NVarChar,200),
					new SqlParameter("@TargetId", SqlDbType.Int,4),
					new SqlParameter("@UserType", SqlDbType.Char,2)};
			parameters[0].Value = model.KeyName;
			parameters[1].Value = model.Value;
			parameters[2].Value = model.Description;
			parameters[3].Value = model.TargetId;
			parameters[4].Value = model.UserType;

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
		public bool Update(YSWL.WeChat.Model.Core.Config model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update WeChat_Config set ");
			strSql.Append("KeyName=@KeyName,");
			strSql.Append("Value=@Value,");
			strSql.Append("Description=@Description,");
			strSql.Append("TargetId=@TargetId,");
			strSql.Append("UserType=@UserType");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@KeyName", SqlDbType.NVarChar,50),
					new SqlParameter("@Value", SqlDbType.NVarChar,-1),
					new SqlParameter("@Description", SqlDbType.NVarChar,200),
					new SqlParameter("@TargetId", SqlDbType.Int,4),
					new SqlParameter("@UserType", SqlDbType.Char,2),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = model.KeyName;
			parameters[1].Value = model.Value;
			parameters[2].Value = model.Description;
			parameters[3].Value = model.TargetId;
			parameters[4].Value = model.UserType;
			parameters[5].Value = model.ID;

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
		public bool Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_Config ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

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
		public bool DeleteList(string IDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_Config ");
			strSql.Append(" where ID in ("+IDlist + ")  ");
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
		public YSWL.WeChat.Model.Core.Config GetModel(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,KeyName,Value,Description,TargetId,UserType from WeChat_Config ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			YSWL.WeChat.Model.Core.Config model=new YSWL.WeChat.Model.Core.Config();
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
		public YSWL.WeChat.Model.Core.Config DataRowToModel(DataRow row)
		{
			YSWL.WeChat.Model.Core.Config model=new YSWL.WeChat.Model.Core.Config();
			if (row != null)
			{
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				if(row["KeyName"]!=null)
				{
					model.KeyName=row["KeyName"].ToString();
				}
				if(row["Value"]!=null)
				{
					model.Value=row["Value"].ToString();
				}
				if(row["Description"]!=null)
				{
					model.Description=row["Description"].ToString();
				}
				if(row["TargetId"]!=null && row["TargetId"].ToString()!="")
				{
					model.TargetId=int.Parse(row["TargetId"].ToString());
				}
				if(row["UserType"]!=null)
				{
					model.UserType=row["UserType"].ToString();
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
			strSql.Append("select ID,KeyName,Value,Description,TargetId,UserType ");
			strSql.Append(" FROM WeChat_Config ");
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
			strSql.Append(" ID,KeyName,Value,Description,TargetId,UserType ");
			strSql.Append(" FROM WeChat_Config ");
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
			strSql.Append("select count(1) FROM WeChat_Config ");
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
				strSql.Append("order by T.ID desc");
			}
			strSql.Append(")AS Row, T.*  from WeChat_Config T ");
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
       public bool Exists(string key, int TargetId, string UserType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_Config");
            strSql.Append(" where KeyName=@KeyName and TargetId=@TargetId and UserType=@UserType");
            SqlParameter[] parameters = {
					new SqlParameter("@KeyName", SqlDbType.NVarChar,50),
					new SqlParameter("@TargetId", SqlDbType.Int,4),
					new SqlParameter("@UserType", SqlDbType.Char,2)};
            parameters[0].Value = key;
            parameters[1].Value = TargetId;
            parameters[2].Value = UserType;
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

       public bool Exists(string key, string value)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append("select count(1) from WeChat_Config");
           strSql.Append(" where KeyName=@KeyName and Value=@Value");
           SqlParameter[] parameters = {
					new SqlParameter("@KeyName", SqlDbType.NVarChar,50),
					new SqlParameter("@Value", SqlDbType.NVarChar,-1)
                                       };
           parameters[0].Value = key;
           parameters[1].Value = value;
           return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
       }

       public bool Delete(string key, string value)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append(" delete WeChat_Config");
           strSql.Append(" where KeyName=@KeyName and Value=@Value");
           SqlParameter[] parameters = {
					new SqlParameter("@KeyName", SqlDbType.NVarChar,50),
					new SqlParameter("@Value", SqlDbType.NVarChar,-1)
                                       };
           parameters[0].Value = key;
           parameters[1].Value = value;
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

       public bool UpdateEx(YSWL.WeChat.Model.Core.Config model)
       {
            StringBuilder strSql=new StringBuilder();
			strSql.Append("update WeChat_Config set ");
			strSql.Append("Value=@Value,");
			strSql.Append("Description=@Description");
            strSql.Append(" where KeyName=@KeyName and  TargetId=@TargetId and  UserType=@UserType");
			SqlParameter[] parameters = {
					new SqlParameter("@KeyName", SqlDbType.NVarChar,50),
					new SqlParameter("@Value", SqlDbType.NVarChar,-1),
					new SqlParameter("@Description", SqlDbType.NVarChar,200),
					new SqlParameter("@TargetId", SqlDbType.Int,4),
					new SqlParameter("@UserType", SqlDbType.Char,2)
                                        };
			parameters[0].Value = model.KeyName;
			parameters[1].Value = model.Value;
			parameters[2].Value = model.Description;
			parameters[3].Value = model.TargetId;
			parameters[4].Value = model.UserType;

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

    
		#endregion  ExtensionMethod
	}
}

