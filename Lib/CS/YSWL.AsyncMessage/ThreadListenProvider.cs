using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.AsyncMessage
{
    /// <summary>
    /// 线程监听对象的驱动（创建线程监听对象）
    /// </summary>
    public class ThreadListenProvider : IDisposable
    {
        /// <summary>
        /// 缓存监听接口对象
        /// </summary>
        private static Dictionary<string, IThreadListen> threadListens;

        /// <summary>
        /// 全局锁
        /// </summary>
        private static readonly object obj = new object();

        private static bool isInit = false;

        /// <summary>
        /// 获取当前驱动实例
        /// </summary>
        public static ThreadListenProvider Current
        {
            get { return new ThreadListenProvider(); }
        }

        /// <summary>
        /// 单例模式启动
        /// </summary>
        public ThreadListenProvider()
        {
            if (threadListens == null)
            {
                lock (this)
                {
                    if (threadListens == null)
                    {
                        threadListens = new Dictionary<string, IThreadListen>();
                    }
                }
            }
        }

        /// <summary>
        /// 获取监听接口实例
        /// </summary>
        /// <returns>返回实例对象</returns>
        public IThreadListen CreateThreadListen()
        {
            lock (obj)
            {
                if (threadListens.Any())
                {
                    return threadListens.FirstOrDefault().Value;
                }
                if (isInit)
                {
                    return null;
                }
                try
                {
                    isInit = true;
                    Type threadListenType = typeof(IThreadListen);
                    var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(threadListenType))).ToArray();
                    if (types != null && types.Count() > 0)
                    {                        
                        foreach (var item in types)
                        {
                            IThreadListen threadListen = (IThreadListen)Activator.CreateInstance(item);
                            threadListens.Add(item.FullName, threadListen);
                            return threadListen;
                        }
                    }
                }
                catch
                {
                    isInit = false;
                }
                return null;
            }

        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            threadListens = null;
        }
    }
}
