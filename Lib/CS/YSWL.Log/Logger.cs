using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace YSWL.Log
{
    public class Logger : ILogger
    {
        internal Logger(string name)
        {
            _name = name;
            string logLevel = ConfigurationManager.AppSettings.Get("EntryLogLevel");
            EntryLevel = string.IsNullOrEmpty(logLevel) ? LogLevel.All : (LogLevel)Enum.Parse(typeof(LogLevel), logLevel);
        }

        private string _name;

        /// <summary>
        /// 获取 日志记录者名称
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    _name = ConfigurationManager.AppSettings.Get("adapterName");
                    return _name;
                }
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// 获取或设置 日志级别的入口控制，级别决定是否执行相应级别的日志记录功能
        /// </summary>
        public LogLevel EntryLevel { get; set; }

        #region Implementation of ILogger

        /// <summary>
        /// 写入LogLevel.Trace日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Trace<T>(T message)
        {
            if (!IsEnabledFor(LogLevel.Trace))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Trace(message);
        }

        /// <summary>
        /// 写入LogLevel.Trace格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public void Trace(string format, params object[] args)
        {
            if (!IsEnabledFor(LogLevel.Trace))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Trace(format, args);
        }

        /// <summary>
        /// 写入LogLevel.Debug日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Debug<T>(T message)
        {
            if (!IsEnabledFor(LogLevel.Debug))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Debug(message);
        }

        /// <summary>
        /// 写入LogLevel.Debug格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public void Debug(string format, params object[] args)
        {
            if (!IsEnabledFor(LogLevel.Debug))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Debug(format, args);
        }

        /// <summary>
        /// 写入LogLevel.Info日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Info<T>(T message)
        {
            if (!IsEnabledFor(LogLevel.Info))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Info(message);
        }

        /// <summary>
        /// 写入LogLevel.Info格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public void Info(string format, params object[] args)
        {
            if (!IsEnabledFor(LogLevel.Info))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Info(format, args);
        }

        /// <summary>
        /// 写入LogLevel.Warn日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Warn<T>(T message)
        {
            if (!IsEnabledFor(LogLevel.Warn))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Warn(message);
        }

        /// <summary>
        /// 写入LogLevel.Warn格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public void Warn(string format, params object[] args)
        {
            if (!IsEnabledFor(LogLevel.Warn))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Warn(format, args);
        }

        /// <summary>
        /// 写入LogLevel.Error日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Error<T>(T message)
        {
            if (!IsEnabledFor(LogLevel.Error))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Error(message);
        }

        /// <summary>
        /// 写入LogLevel.Error格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public void Error(string format, params object[] args)
        {
            if (!IsEnabledFor(LogLevel.Error))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Error(format, args);
        }

        /// <summary>
        /// 写入LogLevel.Error日志消息，并记录异常
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常</param>
        public void Error<T>(T message, Exception exception)
        {
            if (!IsEnabledFor(LogLevel.Error))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Error(message, exception);
        }

        /// <summary>
        /// 写入LogLevel.Error格式化日志消息，并记录异常
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="exception">异常</param>
        /// <param name="args">格式化参数</param>
        public void Error(string format, Exception exception, params object[] args)
        {
            if (!IsEnabledFor(LogLevel.Error))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Error(format, exception, args);
        }

        /// <summary>
        /// 写入LogLevel.Fatal日志消息
        /// </summary>
        /// <param name="message">日志消息</param>
        public void Fatal<T>(T message)
        {
            if (!IsEnabledFor(LogLevel.Fatal))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Fatal(message);
        }

        /// <summary>
        /// 写入LogLevel.Fatal格式化日志消息
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="args">格式化参数</param>
        public void Fatal(string format, params object[] args)
        {
            if (!IsEnabledFor(LogLevel.Fatal))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Fatal(format, args);
        }

        /// <summary>
        /// 写入LogLevel.Fatal日志消息，并记录异常
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="exception">异常</param>
        public void Fatal<T>(T message, Exception exception)
        {
            if (!IsEnabledFor(LogLevel.Fatal))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Fatal(message, exception);
        }

        /// <summary>
        /// 写入LogLevel.Fatal格式化日志消息，并记录异常
        /// </summary>
        /// <param name="format">日志消息格式</param>
        /// <param name="exception">异常</param>
        /// <param name="args">格式化参数</param>
        public void Fatal(string format, Exception exception, params object[] args)
        {
            if (!IsEnabledFor(LogLevel.Fatal))
            {
                return;
            }
            LogManager.Adapters[Name].GetLogger(Name).Fatal(format, exception, args);
        }

        #endregion

        public bool IsEnabledFor(LogLevel level)
        {
            return level >= EntryLevel;
        }


        bool ILogger.IsEnabledFor(LogLevel level)
        {
            return IsEnabledFor(level);
        }
    }
}
