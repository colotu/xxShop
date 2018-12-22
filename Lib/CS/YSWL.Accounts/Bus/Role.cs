using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization.Configuration;
using YSWL.Accounts.IData;

namespace YSWL.Accounts.Bus
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Serializable]
    public class Role
    {
        #region 属性
        private int roleId;
        private string description;
        private DataSet permissions;
        private DataSet nopermissions;
        private DataSet users;
        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleID
        {
            get
            {
                return roleId;
            }
            set
            {
                roleId = value;
            }
        }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        /// <summary>
        /// 该角色拥有的权限列表
        /// </summary>
        public DataSet Permissions
        {
            get
            {
                return permissions;
            }
        }
        /// <summary>
        /// 该角色没有的权限列表
        /// </summary>
        public DataSet NoPermissions
        {
            get
            {
                return nopermissions;
            }
        }
        /// <summary>
        /// 该角色下的所有用户
        /// </summary>
        public DataSet Users
        {
            get
            {
                return users;
            }
        }
        #endregion

        private IData.IRole dataRole;

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        public Role()
        {
            dataRole = PubConstant.IsSQLServer ? (IRole)new Data.Role() : new MySqlData.Role();
        }


        /// <summary>
        /// 根据角色ID构造角色的信息
        /// </summary>
        public Role(int currentRoleId)
        {
            dataRole = PubConstant.IsSQLServer ? (IRole)new Data.Role() : new MySqlData.Role();
            DataRow roleRow;
            roleRow = dataRole.Retrieve(currentRoleId);
            roleId = currentRoleId;
            if (roleRow["Description"] != null)
            {
                description = (string)roleRow["Description"];
            }
            IData.IPermission dataPermission = PubConstant.IsSQLServer ? (IPermission)new Data.Permission() : new MySqlData.Permission();
            permissions = dataPermission.GetPermissionList(currentRoleId);
            nopermissions = dataPermission.GetNoPermissionList(currentRoleId);

            IData.IUser user = PubConstant.IsSQLServer ? (IUser)new Data.User() : new MySqlData.User();
            users = user.GetUsersByRole(currentRoleId);
        }

        /// <summary>
        /// 是否存在该角色
        /// </summary>
        public bool RoleExists(string Description)
        {
            return dataRole.RoleExists(Description);
        }
        /// <summary>
        /// 增加角色
        /// </summary>
        public int Create()
        {
            roleId = dataRole.Create(description);
            return roleId;
        }
        /// <summary>
        /// 更新角色
        /// </summary>
        public bool Update()
        {
            return dataRole.Update(roleId, description);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        public bool Delete()
        {
            return dataRole.Delete(roleId);
        }
        /// <summary>
        /// 为角色增加权限
        /// </summary>
        public void AddPermission(int permissionId)
        {
            dataRole.AddPermission(roleId, permissionId);
        }
        /// <summary>
        /// 从角色移除权限
        /// </summary>
        public void RemovePermission(int permissionId)
        {
            dataRole.RemovePermission(roleId, permissionId);
        }
        /// <summary>
        /// 清空角色的权限
        /// </summary>
        public void ClearPermissions()
        {
            dataRole.ClearPermissions(roleId);
        }
        /// <summary>
        /// 获取所有角色的列表
        /// </summary>
        public DataSet GetRoleList()
        {
            return dataRole.GetRoleList();
        }
        #endregion

        #region  用户角色权限关联数据

        public List<UserRoles> GetALLUserRole()
        {
            DataSet ds = dataRole.GetALLUserRole();
            List<UserRoles> modelList = new List<UserRoles>();
            int rowsCount = ds.Tables[0].Rows.Count;
            if (rowsCount > 0)
            {
                UserRoles model=null;
                for (int n = 0; n < rowsCount; n++)
                {
                    DataRow row = ds.Tables[0].Rows[n];
                    model=new UserRoles();
                    if (row != null)
                    {
                        if (row["RoleID"] != null && row["RoleID"].ToString() != "")
                        {
                            model.RoleID =Common.Globals.SafeInt(row["RoleID"],0);
                        }
                        if (row["UserID"] != null)
                        {
                            model.UserID = Common.Globals.SafeInt(row["UserID"],0);
                        }
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        public List<RolePermissions> GetALLRolePerm()
        {
            DataSet ds = dataRole.GetALLRolePerm();
            List<RolePermissions> modelList = new List<RolePermissions>();
            int rowsCount = ds.Tables[0].Rows.Count;
            if (rowsCount > 0)
            {
                RolePermissions model = null;
                for (int n = 0; n < rowsCount; n++)
                {
                    DataRow row = ds.Tables[0].Rows[n];
                    model = new RolePermissions();
                    if (row != null)
                    {
                        if (row["RoleID"] != null && row["RoleID"].ToString() != "")
                        {
                            model.RoleID = Common.Globals.SafeInt(row["RoleID"], 0);
                        }
                        if (row["PermissionID"] != null)
                        {
                            model.PermissionID = Common.Globals.SafeInt(row["PermissionID"], 0);
                        }
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }


        #endregion
    }


    #region 角色权限
    public class RolePermissions
    {
        private int _roleID;
        /// <summary>
        /// 用户角色
        /// </summary>
        public int RoleID
        {
            get
            {
                return _roleID;
            }
            set
            {
                _roleID = value;
            }
        }

        private int _permissionID;
        /// <summary>
        /// 用户权限
        /// </summary>
        public int PermissionID
        {
            get
            {
                return _permissionID;
            }
            set
            {
                _permissionID = value;
            }
        }

    }
    #endregion


    #region  用户角色
    public class UserRoles
    {
        private int _roleID;
        /// <summary>
        /// 用户角色
        /// </summary>
        public int RoleID
        {
            get
            {
                return _roleID;
            }
            set
            {
                _roleID = value;
            }
        }

        private int _userID;
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }
    }
    #endregion 
}
