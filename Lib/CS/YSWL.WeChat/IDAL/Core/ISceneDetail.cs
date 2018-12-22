/**  版本信息模板在安装目录下，可自行修改。
* SceneDetail.cs
*
* 功 能： N/A
* 类 名： SceneDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/2/20 12:32:25   N/A    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.WeChat.IDAL.Core
{
	/// <summary>
	/// 接口层SceneDetail
	/// </summary>
	public interface ISceneDetail
	{
		#region  成员方法
		/// <summary>
		/// 得到最大ID
		/// </summary>
		int GetMaxId();
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(int DetailId);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		int Add(YSWL.WeChat.Model.Core.SceneDetail model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.WeChat.Model.Core.SceneDetail model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(int DetailId);
		bool DeleteList(string DetailIdlist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.WeChat.Model.Core.SceneDetail GetModel(int DetailId);
		YSWL.WeChat.Model.Core.SceneDetail DataRowToModel(DataRow row);
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
        DataSet GetList(int top, int sceneId, string startdate, string enddate, string filedOrder);

	    YSWL.WeChat.Model.Core.SceneDetail GetSceneDetail(string openId, string userOpen);

	    bool IsExist(int sceneId, string openId, string userOpen);
        /// <summary>
        /// 推广渠道统计
        /// </summary>
        DataSet GetList(string openId, DateTime startDate, DateTime endDate);
        #endregion  MethodEx

    } 
}
