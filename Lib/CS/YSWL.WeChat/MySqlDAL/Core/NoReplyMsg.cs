﻿/**  版本信息模板在安装目录下，可自行修改。
* NoReplyMsg.cs
*
* 功 能： N/A
* 类 名： NoReplyMsg
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/2/23 17:18:18   N/A    初版
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
	/// 数据访问类:NoReplyMsg
	/// </summary>
	public partial class NoReplyMsg:INoReplyMsg
	{
		public NoReplyMsg()
		{}

		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long MsgId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from WeChat_NoReplyMsg");
			strSql.Append(" where MsgId=?MsgId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?MsgId", MySqlDbType.Int64)
			};
			parameters[0].Value = MsgId;

			return DbHelperMySQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(YSWL.WeChat.Model.Core.NoReplyMsg model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into WeChat_NoReplyMsg(");
			strSql.Append("OpenId,UserName,MsgType,CreateTime,Description,Location_X,Location_Y,Scale,PicUrl,Title,Url,Event,EventKey,Status)");
			strSql.Append(" values (");
			strSql.Append("?OpenId,?UserName,?MsgType,?CreateTime,?Description,?Location_X,?Location_Y,?Scale,?PicUrl,?Title,?Url,?Event,?EventKey,?Status)");
			strSql.Append(";select last_insert_id()");
			MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?Location_X", MySqlDbType.VarChar,50),
					new MySqlParameter("?Location_Y", MySqlDbType.VarChar,50),
					new MySqlParameter("?Scale", MySqlDbType.Int32,4),
					new MySqlParameter("?PicUrl", MySqlDbType.VarChar,200),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?Url", MySqlDbType.VarChar,200),
					new MySqlParameter("?Event", MySqlDbType.VarChar,50),
					new MySqlParameter("?EventKey", MySqlDbType.VarChar,200),
					new MySqlParameter("?Status", MySqlDbType.Int32,4)};
			parameters[0].Value = model.OpenId;
			parameters[1].Value = model.UserName;
			parameters[2].Value = model.MsgType;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.Description;
			parameters[5].Value = model.Location_X;
			parameters[6].Value = model.Location_Y;
			parameters[7].Value = model.Scale;
			parameters[8].Value = model.PicUrl;
			parameters[9].Value = model.Title;
			parameters[10].Value = model.Url;
			parameters[11].Value = model.Event;
			parameters[12].Value = model.EventKey;
			parameters[13].Value = model.Status;

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
		public bool Update(YSWL.WeChat.Model.Core.NoReplyMsg model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update WeChat_NoReplyMsg set ");
			strSql.Append("OpenId=?OpenId,");
			strSql.Append("UserName=?UserName,");
			strSql.Append("MsgType=?MsgType,");
			strSql.Append("CreateTime=?CreateTime,");
			strSql.Append("Description=?Description,");
			strSql.Append("Location_X=?Location_X,");
			strSql.Append("Location_Y=?Location_Y,");
			strSql.Append("Scale=?Scale,");
			strSql.Append("PicUrl=?PicUrl,");
			strSql.Append("Title=?Title,");
			strSql.Append("Url=?Url,");
			strSql.Append("Event=?Event,");
			strSql.Append("EventKey=?EventKey,");
			strSql.Append("Status=?Status");
			strSql.Append(" where MsgId=?MsgId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?Location_X", MySqlDbType.VarChar,50),
					new MySqlParameter("?Location_Y", MySqlDbType.VarChar,50),
					new MySqlParameter("?Scale", MySqlDbType.Int32,4),
					new MySqlParameter("?PicUrl", MySqlDbType.VarChar,200),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?Url", MySqlDbType.VarChar,200),
					new MySqlParameter("?Event", MySqlDbType.VarChar,50),
					new MySqlParameter("?EventKey", MySqlDbType.VarChar,200),
					new MySqlParameter("?Status", MySqlDbType.Int32,4),
					new MySqlParameter("?MsgId", MySqlDbType.Int64,8)};
			parameters[0].Value = model.OpenId;
			parameters[1].Value = model.UserName;
			parameters[2].Value = model.MsgType;
			parameters[3].Value = model.CreateTime;
			parameters[4].Value = model.Description;
			parameters[5].Value = model.Location_X;
			parameters[6].Value = model.Location_Y;
			parameters[7].Value = model.Scale;
			parameters[8].Value = model.PicUrl;
			parameters[9].Value = model.Title;
			parameters[10].Value = model.Url;
			parameters[11].Value = model.Event;
			parameters[12].Value = model.EventKey;
			parameters[13].Value = model.Status;
			parameters[14].Value = model.MsgId;

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
		public bool Delete(long MsgId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_NoReplyMsg ");
			strSql.Append(" where MsgId=?MsgId");
			MySqlParameter[] parameters = {
					new MySqlParameter("?MsgId", MySqlDbType.Int64)
			};
			parameters[0].Value = MsgId;

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
		public bool DeleteList(string MsgIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from WeChat_NoReplyMsg ");
			strSql.Append(" where MsgId in ("+MsgIdlist + ")  ");
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
		public YSWL.WeChat.Model.Core.NoReplyMsg GetModel(long MsgId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select MsgId,OpenId,UserName,MsgType,CreateTime,Description,Location_X,Location_Y,Scale,PicUrl,Title,Url,Event,EventKey,Status from WeChat_NoReplyMsg ");
            strSql.Append(" where MsgId=?MsgId LIMIT 1 ");
			MySqlParameter[] parameters = {
					new MySqlParameter("?MsgId", MySqlDbType.Int64)
			};
			parameters[0].Value = MsgId;

			YSWL.WeChat.Model.Core.NoReplyMsg model=new YSWL.WeChat.Model.Core.NoReplyMsg();
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
		public YSWL.WeChat.Model.Core.NoReplyMsg DataRowToModel(DataRow row)
		{
			YSWL.WeChat.Model.Core.NoReplyMsg model=new YSWL.WeChat.Model.Core.NoReplyMsg();
			if (row != null)
			{
				if(row["MsgId"]!=null && row["MsgId"].ToString()!="")
				{
					model.MsgId=long.Parse(row["MsgId"].ToString());
				}
				if(row["OpenId"]!=null)
				{
					model.OpenId=row["OpenId"].ToString();
				}
				if(row["UserName"]!=null)
				{
					model.UserName=row["UserName"].ToString();
				}
				if(row["MsgType"]!=null)
				{
					model.MsgType=row["MsgType"].ToString();
				}
				if(row["CreateTime"]!=null && row["CreateTime"].ToString()!="")
				{
					model.CreateTime=DateTime.Parse(row["CreateTime"].ToString());
				}
				if(row["Description"]!=null)
				{
					model.Description=row["Description"].ToString();
				}
				if(row["Location_X"]!=null)
				{
					model.Location_X=row["Location_X"].ToString();
				}
				if(row["Location_Y"]!=null)
				{
					model.Location_Y=row["Location_Y"].ToString();
				}
				if(row["Scale"]!=null && row["Scale"].ToString()!="")
				{
					model.Scale=int.Parse(row["Scale"].ToString());
				}
				if(row["PicUrl"]!=null)
				{
					model.PicUrl=row["PicUrl"].ToString();
				}
				if(row["Title"]!=null)
				{
					model.Title=row["Title"].ToString();
				}
				if(row["Url"]!=null)
				{
					model.Url=row["Url"].ToString();
				}
				if(row["Event"]!=null)
				{
					model.Event=row["Event"].ToString();
				}
				if(row["EventKey"]!=null)
				{
					model.EventKey=row["EventKey"].ToString();
				}
				if(row["Status"]!=null && row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
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
			strSql.Append("select MsgId,OpenId,UserName,MsgType,CreateTime,Description,Location_X,Location_Y,Scale,PicUrl,Title,Url,Event,EventKey,Status ");
			strSql.Append(" FROM WeChat_NoReplyMsg ");
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
			
			strSql.Append(" MsgId,OpenId,UserName,MsgType,CreateTime,Description,Location_X,Location_Y,Scale,PicUrl,Title,Url,Event,EventKey,Status ");
			strSql.Append(" FROM WeChat_NoReplyMsg ");
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
			strSql.Append("select count(1) FROM WeChat_NoReplyMsg ");
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
            strSql.Append("SELECT T.* from WeChat_NoReplyMsg T ");
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
                strSql.Append(" order by T.MsgId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
		}




		#endregion  BasicMethod

		#region  ExtensionMethod
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateStatus(int msgId, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_NoReplyMsg set ");
            strSql.Append("Status=?Status");
            strSql.Append(" where MsgId=?MsgId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Status", MySqlDbType.Int32,4),
					new MySqlParameter("?MsgId", MySqlDbType.Int64,8)};
            parameters[0].Value = status;
            parameters[1].Value = msgId;

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

        public long AddMsg(YSWL.WeChat.Model.Core.RequestMsg model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_NoReplyMsg(");
            strSql.Append("OpenId,UserName,MsgType,CreateTime,Description,Location_X,Location_Y,Scale,PicUrl,Title,Url,Event,EventKey,Status)");
            strSql.Append(" values (");
            strSql.Append("?OpenId,?UserName,?MsgType,?CreateTime,?Description,?Location_X,?Location_Y,?Scale,?PicUrl,?Title,?Url,?Event,?EventKey,?Status)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?Location_X", MySqlDbType.VarChar,50),
					new MySqlParameter("?Location_Y", MySqlDbType.VarChar,50),
					new MySqlParameter("?Scale", MySqlDbType.Int32,4),
					new MySqlParameter("?PicUrl", MySqlDbType.VarChar,200),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?Url", MySqlDbType.VarChar,200),
					new MySqlParameter("?Event", MySqlDbType.VarChar,50),
					new MySqlParameter("?EventKey", MySqlDbType.VarChar,200),
					new MySqlParameter("?Status", MySqlDbType.Int32,4)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.UserName;
            parameters[2].Value = model.MsgType;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.Location_X;
            parameters[6].Value = model.Location_Y;
            parameters[7].Value = model.Scale;
            parameters[8].Value = model.PicUrl;
            parameters[9].Value = model.Title;
            parameters[10].Value = model.Url;
            parameters[11].Value = model.Event;
            parameters[12].Value = model.EventKey;
            parameters[13].Value = 0;

            object obj = DbHelperMySQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }
        public DataSet GetList(int top, int status, string userName, string startdate, string enddate, string keyword, string filedOrder)
        {
            StringBuilder strWhere = new StringBuilder();
            if (status > -1)
            {
                strWhere.AppendFormat("Status={0}", status);
            }
            //用户
            if (!String.IsNullOrWhiteSpace(userName))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat("UserName='{0}'", Common.InjectionFilter.SqlFilter(userName));
            }
            if (!String.IsNullOrWhiteSpace(startdate) && Common.PageValidate.IsDateTime(startdate))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat("  CreateTime >='" + Common.InjectionFilter.SqlFilter(startdate) + "' ");
            }
            if (!String.IsNullOrWhiteSpace(enddate) && Common.PageValidate.IsDateTime(enddate))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat("  CreateTime< DATE_ADD('{0}',INTERVAL 1 DAY)", enddate);
            }
            //关键字
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat(" Description like '%{0}%' ", Common.InjectionFilter.SqlFilter(keyword));
            }
            return GetList(top, strWhere.ToString(), filedOrder);
        }
		#endregion  ExtensionMethod
    }
}
