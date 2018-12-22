using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.IDAL.Tao;
using YSWL.DBUtility;
using System.Collections.Generic;//Please add references
namespace YSWL.SQLServerDAL.Tao
{
	/// <summary>
	/// 数据访问类:CategorySource
	/// </summary>
	public partial class CategorySource:ICategorySource
	{
		public CategorySource()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long SourceCId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Tao_CategorySource");
            strSql.Append(" where SourceCId=@SourceCId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SourceCId", SqlDbType.BigInt,8)			};
            parameters[0].Value = SourceCId;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.Model.Tao.CategorySource model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Tao_CategorySource(");
            strSql.Append("SourceCId,Name,Description,ParentID,Path,Depth,Sequence,HasChildren,CreatedUserID,CreatedDate,Status,TaoCategoryId)");
            strSql.Append(" values (");
            strSql.Append("@SourceCId,@Name,@Description,@ParentID,@Path,@Depth,@Sequence,@HasChildren,@CreatedUserID,@CreatedDate,@Status,@TaoCategoryId)");
            SqlParameter[] parameters = {
					new SqlParameter("@SourceCId", SqlDbType.BigInt,8),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Description", SqlDbType.NVarChar,200),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Path", SqlDbType.NVarChar,50),
					new SqlParameter("@Depth", SqlDbType.Int,4),
					new SqlParameter("@Sequence", SqlDbType.Int,4),
					new SqlParameter("@HasChildren", SqlDbType.Bit,1),
					new SqlParameter("@CreatedUserID", SqlDbType.Int,4),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@TaoCategoryId", SqlDbType.Int,4)};
            parameters[0].Value = model.SourceCId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Description;
            parameters[3].Value = model.ParentID;
            parameters[4].Value = model.Path;
            parameters[5].Value = model.Depth;
            parameters[6].Value = model.Sequence;
            parameters[7].Value = model.HasChildren;
            parameters[8].Value = model.CreatedUserID;
            parameters[9].Value = model.CreatedDate;
            parameters[10].Value = model.Status;
            parameters[11].Value = model.TaoCategoryId;

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
        public bool Update(YSWL.Model.Tao.CategorySource model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_CategorySource set ");
            strSql.Append("Name=@Name,");
            strSql.Append("Description=@Description,");
            strSql.Append("ParentID=@ParentID,");
            strSql.Append("Path=@Path,");
            strSql.Append("Depth=@Depth,");
            strSql.Append("Sequence=@Sequence,");
            strSql.Append("HasChildren=@HasChildren,");
            strSql.Append("CreatedUserID=@CreatedUserID,");
            strSql.Append("CreatedDate=@CreatedDate,");
            strSql.Append("Status=@Status,");
            strSql.Append("TaoCategoryId=@TaoCategoryId");
            strSql.Append(" where SourceCId=@SourceCId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Description", SqlDbType.NVarChar,200),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@Path", SqlDbType.NVarChar,50),
					new SqlParameter("@Depth", SqlDbType.Int,4),
					new SqlParameter("@Sequence", SqlDbType.Int,4),
					new SqlParameter("@HasChildren", SqlDbType.Bit,1),
					new SqlParameter("@CreatedUserID", SqlDbType.Int,4),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@TaoCategoryId", SqlDbType.Int,4),
					new SqlParameter("@SourceCId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.Description;
            parameters[2].Value = model.ParentID;
            parameters[3].Value = model.Path;
            parameters[4].Value = model.Depth;
            parameters[5].Value = model.Sequence;
            parameters[6].Value = model.HasChildren;
            parameters[7].Value = model.CreatedUserID;
            parameters[8].Value = model.CreatedDate;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.TaoCategoryId;
            parameters[11].Value = model.SourceCId;

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
        public bool Delete(long SourceCId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_CategorySource ");
            strSql.Append(" where SourceCId=@SourceCId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SourceCId", SqlDbType.BigInt,8)			};
            parameters[0].Value = SourceCId;

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
        public bool DeleteList(string SourceCIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_CategorySource ");
            strSql.Append(" where SourceCId in (" + SourceCIdlist + ")  ");
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
        public YSWL.Model.Tao.CategorySource GetModel(long SourceCId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 SourceCId,Name,Description,ParentID,Path,Depth,Sequence,HasChildren,CreatedUserID,CreatedDate,Status,TaoCategoryId from Tao_CategorySource ");
            strSql.Append(" where SourceCId=@SourceCId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SourceCId", SqlDbType.BigInt,8)			};
            parameters[0].Value = SourceCId;

            YSWL.Model.Tao.CategorySource model = new YSWL.Model.Tao.CategorySource();
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
        public YSWL.Model.Tao.CategorySource DataRowToModel(DataRow row)
        {
            YSWL.Model.Tao.CategorySource model = new YSWL.Model.Tao.CategorySource();
            if (row != null)
            {
                if (row["SourceCId"] != null && row["SourceCId"].ToString() != "")
                {
                    model.SourceCId = long.Parse(row["SourceCId"].ToString());
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
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
                if (row["CreatedUserID"] != null && row["CreatedUserID"].ToString() != "")
                {
                    model.CreatedUserID = int.Parse(row["CreatedUserID"].ToString());
                }
                if (row["CreatedDate"] != null && row["CreatedDate"].ToString() != "")
                {
                    model.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["TaoCategoryId"] != null && row["TaoCategoryId"].ToString() != "")
                {
                    model.TaoCategoryId = int.Parse(row["TaoCategoryId"].ToString());
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
            strSql.Append("select SourceCId,Name,Description,ParentID,Path,Depth,Sequence,HasChildren,CreatedUserID,CreatedDate,Status,TaoCategoryId ");
            strSql.Append(" FROM Tao_CategorySource ");
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
            strSql.Append(" SourceCId,Name,Description,ParentID,Path,Depth,Sequence,HasChildren,CreatedUserID,CreatedDate,Status,TaoCategoryId ");
            strSql.Append(" FROM Tao_CategorySource ");
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
            strSql.Append("select count(1) FROM Tao_CategorySource ");
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
                strSql.Append("order by T.SourceCId desc");
            }
            strSql.Append(")AS Row, T.*  from Tao_CategorySource T ");
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
            parameters[0].Value = "Tao_CategorySource";
            parameters[1].Value = "SourceCId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region 扩展方法
        public bool UpdatePathAndDepth(long id, int parentid)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd = new CommandInfo();
            if (parentid == 0)
            {
                //更新自己
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update Tao_CategorySource set ");
                strSql.Append("Depth=@Depth,");
                strSql.Append("Path=@Path,");
                strSql.Append("HasChildren='false'");
                strSql.Append(" where SourceCId=@CategoryID");
                SqlParameter[] parameters = {
					new SqlParameter("@Depth", SqlDbType.Int,4),
					new SqlParameter("@Path", SqlDbType.NVarChar,200),
					new SqlParameter("@CategoryID", SqlDbType.Int,4)};
                parameters[0].Value = 1;
                parameters[1].Value = id;
                parameters[2].Value = id;
                cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);
            }
            else
            {
                //更新自己
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update Tao_CategorySource set ");
                strSql.Append("Depth=(select Tao_CategorySource.depth from Tao_CategorySource where SourceCId=@ParentID)+1,");
                strSql.Append("Path=(select Tao_CategorySource.Path from Tao_CategorySource where SourceCId=@ParentID)+@Path,");
                strSql.Append("HasChildren='true'");
                strSql.Append(" where SourceCId=@CategoryID");
                SqlParameter[] parameters = {
					new SqlParameter("@Path", SqlDbType.NVarChar,200),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@CategoryID", SqlDbType.Int,4)};
                parameters[0].Value = "|" + id;
                parameters[1].Value = parentid;
                parameters[2].Value = id;
                cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);


            }
            //更新子类
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("UPDATE Tao_CategorySource set");
            strSql2.Append(" Depth=(select Tao_CategorySource.depth from Tao_CategorySource where SourceCId=@CategoryID)+1,");
            strSql2.Append(" Path=(select Tao_CategorySource.Path from Tao_CategorySource where SourceCId=@CategoryID)+@Path ");
            strSql2.Append("where ParentID=@CategoryID");
            SqlParameter[] parameters2 = {
					new SqlParameter("@Path", SqlDbType.NVarChar,200),
					new SqlParameter("@ParentID", SqlDbType.Int,4),
					new SqlParameter("@CategoryID", SqlDbType.Int,4)};
            parameters2[0].Value = "|" + id;
            parameters2[1].Value = parentid;
            parameters2[2].Value = id;
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            int rowsAffected = DbHelperSQL.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddCategory(YSWL.Model.Tao.CategorySource model)
        {
            if (Add(model))
            {
                return UpdatePathAndDepth(model.SourceCId, model.ParentID);
            }
            return false;
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateCategory(YSWL.Model.Tao.CategorySource model)
        {
            if (Update(model))
            {
                return UpdatePathAndDepth(model.SourceCId, model.ParentID);
            }
            return false;
        }

        /// <summary>
        /// 获得数据列表(是否排序)
        /// </summary>
        public DataSet GetCategoryList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM Tao_CategorySource ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" ORDER BY path ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 删除分类信息
        /// </summary>
        public bool DeleteCategory(int CategoryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Tao_CategorySource ");
            strSql.Append(" where path like (select Tao_CategorySource.Path from Tao_CategorySource where SourceCId=@CategoryId)+'%'");
            SqlParameter[] parameters = {
					new SqlParameter("@CategoryId", SqlDbType.Int,4)
			};
            parameters[0].Value = CategoryID;
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
        /// 对分类进行排序
        /// </summary>
        public bool SwapSequence(int CategoryId, Model.Shop.Products.SwapSequenceIndex zIndex)
        {
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("update Tao_CategorySource set ");
            //if ((int)zIndex == 1)
            //{
            //    strSql.Append("DisplaySequence=DisplaySequence-1");
            //}
            //if ((int)zIndex == 0)
            //{
            //    strSql.Append("DisplaySequence=DisplaySequence+1");
            //}
            //strSql.Append(" where CategoryID=@CategoryID");
            //SqlParameter[] parameters = {
            //        new SqlParameter("@CategoryID", SqlDbType.Int,4)
            //                            };
            //parameters[0].Value = CategoryId;
            //int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            //if (rows > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }
        //暂时针对淘宝实现
        public bool UpdateTaoCate(int CategoryId, int TaoCateId, bool IsLoop)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_CategorySource set ");
            strSql.Append("TaoCategoryId=@TaoCategoryId");
            if (IsLoop)
            {
                strSql.Append(" where  path like (select path from Tao_CategorySource where SourceCId=@CategoryId)+'%';");
            }
            else
            {
                strSql.Append(" where  SourceCId=@CategoryId ");
            }
            SqlParameter[] parameters = {
					new SqlParameter("@TaoCategoryId", SqlDbType.Int,4),
					new SqlParameter("@CategoryId", SqlDbType.Int,4)};
            parameters[0].Value = TaoCateId;
            parameters[1].Value = CategoryId;


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
        /// 批量对应分类
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="TaoCateId"></param>
        /// <param name="IsLoop"></param>
        /// <returns></returns>
        public bool UpdateTaoCateList(string ids, int TaoCateId, bool IsLoop)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Tao_CategorySource set ");
            strSql.Append("TaoCategoryId=@TaoCategoryId");
            if (IsLoop)
            {

                var arry = ids.Split(',');
                int i = 0;
                foreach (string str in arry)
                {
                    if (i == 0)
                    {
                        strSql.Append(" where  path like (select path from Tao_CategorySource where SourceCId =" + str + ")+'%'");
                    }
                    else
                    {
                        strSql.Append(" or  path like (select path from Tao_CategorySource where SourceCId =" + str + ")+'%'");
                    }
                    i++;
                }

            }
            else
            {
                strSql.Append(" where  SourceCId in (" + ids + ") ");
            }
            SqlParameter[] parameters = {
					new SqlParameter("@TaoCategoryId", SqlDbType.Int,4)};
            parameters[0].Value = TaoCateId;


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
        #endregion
	}
}

