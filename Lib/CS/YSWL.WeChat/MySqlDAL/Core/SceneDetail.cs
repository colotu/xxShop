/**  版本信息模板在安装目录下，可自行修改。
* SceneDetail.cs
*
* 功 能： N/A
* 类 名： SceneDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/2/20 12:32:25   N/A    初版
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
using YSWL.WeChat.Model.Core;

namespace YSWL.WeChat.MySqlDAL.Core
{
	/// <summary>
	/// 数据访问类:SceneDetail
	/// </summary>
	public partial class SceneDetail:ISceneDetail
	{
		public SceneDetail()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("DetailId", "WeChat_SceneDetail");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int DetailId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_SceneDetail");
            strSql.Append(" where DetailId=?DetailId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?DetailId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = DetailId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Core.SceneDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_SceneDetail(");
            strSql.Append("SceneId,OpenId,UserName,NickName,Sex,City,Province,Country,Language,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("?SceneId,?OpenId,?UserName,?NickName,?Sex,?City,?Province,?Country,?Language,?CreateTime)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?SceneId", MySqlDbType.Int32,4),
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?NickName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Sex", MySqlDbType.Int32,4),
					new MySqlParameter("?City", MySqlDbType.VarChar,200),
					new MySqlParameter("?Province", MySqlDbType.VarChar,200),
					new MySqlParameter("?Country", MySqlDbType.VarChar,200),
					new MySqlParameter("?Language", MySqlDbType.VarChar,200),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime)};
            parameters[0].Value = model.SceneId;
            parameters[1].Value = model.OpenId;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.NickName;
            parameters[4].Value = model.Sex;
            parameters[5].Value = model.City;
            parameters[6].Value = model.Province;
            parameters[7].Value = model.Country;
            parameters[8].Value = model.Language;
            parameters[9].Value = model.CreateTime;

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
        public bool Update(YSWL.WeChat.Model.Core.SceneDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_SceneDetail set ");
            strSql.Append("SceneId=?SceneId,");
            strSql.Append("OpenId=?OpenId,");
            strSql.Append("UserName=?UserName,");
            strSql.Append("NickName=?NickName,");
            strSql.Append("Sex=?Sex,");
            strSql.Append("City=?City,");
            strSql.Append("Province=?Province,");
            strSql.Append("Country=?Country,");
            strSql.Append("Language=?Language,");
            strSql.Append("CreateTime=?CreateTime");
            strSql.Append(" where DetailId=?DetailId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?SceneId", MySqlDbType.Int32,4),
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?NickName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Sex", MySqlDbType.Int32,4),
					new MySqlParameter("?City", MySqlDbType.VarChar,200),
					new MySqlParameter("?Province", MySqlDbType.VarChar,200),
					new MySqlParameter("?Country", MySqlDbType.VarChar,200),
					new MySqlParameter("?Language", MySqlDbType.VarChar,200),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?DetailId", MySqlDbType.Int32,4)};
            parameters[0].Value = model.SceneId;
            parameters[1].Value = model.OpenId;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.NickName;
            parameters[4].Value = model.Sex;
            parameters[5].Value = model.City;
            parameters[6].Value = model.Province;
            parameters[7].Value = model.Country;
            parameters[8].Value = model.Language;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.DetailId;

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
        public bool Delete(int DetailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_SceneDetail ");
            strSql.Append(" where DetailId=?DetailId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?DetailId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = DetailId;

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
        public bool DeleteList(string DetailIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_SceneDetail ");
            strSql.Append(" where DetailId in (" + DetailIdlist + ")  ");
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
        public YSWL.WeChat.Model.Core.SceneDetail GetModel(int DetailId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DetailId,SceneId,OpenId,UserName,NickName,Sex,City,Province,Country,Language,CreateTime from WeChat_SceneDetail ");
            strSql.Append(" where DetailId=?DetailId LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?DetailId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = DetailId;

            YSWL.WeChat.Model.Core.SceneDetail model = new YSWL.WeChat.Model.Core.SceneDetail();
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
        public YSWL.WeChat.Model.Core.SceneDetail DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.SceneDetail model = new YSWL.WeChat.Model.Core.SceneDetail();
            if (row != null)
            {
                if (row["DetailId"] != null && row["DetailId"].ToString() != "")
                {
                    model.DetailId = int.Parse(row["DetailId"].ToString());
                }
                if (row["SceneId"] != null && row["SceneId"].ToString() != "")
                {
                    model.SceneId = int.Parse(row["SceneId"].ToString());
                }
                if (row["OpenId"] != null)
                {
                    model.OpenId = row["OpenId"].ToString();
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["NickName"] != null)
                {
                    model.NickName = row["NickName"].ToString();
                }
                if (row["Sex"] != null && row["Sex"].ToString() != "")
                {
                    model.Sex = int.Parse(row["Sex"].ToString());
                }
                if (row["City"] != null)
                {
                    model.City = row["City"].ToString();
                }
                if (row["Province"] != null)
                {
                    model.Province = row["Province"].ToString();
                }
                if (row["Country"] != null)
                {
                    model.Country = row["Country"].ToString();
                }
                if (row["Language"] != null)
                {
                    model.Language = row["Language"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
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
            strSql.Append("select DetailId,SceneId,OpenId,UserName,NickName,Sex,City,Province,Country,Language,CreateTime ");
            strSql.Append(" FROM WeChat_SceneDetail ");
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
            
            strSql.Append(" DetailId,SceneId,OpenId,UserName,NickName,Sex,City,Province,Country,Language,CreateTime ");
            strSql.Append(" FROM WeChat_SceneDetail ");
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
            strSql.Append("select count(1) FROM WeChat_SceneDetail ");
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
            strSql.Append("SELECT T.* from WeChat_SceneDetail T ");
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
                strSql.Append(" order by T.DetailId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }

 

        #endregion  BasicMethod

		#region  ExtensionMethod
        public DataSet GetList(int top, int sceneId, string startdate, string enddate, string filedOrder)
        {
            StringBuilder strWhere = new StringBuilder();
            if (sceneId > 0)
            {
                strWhere.AppendFormat("SceneId={0}", sceneId);
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


        public YSWL.WeChat.Model.Core.SceneDetail GetSceneDetail(string openId, string userOpen)
        {
            throw new NotImplementedException();
        }

	    public bool IsExist(int sceneId, string openId, string userOpen)
	    {
            throw new NotImplementedException();
        }

        int ISceneDetail.GetMaxId()
        {
            throw new NotImplementedException();
        }

        bool ISceneDetail.Exists(int DetailId)
        {
            throw new NotImplementedException();
        }

        int ISceneDetail.Add(Model.Core.SceneDetail model)
        {
            throw new NotImplementedException();
        }

        bool ISceneDetail.Update(Model.Core.SceneDetail model)
        {
            throw new NotImplementedException();
        }

        bool ISceneDetail.Delete(int DetailId)
        {
            throw new NotImplementedException();
        }

        bool ISceneDetail.DeleteList(string DetailIdlist)
        {
            throw new NotImplementedException();
        }

        Model.Core.SceneDetail ISceneDetail.GetModel(int DetailId)
        {
            throw new NotImplementedException();
        }

        Model.Core.SceneDetail ISceneDetail.DataRowToModel(DataRow row)
        {
            throw new NotImplementedException();
        }

        DataSet ISceneDetail.GetList(string strWhere)
        {
            throw new NotImplementedException();
        }

        DataSet ISceneDetail.GetList(int Top, string strWhere, string filedOrder)
        {
            throw new NotImplementedException();
        }

        int ISceneDetail.GetRecordCount(string strWhere)
        {
            throw new NotImplementedException();
        }

        DataSet ISceneDetail.GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            throw new NotImplementedException();
        }

        DataSet ISceneDetail.GetList(int top, int sceneId, string startdate, string enddate, string filedOrder)
        {
            throw new NotImplementedException();
        }

        Model.Core.SceneDetail ISceneDetail.GetSceneDetail(string openId, string userOpen)
        {
            throw new NotImplementedException();
        }

        bool ISceneDetail.IsExist(int sceneId, string openId, string userOpen)
        {
            throw new NotImplementedException();
        }

        DataSet ISceneDetail.GetList(string openId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        #endregion  ExtensionMethod
    }
}

