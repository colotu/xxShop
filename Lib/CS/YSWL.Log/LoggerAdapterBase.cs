using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log
{
    /// <summary>
    /// 适配器基类（所有适配器必须实现该类），负责创建具体的日志输出对象
    /// </summary>
    public abstract class LoggerAdapterBase : ILoggerAdapter
    {
        private readonly Dictionary<string, ILog> _cacheLoggers;

        /// <summary>
        /// 初始化一个_cacheLoggers类型的新实例
        /// </summary>
        protected LoggerAdapterBase()
        {
            _cacheLoggers = new Dictionary<string, ILog>();
        }

        #region Implementation of ILoggerFactoryAdapter

        /// <summary>
        /// 由指定类型获取日志实例
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        public ILog GetLogger(Type type)
        {
            //type.CheckNotNull("type");
            return GetLoggerInternal(type.Name);
        }

        /// <summary>
        /// 由指定名称获取日志实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns></returns>
        public ILog GetLogger(string name)
        {
            //name.CheckNotNullOrEmpty("name");
            return GetLoggerInternal(name);
        }

        #endregion

        /// <summary>
        /// 创建指定名称的缓存实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns></returns>
        protected abstract ILog CreateLogger(string name);

        /// <summary>
        /// 清除缓存中的日志实例
        /// </summary>
        protected virtual void ClearLoggerCache()
        {
            _cacheLoggers.Clear();
        }

        /// <summary>
        /// 获得具体写入日志信息实例方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private ILog GetLoggerInternal(string name)
        {
            ILog log;
            if (_cacheLoggers.TryGetValue(name, out log))
            {
                return log;
            }
            log = CreateLogger(name);
            if (log == null)
            {
                //throw new NotSupportedException(Resources.Logging_CreateLogInstanceReturnNull.FormatWith(name, GetType().FullName));                
            }
            _cacheLoggers.Add(name, log);
            return log;
        }
    }
}
