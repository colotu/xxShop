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
namespace YSWL.WeChat.IDAL.Push
{
	/// <summary>
	/// 接口层TaskMsg
	/// </summary>
	public interface ITaskMsg
	{
		#region  成员方法
		/// <summary>
		/// 得到最大ID
		/// </summary>
		int GetMaxId();
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(int TaskId);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		int Add(YSWL.WeChat.Model.Push.TaskMsg model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.WeChat.Model.Push.TaskMsg model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(int TaskId);
		bool DeleteList(string TaskIdlist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.WeChat.Model.Push.TaskMsg GetModel(int TaskId);
		YSWL.WeChat.Model.Push.TaskMsg DataRowToModel(DataRow row);
		/// <summary>
		/// 获得数据列表
		/// </summary>
		DataSet GetList(string strWhere);
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		DataSet GetList(int Top,string strWhere,string filedOrder);
		int GetRecordCount(string strWhere);
		DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex);
		/// <summary>
		/// 根据分页获得数据列表
		/// </summary>
		//DataSet GetList(int PageSize,int PageIndex,string strWhere);
		#endregion  成员方法
		#region  MethodEx
        DataSet GetMsgList(string openid, string datetime);

	    DataSet GetList(int top, string openId, string startdate, string enddate, string filedOrder);

	    #endregion  MethodEx
    } 
}
