/**
* Task.cs
*
* 功 能： [N/A]
* 类 名： Task
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/8/21 19:20:39  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using YSWL.TimerTask.Model;
using System.Reflection;
using System.Linq.Expressions;
namespace YSWL.TimerTask
{
    public class Task : IDisposable
    {
        private static readonly Task _task = new Task();
        private bool _disposed;
        private List<Timer> _listTask = new List<Timer>();

        private Task() { }

        public static Task Instance()
        {
            return _task;
        }

        public List<Timer> CurrentTasks
        {
            get { return _listTask; }
        }

        public static void Add(DateTime executeTime, Action<string[]> action, string[] args)
        {
            Add(executeTime, action, args, true);
        }


        public static int Add(DateTime executeTime, Action<string[]> action, string[] args, bool isSingle)
        {
            DateTime timeNow = DateTime.Now;

            if (executeTime < DateTime.Now) executeTime = timeNow.AddSeconds(1);

            if (action.Method.DeclaringType == null) return -1;

            TaskTimer model = new TaskTimer
            {
                IsSingle = isSingle,
                ExecuteType = string.Format("{0}|{1}", action.Method.DeclaringType.AssemblyQualifiedName, action.Method.Name),
                ExecuteTime = executeTime,
                ExecuteNumber = 0,
                //TODO: 此属性未使用
                Interval = (decimal)(executeTime - timeNow).TotalMilliseconds,
                Params = args
            };
            model.ID = BLL.TaskTimer.Add(model);

            Task.Instance().CurrentTasks.Add(new Timer(model, executeTime, action, CallBack, args));
            return model.ID;
        }

        #region DeleteTask
        public static bool Delete(int id)
        {
            return Task.Instance().DeleteTask(id);
        }

        private bool DeleteTask(int id)
        {
            lock (this)
            {
                _listTask.ForEach(item =>
                {
                    if (item != null && item.TaskTimer.ID == id)
                    {
                        item.Dispose();
                    }
                });
                return BLL.TaskTimer.Delete(id);
            }
        }
        #endregion

        #region ResetTask

        public static void Reset(TaskTimer taskTimer)
        {
             Task.Instance().ResetTask(taskTimer);
        }
        private void ResetTask(TaskTimer taskTimer)
        {
            lock (this)
            {
                _listTask.ForEach(item =>
                {
                    if (item != null && item.TaskTimer.ID == taskTimer.ID)
                    {
                        item.TaskTimer.ExecuteTime = taskTimer.ExecuteTime;
                        double interval = (item.TaskTimer.ExecuteTime - DateTime.Now).TotalMilliseconds;
                        item.Interval = interval > 0 && interval < int.MaxValue ? interval : 100;
                        item.Enabled = true;
                    }
                });
            }
        }
        #endregion


        public static void CallBack(TaskTimer taskTimer)
        {
            if (!taskTimer.IsSingle)
            {
                //TODO: 时间维度无法控制
                taskTimer.ExecuteTime = taskTimer.ExecuteTime.AddDays(1);
                taskTimer.ExecuteNumber++;
                BLL.TaskTimer.Update(taskTimer);
                Reset(taskTimer);
            }
            else
            {
                BLL.TaskTimer.Delete(taskTimer.ID);
            }
        }

        public void Start()
        {
            lock (this)
            {
                _listTask.Clear();

                List<Model.TaskTimer> list = BLL.TaskTimer.GetModelList("");
                if (list == null || list.Count < 1) return;

                Type type;
                MethodInfo methodInfo = null;
                Action<string[]> action = null;

                list.ForEach(item =>
                {
                    if (string.IsNullOrEmpty(item.ExecuteType)) return;

                    string[] tmpType = item.ExecuteType.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmpType.Length != 2) return;

                    string[] args =
                    {
                        item.Param1, item.Param2, item.Param3, item.Param4,
                        item.Param5, item.Param6, item.Param7,
                        item.Param8, item.Param9, item.Param10
                    };

                    try
                    {
                        type = Type.GetType(tmpType[0]);
                        if (type == null) return;

                        methodInfo = type.GetMethod(tmpType[1]);

                        if (methodInfo.IsStatic)
                        {
                            ParameterExpression parameterExpression = Expression.Parameter(typeof(string[]));
                            MethodCallExpression methodCallExpression = Expression.Call(methodInfo, parameterExpression);
                            action =
                                Expression.Lambda<Action<string[]>>(methodCallExpression, parameterExpression).Compile();
                        }
                        else
                        {
                            object obj = Activator.CreateInstance(type);
                            ParameterExpression parameterExpression = Expression.Parameter(typeof(string[]));
                            MethodCallExpression methodCallExpression = Expression.Call(Expression.Constant(obj),
                                methodInfo, parameterExpression);
                            action =
                                Expression.Lambda<Action<string[]>>(methodCallExpression, parameterExpression).Compile();
                        }
                    }
                    catch (Exception){}
                    _listTask.Add(
                        new Timer(item, item.ExecuteTime, action, CallBack, args));
                });
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (!_disposed)
            {
                lock (this)
                {
                    _listTask.ForEach(item =>
                    {
                        if (item != null)
                        {
                            item.Dispose();
                        }
                    });
                    _listTask.Clear();

                    this._disposed = true;
                }
            }
        }

        #endregion
    }
}
