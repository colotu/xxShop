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
    /// 权限管理
    /// </summary>
    public class Permission : IPermission
    {

        #region
        /// <summary>
        /// 创建一个权限
        /// </summary>
        public int Create(int categoryID, string description)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO Accounts_Permissions(CategoryID,Description)  ");
            strSql.Append(" values (");
            strSql.Append("  @CategoryID,@Description )");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = 
                {
                    new SqlParameter("@CategoryID", SqlDbType.Int, 8),
                    new SqlParameter("@Description", SqlDbType.NVarChar, 50)
                };
            parameters[0].Value = categoryID;
            parameters[1].Value = description;

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
        /// 更新权限信息
        /// </summary>
        public bool Update(int PermissionID, string description)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Accounts_Permissions SET  ");
            strSql.Append(" Description = @Description ");
            strSql.Append(" WHERE PermissionID = @PermissionID ");
            SqlParameter[] parameters =
                  {
                    new SqlParameter("@PermissionID", SqlDbType.Int, 8),
                    new SqlParameter("@Description", SqlDbType.NVarChar, 50)
                };
            parameters[0].Value = PermissionID;
            parameters[1].Value = description;

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

        public void UpdateCategory(string PermissionIDlist, int CategoryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_Permissions set ");
            strSql.AppendFormat(" CategoryID={0}", CategoryID);
            strSql.AppendFormat(" where PermissionID in({0})", PermissionIDlist);

            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());

        }


        /// <summary>
        /// 删除权限
        /// </summary>        
        public bool Delete(int id)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
           
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("DELETE Accounts_Permissions ");
            strSql2.Append(" WHERE PermissionID = @PermissionID");
            SqlParameter[] parameters2 =
                {
                    new SqlParameter("@PermissionID", SqlDbType.BigInt, 4)
                };
            parameters2[0].Value = id;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);



            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("DELETE Accounts_RolePermissions ");
            strSql3.Append("WHERE PermissionID = @PermissionID ");
            SqlParameter[] parameters3 =
                {
                      new SqlParameter("@PermissionID", SqlDbType.BigInt, 4)
                };
            parameters3[0].Value = id;
            cmd = new CommandInfo(strSql3.ToString(), parameters3, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);



            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            return rowsAffected > 0;
        }

        /// <summary>
        /// 根据权限ID获取权限信息
        /// </summary>
        public DataSet Retrieve(int permissionId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Accounts_Permissions  ");
            strSql.Append("  WHERE PermissionID = @PermissionID");
            SqlParameter[] parameters =
            {
                new SqlParameter("@PermissionID", SqlDbType.Int, 4)
            };
            parameters[0].Value = permissionId;

            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获取权限列表
        /// </summary>        
        public DataSet GetPermissionList()
        {
            //SqlParameter[] parameters = { new SqlParameter("@RoleID", SqlDbType.NVarChar, 4) };
            //using (DataSet permissions = DBHelper.DefaultDBHelper.RunProcedure("sp_Accounts_GetPermissionCategories", new IDataParameter[] { }, "Categories"))
            //{
            //    DBHelper.DefaultDBHelper.RunProcedure("sp_Accounts_GetPermissionList", parameters, permissions, "Permissions");
            //    DataRelation permissionCategories = new DataRelation("PermissionCategories",
            //        permissions.Tables["Categories"].Columns["CategoryID"],
            //        permissions.Tables["Permissions"].Columns["CategoryID"], true);
            //    permissions.Relations.Add(permissionCategories);
            //    DataColumn[] categoryKeys = new DataColumn[1];
            //    categoryKeys[0] = permissions.Tables["Categories"].Columns["CategoryID"];
            //    DataColumn[] permissionKeys = new DataColumn[1];
            //    permissionKeys[0] = permissions.Tables["Permissions"].Columns["PermissionID"];
            //    permissions.Tables["Categories"].PrimaryKey = categoryKeys;
            //    permissions.Tables["Permissions"].PrimaryKey = permissionKeys;
            //    return permissions;
            //}

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT PermissionID, Description, CategoryID FROM Accounts_Permissions ORDER BY Description   ");

            return DBHelper.DefaultDBHelper.Query(strSql.ToString());

        }

        #endregion



        /// <summary>
        /// 获取指定角色的权限列表
        /// </summary>        
        public DataSet GetPermissionList(int roleId)
        {
            //SqlParameter[] parameters = { new SqlParameter("@RoleID", SqlDbType.NVarChar, 4) };
            //parameters[0].Value = roleId;
            //using (DataSet permissions = DBHelper.DefaultDBHelper.RunProcedure("sp_Accounts_GetPermissionCategories", new IDataParameter[] { }, "Categories"))
            //{
            //    DBHelper.DefaultDBHelper.RunProcedure("sp_Accounts_GetPermissionList", parameters, permissions, "Permissions");
            //    DataRelation permissionCategories = new DataRelation("PermissionCategories",
            //        permissions.Tables["Categories"].Columns["CategoryID"],
            //        permissions.Tables["Permissions"].Columns["CategoryID"], true);
            //    permissions.Relations.Add(permissionCategories);
            //    DataColumn[] categoryKeys = new DataColumn[1];
            //    categoryKeys[0] = permissions.Tables["Categories"].Columns["CategoryID"];
            //    DataColumn[] permissionKeys = new DataColumn[1];
            //    permissionKeys[0] = permissions.Tables["Permissions"].Columns["PermissionID"];
            //    permissions.Tables["Categories"].PrimaryKey = categoryKeys;
            //    permissions.Tables["Permissions"].PrimaryKey = permissionKeys;
            //    return permissions;
            //}

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ap.PermissionID, ap.Description, ap.CategoryID FROM Accounts_Permissions ap INNER JOIN   ");
            strSql.Append(" Accounts_RolePermissions apr ON ap.PermissionID = apr.PermissionID WHERE apr.RoleID = @RoleID ORDER BY ap.Description   ");
            SqlParameter[] parameters =
         {
                new SqlParameter("@RoleID", SqlDbType.Int, 4)
            };
            parameters[0].Value = roleId;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获取指定角色没有的权限列表
        /// </summary>        
        public DataSet GetNoPermissionList(int roleId)
        {
            //SqlParameter[] parameters = { new SqlParameter("@RoleID", SqlDbType.NVarChar, 4) };
            //parameters[0].Value = roleId;
            //using (DataSet permissions = DBHelper.DefaultDBHelper.RunProcedure("sp_Accounts_GetPermissionCategories", new IDataParameter[] { }, "Categories"))
            //{
            //    DBHelper.DefaultDBHelper.RunProcedure("sp_Accounts_GetNoPermissionList", parameters, permissions, "Permissions");
            //    DataRelation permissionCategories = new DataRelation("PermissionCategories",
            //        permissions.Tables["Categories"].Columns["CategoryID"],
            //        permissions.Tables["Permissions"].Columns["CategoryID"], true);
            //    permissions.Relations.Add(permissionCategories);
            //    DataColumn[] categoryKeys = new DataColumn[1];
            //    categoryKeys[0] = permissions.Tables["Categories"].Columns["CategoryID"];
            //    DataColumn[] permissionKeys = new DataColumn[1];
            //    permissionKeys[0] = permissions.Tables["Permissions"].Columns["PermissionID"];
            //    permissions.Tables["Categories"].PrimaryKey = categoryKeys;
            //    permissions.Tables["Permissions"].PrimaryKey = permissionKeys;
            //    return permissions;
            //}

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT PermissionID, Description, CategoryID  FROM Accounts_Permissions   ");
            strSql.Append("where PermissionID not in(select PermissionID from Accounts_RolePermissions   ");
            strSql.Append(" WHERE RoleID = @RoleID ) ORDER BY Description   ");
            SqlParameter[] parameters =
         {
                new SqlParameter("@RoleID", SqlDbType.Int, 4)
            };
            parameters[0].Value = roleId;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);

        }




    }
}
