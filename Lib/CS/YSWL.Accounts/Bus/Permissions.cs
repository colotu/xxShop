using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using YSWL.Accounts.IData;
using YSWL.Common;
using YSWL.DBUtility;

namespace YSWL.Accounts.Bus
{
    /// <summary>
    /// 权限管理。
    /// </summary>
    [Serializable]
    public class Permissions
    {
        private IData.IPermission dalPermission = PubConstant.IsSQLServer
                                                       ? (IPermission)new Data.Permission()
                                                       : new MySqlData.Permission();


        private int _permissionID;
        /// <summary>
        /// 用户编号
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


        private string _description;
        /// <summary>
        /// 用户编号
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        private int _categoryID;
        /// <summary>
        /// 用户编号
        /// </summary>
        public int CategoryID
        {
            get
            {
                return _categoryID;
            }
            set
            {
                _categoryID = value;
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public Permissions()
        {
        }



        /// <summary>
        /// 创建一个权限
        /// </summary>
        /// <param name="pcID">类别ID</param>
        /// <param name="description">权限描述</param>
        /// <returns></returns>
        public int Create(int pcID, string description)
        {
            int pID = dalPermission.Create(pcID, description);
            return pID;
        }
        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="pcID">权限ID</param>
        /// <param name="description">权限描述</param>
        /// <returns></returns>
        public bool Update(int pcID, string description)
        {
            return dalPermission.Update(pcID, description);
        }

        public void UpdateCategory(string PermissionIDlist, int CategoryID)
        {
            dalPermission.UpdateCategory(PermissionIDlist, CategoryID);
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public bool Delete(int pID)
        {
            return dalPermission.Delete(pID);
        }

        /// <summary>
        /// 得到权限名称
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public string GetPermissionName(int permissionId)
        {
            DataSet permissions = dalPermission.Retrieve(permissionId);
            if (permissions.Tables[0].Rows.Count == 0)
            {
                throw new Exception("找不到权限 （" + permissionId + "）");
            }
            else
            {
                return permissions.Tables[0].Rows[0]["Description"].ToString();
            }
        }

        /// <summary>
        /// 得到权限信息
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public void GetPermissionDetails(int pID)
        {
            DataSet permissions = dalPermission.Retrieve(pID);
            if (permissions.Tables[0].Rows.Count > 0)
            {
                if (permissions.Tables[0].Rows[0]["PermissionID"] != null)
                {
                    _permissionID = Convert.ToInt32(permissions.Tables[0].Rows[0]["PermissionID"]);
                }
                _description = permissions.Tables[0].Rows[0]["Description"].ToString();
                if (permissions.Tables[0].Rows[0]["CategoryID"] != null)
                {
                    _categoryID = Convert.ToInt32(permissions.Tables[0].Rows[0]["CategoryID"]);
                }
            }
        }


        #region  XML  权限处理

        private static DataCacheCore dataCache = new DataCacheCore(new CacheOption
        {
            CacheType = SAASInfo.GetSystemBoolValue("RedisCacheUse") ? CacheType.Redis : CacheType.IIS,
            ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
            ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            CancelProductKey = true,
            CancelEnterpriseKey = true,
            DefaultDb = 1
        });
        /// <summary>
        /// 获取所有的XML 权限
        /// </summary>
        /// <returns></returns>
        public static List<Permissions> GetAllXMLPermission()
        {
            string path = HttpContext.Current.Server.MapPath("/Config/Permission.config");
            List<Permissions> permissionList = new List<Permissions>();
            if (!File.Exists(path))
            {
                return permissionList;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            //取指定的结点的集合
            XmlNodeList nodes = xmlDoc.SelectNodes("permissions/application");
            if (nodes != null)
            {
                Permissions model = null;
                foreach (XmlElement parentNode in nodes)
                {
                    foreach (XmlElement node in parentNode.ChildNodes)
                    {
                        model = new Permissions
                        {
                            PermissionID = Common.Globals.SafeInt(node.GetAttribute("PermissionID"), 0),
                            Description = node.GetAttribute("Description"),
                            CategoryID = Common.Globals.SafeInt(node.GetAttribute("CategoryID"), 0)
                        };
                        permissionList.Add(model);
                    }
                }
            }
            return permissionList;
        }
        /// <summary>
        /// 获取所有的XML 权限缓存
        /// </summary>
        /// <returns></returns>
        public static List<Permissions> GetAllXMLPermissionCache()
        {
            string CacheKey = "GetAllXMLPermissionCache";
            List<Permissions> allPermissions = dataCache.GetCache<List<Permissions>>(CacheKey);
            if (allPermissions == null)
            {
                allPermissions = GetAllXMLPermission();
                if (allPermissions != null)
                {
                    dataCache.SetCache(CacheKey, allPermissions, DateTime.MaxValue, TimeSpan.Zero);
                }
            }
            return allPermissions;
        }
        /// <summary>
        /// 获取某用户的权限
        /// </summary>
        /// <returns></returns>
        public static List<Permissions> GetPermissionByUser(int userId)
        {
            List<Permissions> ALLList = GetAllXMLPermissionCache();
            //获取所有的用户角色关联
            YSWL.Accounts.Bus.Role roleBll=new Role();
            List<UserRoles> ALLUserRoleList= roleBll.GetALLUserRole();
            //获取所有的角色权限关联
            List<RolePermissions> ALLRolePermList = roleBll.GetALLRolePerm();

            #region  处理权限逻辑

            List<UserRoles> userRoleList = ALLUserRoleList.Where(c => c.UserID == userId).ToList();

            #region  处理系统管理员权限，系统管理员默认加载所有的权限
            var systemRole= userRoleList.Find(c => c.RoleID == 1);
            if (systemRole != null) //如果包含系统管理员角色，则加载所有的权限
            {
                return ALLList;
            }
            #endregion

            if (userRoleList == null || userRoleList.Count == 0)
            {
                return null;
            }
            List<RolePermissions> rolePermList = ALLRolePermList.Where(c => userRoleList.Select(k=>k.RoleID).Contains(c.RoleID)).ToList();

            if (rolePermList==null || rolePermList.Count==0)
            {
                return null;
            }
            List<Permissions> permissionList =
                ALLList.Where(c => rolePermList.Select(k => k.PermissionID).Contains(c.PermissionID)).ToList();
            #endregion

            return permissionList;  
        }

        #endregion
    }


}
