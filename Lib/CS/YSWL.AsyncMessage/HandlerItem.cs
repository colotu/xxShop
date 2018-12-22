using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace YSWL.AsyncMessage
{
    /// <summary>
    /// 处理项
    /// </summary>
    public class HandlerItem<T>
    {
        /// <summary>
        /// 目标执行对象
        /// </summary>
        public Action<T> taskItemObj;

        /// <summary>
        /// 回调方法
        /// </summary>
        public WaitCallback backAction;

        /// <summary>
        /// 执行上下文参数
        /// </summary>
        public T contextObj;

        /// <summary>
        /// 异常处理对象标识（没有实现可以不设置，走默认设置）
        /// </summary>
        public string exceptionHandlerTarget = "Default";

        /// <summary>
        /// 异常处理对象类型，必须实现TaskExceptionHandler异常处理抽象基类。（可以不设置，走默认）
        /// </summary>
        public Type exceptionHandlerTargetType;
    }
}
