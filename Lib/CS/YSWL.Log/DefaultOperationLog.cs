using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log
{
    public class DefaultOperationLog
    {
        /// <summary>
        /// 记录跟踪日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="operatorName"></param>
        public static void Trace(string title, string msg, string operatorName)
        {
            LogHelper.Trace<OperationLog>(OperationLogHelper.GetOperationLog(msg, title, operatorName), typeof(OperationLog));
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="operatorName"></param>
        public static void Debug(string title, string msg, string operatorName)
        {
            LogHelper.Debug<OperationLog>(OperationLogHelper.GetOperationLog(msg, title, operatorName), typeof(OperationLog));
        }

        /// <summary>
        /// 记录正常行为日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="operatorName"></param>
        public static void Info(string title, string msg, string operatorName)
        {
            LogHelper.Info<OperationLog>(OperationLogHelper.GetOperationLog(msg, title, operatorName), typeof(OperationLog));
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="operatorName"></param>
        public static void Warn(string title, string msg, string operatorName)
        {
            LogHelper.Warn<OperationLog>(OperationLogHelper.GetOperationLog(msg, title, operatorName), typeof(OperationLog));
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="operatorName"></param>
        public static void Error(string title, string msg, string operatorName)
        {
            LogHelper.Error<OperationLog>(OperationLogHelper.GetOperationLog(msg, title, operatorName), typeof(OperationLog));
        }

        /// <summary>
        /// 记录其他异常日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="operatorName"></param>
        public static void Fatal(string title, string msg, string operatorName)
        {
            LogHelper.Fatal<OperationLog>(OperationLogHelper.GetOperationLog(msg, title, operatorName), typeof(OperationLog));
        }

    }
}
