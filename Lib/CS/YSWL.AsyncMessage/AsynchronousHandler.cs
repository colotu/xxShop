using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace YSWL.AsyncMessage
{
    /// <summary>
    /// 异常处理管理
    /// </summary>
    public class AsynchronousHandler<T>
    {
        /// <summary>
        /// 线程池对象
        /// </summary>
        private ThreadPoolExtend<T> threadPoolExtend;

        private bool isInit = false;

        public AsynchronousHandler()
        {
            threadPoolExtend = new ThreadPoolExtend<T>();
        }

        /// <summary>
        /// 设置最大线程数
        /// </summary>
        /// <param name="max"></param>
        public AsynchronousHandler<T> SetMaxThread(int max)
        {
            if (isInit)
            {
                throw ThrowException("参数设定在初始化之前进行");
            }
            if (max <= 0)
            {
                throw ThrowException("最大线程数必须大于0");
            }
            threadPoolExtend.Max = max;
            return this;
        }

        /// <summary>
        /// 设置初始线程数
        /// </summary>
        /// <param name="min"></param>
        public AsynchronousHandler<T> SetMinThread(int min)
        {
            if (isInit)
            {
                throw ThrowException("参数设定在初始化线程之前进行");
            }
            if (min <= 0)
            {
                throw ThrowException("初始线程必须大于0");
            }
            if (min > threadPoolExtend.Max)
            {
                throw ThrowException("最大线程数不能小于起始线程数");
            }
            threadPoolExtend.Min = min;
            return this;
        }

        /// <summary>
        /// 初始化线程池
        /// </summary>
        /// <returns></returns>
        public AsynchronousHandler<T> Start()
        {
            threadPoolExtend.Start();
            isInit = true;
            return this;
        }

        /// <summary>
        /// 添加处理任务对象
        /// </summary>
        /// <param name="taskItem">目标执行方法</param>
        /// <param name="context">上下文对象</param>
        /// <param name="backAction">回调方法</param>
        /// <returns></returns>
        public AsynchronousHandler<T> AddTaskItem(Action<T> taskItem, T context, WaitCallback backAction = null)
        {
            AddTask(taskItem, context, null, backAction);
            return this;
        }

        /// <summary>
        ///  添加处理任务对象，同时指定异常处理
        /// </summary>
        /// <param name="taskItem">目标执行委托</param>
        /// <param name="context">执行上下文参数</param>
        /// <param name="excetionHandlerTargetType">异常处理类型（必须继承实现TaskExceptionHandler）</param>
        /// <param name="backAction">回调方法</param>
        /// <returns></returns>
        public AsynchronousHandler<T> AddTaskItem(Action<T> taskItem, T context, Type excetionHandlerTargetType, WaitCallback backAction = null)
        {
            AddTask(taskItem, context, excetionHandlerTargetType, backAction);
            return this;
        }

        private void AddTask(Action<T> taskItem, T context, Type excetionHandlerTargetType, WaitCallback backAction)
        {
            if (taskItem == null)
            {
                throw ThrowException("必须设置目标执行方法");
            }
            threadPoolExtend.AddTaskItem(taskItem, context, excetionHandlerTargetType, backAction);
        }

        /// <summary>
        /// 扔出异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Exception ThrowException(string ex)
        {
            throw new Exception(ex);
        }
    }
}
