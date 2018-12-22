using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Collections.Concurrent;

namespace YSWL.Log
{
    /// <summary>
    /// 日志入口辅助类（所有的日志记录都通过该类来记录日志）
    /// </summary>
    public class LogHelper
    {
        #region 内存日志队列属性

        /// <summary>
        /// 日志存放字典队列（后期可考虑使用redis改造成分布式日志记录队列，扩展该成分布式日志记录系统）
        /// 日志缓存队列
        /// </summary>
        public static ConcurrentDictionary<string, Queue<object>> cacheLogQueue = new ConcurrentDictionary<string, Queue<object>>();

        /// <summary>
        /// 待处理日志队列
        /// </summary>
        public static ConcurrentDictionary<string, Queue<object>> waitHandleQueue = new ConcurrentDictionary<string, Queue<object>>();

        /// <summary>
        /// 处理异常日志队列
        /// </summary>
        public static ConcurrentDictionary<string, Queue<object>> fileHandleQueue = new ConcurrentDictionary<string, Queue<object>>();

        #endregion


        public static readonly object obj = new object();

        /// <summary>
        /// 启动日志线程（集合的线程安全占时未解决）
        /// </summary>
        static LogHelper()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {                       
                        foreach (var item in cacheLogQueue)
                        {
                            if (!item.Value.Any())
                            {
                                continue;
                            }
                            while (true)
                            {
                                if (item.Value.Any())
                                {
                                    object writerObj = null;
                                    lock (obj)
                                    {
                                        writerObj = item.Value.Dequeue();
                                    }
                                    var logType = writerObj.GetType().GetProperty("LogType").GetValue(writerObj, null);//获取日志类型
                                    string clientKey = writerObj.GetType().GetProperty("TargetAdapterAndClientObj").GetValue(writerObj, null) + "";
                                    //clientKey = string.IsNullOrEmpty(clientKey) ? ConfigurationManager.AppSettings.Get("clientLoggerKey") : clientKey;
                                    ILogger logger = LogManager.GetLogger(clientKey);//获取客户端logger方法日志对象
                                    switch (logType + "")
                                    {
                                        case "Trace":
                                            logger.Trace(writerObj);
                                            break;
                                        case "Debug":
                                            logger.Debug(writerObj);
                                            break;
                                        case "Info":
                                            logger.Info(writerObj);
                                            break;
                                        case "Warn":
                                            logger.Warn(writerObj);
                                            break;
                                        case "Error":
                                            logger.Error(writerObj);
                                            break;
                                        case "Fatal":
                                            logger.Fatal(writerObj);
                                            break;
                                    }
                                    logger = null;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                        Thread.Sleep(500);
                    }
                    catch
                    {
                        //待处理
                    }
                }

            });
            thread.Start();
            thread.IsBackground = true;

        }

        #region 日志入队列


