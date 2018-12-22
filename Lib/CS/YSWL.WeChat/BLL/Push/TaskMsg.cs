/**  版本信息模板在安装目录下，可自行修改。
* TaskMsg.cs
*
* 功 能： N/A
* 类 名： TaskMsg
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/7 17:58:09   N/A    初版
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
using YSWL.WeChat.BLL.Core;
using System.Linq;
namespace YSWL.WeChat.BLL.Push
{
	/// <summary>
	/// TaskMsg
	/// </summary>
	public partial class TaskMsg
	{
        private readonly ITaskMsg dal = YSWL.DBUtility.PubConstant.IsSQLServer ? (ITaskMsg)new YSWL.WeChat.SQLServerDAL.Push.TaskMsg() : (ITaskMsg)new YSWL.WeChat.MySqlDAL.Push.TaskMsg();
		public TaskMsg()
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
		public bool Exists(int TaskId)
		{
			return dal.Exists(TaskId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(YSWL.WeChat.Model.Push.TaskMsg model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.WeChat.Model.Push.TaskMsg model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int TaskId)
		{
			
			return dal.Delete(TaskId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string TaskIdlist )
		{
			return dal.DeleteList(TaskIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public YSWL.WeChat.Model.Push.TaskMsg GetModel(int TaskId)
		{
			
			return dal.GetModel(TaskId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public YSWL.WeChat.Model.Push.TaskMsg GetModelByCache(int TaskId)
		{
			
			string CacheKey = "TaskMsgModel-" + TaskId;
			object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(TaskId);
					if (objModel != null)
					{
						int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
						YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (YSWL.WeChat.Model.Push.TaskMsg)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.WeChat.Model.Push.TaskMsg> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<YSWL.WeChat.Model.Push.TaskMsg> DataTableToList(DataTable dt)
		{
			List<YSWL.WeChat.Model.Push.TaskMsg> modelList = new List<YSWL.WeChat.Model.Push.TaskMsg>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				YSWL.WeChat.Model.Push.TaskMsg model;
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
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
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
        /// 添加任务消息
        /// </summary>
        /// <param name="msgModel"></param>
        /// <returns></returns>
        public int AddEx(YSWL.WeChat.Model.Push.TaskMsg msgModel)
        {
            int  msgId = Add(msgModel);

            if (msgModel.MsgType == "news")
            {
                YSWL.WeChat.BLL.Core.PostMsgItem postItemBll = new PostMsgItem();
                YSWL.WeChat.Model.Core.PostMsgItem model = null;
                foreach (var item in msgModel.MsgItems)
                {
                    model = new Model.Core.PostMsgItem();
                    model.ItemId = item.ItemId;
                    model.PostMsgId = Common.Globals.SafeInt(msgId, 0);
                    model.Type = 2;
                    postItemBll.Add(model);
                }
            }
            return msgId;
        }
        /// <summary>
        /// 获取推送消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public  YSWL.WeChat.Model.Push.TaskMsg GetMsg(string openId, string userName)
        {
            YSWL.WeChat.Model.Core.User weUser = YSWL.WeChat.BLL.Core.User.GetUserInfo(openId, userName);
            //查找该用户的消息推送记录
            List<YSWL.WeChat.Model.Push.TaskLog> logs=YSWL.WeChat.BLL.Push.TaskLog.GetUserLogs(userName);
            var hasTasks = new List<int>();
            if (logs != null)
            {
                hasTasks = logs.Select(c => c.TaskId).ToList();
            }
            //查找该公众号的所有的任务消息
            List<YSWL.WeChat.Model.Push.TaskMsg> msgList = GetMsgList(openId);
            //过滤条件
           return  msgList.Where(c => !hasTasks.Contains(c.TaskId)).Where(c => (c.GroupId == 0) || (c.GroupId != 0 && c.GroupId == weUser.GroupId)).
                 Where(c => (String.IsNullOrWhiteSpace(c.UserName) || (c.UserName == userName))).FirstOrDefault();
        }

        public List<YSWL.WeChat.Model.Push.TaskMsg> GetMsgList(string openId)
        {
            DataSet ds=dal.GetMsgList(openId,DateTime.Now.ToString("yyyy-MM-dd"));
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获取任务推送消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static YSWL.WeChat.Model.Core.PostMsg GetPushMsg(string openId, string userName)
        {
            YSWL.WeChat.BLL.Push.TaskMsg taskBll=new TaskMsg();
            YSWL.WeChat.BLL.Core.MsgItem itemBll=new MsgItem();
            YSWL.WeChat.Model.Core.PostMsg postMsg = new Model.Core.PostMsg();
            YSWL.WeChat.Model.Push.TaskMsg taskMsg = taskBll.GetMsg(openId, userName);
            if (taskMsg != null)
            {
                postMsg.CreateTime = DateTime.Now;
                postMsg.OpenId = openId;
                postMsg.UserName = userName;
                postMsg.MsgType = taskMsg.MsgType;
                postMsg.Description = taskMsg.Description;
                postMsg.MediaId = taskMsg.MediaId;
                if (postMsg.MsgType == "news")
                {
                   List<YSWL.WeChat.Model.Core.MsgItem> MsgItems = itemBll.GetItemList(taskMsg.TaskId, 2);

                   postMsg.ArticleCount = MsgItems.Count;
                    YSWL.WeChat.Model.Core.MsgItem item = null;
                    for (int i = 0; i < MsgItems.Count; i++)
                    {
                        item = new Model.Core.MsgItem();
                        item.Title = MsgItems[i].Title;
                        item.Description = MsgItems[i].Description;
                        if (i == 0)
                        {
                            item.PicUrl = String.IsNullOrWhiteSpace(MsgItems[i].PicUrl) ? "" :
                                "http://" + Common.Globals.DomainFullName + String.Format(MsgItems[i].PicUrl, "N_");
                        }
                        else
                        {
                            item.PicUrl = String.IsNullOrWhiteSpace(MsgItems[i].PicUrl) ? "" :
                                                                  "http://" + Common.Globals.DomainFullName + String.Format(MsgItems[i].PicUrl, "T_");
                        }
                        item.Url = YSWL.WeChat.BLL.Core.Utils.GetWCUrl(openId,userName, MsgItems[i].Url);
                        postMsg.MsgItems.Add(item);
                    }
                  
                }
                YSWL.WeChat.BLL.Push.TaskLog.Add(taskMsg.TaskId, userName);
            }
            return postMsg;
        }

        public DataSet GetList(int top, string openId, string startdate, string enddate, string filedOrder)
        {
            return dal.GetList(top, openId, startdate, enddate, filedOrder);
        }
		#endregion  ExtensionMethod
	}
}

