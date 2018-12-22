using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.RedisClientManager
{
    public interface IRedisClientFactoryExt
    {
        RedisClientExt CreateRedisClient(string host, int port);
    }
}
