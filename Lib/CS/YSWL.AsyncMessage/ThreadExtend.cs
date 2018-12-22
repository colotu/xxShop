using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace YSWL.AsyncMessage
{
    /// <summary>
    /// 线程扩展
    /// </summary>
    public class ThreadExtend<T> : IDisposable
    {
        /// <summary>
        /// 线程锁
        /// </summary>
        private AutoResetEvent locks;

        /// <summary>
        /// 线程对象
        /// </summary>
        private Thread thread_obj;

        /// <summary>
        /// 线程执行完成后回调方法
        /// </summary>
        public Action<ThreadExtend<T>> WorkComplete;

        /// <summary>
        /// 监听对象
        /// </summary>
        private IThreadListen threadListen;

        /// <summary>
        /// 工作标识
        /// </summary>
        private bool work;

        /// <summary>
        /// 线程执行上下文对象
        /// </summary>
        public HandlerItem<T> contextObj
        {
            get;
            set;
        }

        /// <summary>
        /// 线程的唯一标识
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        public ThreadExtend(int index)
        {
            locks = new AutoResetEvent(false);
            work = true;
            Key = Guid.NewGuid().ToString() + index;//线程唯一标识
            //初始化线程
            thread_obj = new Thread(Work);
            thread_obj.IsBackground = true;
            thread_obj.Start();
            //初始化监听对象
            threadListen = ThreadListenProvider.Current.CreateThreadListen();
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        public void StartThread()
        {
            locks.Set();//激活线程执行
        }

        /// <summary>
        /// 设置工作任务和上下文数据
        /// </summary>
        /// <param name="action"></param>
        /// <param name="contextObj"></param>
        public void SetWorkTask(HandlerItem<T> contextObj)
        {
            this.contextObj = contextObj;
        }

        /// <summary>
        /// 线程工作方法
        /// </summary>
        private void Work()
        {
            while (work)
            {
                try
                {
                    locks.WaitOne();//阻塞当前线程   
                    if (threadListen != null)
                    {
                        threadListen.BeforeExcuting(contextObj.contextObj);
                    }
                    contextObj.taskItemObj(contextObj.contextObj);
                    if (contextObj.backAction != null)
                    {
                        contextObj.backAction(contextObj.contextObj);
                    }
                    if (threadListen != null)
                    {
                        threadListen.AfterExcuted(contextObj.contextObj);
                    }
                }
                catch (Exception ex)
                {
                    if (threadListen != null)
                    {
                        threadListen.ExceptionExcute(contextObj.contextObj, ex);
                    }
                    //执行异常操作
                    TaskExceptionHandler<T>.StartExceptionHandler(contextObj);
                }
                finally
                {
                    WorkComplete(this);
                }
            }
        }

        public void Close()
        {
            work = false;
        }

        /// <summary>
        /// 清理当前线程
        /// </summary>
        public void Dispose()
        {
            try
            {
                thread_obj.DisableComObjectEagerCleanup();//关闭线程并清理
            }
            catch { }
        }
    }
}
