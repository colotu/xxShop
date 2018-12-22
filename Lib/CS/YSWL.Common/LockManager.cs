using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace YSWL.Common
{
    /// <summary>
    /// 自定义锁管理
    /// </summary>
    public class LockManager 
    {
        /// <summary>
        /// 锁字典
        /// </summary>
        private static Dictionary<string, Locker> _lockobj;

        public readonly static object obj = new object();
        static LockManager()
        {
            _lockobj = new Dictionary<string, Locker>();
        }

        #region 设置区域锁
        /// <summary>
        /// 全局变量，用于是否强制断开当前请求
        /// </summary>
        private bool _isBreak = false;

        /// <summary>
        /// 获得指定区域的锁
        /// </summary>
        /// <param name="areaName">区域名称（可以自定义）</param>
        /// <param name="lockType">锁类型</param>
        /// <returns></returns>
        public static Locker SetAreaLock(string areaName, LockType lockType,Type fromType)
        {
            lock (obj)
            {
                areaName += fromType.Name;
                if (_lockobj.Any(l => l.Key == areaName))
                {
                    if (!_lockobj[areaName].IsUser)
                    {
                        _lockobj[areaName].IsUser = true;
                        _lockobj[areaName].Lockrange = LockRange.GLOBAL;
                        _lockobj[areaName].Locktype = lockType;
                        _lockobj[areaName].LockLevel = 1;                                          
                        return _lockobj[areaName];
                    }
                    return null;
                }
                Locker lockTemp = new Locker();
                lockTemp.IsUser = true;
                lockTemp.Lockrange = LockRange.GLOBAL;
                lockTemp.LockLevel = 1;
                lockTemp.Locktype = lockType;
                _lockobj.Add(areaName, lockTemp);
                return lockTemp;
            }
        }

        /// <summary>
        /// 获得指定区域的锁
        /// </summary>
        /// <param name="areaName">区域名称（可以自定义）</param>
        /// <param name="lockType">锁类型</param>
        /// <returns></returns>
        public static bool SetAreaLock(LockType lockType, string areaName, Type fromType)
        {
            lock (obj)
            {
                areaName += fromType.Name;
                if (_lockobj.Any(l => l.Key == areaName))
                {
                    if (!_lockobj[areaName].IsUser)
                    {
                        _lockobj[areaName].IsUser = true;
                        _lockobj[areaName].Lockrange = LockRange.GLOBAL;
                        _lockobj[areaName].Locktype = lockType;
                        _lockobj[areaName].LockLevel = 1;                                                 
                        return true;
                    }
                    return false;
                }
                Locker lockTemp = new Locker();
                lockTemp.IsUser = true;
                lockTemp.Lockrange = LockRange.GLOBAL;
                lockTemp.LockLevel = 1;
                lockTemp.Locktype = lockType;
                _lockobj.Add(areaName, lockTemp);
                return true;
            }
        }

        /// <summary>
        /// 删除区域锁
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public static bool DeleteAreaLock(string areaName, Type fromType)
        {
            lock (obj)
            {
                areaName = areaName + fromType.Name;
                if (_lockobj.Any(l => l.Key == areaName))
                {
                    _lockobj.Remove(areaName);
                    return true;
                }
                return false;
            }
        }

        #endregion


        /// <summary>
        /// 开始一个计时器
        /// </summary>
        /// <param name="handlerEvent"></param>
        /// <returns></returns>
        private static Timer StartTimer(ElapsedEventHandler handlerEvent, int timeCount)
        {
            Timer timer = new System.Timers.Timer(timeCount);
            timer.Elapsed += handlerEvent;
            timer.AutoReset = false;
            timer.Enabled = true;
            return timer;
        }

        /// <summary>
        /// 执行后会强制断开当前线程的执行
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void BreakCurrentThread(object source, System.Timers.ElapsedEventArgs e)
        {
            _isBreak = true;
        }
    }

    /// <summary>
    /// 锁的范围
    /// </summary>
    public enum LockRange
    {
        CURRENT,    //当前锁  只在当前线程独享
        GLOBAL      //全局锁  全局范围内独享锁
    }

     /// <summary>
    /// 锁的类型
    /// </summary>
    public enum LockType
    {
        READ,        //读锁，同时可以进行写入操作
        WRITE,       //写锁，同时可以进行读操作
        ONLYREAD,    //只读锁
        ONLYWRITE    //只写锁
    }

    /// <summary>
    /// 锁
    /// </summary>
    public class Locker
    {
        private string _targetareanum;
        private int _locktime;
        private LockRange _lockrange;
        private LockType _locktype;
        private long _locklevel;
        private bool _isuser;

        /// <summary>
        /// 目标区域编号
        /// </summary>
        public string TargetAreaNum
        {
            get { return _targetareanum; }
            set { _targetareanum = value; }
        }

        /// <summary>
        /// 锁占有时间（毫秒为单位）
        /// </summary>
        public int Locktime
        {
            get { return _locktime; }
            set { _locktime = value; }
        }

        /// <summary>
        /// 锁的范围
        /// </summary>
        public LockRange Lockrange
        {
            get { return _lockrange; }
            set { _lockrange = value; }
        }

        /// <summary>
        /// 锁的类型
        /// </summary>
        public LockType Locktype
        {
            get { return _locktype; }
            set { _locktype = value; }
        }

        public long LockLevel
        {
            get { return _locklevel; }
            set { _locklevel = value; }
        }

        /// <summary>
        /// 是否启用锁
        /// </summary>
        public bool IsUser
        {
            get { return _isuser; }
            set { _isuser = value; }
        }
    }
}
