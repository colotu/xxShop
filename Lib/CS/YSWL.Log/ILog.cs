using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log
{
    public interface ILog : ILogger
    {
        #region 属性

        /// <summary>
        /// 获取 是否允许LogLevel.Trace级别的日志
        /// </summary>
        bool IsTraceEnabled { get; }

        /// <summary>
        /// 获取 是否允许LogLevel.Debug级别的日志
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// 获取 是否允许LogLevel.Info级别的日志
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// 获取 是否允许LogLevel.Warn级别的日志
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// 获取 是否允许LogLevel.Error"/>级别的日志
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// 获取 是否允许LogLevel.Fatal级别的日志
        /// </summary>
        bool IsFatalEnabled { get; }

        #endregion
    }
}
