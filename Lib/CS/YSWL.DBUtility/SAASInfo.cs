using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using YSWL.Common;

namespace YSWL.DBUtility
{


    public class SAASInfo
    {
        /// <summary>
        /// 锁的对象
        /// </summary>
        public static object obj = new object();

        /// <summary>
        /// 是否开启自动连接标识key，通过配置文件获取
        /// </summary>
        protected const string KEY_AUTOCONNECTION = "AutoConnection";

        /// <summary>
        /// 系统标识key，从配置文件中获取系统标识
        /// </summary>
        protected const string KEY_SYSTEM_FLAG = "SystemFlag";

        /// <summary> 
        /// 企业的数据库配置数据库链接串
        /// </summary>
        public static string BaseDBStr = ConfigurationManager.AppSettings["BaseConnectionStr"];

        #region 获取登录用户信息

        public static DataSet GetSAASUserInfo(string userName, byte[] encPassword, int userType = 1,
            long enterpriseId = 0)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(
                    " select * from  SA_UserInfo where LoginName=@LoginName and Passworld=@Passworld and UserType=@UserType and State=1   ");
                if (enterpriseId > 0)
                {
                    strSql.Append(" and EnterpriseId=@EnterpriseId ");
                }
                DataSet ds = YSWL.DBUtility.ConnectionStrManage.Query(strSql.ToString(), new SqlParameter[]
                {
                    new SqlParameter("@LoginName", userName),
                    new SqlParameter("@Passworld", encPassword),
                    new SqlParameter("@UserType", userType),
                    new SqlParameter("@EnterpriseId", enterpriseId)
                });
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count > 1)
                {
                    return null;
                }
                else
                {
                    enterpriseId = Common.Globals.SafeLong(ds.Tables[0].Rows[0]["EnterpriseId"], 0);
                    int userId = Common.Globals.SafeInt(ds.Tables[0].Rows[0]["UserId"], 0);
                    Common.CallContextHelper.SetAutoTag(enterpriseId);
                    if (userType == 1 || userType == 3) //客户账号，不区分应用类型
                    {
                        return ds;
                    }
                    var applicationId = YSWL.DBUtility.ConnectionStrManage.GetApplicationId();
                    if (applicationId <= 0)
                    {
                        return null;
                    }
                    //需要判断应用是否已经过期
                    if (IsExistsUserLink(userId, applicationId))
                    {
                        return ds;
                    }
                    else
                    {
                        return null;
                    }
                }
                return ds;

            }
            catch (Exception ex)
            {
                YSWL.Log.LogHelper.AddErrorLog(ex.Message, ex.StackTrace);
                return null;
            }

        }

        #endregion

        #region   添加SAAS用户

        public static bool CreateSAASUser(string userName, byte[] encPassword, string trueName, string phone,
            long enterpriseId, int userType = 1, string applicationids = "")
        {
            //如果存在
            if (IsExistsUser(userName) > 0)
            {
                return false;
            }

            using (SqlConnection connection = new SqlConnection(BaseDBStr))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    object result;
                    try
                    {
                        result =
                            YSWL.DBUtility.ConnectionStrManage.GetSingle4Trans(
                                GenSAASUser(userName, encPassword, trueName, phone, enterpriseId, userType), transaction);

                        int userId = Common.Globals.SafeInt(result, -1);
                        if (String.IsNullOrWhiteSpace(applicationids))
                        {
                            applicationids = YSWL.DBUtility.ConnectionStrManage.GetApplicationId().ToString();
                        }

                        YSWL.DBUtility.ConnectionStrManage.ExecuteSqlTran4Indentity(GenUserLink(userId, enterpriseId, applicationids),
                            transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public static CommandInfo GenSAASUser(string userName, byte[] encPassword, string trueName, string phone,
            long enterpriseId, int usertype = 1)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SA_UserInfo(");
            strSql.Append(
                "LoginName,Passworld,RealName,Moblie,State,UserType,ParentId,EnterpriseName,CreateTime,ModeifyTime,CreateBy,ModifyBy,AdministratorLevel,UserNumber,EnterpriseId,Email,FromTargetType)");
            strSql.Append(" values (");
            strSql.Append(
                "@LoginName,@Passworld,@RealName,@Moblie,@State,@UserType,@ParentId,@EnterpriseName,@CreateTime,@ModeifyTime,@CreateBy,@ModifyBy,@AdministratorLevel,@UserNumber,@EnterpriseId,@Email,@FromTargetType)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters =
            {
                new SqlParameter("@LoginName", SqlDbType.NVarChar, 50),
                new SqlParameter("@Passworld", SqlDbType.Binary, 20),
                new SqlParameter("@RealName", SqlDbType.NVarChar, 80),
                new SqlParameter("@Moblie", SqlDbType.NVarChar, 20),
                new SqlParameter("@State", SqlDbType.SmallInt, 2),
                new SqlParameter("@UserType", SqlDbType.SmallInt, 2),
                new SqlParameter("@ParentId", SqlDbType.BigInt, 8),
                new SqlParameter("@EnterpriseName", SqlDbType.NVarChar, 120),
                new SqlParameter("@CreateTime", SqlDbType.DateTime),
                new SqlParameter("@ModeifyTime", SqlDbType.DateTime),
                new SqlParameter("@CreateBy", SqlDbType.NVarChar, 50),
                new SqlParameter("@ModifyBy", SqlDbType.NVarChar, 50),
                new SqlParameter("@AdministratorLevel", SqlDbType.SmallInt, 2),
                new SqlParameter("@UserNumber", SqlDbType.NVarChar, 50),
                new SqlParameter("@EnterpriseId", SqlDbType.BigInt, 8),
                new SqlParameter("@Email", SqlDbType.NVarChar, 50),
                new SqlParameter("@FromTargetType", SqlDbType.SmallInt, 2)
            };
            parameters[0].Value = userName;
            parameters[1].Value = encPassword;
            parameters[2].Value = trueName;
            parameters[3].Value = phone;
            parameters[4].Value = 1;
            parameters[5].Value = usertype;
            parameters[6].Value = 0;
            parameters[7].Value = "";
            parameters[8].Value = DateTime.Now;
            parameters[9].Value = DateTime.Now;
            parameters[10].Value = trueName;
            parameters[11].Value = trueName;
            parameters[12].Value = 0;
            parameters[13].Value = "";
            parameters[14].Value = enterpriseId;
            parameters[15].Value = "";
            parameters[16].Value = 2;

            return new CommandInfo(strSql.ToString(), parameters);
        }


        public static List<CommandInfo> GenUserLink(int userId, long enterpriseId, string applicationIds)
        {
            List<CommandInfo> commandInfos = new List<CommandInfo>();
            var appArry = applicationIds.Split('|');
            foreach (var item in appArry)
            { 
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SA_AccountLinkSystem(");
                strSql.Append("UserId,EnterpriseTag,EnterpriseId,ApplicationId,Url,ApplicationName)");
                strSql.Append(" select  ");
                strSql.Append(
                    "@UserId,@EnterpriseTag,@EnterpriseId,@ApplicationId,UrlAddress,ApplicationName from   SA_EnterpriseBuyJurisdiction where EnterpriseId=@EnterpriseId and ApplicationId=@ApplicationId ");

                SqlParameter[] parameters =
                {
                new SqlParameter("@UserId", SqlDbType.BigInt, 8),
                new SqlParameter("@EnterpriseTag", SqlDbType.NVarChar, 70),
                new SqlParameter("@EnterpriseId", SqlDbType.BigInt, 8),
                new SqlParameter("@ApplicationId", SqlDbType.Int, 4)
            };
                parameters[0].Value = userId;
                parameters[1].Value = Common.DEncrypt.DEncrypt.GetEncryptionStr(enterpriseId);
                parameters[2].Value = enterpriseId;
                parameters[3].Value = Common.Globals.SafeInt(item, 0); ;
                commandInfos.Add(new CommandInfo(strSql.ToString(), parameters));
            }
            return commandInfos;
        }

        #endregion

        #region  SAAS 用户是否存在

        public static int IsExistsUser(string userName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserId  from SA_UserInfo");
            strSql.Append(" where LoginName=@LoginName");
            SqlParameter[] parameters =
            {
                new SqlParameter("@LoginName", SqlDbType.NVarChar, 50)
            };
            parameters[0].Value = userName;

            object obj = YSWL.DBUtility.ConnectionStrManage.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Common.Globals.SafeInt(obj, 0);
            }
        }

        public static bool IsExistsUserLink(int userId, int applicationId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  count(1)  from SA_AccountLinkSystem");
            strSql.Append(" where UserId=@UserId and ApplicationId=@ApplicationId");
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", SqlDbType.Int, 4),
                new SqlParameter("@ApplicationId", SqlDbType.Int, 4)
            };
            parameters[0].Value = userId;
            parameters[1].Value = applicationId;
            return YSWL.DBUtility.ConnectionStrManage.Exists(strSql.ToString(), parameters);
        }

        #endregion

        #region  修改密码同步至SAAS系统

        public static bool SetPassword(string userName, byte[] encPassword)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SA_UserInfo set ");
            strSql.Append("Passworld=@Passworld ");
            strSql.Append(" where LoginName=@userName");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Passworld", SqlDbType.Binary, 20),
                new SqlParameter("@userName", SqlDbType.NVarChar, 50)
            };
            parameters[0].Value = encPassword;
            parameters[1].Value = userName;
            int rows = YSWL.DBUtility.ConnectionStrManage.ExecuteSql(strSql.ToString(), parameters);
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

        #region 更新用户信息

        /// <summary>
        /// 更新SAAS用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="trueName"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool UpdateUser(string userName, byte[] encPassword, string trueName, string phone,
            long enterpriseId,bool activity)
        {
            //如果存在
            if (IsExistsUser(userName) == 0)
            {
                return CreateSAASUser(userName, encPassword, trueName, phone, enterpriseId);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update SA_UserInfo set ");
                strSql.Append("RealName=@RealName, ");
                strSql.Append("Moblie=@Moblie ,");
                strSql.Append("State=@State  ");
                strSql.Append(" where LoginName=@userName");
                SqlParameter[] parameters =
                {
                    new SqlParameter("@RealName", SqlDbType.NVarChar, 200),
                    new SqlParameter("@Moblie", SqlDbType.NVarChar, 20),
                     new SqlParameter("@State", SqlDbType.SmallInt, 4),
                    new SqlParameter("@userName", SqlDbType.NVarChar, 50)
                };
                parameters[0].Value = trueName;
                parameters[1].Value = phone;
                parameters[2].Value = activity?1:3;
                parameters[3].Value = userName;
                int rows = YSWL.DBUtility.ConnectionStrManage.ExecuteSql(strSql.ToString(), parameters);
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

        #endregion

        #region 应用是否开通

        /// <summary>
        /// APP 是否开通应用
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="enterpeiseId"></param>
        /// <returns></returns>
        public static bool AppIsOpenCache(string tag, int enterpeiseId)
        {
            bool isOpen = false;
            string key = tag + "_" + enterpeiseId;
            string isOpenStr = "";

            try
            {
                isOpenStr = YSWL.RedisClient.RedisBase.GetValue(key);
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog("获取是否开启ERP失败" + ex.Message, "错误信息：" + ex.StackTrace);
                throw;
            }

            if (String.IsNullOrWhiteSpace(isOpenStr))
            {
                try
                {
                    isOpen = AppIsOpen(tag, enterpeiseId);
                    //写Redis
                    YSWL.RedisClient.RedisBase.SetValue(key, isOpen.ToString(), DateTime.MaxValue);
                }
                catch (Exception ex)
                {
                    Log.LogHelper.AddTextLog("获取是否开启ERP失败" + ex.Message, "错误信息：" + ex.StackTrace);
                    throw ex;
                }
            }
            else
            {
                isOpen = Common.Globals.SafeBool(isOpenStr, false);
            }
            return isOpen;
        }

        public static bool AppIsOpen(string tag, int enterpeiseId)
        {
            int applicationId = YSWL.DBUtility.ConnectionStrManage.GetApplicationId(tag);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  count(1)  from SA_EnterpriseBuyJurisdiction");
            strSql.Append(" where EnterpriseId=@EnterpriseId and ApplicationId=@ApplicationId and (IsTrial=1 or (IsTrial=0 and   EndTime>@EndTime))");
            SqlParameter[] parameters =
            {
                new SqlParameter("@EnterpriseId", SqlDbType.Int, 4),
                new SqlParameter("@ApplicationId", SqlDbType.Int, 4),
                new SqlParameter("@EndTime", SqlDbType.DateTime)
            };
            parameters[0].Value = enterpeiseId;
            parameters[1].Value = applicationId;
            parameters[2].Value = DateTime.Now;

            object obj = YSWL.DBUtility.ConnectionStrManage.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return false;
            }
            else
            {
                return Common.Globals.SafeInt(obj, 0) > 0;
            }
        }

        #endregion

        #region 获取SaaS参数

        public static string GetSystemValue(string Keyname)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Value from SA_Config_System ");
            strSql.Append(" where Keyname=@Keyname ");
            SqlParameter[] parameters =
            {
                new SqlParameter("@Keyname", SqlDbType.NVarChar)
            };
            parameters[0].Value = Keyname;
            object obj = YSWL.DBUtility.ConnectionStrManage.GetSingle(strSql.ToString(), parameters);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }

        public static bool GetSystemBoolValue(string Keyname)
        {
            return Globals.SafeBool(GetSystemValue(Keyname), false);
        }

        #endregion

        #region   获取SAAS企业列表（正常企业）

        public static DataSet GetSAASEnterprises()
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select * from  FROM Ms_Enterprise where  Status=1  ");
                DataSet ds = YSWL.DBUtility.ConnectionStrManage.Query(strSql.ToString());
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count > 1)
                {
                    return null;
                }
                return ds;
            }
            catch (Exception ex)
            {
                YSWL.Log.LogHelper.AddErrorLog(ex.Message, ex.StackTrace);
                return null;
            }

        }

        public static readonly string[] DOMAIN_SAAS = new string[] { "ys56.com", "yuns56.cn" };
        public static readonly string[] DOMAIN_SAAS_SEC = new string[] { "saas.ys56.com", "saas.yuns56.cn" };
        public static int GetSAASEnterpriseIdByDomain(string domain)
        {
            if (string.IsNullOrWhiteSpace(domain) || DOMAIN_SAAS_SEC.Contains(domain))
            {
                //FileManage.WriteText(new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " SAASInfo GetSAASEnterpriseIdByDomain:" + domain));
                return 0;
            }

            string key =  "SAAS_" + domain;
            int enterpriseId = -1;
            try
            {
                enterpriseId = Globals.SafeInt(YSWL.RedisClient.RedisBase.GetValue(key), -1);
                if (enterpriseId == -1)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(" select top 1 EnterpriseID FROM Ms_Enterprise where CustomDomain=@CustomDomain");
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@CustomDomain", SqlDbType.NVarChar)
                    };
                    parameters[0].Value = domain;
                    object obj = YSWL.DBUtility.ConnectionStrManage.GetSingle(strSql.ToString(), parameters);
                    enterpriseId = Globals.SafeInt(obj, -1);
                    if (enterpriseId > 0)
                    {
                        //FileManage.WriteText(new System.Text.StringBuilder("SAASInfo GetSAASEnterpriseIdByDomain enterpriseId:" + enterpriseId));
                        RedisClient.RedisBase.SetValue(key, enterpriseId, DateTime.Now.AddHours(5));
                    }
                }
            }
            catch (Exception ex)
            {
                YSWL.Log.LogHelper.AddErrorLog(ex.Message, ex.StackTrace);
                return -1;
            }
            return enterpriseId;
        }

        public static bool ClearSAASEnterpriseDomain(string domain)
        {
            string key = "SAAS_" + domain;
            try
            {
               return RedisClient.RedisBase.Remove(key);
            }
            catch (Exception ex)
            {
                YSWL.Log.LogHelper.AddErrorLog(ex.Message, ex.StackTrace);
                return false;
            }
        }
        #endregion

        #region  应用限制访问相关方法

        /// <summary>
        /// 是否可以增加客户 
        /// </summary>
        /// <param name="usertype">用户类型 1：客户类型 2：管理员类型  3：业务员类型</param>
        /// <returns></returns>
        public static bool IsCanAddCust(long enterpriseId, string appTag = "")
        {
            int applicationId = YSWL.DBUtility.ConnectionStrManage.GetApplicationId(appTag);
            if (IsBuy(applicationId, enterpriseId)) //是否已经购买了
            {
                return true;
            }
            else
            {
                //获取免费的客户数
                int freeCusts = GetFreeCusts();
                //获取当前的客户数
                int countCust = GetCacheCusts(enterpriseId);
                return freeCusts > countCust;
            }
        }

        public static bool IsCanAddSales(long enterpriseId, string appTag = "")
        {
            int applicationId = YSWL.DBUtility.ConnectionStrManage.GetApplicationId(appTag);
            if (IsBuy(applicationId, enterpriseId)) //是否已经购买了
            {
                return true;
            }
            else
            {
                //获取免费的客户数
                int freeSales = GetFreeSalses();
                //获取当前的客户数
                int countSales = GetCacheSales(enterpriseId);
                return freeSales > countSales;
            }
        }

        /// <summary>
        /// 获取企业客户数 (Redis 缓存)
        /// </summary>
        /// <returns></returns>
        public static int GetCacheCusts(long enterpriseId)
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                CancelEnterpriseKey = true,
                DefaultDb = 0,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string CacheKey = "SAAS_EnterpriseCusts_" + enterpriseId;
            object objModel = coreBll.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetUserCounts(1, enterpriseId);
                    if (objModel != null)
                    {
                        coreBll.SetCache(CacheKey, objModel, DateTime.MaxValue, TimeSpan.Zero);
                    }
                }
                catch
                {
                }
            }
            return Common.Globals.SafeInt(objModel, 0);
        }

        /// <summary>
        /// 清除客户缓存数
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public static bool ClearCacheCusts(long enterpriseId)
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                CancelEnterpriseKey = true,
                DefaultDb = 0,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string CacheKey = "SAAS_EnterpriseCusts_" + enterpriseId;
            return coreBll.DeleteCache(CacheKey);
        }

        /// <summary>
        /// 获取企业员工数(Redis 缓存)
        /// </summary>
        /// <returns></returns>
        public static int GetCacheSales(long enterpriseId)
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                CancelEnterpriseKey = true,
                DefaultDb = 0,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string CacheKey = "SAAS_EnterpriseSales_" + enterpriseId;
            object objModel = coreBll.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetUserCounts(3, enterpriseId);
                    if (objModel != null)
                    {
                        coreBll.SetCache(CacheKey, objModel, DateTime.MaxValue, TimeSpan.Zero);
                    }
                }
                catch
                {
                }
            }
            return Common.Globals.SafeInt(objModel, 0);
        }

        /// <summary>
        /// 清除员工缓存数
        /// </summary>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public static bool ClearCacheSales(long enterpriseId)
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                CancelEnterpriseKey = true,
                DefaultDb = 0,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string CacheKey = "SAAS_EnterpriseSales_" + enterpriseId;
            return coreBll.DeleteCache(CacheKey);
        }

        /// <summary>
        /// 获取用户数量
        /// </summary>
        /// <param name="usertype"></param>
        /// <returns></returns>
        private static int GetUserCounts(int usertype, long enterpriseId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  count(1)  from SA_UserInfo");
            strSql.Append(" where EnterpriseId=@EnterpriseId and UserType=@UserType");
            SqlParameter[] parameters =
            {
                new SqlParameter("@EnterpriseId", SqlDbType.Int, 8),
                new SqlParameter("@UserType", SqlDbType.Int, 4)
            };
            parameters[0].Value = enterpriseId;
            parameters[1].Value = usertype;
            object obj = YSWL.DBUtility.ConnectionStrManage.GetSingle(strSql.ToString(), parameters);

            return Common.Globals.SafeInt(obj, 0);
        }

        /// <summary>
        /// 购买的应用是否过期
        /// </summary>
        /// <param name="appTag"></param>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public static bool IsExpired(int applicationId, long enterpriseId)
        {
            DateTime endTime = GetEndTime(applicationId, enterpriseId);
            return endTime < DateTime.Now;
        }
        /// <summary>
        /// 清空过期缓存
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public static bool DeleteExpiredCache(int applicationId, long enterpriseId)
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                DefaultDb = 0,
                CancelEnterpriseKey = true,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string CacheKey = "SAAS_AppEndTime_" + applicationId + "_" + enterpriseId;
            return coreBll.DeleteCache(CacheKey);
        }


        private static DateTime GetEndTime(int applicationId, long enterpriseId)
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                DefaultDb = 0,
                CancelEnterpriseKey = true,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string CacheKey = "SAAS_AppEndTime_" + applicationId + "_" + enterpriseId;
            object objModel = coreBll.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("select EndTime  from SA_EnterpriseBuyJurisdiction");
                    strSql.Append(" where EnterpriseId=@EnterpriseId and ApplicationId=@ApplicationId  and IsTrial=0");
                    SqlParameter[] parameters = {
                    new SqlParameter("@EnterpriseId", SqlDbType.Int,4),
                    new SqlParameter("@ApplicationId", SqlDbType.Int,4)
            };
                    parameters[0].Value = enterpriseId;
                    parameters[1].Value = applicationId;

                    objModel = YSWL.DBUtility.ConnectionStrManage.GetSingle(strSql.ToString(), parameters);

                    if (objModel != null)
                    {
                        coreBll.SetCache(CacheKey, objModel, DateTime.MaxValue, TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return Common.Globals.SafeDateTime(objModel, DateTime.Now);
        }

        /// <summary>
        /// 应用是否购买
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public static bool IsBuy(int applicationId, long enterpriseId)
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                DefaultDb = 0,
                CancelEnterpriseKey = true,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string CacheKey = "SAAS_AppIsBuy_" + applicationId + "_" + enterpriseId;
            object objModel = coreBll.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("select  count(1)  from SA_EnterpriseBuyJurisdiction");
                    strSql.Append(" where EnterpriseId=@EnterpriseId and ApplicationId=@ApplicationId  and IsTrial=0");
                    SqlParameter[] parameters = {
                    new SqlParameter("@EnterpriseId", SqlDbType.Int,4),
                    new SqlParameter("@ApplicationId", SqlDbType.Int,4)
            };
                    parameters[0].Value = enterpriseId;
                    parameters[1].Value = applicationId;
                    object obj = YSWL.DBUtility.ConnectionStrManage.GetSingle(strSql.ToString(), parameters);
                    objModel = Common.Globals.SafeInt(obj, 0) > 0;
                    coreBll.SetCache(CacheKey, objModel, DateTime.MaxValue, TimeSpan.Zero);
                }
                catch { }
            }
            return Common.Globals.SafeBool(objModel, false);
        }
        /// <summary>
        /// 设置购买状态缓存
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="enterpriseId"></param>
        /// <returns></returns>
        public static bool SetBuyCache(int applicationId, long enterpriseId)
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                CancelEnterpriseKey = true,
                DefaultDb = 0,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string CacheKey = "SAAS_AppIsBuy_" + applicationId + "_" + enterpriseId;
            return coreBll.SetCache(CacheKey, true);
        }

        /// <summary>
        /// 免费的客户数
        /// </summary>
        /// <returns></returns>
        private static int GetFreeCusts()
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                CancelEnterpriseKey = true,
                DefaultDb = 0,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string CacheKey = "SAAS_FreeEnterpriseCusts";
            object objModel = coreBll.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetSysValue(CacheKey);
                    if (objModel != null)
                    {
                        coreBll.SetCache(CacheKey, objModel, DateTime.MaxValue, TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return Common.Globals.SafeInt(objModel, 10);
        }
        /// <summary>
        /// 免费的员工数 （业务员数）
        /// </summary>
        /// <returns></returns>
        private static int GetFreeSalses()
        {
            YSWL.Common.DataCacheCore coreBll = new DataCacheCore(new CacheOption
            {
                CacheType = CacheType.Redis,
                CancelProductKey = true,
                CancelEnterpriseKey = true,
                DefaultDb = 0,
                ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
                ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            });
            string CacheKey = "SAAS_FreeEnterpriseSales";
            object objModel = coreBll.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetSysValue(CacheKey);
                    if (objModel != null)
                    {
                        coreBll.SetCache(CacheKey, objModel, DateTime.MaxValue, TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return Common.Globals.SafeInt(objModel, 5);
        }

        private static string GetSysValue(string key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Value from SA_Config_System ");
            strSql.Append(" where Keyname=@Keyname ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Keyname", SqlDbType.NVarChar)};
            parameters[0].Value = key;

            object obj = YSWL.DBUtility.ConnectionStrManage.GetSingle(strSql.ToString(), parameters);
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }




        #endregion


    }
}
