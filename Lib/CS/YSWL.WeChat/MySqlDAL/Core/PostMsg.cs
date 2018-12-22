/**
* PostMsg.cs
*
* 功 能： N/A
* 类 名： PostMsg
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/7/22 17:43:13   N/A    初版
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
using YSWL.DBUtility;
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Core
{
	/// <summary>
	/// 数据访问类:PostMsg
	/// </summary>
	public partial class PostMsg:IPostMsg
	{
		public PostMsg()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long PostMsgId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_PostMsg");
            strSql.Append(" where PostMsgId=?PostMsgId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PostMsgId", MySqlDbType.Int64)
			};
            parameters[0].Value = PostMsgId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.WeChat.Model.Core.PostMsg model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_PostMsg(");
            strSql.Append("RuleId,MsgType,CreateTime,Title,Description,MusicUrl,HQMusicUrl,ArticleCount)");
            strSql.Append(" values (");
            strSql.Append("?RuleId,?MsgType,?CreateTime,?Title,?Description,?MusicUrl,?HQMusicUrl,?ArticleCount)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?RuleId", MySqlDbType.Int32,4),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?MusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?HQMusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?ArticleCount", MySqlDbType.Int32,4)};
            parameters[0].Value = model.RuleId;
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
        public bool Update(YSWL.WeChat.Model.Core.PostMsg model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_PostMsg set ");
            strSql.Append("RuleId=?RuleId,");
            strSql.Append("MsgType=?MsgType,");
            strSql.Append("CreateTime=?CreateTime,");
            strSql.Append("Title=?Title,");
            strSql.Append("Description=?Description,");
            strSql.Append("MusicUrl=?MusicUrl,");
            strSql.Append("HQMusicUrl=?HQMusicUrl,");
            strSql.Append("ArticleCount=?ArticleCount");
            strSql.Append(" where PostMsgId=?PostMsgId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?RuleId", MySqlDbType.Int32,4),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?MusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?HQMusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?ArticleCount", MySqlDbType.Int32,4),
					new MySqlParameter("?PostMsgId", MySqlDbType.Int64,8)};
            parameters[0].Value = model.RuleId;
            parameters[1].Value = model.MsgType;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.MusicUrl;
            parameters[6].Value = model.HQMusicUrl;
            parameters[7].Value = model.ArticleCount;
            parameters[8].Value = model.PostMsgId;

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
        public bool Delete(long PostMsgId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_PostMsg ");
            strSql.Append(" where PostMsgId=?PostMsgId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PostMsgId", MySqlDbType.Int64)
			};
            parameters[0].Value = PostMsgId;

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
        public bool DeleteList(string PostMsgIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_PostMsg ");
            strSql.Append(" where PostMsgId in (" + PostMsgIdlist + ")  ");
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
        public YSWL.WeChat.Model.Core.PostMsg GetModel(long PostMsgId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PostMsgId,RuleId,MsgType,CreateTime,Title,Description,MusicUrl,HQMusicUrl,ArticleCount from WeChat_PostMsg ");
            strSql.Append(" where PostMsgId=?PostMsgId  LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PostMsgId", MySqlDbType.Int64)
			};
            parameters[0].Value = PostMsgId;

            YSWL.WeChat.Model.Core.PostMsg model = new YSWL.WeChat.Model.Core.PostMsg();
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
        public YSWL.WeChat.Model.Core.PostMsg DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.PostMsg model = new YSWL.WeChat.Model.Core.PostMsg();
            if (row != null)
            {
                if (row["PostMsgId"] != null && row["PostMsgId"].ToString() != "")
                {
                    model.PostMsgId = long.Parse(row["PostMsgId"].ToString());
                }
                if (row["RuleId"] != null && row["RuleId"].ToString() != "")
                {
                    model.RuleId = int.Parse(row["RuleId"].ToString());
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
            strSql.Append("select PostMsgId,RuleId,MsgType,CreateTime,Title,Description,MusicUrl,HQMusicUrl,ArticleCount ");
            strSql.Append(" FROM WeChat_PostMsg ");
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
            
            strSql.Append(" PostMsgId,RuleId,MsgType,CreateTime,Title,Description,MusicUrl,HQMusicUrl,ArticleCount ");
            strSql.Append(" FROM WeChat_PostMsg ");
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
            strSql.Append("select count(1) FROM WeChat_PostMsg ");
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
            strSql.Append("SELECT T.* from WeChat_PostMsg T ");
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
                strSql.Append(" order by T.PostMsgId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }

        


        #endregion  BasicMethod
		#region  ExtensionMethod

	    public YSWL.WeChat.Model.Core.PostMsg GetRanMsg(int ruleId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ");
            strSql.Append(" *  from WeChat_PostMsg where RuleId=?RuleId order By RAND() LIMIT 1 ");
            MySqlParameter[] parameters = {
							new MySqlParameter("?RuleId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = ruleId;
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

	    #endregion  ExtensionMethod
	}
}

