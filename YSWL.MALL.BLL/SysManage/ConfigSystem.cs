using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.SysManage;
using YSWL.Common;
using YSWL.DBUtility;
#pragma warning disable 612

namespace YSWL.MALL.BLL.SysManage
{
    /// <summary>
    /// 系统参数配置
    /// </summary>
    public class ConfigSystem
    {
        private static IConfigSystem dal = DASysManage.CreateConfigSystem();
        private static DataCacheCore dataCache = new DataCacheCore(new CacheOption
        {
            CacheType = SAASInfo.GetSystemBoolValue("RedisCacheUse") ? CacheType.Redis : CacheType.IIS,
            ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
            ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            CancelProductKey = true,
            DefaultDb = 1
        });

        #region  Method

        /// <summary>
        /// Whether there is Exists
        /// </summary>
        public static bool Exists(string Keyname)
        {
            return dal.Exists(Keyname);
        }

        /// <summary>
        /// Add a record
        /// </summary>
        public static int Add(string Keyname, string Value, string Description)
        {
            return dal.Add(Keyname, Value, Description);
        }
        /// <summary>
        /// Add a record
        /// </summary>
        public static int Add(string Keyname, string Value, string Description, ApplicationKeyType KeyType)
        {
            return dal.Add(Keyname, Value, Description, KeyType);
        }

        public static void Update(int ID, string Keyname, string Value, string Description)
        {
            SetValueByCache(Keyname, Value);
            dal.Update(ID, Keyname, Value, Description);
        }

        /// <summary>
        /// Update a record
        /// </summary>
        public static bool Update(string Keyname, string Value, string Description)
        {
            SetValueByCache(Keyname, Value);
            return dal.Update(Keyname, Value, Description);
        }

        /// <summary>
        /// Update a record
        /// </summary>
        public static bool Update(string Keyname, string Value)
        {
            SetValueByCache(Keyname, Value);
            return dal.Update(Keyname, Value);
        }

