using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using YSWL.Log.ClientLog;

namespace YSWL.Log
{
    /// <summary>
    /// 日志和适配器对象管理器
    /// </summary>
    public class LogManager
    {
        private static readonly ConcurrentDictionary<string, ILogger> Loggers;
        private static readonly object LockObj = new object();

        static LogManager()
        {
            Loggers = new ConcurrentDictionary<string, ILogger>();
            Adapters = new Dictionary<string, ILoggerAdapter>();
            AddLoggerAdapter(new DefaultLoggerAdapter(), typeof(DefaultLoggerAdapter));//初始化日志模块  入库
            AddLoggerAdapter(new LoggerAdapterOfTxt(), typeof(LoggerAdapterOfTxt));//写txt日志
        }

        /// <summary>
        /// 获取 日志适配器字典 根据字典中的日子适配器集合进行写日志操作
        /// </summary>
        internal static Dictionary<string, ILoggerAdapter> Adapters { get; private set; }

        /// <summary>
        /// 按名称 添加日志适配器
        /// </summary>
        public static void AddLoggerAdapter(ILoggerAdapter adapter, string name)
        {
            lock (LockObj)
            {
                if (Adapters.Any(m => m.Key == name))
                {
                    return;
                }
                Adapters.Add(name, adapter);
            }
        }

        /// <summary>
        /// 按类型 添加日志适配器
        /// </summary>
        public static void AddLoggerAdapter(ILoggerAdapter adapter, Type type)
        {
            AddLoggerAdapter(adapter, type.Name);
        }

        /// <summary>
        /// 按名称 移除日志适配器
        /// </summary>
        public static void RemoveLoggerAdapter(string name)
        {
            lock (LockObj)
            {
                if (Adapters.Any(m => m.Key == name))
                {
                    Adapters.Remove(name);
                }
                return;
            }
        }

        /// <summary>
        /// 按名称 移除日志适配器
        /// </summary>
        public static void RemoveLoggerAdapter(Type type)
        {
            RemoveLoggerAdapter(type.Name);
        }

        /// <summary>
        /// 获取日志记录者实例
        /// </summary>
        public static ILogger GetLogger(string name)
        {
            //name.CheckNotNullOrEmpty("name");
            ILogger logger = null;
            if (Loggers.TryGetValue(name, out logger))
            {
                return logger;
            }
            logger = new Logger(name);
            Loggers[name] = logger;
            return logger;
        }

        /// <summary>
        /// 获取指定类型的日志记录实例
        /// </summary>
        public static ILogger GetLogger(Type type)
        {
            //type.CheckNotNull("type");
            return GetLogger(type.Name);
        }
    }
}
