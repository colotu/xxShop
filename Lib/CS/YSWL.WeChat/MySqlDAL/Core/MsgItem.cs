/**
* MsgItem.cs
*
* 功 能： N/A
* 类 名： MsgItem
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/7/22 17:43:09   N/A    初版
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
	/// 数据访问类:MsgItem
	/// </summary>
	public partial class MsgItem:IMsgItem
	{
		public MsgItem()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("ItemId", "WeChat_MsgItem");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ItemId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_MsgItem");
            strSql.Append(" where ItemId=?ItemId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ItemId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = ItemId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Core.MsgItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_MsgItem(");
            strSql.Append("OpenId,Title,PicUrl,Url,Description)");
            strSql.Append(" values (");
            strSql.Append("?OpenId,?Title,?PicUrl,?Url,?Description)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,50),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?PicUrl", MySqlDbType.VarChar,200),
					new MySqlParameter("?Url", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.PicUrl;
            parameters[3].Value = model.Url;
            parameters[4].Value = model.Description;

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
        public bool Update(YSWL.WeChat.Model.Core.MsgItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_MsgItem set ");
            strSql.Append("OpenId=?OpenId,");
            strSql.Append("Title=?Title,");
            strSql.Append("PicUrl=?PicUrl,");
            strSql.Append("Url=?Url,");
            strSql.Append("Description=?Description");
            strSql.Append(" where ItemId=?ItemId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,50),
					new MySqlParameter("?Title", MySqlDbType.VarChar,200),
					new MySqlParameter("?PicUrl", MySqlDbType.VarChar,200),
					new MySqlParameter("?Url", MySqlDbType.VarChar,200),
					new MySqlParameter("?Description", MySqlDbType.VarChar,-1),
					new MySqlParameter("?ItemId", MySqlDbType.Int32,4)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.PicUrl;
            parameters[3].Value = model.Url;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.ItemId;

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
        public bool Delete(int ItemId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_MsgItem ");
            strSql.Append(" where ItemId=?ItemId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ItemId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = ItemId;

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
        public bool DeleteList(string ItemIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_MsgItem ");
            strSql.Append(" where ItemId in (" + ItemIdlist + ")  ");
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
        public YSWL.WeChat.Model.Core.MsgItem GetModel(int ItemId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ItemId,OpenId,Title,PicUrl,Url,Description from WeChat_MsgItem ");
            strSql.Append(" where ItemId=?ItemId LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ItemId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = ItemId;

            YSWL.WeChat.Model.Core.MsgItem model = new YSWL.WeChat.Model.Core.MsgItem();
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
        public YSWL.WeChat.Model.Core.MsgItem DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.MsgItem model = new YSWL.WeChat.Model.Core.MsgItem();
            if (row != null)
            {
                if (row["ItemId"] != null && row["ItemId"].ToString() != "")
                {
                    model.ItemId = int.Parse(row["ItemId"].ToString());
                }
                if (row["OpenId"] != null)
                {
                    model.OpenId = row["OpenId"].ToString();
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
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ItemId,OpenId,Title,PicUrl,Url,Description ");
            strSql.Append(" FROM WeChat_MsgItem ");
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
            
            strSql.Append(" ItemId,OpenId,Title,PicUrl,Url,Description ");
            strSql.Append(" FROM WeChat_MsgItem ");
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
            strSql.Append("select count(1) FROM WeChat_MsgItem ");
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
            strSql.Append("SELECT T.* from WeChat_MsgItem T ");
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
                strSql.Append(" order by T.ItemId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }

      


        #endregion  BasicMethod
		#region  ExtensionMethod
        public DataSet GetItemList(int msgId, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select *  from WeChat_MsgItem M where EXISTS   ");
            strSql.Append(" (select * from WeChat_PostMsgItem P where P.PostMsgId=?PostMsgId and P.Type=?Type  and P.ItemId=M.ItemId) ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PostMsgId", MySqlDbType.VarChar,4),
					new MySqlParameter("?Type", MySqlDbType.VarChar,4)
        };
            parameters[0].Value = msgId;
            parameters[1].Value = type;
            return DbHelperMySQL.Query(strSql.ToString(),parameters);
        }
		#endregion  ExtensionMethod
	}
}

