using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log
{
    /// <summary>
    /// 具体写日志的标记接口（为支持工厂创建对象使用，配合TagLogBase中TargetObj字段启用工厂模式）
    /// </summary>
    public interface ILogTag
    {
        void Write(LogLevel level, object message, Exception exception);
    }
}
