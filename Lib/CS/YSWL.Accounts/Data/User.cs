using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using YSWL.Accounts.IData;
using YSWL.DBUtility;

namespace YSWL.Accounts.Data
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Serializable]
    public class User : IUser
    {
        public User()
        { }

        #region 增加用户

        /// <summary>
        /// 创建用户
        /// </summary>
        public int Create(string userName, byte[] password, string nickName, string trueName, string sex, string phone, string email, int employeeID, string departmentID, bool activity, string userType, int style, int User_iCreator, DateTime User_dateValid, string User_cLang, DateTime User_dateApprove, int User_iApproveState = 0)
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (password == null) throw new ArgumentNullException("password");

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_Users(");
            strSql.Append("UserName,Password,NickName,TrueName,Sex,Phone,Email,EmployeeID,DepartmentID,Activity,UserType,Style,User_iCreator,User_dateCreate,User_dateValid,User_dateApprove,User_iApproveState,User_cLang)");
            strSql.Append(" values (");
            strSql.Append("@UserName,@Password,@NickName,@TrueName,@Sex,@Phone,@Email,@EmployeeID,@DepartmentID,@Activity,@UserType,@Style,@User_iCreator,@User_dateCreate,@User_dateValid,@User_dateApprove,@User_iApproveState,@User_cLang)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Password", SqlDbType.Binary,20),
                    new SqlParameter("@NickName", SqlDbType.NVarChar,50),
                    new SqlParameter("@TrueName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Sex", SqlDbType.Char,10),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,20),
                    new SqlParameter("@Email", SqlDbType.NVarChar,100),
                    new SqlParameter("@EmployeeID", SqlDbType.Int,4),
                    new SqlParameter("@DepartmentID", SqlDbType.NVarChar,15),
                    new SqlParameter("@Activity", SqlDbType.Bit,1),
                    new SqlParameter("@UserType", SqlDbType.Char,2),
                    new SqlParameter("@Style", SqlDbType.Int,4),
                    new SqlParameter("@User_iCreator", SqlDbType.Int,4),
                    new SqlParameter("@User_dateCreate", SqlDbType.DateTime),
                    new SqlParameter("@User_dateValid", SqlDbType.DateTime),
                    new SqlParameter("@User_dateApprove", SqlDbType.DateTime),
                    new SqlParameter("@User_iApproveState", SqlDbType.Int,4),
                    new SqlParameter("@User_cLang", SqlDbType.NVarChar,10)};
            parameters[0].Value = userName;
            parameters[1].Value = password;
            parameters[2].Value = nickName;
            parameters[3].Value = trueName;
            parameters[4].Value = sex;
            parameters[5].Value = phone;
            parameters[6].Value = email;
            parameters[7].Value = employeeID;
            parameters[8].Value = departmentID;
            parameters[9].Value = activity ? 1 : 0;
            parameters[10].Value = userType;
            parameters[11].Value = style;
            parameters[12].Value = User_iCreator;
            parameters[13].Value = DateTime.Now;
            parameters[14].Value = DateTime.Now;
            parameters[15].Value = User_dateApprove;
            parameters[16].Value = User_iApproveState;
            parameters[17].Value = User_cLang;
            try
            {
            
                object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
                return obj == null ? 0 : Convert.ToInt32(obj);
            }
            catch (SqlException e)
            {
                if (e.Number == 2601)
                {
                    return -100;
                }
            }
            return 0;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int Create(string userName,
            byte[] password,
            string nickName,
            string trueName,
            string sex,
            string phone,
            string email,
            int employeeID,
            string departmentID,
            bool activity,
            string userType,
            int style,
            int User_iCreator,
            DateTime User_dateCreate,
            DateTime User_dateValid,
            string User_cLang
            )
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (password == null) throw new ArgumentNullException("password");

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_Users(");
            strSql.Append("UserName,Password,NickName,TrueName,Sex,Phone,Email,EmployeeID,DepartmentID,Activity,UserType,Style,User_iCreator,User_dateCreate,User_dateValid, User_cLang)");
            strSql.Append(" values (");
            strSql.Append("@UserName,@Password,@NickName,@TrueName,@Sex,@Phone,@Email,@EmployeeID,@DepartmentID,@Activity,@UserType,@Style,@User_iCreator,@User_dateCreate,@User_dateValid,@User_cLang)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[]
                parameters = {
                                 new SqlParameter("@UserName", SqlDbType.NVarChar, 100),
                                 new SqlParameter("@Password", SqlDbType.Binary, 20),
                                 new SqlParameter("@NickName", SqlDbType.NVarChar, 50),
                                 new SqlParameter("@TrueName", SqlDbType.NVarChar, 50),
                                 new SqlParameter("@Sex", SqlDbType.Char, 2),
                                 new SqlParameter("@Phone", SqlDbType.NVarChar, 20),
                                 new SqlParameter("@Email", SqlDbType.NVarChar, 100),
                                 new SqlParameter("@EmployeeID", SqlDbType.Int, 4),
                                 new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 15),
                                 new SqlParameter("@Activity", SqlDbType.Bit, 1),
                                 new SqlParameter("@UserType", SqlDbType.Char, 2),
                                 new SqlParameter("@Style", SqlDbType.Int, 4),
                                 new SqlParameter("@User_iCreator", SqlDbType.Int, 4),
                                 new SqlParameter("@User_dateCreate", SqlDbType.DateTime),
                                 new SqlParameter("@User_dateValid", SqlDbType.DateTime),
                                 new SqlParameter("@User_cLang",  SqlDbType.NVarChar, 10)
                             };

            parameters[0].Value = userName;
            parameters[1].Value = password;
            parameters[2].Value = nickName;
            parameters[3].Value = trueName;
            parameters[4].Value = sex;
            parameters[5].Value = phone;
            parameters[6].Value = email;
            parameters[7].Value = employeeID;
            parameters[8].Value = departmentID;
            parameters[9].Value = activity ? 1 : 0;
            parameters[10].Value = userType;
            parameters[11].Value = style;
            parameters[12].Value = User_iCreator;
            parameters[13].Value = User_dateCreate == DateTime.MinValue ? DateTime.Now : User_dateCreate;
            parameters[14].Value = DateTime.Now;
            parameters[15].Value = User_cLang;

            try
            {
                object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
                return obj == null ? 0 : Convert.ToInt32(obj);
            }
            catch (SqlException e)
            {
                if (e.Number == 2601)
                {
                    return -100;
                }
            }

            return (int)parameters[11].Value;
        }
        #endregion

        #region 得到用户信息
        /// <summary>
        /// 根据UserID查询用户详细信息
        /// </summary>
        public DataRow Retrieve(int userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("	 SELECT * FROM Accounts_Users WHERE UserID = @UserID ");

            SqlParameter[] parameters = { new SqlParameter("@UserID", SqlDbType.Int, 4) };
            parameters[0].Value = userID;

            using (DataSet users = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters))
            {
                if (users.Tables[0].Rows.Count > 0)
                {
                    return users.Tables[0].Rows[0];
                }
                else
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// 根据UserName查询用户详细信息
        /// </summary>
        public DataRow Retrieve(string userName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("	 SELECT * FROM Accounts_Users WHERE UserName = @UserName ");
            SqlParameter[] parameters = { new SqlParameter("@UserName", SqlDbType.NVarChar, 100) };
            parameters[0].Value = userName;

            using (DataSet users = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters))
            {
                if (users.Tables[0].Rows.Count == 0)
                {
                    throw new System.Security.Principal.IdentityNotMappedException("无此用户或用户已过期：" + userName);
                }
                else
                    return users.Tables[0].Rows[0];
            }
        }

        /// <summary>
        /// 根据NickName查询用户详细信息
        /// </summary>
        public DataRow RetrieveByNickName(string nickName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1) from Accounts_Users ");
            strSql.Append("where NickName = @NickName ");

            SqlParameter[] parameters = { new SqlParameter("@NickName", SqlDbType.NVarChar, 50) };
            parameters[0].Value = nickName;
            DataSet users = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (users.Tables[0].Rows.Count > 0)
            {
                return users.Tables[0].Rows[0];
            }
            return null;
        }

        #endregion

        #region 是否存在
        /// <summary>
        /// 用户名是否已经存在
        /// </summary>
        [Obsolete]
        public bool HasUser(string userName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("	 SELECT * FROM Accounts_Users WHERE UserName = @UserName ");
            SqlParameter[] parameters = { new SqlParameter("@UserName", SqlDbType.NVarChar, 100) };
            parameters[0].Value = userName;

            using (DataSet users = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters))
            {
                if (users.Tables[0].Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        /// <summary>
        /// 用户名是否已经存在
        /// </summary>
        public bool HasUserByUserName(string userName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users ");
            strSql.Append("where UserName = @UserName ");
            SqlParameter[]
                parameters = {
                    new SqlParameter("@UserName", SqlDbType.NVarChar,100)
                             };
            parameters[0].Value = userName;
            return (DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters).Tables[0].Rows.Count > 0);
        }
        /// <summary>
        /// 邮箱是否已经存在
        /// </summary>
        public bool HasUserByEmail(string email)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users ");
            strSql.Append("where Email = @Email ");
            SqlParameter[]
                parameters = {
                    new SqlParameter("@Email", SqlDbType.NVarChar,100)
                             };
            parameters[0].Value = email;
            return (DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters).Tables[0].Rows.Count > 0);
        }
        /// <summary>
        /// 昵称是否已经存在
        /// </summary>
        public bool HasUserByNickName(string nickName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users ");
            strSql.Append("where NickName = @NickName ");
            SqlParameter[]
                parameters = {
                    new SqlParameter("@NickName", SqlDbType.NVarChar,50)
                             };
            parameters[0].Value = nickName;
            return (DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters).Tables[0].Rows.Count > 0);
        }
        /// <summary>
        /// 手机是否已经存在
        /// </summary>
        public bool HasUserByPhone(string phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users ");
            strSql.Append("where Phone = @Phone ");
            SqlParameter[]
                parameters = {
                    new SqlParameter("@Phone", SqlDbType.NVarChar,20)
                             };
            parameters[0].Value = phone;
            return (DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters).Tables[0].Rows.Count > 0);
        }
        /// <summary>
        /// 手机是否已经存在
        /// </summary>
        public bool HasUserByPhone(string phone, string userType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users ");
            strSql.Append("where Phone = @Phone and UserType = @UserType");
            SqlParameter[]
                parameters = {
                    new SqlParameter("@Phone", SqlDbType.NVarChar,20),
                    new SqlParameter("@UserType", SqlDbType.Char,2)
                             };
            parameters[0].Value = phone;
            parameters[1].Value = userType;
            return (DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters).Tables[0].Rows.Count > 0);
        }
        #endregion

        #region 修改用户

        /// <summary>
        /// 更新用户状态
        /// </summary>
        public bool UpdateActivity(int userId, bool activity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_Users ");
            strSql.Append("SET Activity=@Activity ");
            strSql.Append("WHERE UserID=@UserID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@Activity", SqlDbType.Bit,1)};
            parameters[0].Value = userId;
            parameters[1].Value = activity;
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            return (rows > 0);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        public bool Update(int userID,
            string userName,
            byte[] password,
            string nickName,
            string trueName,
            string sex,
            string phone,
            string email,
            int employeeID,
            string departmentID,
            bool activity,
            string userType,
            int style
            )
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_Users SET   ");
            strSql.Append("UserName = @UserName, ");
            strSql.Append("Password = @Password, ");
            strSql.Append("NickName = @NickName, ");
            strSql.Append("TrueName = @TrueName, ");
            strSql.Append("Sex = @Sex, ");
            strSql.Append("Phone = @Phone, ");
            strSql.Append("Email = @Email, ");
            strSql.Append("EmployeeID = @EmployeeID, ");
            strSql.Append("DepartmentID = @DepartmentID, ");
            strSql.Append("Activity = @Activity,  ");
            strSql.Append("UserType = @UserType,  ");
            strSql.Append("Style=@Style  ");
            strSql.Append(" WHERE UserID = @UserID ");

            SqlParameter[] parameters = {
                                            new SqlParameter("@UserName", SqlDbType.NVarChar, 100),
                                            new SqlParameter("@Password", SqlDbType.Binary, 20),
                                            new SqlParameter("@NickName", SqlDbType.NVarChar, 50),
                                            new SqlParameter("@TrueName", SqlDbType.NVarChar, 50),
                                            new SqlParameter("@Sex", SqlDbType.Char, 2),
                                            new SqlParameter("@Phone", SqlDbType.NVarChar, 20),
                                            new SqlParameter("@Email", SqlDbType.NVarChar, 100),
                                            new SqlParameter("@EmployeeID", SqlDbType.Int, 4),
                                            new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 15),
                                            new SqlParameter("@Activity", SqlDbType.Bit, 1),
                                            new SqlParameter("@UserType", SqlDbType.Char, 2),
                                            new SqlParameter("@UserID", SqlDbType.Int, 4),
                                            new SqlParameter("@Style", SqlDbType.Int,4)

                                        };

            parameters[0].Value = userName;
            parameters[1].Value = password;
            parameters[2].Value = nickName;
            parameters[3].Value = trueName;
            parameters[4].Value = sex;
            parameters[5].Value = phone;
            parameters[6].Value = email;
            parameters[7].Value = employeeID;
            parameters[8].Value = departmentID;
            parameters[9].Value = activity;
            parameters[10].Value = userType;
            parameters[11].Value = userID;
            parameters[12].Value = style;

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
        /// 设置部门和员工编号
        /// </summary>
        public bool Update(int UserID, int EmployeeID, string DepartmentID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_Users set ");
            strSql.Append("EmployeeID=@EmployeeID,");
            strSql.Append("DepartmentID=@DepartmentID ");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@EmployeeID", SqlDbType.Int,4),
                    new SqlParameter("@DepartmentID", SqlDbType.NVarChar,15)
                };
            parameters[0].Value = UserID;
            parameters[1].Value = EmployeeID;
            parameters[2].Value = DepartmentID;
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
        /// 设置审核
        /// </summary>
        public bool Update(int UserID, int User_iApprover, int User_iApproveState)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_Users set ");
            strSql.Append("User_iApprover=@User_iApprover,");
            strSql.Append("User_dateApprove=@User_dateApprove,");
            strSql.Append("User_iApproveState=@User_iApproveState");
            strSql.Append(" where UserID=@UserID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@User_iApprover", SqlDbType.Int,4),
                    new SqlParameter("@User_dateApprove", SqlDbType.DateTime),
                    new SqlParameter("@User_iApproveState", SqlDbType.Int,4)};
            parameters[0].Value = UserID;
            parameters[1].Value = User_iApprover;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = User_iApproveState;
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
        /// 设置用户密码
        /// </summary>
        public bool SetPassword(string UserName, byte[] encPassword)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_Users SET  ");
            strSql.Append(" Password = @EncryptedPassword  ");
            strSql.Append(" WHERE UserName = @UserName  ");

            SqlParameter[] parameters =
            {
                new SqlParameter("@UserName", SqlDbType.NVarChar),
                new SqlParameter("@EncryptedPassword", SqlDbType.Binary, 20)
            };

            parameters[0].Value = UserName;
            parameters[1].Value = encPassword;

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
        #endregion

        #region 删除用户
        /// <summary>
        /// 删除用户
        /// </summary>
        public bool Delete(int userID)
        {

            List<CommandInfo> sqllist = new List<CommandInfo>();

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append(" DELETE Accounts_UserRoles ");
            strSql2.Append(" WHERE UserId = @UserId");
            SqlParameter[] parameters2 =
                {
                    new SqlParameter("@UserId", SqlDbType.BigInt, 4)
                };
            parameters2[0].Value = userID;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append(" DELETE Accounts_Users  ");
            strSql3.Append(" WHERE UserId = @UserId  ");
            SqlParameter[] parameters3 =
                {
                       new SqlParameter("@UserId", SqlDbType.BigInt, 4)
                };
            parameters3[0].Value = userID;
            cmd = new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            return rowsAffected > 0;
            
        }
        #endregion

        #region 验证登陆信息

        /// <summary>
        /// 验证用户登录信息
        /// 用户名登录
        /// </summary>
        public int ValidateLogin(string userName, byte[] encPassword)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("   SELECT  UserID   FROM    Accounts_Users   ");
            strSql.Append("    WHERE   UserName = @UserName  ");
            strSql.Append("  AND Password = @EncryptedPassword   ");
      
            SqlParameter[] parameters =       {
                new SqlParameter("@UserName", SqlDbType.NVarChar, 100),
                new SqlParameter("@EncryptedPassword", SqlDbType.Binary, 20)};

            parameters[0].Value = userName;
            parameters[1].Value = encPassword;
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            return Common.Globals.SafeInt(obj, -1);
        }

        /// <summary>
        /// 验证用户登录信息
        /// 邮箱登录
        /// </summary>
        public int ValidateLogin4Email(string email, byte[] encPassword)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("   SELECT  UserID   FROM    Accounts_Users   ");
            strSql.Append(" WHERE Email = @Email  ");
            strSql.Append("  AND Password = @EncryptedPassword   ");

            SqlParameter[] parameters =       {
                new SqlParameter("@Email", SqlDbType.NVarChar, 50),
                new SqlParameter("@EncryptedPassword", SqlDbType.Binary, 20)};

            parameters[0].Value = email;
            parameters[1].Value = encPassword;
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            return Common.Globals.SafeInt(obj, -1);
        }

        /// <summary>
        /// 测试用户密码
        /// </summary>
        public int TestPassword(int userID, byte[] encPassword)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("   SELECT  UserID FROM Accounts_Users  ");
            strSql.Append("   WHERE UserID = @UserID ");
            strSql.Append("  AND  Password = @EncryptedPassword ");
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserID", SqlDbType.Int, 4),
                new SqlParameter("@EncryptedPassword", SqlDbType.Binary, 20)
            };
            parameters[0].Value = userID;
            parameters[1].Value = encPassword;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            return Common.Globals.SafeInt(obj, 0);
         
        }


        #endregion

        #region 查询用户信息

        /// <summary>
        /// 根据关键字查询用户
        /// </summary>
        public DataSet GetUserList(string key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Accounts_Users   ");
            strSql.Append("  where UserName like '%'+@key+'%' or TrueName like '%'+@key+'%' ");
            strSql.Append("  order by UserID ");
            SqlParameter[]
                parameters = {
                                 new SqlParameter("@key", SqlDbType.NVarChar, 50)
                             };
            parameters[0].Value = key;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
     
        }
        /// <summary>
        /// 根据用户类型和关键字查询用户信息
        /// </summary>
        public DataSet GetUsersByType(string UserType, string Key)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users ");
            strSql.Append("where ");
            if (UserType.Length > 0)
            {
                strSql.Append(" UserType = @UserType ");
            }
            if (Key.Length > 0)
            {
                if (UserType.Length > 0)
                {
                    strSql.Append(" and ");
                }
                strSql.Append(" (UserName like '%'+@Key+'%' or TrueName like '%'+@Key+'%')  ");
            }
            strSql.Append(" order by UserName ");
            SqlParameter[]
                parameters = {
                    new SqlParameter("@UserType", SqlDbType.NVarChar, 50),
                    new SqlParameter("@Key", SqlDbType.NVarChar, 50)
                             };
            parameters[0].Value = UserType;
            parameters[1].Value = Key;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);

        }
        /// <summary>
        /// 根据部门和关键字查询用户信息
        /// </summary>
        public DataSet GetUsersByDepart(string DepartmentID, string Key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Accounts_Users   ");
            strSql.AppendFormat("      WHERE   DepartmentID = '{0}' ", DepartmentID);
            strSql.Append("      AND ( UserName LIKE '%' + @Key + '%'  OR TrueName LIKE '%' + @Key + '%' ) ");
            strSql.Append("  order by UserID ");

            SqlParameter[]
                parameters = {
                    new SqlParameter("@key", SqlDbType.NVarChar, 50)
                             };

            parameters[0].Value = Key;

            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据用户类型，部门，关键字查询用户
        /// </summary>
        /// <param name="UserType"></param>
        /// <param name="DepartmentID"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public DataSet GetUserList(string UserType, string DepartmentID, string Key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users ");
            strSql.Append(" where  UserType in ( @UserType )");
            strSql.Append(" and DepartmentID= @DepartmentID ");
            strSql.Append(" and (UserName like '%'+@Key+'%' or TrueName like '%'+@Key+'%')  ");
            strSql.Append(" order by UserName ");
            SqlParameter[]
                parameters = {
                    new SqlParameter("@UserType", SqlDbType.NVarChar, 50),
                    new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 30),
                    new SqlParameter("@Key", SqlDbType.NVarChar, 50)
                             };
            parameters[0].Value = UserType;
            parameters[1].Value = DepartmentID;
            parameters[2].Value = Key;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据员工编号，获取员工编号的用户信息
        /// </summary>        
        /// <param name="EmployeeID">员工编号</param>        
        /// <returns></returns>
        public DataSet GetUsersByEmp(int EmployeeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users ");
            strSql.Append("where EmployeeID= @EmployeeID ");
            SqlParameter[]
                parameters = {
                    new SqlParameter("@EmployeeID", SqlDbType.Int,4)
                             };
            parameters[0].Value = EmployeeID;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }
        #endregion

        #region 获取某角色下的所有用户
        /// <summary>
        /// 获取某角色下的所有用户
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public DataSet GetUsersByRole(int RoleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Users where UserID in ");
            strSql.Append("(select UserID from Accounts_UserRoles ");
            strSql.Append(" where RoleID= @RoleID) ");
            //strSql.Append(" ORDER BY Description ASC ");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleID", SqlDbType.Int,4)
                };
            parameters[0].Value = RoleID;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        #endregion

        #region 得到用户的角色信息
        /// <summary>
        /// 获取用户的角色信息
        /// </summary>
        [Obsolete]
        public ArrayList GetUserRoles(int userID)
        {
            ArrayList roles = new ArrayList();

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ur.RoleID, r.Description FROM Accounts_UserRoles ur  ");
            strSql.Append(" INNER JOIN Accounts_Roles r ON ur.RoleID = r.RoleID  ");
            strSql.Append("  WHERE ur.UserID = @UserID  ");

            SqlParameter[] parameters = { new SqlParameter("@UserID", SqlDbType.Int, 4) };
            parameters[0].Value = userID;

            SqlDataReader tmpReader = DBHelper.DefaultDBHelper.ExecuteReader(strSql.ToString(), parameters);
            while (tmpReader.Read())
            {
                roles.Add(tmpReader.GetString(1));
            }
            tmpReader.Close();
            return roles;
        }
        /// <summary>
        /// 获取用户的角色信息
        /// </summary>
        public Dictionary<int, string> GetUserRoles4KeyValues(int userID)
        {
            Dictionary<int, string> roles = new Dictionary<int, string>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ur.RoleID, r.Description FROM Accounts_UserRoles ur  ");
            strSql.Append(" INNER JOIN Accounts_Roles r ON ur.RoleID = r.RoleID  ");
            strSql.Append("  WHERE ur.UserID = @UserID  ");

            SqlParameter[] parameters = { new SqlParameter("@UserID", SqlDbType.Int, 4) };
            parameters[0].Value = userID;

            SqlDataReader tmpReader = DBHelper.DefaultDBHelper.ExecuteReader(strSql.ToString(), parameters);
            while (tmpReader.Read())
            {
                roles.Add(tmpReader.GetInt32(0), tmpReader.GetString(1));
            }
            tmpReader.Close();
            return roles;
        }
        #endregion

        #region 得到用户权限信息

        /// <summary>
        /// 获取用户有效的权限列表数据集
        /// </summary>
        public DataSet GetEffectivePermissionLists(int userID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT DISTINCT Accounts_Permissions.PermissionID,Accounts_Permissions.Description,Accounts_Permissions.CategoryID FROM Accounts_RolePermissions   ");
            strSql.Append(" inner join Accounts_Permissions on  Accounts_RolePermissions.PermissionID=Accounts_Permissions.PermissionID WHERE RoleID IN   ");
            strSql.Append(" (SELECT RoleID FROM Accounts_UserRoles WHERE UserID = @UserID)  ");

            SqlParameter[] parameters = { new SqlParameter("@UserID", SqlDbType.Int, 4) };
            parameters[0].Value = userID;


            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            return ds;
        }
        /// <summary>
        /// 获取用户有效的权限名称列表
        /// </summary>
        public ArrayList GetEffectivePermissionList(int userID)
        {
            ArrayList permissions = new ArrayList();

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT DISTINCT Accounts_Permissions.PermissionID,Accounts_Permissions.Description,Accounts_Permissions.CategoryID FROM Accounts_RolePermissions   ");
            strSql.Append(" inner join Accounts_Permissions on  Accounts_RolePermissions.PermissionID=Accounts_Permissions.PermissionID WHERE RoleID IN   ");
            strSql.Append(" (SELECT RoleID FROM Accounts_UserRoles WHERE UserID = @UserID)  ");

            SqlParameter[] parameters = { new SqlParameter("@UserID", SqlDbType.Int, 4) };
            parameters[0].Value = userID;

            SqlDataReader tmpReader = DBHelper.DefaultDBHelper.ExecuteReader(strSql.ToString(), parameters);
            while (tmpReader.Read())
            {
                permissions.Add(tmpReader.GetString(1));
            }
            tmpReader.Close();
            return permissions;
        }
        /// <summary>
        /// 获取用户有效的权限ID列表
        /// </summary>
        public ArrayList GetEffectivePermissionListID(int userID)
        {
            ArrayList permissionsid = new ArrayList();

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT DISTINCT PermissionID FROM Accounts_RolePermissions    ");
            strSql.Append(" where RoleID IN (SELECT RoleID FROM Accounts_UserRoles WHERE UserID = @UserID)   ");

            SqlParameter[] parameters = { new SqlParameter("@UserID", SqlDbType.Int, 4) };
            parameters[0].Value = userID;

            SqlDataReader tmpReader = DBHelper.DefaultDBHelper.ExecuteReader(strSql.ToString(), parameters);
            while (tmpReader.Read())
            {
                permissionsid.Add(tmpReader.GetInt32(0));
            }
            tmpReader.Close();
            return permissionsid;
        }
        #endregion

        #region 增加/移除 所属角色
        /// <summary>
        /// 为用户增加角色
        /// </summary>
        public bool AddRole(int userId, int roleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  delete FROM Accounts_UserRoles WHERE   RoleID = @RoleID AND UserID = @UserID ");
            strSql.Append(" INSERT INTO Accounts_UserRoles(UserID, RoleID)    ");
            strSql.Append(" 	VALUES(@UserID, @RoleID)    ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@UserID", SqlDbType.Int, 4),
                                            new SqlParameter("@RoleID", SqlDbType.Int, 4)
                                        };
            parameters[0].Value = userId;
            parameters[1].Value = roleId;
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            return rows > 0;
        }

        /// <summary>
        /// 从用户移除角色
        /// </summary>
        public bool RemoveRole(int userId, int roleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE Accounts_UserRoles   ");
            strSql.Append(" WHERE UserID = @UserID AND RoleID = @RoleID ");
            SqlParameter[] parameters = {
                                            new SqlParameter("@UserID", SqlDbType.Int, 4),
                                            new SqlParameter("@RoleID", SqlDbType.Int,4 )
                                        };
            parameters[0].Value = userId;
            parameters[1].Value = roleId;
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
        #endregion

        #region  普通管理员（有权）可以为别的用户分配的角色

        /// <summary>
        /// 要分配是否存在该记录
        /// </summary>
        public bool AssignRoleExists(int UserID, int RoleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_UserAssignmentRoles");
            strSql.Append(" where UserID= @UserID and RoleID=@RoleID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@RoleID", SqlDbType.Int,4)
                };
            parameters[0].Value = UserID;
            parameters[1].Value = RoleID;
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条关联数据
        /// </summary>
        public void AddAssignRole(int UserID, int RoleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_UserAssignmentRoles(");
            strSql.Append("UserID,RoleID)");
            strSql.Append(" values (");
            strSql.Append("@UserID,@RoleID)");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@RoleID", SqlDbType.Int,4)};
            parameters[0].Value = UserID;
            parameters[1].Value = RoleID;

            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一条关联数据
        /// </summary>
        public void DeleteAssignRole(int UserID, int RoleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete Accounts_UserAssignmentRoles ");
            strSql.Append(" where UserID= @UserID and RoleID=@RoleID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@RoleID", SqlDbType.Int,4)
                };
            parameters[0].Value = UserID;
            parameters[1].Value = RoleID;
            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获取用户分配的角色列表
        /// </summary>
        public DataSet GetAssignRolesByUser(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Roles where RoleID in ");
            strSql.Append("(select RoleID from Accounts_UserAssignmentRoles ");
            strSql.Append(" where UserID= @UserID) ");
            strSql.Append(" ORDER BY Description ASC ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4)
                };
            parameters[0].Value = UserID;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获取用户的未分配的角色列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataSet GetNoAssignRolesByUser(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Accounts_Roles where RoleID not in ");
            strSql.Append("(select RoleID from Accounts_UserAssignmentRoles ");
            strSql.Append(" where UserID= @UserID) ");
            strSql.Append(" ORDER BY Description ASC ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4)
                };
            parameters[0].Value = UserID;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int CreateAndRole(string userName,
            byte[] password,
            string nickName,
            string trueName,
            string sex,
            string phone,
            string email,
            int employeeID,
            string departmentID,
            bool activity,
            string userType,
            int style,
            int User_iCreator,
            DateTime User_dateValid,
            string User_cLang,
            int roleId
            )
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (password == null) throw new ArgumentNullException("password");

            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    object result;
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into Accounts_Users(");
                        strSql.Append("UserName,Password,NickName,TrueName,Sex,Phone,Email,EmployeeID,DepartmentID,Activity,UserType,Style,User_iCreator,User_dateCreate,User_dateValid,User_cLang)");
                        strSql.Append(" values (");
                        strSql.Append("@UserName,@Password,@NickName,@TrueName,@Sex,@Phone,@Email,@EmployeeID,@DepartmentID,@Activity,@UserType,@Style,@User_iCreator,@User_dateCreate,@User_dateValid,@User_cLang)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[]
                      parameters = {
                                 new SqlParameter("@UserName", SqlDbType.NVarChar, 100),
                                 new SqlParameter("@Password", SqlDbType.Binary, 20),
                                 new SqlParameter("@NickName", SqlDbType.NVarChar, 50),
                                 new SqlParameter("@TrueName", SqlDbType.NVarChar, 50),
                                 new SqlParameter("@Sex", SqlDbType.Char, 2),
                                 new SqlParameter("@Phone", SqlDbType.NVarChar, 20),
                                 new SqlParameter("@Email", SqlDbType.NVarChar, 100),
                                 new SqlParameter("@EmployeeID", SqlDbType.Int, 4),
                                 new SqlParameter("@DepartmentID", SqlDbType.NVarChar, 15),
                                 new SqlParameter("@Activity", SqlDbType.Bit, 1),
                                 new SqlParameter("@UserType", SqlDbType.Char, 2),
                                 new SqlParameter("@Style", SqlDbType.Int, 4),
                                 new SqlParameter("@User_iCreator", SqlDbType.Int, 4),
                                 new SqlParameter("@User_dateCreate", SqlDbType.DateTime),
                                 new SqlParameter("@User_dateValid", SqlDbType.DateTime),
                                 new SqlParameter("@User_cLang",  SqlDbType.NVarChar, 10)
                             };

                        parameters[0].Value = userName;
                        parameters[1].Value = password;
                        parameters[2].Value = nickName;
                        parameters[3].Value = trueName;
                        parameters[4].Value = sex;
                        parameters[5].Value = phone;
                        parameters[6].Value = email;
                        parameters[7].Value = employeeID;
                        parameters[8].Value = departmentID;
                        parameters[9].Value = activity ? 1 : 0;
                        parameters[10].Value = userType;
                        parameters[11].Value = style;
                        parameters[12].Value = User_iCreator;
                        parameters[13].Value = DateTime.Now;
                        parameters[14].Value = DateTime.Now;
                        parameters[15].Value = User_cLang;

                        //DONE: 新增基础用户信息
                        result = DBHelper.DefaultDBHelper.GetSingle4Trans(new CommandInfo(strSql.ToString(), parameters), transaction);
                        //加载用户
                        int userId = Common.Globals.SafeInt(result, -1);
                        if (userId == -1)
                        {
                            transaction.Rollback();
                            return userId;
                        }

                        //新增用户扩展属性信息
                        StringBuilder strSql3 = new StringBuilder();
                        strSql3.Append("insert into Accounts_UsersExp(");
                        strSql3.Append("UserID,Gravatar,Singature,TelPhone,QQ,MSN,HomePage,Birthday,BirthdayVisible,BirthdayIndexVisible,Constellation,ConstellationVisible,ConstellationIndexVisible,NativePlace,NativePlaceVisible,NativePlaceIndexVisible,RegionId,Address,AddressVisible,AddressIndexVisible,BodilyForm,BodilyFormVisible,BodilyFormIndexVisible,BloodType,BloodTypeVisible,BloodTypeIndexVisible,Marriaged,MarriagedVisible,MarriagedIndexVisible,PersonalStatus,PersonalStatusVisible,PersonalStatusIndexVisible,Grade,Balance,BalanceCredit,Points,RankScore,TopicCount,ReplyTopicCount,FavTopicCount,PvCount,FansCount,FellowCount,AblumsCount,FavouritesCount,FavoritedCount,ShareCount,ProductsCount,PersonalDomain,LastAccessTime,LastAccessIP,LastPostTime,LastLoginTime,Remark,IsUserDPI,PayAccount,UserCardCode,UserCardType,SourceType,SalesId)");
                        strSql3.Append(" values (");
                        strSql3.Append("@UserID,@Gravatar,@Singature,@TelPhone,@QQ,@MSN,@HomePage,@Birthday,@BirthdayVisible,@BirthdayIndexVisible,@Constellation,@ConstellationVisible,@ConstellationIndexVisible,@NativePlace,@NativePlaceVisible,@NativePlaceIndexVisible,@RegionId,@Address,@AddressVisible,@AddressIndexVisible,@BodilyForm,@BodilyFormVisible,@BodilyFormIndexVisible,@BloodType,@BloodTypeVisible,@BloodTypeIndexVisible,@Marriaged,@MarriagedVisible,@MarriagedIndexVisible,@PersonalStatus,@PersonalStatusVisible,@PersonalStatusIndexVisible,@Grade,@Balance,@BalanceCredit,@Points,@RankScore,@TopicCount,@ReplyTopicCount,@FavTopicCount,@PvCount,@FansCount,@FellowCount,@AblumsCount,@FavouritesCount,@FavoritedCount,@ShareCount,@ProductsCount,@PersonalDomain,@LastAccessTime,@LastAccessIP,@LastPostTime,@LastLoginTime,@Remark,@IsUserDPI,@PayAccount,@UserCardCode,@UserCardType,@SourceType,@SalesId)");
                        SqlParameter[] parameters3 = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@Gravatar", SqlDbType.NVarChar,200),
                    new SqlParameter("@Singature", SqlDbType.NVarChar,200),
                    new SqlParameter("@TelPhone", SqlDbType.NVarChar,50),
                    new SqlParameter("@QQ", SqlDbType.NVarChar,50),
                    new SqlParameter("@MSN", SqlDbType.NVarChar,50),
                    new SqlParameter("@HomePage", SqlDbType.NVarChar,50),
                    new SqlParameter("@Birthday", SqlDbType.DateTime),
                    new SqlParameter("@BirthdayVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@BirthdayIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@Constellation", SqlDbType.NVarChar,50),
                    new SqlParameter("@ConstellationVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@ConstellationIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@NativePlace", SqlDbType.NVarChar,300),
                    new SqlParameter("@NativePlaceVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@NativePlaceIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@RegionId", SqlDbType.Int,4),
                    new SqlParameter("@Address", SqlDbType.NVarChar,300),
                    new SqlParameter("@AddressVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@AddressIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@BodilyForm", SqlDbType.NVarChar,10),
                    new SqlParameter("@BodilyFormVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@BodilyFormIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@BloodType", SqlDbType.NVarChar,10),
                    new SqlParameter("@BloodTypeVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@BloodTypeIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@Marriaged", SqlDbType.NVarChar,10),
                    new SqlParameter("@MarriagedVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@MarriagedIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@PersonalStatus", SqlDbType.NVarChar,10),
                    new SqlParameter("@PersonalStatusVisible", SqlDbType.SmallInt,2),
                    new SqlParameter("@PersonalStatusIndexVisible", SqlDbType.Bit,1),
                    new SqlParameter("@Grade", SqlDbType.Int,4),
                    new SqlParameter("@Balance", SqlDbType.Money,8),
                    new SqlParameter("@BalanceCredit", SqlDbType.Money,8),
                    new SqlParameter("@Points", SqlDbType.Int,4),
                    new SqlParameter("@RankScore", SqlDbType.Int,4),
                    new SqlParameter("@TopicCount", SqlDbType.Int,4),
                    new SqlParameter("@ReplyTopicCount", SqlDbType.Int,4),
                    new SqlParameter("@FavTopicCount", SqlDbType.Int,4),
                    new SqlParameter("@PvCount", SqlDbType.Int,4),
                    new SqlParameter("@FansCount", SqlDbType.Int,4),
                    new SqlParameter("@FellowCount", SqlDbType.Int,4),
                    new SqlParameter("@AblumsCount", SqlDbType.Int,4),
                    new SqlParameter("@FavouritesCount", SqlDbType.Int,4),
                    new SqlParameter("@FavoritedCount", SqlDbType.Int,4),
                    new SqlParameter("@ShareCount", SqlDbType.Int,4),
                    new SqlParameter("@ProductsCount", SqlDbType.Int,4),
                    new SqlParameter("@PersonalDomain", SqlDbType.NVarChar,50),
                    new SqlParameter("@LastAccessTime", SqlDbType.DateTime),
                    new SqlParameter("@LastAccessIP", SqlDbType.NVarChar,50),
                    new SqlParameter("@LastPostTime", SqlDbType.DateTime),
                    new SqlParameter("@LastLoginTime", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,-1),
                    new SqlParameter("@IsUserDPI", SqlDbType.Bit,1),
                    new SqlParameter("@PayAccount", SqlDbType.NVarChar,200),
                    new SqlParameter("@UserCardCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserCardType", SqlDbType.SmallInt,2),
                    new SqlParameter("@SourceType", SqlDbType.Int,4),
                    new SqlParameter("@SalesId", SqlDbType.Int,4)};
                        parameters3[0].Value = userId;
                        parameters3[1].Value = "";
                        parameters3[2].Value ="";
                        parameters3[3].Value = phone;
                        parameters3[4].Value ="";
                        parameters3[5].Value = "";
                        parameters3[6].Value = "";
                        parameters3[7].Value =null ;
                        parameters3[8].Value =0;
                        parameters3[9].Value =false;
                        parameters3[10].Value = "";
                        parameters3[11].Value =0;
                        parameters3[12].Value = false;
                        parameters3[13].Value ="";
                        parameters3[14].Value = 0;
                        parameters3[15].Value =false;
                        parameters3[16].Value =0;
                        parameters3[17].Value = "";
                        parameters3[18].Value =0;
                        parameters3[19].Value = false;
                        parameters3[20].Value = "";
                        parameters3[21].Value = 0;
                        parameters3[22].Value = false;
                        parameters3[23].Value = "";
                        parameters3[24].Value =0;
                        parameters3[25].Value =false;
                        parameters3[26].Value = "";
                        parameters3[27].Value = 0;
                        parameters3[28].Value = false;
                        parameters3[29].Value = "";
                        parameters3[30].Value = 0;
                        parameters3[31].Value = false;
                        parameters3[32].Value = 0;
                        parameters3[33].Value = 0;
                        parameters3[34].Value =0;
                        parameters3[35].Value = 0;
                        parameters3[36].Value = 0;
                        parameters3[37].Value = 0;
                        parameters3[38].Value = 0;
                        parameters3[39].Value = 0;
                        parameters3[40].Value = 0;
                        parameters3[41].Value = 0;
                        parameters3[42].Value = 0;
                        parameters3[43].Value = 0;
                        parameters3[44].Value = 0;
                        parameters3[45].Value = 0;
                        parameters3[46].Value = 0;
                        parameters3[47].Value = 0;
                        parameters3[48].Value = "";
                        parameters3[49].Value = null;
                        parameters3[50].Value ="";
                        parameters3[51].Value = null;
                        parameters3[52].Value =null;
                        parameters3[53].Value = "";
                        parameters3[54].Value = false;
                        parameters3[55].Value = "";
                        parameters3[56].Value = "";
                        parameters3[57].Value =0;
                        parameters3[58].Value =0;
                        parameters3[59].Value =0;

                        DBHelper.DefaultDBHelper.GetSingle4Trans(new CommandInfo(strSql3.ToString(), parameters3), transaction);


                        System.Text.StringBuilder strSql2 = new System.Text.StringBuilder();
                        strSql2.Append("insert into Accounts_UserRoles(");
                        strSql2.Append("UserID,RoleID)");
                        strSql2.Append(" values (");
                        strSql2.Append("@UserID,@RoleID)");
                        SqlParameter[] parameters2 = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@RoleID", SqlDbType.Int,4)
                };
                        parameters2[0].Value = userId;
                        parameters2[1].Value = roleId;
                        DBHelper.DefaultDBHelper.GetSingle4Trans(new CommandInfo(strSql2.ToString(), parameters2), transaction);

                        transaction.Commit();
                        return userId;
                    }
                    catch (SqlException ex)
                    {
                        Log.LogHelper.AddTextLog("创建用户失败:"+ex.Message,"错误为："+ex.StackTrace);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// 更新信息 （SAAS 创建子账号使用）
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="trueName"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool UpdateEx(string userName, byte[] password, string trueName, string phone)
        {
            StringBuilder strSql = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            strSql.Append("update Accounts_Users set ");
            if (!String.IsNullOrWhiteSpace(phone))
            {
                strSql.AppendFormat("Phone='{0}', ",Common.InjectionFilter.SqlFilter(phone));
            }
            if (password != null)
            {
                strSql.AppendFormat("Password=@Password, ");
                parameters.Add(new SqlParameter("@Password", password));
            }
            if (!String.IsNullOrWhiteSpace(trueName))
            {
                strSql.AppendFormat("TrueName='{0}', ", Common.InjectionFilter.SqlFilter(trueName));
            }
            strSql.Append(" Activity=@Activity ");
            strSql.Append("  where UserName=@UserName");
            parameters.Add(new SqlParameter("@UserName", userName));
            parameters.Add(new SqlParameter("@Activity", true));
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters.ToArray());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool UpdateActivity(string userName, bool activity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_Users ");
            strSql.Append("SET Activity=@Activity ");
            strSql.Append("WHERE UserName=@UserName");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserName", SqlDbType.NVarChar,100),
                    new SqlParameter("@Activity", SqlDbType.Bit,1)};
            parameters[0].Value = userName;
            parameters[1].Value = activity;
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            return (rows > 0);
        }




        #endregion
    }
}
