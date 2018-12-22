using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log
{
    /// <summary>
    /// 操作日志临时实体
    /// </summary>
    public class OperationLog : TagLogBase
    {
        public OperationLog()
        {
            base.TargetAdapterAndClientObj = "LoggerAdapterOfTxt";
        }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string OperationName { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime LogDate { get; set; }
    }

    public class OperationLogHelper
    {
        public static OperationLog GetOperationLog(string userName, string userId, string operationName, DateTime dt = new DateTime(), string exceptionMessage = "")
        {
            OperationLog log = new OperationLog();
            log.UserId = userId;
            log.UserName = userName;
            log.OperationName = operationName;
            log.ExceptionMessage = exceptionMessage;
            log.LogDate = dt;
            return log;
        }

        public static OperationLog GetOperationLog(string exceptionMessage, DateTime dt = new DateTime(), string operationName = "", string userName = "", string userId = "")
        {
            OperationLog log = new OperationLog();
            log.UserId = userId;
            log.UserName = userName;
            log.OperationName = operationName;
            log.ExceptionMessage = exceptionMessage;
            log.LogDate = dt == DateTime.MinValue ? DateTime.Now : dt;
            return log;
        }

        /// <summary>
        /// 获取日期对象
        /// </summary>
        /// <param name="exceptionMessage">记录内容</param>
        /// <param name="operationName">主题或操作名称</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static OperationLog GetOperationLog(string exceptionMessage, string operationName, string userName)
        {
            OperationLog log = new OperationLog();
            log.UserName = userName;
            log.OperationName = operationName;
            log.ExceptionMessage = exceptionMessage;
            log.LogDate = DateTime.Now;
            return log;
        }
    }
}
