using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log
{
    /// <summary>
    /// 日志类型枚举
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 表示跟踪的日志级别
        /// </summary>
        Trace = 1,

        /// <summary>
        /// 表示调试的日志级别
        /// </summary>
        Debug = 2,

        /// <summary>
        /// 表示正常的日志级别
        /// </summary>
        Info = 3,

        /// <summary>
        /// 表示警告的日志级别
        /// </summary>
        Warn = 4,

        /// <summary>
        /// 表示错误的日志级别
        /// </summary>
        Error = 5,

        /// <summary>
        /// 表示严重错误的日志级别
        /// </summary>
        Fatal = 6
    }
}
