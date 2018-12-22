/**
* Group.cs
*
* 功 能： N/A
* 类 名： Group
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/7/22 17:43:07   N/A    初版
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
using YSWL.DBUtility;
using YSWL.WeChat.IDAL.Core;
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Core
{
	/// <summary>
	/// 数据访问类:Group
	/// </summary>
	public partial class Group:IGroup
	{
		public Group()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("GroupId", "WeChat_Group");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int GroupId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_Group");
            strSql.Append(" where GroupId=?GroupId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?GroupId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = GroupId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Core.Group model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_Group(");
            strSql.Append("OpenId,GroupName,Remark)");
            strSql.Append(" values (");
            strSql.Append("?OpenId,?GroupName,?Remark)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?GroupName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,300)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.GroupName;
            parameters[2].Value = model.Remark;

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
        public bool Update(YSWL.WeChat.Model.Core.Group model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_Group set ");
            strSql.Append("OpenId=?OpenId,");
            strSql.Append("GroupName=?GroupName,");
            strSql.Append("Remark=?Remark");
            strSql.Append(" where GroupId=?GroupId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?GroupName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,300),
					new MySqlParameter("?GroupId", MySqlDbType.Int32,4)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.GroupName;
            parameters[2].Value = model.Remark;
            parameters[3].Value = model.GroupId;

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
        public bool Delete(int GroupId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_Group ");
            strSql.Append(" where GroupId=?GroupId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?GroupId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = GroupId;

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
        public bool DeleteList(string GroupIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_Group ");
            strSql.Append(" where GroupId in (" + GroupIdlist + ")  ");
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
        public YSWL.WeChat.Model.Core.Group GetModel(int GroupId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GroupId,OpenId,GroupName,Remark from WeChat_Group ");
            strSql.Append(" where GroupId=?GroupId LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?GroupId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = GroupId;

            YSWL.WeChat.Model.Core.Group model = new YSWL.WeChat.Model.Core.Group();
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
        public YSWL.WeChat.Model.Core.Group DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.Group model = new YSWL.WeChat.Model.Core.Group();
            if (row != null)
            {
                if (row["GroupId"] != null && row["GroupId"].ToString() != "")
                {
                    model.GroupId = int.Parse(row["GroupId"].ToString());
                }
                if (row["OpenId"] != null)
                {
                    model.OpenId = row["OpenId"].ToString();
                }
                if (row["GroupName"] != null)
                {
                    model.GroupName = row["GroupName"].ToString();
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
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
            strSql.Append("select GroupId,OpenId,GroupName,Remark ");
            strSql.Append(" FROM WeChat_Group ");
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
            
            strSql.Append(" GroupId,OpenId,GroupName,Remark ");
            strSql.Append(" FROM WeChat_Group ");
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
            strSql.Append("select count(1) FROM WeChat_Group ");
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
            strSql.Append("SELECT T.* from WeChat_Group T ");
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
                strSql.Append(" order by T.GroupId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }




        #endregion  BasicMethod
		#region  ExtensionMethod
       public bool Delete(string openId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_Group where OpenId=?OpenId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200)
			};
            parameters[0].Value = openId;
            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(),parameters);
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

