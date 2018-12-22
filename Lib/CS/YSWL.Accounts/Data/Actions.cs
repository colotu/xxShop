using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using YSWL.Accounts.IData;
using YSWL.DBUtility;

namespace YSWL.Accounts.Data
{
    public class Actions : IActions
    {

        #region basic

        /// <summary>
        /// Whether Exists
        /// </summary>
        public bool Exists(string Description)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_Actions_Permission");
            strSql.Append(" where Description=@Description ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Description", SqlDbType.NVarChar,200)};
            parameters[0].Value = Description;
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }



        /// <summary>
        /// Add a record
        /// </summary>
        public int Add(string Description)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_Actions_Permission(");
            strSql.Append("Description)");
            strSql.Append(" values (");
            strSql.Append("@Description)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Description", SqlDbType.NVarChar,200)};
            parameters[0].Value = Description;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// Add a record,include perimission
        /// </summary>
        public int Add(string Description, int PermissionID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_Actions_Permission(");
            strSql.Append("Description,PermissionID)");
            strSql.Append(" values (");
            strSql.Append("@Description,@PermissionID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Description", SqlDbType.NVarChar,200),
                    new SqlParameter("@PermissionID", SqlDbType.Int,4)};
            parameters[0].Value = Description;
            parameters[1].Value = PermissionID;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }

        }

        /// <summary>
        /// Update a record
        /// </summary>
        public void Update(int ActionID, string Description)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_Actions_Permission set ");
            strSql.Append("Description=@Description");
            strSql.Append(" where ActionID=@ActionID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ActionID", SqlDbType.Int,4),
                    new SqlParameter("@Description", SqlDbType.NVarChar,200)};
            parameters[0].Value = ActionID;
            parameters[1].Value = Description;

            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// Update a record, include permission
        /// </summary>
        public void Update(int ActionID, string Description, int PermissionID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_Actions_Permission set ");
            strSql.Append("Description=@Description,");
            strSql.Append("PermissionID=@PermissionID");
            strSql.Append(" where ActionID=@ActionID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ActionID", SqlDbType.Int,4),
                    new SqlParameter("@Description", SqlDbType.NVarChar,200),
                    new SqlParameter("@PermissionID", SqlDbType.Int,4)};
            parameters[0].Value = ActionID;
            parameters[1].Value = Description;
            parameters[2].Value = PermissionID;

            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
        }



        /// <summary>
        /// Delete a record
        /// </summary>
        public void Delete(int ActionID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Accounts_Actions_Permission ");
            strSql.Append(" where ActionID=@ActionID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ActionID", SqlDbType.Int,4)};
            parameters[0].Value = ActionID;

            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// Get Description
        /// </summary>
        public string GetDescription(int ActionID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 Description from Accounts_Actions_Permission ");
            strSql.Append(" where ActionID=@ActionID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ActionID", SqlDbType.Int,4)};
            parameters[0].Value = ActionID;
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["Description"].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Query data list
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ActionID,Description,PermissionID ");
            strSql.Append(" FROM Accounts_Actions_Permission ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        #endregion


        #region expand

        public void AddPermission(int ActionID, int PermissionID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_Actions_Permission set ");
            strSql.Append("PermissionID=@PermissionID");
            strSql.Append(" where ActionID=@ActionID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ActionID", SqlDbType.Int,4),
                    new SqlParameter("@PermissionID", SqlDbType.Int,4)};
            parameters[0].Value = ActionID;
            parameters[1].Value = PermissionID;

            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 批量增加权限设置
        /// </summary>
        /// <param name="ActionIDs"></param>
        /// <param name="PermissionID"></param>
        public void AddPermission(string ActionIDs, int PermissionID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_Actions_Permission set ");
            strSql.Append("PermissionID=" + PermissionID);
            strSql.Append(" where ActionID in (" + ActionIDs + ")");

            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
        }

        public void ClearPermissions(int ActionID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_Actions_Permission set ");
            strSql.Append("PermissionID=@PermissionID");
            strSql.Append(" where ActionID=@ActionID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ActionID", SqlDbType.Int,4),
                    new SqlParameter("@PermissionID", SqlDbType.Int,4)};
            parameters[0].Value = ActionID;
            parameters[1].Value = 0;

            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        #endregion

    }
}
