using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.IDAL.Tao;
using YSWL.DBUtility;//Please add references
namespace YSWL.SQLServerDAL.Tao
{
	/// <summary>
	/// 数据访问类:ShopCateSource
	/// </summary>
	public partial class ShopCateSource:IShopCateSource
	{
		public ShopCateSource()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("SourceCateId", "Tao_ShopCateSource");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int SourceCateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Tao_ShopCateSource");
            strSql.Append(" where SourceCateId=@SourceCateId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SourceCateId", SqlDbType.Int,4)			};
            parameters[0].Value = SourceCateId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.Model.Tao.ShopCateSource model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Tao_ShopCateSource(");
            strSql.Append("SourceCateId,ParentId,SourceName,IsParent,Path,Depth,ShopCateId,Status)");
            strSql.Append(" values (");
            strSql.Append("@SourceCateId,@ParentId,@SourceName,@IsParent,@Path,@Depth,@ShopCateId,@Status)");
            SqlParameter[] parameters = {
					new SqlParameter("@SourceCateId", SqlDbType.Int,4),
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@SourceName", SqlDbType.NVarChar,200),
					new SqlParameter("@IsParent", SqlDbType.Bit,1),
					new SqlParameter("@Path", SqlDbType.NVarChar,50),
					new SqlParameter("@Depth", SqlDbType.Int,4),
					new SqlParameter("@ShopCateId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.SourceCateId;
            parameters[1].Value = model.ParentId;
            parameters[2].Value = model.SourceName;
            parameters[3].Value = model.IsParent;
            parameters[4].Value = model.Path;
            parameters[5].Value = model.Depth;
            parameters[6].Value = model.ShopCateId;
            parameters[7].Value = model.Status;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Update(YSWL.Model.Tao.ShopCateSource model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_ShopCateSource set ");
            strSql.Append("ParentId=@ParentId,");
            strSql.Append("SourceName=@SourceName,");
            strSql.Append("IsParent=@IsParent,");
            strSql.Append("Path=@Path,");
            strSql.Append("Depth=@Depth,");
            strSql.Append("ShopCateId=@ShopCateId,");
            strSql.Append("Status=@Status");
            strSql.Append(" where SourceCateId=@SourceCateId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@SourceName", SqlDbType.NVarChar,200),
					new SqlParameter("@IsParent", SqlDbType.Bit,1),
					new SqlParameter("@Path", SqlDbType.NVarChar,50),
					new SqlParameter("@Depth", SqlDbType.Int,4),
					new SqlParameter("@ShopCateId", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@SourceCateId", SqlDbType.Int,4)};
            parameters[0].Value = model.ParentId;
            parameters[1].Value = model.SourceName;
            parameters[2].Value = model.IsParent;
            parameters[3].Value = model.Path;
            parameters[4].Value = model.Depth;
            parameters[5].Value = model.ShopCateId;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.SourceCateId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(int SourceCateId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_ShopCateSource ");
            strSql.Append(" where SourceCateId=@SourceCateId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SourceCateId", SqlDbType.Int,4)			};
            parameters[0].Value = SourceCateId;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool DeleteList(string SourceCateIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_ShopCateSource ");
            strSql.Append(" where SourceCateId in (" + SourceCateIdlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
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
        public YSWL.Model.Tao.ShopCateSource GetModel(int SourceCateId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 SourceCateId,ParentId,SourceName,IsParent,Path,Depth,ShopCateId,Status from Tao_ShopCateSource ");
            strSql.Append(" where SourceCateId=@SourceCateId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SourceCateId", SqlDbType.Int,4)			};
            parameters[0].Value = SourceCateId;

            YSWL.Model.Tao.ShopCateSource model = new YSWL.Model.Tao.ShopCateSource();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
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
        public YSWL.Model.Tao.ShopCateSource DataRowToModel(DataRow row)
        {
            YSWL.Model.Tao.ShopCateSource model = new YSWL.Model.Tao.ShopCateSource();
            if (row != null)
            {
                if (row["SourceCateId"] != null && row["SourceCateId"].ToString() != "")
                {
                    model.SourceCateId = int.Parse(row["SourceCateId"].ToString());
                }
                if (row["ParentId"] != null && row["ParentId"].ToString() != "")
                {
                    model.ParentId = int.Parse(row["ParentId"].ToString());
                }
                if (row["SourceName"] != null)
                {
                    model.SourceName = row["SourceName"].ToString();
                }
                if (row["IsParent"] != null && row["IsParent"].ToString() != "")
                {
                    if ((row["IsParent"].ToString() == "1") || (row["IsParent"].ToString().ToLower() == "true"))
                    {
                        model.IsParent = true;
                    }
                    else
                    {
                        model.IsParent = false;
                    }
                }
                if (row["Path"] != null)
                {
                    model.Path = row["Path"].ToString();
                }
                if (row["Depth"] != null && row["Depth"].ToString() != "")
                {
                    model.Depth = int.Parse(row["Depth"].ToString());
                }
                if (row["ShopCateId"] != null && row["ShopCateId"].ToString() != "")
                {
                    model.ShopCateId = int.Parse(row["ShopCateId"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
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
            strSql.Append("select SourceCateId,ParentId,SourceName,IsParent,Path,Depth,ShopCateId,Status ");
            strSql.Append(" FROM Tao_ShopCateSource ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
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
            strSql.Append(" SourceCateId,ParentId,SourceName,IsParent,Path,Depth,ShopCateId,Status ");
            strSql.Append(" FROM Tao_ShopCateSource ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Tao_ShopCateSource ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
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
                strSql.Append("order by T.SourceCateId desc");
            }
            strSql.Append(")AS Row, T.*  from Tao_ShopCateSource T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.NVarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.NVarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.NVarChar,1000),
                    };
            parameters[0].Value = "Tao_ShopCateSource";
            parameters[1].Value = "SourceCateId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
		#region  ExtensionMethod
        public bool UpdateStateList(string ids, int state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_ShopCateSource  set Status=@Status");
            strSql.Append(" where SourceCateId in (" + ids + ")  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Status", SqlDbType.Int,4)			};
            parameters[0].Value = state;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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

