using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace YSWL.Log.ClientLog
{
    /// <summary>
    /// 工厂模式日志记录实现
    /// </summary>
    public class DefaultLogger : LogBase
    {
        private static Dictionary<string, ILogTag> clientLogs = new Dictionary<string, ILogTag>();
       
        #region


        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public DefaultLogger()
        {
        }

        /// <summary>
        /// 获取日志输出处理委托实例
        /// </summary>
        /// <param name="level">日志输出级别</param>
        /// <param name="message">日志消息</param>
        /// <param name="exception">日志异常</param>
        protected override void Write(LogLevel level, object message, Exception exception)
        {
            var targetObj = message.GetType().GetProperty("TargetObj").GetValue(message, null) + "";
            if (string.IsNullOrEmpty(targetObj + ""))
            {
                throw new Exception("必须设置目标日志记录对象！");
            }
            ILogTag targetLogger;
            if (clientLogs.TryGetValue(targetObj, out targetLogger))
            {
                //调用目标对象
                targetLogger.Write(level, message, exception);
                return;
            }
            else
            {
                var tagetStr = ConfigurationManager.AppSettings[targetObj];
                if (!string.IsNullOrEmpty(tagetStr))
                {
                    string[] strs = tagetStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    targetLogger = Assembly.Load(strs[1]).CreateInstance(strs[0]) as ILogTag;
                    if (targetLogger != null)
                    {
                        if (!clientLogs.Any(temp => temp.Key == targetObj))
                        {
                            clientLogs.Add(targetObj, targetLogger);//缓存
                        }
                        //调用目标对象
                        targetLogger.Write(level, message, exception);
                        return;
                    }
                }
                throw new Exception("必须在配置中设置目标日志对象的程序集配置！");
            }
        }
    }
}
