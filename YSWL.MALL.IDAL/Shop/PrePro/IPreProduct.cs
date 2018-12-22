/**  版本信息模板在安装目录下，可自行修改。
* PreProduct.cs
*
* 功 能： N/A
* 类 名： PreProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/8/24 16:08:40   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Shop.PrePro
{
	/// <summary>
	/// 接口层预定
	/// </summary>
	public interface IPreProduct
	{
		#region  成员方法
		/// <summary>
		/// 得到最大ID
		/// </summary>
		int GetMaxId();
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(int PreProId);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		int Add(YSWL.MALL.Model.Shop.PrePro.PreProduct model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.MALL.Model.Shop.PrePro.PreProduct model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(int PreProId);
		bool DeleteList(string PreProIdlist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.MALL.Model.Shop.PrePro.PreProduct GetModel(int PreProId);
		YSWL.MALL.Model.Shop.PrePro.PreProduct DataRowToModel(DataRow row);
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
        bool UpdateStatus(string ids,int status);

	    bool IsExists(long productId);

	    int GetTotalCount();

	    DataSet GetListByPage(int cid, string orderby, int startIndex, int endIndex);
        YSWL.MALL.Model.Shop.PrePro.PreProduct DataRowToModelEx(DataRow row);

	    YSWL.MALL.Model.Shop.PrePro.PreProduct GetModelInfo(long productId);

	    #endregion  MethodEx
	} 
}
