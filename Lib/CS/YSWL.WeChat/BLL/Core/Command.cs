/**
* Command.cs
*
* 功 能： N/A
* 类 名： Command
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/7/29 15:35:22   N/A    初版
*
* Copyright (c) 2012-2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using YSWL.Common;
using YSWL.WeChat.Model.Core;
using YSWL.WeChat.IDAL.Core;
using System.Linq;

namespace YSWL.WeChat.BLL.Core
{
	/// <summary>
	/// Command
	/// </summary>
	public partial class Command
	{
        private readonly ICommand dal = YSWL.DBUtility.PubConstant.IsSQLServer ? (ICommand)new YSWL.WeChat.SQLServerDAL.Core.Command() : (ICommand)new YSWL.WeChat.MySqlDAL.Core.Command();//暂时预留
		public Command()
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
        public bool Exists(int CommandId)
        {
            return dal.Exists(CommandId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Core.Command model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.WeChat.Model.Core.Command model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int CommandId)
        {

            return dal.Delete(CommandId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string CommandIdlist)
        {
            return dal.DeleteList(CommandIdlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.WeChat.Model.Core.Command GetModel(int CommandId)
        {

            return dal.GetModel(CommandId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.WeChat.Model.Core.Command GetModelByCache(int CommandId)
        {

            string CacheKey = "CommandModel-" + CommandId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(CommandId);
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.WeChat.Model.Core.Command)objModel;
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.WeChat.Model.Core.Command> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.WeChat.Model.Core.Command> DataTableToList(DataTable dt)
        {
            List<YSWL.WeChat.Model.Core.Command> modelList = new List<YSWL.WeChat.Model.Core.Command>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.WeChat.Model.Core.Command model;
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
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
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
        /// 获取指令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static  string GetComName(YSWL.WeChat.Model.Core.Command command, string value)
        {
            string name = "";
            switch (command.ParseType)
            {
                    //按长度处理
                case 0:
                    name = value.Substring(0, command.ParseLength);
                    break;
                case 1:
                    name = value.Split(Convert.ToChar(command.ParseChar))[0];
                    break;
            }
            return name;
        }
        /// <summary>
        /// 获取关键字
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetKeyWord(YSWL.WeChat.Model.Core.Command command, string value)
        {
            string name = "";
            if (command != null && command.CommandId > 0 && !String.IsNullOrWhiteSpace(value))
            {
                switch (command.ParseType)
                {
                    //按长度处理
                    case 0:
                        name = value.Substring(command.ParseLength);
                        break;
                    case 1:
                        name = value.Split(Convert.ToChar(command.ParseChar))[1];
                        break;
                }
            }
       
            return name;
        }
	    /// <summary>
        /// 匹配指令
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static YSWL.WeChat.Model.Core.Command MatchCommand(YSWL.WeChat.Model.Core.RequestMsg msgModel)
	    {
            //如果不是文本消息，就不需要走指令处理
	        if (msgModel.MsgType != "text")
	        {
	            return null;
	        }
	        string CacheKey = "MatchCommand-" + msgModel.Description + msgModel.MsgType;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    //获取所有指令
                     List<YSWL.WeChat.Model.Core.Command> AllCommand = YSWL.WeChat.BLL.Core.Command.GetAllCommand(msgModel.OpenId);
                    if (AllCommand != null && AllCommand.Count > 0)
                    {
                        AllCommand = AllCommand.OrderBy(c => c.Sequence).ToList();
                        foreach (var command in AllCommand)
                        {
                            if (msgModel.Description.ToLower().StartsWith(command.Name.ToLower()))
                            {
                                objModel = command;
                                break;
                            }
                        }
                    }
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.WeChat.Model.Core.Command)objModel;
	    }

        /// <summary>
        /// 获取最大顺序值
        /// </summary>
        /// <returns></returns>
	    public int GetSequence(string openId)
	    {
            return dal.GetSequence(openId);
	    }
        /// <summary>
        /// 获取所有可用的指令
        /// </summary>
        /// <returns></returns>
	    public static List<YSWL.WeChat.Model.Core.Command> GetAllCommand(string openId)
	    {
            string CacheKey = "GetAllCommand"+openId ;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    YSWL.WeChat.BLL.Core.Command commandBll = new Command();
                    objModel = commandBll.GetModelList(" Status=1 and OpenId='" + Common.InjectionFilter.SqlFilter(openId)+"'");
                    if (objModel != null)
                    {
                        int ModelCache = YSWL.Common.ConfigHelper.GetConfigInt("ModelCache");
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (List<YSWL.WeChat.Model.Core.Command>)objModel;
	    }

        public static string GetCommandStr(string openId)
	    {
	        List<YSWL.WeChat.Model.Core.Command> commandList = GetAllCommand(openId);
            StringBuilder commandStr = new StringBuilder();
            if (commandList != null && commandList.Count>0)
	        {
	            foreach (var command in commandList)
	            {
	                commandStr.Append(command.Name + "  " + command.Remark).Append("\n");
	            }
	        }
	        return commandStr.ToString();
	    }


        public List<YSWL.WeChat.Model.Core.Command> GetCommandList(string openId, int status, string keyword, int startIndex, int endIndex)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" OpenId='{0}'", Common.InjectionFilter.SqlFilter(openId));
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                strWhere.AppendFormat(" and  Name like '%{0}%'", Common.InjectionFilter.SqlFilter(keyword));
            }
            if (status != -1)
            {
                strWhere.AppendFormat(" and  Status ={0}", status);
            }
            DataSet ds = GetListByPage(strWhere.ToString(), "", startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        public int GetCount(string openId, int status, string keyword)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" OpenId='{0}'", Common.InjectionFilter.SqlFilter(openId));
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                strWhere.AppendFormat(" and  Name like '%{0}%'", Common.InjectionFilter.SqlFilter(keyword));
            }
            if (status != -1)
            {
                strWhere.AppendFormat(" and  Status ={0}", status);
            }

            return GetRecordCount(strWhere.ToString());
        }

	    #endregion  ExtensionMethod
	}
}

