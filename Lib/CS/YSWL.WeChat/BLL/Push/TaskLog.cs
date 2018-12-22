/**  版本信息模板在安装目录下，可自行修改。
* TaskLog.cs
*
* 功 能： N/A
* 类 名： TaskLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/7 17:57:54   N/A    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.Common;
using YSWL.WeChat.Model.Push;
using YSWL.WeChat.IDAL.Push;
namespace YSWL.WeChat.BLL.Push
{
	/// <summary>
	/// TaskLog
	/// </summary>
	public partial class TaskLog
	{
        private readonly ITaskLog dal = YSWL.DBUtility.PubConstant.IsSQLServer ? (ITaskLog)new YSWL.WeChat.SQLServerDAL.Push.TaskLog() : (ITaskLog)new YSWL.WeChat.MySqlDAL.Push.TaskLog();
		public TaskLog()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int TaskId,string UserName)
		{
			return dal.Exists(TaskId,UserName);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.WeChat.Model.Push.TaskLog model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.WeChat.Model.Push.TaskLog model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
        public bool Delete(int TaskId, string UserName, string tableName)
		{

            return dal.Delete(TaskId, UserName, tableName);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public YSWL.WeChat.Model.Push.TaskLog GetModel(int TaskId, string UserName, string tableName)
		{

            return dal.GetModel(TaskId, UserName, tableName);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.WeChat.Model.Push.TaskLog GetModelByCache(int TaskId,string UserName,string tableName)
		{
			
			string CacheKey = "TaskLogModel-" + TaskId+UserName;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
                    objModel = dal.GetModel(TaskId, UserName, tableName);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.WeChat.Model.Push.TaskLog)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
        public DataSet GetList(string strWhere, string tableName)
		{
            return dal.GetList(strWhere, tableName);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder, string tableName)
		{
            return dal.GetList(Top, strWhere, filedOrder, tableName);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
        public List<YSWL.WeChat.Model.Push.TaskLog> GetModelList(string strWhere, string tableName)
		{
            DataSet ds = dal.GetList(strWhere, tableName);
            if (ds != null)
            {
                return DataTableToList(ds.Tables[0]);
            }
            return null;
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.WeChat.Model.Push.TaskLog> DataTableToList(DataTable dt)
		{
			List<YSWL.WeChat.Model.Push.TaskLog> modelList = new List<YSWL.WeChat.Model.Push.TaskLog>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.WeChat.Model.Push.TaskLog model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList(string tableName)
		{
			return GetList("", tableName);
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
        public int GetRecordCount(string strWhere, string tableName)
		{
			return dal.GetRecordCount(strWhere,tableName);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex, string tableName)
		{
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex, tableName);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod

		#region  ExtensionMethod
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool Add(int taskId, string userName)
        {
            YSWL.WeChat.BLL.Push.TaskLog logBll = new TaskLog();
            return logBll.dal.Add(taskId, userName);
        }
        /// <summary>
        /// 获取用户的记录
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static List<YSWL.WeChat.Model.Push.TaskLog> GetUserLogs(string userName)
        {
             YSWL.WeChat.BLL.Push.TaskLog logBll = new TaskLog();
             string tableName = "WeChat_TaskLog_" + DateTime.Today.ToString("yyyyMMdd");
             return logBll.GetModelList(" userName='" + Common.InjectionFilter.SqlFilter(userName) + "'", tableName);
        }
		#endregion  ExtensionMethod
	}
}

