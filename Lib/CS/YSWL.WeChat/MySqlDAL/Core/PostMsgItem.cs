/**
* PostMsgItem.cs
*
* 功 能： N/A
* 类 名： PostMsgItem
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/7/29 18:14:12   N/A    初版
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
	/// 数据访问类:PostMsgItem
	/// </summary>
	public partial class PostMsgItem:IPostMsgItem
	{
		public PostMsgItem()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("PostMsgId", "WeChat_PostMsgItem");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PostMsgId, int ItemId, int Type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_PostMsgItem");
            strSql.Append(" where PostMsgId=?PostMsgId and ItemId=?ItemId and Type=?Type ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PostMsgId", MySqlDbType.Int32,4),
					new MySqlParameter("?ItemId", MySqlDbType.Int32,4),
					new MySqlParameter("?Type", MySqlDbType.Int32,4)			};
            parameters[0].Value = PostMsgId;
            parameters[1].Value = ItemId;
            parameters[2].Value = Type;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.WeChat.Model.Core.PostMsgItem model)
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
            parameters[0].Value = model.PostMsgId;
            parameters[1].Value = model.ItemId;
            parameters[2].Value = model.Type;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.WeChat.Model.Core.PostMsgItem model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_PostMsgItem set ");
#warning 系统发现缺少更新的字段，请手工确认如此更新是否正确！
            strSql.Append("PostMsgId=?PostMsgId,");
            strSql.Append("ItemId=?ItemId,");
            strSql.Append("Type=?Type");
            strSql.Append(" where PostMsgId=?PostMsgId and ItemId=?ItemId and Type=?Type ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PostMsgId", MySqlDbType.Int32,4),
					new MySqlParameter("?ItemId", MySqlDbType.Int32,4),
					new MySqlParameter("?Type", MySqlDbType.Int32,4)};
            parameters[0].Value = model.PostMsgId;
            parameters[1].Value = model.ItemId;
            parameters[2].Value = model.Type;

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
        public bool Delete(int PostMsgId, int ItemId, int Type)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_PostMsgItem ");
            strSql.Append(" where PostMsgId=?PostMsgId and ItemId=?ItemId and Type=?Type ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PostMsgId", MySqlDbType.Int32,4),
					new MySqlParameter("?ItemId", MySqlDbType.Int32,4),
					new MySqlParameter("?Type", MySqlDbType.Int32,4)			};
            parameters[0].Value = PostMsgId;
            parameters[1].Value = ItemId;
            parameters[2].Value = Type;

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
        /// 得到一个对象实体
        /// </summary>
        public YSWL.WeChat.Model.Core.PostMsgItem GetModel(int PostMsgId, int ItemId, int Type)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PostMsgId,ItemId,Type from WeChat_PostMsgItem ");
            strSql.Append(" where PostMsgId=?PostMsgId and ItemId=?ItemId and Type=?Type  LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?PostMsgId", MySqlDbType.Int32,4),
					new MySqlParameter("?ItemId", MySqlDbType.Int32,4),
					new MySqlParameter("?Type", MySqlDbType.Int32,4)			};
            parameters[0].Value = PostMsgId;
            parameters[1].Value = ItemId;
            parameters[2].Value = Type;

            YSWL.WeChat.Model.Core.PostMsgItem model = new YSWL.WeChat.Model.Core.PostMsgItem();
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
        public YSWL.WeChat.Model.Core.PostMsgItem DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.PostMsgItem model = new YSWL.WeChat.Model.Core.PostMsgItem();
            if (row != null)
            {
                if (row["PostMsgId"] != null && row["PostMsgId"].ToString() != "")
                {
                    model.PostMsgId = int.Parse(row["PostMsgId"].ToString());
                }
                if (row["ItemId"] != null && row["ItemId"].ToString() != "")
                {
                    model.ItemId = int.Parse(row["ItemId"].ToString());
                }
                if (row["Type"] != null && row["Type"].ToString() != "")
                {
                    model.Type = int.Parse(row["Type"].ToString());
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
            strSql.Append("select PostMsgId,ItemId,Type ");
            strSql.Append(" FROM WeChat_PostMsgItem ");
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

            strSql.Append(" PostMsgId,ItemId,Type ");
            strSql.Append(" FROM WeChat_PostMsgItem ");
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
            strSql.Append("select count(1) FROM WeChat_PostMsgItem ");
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
            strSql.Append("SELECT T.* from WeChat_PostMsgItem T ");
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
                strSql.Append(" order by T.Type desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }


        #endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

