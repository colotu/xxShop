using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log
{
    public abstract class LogBase : ILog
    {
        public ILogger _logger = new Logger("");
        /// <summary>
        /// 获取日志输出处理委托实例
        /// </summary>
        /// <param name="level">日志输出级别</param>
        /// <param name="message">日志消息</param>
        /// <param name="exception">日志异常</param>
        protected abstract void Write(LogLevel level, object message, Exception exception);

        #region 日志对应级别属性

        /// <summary>
        /// 获取 是否允许输出LogLevel.Trace级别的日志
        /// </summary>
        public bool IsTraceEnabled { get { return _logger.IsEnabledFor(LogLevel.Trace); } }

        /// <summary>
        /// 获取 是否允许输出LogLevel.Debug级别的日志
        /// </summary>
        public bool IsDebugEnabled { get { return _logger.IsEnabledFor(LogLevel.Debug); } }

        /// <summary>
        /// 获取 是否允许输出LogLevel.Info级别的日志
        /// </summary>
        public bool IsInfoEnabled { get { return _logger.IsEnabledFor(LogLevel.Info); } }

        /// <summary>
        /// 获取 是否允许输出LogLevel.Warn级别的日志
        /// </summary>
        public bool IsWarnEnabled { get { return _logger.IsEnabledFor(LogLevel.Warn); } }

        /// <summary>
        /// 获取 是否允许输出<see cref="LogLevel.Error"/>级别的日志
        /// </summary>
        public bool IsErrorEnabled { get { return _logger.IsEnabledFor(LogLevel.Error); } }

        /// <summary>
        /// 获取 是否允许输出<see cref="LogLevel.Fatal"/>级别的日志
        /// </summary>
        public bool IsFatalEnabled { get { return _logger.IsEnabledFor(LogLevel.Fatal); } }

        #endregion

        #region Implementation of ILog

        /// <summary>
        /// 写入<see cref="LogLevel.Trace"/>日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public virtual void Trace<T>(T message)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, message, null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Trace"/>格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public virtual void Trace(string format, params object[] args)
        {
            if (IsTraceEnabled)
            {
                Write(LogLevel.Trace, string.Format(format, args), null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Debug"/>日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public virtual void Debug<T>(T message)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, message, null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Debug"/>格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public virtual void Debug(string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                Write(LogLevel.Debug, string.Format(format, args), null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Info"/>日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public virtual void Info<T>(T message)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, message, null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Info"/>格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public virtual void Info(string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                Write(LogLevel.Info, string.Format(format, args), null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Warn"/>日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public virtual void Warn<T>(T message)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, message, null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Warn"/>格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public virtual void Warn(string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                Write(LogLevel.Warn, string.Format(format, args), null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Error"/>日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public virtual void Error<T>(T message)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, message, null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Error"/>格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public void Error(string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, string.Format(format, args), null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Error"/>日志消息，并记录异常
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常</param>
        public virtual void Error<T>(T message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, message, exception);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Error"/>格式化日志消息，并记录异常
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="exception">异常</param>
        /// <param name="args">格式化参数</param>
        public virtual void Error(string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
            {
                Write(LogLevel.Error, string.Format(format, args), exception);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Fatal"/>日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public virtual void Fatal<T>(T message)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, message, null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Fatal"/>格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public void Fatal(string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, string.Format(format, args), null);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Fatal"/>日志消息，并记录异常
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常</param>
        public virtual void Fatal<T>(T message, Exception exception)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, message, exception);
            }
        }

        /// <summary>
        /// 写入<see cref="LogLevel.Fatal"/>格式化日志消息，并记录异常
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="exception">异常</param>
        /// <param name="args">格式化参数</param>
        public virtual void Fatal(string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
            {
                Write(LogLevel.Fatal, string.Format(format, args), exception);
            }
        }

        #endregion

        public bool IsEnabledFor(LogLevel level)
        {
            throw new NotImplementedException();
        }
    }
}
