using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using System.Configuration;
using System.Net.Sockets;
using YSWL.RedisClient;

namespace YSWL.RedisClient
{
    /// <summary>
    /// 操作redis基类
    /// </summary>
    public class RedisBase
    {
        public static int maxWritePoolSize = 100;

        public static int maxReadPoolSize = 100;

        /// <summary>
        /// 读写主库
        /// </summary>
        private static string[] ReadWriteHosts
        {
            get
            {
                try
                {
                    string readWriteTemp = ConfigurationManager.AppSettings["readWriteHosts"];
                    if (!string.IsNullOrEmpty(readWriteTemp))
                    {
                        return readWriteTemp.Split(new char[] { ';' });
                    }
                    throw new Exception("必须设置读写Ip地址");
                }
                catch (Exception ex)
                {
                    Log.LogHelper.AddErrorLog(ex.Message,ex.StackTrace);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 只读重库
        /// </summary>
        private static string[] ReadOnlyHosts
        {
            get
            {
                try
                {
                    string readOnlyTemp = ConfigurationManager.AppSettings["readOnlyHosts"];
                    if (!string.IsNullOrEmpty(readOnlyTemp))
                    {
                        return readOnlyTemp.Split(new char[] {';'});
                    }
                }
                catch(Exception ex)
                {
                    Log.LogHelper.AddErrorLog(ex.Message, ex.StackTrace);
                    throw ex;
                }
                return null;
            }
        }

        static RedisBase()
        {
            prcm = CreateManager(ReadWriteHosts, ReadOnlyHosts);
        }

        #region redis池信息

        public static PoolRedisClientManagerExt prcm;

        private static PoolRedisClientManagerExt CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            try
            {
                return new PoolRedisClientManagerExt(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
                {
                    MaxWritePoolSize = maxWritePoolSize,
                    MaxReadPoolSize = maxReadPoolSize,
                    AutoStart = true,
                });
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog(ex.Message,ex.StackTrace);
                throw;
            }
          
        }
        #endregion

        #region 字符串或单实体操作

        /// <summary> 
        /// 设置单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <param name="timeSpan"></param> 
        /// <returns></returns> 
        public static bool SetValue<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Set<T>(key, t, new TimeSpan(1, 0, 0));
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
            }
            return false;
        }

