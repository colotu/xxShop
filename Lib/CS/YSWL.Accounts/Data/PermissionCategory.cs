using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using YSWL.Accounts.IData;
using YSWL.DBUtility;

namespace YSWL.Accounts.Data
{
    /// <summary>
    ///权限类别
    /// </summary>
    public class PermissionCategory : IPermissionCategory
    {
        public PermissionCategory()
        { }

        /// <summary>
        /// 创建权限类别
        /// </summary>        
        public int Create(string description)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO Accounts_PermissionCategories(Description) VALUES(@Description) ");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters =
             {
                    new SqlParameter("@Description", SqlDbType.NVarChar, 50)
                };
            parameters[0].Value = description;

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
        /// 该类别下是否存在权限记录
        /// </summary>
        public bool ExistsPerm(int CategoryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Accounts_Permissions");
            strSql.Append(" where CategoryID=@CategoryID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CategoryID", SqlDbType.Int,4)
            };
            parameters[0].Value = CategoryID;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除权限类别
        /// </summary>        
        public bool Delete(int CategoryID)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("DELETE Accounts_Permissions  ");
            strSql2.Append(" WHERE CategoryID = @CategoryID ");
            SqlParameter[] parameters2 =
                {
                     new SqlParameter("@CategoryID", SqlDbType.Int, 4)
                };
            parameters2[0].Value = CategoryID;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);



            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("DELETE Accounts_PermissionCategories  ");
            strSql3.Append("WHERE CategoryID = @CategoryID  ");
            SqlParameter[] parameters3 =
                {
                    new SqlParameter("@CategoryID", SqlDbType.Int, 4)
                };
            parameters3[0].Value = CategoryID;
            cmd = new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);



            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            return rowsAffected > 0;
        }

        /// <summary>
        /// 获取权限类别信息
        /// </summary>        
        public DataRow Retrieve(int categoryId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT CategoryID, Description FROM Accounts_PermissionCategories ");
            strSql.Append(" WHERE CategoryID = @CategoryID  ");
            SqlParameter[] parameters = 
                {
                    new SqlParameter("@CategoryID", SqlDbType.Int, 4)
                };
            parameters[0].Value = categoryId;

            using (DataSet categories = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters))
            {
                return categories.Tables[0].Rows[0];
            }
        }

        /// <summary>
        /// 获取指定类别下的权限列表
        /// </summary>        
        public DataSet GetPermissionsInCategory(int categoryId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Accounts_Permissions  ");
            strSql.Append(" where CategoryID=@CategoryID ORDER BY Description  ");

            SqlParameter[] parameters =
                {
                    new SqlParameter("@CategoryID", SqlDbType.Int, 4)
                };
            parameters[0].Value = categoryId;

            using (DataSet permissions = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters))
            {
                return permissions;
            }
        }

        /// <summary>
        /// 获取权限类别的列表
        /// </summary>        
        public DataSet GetCategoryList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Accounts_PermissionCategories   ");
            using (DataSet categories = DBHelper.DefaultDBHelper.Query(strSql.ToString()))
            {
                return categories;
            }
        }
    }
}
