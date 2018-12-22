using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.IDAL.Tao;
using YSWL.DBUtility;//Please add references
namespace YSWL.SQLServerDAL.Tao
{
	/// <summary>
	/// 数据访问类:ShopCategory
	/// </summary>
	public partial class ShopCategory:IShopCategory
	{
		public ShopCategory()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("ShopCateId", "Tao_ShopCategory");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ShopCateId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Tao_ShopCategory");
            strSql.Append(" where ShopCateId=@ShopCateId");
            SqlParameter[] parameters = {
					new SqlParameter("@ShopCateId", SqlDbType.Int,4)
			};
            parameters[0].Value = ShopCateId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.Model.Tao.ShopCategory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Tao_ShopCategory(");
            strSql.Append("Name,ParentID,Path,Depth,Sequence,HasChildren,Status)");
            strSql.Append(" values (");
            strSql.Append("@Name,@ParentID,@Path,@Depth,@Sequence,@HasChildren,@Status)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Path", SqlDbType.NVarChar,200),
					new SqlParameter("@Depth", SqlDbType.Int,4),
					new SqlParameter("@Sequence", SqlDbType.Int,4),
					new SqlParameter("@HasChildren", SqlDbType.Bit,1),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.ParentID;
            parameters[2].Value = model.Path;
            parameters[3].Value = model.Depth;
            parameters[4].Value = model.Sequence;
            parameters[5].Value = model.HasChildren;
            parameters[6].Value = model.Status;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        public bool Update(YSWL.Model.Tao.ShopCategory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_ShopCategory set ");
            strSql.Append("Name=@Name,");
            strSql.Append("ParentID=@ParentID,");
            strSql.Append("Path=@Path,");
            strSql.Append("Depth=@Depth,");
            strSql.Append("Sequence=@Sequence,");
            strSql.Append("HasChildren=@HasChildren,");
            strSql.Append("Status=@Status");
            strSql.Append(" where ShopCateId=@ShopCateId");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Path", SqlDbType.NVarChar,200),
					new SqlParameter("@Depth", SqlDbType.Int,4),
					new SqlParameter("@Sequence", SqlDbType.Int,4),
					new SqlParameter("@HasChildren", SqlDbType.Bit,1),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@ShopCateId", SqlDbType.Int,4)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.ParentID;
            parameters[2].Value = model.Path;
            parameters[3].Value = model.Depth;
            parameters[4].Value = model.Sequence;
            parameters[5].Value = model.HasChildren;
            parameters[6].Value = model.Status;
            parameters[7].Value = model.ShopCateId;

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
        public bool Delete(int ShopCateId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_ShopCategory ");
            strSql.Append(" where ShopCateId=@ShopCateId");
            SqlParameter[] parameters = {
					new SqlParameter("@ShopCateId", SqlDbType.Int,4)
			};
            parameters[0].Value = ShopCateId;

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
        public bool DeleteList(string ShopCateIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_ShopCategory ");
            strSql.Append(" where ShopCateId in (" + ShopCateIdlist + ")  ");
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
        public YSWL.Model.Tao.ShopCategory GetModel(int ShopCateId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ShopCateId,Name,ParentID,Path,Depth,Sequence,HasChildren,Status from Tao_ShopCategory ");
            strSql.Append(" where ShopCateId=@ShopCateId");
            SqlParameter[] parameters = {
					new SqlParameter("@ShopCateId", SqlDbType.Int,4)
			};
            parameters[0].Value = ShopCateId;

            YSWL.Model.Tao.ShopCategory model = new YSWL.Model.Tao.ShopCategory();
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
        public YSWL.Model.Tao.ShopCategory DataRowToModel(DataRow row)
        {
            YSWL.Model.Tao.ShopCategory model = new YSWL.Model.Tao.ShopCategory();
            if (row != null)
            {
                if (row["ShopCateId"] != null && row["ShopCateId"].ToString() != "")
                {
                    model.ShopCateId = int.Parse(row["ShopCateId"].ToString());
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["ParentID"] != null && row["ParentID"].ToString() != "")
                {
                    model.ParentID = int.Parse(row["ParentID"].ToString());
                }
                if (row["Path"] != null)
                {
                    model.Path = row["Path"].ToString();
                }
                if (row["Depth"] != null && row["Depth"].ToString() != "")
                {
                    model.Depth = int.Parse(row["Depth"].ToString());
                }
                if (row["Sequence"] != null && row["Sequence"].ToString() != "")
                {
                    model.Sequence = int.Parse(row["Sequence"].ToString());
                }
                if (row["HasChildren"] != null && row["HasChildren"].ToString() != "")
                {
                    if ((row["HasChildren"].ToString() == "1") || (row["HasChildren"].ToString().ToLower() == "true"))
                    {
                        model.HasChildren = true;
                    }
                    else
                    {
                        model.HasChildren = false;
                    }
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
            strSql.Append("select ShopCateId,Name,ParentID,Path,Depth,Sequence,HasChildren,Status ");
            strSql.Append(" FROM Tao_ShopCategory ");
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
            strSql.Append(" ShopCateId,Name,ParentID,Path,Depth,Sequence,HasChildren,Status ");
            strSql.Append(" FROM Tao_ShopCategory ");
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
            strSql.Append("select count(1) FROM Tao_ShopCategory ");
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
                strSql.Append("order by T.ShopCateId desc");
            }
            strSql.Append(")AS Row, T.*  from Tao_ShopCategory T ");
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
            parameters[0].Value = "Tao_ShopCategory";
            parameters[1].Value = "ShopCateId";
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
            strSql.Append("update Tao_ShopCategory  set Status=@Status");
            strSql.Append(" where ShopCateId in (" + ids + ")  ");
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

