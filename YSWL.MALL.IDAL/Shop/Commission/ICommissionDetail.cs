/**  版本信息模板在安装目录下，可自行修改。
* CommissionDetail.cs
*
* 功 能： N/A
* 类 名： CommissionDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/2/26 16:51:35   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using YSWL.MALL.Model.Shop.Order;

namespace YSWL.MALL.IDAL.Shop.Commission
{
	/// <summary>
	/// 接口层CommissionDetail
	/// </summary>
	public interface ICommissionDetail
	{
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(long DetailId);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		long Add(YSWL.MALL.Model.Shop.Commission.CommissionDetail model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.MALL.Model.Shop.Commission.CommissionDetail model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(long DetailId);
		bool DeleteList(string DetailIdlist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.MALL.Model.Shop.Commission.CommissionDetail GetModel(long DetailId);
		YSWL.MALL.Model.Shop.Commission.CommissionDetail DataRowToModel(DataRow row);
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
	    bool AddDetail(YSWL.MALL.Model.Shop.Commission.CommissionDetail model);

	    decimal GetUserFees(int userId);

	   DataSet StatUserFee(DateTime startDate, DateTime endDate);

       DataSet StatProFee(DateTime startDate, DateTime endDate);

	    DataSet StatCommission(DateTime startDate, DateTime endDate, StatisticMode mode = StatisticMode.Day);
        DataSet StatRuleFee(DateTime startDate, DateTime endDate);
        /// <summary>
        /// 按佣金规则统计佣金商品数量
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataSet StatRulePro(DateTime startDate, DateTime endDate);


        /// <summary>
        /// 按商品统计
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="RuleLevel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataSet StatPro(int userId, int RuleLevel, DateTime startDate, DateTime endDate);
        /// <summary>
        /// 按商品统计
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="RuleLevel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataSet StatPro(int userId, int RuleLevel, DateTime startDate, DateTime endDate, int startIndex, int endIndex);

        int StatProRecordCount(int userId, int RuleLevel, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 根据条件获取总佣金
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="RuleLevel"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        decimal GetTotalFee(int userId, int RuleLevel, DateTime startDate, DateTime endDate);
      

            #region 盟友排行
            /// <summary>
            /// 盟友排行  佣金维度（分页）
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="startDate"></param>
            /// <param name="endDate"></param>
            /// <param name="startIndex"></param>
            /// <param name="endIndex"></param>
            /// <returns></returns>
            DataSet AllyRanking(int userId, DateTime startDate, DateTime endDate, int startIndex, int endIndex);
        /// <summary>
        /// 盟友排行总数   佣金维度
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        int AllyRankingRecordCount(int userId, DateTime startDate, DateTime endDate);
        #endregion
        /// <summary>
        /// 获取订单数和商品数  
        /// </summary> 
        /// <param name="userId"></param>
        /// <returns></returns>
        DataSet GetOrderCount(int userId);
        #endregion  MethodEx
    }
}
