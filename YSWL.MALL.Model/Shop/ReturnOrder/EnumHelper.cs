/** 
* EnumHelper.cs
*
* 功 能： N/A
* 类 名： ReturnOrderAction
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/9 10:15:35   N/A    初版
* 负责人    hhy
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
namespace YSWL.MALL.Model.Shop.ReturnOrder
{
    public static class EnumHelper
    {
        /// <summary>
        /// 申请状态   |-3拒绝 | -2锁定 |  -1取消申请  |   0未处理  |  1处理中| 2 完成|
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// 拒绝
            /// </summary>
            Refuse = -3,

            /// <summary>
            /// 锁定
            /// </summary>
            Lock = -2,

            /// <summary>
            /// 取消
            /// </summary>
            Cancel = -1,

            /// <summary>
            /// 未处理
            /// </summary>
            UnHandle = 0,
            /// <summary>
            /// 处理中
            /// </summary>
            Handling = 1,

            /// <summary>
            /// 已完成
            /// </summary>
            Complete = 2

        }

        /// <summary>
        /// 退款状态 0 未退款 | 1 申请退款 | 2 退款中 | 3 已退款 | 4 拒绝退款
        /// </summary>
        public enum RefundStatus
        {
             /// <summary>
            /// 未退款
            /// </summary>
            UnRefund = 0,
            /// <summary>
            /// 申请退款
            /// </summary>
            Apply = 1,

            /// <summary>
            /// 退款中
            /// </summary>
            Refunding = 2,

            /// <summary>
            /// 已退款
            /// </summary>
            Refunds = 3,
            
            /// <summary>
            /// 拒绝
            /// </summary>
            Refuse = 4
        }

     
        
        /// <summary>
        /// 物流状态      //0 未取货  |    1 未打印   |   2 取货中  |   3 返程中  |  4 已入库  配送状态 0 未发货 | 1 打包中 | 2 已发货 | 3 已确认收货 | 4 拒收退货中 | 5 拒收已退货
        /// </summary>
        public enum LogisticStatus
        {
            /// <summary>
            /// 未取货
            /// </summary>
            UnPick = 0,
            /// <summary>
            /// 未打印
            /// </summary>
            UnPrint=1,//Packing = 1,
            
            /// <summary>
            /// 取货中
            /// </summary>
            Packing=2,//Shipped = 2,

            /// <summary>
            /// 返程中
            /// </summary>
            Returning= 3,

            /// <summary>
            /// 已入库
            /// </summary>
            Storaged = 4
        }

        /// <summary>
        /// 获取组合状态
        /// </summary>
        public enum MainStatus 
        {
            /// <summary>
            ///  等待审核
            /// </summary>
            Auditing = 1,
            /// <summary>
            ///  取消申请
            /// </summary>
            Cancel = 2,
            /// <summary>
            /// 拒绝
            /// </summary>
            Refuse = 3,
            /// <summary>
            ///  正在处理
            /// </summary>
            Handling = 4,
            /// <summary>
            ///  取货中
            /// </summary>
            Packing = 5,
            /// <summary>
            ///返程中
            /// </summary>
            Returning= 6,
            /// <summary>
            ///等待退款
            /// </summary>
            WaitingRefund = 7,
            /// <summary>
            /// 退款中
            /// </summary>
            Refunding = 8,
            /// <summary>
            /// 已完成
            /// </summary>
            Complete = 9
        }

        /// <summary>
        /// 退/换单操作名   客户创建退/换单  100 |  客户取消  101  | 
        ///   系统取消  200 | 系统完成
        /// </summary>
        public enum ActionCode
        { 
            #region 客户
            /// <summary>
            /// 客户创建退/换单
            /// </summary>
            CustomersCreate= 100,
            /// <summary>
            /// 客户取消
            /// </summary>
            CustomersCancel = 101,
            #endregion
           
            #region 系统
            /// <summary>
            /// 系统创建退/换单
            /// </summary>
            SystemCreate = 200,
            /// <summary>
            /// 系统取消
            /// </summary>
            SystemCancel = 201,
            /// <summary>
            /// 系统修改应退金额
            /// </summary>
            SystemUpdateAmountAdjusted = 202,
            /// <summary>
            /// 系统审核
            /// </summary>
            SystemAudit = 203,
           /// <summary>
            /// 系统修改取货信息
            /// </summary>
            SystemUpdatePick = 204,            
             /// <summary>
            /// 系统确认取货
            /// </summary>
            SystemPicked = 205,
            /// <summary>
            /// 系统修改实退金额
            /// </summary>
            SystemUpdateAmountActual = 206,
             /// <summary>
            /// 系统返还优惠劵
            /// </summary>
            SystemReturnCoupon = 207,
            /// <summary>
            /// 系统完成(确认退款)
            /// </summary>
            SystemComplete = 208,
            #endregion


            #region 商家
            /// <summary>
            /// 商家创建退/换单   （预留）
            /// </summary>
            SellerCreate = 300,
            /// <summary>
            /// 商家取消       
            /// </summary>
            SellerCancel = 301,
            /// <summary>
            /// 商家修改应退金额
            /// </summary>
            SellerUpdateAmountAdjusted = 302,
            /// <summary>
            /// 商家审核
            /// </summary>
            SellerAudit = 303,
            /// <summary>
            /// 商家修改取货信息
            /// </summary>
            SellerUpdatePick = 304,
            /// <summary>
            /// 商家确认取货
            /// </summary>
            SellerPicked = 305,
            /// <summary>
            /// 商家修改实退金额
            /// </summary>
            SellerUpdateAmountActual = 306,
            /// <summary>
            /// 商家返还优惠劵
            /// </summary>
            SellerReturnCoupon = 307,
            /// <summary>
            /// 商家完成(确认退款)
            /// </summary>
            SellerComplete = 308
            #endregion


        }

        /// <summary>
        /// 退货类型
        /// </summary>
        public enum ReturngoodsType
        { 
            /// <summary>
            ///整单退
            /// </summary>
            All= 1,
            /// <summary>
            /// 部分退
            /// </summary>
            Part = 2  
        }
        


    }
}
