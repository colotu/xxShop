using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogger
    {
        #region 方法

        /// <summary>
        /// 写入LogLevel.Trace日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Trace<T>(T message);

        /// <summary>
        /// 写入LogLevel.Trace格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Trace(string format, params object[] args);

        /// <summary>
        /// 写入LogLevel.Debug日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Debug<T>(T message);

        /// <summary>
        /// 写入LogLevel.Debug格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Debug(string format, params object[] args);

        /// <summary>
        /// 写入LogLevel.Info日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Info<T>(T message);

        /// <summary>
        /// 写入LogLevel.Info格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Info(string format, params object[] args);

        /// <summary>
        /// 写入LogLevel.Warn日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Warn<T>(T message);

        /// <summary>
        /// 写入LogLevel.Warn格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Warn(string format, params object[] args);

        /// <summary>
        /// 写入LogLevel.Error日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        void Error<T>(T message);

        /// <summary>
        /// 写入LogLevel.Error格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Error(string format, params object[] args);

        /// <summary>
        /// 写入LogLevel.Error日志消息，并记录异常
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常</param>
        void Error<T>(T message, Exception exception);

        /// <summary>
        /// 写入LogLevel.Error格式化日志消息，并记录异常
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="exception">异常</param>
        /// <param name="args">格式化参数</param>
        void Error(string format, Exception exception, params object[] args);

        /// <summary>
        /// 写入LogLevel.Fatal日志消息
        /// </summary>
        /// <param name="message">日志消息</paramk>
        void Fatal<T>(T message);

        /// <summary>
        /// 写入LogLevel.Fatal格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        void Fatal(string format, params object[] args);

        /// <summary>
        /// 写入LogLevel.Fatal日志消息，并记录异常
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常</param>
        void Fatal<T>(T message, Exception exception);

        /// <summary>
        /// 写入LogLevel.Fatal格式化日志消息，并记录异常
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="exception">异常</param>
        /// <param name="args">格式化参数</param>
        void Fatal(string format, Exception exception, params object[] args);

        #endregion

        bool IsEnabledFor(LogLevel level);
    }
}
