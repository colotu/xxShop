using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.RedisClient
{
    public class RedisClientExt : ServiceStack.Redis.RedisClient, IDisposable
    {
        /// <summary>
        /// 是否活跃
        /// </summary>
        internal bool Active { get; set; }

        /// <summary>
        /// 是否销毁
        /// </summary>
        internal bool IsDisposed { get; set; }

        /// <summary>
        /// redisClient管理池
        /// </summary>
        internal PoolRedisClientManagerExt ClientManager { get; set; }

        /// <summary>
        /// 进行db数据库切换，db的数量可以在redis配置文件中进行设置
        /// </summary>
        public int DB
        {
            get
            {
                return base.Db;
            }
            set
            {
                base.Db = value;
            }
        }

        public RedisClientExt() : base()
        { }

        public RedisClientExt(string host) : base(host)
        { }

        public RedisClientExt(string host, int port) : base(host, port)
        { }

        public new void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (this.ClientManager != null)
            {
                this.ClientManager.DisposeClient(this);
            }
            else if (disposing)
            {
                this.DisposeConnection();
            }
        }

        internal void DisposeConnection()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException("Redis client already disposed");
            }
            this.IsDisposed = true;
            if (this.socket != null)
            {
                try
                {
                    this.Quit();
                }
                catch (Exception exception)
                {
                    //记录日志
                }
                finally
                {
                    this.SafeConnectionClose();
                }
            }
        }


        private void SafeConnectionClose()
        {
            try
            {
                if (this.Bstream != null)
                {
                    this.Bstream.Close();
                }
            }
            catch
            {
            }
            try
            {
                if (this.socket != null)
                {
                    this.socket.Close();
                }
            }
            catch
            {
            }
            this.Bstream = null;
            this.socket = null;
        }
    }
}
