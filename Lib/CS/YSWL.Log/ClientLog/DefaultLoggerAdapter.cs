using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log.ClientLog
{
    /// <summary>
    /// 创建工厂模式对象
    /// </summary>
    public class DefaultLoggerAdapter : LoggerAdapterBase
    {

        protected override ILog CreateLogger(string name)
        {
            return new DefaultLogger();
        }
    }
}
