using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace YSWL.Common
{
   public class IISDataCache : IDataCacheBaseProvider
    {
        /// <summary>
		/// 获取当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <returns></returns>
		public  T GetCache<T>(string CacheKey) where T : class
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return (T)objCache[CacheKey];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public  bool SetCache<T>(string CacheKey, T objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
            return true;
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public  bool SetCache<T>(string CacheKey, T objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
            return true;
        }

        /// <summary>
        /// 删除当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public  bool DeleteCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Remove(CacheKey);
            return true;
        }
        /// <summary>
        /// 批量移除缓存
        /// </summary>
        /// <param name="CacheKey"></param>
        public  bool ClearBatch(string CacheKey)
        {
            IDictionaryEnumerator de = HttpRuntime.Cache.GetEnumerator();
            ArrayList list = new ArrayList();
            while (de.MoveNext())
            {
                if (de.Key.ToString().Contains(CacheKey))
                {
                    list.Add(de.Key.ToString());
                }
            }
            foreach (string key in list)
            {
                HttpRuntime.Cache.Remove(key);
            }
            return true;
        }
        /// <summary>
        /// 移除所有的缓存
        /// </summary>
        public bool ClearAll(bool IsAutoConn = false)
        {
            IDictionaryEnumerator de = HttpRuntime.Cache.GetEnumerator();
            ArrayList list = new ArrayList();
            while (de.MoveNext())
            {
                if (IsAutoConn) //开启自动链接
                {
                    string tag = Common.CallContextHelper.GetClearTag();
                    if (de.Key.ToString().EndsWith("-" + tag) && !de.Key.ToString().StartsWith("ValidateLoginEx-"))
                    {
                        list.Add(de.Key.ToString());
                    }
                }
                else
                {
                    list.Add(de.Key.ToString());
                }
            }

            foreach (string key in list)
            {
                HttpRuntime.Cache.Remove(key);
            }
            return true;
        }
    }
}
