/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：IAdvertisePosition.cs
// 文件功能描述：
// 
// 创建标识： [Ben]  2012/05/31 13:22:19
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Settings
{
	/// <summary>
	/// 接口层AdvertisePosition
	/// </summary>
	public interface IAdvertisePosition
	{
        #region  成员方法		
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int AdvPositionId);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		int Add(YSWL.MALL.Model.Settings.AdvertisePosition model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.MALL.Model.Settings.AdvertisePosition model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(int AdvPositionId);
		bool DeleteList(string AdvPositionIdlist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.MALL.Model.Settings.AdvertisePosition GetModel(int AdvPositionId);
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
        /// 编辑广告位方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
	    bool UpdateAdvPostion(ViewModel.AdvertisePositionVm model);
	    /// <summary>
	    /// 根据分页获得数据列表
	    /// </summary>
	    //DataSet GetList(int PageSize,int PageIndex,string strWhere);

	    #endregion  成员方法
	} 
}
