using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.Log
{
    public interface ILoggerAdapter
    {
        /// <summary>
        /// 由指定类型获取ILog日志实例
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        ILog GetLogger(Type type);

        /// <summary>
        /// 由指定名称获取ILog日志实例
        /// </summary>
        /// <param name="name">指定名称</param>
        /// <returns></returns>
        ILog GetLogger(string name);
    }
}
