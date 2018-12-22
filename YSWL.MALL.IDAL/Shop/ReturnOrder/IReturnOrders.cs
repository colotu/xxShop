/**  版本信息模板在安装目录下，可自行修改。
* ReturnOrders.cs
*
* 功 能： N/A
* 类 名： ReturnOrders
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/2 11:50:36   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
namespace YSWL.MALL.IDAL.Shop.ReturnOrder
{
	/// <summary>
	/// 接口层ReturnOrders
	/// </summary>
	public interface IReturnOrders
	{
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		bool Exists(long ReturnOrderId);
		/// <summary>
		/// 增加一条数据
		/// </summary>
		long Add(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model);
		/// <summary>
		/// 更新一条数据
		/// </summary>
		bool Update(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model);
		/// <summary>
		/// 删除一条数据
		/// </summary>
		bool Delete(long ReturnOrderId);
		bool DeleteList(string ReturnOrderIdlist );
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders GetModel(long ReturnOrderId);
		YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders DataRowToModel(DataRow row);
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
        /// <summary>
        /// 根据订单号获取退货记录总数 (不包含已取消的记录)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        int GetCountByOrderId(long orderId);

        long CreateReturnOrder(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders returnOrders, Accounts.Bus.User currentUser);
        /// <summary>
        /// 审核通过 修改数据
     /// </summary>
     /// <param name="model"></param>
     /// <param name="IsReturnGoods">是否需要退货</param>
        /// <param name="orderInfo">原订单</param>
     /// <returns></returns>
        bool AuditPass(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model, bool IsReturnGoods, Model.Shop.Order.OrderInfo orderInfo);
        /// <summary>
        /// 审核未通过
        /// </summary>
        /// <param name="ReturnOrderId"></param>
        /// <param name="refuseReason">原因</param>
        /// <param name="UpdateUserId"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        bool AuditFail(long ReturnOrderId, string refuseReason, int UpdateUserId, string Remark);
        /// <summary>
        /// 取消退单申请
        /// </summary>
        /// <param name="ReturnOrderId"></param>
        /// <param name="ReturnOrderCode"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        bool CancelReturnOrder(long ReturnOrderId, string ReturnOrderCode, int  userId);

        bool PackedOrder(Model.Shop.ReturnOrder.ReturnOrders Info, Accounts.Bus.User currentUser);

        /// <summary>
        ///  完成退款
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUser">当前用户</param>
        /// <param name="IsReturnCoupon">是否退优惠劵</param>
        /// <param name="suppUserId">商家的用户Id</param>
        /// <param name="deductionSuppAmount">扣除商家的金额</param>
        /// <returns></returns>
        bool Refunds(Model.Shop.ReturnOrder.ReturnOrders model, Accounts.Bus.User currentUser, bool IsReturnCoupon, int suppUserId, decimal deductionSuppAmount);

          /// <summary>
        ///  修改应退金额
     /// </summary>
     /// <param name="ReturnOrderId"></param>
     /// <param name="ReturnOrderCode"></param>
     /// <param name="oldAmountAdjusted"></param>
     /// <param name="newAmountAdjusted"></param>
     /// <param name="currentUser"></param>
     /// <returns></returns>
        bool UpdateAmountAdjusted(long ReturnOrderId, string ReturnOrderCode, decimal oldAmountAdjusted, decimal newAmountAdjusted, Accounts.Bus.User currentUser);
      /// <summary>
        /// 更新备注
        /// </summary>
        /// <param name="ReturnOrderId"></param>
        /// <param name="Remark"></param>
        /// <param name="SupplierId"></param>
        /// <returns></returns>
        bool UpdateRemark(long returnOrderId, string Remark, int SupplierId);
       
        #endregion  MethodEx

	   

	} 
}
