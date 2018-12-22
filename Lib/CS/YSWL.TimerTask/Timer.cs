/**
* Timer.cs
*
* 功 能： [N/A]
* 类 名： Timer
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/8/21 19:11:18  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using YSWL.TimerTask.Model;
namespace YSWL.TimerTask
{
    public class Timer : System.Timers.Timer
    {
        private readonly TaskTimer _taskTimer;

        /// <summary>
        /// 流水号
        /// </summary>
        public TaskTimer TaskTimer
        {
            get { return _taskTimer; }
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="taskTimer">流水号</param>
        /// <param name="executeTime">max24天</param>
        /// <param name="action">方法</param>
        /// <param name="callback">执行后回调</param>
        /// <param name="args">参数 max10</param>
        public Timer(TaskTimer taskTimer, DateTime executeTime, Action<string[]> action, Action<TaskTimer> callback, string[] args)
        {
            _taskTimer = taskTimer;
            double interval = (executeTime - DateTime.Now).TotalMilliseconds;
            if (interval >= int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("执行时间超过最大值 24天!");
            }
            base.Elapsed += (obj, e) => action(args);
            base.Elapsed += (obj, e) => callback(_taskTimer);
            base.AutoReset = false; //TODO: 循环执行暂不支持
            base.Interval = interval > 0 && interval < int.MaxValue ? interval : 100;
            base.Enabled = true;
        }
    }
}
