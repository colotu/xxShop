using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace YSWL.AsyncMessage
{
    /// <summary>
    /// 默认异常处理
    /// </summary>
    public class DefaultExceptionHandler<T> : TaskExceptionHandler<T>, IDisposable
    {
        /// <summary>
        /// 全局的异常处理队列
        /// </summary>
        private static Queue<HandlerItem<T>> exceptionQueue = new Queue<HandlerItem<T>>();

        private static readonly object obj = new object();

        private static bool isStart = false;

        /// <summary>
        /// 添加异常任务到异常队列
        /// </summary>
        public override void AddExceptionTaskItem(HandlerItem<T> item)
        {
            lock (obj)
            {
                exceptionQueue.Enqueue(item);
            }
        }

        /// <summary>
        /// 具体处理对象
        /// </summary>
        public override void Handler()
        {
            if (!isStart)
            {
                isStart = true;//同一时间只能开启一个线程处理               
                Thread thread = new Thread(() =>
                {
                    while (isStart)
                    {
                        try
                        {
                            //停留5秒后判断是否有未处理，如果没有直接停掉当前线程
                            if (!exceptionQueue.Any())
                            {
                                Thread.Sleep(5000);
                            }
                            if (!exceptionQueue.Any())
                            {
                                isStart = false;//当前处理完成后停止当前线程
                                Thread.CurrentThread.DisableComObjectEagerCleanup();
                                return;
                            }
                            HandlerItem<T> hadlerItem = null;
                            lock (obj)
                            {
                                hadlerItem = exceptionQueue.Dequeue();
                            }
                            hadlerItem.taskItemObj(hadlerItem.contextObj);
                        }
                        catch (Exception ex)
                        {
                            //待处理
                        }
                    }
                });
                thread.Start();
                thread.IsBackground = true;
            }
        }

        public void Dispose()
        {
            exceptionQueue.Clear();
        }
    }
}
