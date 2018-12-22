using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace YSWL.AsyncMessage
{
    /// <summary>
    /// 线程池扩展
    /// </summary>
    public class ThreadPoolExtend<T> : IDisposable
    {

        #region 线程本身属性设置相关

        private int max = 4;
        private int min = 2;
        private int increment = 2;

        /// <summary>
        /// 最大线程数
        /// </summary>
        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        /// <summary>
        /// 最小开启线程数
        /// </summary>
        public int Min
        {
            get { return min; }
            set { min = value; }
        }

        /// <summary>
        /// 每次增量开启线程数
        /// </summary>
        public int Increment
        {
            private get { return increment; }
            set { increment = value; }
        }

        #endregion

        //private int obj122 = 0;

        #region 线程管理字典相关

        /// <summary>
        /// 所有的线程管理对象
        /// </summary>
        private Dictionary<string, ThreadExtend<T>> allThread = null;

        /// <summary>
        /// 工作线程
        /// </summary>
        private Dictionary<string, ThreadExtend<T>> workThread = null;

        /// <summary>
        /// 空闲线程
        /// </summary>
        private Queue<ThreadExtend<T>> freeThread = null;

        #endregion

        /// <summary>
        /// 任务队列
        /// </summary>
        private Queue<HandlerItem<T>> taskQueue;

        public static readonly object obj = new object();

        private int index = 10;
        public ThreadPoolExtend()
        {
            taskQueue = new Queue<HandlerItem<T>>();
        }


        private bool isOpenThread = false;

        /// <summary>
        /// 开启线程池
        /// </summary>
        public void Start()
        {
            if (min <= 0 || max <= 0 || min > max)
            {
                throw new Exception("参数设置异常，必须正确设置初始线程数、最大线程数，或者使用默认设置");
            }
            allThread = new Dictionary<string, ThreadExtend<T>>();
            workThread = new Dictionary<string, ThreadExtend<T>>();
            freeThread = new Queue<ThreadExtend<T>>();
            ThreadExtend<T> thread = null;
            for (int i = 0; i < min; i++)
            {
                index++;
                thread = new ThreadExtend<T>(index);
                thread.WorkComplete += new Action<ThreadExtend<T>>(WorkComplete);
                allThread.Add(thread.Key, thread);
                freeThread.Enqueue(thread);
            }
            if (!isOpenThread)
            {
                isOpenThread = true;
                RealStartThread();
            }
        }

        /// <summary>
        /// 添加处理任务对象
        /// </summary>
        /// <param name="taskItem">目标执行方法</param>
        /// <param name="context">上下文对象</param>
        /// <param name="backAction">回调方法</param>       
        public void AddTaskItem(Action<T> taskItem, T context, WaitCallback backAction = null)
        {
            HandlerItem<T> item = new HandlerItem<T>();
            item.contextObj = context;
            item.taskItemObj = taskItem;
            item.backAction = backAction;
            AddTaskItem(item);
        }

        /// <summary>
        ///  添加处理任务对象，同时指定异常处理
        /// </summary>
        /// <param name="taskItem">目标执行委托</param>
        /// <param name="context">执行上下文参数</param>
        /// <param name="excetionHandlerTargetType">异常处理类型（必须继承实现TaskExceptionHandler）</param>
        /// <param name="backAction">回调方法</param>       
        public void AddTaskItem(Action<T> taskItem, T context, Type excetionHandlerTargetType, WaitCallback backAction = null)
        {
            HandlerItem<T> item = new HandlerItem<T>();
            item.contextObj = context;
            item.taskItemObj = taskItem;
            item.backAction = backAction;
            if (excetionHandlerTargetType != null)
            {
                item.exceptionHandlerTarget = excetionHandlerTargetType.FullName;
                item.exceptionHandlerTargetType = excetionHandlerTargetType;
            }
            AddTaskItem(item);
        }


        private void AddTaskItem(HandlerItem<T> item)
        {
            lock (obj)
            {
                taskQueue.Enqueue(item);
                #region old

                //ThreadExtend thread = null;
                //if (freeThread.Count > 0)
                //{
                //    thread = freeThread.Dequeue();//获取空闲线程
                //    workThread.Add(thread.Key, thread);//工作线程添加该执行线程
                //    thread.SetWorkTask(item);
                //    thread.StartThread();
                //    return;
                //}
                //int threadCount = allThread.Values.Count();
                //if (threadCount < max)
                //{
                //    //增量创建线程
                //    for (int i = 0; i < increment; i++)
                //    {
                //        if ((threadCount + i) < max)
                //        {
                //            index++;
                //            thread = new ThreadExtend(index);
                //            thread.WorkComplete += new Action<ThreadExtend>(WorkComplete);
                //            allThread.Add(thread.Key, thread);
                //            freeThread.Enqueue(thread);
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //    //获取空闲线程
                //    var freeWorkThread = freeThread.Dequeue();
                //    workThread.Add(freeWorkThread.Key, freeWorkThread);
                //    freeWorkThread.SetWorkTask(item);
                //    freeWorkThread.StartThread();
                //    return;
                //}
                //else
                //{
                //    taskQueue.Enqueue(item);//将消息入队
                //}

                #endregion
            }
        }

        /// <summary>
        /// 开启线程方法
        /// </summary>
        private void RealStartThread()
        {
            if (taskQueue.Any() && allThread != null)
            {
                lock (obj)
                {
                    int count = taskQueue.Count();
                    int threadCount = allThread.Count();

                    if (15 <= count && count <= 35)
                    {
                        OpenThread(increment, threadCount);
                    }
                    else if (count > 35)
                    {
                        int sCount = max - threadCount;
                        OpenThread(sCount, threadCount);
                    }
                    threadCount = allThread.Count();
                    for (int i = 0; i < threadCount; i++)
                    {
                        var item = taskQueue.Dequeue();
                        //获取空闲线程
                        var freeWorkThread = freeThread.Dequeue();
                        workThread.Add(freeWorkThread.Key, freeWorkThread);
                        freeWorkThread.SetWorkTask(item);
                        freeWorkThread.StartThread();
                    }
                }
            }
            else
            {
                throw new Exception("任务队列为空，请先添加任务");
            }
        }

        /// <summary>
        /// 开启线程（总线程不能大于最大线程数）
        /// </summary>
        /// <param name="count">要开启的线程数</param>
        /// <param name="threadCount">已有线程数量</param>
        private void OpenThread(int count, int threadCount)
        {
            ThreadExtend<T> thread = null;
            //增量创建线程
            for (int i = 0; i < count; i++)
            {
                if ((threadCount + i) < max)
                {
                    index++;
                    thread = new ThreadExtend<T>(index);
                    thread.WorkComplete += new Action<ThreadExtend<T>>(WorkComplete);
                    allThread.Add(thread.Key, thread);
                    freeThread.Enqueue(thread);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 线程工作执行完成后执行回调方法
        /// </summary>
        /// <param name="thread">当前执行线程</param>
        public void WorkComplete(ThreadExtend<T> thread)
        {
            lock (obj)
            {
                if (taskQueue.Count() > 0)
                {
                    HandlerItem<T> taskItem = taskQueue.Dequeue();
                    thread.SetWorkTask(taskItem);
                    thread.StartThread();
                }
                else
                {
                    workThread.Remove(thread.Key);
                    if (freeThread.Count() >= min)
                    {
                        allThread.Remove(thread.Key);
                        thread.Close();
                        thread.Dispose();
                    }
                    else
                    {
                        thread.SetWorkTask(null);
                        freeThread.Enqueue(thread);
                    }
                    if (!workThread.Any())
                    {
                        Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 是否存在未执行任务，如果不存在同时工作线程执行完成，清理当前线程池
        /// </summary>
        /// <returns></returns>
        public bool IsExistTask()
        {
            return workThread.Any() || taskQueue.Any();
        }

        /// <summary>
        /// 是否被清空
        /// </summary>
        /// <returns></returns>
        public bool IsExistThreadPoolObj()
        {
            return allThread.Any();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            lock (obj)
            {
                foreach (var item in allThread)
                {
                    using (item.Value)
                    {
                        item.Value.Close();
                        item.Value.Dispose();
                    }
                }
                allThread.Clear();
                workThread.Clear();
                freeThread.Clear();
                taskQueue.Clear();
            }
        }
    }
}
