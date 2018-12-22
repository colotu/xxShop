using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Members;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Members
{
	/// <summary>
	/// 数据访问类:UserRank
	/// </summary>
	public partial class UserRank:IUserRank
	{
		public UserRank()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("RankId", "Accounts_UserRank");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int RankId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_UserRank");
            strSql.Append(" where RankId=@RankId");
            SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4)
			};
            parameters[0].Value = RankId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Members.UserRank model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_UserRank(");
            strSql.Append("Name,RankLevel,ScoreMax,ScoreMin,IsDefault,RankType,Description,CreatedUserId)");
            strSql.Append(" values (");
            strSql.Append("@Name,@RankLevel,@ScoreMax,@ScoreMin,@IsDefault,@RankType,@Description,@CreatedUserId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@RankLevel", SqlDbType.Int,4),
					new SqlParameter("@ScoreMax", SqlDbType.Int,4),
					new SqlParameter("@ScoreMin", SqlDbType.Int,4),
					new SqlParameter("@IsDefault", SqlDbType.Bit,1),
					new SqlParameter("@RankType", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,200),
					new SqlParameter("@CreatedUserId", SqlDbType.Int,4)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.RankLevel;
            parameters[2].Value = model.ScoreMax;
            parameters[3].Value = model.ScoreMin;
            parameters[4].Value = model.IsDefault;
            parameters[5].Value = model.RankType;
            parameters[6].Value = model.Description;
            parameters[7].Value = model.CreatedUserId;

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
        public bool Update(YSWL.MALL.Model.Members.UserRank model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_UserRank set ");
            strSql.Append("Name=@Name,");
            strSql.Append("RankLevel=@RankLevel,");
            strSql.Append("ScoreMax=@ScoreMax,");
            strSql.Append("ScoreMin=@ScoreMin,");
            strSql.Append("IsDefault=@IsDefault,");
            strSql.Append("RankType=@RankType,");
            strSql.Append("Description=@Description,");
            strSql.Append("CreatedUserId=@CreatedUserId");
            strSql.Append(" where RankId=@RankId");
            SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@RankLevel", SqlDbType.Int,4),
					new SqlParameter("@ScoreMax", SqlDbType.Int,4),
					new SqlParameter("@ScoreMin", SqlDbType.Int,4),
					new SqlParameter("@IsDefault", SqlDbType.Bit,1),
					new SqlParameter("@RankType", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,200),
					new SqlParameter("@CreatedUserId", SqlDbType.Int,4),
					new SqlParameter("@RankId", SqlDbType.Int,4)};
            parameters[0].Value = model.Name;
            parameters[1].Value = model.RankLevel;
            parameters[2].Value = model.ScoreMax;
            parameters[3].Value = model.ScoreMin;
            parameters[4].Value = model.IsDefault;
            parameters[5].Value = model.RankType;
            parameters[6].Value = model.Description;
            parameters[7].Value = model.CreatedUserId;
            parameters[8].Value = model.RankId;

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
        public bool Delete(int RankId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_UserRank ");
            strSql.Append(" where RankId=@RankId");
            SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4)
			};
            parameters[0].Value = RankId;

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
        public bool DeleteList(string RankIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_UserRank ");
            strSql.Append(" where RankId in (" + RankIdlist + ")  ");
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
        public YSWL.MALL.Model.Members.UserRank GetModel(int RankId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RankId,Name,RankLevel,ScoreMax,ScoreMin,IsDefault,RankType,Description,CreatedUserId from Accounts_UserRank ");
            strSql.Append(" where RankId=@RankId");
            SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4)
			};
            parameters[0].Value = RankId;

            YSWL.MALL.Model.Members.UserRank model = new YSWL.MALL.Model.Members.UserRank();
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
        public YSWL.MALL.Model.Members.UserRank DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Members.UserRank model = new YSWL.MALL.Model.Members.UserRank();
            if (row != null)
            {
                if (row["RankId"] != null && row["RankId"].ToString() != "")
                {
                    model.RankId = int.Parse(row["RankId"].ToString());
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["RankLevel"] != null && row["RankLevel"].ToString() != "")
                {
                    model.RankLevel = int.Parse(row["RankLevel"].ToString());
                }
                if (row["ScoreMax"] != null && row["ScoreMax"].ToString() != "")
                {
                    model.ScoreMax = int.Parse(row["ScoreMax"].ToString());
                }
                if (row["ScoreMin"] != null && row["ScoreMin"].ToString() != "")
                {
                    model.ScoreMin = int.Parse(row["ScoreMin"].ToString());
                }
                if (row["IsDefault"] != null && row["IsDefault"].ToString() != "")
                {
                    if ((row["IsDefault"].ToString() == "1") || (row["IsDefault"].ToString().ToLower() == "true"))
                    {
                        model.IsDefault = true;
                    }
                    else
                    {
                        model.IsDefault = false;
                    }
                }
                if (row["RankType"] != null && row["RankType"].ToString() != "")
                {
                    model.RankType = int.Parse(row["RankType"].ToString());
                }
                if (row["Description"] != null)
                {
                    model.Description = row["Description"].ToString();
                }
                if (row["CreatedUserId"] != null && row["CreatedUserId"].ToString() != "")
                {
                    model.CreatedUserId = int.Parse(row["CreatedUserId"].ToString());
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
            strSql.Append("select RankId,Name,RankLevel,ScoreMax,ScoreMin,IsDefault,RankType,Description,CreatedUserId ");
            strSql.Append(" FROM Accounts_UserRank ");
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
            strSql.Append(" RankId,Name,RankLevel,ScoreMax,ScoreMin,IsDefault,RankType,Description,CreatedUserId ");
            strSql.Append(" FROM Accounts_UserRank ");
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
            strSql.Append("select count(1) FROM Accounts_UserRank ");
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
                strSql.Append("order by T.RankId desc");
            }
            strSql.Append(")AS Row, T.*  from Accounts_UserRank T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "Accounts_UserRank";
            parameters[1].Value = "RankId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
		#region  ExtensionMethod
        /// <summary>
        /// 根据成长值获取等级
        /// </summary>
        /// <param name="grades">用户分数</param>
        /// <returns></returns>
        public string GetUserLevel(int grades)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select   top 1 name from Accounts_UserRank ");
            strSql.Append(" WHERE @Score >=ScoreMin AND @Score<ScoreMax");
            SqlParameter[] parameters = {
					new SqlParameter("@Score", SqlDbType.Int,4)
			};
            parameters[0].Value = grades;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }
 
	    /// <summary>
        /// 获得会员等级数据
	    /// </summary>
	    /// <param name="ruleId">批发规则Id</param>
	    /// <returns></returns>
        public DataSet GetList(int ruleId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from Accounts_UserRank ur ");
            strSql.Append(" where  ");
            strSql.AppendFormat("  EXISTS  ( select rankId  FROM Shop_SalesUserRank sur  where ruleId={0}  and ur.rankId=sur.rankId ) ", ruleId);
            strSql.Append("  order by   ranklevel    "); 
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
	    }

	    #endregion  ExtensionMethod
	}
}

