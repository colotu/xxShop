using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.RedisClient
{
    /// <summary>
    /// redisClient创建工厂
    /// </summary>
    public class RedisClientFactoryExt : IRedisClientFactoryExt
    {
        public static RedisClientFactoryExt Instance = new RedisClientFactoryExt();

        public RedisClientExt CreateRedisClient(string host, int port)
        {
            return new RedisClientExt(host, port);
        }
    }
}
