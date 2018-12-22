using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Web;
using YSWL.Accounts.IData;
using YSWL.Log;

namespace YSWL.Accounts.Bus
{
    /// <summary>
    /// 用户对象的安全上下文信息
    /// </summary>
    public class AccountsPrincipal : System.Security.Principal.IPrincipal
    {
        private IData.IUser dataUser = PubConstant.IsSQLServer ? (IUser)new Data.User() : new MySqlData.User();

        #region 属性
        protected System.Security.Principal.IIdentity identity;
        protected DataSet permissionLists;
        protected List<string> permissionsDesc = new List<string>();
        protected List<int> permissionListid = new List<int>();
        protected Dictionary<int, string> rolesKeyValue;

        protected string token;              //安全校验Token
        protected string timestamp;       //安全校验时间搓
        /// <summary>
        /// 当前用户的所有角色
        /// </summary>
        public ArrayList Roles
        {
            get
            {
                return rolesKeyValue == null ?
                    null : new ArrayList(rolesKeyValue.Values);
            }
        }
        /// <summary>
        /// 当前用户拥有的权限列表数据集
        /// </summary>
        public DataSet PermissionLists
        {
            get
            {
                return permissionLists;
            }
        }

        /// <summary>
        /// 当前用户拥有的权限名称列表
        /// </summary>
        public List<string> PermissionsDesc
        {
            get
            {
                return permissionsDesc;
            }
        }
        /// <summary>
        /// 当前用户拥有的权限ID列表
        /// </summary>
        public List<int> PermissionsID
        {
            get
            {
                return permissionListid;
            }
        }


        // IPrincipal Interface Requirements:
        /// <summary>
        /// 当前用户的标识对象
        /// </summary>
        public System.Security.Principal.IIdentity Identity
        {
            get
            {
                return identity;
            }
            set
            {
                identity = value;
            }
        }

        public string Token
        {
            get
            {
                return token;
            }
        }


        public string Timestamp
        {
            get
            {
                return timestamp;
            }
        }

        #endregion

        /// <summary>
        /// 根据用户编号构造
        /// </summary>
        public AccountsPrincipal(int userID)
        {
            identity = new SiteIdentity(userID);

            #region XML 权限
            if (isXML())
            {
                List<Permissions> permissionList = YSWL.Accounts.Bus.Permissions.GetPermissionByUser(userID);
                if (permissionList != null)
                {
                    foreach (var item in permissionList)
                    {
                        permissionListid.Add(item.PermissionID);
                        permissionsDesc.Add(item.Description);
                    }
                }
            }
            #endregion
            
            #region 数据库权限
            else
            {
                permissionLists = dataUser.GetEffectivePermissionLists(userID);
                if (permissionLists.Tables.Count > 0)
                {
                    foreach (DataRow dr in permissionLists.Tables[0].Rows)
                    {
                        permissionListid.Add(Convert.ToInt32(dr["PermissionID"]));
                        permissionsDesc.Add(dr["Description"].ToString());
                        //增加用户 的特别权限
                    }
                }
            }
            #endregion

            rolesKeyValue = dataUser.GetUserRoles4KeyValues(userID);
        }
        /// <summary>
        /// 根据用户名构造
        /// </summary>
        public AccountsPrincipal(string userName, bool IsVerify = false)
        {
            SiteIdentity _identity;
            identity = _identity = new SiteIdentity(userName);

            #region XML 权限
            if (isXML())
            {
                List<Permissions> permissionList = YSWL.Accounts.Bus.Permissions.GetPermissionByUser(_identity.UserID);
                if (permissionList != null)
                {
                    foreach (var item in permissionList)
                    {
                        permissionListid.Add(item.PermissionID);
                        permissionsDesc.Add(item.Description);
                    }
                }
            }
            #endregion

            #region 数据库权限
            else
            {
                permissionLists = dataUser.GetEffectivePermissionLists(_identity.UserID);
                if (permissionLists.Tables.Count > 0)
                {
                    foreach (DataRow dr in permissionLists.Tables[0].Rows)
                    {
                        permissionListid.Add(Convert.ToInt32(dr["PermissionID"]));
                        permissionsDesc.Add(dr["Description"].ToString());
                    }
                }
            }
            #endregion
            
            rolesKeyValue = dataUser.GetUserRoles4KeyValues(_identity.UserID);
            if (IsVerify)//需要认证
            {
                token = Common.DEncrypt.DEncrypt.GetMD5FromStr(userName);
                timestamp = Common.TimeParser.GetTimeStamp(DateTime.Now);
            }
        }

