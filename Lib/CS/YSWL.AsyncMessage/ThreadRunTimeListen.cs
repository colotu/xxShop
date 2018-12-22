using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace YSWL.AsyncMessage
{
    /// <summary>
    /// 线程运行时间监听
    /// </summary>
    public class ThreadRunTimeListen<T>
    {
        /// <summary>
        /// 开启的线程字典对象
        /// </summary>
        private static Dictionary<string, ThreadObject> threadPoolExtends = new Dictionary<string, ThreadObject>();

        private static readonly object obj = new object();

        private static bool isStart = false;

        private static int index = 10;

        /// <summary>
        /// 添加线程池监听
        /// </summary>
        /// <param name="threadPoolExtend"></param>
        public static void AddThreadListen(ThreadPoolExtend<T> threadPoolExtend)
        {
            ThreadObject threadObj = new ThreadObject();
            threadObj.threadPoolExtend = threadPoolExtend;
            threadObj.dt = DateTime.Now;
            threadObj.timeSpan = 1;
            index++;
            lock (obj)
            {
                if (!threadPoolExtends.Any(k => k.Value.threadPoolExtend == threadPoolExtend))
                {
                    threadPoolExtends.Add(new Guid().ToString() + index, threadObj);
                }
            }
        }

        /// <summary>
        /// 监控线程池运行时间并及时清理
        /// </summary>
        public static void RunTimeListen()
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
                            if (!threadPoolExtends.Any())
                            {
                                isStart = false;//当前处理完成后停止当前线程
                                Thread.CurrentThread.DisableComObjectEagerCleanup();
                                return;
                            }
                            Dictionary<string, ThreadObject> temps = new Dictionary<string, ThreadObject>();
                            foreach (var item in threadPoolExtends)
                            {
                                if (item.Value.dt.AddMinutes(item.Value.timeSpan) > DateTime.Now)
                                {
                                    continue;
                                }
                                if (!item.Value.threadPoolExtend.IsExistTask())
                                {
                                    item.Value.threadPoolExtend.Dispose();
                                    temps.Add(item.Key, item.Value);
                                }
                                else
                                {
                                    item.Value.dt = DateTime.Now;
                                }
                            }
                            foreach (var item in temps)
                            {
                                threadPoolExtends.Remove(item.Key);
                            }
                            Thread.Sleep(1000);
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

        /// <summary>
        /// 监控线程临时对象
        /// </summary>
        class ThreadObject
        {
            public ThreadPoolExtend<T> threadPoolExtend;

            /// <summary>
            /// 起始时间
            /// </summary>
            public DateTime dt;

            /// <summary>
            /// 时间间隔
            /// </summary>
            public int timeSpan;
        }
    }
}
