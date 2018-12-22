using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using YSWL.DBUtility;//请先添加引用
using YSWL.MALL.IDAL.Poll;
namespace YSWL.MALL.SQLServerDAL.Poll
{
	/// <summary>
	/// 数据访问类Users。
	/// </summary>
	public class PollUsers:IPollUsers
	{



        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("UserID", "Poll_Users");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Poll_Users");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)
			};
            parameters[0].Value = UserID;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Poll.PollUsers model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Poll_Users(");
            strSql.Append("UserName,Password,TrueName,Age,Sex,Phone,Email,UserType,SysUserId)");
            strSql.Append(" values (");
            strSql.Append("@UserName,@Password,@TrueName,@Age,@Sex,@Phone,@Email,@UserType,@SysUserId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Password", SqlDbType.Binary,50),
					new SqlParameter("@TrueName", SqlDbType.NVarChar,50),
					new SqlParameter("@Age", SqlDbType.Int,4),
					new SqlParameter("@Sex", SqlDbType.Char,2),
					new SqlParameter("@Phone", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
					new SqlParameter("@UserType", SqlDbType.Char,2),
					new SqlParameter("@SysUserId", SqlDbType.Int,4)};
            parameters[0].Value = model.UserName;
            parameters[1].Value = model.Password;
            parameters[2].Value = model.TrueName;
            parameters[3].Value = model.Age;
            parameters[4].Value = model.Sex;
            parameters[5].Value = model.Phone;
            parameters[6].Value = model.Email;
            parameters[7].Value = model.UserType;
            parameters[8].Value = model.SysUserId;

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
        public bool Update(YSWL.MALL.Model.Poll.PollUsers model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Poll_Users set ");
            strSql.Append("UserName=@UserName,");
            strSql.Append("Password=@Password,");
            strSql.Append("TrueName=@TrueName,");
            strSql.Append("Age=@Age,");
            strSql.Append("Sex=@Sex,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("Email=@Email,");
            strSql.Append("UserType=@UserType,");
            strSql.Append("SysUserId=@SysUserId");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Password", SqlDbType.Binary,50),
					new SqlParameter("@TrueName", SqlDbType.NVarChar,50),
					new SqlParameter("@Age", SqlDbType.Int,4),
					new SqlParameter("@Sex", SqlDbType.Char,2),
					new SqlParameter("@Phone", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
					new SqlParameter("@UserType", SqlDbType.Char,2),
					new SqlParameter("@SysUserId", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = model.UserName;
            parameters[1].Value = model.Password;
            parameters[2].Value = model.TrueName;
            parameters[3].Value = model.Age;
            parameters[4].Value = model.Sex;
            parameters[5].Value = model.Phone;
            parameters[6].Value = model.Email;
            parameters[7].Value = model.UserType;
            parameters[8].Value = model.SysUserId;
            parameters[9].Value = model.UserID;

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
        public bool Delete(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Poll_Users ");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)
			};
            parameters[0].Value = UserID;

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
        public bool DeleteList(string UserIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Poll_Users ");
            strSql.Append(" where UserID in (" + UserIDlist + ")  ");
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
        public YSWL.MALL.Model.Poll.PollUsers GetModel(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserID,UserName,Password,TrueName,Age,Sex,Phone,Email,UserType,SysUserId from Poll_Users ");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)
			};
            parameters[0].Value = UserID;

            YSWL.MALL.Model.Poll.PollUsers model = new YSWL.MALL.Model.Poll.PollUsers();
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
        public YSWL.MALL.Model.Poll.PollUsers DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Poll.PollUsers model = new YSWL.MALL.Model.Poll.PollUsers();
            if (row != null)
            {
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(row["UserID"].ToString());
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["Password"] != null && row["Password"].ToString() != "")
                {
                    model.Password = (byte[])row["Password"];
                }
                if (row["TrueName"] != null)
                {
                    model.TrueName = row["TrueName"].ToString();
                }
                if (row["Age"] != null && row["Age"].ToString() != "")
                {
                    model.Age = int.Parse(row["Age"].ToString());
                }
                if (row["Sex"] != null)
                {
                    model.Sex = row["Sex"].ToString();
                }
                if (row["Phone"] != null)
                {
                    model.Phone = row["Phone"].ToString();
                }
                if (row["Email"] != null)
                {
                    model.Email = row["Email"].ToString();
                }
                if (row["UserType"] != null)
                {
                    model.UserType = row["UserType"].ToString();
                }
                if (row["SysUserId"] != null && row["SysUserId"].ToString() != "")
                {
                    model.SysUserId = int.Parse(row["SysUserId"].ToString());
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
            strSql.Append("select UserID,UserName,Password,TrueName,Age,Sex,Phone,Email,UserType,SysUserId ");
            strSql.Append(" FROM Poll_Users ");
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
            strSql.Append(" UserID,UserName,Password,TrueName,Age,Sex,Phone,Email,UserType,SysUserId ");
            strSql.Append(" FROM Poll_Users ");
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
            strSql.Append("select count(1) FROM Poll_Users ");
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
                strSql.Append("order by T.UserID desc");
            }
            strSql.Append(")AS Row, T.*  from Poll_Users T ");
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
            parameters[0].Value = "Poll_Users";
            parameters[1].Value = "UserID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod


        /// <summary>
        /// 是否存在该记录（系统中的用户）
        /// </summary>
        /// <param name="UserId">系统用户UserID</param>
        /// <returns></returns>
        public bool ExistsSysUser(int UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Poll_Users");
            strSql.Append(" where SysUserId=@SysUserId");
            SqlParameter[] parameters = {
					new SqlParameter("@SysUserId", SqlDbType.Int,4)
			};
            parameters[0].Value = UserId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }
       
    }
}