        /// <summary>
        /// 设置单体并设置过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static bool SetValue<T>(string key, T t, TimeSpan timeSpan)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Set<T>(key, t, timeSpan);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
            }
            return false;
        }

        /// <summary>
        /// 设置单体并设置过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static bool SetValue<T>(string key, T t, DateTime endTime)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Set<T>(key, t, endTime);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
            }
            return false;
        }

        /// <summary> 
        /// 获取单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static T GetValue<T>(string key) where T : class
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Get<T>(key);
                }
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog(ex.Message,ex.StackTrace);
                //记录日志
                SwitchWriteRedis();
                return default(T);
            }
        }

        /// <summary> 
        /// 移除单体 
        /// </summary> 
        /// <param name="key"></param> 
        public static bool Remove(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Remove(key);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
                return false;
            }
        }

        #endregion

        #region 简单类型操作
        /// <summary>
        /// 获取KEY 值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string  GetValue(string key)
        {
            try
            {
                return GetValue<String>(key);
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog(ex.Message,ex.StackTrace);
                throw;
            }
          
        }
 
        #endregion 

        #region List集合相关操作

        /// <summary>
        /// 添加list集合信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public static void List_Add<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    redisTypedClient.AddItemToList(redisTypedClient.Lists[key], t);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
            }
        }


        /// <summary>
        /// 删除list集合信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool List_Remove<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    return redisTypedClient.RemoveItemFromList(redisTypedClient.Lists[key], t) > 0;
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
                return false;
            }
        }

        /// <summary>
        /// 删除所有list集合信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public static void List_RemoveAll<T>(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    redisTypedClient.Lists[key].RemoveAll();
                }
            }
            catch (Exception ex)
            {
                //记录日志  
                SwitchWriteRedis();
            }
        }


        /// <summary>
        /// 获取list数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int List_Count(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetReadOnlyClient())
                {
                    return redis.GetListCount(key);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchReadRedis();
                return 0;
            }
        }

        /// <summary>
        /// 分页功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> List_GetRange<T>(string key, int start, int count)
        {
            try
            {
                using (IRedisClient redis = prcm.GetReadOnlyClient())
                {
                    var c = redis.GetTypedClient<T>();
                    return c.Lists[key].GetRange(start, start + count - 1);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchReadRedis();
                return null;
            }
        }



        public static List<T> List_GetList<T>(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetReadOnlyClient())
                {
                    var c = redis.GetTypedClient<T>();
                    return c.Lists[key].GetRange(0, c.Lists[key].Count);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchReadRedis();
                return null;
            }
        }

        public static List<T> List_GetList<T>(string key, int pageIndex, int pageSize)
        {
            int start = pageSize * (pageIndex - 1);
            return List_GetRange<T>(key, start, pageSize);
        }

        /// <summary> 
        /// 设置缓存过期 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static void List_SetExpire(string key, DateTime datetime)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    redis.ExpireEntryAt(key, datetime);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
            }
        }
        #endregion

        #region Set集合操作
        public static void Set_Add<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    redisTypedClient.Sets[key].Add(t);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
            }
        }

        /// <summary>
        /// 判断set中是否存在指定可以
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Set_Contains<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    return redisTypedClient.Sets[key].Contains(t);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
                return false;
            }
        }

        /// <summary>
        /// 从set中移除指定KEY 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Remove<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    return redisTypedClient.Sets[key].Remove(t);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
                return false;
            }
        }
        #endregion

        #region Hash操作

        /// <summary> 
        /// 判断某个数据是否已经被缓存 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Exist<T>(string key, string dataKey)
        {
            try
            {
                using (IRedisClient redis = prcm.GetReadOnlyClient())
                {
                    return redis.HashContainsEntry(key, dataKey);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchReadRedis();
                return false;
            }
        }

        /// <summary> 
        /// 存储数据到hash表 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Set<T>(string key, string dataKey, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                    return redis.SetEntryInHash(key, dataKey, value);
                }
            }
            catch (Exception ex)
            {
                SwitchWriteRedis();
                return false;
                //throw ex;
            }
        }

        #region 事务操作

        /// <summary>
        /// 获取事务对象
        /// </summary>
        /// <param name="redisClient"></param>
        /// <returns></returns>
        public static IRedisTransaction GetRedisTransaction(IRedisClient redisClient)
        {
            return redisClient.CreateTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="trans"></param>
        public static void CommitTransaction(IRedisTransaction trans)
        {
            trans.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="trans"></param>
        public static void RollbackTransaction(IRedisTransaction trans)
        {
            trans.Rollback();
        }

        /// <summary>
        /// 获取redis事务脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="trans"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Func<IRedisClient, bool> GetRedisCommand<T>(string key, string dataKey, T t)
        {
            string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
            return (r) => { return r.SetEntryInHash(key, dataKey, value); };
        }

        /// <summary>
        /// 删除指定hash值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static Func<IRedisClient, bool> RemoveEntryFromHashCommand<T>(string key, string dataKey)
        {
            return (r) => { return r.RemoveEntryFromHash(key, dataKey); };
        }

        /// <summary>
        /// 删除全部hash值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>       
        /// <returns></returns>
        public static Func<IRedisClient, bool> RemoveAllEntryFromHashCommand<T>(string key)
        {
            return (r) => { return r.Remove(key); };
        }

        /// <summary>
        /// 执行redis事务脚本
        /// </summary>
        /// <param name="redisClient"></param>
        /// <param name="trans"></param>
        /// <param name="funcs"></param>
        /// <returns></returns>
        public static bool ExcutRedisTranCommand(IRedisClient redisClient, IRedisTransaction trans, List<Func<IRedisClient, bool>> funcs)
        {
            try
            {
                using (redisClient)
                {
                    using (trans)
                    {
                        foreach (var item in funcs)
                        {
                            trans.QueueCommand(item);
                        }
                        trans.Commit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //记录日志   
                trans.Rollback();
                trans.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 获取redis对象
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetRedisClient()
        {
            IRedisClient redis = null;
            try
            {
                redis = prcm.GetClient();
                return redis;
            }
            catch (Exception ex)
            {
                //记录日志
                if (redis != null)
                {
                    redis.Dispose();
                }
                SwitchWriteRedis();
                return null;
            }
        }

        #endregion

        /// <summary> 
        /// 从hash表获取数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static T Hash_Get<T>(string key, string dataKey)
        {
            try
            {
                using (IRedisClient redis = prcm.GetReadOnlyClient())
                {
                    string value = redis.GetValueFromHash(key, dataKey);
                    return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
                }
            }
            catch (Exception ex)
            {
                SwitchReadRedis();
                return default(T);
            }
        }

        /// <summary> 
        /// 移除hash中的某值 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Remove(string key, string dataKey)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.RemoveEntryFromHash(key, dataKey);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
                return false;
            }
        }
        /// <summary> 
        /// 移除整个hash 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool Hash_Remove(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Remove(key);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
                return false;
            }
        }

        /// <summary> 
        /// 获取整个hash的数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static List<T> Hash_GetAll<T>(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetReadOnlyClient())
                {
                    var list = redis.GetHashValues(key);
                    if (list != null && list.Count > 0)
                    {
                        List<T> result = new List<T>();
                        foreach (var item in list)
                        {
                            var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                            result.Add(value);
                        }
                        return result;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchReadRedis();
                return null;
            }
        }

        /// <summary> 
        /// 获取Hash集合数量 
        /// </summary> 
        /// <param name="key">Hashid</param> 
        public static int Hash_GetCount(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetReadOnlyClient())
                {
                    return redis.GetHashCount(key);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchReadRedis();
                return 0;
            }
        }
        #endregion

        #region SortedSet进行排序

        /// <summary> 
        ///  添加数据到 SortedSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <param name="score"></param> 
        public static bool SortedSet_Add<T>(string key, T t, double score)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                    return redis.AddItemToSortedSet(key, value, score);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
                return false;
            }
        }

        /// <summary> 
        /// 移除数据从SortedSet 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <returns></returns> 
        public static bool SortedSet_Remove<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                    return redis.RemoveItemFromSortedSet(key, value);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
                return false;
            }
        }
        /// <summary> 
        /// 修剪SortedSet 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="size">保留的条数</param> 
        /// <returns></returns> 
        public static int SortedSet_Trim(string key, int size)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.RemoveRangeFromSortedSet(key, size, 9999999);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
                return 0;
            }
        }
        /// <summary> 
        /// 获取SortedSet的长度 
        /// </summary> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static int SortedSet_Count(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetReadOnlyClient())
                {
                    return redis.GetSortedSetCount(key);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchReadRedis();
                return 0;
            }
        }

        /// <summary> 
        /// 获取SortedSet的分页数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="pageIndex"></param> 
        /// <param name="pageSize"></param> 
        /// <returns></returns> 
        public static List<T> SortedSet_GetList<T>(string key, int pageIndex, int pageSize)
        {
            try
            {
                using (IRedisClient redis = prcm.GetReadOnlyClient())
                {
                    var list = redis.GetRangeFromSortedSet(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                    if (list != null && list.Count > 0)
                    {
                        List<T> result = new List<T>();
                        foreach (var item in list)
                        {
                            var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                            result.Add(data);
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchReadRedis();
            }
            return null;
        }


        /// <summary> 
        /// 获取SortedSet的全部数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="pageIndex"></param> 
        /// <param name="pageSize"></param> 
        /// <returns></returns> 
        public static List<T> SortedSet_GetListALL<T>(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetReadOnlyClient())
                {
                    var list = redis.GetRangeFromSortedSet(key, 0, 9999999);
                    if (list != null && list.Count > 0)
                    {
                        List<T> result = new List<T>();
                        foreach (var item in list)
                        {
                            var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                            result.Add(data);
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchReadRedis();
            }
            return null;
        }

        /// <summary> 
        /// 设置缓存过期 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static void SortedSet_SetExpire(string key, DateTime datetime)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    redis.ExpireEntryAt(key, datetime);
                }
            }
            catch (Exception ex)
            {
                //记录日志 
                SwitchWriteRedis();
            }
        }
        #endregion


        #region 更新指定key的过期时间

        /// <summary> 
        /// 设置缓存过期 
        /// </summary> 
        /// <param name="key"></param> 
        /// <param name="datetime"></param> 
        public static void SetExpireAt(string key, DateTime datetime)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    redis.ExpireEntryAt(key, datetime);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
            }
        }

        /// <summary>
        /// 设置滑动过期时间
        /// </summary>
        /// <param name="key">指定key</param>
        /// <param name="timeSpan">过期时间间隔</param>
        public static void SetExpireIn(string key, TimeSpan timeSpan)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    redis.ExpireEntryIn(key, timeSpan);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                SwitchWriteRedis();
            }
        }

        #endregion

        #region 获取指定key可以存活时间

        /// <summary>
        /// 获取指定key可以存活的时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TimeSpan GetTimeToLive(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.GetTimeToLive(key);
                }
            }
            catch (Exception ex)
            {
                return new TimeSpan();
            }
        }

        #endregion

        private static void SwitchWriteRedis()
        {
            prcm.SwitchWriteRedisOperator();
        }

        private static void SwitchReadRedis()
        {
            prcm.SwitchReadRedisClient();
        }

        /// <summary>
        /// 将实体对象转换为json串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertObjectToJson<T>(T obj)
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString<T>(obj);
        }

        /// <summary>
        /// 将json串转换为实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T ConvertJsonToObject<T>(string jsonStr)
        {
            return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(jsonStr);
        }
    }
}