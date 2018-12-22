﻿/**
* Agents.cs
*
* 功 能： N/A
* 类 名： Agents
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/11/28 18:17:12   Ben    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Ms.Agent
{
	/// <summary>
	/// 接口层代理商
	/// </summary>
	public interface IAgents
	{
		#region  成员方法
		/// <summary>
		/// 得到最大ID
		/// </summary>
		int GetMaxId();
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(int AgentId);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		int Add(YSWL.MALL.Model.Ms.Agent.AgentInfo model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.MALL.Model.Ms.Agent.AgentInfo model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(int AgentId);
		bool DeleteList(string AgentIdlist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.MALL.Model.Ms.Agent.AgentInfo GetModel(int AgentId);
		YSWL.MALL.Model.Ms.Agent.AgentInfo DataRowToModel(DataRow row);
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

		#endregion  MethodEx

	    /// <summary>
	    /// 代理商名称是否已存在
	    /// </summary>
	    bool Exists(string Name, int id = 0);

	    /// <summary>
	    /// 得到一个对象实体
	    /// </summary>
	    YSWL.MALL.Model.Ms.Agent.AgentInfo GetModelByUserId(int UserId);

	    /// <summary>
	    /// 店铺名称是否已存在
	    /// </summary>
	    bool ExistsShopName(string Name);

	    /// <summary>
	    /// 店铺名称是否已存在
	    /// </summary>
	    bool ExistsShopName(string Name, int AgentID);

	    /// <summary>
	    /// 批量处理状态
	    /// </summary>
	    /// <param name="IDlist"></param>
	    /// <param name="strWhere"></param>
	    /// <returns></returns>
	    bool UpdateList(string IDlist, string strWhere);

	    /// <summary>
	    /// 关闭店铺 
	    /// </summary>
	    bool CloseShop(int AgentID);

	    /// <summary>
	    /// 根据店铺名称得到店铺model
	    /// </summary>
	    /// <param name="ShopName"></param>
	    /// <returns></returns>
	    YSWL.MALL.Model.Ms.Agent.AgentInfo GetModelByShopName(string ShopName);
	} 
}
