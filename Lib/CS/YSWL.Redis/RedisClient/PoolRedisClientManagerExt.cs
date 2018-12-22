using ServiceStack.CacheAccess;
using ServiceStack.Common.Web;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace YSWL.RedisClient
{
    /// <summary>
    /// 重构redisClient管理池
    /// </summary>
    public class PoolRedisClientManagerExt : IDisposable
    {
        public const int DefaultCacheDb = 9;
        protected const int PoolSizeMultiplier = 10;
        protected int RedisClientCounter;
        protected bool IsSlavOf = false;

        /// <summary>
        /// 是否正在切换主从
        /// </summary>
        protected bool IsSwitchWrite = false;

        /// <summary>
        /// 是否切换读
        /// </summary>
        protected bool IsSwitchRead = false;
     
        #region new

        /// <summary>
        /// 读缓存池
        /// </summary>
        private Dictionary<string, List<RedisClientExt>> readClients;
        protected int ReadPoolIndex;

        /// <summary>
        /// 写缓存池
        /// </summary>
        //private RedisClientExt[] writeClients;
        private Dictionary<string, List<RedisClientExt>> writeReadClients;
        protected int WritePoolIndex;

        /// <summary>
        /// 异常缓存池
        /// </summary>
        private List<RedisClientExt> exceptionClients;
        protected int ExceptionIndex;

        #endregion

        public PoolRedisClientManagerExt() : this(new string[] { "localhost" })
        {
        }

        public PoolRedisClientManagerExt(params string[] readWriteHosts) : this(readWriteHosts, readWriteHosts)
        {
        }

        public PoolRedisClientManagerExt(IEnumerable<string> readWriteHosts, IEnumerable<string> readOnlyHosts) : this(readWriteHosts, readOnlyHosts, (RedisClientManagerConfig)null)
        {
        }

        public PoolRedisClientManagerExt(IEnumerable<string> readWriteHosts, IEnumerable<string> readOnlyHosts, RedisClientManagerConfig config) : this(readWriteHosts, readOnlyHosts, config, 0)
        {
        }

        public PoolRedisClientManagerExt(IEnumerable<string> readWriteHosts, IEnumerable<string> readOnlyHosts, int initalDb) : this(readWriteHosts, readOnlyHosts, null, initalDb)
        {
        }

        public PoolRedisClientManagerExt(IEnumerable<string> readWriteHosts, IEnumerable<string> readOnlyHosts, RedisClientManagerConfig config, int initalDb)
        {
            this.writeReadClients = new Dictionary<string, List<RedisClientExt>>();
            this.readClients = new Dictionary<string, List<RedisClientExt>>();
            this.Db = (config != null) ? config.DefaultDb.GetValueOrDefault(initalDb) : initalDb;
            this.ReadWriteHosts = readWriteHosts.ToIpEndPoints();
            this.ReadOnlyHosts = readOnlyHosts.ToIpEndPoints();
            this.RedisClientFactory = RedisClient.RedisClientFactoryExt.Instance;
            if (config == null)
            {
                config = new RedisClientManagerConfig
                {
                    MaxWritePoolSize = this.ReadWriteHosts.Count * 10,
                    MaxReadPoolSize = this.ReadOnlyHosts.Count * 10,
                    AutoStart = true
                };
            }
            this.Config = config;
            if (this.Config.AutoStart)
            {
                this.OnStart();
            }
        }


        #region 获取redis客户端


        #region 获取写redis客户端

        /// <summary>
        /// 缓存获取redis客户端
        /// </summary>
        /// <returns></returns>
        public ICacheClient GetCacheClient()
        {
            return this.ConfigureRedisClient(this.GetClient());
        }

        /// <summary>
        /// 获取redis客户端
        /// </summary>
        /// <returns></returns>
        public IRedisClient GetClient()
        {
            lock (this.writeReadClients)
            {
                RedisClientExt client;
                this.AssertValidReadWritePool();
                while ((client = this.GetInActiveWriteClient()) == null)
                {
                    Monitor.Wait(this.writeReadClients);
                }
                this.WritePoolIndex++;
                client.Active = true;
                if (client.Db != this.Db)
                {
                    client.Db = this.Db;
                }
                return client;
            }
        }

        /// <summary>
        /// 获取可使用的redis写客户端
        /// </summary>
        /// <returns></returns>
        private RedisClientExt GetInActiveWriteClient()
        {
            RedisClientExt client;
            foreach (var item in this.writeReadClients)
            {
                if (item.Value == null || !item.Value.Any())
                {
                    continue;
                }
                var ips = item.Key.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                var clientTemps = item.Value.Where(c => !c.Active);
                int count = item.Value.Count();
                if (clientTemps == null || !clientTemps.Any())
                {
                    if (count < this.Config.MaxWritePoolSize)
                    {
                        client = this.RedisClientFactory.CreateRedisClient(ips[0], int.Parse(ips[1]));
                        client.Id = this.RedisClientCounter++;
                        client.ClientManager = this;
                        item.Value.Add(client);
                        return client;
                    }
                    return null;
                }
                client = clientTemps.FirstOrDefault();
                if (client.HadExceptions)
                {
                    item.Value.Remove(client);
                    client.DisposeConnection();
                    RedisClientExt client2 = this.RedisClientFactory.CreateRedisClient(ips[0], int.Parse(ips[1]));
                    client2.Id = this.RedisClientCounter++;
                    client2.ClientManager = this;
                    item.Value.Add(client2);
                    return client2;
                }
                return client;
            }
            return null;
        }

        /// <summary>
        /// 切换redis主从服务
        /// </summary>
        public void SwitchWriteRedisOperator()
        {
            if (IsSwitchWrite)
            {
                return;
            }
            lock (this.writeReadClients)
            {
                if (IsSwitchWrite)
                {
                    return;
                }
                IsSwitchWrite = true;
                RedisClientExt client;
                List<string> deleteObjs = new List<string>();
                string obj = "";
                try
                {
                    foreach (var item in this.writeReadClients)
                    {
                        var ips = item.Key.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        client = this.RedisClientFactory.CreateRedisClient(ips[0], int.Parse(ips[1]));
                        client.Id = this.RedisClientCounter++;
                        client.ClientManager = this;
                        obj = item.Key;
                        if (client.Ping())
                        {
                            continue;
                        }
                        deleteObjs.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    //记录日志
                    deleteObjs.Add(obj);
                }
                if (deleteObjs.Count <= 0)
                {
                    return;
                }
                foreach (var item in deleteObjs)
                {
                    this.writeReadClients.Remove(item);
                }
                if (this.writeReadClients == null || !this.writeReadClients.Any())
                {
                    foreach (var point in this.ReadOnlyHosts)
                    {
                        client = this.RedisClientFactory.CreateRedisClient(point.Host, point.Port);
                        client.Id = this.RedisClientCounter++;
                        client.ClientManager = this;
                        bool isAdd = false;
                        try
                        {
                            if (client.Ping())
                            {
                                client.SlaveOfNoOne();
                                IsSwitchWrite = false;
                                isAdd = true;
                            }
                        }
                        catch
                        {
                            IsSwitchWrite = false;
                        }
                        if (isAdd)
                        {
                            List<RedisClientExt> clients = new List<RedisClientExt>();
                            clients.Add(client);
                            for (int i = 1; i < this.Config.MaxWritePoolSize; i++)
                            {
                                client = this.RedisClientFactory.CreateRedisClient(point.Host, point.Port);
                                client.Id = this.RedisClientCounter++;
                                client.ClientManager = this;
                                clients.Add(client);
                            }
                            if (!this.writeReadClients.Any(k => k.Key == (point.Host + ":" + point.Port)))
                            {
                                this.writeReadClients.Add(point.Host + ":" + point.Port, clients);
                                Monitor.PulseAll(this.writeReadClients);
                                return;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 获取读redis客户端       

        public ICacheClient GetReadOnlyCacheClient()
        {
            return this.ConfigureRedisClient(this.GetReadOnlyClient());
        }

        public virtual IRedisClient GetReadOnlyClient()
        {
            if (this.readClients == null || !this.readClients.Any())
            {
                return GetClient();//没有主从，直接从主redis读取信息
            }
            lock (this.readClients)
            {
                RedisClientExt client;
                this.AssertValidReadOnlyPool();
                while ((client = this.GetInActiveReadClient()) == null)
                {
                    Monitor.Wait(this.readClients);
                }
                this.ReadPoolIndex++;
                client.Active = true;//自己维护解决
                if (client.Db != this.Db)
                {
                    client.Db = this.Db;
                }
                return client;
            }
        }

        /// <summary>
        /// 获取可使用的redis读客户端
        /// </summary>
        /// <returns></returns>
        private RedisClientExt GetInActiveReadClient()
        {
            RedisClientExt client;
            foreach (var item in this.readClients)
            {
                if (item.Value == null || !item.Value.Any())
                {
                    continue;
                }
                var ips = item.Key.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                var clientTemps = item.Value.Where(c => !c.Active);
                int count = item.Value.Count();
                if (clientTemps == null || !clientTemps.Any())
                {
                    if (count < this.Config.MaxWritePoolSize)
                    {
                        client = this.RedisClientFactory.CreateRedisClient(ips[0], int.Parse(ips[1]));
                        client.Id = this.RedisClientCounter++;
                        client.ClientManager = this;
                        item.Value.Add(client);
                        return client;
                    }
                    return null;
                }
                client = clientTemps.FirstOrDefault();
                if (client.HadExceptions)
                {
                    item.Value.Remove(client);
                    client.DisposeConnection();
                    RedisClientExt client2 = this.RedisClientFactory.CreateRedisClient(ips[0], int.Parse(ips[1]));
                    client2.Id = this.RedisClientCounter++;
                    client2.ClientManager = this;
                    item.Value.Add(client2);
                    return client2;
                }
                return client;
            }
            return null;
        }

        /// <summary>
        /// 切换从到主读取数据
        /// </summary>
        public void SwitchReadRedisClient()
        {
            if (IsSwitchRead)
            {
                return;
            }
            lock (this.readClients)
            {
                if (IsSwitchRead)
                {
                    return;
                }
                IsSwitchRead = true;
                RedisClientExt client;
                List<string> deleteObjs = new List<string>();
                string obj = "";
                try
                {
                    foreach (var item in this.readClients)
                    {
                        var ips = item.Key.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        client = this.RedisClientFactory.CreateRedisClient(ips[0], int.Parse(ips[1]));
                        client.Id = this.RedisClientCounter++;
                        client.ClientManager = this;
                        obj = item.Key;
                        if (client.Ping())
                        {
                            continue;
                        }
                        deleteObjs.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    //记录日志
                    deleteObjs.Add(obj);
                }
                foreach (var item in deleteObjs)
                {
                    this.readClients.Remove(item);
                }
                IsSwitchRead = false;
                Monitor.PulseAll(this.readClients);
            }
        }

        #endregion        

        private ICacheClient ConfigureRedisClient(IRedisClient client)
        {
            if (this.Db == 0)
            {
                client.Db = 9;
            }
            return client;
        }

        #endregion



        #region 释放池中对象

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            foreach (var item in this.writeReadClients)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    this.Dispose(item.Value[i]);
                }
            }
            foreach (var item in this.readClients)
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    this.Dispose(item.Value[i]);
                }
            }
        }

        protected void Dispose(RedisClientExt redisClient)
        {
            if (redisClient != null)
            {
                try
                {
                    redisClient.DisposeConnection();
                }
                catch (Exception exception)
                {
                    //记录日志
                }
            }
        }

        public void DisposeClient(RedisClientExt client)
        {
            lock (this.readClients)
            {
                if (this.readClients.Count > 0)
                {
                    foreach (var item in this.readClients)
                    {
                        for (int i = 0; i < item.Value.Count; i++)
                        {
                            RedisClientExt client2 = item.Value[i];
                            if (client == client2)
                            {
                                client.Active = false;
                                Monitor.PulseAll(this.readClients);
                                return;
                            }
                        }
                    }
                }               
            }
            lock (this.writeReadClients)
            {
                if (this.writeReadClients.Count > 0)
                {
                    foreach (var item in this.writeReadClients)
                    {
                        for (int i = 0; i < item.Value.Count; i++)
                        {
                            RedisClientExt client2 = item.Value[i];
                            if (client == client2)
                            {
                                client.Active = false;
                                Monitor.PulseAll(this.writeReadClients);
                                return;
                            }
                        }
                    }
                }              
            }
            if (!client.IsDisposed)
            {
                client.Active = false;
                client = null;
            }
        }

        #endregion

        /// <summary>
        /// 判断读缓存池是否存在对象，无直接抛出异常
        /// </summary>
        private void AssertValidReadOnlyPool()
        {
            if (this.readClients.Count < 1)
            {
                this.Start();
                throw new InvalidOperationException("Need a minimum read pool size of 1, then call Start()");
            }
        }

        /// <summary>
        /// 判断写缓存池是否存在对象，无直接抛出异常
        /// </summary>
        private void AssertValidReadWritePool()
        {
            if (this.writeReadClients.Count < 1)
            {
                this.Start();
                throw new InvalidOperationException("Need a minimum read-write pool size of 1, then call Start()");
            }
        }

        /// <summary>
        /// 进行redisclient的初始化
        /// </summary>
        protected virtual void OnStart()
        {
            this.Start();
        }

        public void Start()
        {
            if ((this.writeReadClients.Count > 0) || (this.readClients.Count > 0))
            {
                throw new InvalidOperationException("Pool has already been started");
            }
            List<RedisClientExt> clientTemps = null;
            foreach (var point in this.ReadWriteHosts)
            {
                clientTemps = new List<RedisClientExt>();
                for (int i = 0; i < this.Config.MaxWritePoolSize; i++)
                {
                    RedisClientExt client = this.RedisClientFactory.CreateRedisClient(point.Host, point.Port);
                    client.ClientManager = this;
                    clientTemps.Add(client);
                }
                if (!this.writeReadClients.Any(k => k.Key == (point.Host + ":" + point.Port)))
                {
                    this.writeReadClients.Add(point.Host + ":" + point.Port, clientTemps);
                }
            }
            this.WritePoolIndex = 0;

            foreach (var point in this.ReadOnlyHosts)
            {
                clientTemps = new List<RedisClientExt>();
                for (int i = 0; i < this.Config.MaxReadPoolSize; i++)
                {
                    RedisClientExt client = this.RedisClientFactory.CreateRedisClient(point.Host, point.Port);
                    client.ClientManager = this;
                    clientTemps.Add(client);
                }
                if (!this.readClients.Any(k => k.Key == (point.Host + ":" + point.Port)))
                {
                    this.readClients.Add(point.Host + ":" + point.Port, clientTemps);
                }
            }
            this.ReadPoolIndex = 0;

            //暂时未实现
            this.exceptionClients = new List<RedisClientExt>();
            this.ExceptionIndex = 0;
        }

        protected RedisClientManagerConfig Config { get; set; }

        public int Db { get; private set; }      

        #region new

        private List<EndPoint> ReadOnlyHosts { get; set; }

        private List<EndPoint> ReadWriteHosts { get; set; }

        #endregion

        public IRedisClientFactoryExt RedisClientFactory { get; set; }
    }
}
