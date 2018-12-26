using System;
using System.Collections.Generic;
using System.Data;
namespace YSWL.MALL.IDAL.Shop.Order
{
	/// <summary>
	/// 接口层Orders
	/// </summary>
	public interface IOrders
	{
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(long OrderId);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        long Add(YSWL.MALL.Model.Shop.Order.OrderInfo model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Shop.Order.OrderInfo model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(long OrderId);
        bool DeleteList(string OrderIdlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Shop.Order.OrderInfo GetModel(long OrderId);
        YSWL.MALL.Model.Shop.Order.OrderInfo DataRowToModel(DataRow row);
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
	    bool UpdateOrderStatus(long orderId, int status);

	    bool ReturnStatus(long orderId);

	    bool UpdateShipped(YSWL.MALL.Model.Shop.Order.OrderInfo orderModel);

	    /// <summary>
	    /// 根据条件获取对应的订单状态的数量
	    /// </summary>
	    /// <param name="userid">下单人 ID</param>
	    /// <param name="PaymentStatus">支付状态</param>
	    /// <param name="OrderStatusCancel">订单的取消状态</param>
	    /// <returns></returns>
	    int GetPaymentStatusCounts(int userid, int PaymentStatus, int OrderStatusCancel);

	    /// <summary>
	    /// 更新订单备注
	    /// </summary>
	    /// <param name="orderId"></param>
	    /// <param name="status"></param>
	    /// <returns></returns>
	    bool UpdateOrderRemark(long orderId, string Remark, string strWhere);

       YSWL.MALL.Model.Shop.Order.OrderInfo  GetOrderInfo(string ordercode);

       DataSet GetSalesStatis(string strWhere);

       Decimal GetOrderTotal(string strWhere);

        Decimal GetOrderDpxfjf(string strWhere);

        int GetOrderCount(string strWhere);

       DataSet GetMySalesStatis(string strWhere);

        int GetCustomCount(string strWhere);

        long PakingMainOrder(YSWL.MALL.Model.Shop.Order.OrderInfo mainOrder);

	    DataSet GetBrandList(int supplierId);
          /// <summary>
        /// 根据购买用户Id获取是否是首单 排除已取消订单 （返回  true 为首单  false  为否）
      /// </summary>
        /// <param name="buyerID">购买用户Id</param>
      /// <returns></returns>
        bool IsFirstOrder(int buyerID);


	    YSWL.MALL.Model.Shop.Order.OrderInfo GetOrderByCoupon(string coupon);

        bool UpdateOrderTypeSub(long orderId, int orderTypeSub);
          /// <summary>
        /// 是否允许修改当前订单的状态
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="ShippingStatus"></param>
        /// <param name="OrderStatus"></param>
        /// <returns></returns>
        bool IsAllowModify(long OrderId, int ShippingStatus, int OrderStatus);

        int GetUnPaidCounts(int userid);
        YSWL.MALL.Model.Shop.Order.OrderInfo GetModel(string orderCode);
        /// <summary>
        ///  根据日期获取订单数及总支付金额
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataSet GetOrderCountAmount(DateTime startDate, DateTime endDate);
        /// <summary>
        /// 按天统计订单金额
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataSet StatOrderAmount(DateTime startDate, DateTime endDate);
        /// <summary>
        ///   统计用户订单数及金额
        /// </summary>
        /// <param name="top"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataSet StatBuyerOrderCountAmount(int top, DateTime startDate, DateTime endDate);
        /// <summary>
        /// 获取未付款金额 (不排除货到付款)
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
         decimal GetUnPaidAmount(int userid);
        /// <summary>
        /// 获取已付款金额 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
         decimal GetPaidAmount(int userid);
        #endregion  MethodEx

        #region pos统计接口

	    DataSet GetOrderCountAmountByUser(int userId, DateTime startDate, DateTime endDate, int referType);
	    decimal GetUnPaidAmountByUser(int userid, DateTime startDate, DateTime endDate, int referType);
	    decimal GetPaidAmountByUser(int userid, DateTime startDate, DateTime endDate, int referType);
	    DataSet GetPaymentByUser(int userid, DateTime startDate, DateTime endDate, int referType);
	    DataSet GetOrderItemByUser(int userId, DateTime startDate, DateTime endDate, int referType);

	    #endregion
	} 
}
