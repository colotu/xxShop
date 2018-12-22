/**  版本信息模板在安装目录下，可自行修改。
* SuppDistProduct.cs
*
* 功 能： N/A
* 类 名： SuppDistProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/27 17:36:25   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Shop.Distribution
{
	/// <summary>
	/// 接口层SuppDistProduct
	/// </summary>
	public interface ISuppDistProduct
	{
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
        bool Exists(int supplierId, long ProductId);
		/// <summary>
		/// 增加一条数据
		/// </summary>
        bool Add(int supplierId, YSWL.MALL.Model.Shop.Distribution.SuppDistProduct model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
        bool Update(int supplierId, YSWL.MALL.Model.Shop.Distribution.SuppDistProduct model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
        bool Delete(int supplierId, long ProductId);
        bool DeleteList(int supplierId, string ProductIdlist);
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        YSWL.MALL.Model.Shop.Distribution.SuppDistProduct GetModel(int supplierId, long ProductId);
		YSWL.MALL.Model.Shop.Distribution.SuppDistProduct DataRowToModel(DataRow row);
		/// <summary>
		/// 获得数据列表
		/// </summary>
        DataSet GetList(int supplierId, string strWhere);
		/// <summary>
		/// 获得前几行数据
		/// </summary>
        DataSet GetList(int supplierId, int Top, string strWhere, string filedOrder);
        int GetRecordCount(int supplierId, string strWhere);
        DataSet GetListByPage(int supplierId, string strWhere, string orderby, int startIndex, int endIndex);
		/// <summary>
		/// 根据分页获得数据列表
		/// </summary>
		//DataSet GetList(int PageSize,int PageIndex,string strWhere);
		#endregion  成员方法
		#region  MethodEx

		#endregion  MethodEx
	} 
}
