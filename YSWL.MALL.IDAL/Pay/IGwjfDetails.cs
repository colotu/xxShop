/**  版本信息模板在安装目录下，可自行修改。
* GwjfDetails.cs
*
* 功 能： N/A
* 类 名： GwjfDetails
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/9/3 14:45:12   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Pay
{
    /// <summary>
    /// 接口层IGwjfDetails
    /// </summary>
    public interface IGwjfDetails
    {
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(long JournalNumber);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		long Add(YSWL.MALL.Model.Pay.GwjfDetails model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.MALL.Model.Pay.GwjfDetails model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(long JournalNumber);
		bool DeleteList(string JournalNumberlist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.MALL.Model.Pay.GwjfDetails GetModel(long JournalNumber);
		YSWL.MALL.Model.Pay.GwjfDetails DataRowToModel(DataRow row);
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
	    /// 余额支付
	    /// </summary>
	    /// <param name="amount">支付金额</param>
	    /// <param name="userId">支付用户</param>
	    /// <param name="remark">日志信息</param>
	    bool Pay(decimal amount, int userId, string remark);
	} 
}
