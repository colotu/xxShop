/**
* KeyValue.cs
*
* 功 能： N/A
* 类 名： KeyValue
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/7/29 15:35:26   N/A    初版
*
* Copyright (c) 2012-2013 YSWL Corporation. All rights reserved.
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
	/// 数据访问类:KeyValue
	/// </summary>
	public partial class KeyValue:IKeyValue
	{
		public KeyValue()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperMySQL.GetMaxID("ValueId", "WeChat_KeyValue"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ValueId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from WeChat_KeyValue");
			strSql.Append(" where ValueId=?ValueId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?ValueId", MySqlDbType.Int32,4)
			};
			parameters[0].Value = ValueId;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.WeChat.Model.Core.KeyValue model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into WeChat_KeyValue(");
			strSql.Append("RuleId,Value,MatchType)");
			strSql.Append(" values (");
			strSql.Append("?RuleId,?Value,?MatchType)");
			strSql.Append(";select last_insert_id()");
			MySqlParameter[] parameters = {
					new MySqlParameter("?RuleId", MySqlDbType.Int32,4),
					new MySqlParameter("?Value", MySqlDbType.VarChar,-1),
					new MySqlParameter("?MatchType", MySqlDbType.Int32,4)};
			parameters[0].Value = model.RuleId;
			parameters[1].Value = model.Value;
			parameters[2].Value = model.MatchType;

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
		public bool Update(YSWL.WeChat.Model.Core.KeyValue model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update WeChat_KeyValue set ");
			strSql.Append("RuleId=?RuleId,");
			strSql.Append("Value=?Value,");
			strSql.Append("MatchType=?MatchType");
			strSql.Append(" where ValueId=?ValueId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?RuleId", MySqlDbType.Int32,4),
					new MySqlParameter("?Value", MySqlDbType.VarChar,-1),
					new MySqlParameter("?MatchType", MySqlDbType.Int32,4),
					new MySqlParameter("?ValueId", MySqlDbType.Int32,4)};
			parameters[0].Value = model.RuleId;
			parameters[1].Value = model.Value;
			parameters[2].Value = model.MatchType;
			parameters[3].Value = model.ValueId;

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
		public bool Delete(int ValueId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_KeyValue ");
			strSql.Append(" where ValueId=?ValueId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?ValueId", MySqlDbType.Int32,4)
			};
			parameters[0].Value = ValueId;

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
		public bool DeleteList(string ValueIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_KeyValue ");
			strSql.Append(" where ValueId in ("+ValueIdlist + ")  ");
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
		public YSWL.WeChat.Model.Core.KeyValue GetModel(int ValueId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ValueId,RuleId,Value,MatchType from WeChat_KeyValue ");
            strSql.Append(" where ValueId=?ValueId LIMIT 1 ");
			MySqlParameter[] parameters = {
					new MySqlParameter("?ValueId", MySqlDbType.Int32,4)
			};
			parameters[0].Value = ValueId;

			YSWL.WeChat.Model.Core.KeyValue model=new YSWL.WeChat.Model.Core.KeyValue();
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
		public YSWL.WeChat.Model.Core.KeyValue DataRowToModel(DataRow row)
		{
			YSWL.WeChat.Model.Core.KeyValue model=new YSWL.WeChat.Model.Core.KeyValue();
			if (row != null)
			{
				if(row["ValueId"]!=null && row["ValueId"].ToString()!="")
				{
					model.ValueId=int.Parse(row["ValueId"].ToString());
				}
				if(row["RuleId"]!=null && row["RuleId"].ToString()!="")
				{
					model.RuleId=int.Parse(row["RuleId"].ToString());
				}
				if(row["Value"]!=null)
				{
					model.Value=row["Value"].ToString();
				}
				if(row["MatchType"]!=null && row["MatchType"].ToString()!="")
				{
					model.MatchType=int.Parse(row["MatchType"].ToString());
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
			strSql.Append("select ValueId,RuleId,Value,MatchType ");
			strSql.Append(" FROM WeChat_KeyValue ");
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
			
			strSql.Append(" ValueId,RuleId,Value,MatchType ");
			strSql.Append(" FROM WeChat_KeyValue ");
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
			strSql.Append("select count(1) FROM WeChat_KeyValue ");
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
            strSql.Append("SELECT T.* from WeChat_KeyValue T ");
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
                strSql.Append(" order by T.ValueId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
		}

	


		#endregion  BasicMethod
		#region  ExtensionMethod

        public bool UpdateType(int valueId, int type)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_KeyValue set ");
            strSql.Append("MatchType=?MatchType");
            strSql.Append(" where ValueId=?ValueId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?MatchType", MySqlDbType.Int32,4),
					new MySqlParameter("?ValueId", MySqlDbType.Int32,4)};
            parameters[0].Value = type;
            parameters[1].Value = valueId;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string  value,string openId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_KeyValue");
            strSql.Append(" where Value=?Value");
            MySqlParameter[] parameters = {
						new MySqlParameter("?Value", MySqlDbType.VarChar,-1),
                        new MySqlParameter("?OpenId", MySqlDbType.VarChar,200)
			};
            parameters[0].Value = value;
            parameters[1].Value = openId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        public DataSet GetValueList(string openId)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("  select *  from WeChat_KeyValue V join WeChat_KeyRule R  on V.RuleId=R.RuleId and R.OpenId=?OpenId ");
            MySqlParameter[] parameters = {
						new MySqlParameter("?OpenId", MySqlDbType.VarChar,200)
			};
            parameters[0].Value = openId;
            return DbHelperMySQL.Query(strSql.ToString(),parameters);
        }
	    #endregion  ExtensionMethod
	}
}

