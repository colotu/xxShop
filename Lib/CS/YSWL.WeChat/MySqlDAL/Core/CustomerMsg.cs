/**  版本信息模板在安装目录下，可自行修改。
* CustomerMsg.cs
*
* 功 能： N/A
* 类 名： CustomerMsg
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/11/21 20:52:18   N/A    初版
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
using YSWL.DBUtility;
using System.Collections.Generic;//Please add references
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Core
{
	/// <summary>
	/// 数据访问类:CustomerMsg
	/// </summary>
	public partial class CustomerMsg:ICustomerMsg
	{
		public CustomerMsg()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long MsgId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_CustomerMsg");
            strSql.Append(" where MsgId=?MsgId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?MsgId", MySqlDbType.Int64)
			};
            parameters[0].Value = MsgId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.WeChat.Model.Core.CustomerMsg model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_CustomerMsg(");
            strSql.Append("OpenId,MsgType,CreateTime,Title,Description,MusicUrl,HQMusicUrl,ArticleCount)");
            strSql.Append(" values (");
            strSql.Append("?OpenId,?MsgType,?CreateTime,?Title,?Description,?MusicUrl,?HQMusicUrl,?ArticleCount)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?MusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?HQMusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?ArticleCount", MySqlDbType.Int32,4)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.MsgType;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.MusicUrl;
            parameters[6].Value = model.HQMusicUrl;
            parameters[7].Value = model.ArticleCount;

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
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.WeChat.Model.Core.CustomerMsg model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_CustomerMsg set ");
            strSql.Append("OpenId=?OpenId,");
            strSql.Append("MsgType=?MsgType,");
            strSql.Append("CreateTime=?CreateTime,");
            strSql.Append("Title=?Title,");
            strSql.Append("Description=?Description,");
            strSql.Append("MusicUrl=?MusicUrl,");
            strSql.Append("HQMusicUrl=?HQMusicUrl,");
            strSql.Append("ArticleCount=?ArticleCount");
            strSql.Append(" where MsgId=?MsgId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?MusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?HQMusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?ArticleCount", MySqlDbType.Int32,4),
					new MySqlParameter("?MsgId", MySqlDbType.Int64,8)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.MsgType;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.MusicUrl;
            parameters[6].Value = model.HQMusicUrl;
            parameters[7].Value = model.ArticleCount;
            parameters[8].Value = model.MsgId;

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
        /// 删除一条数据
        /// </summary>
        public bool Delete(long MsgId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_CustomerMsg ");
            strSql.Append(" where MsgId=?MsgId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?MsgId", MySqlDbType.Int64)
			};
            parameters[0].Value = MsgId;

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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string MsgIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_CustomerMsg ");
            strSql.Append(" where MsgId in (" + MsgIdlist + ")  ");
            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString());
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
        public YSWL.WeChat.Model.Core.CustomerMsg GetModel(long MsgId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MsgId,OpenId,MsgType,CreateTime,Title,Description,MusicUrl,HQMusicUrl,ArticleCount from WeChat_CustomerMsg ");
            strSql.Append(" where MsgId=?MsgId LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?MsgId", MySqlDbType.Int64)
			};
            parameters[0].Value = MsgId;

            YSWL.WeChat.Model.Core.CustomerMsg model = new YSWL.WeChat.Model.Core.CustomerMsg();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
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
        public YSWL.WeChat.Model.Core.CustomerMsg DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.CustomerMsg model = new YSWL.WeChat.Model.Core.CustomerMsg();
            if (row != null)
            {
                if (row["MsgId"] != null && row["MsgId"].ToString() != "")
                {
                    model.MsgId = long.Parse(row["MsgId"].ToString());
                }
                if (row["OpenId"] != null)
                {
                    model.OpenId = row["OpenId"].ToString();
                }
                if (row["MsgType"] != null)
                {
                    model.MsgType = row["MsgType"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["Title"] != null)
                {
                    model.Title = row["Title"].ToString();
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["MusicUrl"] != null)
                {
                    model.MusicUrl = row["MusicUrl"].ToString();
                }
                if (row["HQMusicUrl"] != null)
                {
                    model.HQMusicUrl = row["HQMusicUrl"].ToString();
                }
                if (row["ArticleCount"] != null && row["ArticleCount"].ToString() != "")
                {
                    model.ArticleCount = int.Parse(row["ArticleCount"].ToString());
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
            strSql.Append("select MsgId,OpenId,MsgType,CreateTime,Title,Description,MusicUrl,HQMusicUrl,ArticleCount ");
            strSql.Append(" FROM WeChat_CustomerMsg ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            
            strSql.Append(" MsgId,OpenId,MsgType,CreateTime,Title,Description,MusicUrl,HQMusicUrl,ArticleCount ");
            strSql.Append(" FROM WeChat_CustomerMsg ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM WeChat_CustomerMsg ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            strSql.Append("SELECT T.* from WeChat_CustomerMsg T ");
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

        public bool DeleteListEx(string ids)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //删除项表
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("delete  WeChat_CustUserMsg  ");
            strSql1.Append(" where MsgId in (" + ids + ")  ");
            CommandInfo cmd1 = new CommandInfo(strSql1.ToString(),null);
            sqllist.Add(cmd1);

            //删除中间表
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete WeChat_CustomerMsg ");
            strSql2.Append(" where MsgId in (" + ids + ")  ");
            CommandInfo cmd2 = new CommandInfo(strSql2.ToString(), null);
            sqllist.Add(cmd2);
            return DbHelperMySQL.ExecuteSqlTran(sqllist) > 0 ? true : false;
        }

        public DataSet GetList(int top, string openId, string startdate, string enddate, string filedOrder)
        {
            StringBuilder strWhere = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(openId))
            {
                strWhere.AppendFormat(" OpenId='{0}'", Common.InjectionFilter.SqlFilter(openId));
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
            return GetList(top, strWhere.ToString(), filedOrder);
        }
		#endregion  ExtensionMethod
    }
}

