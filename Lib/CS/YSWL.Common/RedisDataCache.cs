using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Common
{
    public class RedisDataCache : IDataCacheBaseProvider
    {
        /// <summary>
        /// 构造Redis连接池,使用DB:1
        /// </summary>
        private YSWL.RedisClient.RedisClientPool redisClientPool;

        public RedisDataCache(string readWriteHosts, string readOnlyHosts, int defaultDb = 0)
        {
            redisClientPool = new YSWL.RedisClient.RedisClientPool(readWriteHosts, readOnlyHosts, defaultDb);
        }
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public T GetCache<T>(string CacheKey) where T : class
        {
            return redisClientPool.GetValue<T>(CacheKey);
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public bool SetCache<T>(string CacheKey, T objObject)
        {
            return redisClientPool.SetValue<T>(CacheKey, objObject);
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public bool SetCache<T>(string CacheKey, T objObject, DateTime dateTime, TimeSpan timeSpan)
        {
            if (DateTime.MinValue != dateTime)
                return redisClientPool.SetValue<T>(CacheKey, objObject, dateTime);
            if (timeSpan != TimeSpan.Zero)
                return redisClientPool.SetValue<T>(CacheKey, objObject, timeSpan);
            return redisClientPool.SetValue<T>(CacheKey, objObject);
        }

        /// <summary>
        /// 删除当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public bool DeleteCache(string CacheKey)
        {
            return redisClientPool.Remove(CacheKey);
        }
        /// <summary>
        /// 批量移除缓存
        /// </summary>
        /// <param name="CacheKey"></param>
        [Obsolete]
        public bool ClearBatch(string CacheKey)
        {
            return true;
        }
        /// <summary>
        /// 移除所有的缓存
        /// </summary>
        public bool ClearAll(bool IsAutoConn = false)
        {
#warning 未处理SaaS企业清缓存问题, 方案待定
            redisClientPool.FlushDb();
            return true;
        }
    }
}
