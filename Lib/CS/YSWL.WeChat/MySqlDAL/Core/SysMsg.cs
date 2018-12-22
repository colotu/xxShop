/**
* SysMsg.cs
*
* 功 能： N/A
* 类 名： SysMsg
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/8/2 14:55:58   N/A    初版
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
using System.Collections.Generic;//Please add references
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Core
{
	/// <summary>
	/// 数据访问类:SysMsg
	/// </summary>
	public partial class SysMsg:ISysMsg
	{
		public SysMsg()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("SysMsgId", "WeChat_SysMsg");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int SysMsgId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_SysMsg");
            strSql.Append(" where SysMsgId=?SysMsgId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?SysMsgId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = SysMsgId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Core.SysMsg model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_SysMsg(");
            strSql.Append("OpenId,MsgType,SysType,CreateTime,Title,PicUrl,Url,Description,MusicUrl,HQMusicUrl)");
            strSql.Append(" values (");
            strSql.Append("?OpenId,?MsgType,?SysType,?CreateTime,?Title,?PicUrl,?Url,?Description,?MusicUrl,?HQMusicUrl)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?SysType", MySqlDbType.Int32,4),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?PicUrl", MySqlDbType.VarChar,200),
					new MySqlParameter("?Url", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?MusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?HQMusicUrl", MySqlDbType.VarChar,300)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.MsgType;
            parameters[2].Value = model.SysType;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.PicUrl;
            parameters[6].Value = model.Url;
            parameters[7].Value = model.Description;
            parameters[8].Value = model.MusicUrl;
            parameters[9].Value = model.HQMusicUrl;

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
        public bool Update(YSWL.WeChat.Model.Core.SysMsg model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_SysMsg set ");
            strSql.Append("OpenId=?OpenId,");
            strSql.Append("MsgType=?MsgType,");
            strSql.Append("SysType=?SysType,");
            strSql.Append("CreateTime=?CreateTime,");
            strSql.Append("Title=?Title,");
            strSql.Append("PicUrl=?PicUrl,");
            strSql.Append("Url=?Url,");
            strSql.Append("Description=?Description,");
            strSql.Append("MusicUrl=?MusicUrl,");
            strSql.Append("HQMusicUrl=?HQMusicUrl");
            strSql.Append(" where SysMsgId=?SysMsgId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?SysType", MySqlDbType.Int32,4),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?PicUrl", MySqlDbType.VarChar,200),
					new MySqlParameter("?Url", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?MusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?HQMusicUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?SysMsgId", MySqlDbType.Int32,4)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.MsgType;
            parameters[2].Value = model.SysType;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.PicUrl;
            parameters[6].Value = model.Url;
            parameters[7].Value = model.Description;
            parameters[8].Value = model.MusicUrl;
            parameters[9].Value = model.HQMusicUrl;
            parameters[10].Value = model.SysMsgId;

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
        public bool Delete(int SysMsgId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_SysMsg ");
            strSql.Append(" where SysMsgId=?SysMsgId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?SysMsgId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = SysMsgId;

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
        public bool DeleteList(string SysMsgIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_SysMsg ");
            strSql.Append(" where SysMsgId in (" + SysMsgIdlist + ")  ");
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
        public YSWL.WeChat.Model.Core.SysMsg GetModel(int SysMsgId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SysMsgId,OpenId,MsgType,SysType,CreateTime,Title,PicUrl,Url,Description,MusicUrl,HQMusicUrl from WeChat_SysMsg ");
            strSql.Append(" where SysMsgId=?SysMsgId LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?SysMsgId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = SysMsgId;

            YSWL.WeChat.Model.Core.SysMsg model = new YSWL.WeChat.Model.Core.SysMsg();
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
        public YSWL.WeChat.Model.Core.SysMsg DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.SysMsg model = new YSWL.WeChat.Model.Core.SysMsg();
            if (row != null)
            {
                if (row["SysMsgId"] != null && row["SysMsgId"].ToString() != "")
                {
                    model.SysMsgId = int.Parse(row["SysMsgId"].ToString());
                }
                if (row["OpenId"] != null)
                {
                    model.OpenId = row["OpenId"].ToString();
                }
                if (row["MsgType"] != null)
                {
                    model.MsgType = row["MsgType"].ToString();
                }
                if (row["SysType"] != null && row["SysType"].ToString() != "")
                {
                    model.SysType = int.Parse(row["SysType"].ToString());
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["Title"] != null)
                {
                    model.Title = row["Title"].ToString();
                }
                if (row["PicUrl"] != null)
                {
                    model.PicUrl = row["PicUrl"].ToString();
                }
                if (row["Url"] != null)
                {
                    model.Url = row["Url"].ToString();
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
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SysMsgId,OpenId,MsgType,SysType,CreateTime,Title,PicUrl,Url,Description,MusicUrl,HQMusicUrl ");
            strSql.Append(" FROM WeChat_SysMsg ");
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
            
            strSql.Append(" SysMsgId,OpenId,MsgType,SysType,CreateTime,Title,PicUrl,Url,Description,MusicUrl,HQMusicUrl ");
            strSql.Append(" FROM WeChat_SysMsg ");
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
            strSql.Append("select count(1) FROM WeChat_SysMsg ");
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
            strSql.Append("SELECT T.* from WeChat_SysMsg T ");
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
                strSql.Append(" order by T.SysMsgId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }

   


        #endregion  BasicMethod
		#region  ExtensionMethod

        public DataSet GetSysMsgs(int type, string openId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select   * from WeChat_SysMsg ");
            strSql.Append(" where SysType=?SysType  and OpenId=?OpenId ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?SysType", MySqlDbType.Int32,4),
                    new MySqlParameter("?OpenId", MySqlDbType.VarChar,200)
			};
            parameters[0].Value = type;
            parameters[1].Value = openId;

            YSWL.WeChat.Model.Core.SysMsg model = new YSWL.WeChat.Model.Core.SysMsg();
            return DbHelperMySQL.Query(strSql.ToString(), parameters);
          
	    }

        public bool AddEx(YSWL.WeChat.Model.Core.SysMsg msg)
        {
            using (MySqlConnection connection = DbHelperMySQL.GetConnection)
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int sysId = Common.Globals.SafeInt(DbHelperMySQL.GetSingle4Trans(AddMsgInfo(msg), transaction), 0);
                        if (msg.MsgType == "news" && msg.MsgItems != null && msg.MsgItems.Count > 0)
                        {
                            foreach (var item in msg.MsgItems)
                            {
                             int itemId=Common.Globals.SafeInt( DbHelperMySQL.GetSingle4Trans(AddMsgItem(item), transaction),0);
                             DbHelperMySQL.GetSingle4Trans(AddSysItem(sysId, itemId, 2), transaction);
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


       
        private CommandInfo AddMsgInfo(YSWL.WeChat.Model.Core.SysMsg model)
        {
                       StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_SysMsg(");
            strSql.Append("OpenId,MsgType,CreateTime,Title,Description,SysType)");
            strSql.Append(" values (");
            strSql.Append("?OpenId,?MsgType,?CreateTime,?Title,?Description,?SysType)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?MsgType", MySqlDbType.VarChar,50),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?SysType", MySqlDbType.Int32,4)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.MsgType;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.SysType;
                    return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }

         private CommandInfo AddMsgItem(YSWL.WeChat.Model.Core.MsgItem model)
        {
                     StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_MsgItem(");
            strSql.Append("Title,PicUrl,Url,Description)");
            strSql.Append(" values (");
            strSql.Append("?Title,?PicUrl,?Url,?Description)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?PicUrl", MySqlDbType.VarChar,200),
					new MySqlParameter("?Url", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.PicUrl;
            parameters[2].Value = model.Url;
            parameters[3].Value = model.Description;
         
                    return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }

         private CommandInfo AddSysItem(int systId,int itemId,int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_PostMsgItem(");
            strSql.Append("PostMsgId,ItemId,Type)");
            strSql.Append(" values (");
            strSql.Append("?PostMsgId,?ItemId,?Type)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PostMsgId", MySqlDbType.Int32,4),
					new MySqlParameter("?ItemId", MySqlDbType.Int32,4),
					new MySqlParameter("?Type", MySqlDbType.Int32,4)};
            parameters[0].Value = systId;
            parameters[1].Value = itemId;
            parameters[2].Value = type;
                    return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }

            //级联删除
         public bool DeleteEx(int msgId,int type)
         {
             List<CommandInfo> sqllist = new List<CommandInfo>();
             StringBuilder strSql = new StringBuilder();
             if (type == 0)
             {
                 strSql.Append("Delete WeChat_PostMsg  Where PostMsgId =?PostMsgId");
                 MySqlParameter[] parameters = {
					new MySqlParameter("?PostMsgId", MySqlDbType.Int32,4)
                                         };
                 parameters[0].Value = msgId;
                 CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                 sqllist.Add(cmd);
             }
             if (type == 1)
             {
                 strSql.Append("Delete WeChat_CustomerMsg  Where MsgId =?MsgId");
                 MySqlParameter[] parameters = {
					new MySqlParameter("?MsgId", MySqlDbType.Int32,4)};
                 parameters[0].Value = msgId;
                 CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                 sqllist.Add(cmd);
             }
             if (type == 2)
             {
                 strSql.Append("Delete WeChat_SysMsg  Where SysMsgId =?SysMsgId");
                 MySqlParameter[] parameters = {
					new MySqlParameter("?SysMsgId", MySqlDbType.Int32,4)};
                 parameters[0].Value = msgId;
                 CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                 sqllist.Add(cmd);
             }

             //删除项表
             StringBuilder strSql1 = new StringBuilder();
             strSql1.Append("delete  WeChat_MsgItem  where EXISTS");
             strSql1.Append(" (select * from WeChat_PostMsgItem P where P.PostMsgId=?SysMsgId and P.Type=?Type  and P.ItemId=WeChat_MsgItem.ItemId) ");
             MySqlParameter[] parameters1 = {
					new MySqlParameter("?SysMsgId", MySqlDbType.Int32,4),
                    new MySqlParameter("?Type", MySqlDbType.Int32,4)
                                         };
             parameters1[0].Value = msgId;
             parameters1[0].Value = type;
             CommandInfo cmd1 = new CommandInfo(strSql1.ToString(), parameters1);
             sqllist.Add(cmd1);

             //删除中间表
             StringBuilder strSql2 = new StringBuilder();
             strSql2.Append("delete WeChat_PostMsgItem where PostMsgId=?SysMsgId and Type=?Type");
             MySqlParameter[] parameters2 = {
					new MySqlParameter("?SysMsgId", MySqlDbType.Int32,4),
                    new MySqlParameter("?Type", MySqlDbType.Int32,4)
                                         };
             parameters2[0].Value = msgId;
             parameters2[0].Value = type;
             CommandInfo cmd2 = new CommandInfo(strSql2.ToString(), parameters2);
             sqllist.Add(cmd2);
             return DbHelperMySQL.ExecuteSqlTran(sqllist) > 0 ? true : false;
         }


         public bool DeleteEx(string msgType, int type,string openId)
         {
             StringBuilder strSql = new StringBuilder();
             strSql.Append("delete from WeChat_SysMsg ");
             if (msgType == "text")
             {
                 strSql.AppendFormat(" where openId='{0}' and SysType={1} ", Common.InjectionFilter.SqlFilter(openId), type);
             }
             if (msgType == "news")
             {
                 strSql.AppendFormat(" where openId='{0}' and SysType={1} and MsgType<>'news'", Common.InjectionFilter.SqlFilter(openId), type);
             }
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
        
	    #endregion  ExtensionMethod
	}
}

