﻿using System;
using System.Data;
namespace YSWL.MALL.IDAL.Shop.Order
{
	/// <summary>
	/// 接口层OrderOptions
	/// </summary>
	public interface IOrderOptions
	{
        #region  成员方法
        /// <summary>
        /// 得到最大ID
        /// </summary>
        int GetMaxId();
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(int LookupListId, int LookupItemId, long OrderId);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        bool Add(YSWL.MALL.Model.Shop.Order.OrderOptions model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Shop.Order.OrderOptions model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(int LookupListId, int LookupItemId, long OrderId);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Shop.Order.OrderOptions GetModel(int LookupListId, int LookupItemId, long OrderId);
        YSWL.MALL.Model.Shop.Order.OrderOptions DataRowToModel(DataRow row);
        /// <summary>
        /// 获得数据列表
        /// </summary>
        DataSet GetList(string strWhere);
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        DataSet GetList(int Top, string strWhere, string filedOrder);
        int GetRecordCount(string strWhere);
        DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex);
        /// <summary>
        /// 根据分页获得数据列表
        /// </summary>
        //DataSet GetList(int PageSize,int PageIndex,string strWhere);
        #endregion  成员方法
		#region  MethodEx

	    DataSet Get2ListByOrderId(long orederId);

	    #endregion  MethodEx
	} 
}
