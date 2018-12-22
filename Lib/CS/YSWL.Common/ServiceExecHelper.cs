using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace YSWL.Common
{
    /// <summary>
    /// 服务委托类
    /// </summary>
    public class ServiceExecHelper
    {
        private static List<FuncDelegate> _list = new List<FuncDelegate>();
        private static bool _isRun = false;
        private static int _num = 5; 
        private static int _sleep = 5; 
        private static bool _isEnable = false;  
        private static object _lockObj = new object();

        /// <summary>
        /// 重复次数，默认5
        /// </summary>
        public static int RepateNum
        {
            set { _num = value; }
        }
        /// <summary>
        /// 重复调用休眠时间 单位：分钟
        /// </summary>
        public static int ThreadSheep
        {
            set { _sleep = value; }
        }

        /// <summary>
        /// 是否启用任务队列 默认 false
        /// </summary>
        public static bool IsEnableQueue
        {
            set { _isEnable = value; }
        }

        /// <summary>
        /// 开启服务队列调用线程
        /// </summary>
        private static void RunQueueThread()
        {
            ThreadPool.QueueUserWorkItem(a =>
            {
                while (true)
                {
                    List<FuncDelegate> remove = new List<FuncDelegate>();
                    for (int i = 0; i < _list.Count; i++)
                    {
                        var func = _list[i];
                        bool result = false;
                        if (func.Func != null)
                        {
                            try
                            {
                                result = func.Func();
                            }
                            catch (Exception)
                            {
                            }
                            if (result || ++func.CallNum >= _num)//执行成功，或者次数大于5次 则移除队列
                            {
                                remove.Add(func);
                            }
                        }
                        else
                            remove.Add(func);
                    }
                    lock (_lockObj)
                    {
                        _list.RemoveAll(b => remove.Exists(c => b == c));
                    }
                    Thread.Sleep(1000 * 60 * _sleep);//30分钟执行一次,可以重试5次
                }
            });
        }

        /// <summary>
        /// 服务队列增加方法
        /// </summary>
        /// <param name="func"></param>
        public static void AddQueue(Func<bool> func)
        {
            if (func == null) return;
            //执行一次， 成功则不加入队列
            try
            {
                bool result = func();
                if (result) return;
            }
            catch (Exception)
            {
            }
            if (!_isEnable) return;//如果未开启队列服务 则直接返回
            FuncDelegate funcDelegate = new FuncDelegate();
            funcDelegate.CallNum = 1;
            funcDelegate.Func = func;
            lock (_lockObj)
            {
                _list.Add(funcDelegate);
            }
            // 在添加委托是 直接根据配置开启服务队列
            if (_list.Count > 0 && !_isRun && _isEnable)
            {
                lock (_lockObj)
                {
                    if (_list.Count > 0 && !_isRun && _isEnable)
                    {
                        RunQueueThread();
                        _isRun = true;
                    }
                }
            }
        }
        /// <summary>
        /// 清除队列数据
        /// </summary>
        private static void ClearQueue()
        {
            _list.Clear();
        }
        class FuncDelegate
        {
            public Func<bool> Func { get; set; }
            public int CallNum { get; set; }
        }
    }
}
