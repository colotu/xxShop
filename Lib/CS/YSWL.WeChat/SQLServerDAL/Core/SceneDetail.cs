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
namespace YSWL.WeChat.SQLServerDAL.Core
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
            return DBHelper.DefaultDBHelper.GetMaxID("DetailId", "WeChat_SceneDetail");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int DetailId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_SceneDetail");
            strSql.Append(" where DetailId=@DetailId");
            SqlParameter[] parameters = {
                    new SqlParameter("@DetailId", SqlDbType.Int,4)
            };
            parameters[0].Value = DetailId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Core.SceneDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_SceneDetail(");
            strSql.Append("SceneId,OpenId,UserName,NickName,Sex,City,Province,Country,ReferUserId,Language,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@SceneId,@OpenId,@UserName,@NickName,@Sex,@City,@Province,@Country,@ReferUserId,@Language,@CreateTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@SceneId", SqlDbType.Int,4),
                    new SqlParameter("@OpenId", SqlDbType.NVarChar,200),
                    new SqlParameter("@UserName", SqlDbType.NVarChar,200),
                    new SqlParameter("@NickName", SqlDbType.NVarChar,200),
                    new SqlParameter("@Sex", SqlDbType.Int,4),
                    new SqlParameter("@City", SqlDbType.NVarChar,200),
                    new SqlParameter("@Province", SqlDbType.NVarChar,200),
                    new SqlParameter("@Country", SqlDbType.NVarChar,200),
                    new SqlParameter("@ReferUserId", SqlDbType.Int,4),
                    new SqlParameter("@Language", SqlDbType.NVarChar,200),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.SceneId;
            parameters[1].Value = model.OpenId;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.NickName;
            parameters[4].Value = model.Sex;
            parameters[5].Value = model.City;
            parameters[6].Value = model.Province;
            parameters[7].Value = model.Country;
            parameters[8].Value = model.ReferUserId;
            parameters[9].Value = model.Language;
            parameters[10].Value = model.CreateTime;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
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
            strSql.Append("SceneId=@SceneId,");
            strSql.Append("OpenId=@OpenId,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("NickName=@NickName,");
            strSql.Append("Sex=@Sex,");
            strSql.Append("City=@City,");
            strSql.Append("Province=@Province,");
            strSql.Append("Country=@Country,");
            strSql.Append("ReferUserId=@ReferUserId,");
            strSql.Append("Language=@Language,");
            strSql.Append("CreateTime=@CreateTime");
            strSql.Append(" where DetailId=@DetailId");
            SqlParameter[] parameters = {
                    new SqlParameter("@SceneId", SqlDbType.Int,4),
                    new SqlParameter("@OpenId", SqlDbType.NVarChar,200),
                    new SqlParameter("@UserName", SqlDbType.NVarChar,200),
                    new SqlParameter("@NickName", SqlDbType.NVarChar,200),
                    new SqlParameter("@Sex", SqlDbType.Int,4),
                    new SqlParameter("@City", SqlDbType.NVarChar,200),
                    new SqlParameter("@Province", SqlDbType.NVarChar,200),
                    new SqlParameter("@Country", SqlDbType.NVarChar,200),
                    new SqlParameter("@ReferUserId", SqlDbType.Int,4),
                    new SqlParameter("@Language", SqlDbType.NVarChar,200),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@DetailId", SqlDbType.Int,4)};
            parameters[0].Value = model.SceneId;
            parameters[1].Value = model.OpenId;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.NickName;
            parameters[4].Value = model.Sex;
            parameters[5].Value = model.City;
            parameters[6].Value = model.Province;
            parameters[7].Value = model.Country;
            parameters[8].Value = model.ReferUserId;
            parameters[9].Value = model.Language;
            parameters[10].Value = model.CreateTime;
            parameters[11].Value = model.DetailId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
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
            strSql.Append(" where DetailId=@DetailId");
            SqlParameter[] parameters = {
                    new SqlParameter("@DetailId", SqlDbType.Int,4)
            };
            parameters[0].Value = DetailId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
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
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
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
            strSql.Append("select  top 1 DetailId,SceneId,OpenId,UserName,NickName,Sex,City,Province,Country,ReferUserId,Language,CreateTime from WeChat_SceneDetail ");
            strSql.Append(" where DetailId=@DetailId");
            SqlParameter[] parameters = {
                    new SqlParameter("@DetailId", SqlDbType.Int,4)
            };
            parameters[0].Value = DetailId;

            YSWL.WeChat.Model.Core.SceneDetail model = new YSWL.WeChat.Model.Core.SceneDetail();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
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
                if (row["ReferUserId"] != null && row["ReferUserId"].ToString() != "")
                {
                    model.ReferUserId = int.Parse(row["ReferUserId"].ToString());
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
            strSql.Append("select DetailId,SceneId,OpenId,UserName,NickName,Sex,City,Province,Country,ReferUserId,Language,CreateTime ");
            strSql.Append(" FROM WeChat_SceneDetail ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" DetailId,SceneId,OpenId,UserName,NickName,Sex,City,Province,Country,ReferUserId,Language,CreateTime ");
            strSql.Append(" FROM WeChat_SceneDetail ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
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
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
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
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.DetailId desc");
            }
            strSql.Append(")AS Row, T.*  from WeChat_SceneDetail T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
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
                strWhere.AppendFormat("  CreateTime< dateadd(day,1,'{0}')", enddate);
            }
            return GetList(top, strWhere.ToString(), filedOrder);
        }


	    public YSWL.WeChat.Model.Core.SceneDetail GetSceneDetail(string openId, string userOpen)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from WeChat_SceneDetail ");
            strSql.Append(" where OpenId=@OpenId and UserName=@UserName order by CreateTime desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@OpenId", SqlDbType.NVarChar,200),
                       new SqlParameter("@UserName", SqlDbType.NVarChar,200)
            };
            parameters[0].Value = openId;
            parameters[1].Value = userOpen;

            YSWL.WeChat.Model.Core.SceneDetail model = new YSWL.WeChat.Model.Core.SceneDetail();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        public bool IsExist(int sceneId, string openId, string userOpen)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_SceneDetail");
            strSql.Append(" where SceneId=@SceneId and OpenId=@OpenId and UserName=@UserName");
            SqlParameter[] parameters = {
                    new SqlParameter("@SceneId", SqlDbType.Int,4),
                       new SqlParameter("@OpenId", SqlDbType.NVarChar,200),
                          new SqlParameter("@UserName", SqlDbType.NVarChar,200)
            };
            parameters[0].Value = sceneId;
            parameters[1].Value = openId;
            parameters[2].Value = userOpen;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 推广渠道统计
        /// </summary>
        public DataSet GetList(string openId , DateTime startDate, DateTime endDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select s.Name, sd.* from( ");
            strSql.Append(" select COUNT(1) Count, SceneId ");
            strSql.Append(" FROM WeChat_SceneDetail ");
            strSql.AppendFormat(" where  OpenId = '{0}' " ,openId);
            strSql.AppendFormat("  and  CreateTime >'{0}' AND CreateTime<'{1}' ", startDate, endDate);
            strSql.Append("  group by SceneId  " );
            strSql.Append(" )  sd ");
            strSql.Append("  inner join WeChat_Scene s on s.SceneId = sd.SceneId  ");
            strSql.Append(" order by   Count desc ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        #endregion  ExtensionMethod
    }
}

