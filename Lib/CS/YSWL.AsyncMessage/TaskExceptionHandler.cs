using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.AsyncMessage
{
    /// <summary>
    /// 任务的异常处理
    /// </summary>
    public abstract class TaskExceptionHandler<T>
    {
        /// <summary>
        /// 开启异常处理
        /// </summary>
        public static void StartExceptionHandler(HandlerItem<T> item)
        {
            TaskExceptionHandler<T> handlerObj = null;
            if (item.exceptionHandlerTarget == "Default")
            {
                handlerObj = new DefaultExceptionHandler<T>();
            }
            else
            {
                try
                {
                    handlerObj = (TaskExceptionHandler<T>)Activator.CreateInstance(item.exceptionHandlerTargetType);
                }
                catch
                {
                    handlerObj = new DefaultExceptionHandler<T>();
                }
            }
            handlerObj.AddExceptionTaskItem(item);
            handlerObj.Handler();
        }

        /// <summary>
        /// 具体异常处理抽象方法
        /// </summary>
        public abstract void Handler();

        /// <summary>
        /// 添加异常任务项
        /// </summary>
        /// <param name="item"></param>
        public abstract void AddExceptionTaskItem(HandlerItem<T> item);
    }
}
