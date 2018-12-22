using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace YSWL.Log.ClientLog
{
    /// <summary>
    /// 写txt文件日志的默认实现
    /// </summary>
    public class LoggerOfTxt : LogBase
    {
        protected override void Write(LogLevel level, object message, Exception exception)
        {
            string pathStr = AppDomain.CurrentDomain.BaseDirectory;
            string appSetPath = ConfigurationManager.AppSettings.Get("logPath");
            string dateFormat = ConfigurationManager.AppSettings.Get("dateFormat");
            if (pathStr.IndexOf("bin") > 0)
            {
                pathStr = pathStr.Substring(0, pathStr.IndexOf("bin")) + "logs\\";
            }
            else
            {
                pathStr = pathStr + "logs\\";
            }
            string path = string.IsNullOrEmpty(appSetPath) ? pathStr : appSetPath;
            OperationLog log = (OperationLog)message;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream fs = new FileStream(path + DateTime.Now.ToString(string.IsNullOrEmpty(dateFormat) ? "yyyyMMdd" : dateFormat) + ".txt", FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("发生时间：" + log.LogDate);
                    sb.AppendLine("事件级别：" + level.ToString());
                    sb.AppendLine("事件来源：" + log.OperationName);
                    sb.AppendLine("事件操作人信息：" + log.UserName + "(" + log.UserId + ")");
                    sb.AppendLine("事件内容：" + log.ExceptionMessage);
                    sb.AppendLine("---------------------------------------------------------");
                    sw.Write(sb.ToString());
                    sw.Flush();
                }
            }
        }
    }
}
