﻿/**
* ShippingAddress.cs
*
* 功 能： N/A
* 类 名： ShippingAddress
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/4/27 10:24:44   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Shop.Shipping
{
	/// <summary>
	/// 接口层ShippingAddress
	/// </summary>
	public interface IShippingAddress
	{
		#region  成员方法
		/// <summary>
		/// 得到最大ID
		/// </summary>
		int GetMaxId();
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(int ShippingId);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		int Add(YSWL.MALL.Model.Shop.Shipping.ShippingAddress model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.MALL.Model.Shop.Shipping.ShippingAddress model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(int ShippingId);
		bool DeleteList(string ShippingIdlist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.MALL.Model.Shop.Shipping.ShippingAddress GetModel(int ShippingId);
		YSWL.MALL.Model.Shop.Shipping.ShippingAddress DataRowToModel(DataRow row);
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
	    DataSet GetAddressBySupplier(int supplierId);
        /// <summary>
        /// 设置默认收货地址
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ShippingId"></param>
        /// <returns></returns>
        bool SetDefaultShipAddress(int UserId, int ShippingId);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Shop.Shipping.ShippingAddress GetModelByUserId(int userId);

	    bool UpdateMapInfo(int userId, decimal latitude, decimal longitude, string address);
        /// <summary>
		/// 是否存在默认收货地址
		/// </summary>
		bool ExistsDefaultAddress(int ShippingId);
        #endregion  MethodEx
    } 
}