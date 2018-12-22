using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log.ClientLog
{
    /// <summary>
    /// 默认的txt对象适配器
    /// </summary>
    public class LoggerAdapterOfTxt : LoggerAdapterBase
    {
        protected override ILog CreateLogger(string name)
        {
            return new LoggerOfTxt();
        }
    }
}
