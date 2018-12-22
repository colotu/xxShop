
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Members;
using YSWL.DBUtility;
using YSWL.MALL.Model.Shop.Order;
namespace YSWL.MALL.SQLServerDAL.Members
{
    /// <summary>
    /// 数据访问类:Users
    /// </summary>
    public partial class Users : IUsers
    {
        public Users()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("UserID", "Accounts_Users");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists( int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_Users");
            strSql.Append(" where   UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)			};
 
            parameters[0].Value = UserID;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.MALL.Model.Members.Users model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_Users(");
            strSql.Append("UserName,Password,NickName,TrueName,Sex,Phone,Email,EmployeeID,DepartmentID,Activity,UserType,Style,User_iCreator,User_dateCreate,User_dateValid,User_dateExpire,User_iApprover,User_dateApprove,User_iApproveState,User_cLang)");
            strSql.Append(" values (");
            strSql.Append("@UserName,@Password,@NickName,@TrueName,@Sex,@Phone,@Email,@EmployeeID,@DepartmentID,@Activity,@UserType,@Style,@User_iCreator,@User_dateCreate,@User_dateValid,@User_dateExpire,@User_iApprover,@User_dateApprove,@User_iApproveState,@User_cLang)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,100),
					new SqlParameter("@Password", SqlDbType.Binary,20),
					new SqlParameter("@NickName", SqlDbType.NVarChar,50),
					new SqlParameter("@TrueName", SqlDbType.NVarChar,50),
					new SqlParameter("@Sex", SqlDbType.Char,10),
					new SqlParameter("@Phone", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,100),
					new SqlParameter("@EmployeeID", SqlDbType.Int,4),
					new SqlParameter("@DepartmentID", SqlDbType.NVarChar,50),
					new SqlParameter("@Activity", SqlDbType.Bit,1),
					new SqlParameter("@UserType", SqlDbType.Char,2),
					new SqlParameter("@Style", SqlDbType.Int,4),
					new SqlParameter("@User_iCreator", SqlDbType.Int,4),
					new SqlParameter("@User_dateCreate", SqlDbType.DateTime),
					new SqlParameter("@User_dateValid", SqlDbType.DateTime),
					new SqlParameter("@User_dateExpire", SqlDbType.DateTime),
					new SqlParameter("@User_iApprover", SqlDbType.Int,4),
					new SqlParameter("@User_dateApprove", SqlDbType.DateTime),
					new SqlParameter("@User_iApproveState", SqlDbType.Int,4),
					new SqlParameter("@User_cLang", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.UserName;
            parameters[1].Value = model.Password;
            parameters[2].Value = model.NickName;
            parameters[3].Value = model.TrueName;
            parameters[4].Value = model.Sex;
            parameters[5].Value = model.Phone;
            parameters[6].Value = model.Email;
            parameters[7].Value = model.EmployeeID;
            parameters[8].Value = model.DepartmentID;
            parameters[9].Value = model.Activity;
            parameters[10].Value = model.UserType;
            parameters[11].Value = model.Style;
            parameters[12].Value = model.User_iCreator;
            parameters[13].Value = model.User_dateCreate;
            parameters[14].Value = model.User_dateValid;
            parameters[15].Value = model.User_dateExpire;
            parameters[16].Value = model.User_iApprover;
            parameters[17].Value = model.User_dateApprove;
            parameters[18].Value = model.User_iApproveState;
            parameters[19].Value = model.User_cLang;

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
        public bool Update(YSWL.MALL.Model.Members.Users model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_Users set ");
            strSql.Append("Password=@Password,");
            strSql.Append("NickName=@NickName,");
            strSql.Append("TrueName=@TrueName,");
            strSql.Append("Sex=@Sex,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("Email=@Email,");
            strSql.Append("EmployeeID=@EmployeeID,");
            strSql.Append("DepartmentID=@DepartmentID,");
            strSql.Append("Activity=@Activity,");
            strSql.Append("UserType=@UserType,");
            strSql.Append("Style=@Style,");
            strSql.Append("User_iCreator=@User_iCreator,");
            strSql.Append("User_dateCreate=@User_dateCreate,");
            strSql.Append("User_dateValid=@User_dateValid,");
            strSql.Append("User_dateExpire=@User_dateExpire,");
            strSql.Append("User_iApprover=@User_iApprover,");
            strSql.Append("User_dateApprove=@User_dateApprove,");
            strSql.Append("User_iApproveState=@User_iApproveState,");
            strSql.Append("User_cLang=@User_cLang");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@Password", SqlDbType.Binary,20),
					new SqlParameter("@NickName", SqlDbType.NVarChar,50),
					new SqlParameter("@TrueName", SqlDbType.NVarChar,50),
					new SqlParameter("@Sex", SqlDbType.Char,10),
					new SqlParameter("@Phone", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,100),
					new SqlParameter("@EmployeeID", SqlDbType.Int,4),
					new SqlParameter("@DepartmentID", SqlDbType.NVarChar,50),
					new SqlParameter("@Activity", SqlDbType.Bit,1),
					new SqlParameter("@UserType", SqlDbType.Char,2),
					new SqlParameter("@Style", SqlDbType.Int,4),
					new SqlParameter("@User_iCreator", SqlDbType.Int,4),
					new SqlParameter("@User_dateCreate", SqlDbType.DateTime),
					new SqlParameter("@User_dateValid", SqlDbType.DateTime),
					new SqlParameter("@User_dateExpire", SqlDbType.DateTime),
					new SqlParameter("@User_iApprover", SqlDbType.Int,4),
					new SqlParameter("@User_dateApprove", SqlDbType.DateTime),
					new SqlParameter("@User_iApproveState", SqlDbType.Int,4),
					new SqlParameter("@User_cLang", SqlDbType.NVarChar,50),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.Password;
            parameters[1].Value = model.NickName;
            parameters[2].Value = model.TrueName;
            parameters[3].Value = model.Sex;
            parameters[4].Value = model.Phone;
            parameters[5].Value = model.Email;
            parameters[6].Value = model.EmployeeID;
            parameters[7].Value = model.DepartmentID;
            parameters[8].Value = model.Activity;
            parameters[9].Value = model.UserType;
            parameters[10].Value = model.Style;
            parameters[11].Value = model.User_iCreator;
            parameters[12].Value = model.User_dateCreate;
            parameters[13].Value = model.User_dateValid;
            parameters[14].Value = model.User_dateExpire;
            parameters[15].Value = model.User_iApprover;
            parameters[16].Value = model.User_dateApprove;
            parameters[17].Value = model.User_iApproveState;
            parameters[18].Value = model.User_cLang;
            parameters[19].Value = model.UserID;
            parameters[20].Value = model.UserName;
            try
            {
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
            catch (Exception)
            {
                throw;
            }
          
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_Users ");
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(string UserName, int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_Users ");
            strSql.Append(" where UserName=@UserName and UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,100),
					new SqlParameter("@UserID", SqlDbType.Int,4)			};
            parameters[0].Value = UserName;
            parameters[1].Value = UserID;

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
            strSql.Append("delete from Accounts_Users ");
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
        public YSWL.MALL.Model.Members.Users GetModel(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserID,UserName,Password,NickName,TrueName,Sex,Phone,Email,EmployeeID,DepartmentID,Activity,UserType,Style,User_iCreator,User_dateCreate,User_dateValid,User_dateExpire,User_iApprover,User_dateApprove,User_iApproveState,User_cLang from Accounts_Users ");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)
			};
            parameters[0].Value = UserID;

            YSWL.MALL.Model.Members.Users model = new YSWL.MALL.Model.Members.Users();
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
        public YSWL.MALL.Model.Members.Users DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Members.Users model = new YSWL.MALL.Model.Members.Users();
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
                if (row["NickName"] != null)
                {
                    model.NickName = row["NickName"].ToString();
                }
                if (row["TrueName"] != null)
                {
                    model.TrueName = row["TrueName"].ToString();
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
                if (row["EmployeeID"] != null && row["EmployeeID"].ToString() != "")
                {
                    model.EmployeeID = int.Parse(row["EmployeeID"].ToString());
                }
                if (row["DepartmentID"] != null)
                {
                    model.DepartmentID = row["DepartmentID"].ToString();
                }
                if (row["Activity"] != null && row["Activity"].ToString() != "")
                {
                    if ((row["Activity"].ToString() == "1") || (row["Activity"].ToString().ToLower() == "true"))
                    {
                        model.Activity = true;
                    }
                    else
                    {
                        model.Activity = false;
                    }
                }
                if (row["UserType"] != null)
                {
                    model.UserType = row["UserType"].ToString();
                }
                if (row["Style"] != null && row["Style"].ToString() != "")
                {
                    model.Style = int.Parse(row["Style"].ToString());
                }
                if (row["User_iCreator"] != null && row["User_iCreator"].ToString() != "")
                {
                    model.User_iCreator = int.Parse(row["User_iCreator"].ToString());
                }
                if (row["User_dateCreate"] != null && row["User_dateCreate"].ToString() != "")
                {
                    model.User_dateCreate = DateTime.Parse(row["User_dateCreate"].ToString());
                }
                if (row["User_dateValid"] != null && row["User_dateValid"].ToString() != "")
                {
                    model.User_dateValid = DateTime.Parse(row["User_dateValid"].ToString());
                }
                if (row["User_dateExpire"] != null && row["User_dateExpire"].ToString() != "")
                {
                    model.User_dateExpire = DateTime.Parse(row["User_dateExpire"].ToString());
                }
                if (row["User_iApprover"] != null && row["User_iApprover"].ToString() != "")
                {
                    model.User_iApprover = int.Parse(row["User_iApprover"].ToString());
                }
                if (row["User_dateApprove"] != null && row["User_dateApprove"].ToString() != "")
                {
                    model.User_dateApprove = DateTime.Parse(row["User_dateApprove"].ToString());
                }
                if (row["User_iApproveState"] != null && row["User_iApproveState"].ToString() != "")
                {
                    model.User_iApproveState = int.Parse(row["User_iApproveState"].ToString());
                }
                if (row["User_cLang"] != null)
                {
                    model.User_cLang = row["User_cLang"].ToString();
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
            strSql.Append("select UserID,UserName,Password,NickName,TrueName,Sex,Phone,Email,EmployeeID,DepartmentID,Activity,UserType,Style,User_iCreator,User_dateCreate,User_dateValid,User_dateExpire,User_iApprover,User_dateApprove,User_iApproveState,User_cLang ");
            strSql.Append(" FROM Accounts_Users ");
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
            strSql.Append(" UserID,UserName,Password,NickName,TrueName,Sex,Phone,Email,EmployeeID,DepartmentID,Activity,UserType,Style,User_iCreator,User_dateCreate,User_dateValid,User_dateExpire,User_iApprover,User_dateApprove,User_iApproveState,User_cLang ");
            strSql.Append(" FROM Accounts_Users ");
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
            strSql.Append("select count(1) FROM Accounts_Users ");
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
            strSql.Append(")AS Row, T.*  from Accounts_Users T ");
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
            parameters[0].Value = "Accounts_Users";
            parameters[1].Value = "UserID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod

        #region MethodEx
        /// <summary>
        /// 根据DepartmentID删除一条数据
        /// </summary>
        public bool DeleteByDepartmentID(int DepartmentID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_Users ");
            strSql.Append(" where DepartmentID=@DepartmentID");
            SqlParameter[] parameters = {
					new SqlParameter("@DepartmentID", SqlDbType.Int,4)
			};
            parameters[0].Value = DepartmentID;

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
        /// 根据DepartmentID批量删除数据
        /// </summary>
        public bool DeleteListByDepartmentID(string DepartmentIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_Users ");
            strSql.Append(" where DepartmentID in (" + DepartmentIDlist + ")  ");
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
        /// 根据UsersID批量删除数据
        /// </summary>
        public bool DeleteUserListByUserID(string serInuserid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_Users ");
            strSql.Append(" where UserID in (" + serInuserid + ")  ");
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
        /// 判断电话是否一件存在
        /// </summary>
        public bool ExistByPhone(string Phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from Accounts_Users ");
            strSql.Append(" where Phone=@Phone ");
            SqlParameter[] parameters = {
					new SqlParameter("@Phone", SqlDbType.NVarChar)

			};
            parameters[0].Value = Phone;

            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 根据用户邮箱判断是否存在该记录
        /// </summary>
        public bool ExistsByEmail(string Email)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 *  from Accounts_Users ");
            strSql.Append(" where Email=@Email");
            SqlParameter[] parameters = {
					new SqlParameter("@Email", SqlDbType.NVarChar)
			};
            parameters[0].Value = Email;
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;


            }
        }

        /// <summary>
        ///根据用户输入的昵称是否存在
        /// </summary>
        public bool ExistsNickName(string nickname)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_Users");
            strSql.Append(" where NickName=@NickName");
            SqlParameter[] parameters = {
                    new SqlParameter("@NickName", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = nickname;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }
        #endregion

        /// <summary>
        ///根据用户ID判断昵称是否已被其他用户使用
        /// </summary>
        public bool ExistsNickName(int userid, string nickname)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_Users");
            strSql.Append(" where UserID<>@UserID AND NickName=@NickName");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@NickName", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = userid;
            parameters[1].Value = nickname;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

        public DataSet GetList(string type, string keyWord)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Accounts_Users ");
            StringBuilder strWhere = new StringBuilder();
            strSql.Append(" WHERE Activity=1 ");
            if (!string.IsNullOrWhiteSpace(type))
            {
                strSql.Append(" AND UserType=" + type);
            }
            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                //过滤SQL注入
                strSql.AppendFormat(" AND UserName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(keyWord));
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        //联合查询用户表和用户附件表(普通用户)
        public DataSet GetListEX(string keyWord = "")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users inner join Accounts_UsersExp on Accounts_UsersExp.UserID=Accounts_Users.UserID");
            strSql.Append(" WHERE UserType='UU'");
            if (!string.IsNullOrWhiteSpace(keyWord))
            {
                //过滤SQL注入
               
                strSql.AppendFormat(" AND UserName LIKE '%{0}%' ",Common.InjectionFilter.SqlFilter(keyWord));
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        //联合查询用户表和用户附件表
        public DataSet GetListEXByType(string type, string keyWord = "")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users inner join Accounts_UsersExp on Accounts_UsersExp.UserID=Accounts_Users.UserID");
            StringBuilder strWhere = new StringBuilder();

            if (!string.IsNullOrEmpty(type))
            {
                if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                {
                    strWhere.Append(" AND ");
                }
                strWhere.Append("  UserType='" + Common.InjectionFilter.SqlFilter(type) + "'");
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                if (!String.IsNullOrWhiteSpace(strWhere.ToString()))
                {
                    strWhere.Append(" AND ");
                }
                strWhere.AppendFormat("  UserName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(keyWord));
            }
            strSql.Append(" WHERE   " + strWhere.ToString());
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }



        //联合查询用户表和用户附件表
        public DataSet GetSearchList(string type, string StrWhere = "")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users left join Accounts_UsersExp on Accounts_UsersExp.UserID=Accounts_Users.UserID");
            StringBuilder strWhere2 = new StringBuilder();
            if (!string.IsNullOrEmpty(type))
            {
                if (!String.IsNullOrWhiteSpace(strWhere2.ToString()))
                {
                    strWhere2.Append(" AND ");
                }
                strWhere2.Append("  UserType='" + Common.InjectionFilter.SqlFilter(type) + "'");
            }
            else
            {
                strWhere2.Append(" 1=1 ");
            }
            if (!string.IsNullOrEmpty(StrWhere))
            {
                if (!String.IsNullOrWhiteSpace(strWhere2.ToString()))
                {
                    strWhere2.Append(" AND ");
                }
                strWhere2.Append(StrWhere);
            }
            strSql.Append(" WHERE   " + strWhere2.ToString());
            strSql.Append(" order by  User_dateCreate desc");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public int GetUserIdByNickName(string NickName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserID FROM Accounts_Users ");
            if (NickName.Trim() != "")
            {
                strSql.Append(" where NickName=@NickName");
            }
            SqlParameter[] parameters = {
					new SqlParameter("@NickName", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = NickName;
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


        public string GetUserName(int UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserName FROM Accounts_Users ");
            strSql.Append(" where UserId=" + UserId);
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
        /// 一键更新用户的粉丝数和关注数
        /// </summary>
        /// <returns></returns>
        public bool UpdateFansAndFellowCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_UsersExp SET FansCount=(SELECT COUNT(1) FROM SNS_UserShip us WHERE Accounts_UsersExp.UserID=us.PassiveUserID),FellowCount=(SELECT COUNT(1) FROM SNS_UserShip us WHERE Accounts_UsersExp.UserID=us.ActiveUserID)");
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
        /// 对用户进行批量冻结和解冻
        /// </summary>
        /// <param name="Ids">用户的id集合</param>
        /// <param name="ActiveType">冻结或冻结</param>
        /// <returns></returns>
        public bool UpdateActiveStatus(string Ids, int ActiveType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_Users SET Activity=" + ActiveType + " Where UserID in(" + Ids + ")");
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
        public int GetUserIdByDepartmentID(string DepartmentID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserID FROM Accounts_Users ");
            strSql.Append(" where DepartmentID=@DepartmentID");
            SqlParameter[] parameters = {
					new SqlParameter("@DepartmentID", SqlDbType.NVarChar,15),
			};
            parameters[0].Value = DepartmentID;
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

        public int GetDefaultUserId()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MIN(UserID) from Accounts_Users   where  Activity=1 and UserType='UU'");
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            return obj == null ? 0 : Convert.ToInt32(obj);
        }



        public string GetNickName(int UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NickName FROM Accounts_Users ");
            strSql.Append(" where UserId=" + UserId);
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


        public bool DeleteEx(int userId)
        {

            //事务处理
            List<CommandInfo> sqllist = new List<CommandInfo>();

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from Accounts_UserRoles ");
            strSql2.Append(" where UserID=@UserID  ");
            SqlParameter[] parameters2 = {
						new SqlParameter("@UserID", SqlDbType.Int,4)
                                         };
            parameters2[0].Value = userId;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_Users ");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)
			};
            parameters[0].Value = userId;
             cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

         

            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("delete from Accounts_UsersExp ");
            strSql1.Append(" where UserID=@UserID  ");
            SqlParameter[] parameters1 = {
						new SqlParameter("@UserID", SqlDbType.Int,4)
                                         };
            parameters1[0].Value = userId;
            cmd= new CommandInfo(strSql1.ToString(), parameters1);
            sqllist.Add(cmd);

     

            //StringBuilder strSql3 = new StringBuilder();
            //strSql3.Append("alter table test2 drop constraint FK__test2__id__08EA5793 ");
            //strSql3.Append(" where UserID=@UserID  ");
         
            //cmd = new CommandInfo(strSql2.ToString(), parameters2);
            //sqllist.Add(cmd);
            

            return DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist) > 0 ? true : false;


        }

        public YSWL.MALL.Model.Members.Users GetModel(string userName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserID,UserName,NickName,Password,TrueName,Sex,Phone,Email,EmployeeID,DepartmentID,Activity,UserType,Style,User_iCreator,User_dateCreate,User_dateValid,User_dateExpire,User_iApprover,User_dateApprove,User_iApproveState,User_cLang from Accounts_Users ");
            strSql.Append(" where UserName=@UserName");
            SqlParameter[] parameters = {
					new SqlParameter("@UserName", SqlDbType.NVarChar,200)
			};
            parameters[0].Value = userName;

            YSWL.MALL.Model.Members.Users model = new YSWL.MALL.Model.Members.Users();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["UserID"] != null && ds.Tables[0].Rows[0]["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(ds.Tables[0].Rows[0]["UserID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserName"] != null && ds.Tables[0].Rows[0]["UserName"].ToString() != "")
                {
                    model.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Password"] != null && ds.Tables[0].Rows[0]["Password"].ToString() != "")
                {
                    model.Password = (byte[])ds.Tables[0].Rows[0]["Password"];
                }
                if (ds.Tables[0].Rows[0]["TrueName"] != null && ds.Tables[0].Rows[0]["TrueName"].ToString() != "")
                {
                    model.TrueName = ds.Tables[0].Rows[0]["TrueName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["NickName"] != null && ds.Tables[0].Rows[0]["NickName"].ToString() != "")
                {
                    model.NickName = ds.Tables[0].Rows[0]["NickName"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Sex"] != null && ds.Tables[0].Rows[0]["Sex"].ToString() != "")
                {
                    model.Sex = ds.Tables[0].Rows[0]["Sex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Phone"] != null && ds.Tables[0].Rows[0]["Phone"].ToString() != "")
                {
                    model.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Email"] != null && ds.Tables[0].Rows[0]["Email"].ToString() != "")
                {
                    model.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                }
                if (ds.Tables[0].Rows[0]["EmployeeID"] != null && ds.Tables[0].Rows[0]["EmployeeID"].ToString() != "")
                {
                    model.EmployeeID = int.Parse(ds.Tables[0].Rows[0]["EmployeeID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DepartmentID"] != null && ds.Tables[0].Rows[0]["DepartmentID"].ToString() != "")
                {
                    model.DepartmentID = ds.Tables[0].Rows[0]["DepartmentID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Activity"] != null && ds.Tables[0].Rows[0]["Activity"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Activity"].ToString() == "1") || (ds.Tables[0].Rows[0]["Activity"].ToString().ToLower() == "true"))
                    {
                        model.Activity = true;
                    }
                    else
                    {
                        model.Activity = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["UserType"] != null && ds.Tables[0].Rows[0]["UserType"].ToString() != "")
                {
                    model.UserType = ds.Tables[0].Rows[0]["UserType"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Style"] != null && ds.Tables[0].Rows[0]["Style"].ToString() != "")
                {
                    model.Style = int.Parse(ds.Tables[0].Rows[0]["Style"].ToString());
                }
                if (ds.Tables[0].Rows[0]["User_iCreator"] != null && ds.Tables[0].Rows[0]["User_iCreator"].ToString() != "")
                {
                    model.User_iCreator = int.Parse(ds.Tables[0].Rows[0]["User_iCreator"].ToString());
                }
                if (ds.Tables[0].Rows[0]["User_dateCreate"] != null && ds.Tables[0].Rows[0]["User_dateCreate"].ToString() != "")
                {
                    model.User_dateCreate = DateTime.Parse(ds.Tables[0].Rows[0]["User_dateCreate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["User_dateValid"] != null && ds.Tables[0].Rows[0]["User_dateValid"].ToString() != "")
                {
                    model.User_dateValid = DateTime.Parse(ds.Tables[0].Rows[0]["User_dateValid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["User_dateExpire"] != null && ds.Tables[0].Rows[0]["User_dateExpire"].ToString() != "")
                {
                    model.User_dateExpire = DateTime.Parse(ds.Tables[0].Rows[0]["User_dateExpire"].ToString());
                }
                if (ds.Tables[0].Rows[0]["User_iApprover"] != null && ds.Tables[0].Rows[0]["User_iApprover"].ToString() != "")
                {
                    model.User_iApprover = int.Parse(ds.Tables[0].Rows[0]["User_iApprover"].ToString());
                }
                if (ds.Tables[0].Rows[0]["User_dateApprove"] != null && ds.Tables[0].Rows[0]["User_dateApprove"].ToString() != "")
                {
                    model.User_dateApprove = DateTime.Parse(ds.Tables[0].Rows[0]["User_dateApprove"].ToString());
                }
                if (ds.Tables[0].Rows[0]["User_iApproveState"] != null && ds.Tables[0].Rows[0]["User_iApproveState"].ToString() != "")
                {
                    model.User_iApproveState = int.Parse(ds.Tables[0].Rows[0]["User_iApproveState"].ToString());
                }
                if (ds.Tables[0].Rows[0]["User_cLang"] != null && ds.Tables[0].Rows[0]["User_cLang"].ToString() != "")
                {
                    model.User_cLang = ds.Tables[0].Rows[0]["User_cLang"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        public DataSet GetUserCount(StatisticMode mode, DateTime startDate, DateTime endDate)
        {

            int subLength = 8;
            string method;
            switch (mode)
            {
                case StatisticMode.Year:
                    subLength = 4;
                    method = "GET_GeneratedYear";
                    break;
                case StatisticMode.Month:
                    subLength = 6;
                    method = "GET_GeneratedMonth";
                    break;
                case StatisticMode.Day:
                    subLength = 8;
                    method = "GET_GeneratedDay";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
           @"
--用户统计走势图
SELECT  A.GeneratedDate AS GeneratedDate
        ,UserID as Users
FROM    ( SELECT    *
          FROM      {0}(@StartDate, @EndDate)
        ) A
        LEFT JOIN ( SELECT  CONVERT(varchar({1}) , U.User_dateCreate, 112 ) GeneratedDate
                         ,count(UserID) UserID
                    FROM    Accounts_Users U ", method, subLength);

            strSql.AppendFormat(@" 
                          where U.User_dateCreate BETWEEN @StartDate AND @EndDate 
                    GROUP BY CONVERT(varchar({0}) , U.User_dateCreate, 112 )
                  ) B 
ON CONVERT(varchar({0}) , A.GeneratedDate, 112 ) = CONVERT(varchar({0}) , B.GeneratedDate, 112 ) 
", subLength);
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime)
            };
            parameters[0].Value = startDate;
            parameters[1].Value = endDate;

            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);

        }


        /// <summary>
        ///根据用户ID判断昵称是否已被其他用户使用
        /// </summary>
        public bool ExistsUserName(int userid, string username)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_Users");
            strSql.Append(" where UserID<>@UserID AND UserName=@UserName");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@UserName", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = userid;
            parameters[1].Value = username;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 批量绑定业务员
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="salesId"></param>
        /// <returns></returns>
        public bool UpdateSales(string userIds, int salesId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update   Accounts_Users set EmployeeID= " + salesId);
            strSql.Append(" where UserID in (" + userIds + ")  ");
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
        /// 用户注册统计--日期
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet GetDayRegCount(DateTime startDate, DateTime endDate,StatisticMode mode)
        {
            StringBuilder strSql = new StringBuilder();
            switch (mode)
            {
                case StatisticMode.Month:
                    var startMonth = new DateTime(startDate.Year, startDate.Month, 1);
                    var endMonth = new DateTime(startDate.Year, endDate.Month, 1).AddMonths(1);
             strSql.Append(@"select CONVERT(varchar(7),User_dateCreate,111) D,COUNT(*) UserCount from Accounts_Users ");
             strSql.AppendFormat(" where UserType='UU'  and   User_dateCreate BETWEEN '{0}' AND '{1}'", startMonth, endMonth);
            strSql.AppendFormat(" group by CONVERT(varchar(7),User_dateCreate,111) order by d");
                    break;
                case StatisticMode.Day:
             strSql.Append(@"select CONVERT(varchar(12),User_dateCreate,111) D,COUNT(*) UserCount from Accounts_Users ");
             strSql.AppendFormat(" where UserType='UU'  and   User_dateCreate BETWEEN '{0}' AND '{1}'", startDate, endDate);
            strSql.AppendFormat(" group by CONVERT(varchar(12),User_dateCreate,111)");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        
         
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }


        public DataSet GetCustList(int UserId, int IsAct = -1, string KeyWord = "",string startDate="")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM Accounts_Users");
            strSql.AppendFormat(" where  UserType='UU' and  EmployeeID={0} ", UserId);
            if (!String.IsNullOrWhiteSpace(KeyWord))
            {
                strSql.AppendFormat(" and ( UserName like '%{0}%'   or  NickName like '%{0}%'  ) ", Common.InjectionFilter.SqlFilter(KeyWord));
            }
            if (IsAct == 0)
            {
                strSql.AppendFormat(" and  not EXISTS(select  *  from  OMS_Orders  S where  OrderType=1 and OrderStatus<>-1  and Accounts_Users.UserID=S.BuyerID and CreatedDate>='{0}'  )", startDate);
            }
            if (IsAct == 1)
            {
                strSql.AppendFormat(" and   EXISTS(select  *  from    OMS_Orders S where  OrderType=1 and OrderStatus<>-1  and Accounts_Users.UserID=S.BuyerID and CreatedDate>='{0}' )", startDate);
            }
            strSql.Append("  ORDER BY User_dateCreate desc ");

            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="EmployeeID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public bool Exists(int EmployeeID, int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_Users");
            strSql.Append(" where UserID=@UserID and  EmployeeID=@EmployeeID");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@EmployeeID", SqlDbType.Int,4)
			};
            parameters[0].Value = UserID;
            parameters[1].Value = EmployeeID;
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        public DataSet SalesRegisters(DateTime startDay, DateTime endDay)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  EmployeeID,COUNT(1) As Count FROM    Accounts_Users ");
            strSql.AppendFormat(" WHERE   Activity = 1  AND UserType = 'UU' AND User_dateCreate > '{0}' AND User_dateCreate < '{1}'", startDay, endDay);
            strSql.Append(" GROUP BY EmployeeID");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }


        public int GetSalesRegs(int SalesId, string startDay, string endDay)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT   COUNT(1)  FROM    Accounts_Users ");
            strSql.AppendFormat(" WHERE   Activity = 1  AND UserType = 'UU' AND User_dateCreate > '{0}' AND User_dateCreate < '{1}' ", startDay, endDay);
            strSql.AppendFormat("  And  EmployeeID={0}", SalesId);
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
        /// 
        /// </summary>
        /// <param name="SalesId"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        public DataSet GetSalesRegList(int SalesId, string startDate, string endDate, int dateType = 0)
        {
            int length = 12;
            switch (dateType)
            {
                case 0:
                    length = 12;
                    break;
                case 1:
                    length = 7;
                    break;
                default:
                    length = 12;
                    break;

            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"select CONVERT(varchar({0}),User_dateCreate,111) D,COUNT(*) UserCount from Accounts_Users ", length);
            strSql.Append(" where UserType='UU'   ");
            if (!String.IsNullOrWhiteSpace(startDate) && YSWL.Common.PageValidate.IsDateTime(startDate))
            {
                strSql.AppendFormat("   AND User_dateCreate >= '{0}'  ", startDate);
            }
            if (!String.IsNullOrWhiteSpace(endDate) && YSWL.Common.PageValidate.IsDateTime(endDate))
            {
                strSql.AppendFormat("  AND User_dateCreate <= '{0}'  ", endDate);
            }
            strSql.AppendFormat("  And  EmployeeID={0}", SalesId);
            strSql.AppendFormat(" group by CONVERT(varchar({0}),User_dateCreate,111)  order by D desc", length);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 根据地区或加盟商找商家
        /// </summary>
        /// <param name="regionId">地区Id</param>
        /// <param name="supplierId">加盟商Id</param>
        /// <returns></returns>
        public DataSet GetShopByRegion(int regionId,int supplierId)
        {
            DataSet ds;
            string strWhere =
                "  select s.UserId,s.Latitude,s.Longitude,s.RegionId ,r.ShopPhoto,u.UserName,u.NickName,u.Phone from Shop_ShippingAddress s inner join Accounts_Users u on s.UserId=u.UserId     left join ERP_UsersExp r on r.UserId=u.UserId";
            if (regionId > 0&&supplierId<1)//仅选择了地区
            {
                //strWhere += string.Format(" where  (s.RegionId in ( SELECT RegionId  FROM Ms_Regions where  Path like '%,{0},%' )  or s.RegionId={0}) ",regionId);
                strWhere +=
                    string.Format(
                        "   where exists   (SELECT RegionId FROM Ms_Regions r where s.RegionId=r.RegionId And r.Path like '%,{0},%'  union all SELECT RegionId FROM Ms_Regions r where s.RegionId=r.RegionId And  s.RegionId={0})  ", regionId);
                ds = DBHelper.DefaultDBHelper.Query(strWhere);
            }
            else if (supplierId > 0 && regionId < 1)//仅选择了加盟商
            {
                strWhere +=
                    string.Format(
                        " where exists (select LineId from ERP_Lines e where s.LineId=e.LineId and e.supplierId={0})",
                        supplierId);
                ds = DBHelper.DefaultDBHelper.Query(strWhere);
            }
            else if (regionId > 0 && supplierId > 0) //既选择了加盟商又选择了地区
            {
                strWhere +=
                    string.Format(
                        "   where exists   (SELECT RegionId FROM Ms_Regions r where s.RegionId=r.RegionId And r.Path like '%,{0},%'  union all SELECT RegionId FROM Ms_Regions r where s.RegionId=r.RegionId And  s.RegionId={0})  ", regionId);
                strWhere +=
                    string.Format(
                        " and exists (select  LineId from ERP_Lines e where s.LineId=e.LineId and e.supplierId={0})",
                        supplierId);
                ds = DBHelper.DefaultDBHelper.Query(strWhere);
            }
            else//既没有选择地区也没有选择加盟商
            {
                ds = DBHelper.DefaultDBHelper.Query(strWhere);
            }
            return ds;
        }

        /// <summary>
        /// 获取邀请用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Members.Users GetInviteUser(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 *  from Accounts_Users  WHERE Activity=1 AND EXISTS(");
            strSql.Append(" SELECT *  FROM Accounts_UserInvite WHERE UserId=@UserID AND Status=1 AND Accounts_Users.UserID=InviteUserId)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)
			};
            parameters[0].Value = userId;

            YSWL.MALL.Model.Members.Users model = new YSWL.MALL.Model.Members.Users();
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
        
        #region  设置业务员

        public bool SetSalesInfo(int salesId, string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("UPDATE Accounts_Users SET EmployeeID={0} Where UserID in ({1})", salesId, idlist);
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
        #endregion

        /// <summary>
        /// 通过用户名获得用户的userid zhou20160104xiugai
        /// </summary>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public int GetUserIdByUserName(string UserName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserID FROM Accounts_Users ");
            if (UserName.Trim() != "")
            {
                strSql.Append(" where UserName=@UserName");
            }
            else
            {
                return 0;
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@UserName", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = UserName;
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
        ///  根据USERid获取用户所属店铺用户UserID(EmployeeID表示所属店铺)
        /// </summary>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public int GetEmployeeIDByUserid(string Userid)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select EmployeeID FROM Accounts_Users  where UserID=@UserID");
                //if (Userid.Trim() != "")
                //{
                //    strSql.Append(" where UserID=@UserID");
                //}
                SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.NVarChar,50)
                 };
                parameters[0].Value = Userid;
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
            catch
            {
                return 0;
            }
        }


        public string GetUserTrueNameByUsername(string UserName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NickName FROM Accounts_Users ");
            if (UserName.Trim() != "")
            {
                strSql.Append(" where UserName=@UserName");
            }
            else
            {
                return "";
            }

            SqlParameter[] parameters = {
                    new SqlParameter("@UserName", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = UserName;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        /// 根据USERid让该用户所属店铺用户UserID(EmployeeID表示所属店铺)
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="salesId"></param>
        /// <returns></returns>
        public bool SetEmpidByUserid(string userId,string EmployeeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update  Accounts_Users set EmployeeID='"+ EmployeeID + "' ");
            strSql.Append(" where UserID in (" + userId + ")  ");
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
        ///根据用户名是否在存在//zhou20160104
        /// </summary>
        public bool ExistsUserName(string username)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_Users");
            strSql.Append(" where UserName=@UserName");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserName", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = username;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        ///根据用户名 是否是VIP
        /// </summary>
        public string ExistsUserVIP(string userid)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select BodilyForm from Accounts_UsersExp  ");
                if (userid.Trim() != "")
                {
                    strSql.Append(" where UserID=@UserID");
                }
                SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.NVarChar,50)
            };
                parameters[0].Value = userid;

                object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
                if (obj == null)
                {
                    return "";
                }
                else
                {
                    return obj.ToString();
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 升级为是VIP
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="salesId"></param>
        /// <returns></returns>
        public bool UpvipUserName(string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update  Accounts_UsersExp set BodilyForm='VIP' ");
            strSql.Append(" where UserID in (" + userId + ")  ");
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


    }
}


