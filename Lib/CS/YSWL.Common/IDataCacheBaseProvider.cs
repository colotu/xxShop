using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace YSWL.Common
{
    /// <summary>
    /// 数据缓存引擎
    /// </summary>
    public interface IDataCacheBaseProvider
    {
        #region  成员方法

        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        T GetCache<T>(string CacheKey) where T : class;

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        bool SetCache<T>(string CacheKey, T objObject);

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        bool SetCache<T>(string CacheKey, T objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration);


        /// <summary>
        /// 删除当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        bool DeleteCache(string CacheKey);

        /// <summary>
        /// 批量移除缓存
        /// </summary>
        /// <param name="CacheKey"></param>
       bool ClearBatch(string CacheKey);

        /// <summary>
        /// 移除所有的缓存
        /// </summary>
        bool ClearAll(bool IsAutoConn = false);

        #endregion  成员方法
    }
}