        /// <summary>
        /// 写入LogLevel.Trace日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Trace<T>(T message, Type targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Trace);
            Enqueue<T>(message, targetObj.FullName);
        }

        /// <summary>
        /// 写入LogLevel.Trace日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Trace<T>(T message, string targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Trace);
            Enqueue<T>(message, targetObj);
        }

        /// <summary>
        /// 写入LogLevel.Debug日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Debug<T>(T message, Type targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Debug);
            Enqueue<T>(message, targetObj.FullName);
        }

        /// <summary>
        /// 写入LogLevel.Debug日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Debug<T>(T message, string targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Debug);
            Enqueue<T>(message, targetObj);
        }

        /// <summary>
        /// 写入LogLevel.Info日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Info<T>(T message, Type targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Info);
            Enqueue<T>(message, targetObj.FullName);
        }

        /// <summary>
        /// 写入LogLevel.Info日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Info<T>(T message, string targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Info);
            Enqueue<T>(message, targetObj);
        }
        /// <summary>
        /// 添加默认的信息日志方式
        /// </summary>
        /// <param name="operaInfo"></param>
        /// <param name="excepInfo"></param>
        /// <param name="usernName"></param>
        /// <param name="userId"></param>
        public static void AddInfoLog(string operaInfo, string excepInfo,string usernName="", string userId="")
        {
            YSWL.Log.LogHelper.Info<OperationLog>(OperationLogHelper.GetOperationLog(usernName, userId, operaInfo, DateTime.Now, excepInfo), typeof(OperationLog));
        }

        /// <summary>
        /// 写入LogLevel.Warn日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Warn<T>(T message, Type targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Warn);
            Enqueue<T>(message, targetObj.FullName);
        }

        /// <summary>
        /// 写入LogLevel.Warn日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Warn<T>(T message, string targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Warn);
            Enqueue<T>(message, targetObj);
        }

        /// <summary>
        /// 添加默认的警告日志方式
        /// </summary>
        /// <param name="operaInfo"></param>
        /// <param name="excepInfo"></param>
        /// <param name="usernName"></param>
        /// <param name="userId"></param>
        public static void AddWarnLog(string operaInfo, string excepInfo, string usernName = "", string userId = "")
        {
            YSWL.Log.LogHelper.Warn<OperationLog>(OperationLogHelper.GetOperationLog(usernName, userId, operaInfo, DateTime.Now, excepInfo), typeof(OperationLog));
        }

        /// <summary>
        /// 写入LogLevel.Error日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Error<T>(T message, Type targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Error);
            Enqueue<T>(message, targetObj.FullName);
        }

        /// <summary>
        /// 写入LogLevel.Error日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Error<T>(T message, string targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Error);
            Enqueue<T>(message, targetObj);
        }


        /// <summary>
        /// 添加默认的错误日志方式
        /// </summary>
        /// <param name="operaInfo"></param>
        /// <param name="excepInfo"></param>
        /// <param name="usernName"></param>
        /// <param name="userId"></param>
        public static void AddErrorLog(string operaInfo, string excepInfo, string usernName = "", string userId = "")
        {
            YSWL.Log.LogHelper.Error<OperationLog>(OperationLogHelper.GetOperationLog(usernName, userId, operaInfo, DateTime.Now, excepInfo), typeof(OperationLog));
        }

        /// <summary>
        /// 写入LogLevel.Fatal日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Fatal<T>(T message, Type targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Fatal);
            Enqueue<T>(message, targetObj.FullName);
        }

        /// <summary>
        /// 写入LogLevel.Fatal日志消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">日志对象</param>
        /// <param name="targetObj"></param>
        public static void Fatal<T>(T message, string targetObj)
        {
            message = BackResetLogTypeObj<T>(message, LogType.Fatal);
            Enqueue<T>(message, targetObj);
        }


        /// <summary>
        /// 添加默认的严重错误日志方式
        /// </summary>
        /// <param name="operaInfo"></param>
        /// <param name="excepInfo"></param>
        /// <param name="usernName"></param>
        /// <param name="userId"></param>
        public static void AddFatalLog(string operaInfo, string excepInfo, string usernName = "", string userId = "")
        {
            YSWL.Log.LogHelper.Fatal<OperationLog>(OperationLogHelper.GetOperationLog(usernName, userId, operaInfo, DateTime.Now, excepInfo), typeof(OperationLog));
        }
        #endregion

        private static T BackResetLogTypeObj<T>(T t, LogType logType)
        {
            t.GetType().GetProperty("LogType").SetValue(t, logType, null);
            return t;
        }


        public static void AddTextLog(string errmsg, string errinfo)
        {
            YSWL.Log.LogHelper.Error<OperationLog>(OperationLogHelper.GetOperationLog("", "", errmsg, DateTime.Now, errinfo), typeof(OperationLog));
        }

        /// <summary>
        /// 将日子消息如内存队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="targetObj"></param>
        private static void Enqueue<T>(T message, string targetObj)
        {
            lock (obj)
            {
                if (cacheLogQueue.Any(item => item.Key == targetObj))
                {
                    cacheLogQueue[targetObj].Enqueue(message);
                }
                else
                {
                    cacheLogQueue[targetObj] = new Queue<object>();
                    cacheLogQueue[targetObj].Enqueue(message);
                }
            }
        }
    }
}
