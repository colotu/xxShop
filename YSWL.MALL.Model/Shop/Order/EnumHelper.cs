
using System.ComponentModel;
namespace YSWL.MALL.Model.Shop.Order
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取的订单状态的类型
        /// </summary>
        /// <remarks>订单组合状态 1 等待付款   | 2 等待处理 | 3 取消订单 | 4 订单锁定 | 5 等待付款确认 | 6 正在处理 |7 配货中  |8 已发货 |9  已完成 </remarks>
        public enum OrderMainStatus
        {
            None = -1,
            /// <summary>
            /// 等待付款
            /// </summary>
            Paying = 1,

            /// <summary>
            /// 等待处理
            /// </summary>
            PreHandle = 2,

            /// <summary>
            /// 取消订单
            /// </summary>
            Cancel = 3,

            /// <summary>
            /// 订单锁定
            /// </summary>
            Locking = 4,

            /// <summary>
            /// 等待付款确认
            /// </summary>
            PreConfirm = 5,

            /// <summary>
            /// 正在处理
            /// </summary>
            Handling = 6,

            /// <summary>
            /// 配货中
            /// </summary>
            Shipping = 7,

            /// <summary>
            /// 已发货
            /// </summary>
            Shiped = 8,
            /// <summary>
            /// 已完成
            /// </summary>
            Complete = 9
        }
        /// <summary>
        /// 获取的配送状态的类型    //  配送状态 0 未发货 | 1 打包中 | 2 已发货 | 3 已确认收货 | 4 拒收退货中 | 5 拒收已退货
        /// </summary>
        public enum ShippingStatus
        {
            None = -1,
            /// <summary>
            /// 未发货
            /// </summary>
            UnShipped = 0,
            /// <summary>
            /// 打包中
            /// </summary>
            Packing = 1,

            /// <summary>
            /// 已发货
            /// </summary>
            Shipped = 2,

            /// <summary>
            /// 已确认收货
            /// </summary>
            ConfirmShip = 3,

            /// <summary>
            /// 拒收退货中
            /// </summary>
            RejectedReturning = 4,

            /// <summary>
            /// 拒收已退货
            /// </summary>
            RejectedReturned = 5

        }

        /// <summary>
        /// 支付状态    //  支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中 | 4 支付异常 
        /// </summary>
        public enum PaymentStatus
        {
            None = -1,
            /// <summary>
            /// 未支付
            /// </summary>
            Unpaid = 0,
            /// <summary>
            /// 等待确认
            /// </summary>
            PreConfirm = 1,

            /// <summary>
            /// 已支付
            /// </summary>
            Paid = 2,

            /// <summary>
            /// 处理中
            /// </summary>
            Handling = 3,

            /// <summary>
            /// 支付异常
            /// </summary>
            PayException = 4

        }

        /// <summary>
        /// 订单状态    -4 系统锁定   | -3 后台锁定 | -2 用户锁定 | -1 死单（取消） | 0 未处理 | 1 进行中 |2 已完成 
        /// </summary>
        public enum OrderStatus
        {
            /// <summary>
            /// 系统锁定
            /// </summary>
            SystemLock = -4,
            /// <summary>
            /// 后台锁定
            /// </summary>
            AdminLock = -3,
            /// <summary>
            /// 用户锁定
            /// </summary>
            UserLock = -2,
            /// <summary>
            /// 死单
            /// </summary>
            Cancel = -1,
            /// <summary>
            /// 未处理
            /// </summary>
            UnHandle = 0,
            /// <summary>
            /// 进行中
            /// </summary>
            Handling = 1,
            /// <summary>
            /// 已完成
            /// </summary>
            Complete = 2

        }
        /// <summary>
        /// 网关类型
        /// </summary>
        public enum PaymentGateway
        {
            /// <summary>
            /// 货到付款
            /// </summary>
            cod,
            /// <summary>
            /// 银行汇款
            /// </summary>
            bank,
            /// <summary>
            /// 其他（在线支付）
            /// </summary>
            other
        }
        /// <summary>
        /// 订单操作名   客户创建订单  100 |系统取消订单  101  |系统支付订单  102 |
        ///  系统配货操作  103 |系统发货操作  104 | 系统完成订单  105  | 系统修改收货信息  106 | 系统变更应付金额 107
        /// 商家取消订单  110 | 商家配货操作  111 | 商家发货操作  112 | 商家完成订单  113 | 商家修改收货信息  114
        ///| 客户取消订单  120 | 客户支付订单  121  | 客户完成订单  122
        /// </summary>
        public enum ActionCode
        {
            #region 新的

            /// <summary>
            /// 创建订单
            /// </summary>
            Create = 10000,
            /// <summary>
            /// 系统同步创建
            /// </summary>
            SysCreate = 10001,
            /// <summary>
            /// 后台客服代下单
            /// </summary>
            AdminCreate = 10002,
            /// <summary>
            /// 取消订单
            /// </summary>
            SystemCancel = 11000,
            /// <summary>
            /// 缺货取消
            /// </summary>
            CancelNoStoct = 11001,
            /// <summary>
            /// 商家取消订单
            /// </summary>
            SellerCancel = 11002,
            /// <summary>
            /// 客户取消订单
            /// </summary>
            CustomersCancel = 11003,
            /// <summary>
            /// 代理商取消订单
            /// </summary>
            AgentCancel = 11004,
            /// <summary>
            /// 支付订单
            /// </summary>
            SystemPay = 12000,
            /// <summary>
            /// 客户支付订单
            /// </summary>
            CustomersPay = 12001,
            /// <summary>
            /// 变更应付金额
            /// </summary>
            UpdateAmount = 13100,
            /// <summary>
            /// 系统更改应付金额
            /// </summary>
            SysUpdateAmount = 13101,
            /// <summary>
            /// 修改收货信息
            /// </summary>
            UpdateShip = 13200,
            /// <summary>
            /// 系统更改收货地址
            /// </summary>
            SysUpdateShip = 13201,
            /// <summary>
            /// 商家更改收货地址
            /// </summary>
            SellerUpdateShip = 13202,
            /// <summary>
            /// 代理商更改收货地址
            /// </summary>
            AgentUpdateShip = 13203,
            /// <summary>
            /// 系统更改订单备注
            /// </summary>
            SysUpdateRemark = 13301,
            /// <summary>
            /// 审核订单
            /// </summary>
            Audited = 14000,

            /// <summary>
            /// 准备出库
            /// </summary>
            ReadyOutbound = 20000,
            /// <summary>
            /// 正在分拣
            /// </summary>
            Sorting = 21000,
            /// <summary>
            /// 打印操作
            /// </summary>
            Printed = 22000,
            /// <summary>
            /// 配货操作
            /// </summary>
            SystemPacking = 23000,
            /// <summary>
            /// 商家配货操作
            /// </summary>
            SellerPacking = 23001,
            /// <summary>
            /// 代理商配货操作
            /// </summary>
            AgentPacking = 23002,
            /// <summary>
            /// 发货操作
            /// </summary>
            Shipped = 24000,

            /// <summary>
            /// 商家发货操作
            /// </summary>
            SellerShipped = 24001,
            /// <summary>
            /// 代理商发货操作
            /// </summary>
            AgentShipped = 24002,
            /// <summary>
            /// 配送站验货完成
            /// </summary>
            InspectionComplete = 30000,
            /// <summary>
            /// 配送员正在配送
            /// </summary>
            Shipping = 31000,
            /// <summary>
            /// 延迟送货
            /// </summary>
            DelayShipping = 32000,
            /// <summary>
            /// 完成订单
            /// </summary>
            Complete = 50000,
            /// <summary>
            /// 系统完成
            /// </summary>
            SysComplete = 50001,
            /// <summary>
            /// 商家完成
            /// </summary>
            SellerComplete = 50002,
            /// <summary>
            /// 代理商完成
            /// </summary>
            AgentComplete = 50003,

            /// <summary>
            /// 全部回库   (取消)
            /// </summary>
            AllReturn = 51000,
            /// <summary>
            /// 部分回库  (完成)
            /// </summary>
            PartReturn = 52000
            #endregion
        }

        /// <summary>
        /// 获取商家的订单状态
        /// </summary>
        /// <remarks>订单组合状态 1 等待处理  | 2 未完成 | 3 已完成 </remarks>
        public enum StoreOrderStatus
        {
            /// <summary>
            /// 等待处理
            /// </summary>
            PreHandle = 1,

            /// <summary>
            /// 未完成
            /// </summary>
            NotComplete = 2,

            /// <summary>
            /// 已完成
            /// </summary>
            Complete = 3
        }

       // P：表示PC下单，W：表示微信下单，S：表示业务员代下单，C：表示客服代下单 D:订货APP
        public enum ReferType
        {
            None = -1,
            /// <summary>
            /// PC下单
            /// </summary>
            PC = 1,
            /// <summary>
            /// 微信C端下单
            /// </summary>
            WeChat = 2,
            /// <summary>
            ///业务员代替下单
            /// </summary>
           SalesMan=3,
            /// <summary>
            /// 客服代下单
            /// </summary>
            Cust = 4,
            /// <summary>
            /// 订货APP
            /// </summary>
            Ding = 5,
            /// <summary>
            /// Pos下单
            /// </summary>
            Pos=6,
            /// <summary>
            /// 微信B端下单
            /// </summary>
            WeChatB = 7,
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
    }

    /// <summary>
    /// 统计模式
    /// </summary>
    public enum StatisticMode
    {
        /// <summary>
        /// 按天统计
        /// </summary>
        Day = 0,

        /// <summary>
        /// 按月统计
        /// </summary>
        Month = 1,

        /// <summary>
        /// 按年统计
        /// </summary>
        Year = 2
    }


}