        /// <summary>
        /// Update a record
        /// </summary>
        public static bool Update(string Keyname, string Value, ApplicationKeyType KeyType)
        {
            SetValueByCache(Keyname, Value);
            return dal.Update(Keyname, Value, KeyType);
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        public static void Delete(int ID)
        {
            dal.Delete(ID);
        }

        /// <summary>
        /// Get an object entity
        /// </summary>
        public static string GetValue(int ID)
        {
            return dal.GetValue(ID);
        }

        /// <summary>
        /// Get an object entity
        /// </summary>
        /// <param name="Keyname"></param>
        /// <returns></returns>
        public static string GetValue(string Keyname)
        {
            return dal.GetValue(Keyname);
        }

        /// <summary>
        ///  Get an object entity，From cache
        /// </summary>
        /// <param name="Keyname"></param>
        /// <returns></returns>
        public static string GetValueByCache(string Keyname)
        {
            object cache = dataCache.GetCache(Keyname);
            if (cache != null)
            {
                return cache.ToString();
            }
            cache = GetValue(Keyname);
            dataCache.SetCache(Keyname, cache, DateTime.Now.AddMinutes(Globals.SafeInt(GetCacheTimeByCache("CacheTime"), 30)), TimeSpan.Zero);
            return cache.ToString();
        }

        public static int GetCacheTimeByCache(string Keyname)
        {
            object cache = dataCache.GetCache(Keyname);
            if (!string.IsNullOrWhiteSpace(cache?.ToString()))
            {
                return Globals.SafeInt(cache, 30);
            }
            cache = GetValue(Keyname);
            int cacheTime = Globals.SafeInt(cache, 30);
            dataCache.SetCache(Keyname, cacheTime, DateTime.Now.AddMinutes(cacheTime), TimeSpan.Zero);
            return cacheTime;
        }

        /// <summary>
        ///  Get an object entity，From cache
        /// </summary>
        /// <param name="Keyname"></param>
        /// <returns></returns>
        public static string GetValueByCache(string Keyname, ApplicationKeyType KeyType)
        {
            string cache = dataCache.GetCache(Keyname);
            if (cache != null)
            {
                return cache;
            }
            cache = GetValue(Keyname);
            int cacheTime = Globals.SafeInt(GetValueByCache("CacheTime"), 30);
            dataCache.SetCache(Keyname, cache, DateTime.Now.AddMinutes(cacheTime), TimeSpan.Zero);
            return cache;
        }

        /// <summary>
        ///  Get an object entity for INT，From cache
        /// </summary>
        /// <param name="Keyname"></param>
        /// <remarks>Default -1</remarks>
        public static int GetIntValueByCache(string Keyname)
        {
            dynamic cache = dataCache.GetCache(Keyname);
            if (cache != null)
            {
                return Globals.SafeInt(cache, -1);
            }
            cache = Globals.SafeInt(GetValue(Keyname), -1);
            int cacheTime = Globals.SafeInt(GetValueByCache("CacheTime"), 30);
            dataCache.SetCache(Keyname, cache, DateTime.Now.AddMinutes(cacheTime), TimeSpan.Zero);
            return cache;
        }

        /// <summary>
        ///  Get an object entity for bool，From cache
        /// </summary>
        /// <param name="Keyname"></param>
        /// <remarks>Default false</remarks>
        public static bool GetBoolValueByCache(string Keyname)
        {
            dynamic cache = dataCache.GetCache(Keyname);
            if (cache != null)
            {
                return Globals.SafeBool(cache, false);
            }
            cache = Globals.SafeBool(GetValue(Keyname), false);
            int cacheTime = Globals.SafeInt(GetValueByCache("CacheTime"), 30);
            dataCache.SetCache(Keyname, cache, DateTime.Now.AddMinutes(cacheTime), TimeSpan.Zero);
            return cache;
        }

        /// <summary>
        ///  Get an object entity for bool，From cache
        /// </summary>
        /// <param name="Keyname"></param>
        /// <remarks>Default -1</remarks>
        public static decimal GetDecimalValueByCache(string Keyname)
        {
            dynamic cache = dataCache.GetCache(Keyname);
            if (cache != null)
            {
                return Globals.SafeDecimal(cache, decimal.MinusOne);
            }
            cache = Globals.SafeDecimal(GetValue(Keyname), decimal.MinusOne);
            int cacheTime = Globals.SafeInt(GetValueByCache("CacheTime"), 30);
            dataCache.SetCache(Keyname, cache, DateTime.Now.AddMinutes(cacheTime), TimeSpan.Zero);
            return cache;
        }


        /// <summary>
        /// 回写缓存
        /// </summary>
        /// <param name="keyname"></param>
        /// <param name="value"></param>
        public static void SetValueByCache(string keyname, dynamic value)
        {
            if (value == null) return;

            int cacheTime = Globals.SafeInt(GetValueByCache("CacheTime"), 30);
            dataCache.SetCache(keyname, value, DateTime.Now.AddMinutes(cacheTime), TimeSpan.Zero);
        }

        /// <summary>
        /// Query data list
        /// </summary>
        public static DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }


        #endregion  Method

        #region MethodEx

        /// <summary>
        /// 智能进行新增或更新操作
        /// </summary>
        /// <remarks>当KeyType为None时, 将Update Description字段</remarks>
        public static bool Modify(string keyname, string value, string description = "", ApplicationKeyType keyType = ApplicationKeyType.None)
        {
            if (string.IsNullOrWhiteSpace(description)) description = keyname;

            if (keyType == ApplicationKeyType.None)
            {
                if (Exists(keyname)) return Update(keyname, value, description);
                return Add(keyname, value, description) > 0;
            }

            if (Exists(keyname)) return Update(keyname, value);
            return Add(keyname, value, description, keyType) > 0;
        }

        /// <summary>
        /// clear the hashtable key by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ClearCacheByKey(string key)
        {
            try
            {
                return dataCache.DeleteCache(key);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void UpdateConnectionString(string connectionString)
        {
            dal.UpdateConnectionString(connectionString);
        }
        #endregion

        public static string GetDescription(string keyword)
        {
            return dal.GetDescription(keyword);
        }
    }
}
