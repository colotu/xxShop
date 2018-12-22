/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：IProductAttributes.cs
// 文件功能描述：
// 
// 创建标识： [Ben]  2012/06/11 20:36:25
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Shop.Products
{
	/// <summary>
	/// 接口层ProductAttribute
	/// </summary>
	public interface IProductAttribute
	{
		#region  成员方法
		/// <summary>
		/// 得到最大ID
		/// </summary>
		int GetMaxId();
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(long ProductId,long AttributeId,int ValueId);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		bool Add(YSWL.MALL.Model.Shop.Products.ProductAttribute model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.MALL.Model.Shop.Products.ProductAttribute model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(long ProductId,long AttributeId,int ValueId);
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.MALL.Model.Shop.Products.ProductAttribute GetModel(long ProductId,long AttributeId,int ValueId);
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

        bool Exists(long? ProductId, long? AttributeId, long? ValueId);
	} 
}
