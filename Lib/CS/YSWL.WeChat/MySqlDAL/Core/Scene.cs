/**  版本信息模板在安装目录下，可自行修改。
* Scene.cs
*
* 功 能： N/A
* 类 名： Scene
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/2/20 12:32:06   N/A    初版
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
	/// 数据访问类:Scene
	/// </summary>
	public partial class Scene:IScene
	{
		public Scene()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("SceneId", "WeChat_Scene");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int SceneId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_Scene");
            strSql.Append(" where SceneId=?SceneId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?SceneId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = SceneId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Core.Scene model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_Scene(");
            strSql.Append("OpenId,Name,CreateTime,Remark,ImageUrl)");
            strSql.Append(" values (");
            strSql.Append("?OpenId,?Name,?CreateTime,?Remark,?ImageUrl)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?Name", MySqlDbType.VarChar,200),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,500),
					new MySqlParameter("?ImageUrl", MySqlDbType.VarChar,500)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.ImageUrl;

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
        public bool Update(YSWL.WeChat.Model.Core.Scene model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_Scene set ");
            strSql.Append("OpenId=?OpenId,");
            strSql.Append("Name=?Name,");
            strSql.Append("CreateTime=?CreateTime,");
            strSql.Append("Remark=?Remark,");
            strSql.Append("ImageUrl=?ImageUrl");
            strSql.Append(" where SceneId=?SceneId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?Name", MySqlDbType.VarChar,200),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,500),
					new MySqlParameter("?ImageUrl", MySqlDbType.VarChar,500),
					new MySqlParameter("?SceneId", MySqlDbType.Int32,4)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.Remark;
            parameters[4].Value = model.ImageUrl;
            parameters[5].Value = model.SceneId;

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
        public bool Delete(int SceneId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_Scene ");
            strSql.Append(" where SceneId=?SceneId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?SceneId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = SceneId;

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
        public bool DeleteList(string SceneIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_Scene ");
            strSql.Append(" where SceneId in (" + SceneIdlist + ")  ");
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
        public YSWL.WeChat.Model.Core.Scene GetModel(int SceneId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SceneId,OpenId,Name,CreateTime,Remark,ImageUrl from WeChat_Scene ");
            strSql.Append(" where SceneId=?SceneId LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?SceneId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = SceneId;

            YSWL.WeChat.Model.Core.Scene model = new YSWL.WeChat.Model.Core.Scene();
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
        public YSWL.WeChat.Model.Core.Scene DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.Scene model = new YSWL.WeChat.Model.Core.Scene();
            if (row != null)
            {
                if (row["SceneId"] != null && row["SceneId"].ToString() != "")
                {
                    model.SceneId = int.Parse(row["SceneId"].ToString());
                }
                if (row["OpenId"] != null)
                {
                    model.OpenId = row["OpenId"].ToString();
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["ImageUrl"] != null)
                {
                    model.ImageUrl = row["ImageUrl"].ToString();
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
            strSql.Append("select SceneId,OpenId,Name,CreateTime,Remark,ImageUrl ");
            strSql.Append(" FROM WeChat_Scene ");
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
            
            strSql.Append(" SceneId,OpenId,Name,CreateTime,Remark,ImageUrl ");
            strSql.Append(" FROM WeChat_Scene ");
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
            strSql.Append("select count(1) FROM WeChat_Scene ");
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
            strSql.Append("SELECT T.* from WeChat_Scene T ");
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
                strSql.Append(" order by T.SceneId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }

   


        #endregion  BasicMethod
        #region  ExtensionMethod
        public YSWL.WeChat.Model.Core.Scene GetSceneInfo(int userId)
        {
            throw new NotImplementedException();
        }
        #endregion  ExtensionMethod
    }
}