        private bool isXML()
        {
            string CacheKey = "PermissionsXML";
            object objModel = Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = Common.ConfigHelper.GetConfigBool("IsXMLPermissions");
                    Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(1440), TimeSpan.Zero);
                }
                catch (Exception ex)
                {
                    Log.LogHelper.AddErrorLog(ex.Message, ex.StackTrace);
                }
            }
            return Common.Globals.SafeBool(objModel,false);
        }

        /// <summary>
        /// 当前用户是否属于指定名称的角色
        /// </summary>
        public bool IsInRole(string role)
        {
            return rolesKeyValue != null && rolesKeyValue.ContainsValue(role);
        }
        /// <summary>
        /// 当前用户是否拥有指定的角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public bool HasRole(int roleId)
        {
            return rolesKeyValue != null && rolesKeyValue.ContainsKey(roleId);
        }
        /// <summary>
        /// 当前用户是否拥有指定名称的权限
        /// </summary>
        public bool HasPermission(string permission)
        {
            return permissionsDesc.Contains(permission);
        }
        /// <summary>
        /// 当前用户是否拥有指定的权限
        /// </summary>
        public bool HasPermissionID(int permissionid)
        {
            if (permissionid == -1)
            {
                return true;
            }
            return permissionListid.Contains(permissionid);
        }
        /// <summary>
        /// 验证登录信息
        /// 用户名登录
        /// </summary>
        public static AccountsPrincipal ValidateLogin(string userName, string password)
        {
            int newID;
            byte[] cryptPassword = EncryptPassword(password);
            IData.IUser dataUser = PubConstant.IsSQLServer ? (IUser)new Data.User() : new MySqlData.User();
            if ((newID = dataUser.ValidateLogin(userName, cryptPassword)) > 0)
                return new AccountsPrincipal(newID);
            else
                return null;
        }
        /// <summary>
        /// 验证登录信息
        /// 邮箱登录
        /// </summary>
        public static AccountsPrincipal ValidateLogin4Email(string email, string password)
        {
            int newID;
            byte[] cryptPassword = EncryptPassword(password);
            IData.IUser dataUser = PubConstant.IsSQLServer ? (IUser)new Data.User() : new MySqlData.User();
            if ((newID = dataUser.ValidateLogin4Email(email, cryptPassword)) > 0)
                return new AccountsPrincipal(newID);
            else
                return null;
        }
        /// <summary>
        /// 密码加密
        /// </summary>
        public static byte[] EncryptPassword(string password)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            if (String.IsNullOrEmpty(password))
            {
                return null;
            }
            byte[] hashBytes = encoding.GetBytes(password);
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] cryptPassword = sha1.ComputeHash(hashBytes);
            return cryptPassword;
        }



        #region 验证登录（SAAS）
        public static AccountsPrincipal ValidateLoginEx(string userName, byte[] password, int cacheTime = 30)
        {
            IData.IUser dataUser = PubConstant.IsSQLServer ? (IUser)new Data.User() : new MySqlData.User();
            try
            {
                if ((dataUser.ValidateLogin(userName, password)) > 0)
                {
                    AccountsPrincipal accountsPrincipal = new AccountsPrincipal(userName, true);
                    //存缓存
                    string CacheKey = "ValidateLoginEx-" + accountsPrincipal.Token;

                    YSWL.Common.DataCache.SetCache(CacheKey, accountsPrincipal, DateTime.Now.AddMinutes(cacheTime),
                        TimeSpan.Zero);
                    return accountsPrincipal;

                    return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog(ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// 获取验证信息对象 （SAAS效验使用）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static AccountsPrincipal GetValidateLogin(string token)
        {
            string CacheKey = "ValidateLoginEx-" + token;
            try
            {
                AccountsPrincipal objModel = YSWL.Common.DataCache.GetCache(CacheKey) as AccountsPrincipal;
                return objModel;
            }
            catch(Exception ex)
            {
                Log.LogHelper.AddErrorLog(ex.Message,ex.StackTrace);
            }
            return null;
        }

        #endregion
    }
}
