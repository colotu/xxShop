/**  版本信息模板在安装目录下，可自行修改。
* TaskMsg.cs
*
* 功 能： N/A
* 类 名： TaskMsg
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/7 17:58:09   N/A    初版
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
using YSWL.WeChat.IDAL.Push;
using YSWL.DBUtility;//Please add references
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Push
{
	/// <summary>
	/// 数据访问类:TaskMsg
	/// </summary>
	public partial class TaskMsg:ITaskMsg
	{
		public TaskMsg()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("TaskId", "WeChat_TaskMsg");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TaskId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_TaskMsg");
            strSql.Append(" where TaskId=?TaskId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?TaskId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = TaskId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Push.TaskMsg model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_TaskMsg(");
            strSql.Append("OpenId,GroupId,UserName,MsgType,CreatedDate,Title,Description,MediaId,VoiceUrl,MusicUrl,HQMusicUrl,ArticleCount,PublishDate)");
            strSql.Append(" values (");
            strSql.Append("?OpenId,?GroupId,?UserName,?MsgType,?CreatedDate,?Title,?Description,?MediaId,?VoiceUrl,?MusicUrl,?HQMusicUrl,?ArticleCount,?PublishDate)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?GroupId", MySqlDbType.Int32,4),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreatedDate", MySqlDbType.DateTime),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?MediaId", MySqlDbType.VarChar,300),
					new MySqlParameter("?VoiceUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?MusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?HQMusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?ArticleCount", MySqlDbType.Int32,4),
					new MySqlParameter("?PublishDate", MySqlDbType.DateTime)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.GroupId;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.MsgType;
            parameters[4].Value = model.CreatedDate;
            parameters[5].Value = model.Title;
            parameters[6].Value = model.Description;
            parameters[7].Value = model.MediaId;
            parameters[8].Value = model.VoiceUrl;
            parameters[9].Value = model.MusicUrl;
            parameters[10].Value = model.HQMusicUrl;
            parameters[11].Value = model.ArticleCount;
            parameters[12].Value = model.PublishDate;

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
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.WeChat.Model.Push.TaskMsg model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_TaskMsg set ");
            strSql.Append("OpenId=?OpenId,");
            strSql.Append("GroupId=?GroupId,");
            strSql.Append("UserName=?UserName,");
            strSql.Append("MsgType=?MsgType,");
            strSql.Append("CreatedDate=?CreatedDate,");
            strSql.Append("Title=?Title,");
            strSql.Append("Description=?Description,");
            strSql.Append("MediaId=?MediaId,");
            strSql.Append("VoiceUrl=?VoiceUrl,");
            strSql.Append("MusicUrl=?MusicUrl,");
            strSql.Append("HQMusicUrl=?HQMusicUrl,");
            strSql.Append("ArticleCount=?ArticleCount,");
            strSql.Append("PublishDate=?PublishDate");
            strSql.Append(" where TaskId=?TaskId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?GroupId", MySqlDbType.Int32,4),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreatedDate", MySqlDbType.DateTime),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?MediaId", MySqlDbType.VarChar,300),
					new MySqlParameter("?VoiceUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?MusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?HQMusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?ArticleCount", MySqlDbType.Int32,4),
					new MySqlParameter("?PublishDate", MySqlDbType.DateTime),
					new MySqlParameter("?TaskId", MySqlDbType.Int32,4)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.GroupId;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.MsgType;
            parameters[4].Value = model.CreatedDate;
            parameters[5].Value = model.Title;
            parameters[6].Value = model.Description;
            parameters[7].Value = model.MediaId;
            parameters[8].Value = model.VoiceUrl;
            parameters[9].Value = model.MusicUrl;
            parameters[10].Value = model.HQMusicUrl;
            parameters[11].Value = model.ArticleCount;
            parameters[12].Value = model.PublishDate;
            parameters[13].Value = model.TaskId;

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
        public bool Delete(int TaskId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_TaskMsg ");
            strSql.Append(" where TaskId=?TaskId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?TaskId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = TaskId;

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
        public bool DeleteList(string TaskIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_TaskMsg ");
            strSql.Append(" where TaskId in (" + TaskIdlist + ")  ");
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
        public YSWL.WeChat.Model.Push.TaskMsg GetModel(int TaskId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TaskId,OpenId,GroupId,UserName,MsgType,CreatedDate,Title,Description,MediaId,VoiceUrl,MusicUrl,HQMusicUrl,ArticleCount,PublishDate from WeChat_TaskMsg ");
            strSql.Append(" where TaskId=?TaskId LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?TaskId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = TaskId;

            YSWL.WeChat.Model.Push.TaskMsg model = new YSWL.WeChat.Model.Push.TaskMsg();
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
        public YSWL.WeChat.Model.Push.TaskMsg DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Push.TaskMsg model = new YSWL.WeChat.Model.Push.TaskMsg();
            if (row != null)
            {
                if (row["TaskId"] != null && row["TaskId"].ToString() != "")
                {
                    model.TaskId = int.Parse(row["TaskId"].ToString());
                }
                if (row["OpenId"] != null)
                {
                    model.OpenId = row["OpenId"].ToString();
                }
                if (row["GroupId"] != null && row["GroupId"].ToString() != "")
                {
                    model.GroupId = int.Parse(row["GroupId"].ToString());
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["MsgType"] != null)
                {
                    model.MsgType = row["MsgType"].ToString();
                }
                if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                }
                if (row["Title"] != null)
                {
                    model.Title = row["Title"].ToString();
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["MediaId"] != null)
                {
                    model.MediaId = row["MediaId"].ToString();
                }
                if (row["VoiceUrl"] != null)
                {
                    model.VoiceUrl = row["VoiceUrl"].ToString();
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
                if (row["PublishDate"] != null && row["PublishDate"].ToString() != "")
                {
                    model.PublishDate = DateTime.Parse(row["PublishDate"].ToString());
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
            strSql.Append("select TaskId,OpenId,GroupId,UserName,MsgType,CreatedDate,Title,Description,MediaId,VoiceUrl,MusicUrl,HQMusicUrl,ArticleCount,PublishDate ");
            strSql.Append(" FROM WeChat_TaskMsg ");
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
            
            strSql.Append(" TaskId,OpenId,GroupId,UserName,MsgType,CreatedDate,Title,Description,MediaId,VoiceUrl,MusicUrl,HQMusicUrl,ArticleCount,PublishDate ");
            strSql.Append(" FROM WeChat_TaskMsg ");
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
            strSql.Append("select count(1) FROM WeChat_TaskMsg ");
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
            strSql.Append("SELECT T.* from WeChat_TaskMsg T ");
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
                strSql.Append(" order by T.TaskId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }




        #endregion  BasicMethod
		#region  ExtensionMethod

         public DataSet GetMsgList(string openid, string datetime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM WeChat_TaskMsg ");
            strSql.AppendFormat("  where OpenId='{0}' and PublishDate BETWEEN '{1}' AND GETDATE()", Common.InjectionFilter.SqlFilter(openid), datetime);
            return DbHelperMySQL.Query(strSql.ToString());    
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
                 strWhere.AppendFormat("  CreatedDate >='" + Common.InjectionFilter.SqlFilter(startdate) + "' ");
             }
             if (!String.IsNullOrWhiteSpace(enddate) && Common.PageValidate.IsDateTime(enddate))
             {
                 if (strWhere.Length > 1)
                 {
                     strWhere.Append(" and  ");
                 }
                 strWhere.AppendFormat("  CreatedDate< DATE_ADD('{0}',INTERVAL 1 DAY)", enddate);
             }
             return GetList(top, strWhere.ToString(), filedOrder);
         }

		#endregion  ExtensionMethod
    }
}

