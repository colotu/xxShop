using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Order;
namespace YSWL.MALL.BLL.Shop.Order
{
    /// <summary>
    /// Orders
    /// </summary>
    public partial class Orders
    {
        private readonly IOrders dal = DAShopOrder.CreateOrders();
        private readonly OrderItems orderItemsBll = new OrderItems();
        public Orders()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long OrderId)
        {
            return dal.Exists(OrderId);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.Order.OrderInfo model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Order.OrderInfo model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(long OrderId)
        {

            return dal.Delete(OrderId);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string OrderIdlist)
        {
            return dal.DeleteList(Common.Globals.SafeLongFilter(OrderIdlist, 0));
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Order.OrderInfo GetModel(long OrderId)
        {
            return dal.GetModel(OrderId);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public YSWL.MALL.Model.Shop.Order.OrderInfo GetModelByCache(long OrderId)
        {

            string CacheKey = "OrdersModel-" + OrderId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(OrderId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Order.OrderInfo)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Order.OrderInfo> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Order.OrderInfo> DataTableToList(DataTable dt)
        {
            List<YSWL.MALL.Model.Shop.Order.OrderInfo> modelList = new List<YSWL.MALL.Model.Shop.Order.OrderInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.Model.Shop.Order.OrderInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  BasicMethod

        #region  ExtensionMethod
        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateOrderStatus(long orderId, int status)
        {
            return dal.UpdateOrderStatus(orderId, status);
        }

        /// <summary>
        /// 退货操作
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool ReturnStatus(long orderId)
        {
            return dal.ReturnStatus(orderId);
        }

        /// <summary>
        /// 根据订单组合状态获取查询订单的条件
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public string GetWhereByStatus(YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus orderType)
        {
            string strWhere = "";
            switch (orderType)
            {
                //等待付款
                case YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Paying:
                    strWhere =
                        String.Format(
                            " OrderStatus={0}  and PaymentStatus={1} and ShippingStatus={2} and PaymentGateway!='{3}'",
                            (int)EnumHelper.OrderStatus.UnHandle, (int)EnumHelper.PaymentStatus.Unpaid,
                            (int)EnumHelper.ShippingStatus.UnShipped, EnumHelper.PaymentGateway.cod.ToString());
                    break;
                //等待处理
                case YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.PreHandle:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  and ShippingStatus={1}  and  ( (PaymentGateway='{2}'  and PaymentStatus={3} )  or   ( PaymentStatus={4}  and PaymentGateway!='{2}' )  )",
                          (int)EnumHelper.OrderStatus.UnHandle, (int)EnumHelper.ShippingStatus.UnShipped, EnumHelper.PaymentGateway.cod.ToString(), (int)EnumHelper.PaymentStatus.Unpaid,
                           (int)EnumHelper.PaymentStatus.Paid);
                    break;
                //取消订单
                case YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Cancel:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  and PaymentStatus={1} and ShippingStatus={2} ",
                          (int)EnumHelper.OrderStatus.Cancel, (int)EnumHelper.PaymentStatus.Unpaid,
                          (int)EnumHelper.ShippingStatus.UnShipped);
                    break;
                //订单锁定
                case YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Locking:
                    strWhere =
                      String.Format(
                          "  PaymentStatus={1} and ShippingStatus={2}  and (OrderStatus={0} or OrderStatus={3})  ",
                          (int)EnumHelper.OrderStatus.AdminLock, (int)EnumHelper.PaymentStatus.Unpaid,
                          (int)EnumHelper.ShippingStatus.UnShipped, (int)EnumHelper.OrderStatus.UserLock);
                    break;
                //等待付款确认
                case YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.PreConfirm:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  and PaymentStatus={1} and ShippingStatus={2} and PaymentGateway!='{3}'",
                          (int)EnumHelper.OrderStatus.UnHandle, (int)EnumHelper.PaymentStatus.Paid,
                          (int)EnumHelper.ShippingStatus.UnShipped, EnumHelper.PaymentGateway.bank.ToString());
                    break;
                //正在处理
                case YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Handling:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  and ShippingStatus={1} ",
                          (int)EnumHelper.OrderStatus.Handling,
                          (int)EnumHelper.ShippingStatus.UnShipped);
                    break;
                //配货中
                case YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Shipping:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  and  ShippingStatus={1}",
                          (int)EnumHelper.OrderStatus.Handling,
                          (int)EnumHelper.ShippingStatus.Packing);
                    break;
                //已发货
                case YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Shiped:
                    strWhere =
                     String.Format(
                         " OrderStatus={0}  and  ShippingStatus={1}",
                         (int)EnumHelper.OrderStatus.Handling,
                         (int)EnumHelper.ShippingStatus.Shipped);
                    break;
                //已完成
                case YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Complete:
                    strWhere =
                  String.Format(
                      " OrderStatus={0}  and  ShippingStatus={1}",
                      (int)EnumHelper.OrderStatus.Complete,
                      (int)EnumHelper.ShippingStatus.ConfirmShip);
                    break;
                default:
                    break;
            }
            return strWhere;
        }

        /// <summary>
        /// 根据订单组合状态获取查询订单的条件
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public string GetWhereByStatus(int orderType)
        {
            string strWhere = "";
            switch (orderType)
            {
                //等待付款
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Paying:
                    strWhere =
                        String.Format(
                            " OrderStatus={0}  and PaymentStatus={1} and ShippingStatus={2} and PaymentGateway!='{3}'",
                            (int)EnumHelper.OrderStatus.UnHandle, (int)(int)EnumHelper.PaymentStatus.Unpaid,
                            (int)(int)EnumHelper.ShippingStatus.UnShipped, EnumHelper.PaymentGateway.cod.ToString());
                    break;
                //等待处理
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.PreHandle:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  and ShippingStatus={1}  and  ( (PaymentGateway='{2}'  and PaymentStatus={3} )  or   ( PaymentStatus={4}  and PaymentGateway!='{2}' )  )",
                          (int)EnumHelper.OrderStatus.UnHandle, (int)EnumHelper.ShippingStatus.UnShipped, EnumHelper.PaymentGateway.cod.ToString(), (int)EnumHelper.PaymentStatus.Unpaid,
                           (int)EnumHelper.PaymentStatus.Paid);
                    break;
                //取消订单
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Cancel:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  ",
                          (int)EnumHelper.OrderStatus.Cancel);
                    break;
                //订单锁定
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Locking:
                    strWhere =
                      String.Format(
                          "  PaymentStatus={1} and ShippingStatus={2}  and (OrderStatus={0} or OrderStatus={3})  ",
                          (int)EnumHelper.OrderStatus.AdminLock, (int)EnumHelper.PaymentStatus.Unpaid,
                          (int)EnumHelper.ShippingStatus.UnShipped, (int)EnumHelper.OrderStatus.UserLock);
                    break;
                //等待付款确认
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.PreConfirm:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  and PaymentStatus={1} and ShippingStatus={2} and PaymentGateway!='{3}'",
                          (int)EnumHelper.OrderStatus.UnHandle, (int)EnumHelper.PaymentStatus.Paid,
                          (int)EnumHelper.ShippingStatus.UnShipped, EnumHelper.PaymentGateway.bank.ToString());
                    break;
                //正在处理
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Handling:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  and ShippingStatus={1} ",
                          (int)EnumHelper.OrderStatus.Handling,
                          (int)EnumHelper.ShippingStatus.UnShipped);
                    break;
                //配货中
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Shipping:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  and  ShippingStatus={1}",
                          (int)EnumHelper.OrderStatus.Handling,
                          (int)EnumHelper.ShippingStatus.Packing);
                    break;
                //已发货
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Shiped:
                    strWhere =
                     String.Format(
                         " OrderStatus={0}  and  ShippingStatus={1}",
                         (int)EnumHelper.OrderStatus.Handling,
                         (int)EnumHelper.ShippingStatus.Shipped);
                    break;
                //已完成
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus.Complete:
                    strWhere =
                  String.Format(
                      " OrderStatus={0}  and  ShippingStatus={1}",
                      (int)EnumHelper.OrderStatus.Complete,
                      (int)EnumHelper.ShippingStatus.ConfirmShip);
                    break;
                default:
                    break;
            }
            return strWhere;
        }
        /// <summary>
        /// 根据订单组合状态获取订单
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Order.OrderInfo> GetListByStatus(
            YSWL.MALL.Model.Shop.Order.EnumHelper.OrderMainStatus orderType)
        {
            string strWhere = GetWhereByStatus(orderType) + " order by CreatedDate desc ";
            return GetModelList(strWhere);
        }
        /// <summary>
        /// 根据各种状态返回组合订单状态
        /// </summary>
        /// <returns></returns>
        public EnumHelper.OrderMainStatus GetOrderType(EnumHelper.PaymentGateway paymentGateway, EnumHelper.OrderStatus orderStatus, EnumHelper.PaymentStatus paymentStatus, EnumHelper.ShippingStatus shippingStatus)
        {
            EnumHelper.OrderMainStatus orderType = EnumHelper.OrderMainStatus.PreHandle;
            switch (paymentGateway)
            {
                case EnumHelper.PaymentGateway.cod:
                    //等待处理
                    if (orderStatus == EnumHelper.OrderStatus.UnHandle &&
                        paymentStatus == EnumHelper.PaymentStatus.Unpaid &&
                        shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.PreHandle;
                    }
                    //取消订单
                    if (orderStatus == EnumHelper.OrderStatus.Cancel)
                    {
                        orderType = EnumHelper.OrderMainStatus.Cancel;
                    }
                    //订单锁定
                    if ((orderStatus == EnumHelper.OrderStatus.UserLock || orderStatus == EnumHelper.OrderStatus.AdminLock) &&
                    paymentStatus == EnumHelper.PaymentStatus.Unpaid &&
                    shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Locking;
                    }
                    //正在处理
                    if (orderStatus == EnumHelper.OrderStatus.Handling &&
                     paymentStatus == EnumHelper.PaymentStatus.Unpaid &&
                     shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Handling;
                    }
                    //配货中
                    if (orderStatus == EnumHelper.OrderStatus.Handling &&
                     paymentStatus == EnumHelper.PaymentStatus.Unpaid &&
                     shippingStatus == EnumHelper.ShippingStatus.Packing)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shipping;
                    }
                    //已发货
                    if (orderStatus == EnumHelper.OrderStatus.Handling &&
                     paymentStatus == EnumHelper.PaymentStatus.Unpaid &&
                     shippingStatus == EnumHelper.ShippingStatus.Shipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shiped;
                    }
                    //已完成
                    if (orderStatus == EnumHelper.OrderStatus.Complete &&
                     paymentStatus == EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == EnumHelper.ShippingStatus.ConfirmShip)
                    {
                        orderType = EnumHelper.OrderMainStatus.Complete;
                    }
                    break;
                case EnumHelper.PaymentGateway.bank:
                    //等待付款
                    if (orderStatus == EnumHelper.OrderStatus.UnHandle &&
                        paymentStatus == EnumHelper.PaymentStatus.Unpaid &&
                        shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Paying;
                    }
                    //取消订单
                    if (orderStatus == EnumHelper.OrderStatus.Cancel)
                    {
                        orderType = EnumHelper.OrderMainStatus.Cancel;
                    }
                    //订单锁定
                    if ((orderStatus == EnumHelper.OrderStatus.UserLock || orderStatus == EnumHelper.OrderStatus.AdminLock) &&
                    paymentStatus == EnumHelper.PaymentStatus.Unpaid &&
                    shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Locking;
                    }
                    //等待付款确认
                    if (orderStatus == EnumHelper.OrderStatus.UnHandle &&
                     paymentStatus == EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.PreConfirm;
                    }
                    //正在处理
                    if (orderStatus == EnumHelper.OrderStatus.Handling &&
                     paymentStatus == EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Handling;
                    }
                    //配货中
                    if (orderStatus == EnumHelper.OrderStatus.Handling &&
                     paymentStatus == EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == EnumHelper.ShippingStatus.Packing)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shipping;
                    }
                    //已发货
                    if (orderStatus == EnumHelper.OrderStatus.Handling &&
                     paymentStatus == EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == EnumHelper.ShippingStatus.Shipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shiped;
                    }
                    //已完成
                    if (orderStatus == EnumHelper.OrderStatus.Complete &&
                     paymentStatus == EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == EnumHelper.ShippingStatus.ConfirmShip)
                    {
                        orderType = EnumHelper.OrderMainStatus.Complete;
                    }
                    break;
                default:
                    //等待付款
                    if (orderStatus == EnumHelper.OrderStatus.UnHandle &&
                        paymentStatus == EnumHelper.PaymentStatus.Unpaid &&
                        shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Paying;
                    }
                    //等待处理
                    if (orderStatus == EnumHelper.OrderStatus.UnHandle &&
                        paymentStatus == EnumHelper.PaymentStatus.Paid &&
                        shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.PreHandle;
                    }
                    //取消订单
                    if (orderStatus == EnumHelper.OrderStatus.Cancel)
                    {
                        orderType = EnumHelper.OrderMainStatus.Cancel;
                    }
                    //订单锁定
                    if ((orderStatus == EnumHelper.OrderStatus.UserLock || orderStatus == EnumHelper.OrderStatus.AdminLock) &&
                    paymentStatus == EnumHelper.PaymentStatus.Unpaid &&
                    shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Locking;
                    }
                    //正在处理
                    if (orderStatus == EnumHelper.OrderStatus.Handling &&
                     paymentStatus == EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Handling;
                    }
                    //配货中
                    if (orderStatus == EnumHelper.OrderStatus.Handling &&
                     paymentStatus == EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == EnumHelper.ShippingStatus.Packing)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shipping;
                    }
                    //已发货
                    if (orderStatus == EnumHelper.OrderStatus.Handling &&
                     paymentStatus == EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == EnumHelper.ShippingStatus.Shipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shiped;
                    }
                    //已完成
                    if (orderStatus == EnumHelper.OrderStatus.Complete &&
                     paymentStatus == EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == EnumHelper.ShippingStatus.ConfirmShip)
                    {
                        orderType = EnumHelper.OrderMainStatus.Complete;
                    }
                    break;
            }
            return orderType;

        }

        /// <summary>
        /// 根据各种状态返回组合订单状态
        /// </summary>
        /// <returns></returns>
        public EnumHelper.OrderMainStatus GetOrderType(string paymentGateway, int orderStatus, int paymentStatus, int shippingStatus)
        {
            EnumHelper.OrderMainStatus orderType = EnumHelper.OrderMainStatus.PreHandle;
            switch (paymentGateway)
            {
                case "cod":
                    //等待处理
                    if (orderStatus == (int)EnumHelper.OrderStatus.UnHandle &&
                        paymentStatus == (int)EnumHelper.PaymentStatus.Unpaid &&
                        shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.PreHandle;
                    }
                    //取消订单
                    if (orderStatus == (int)EnumHelper.OrderStatus.Cancel)
                    {
                        orderType = EnumHelper.OrderMainStatus.Cancel;
                    }
                    //订单锁定
                    if ((orderStatus == (int)EnumHelper.OrderStatus.UserLock || orderStatus == (int)EnumHelper.OrderStatus.AdminLock) &&
                    paymentStatus == (int)EnumHelper.PaymentStatus.Unpaid &&
                    shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Locking;
                    }
                    //正在处理
                    if (orderStatus == (int)EnumHelper.OrderStatus.Handling &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Unpaid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Handling;
                    }
                    //配货中
                    if (orderStatus == (int)EnumHelper.OrderStatus.Handling &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Unpaid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.Packing)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shipping;
                    }
                    //已发货
                    if (orderStatus == (int)EnumHelper.OrderStatus.Handling &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Unpaid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.Shipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shiped;
                    }
                    //已完成
                    if (orderStatus == (int)EnumHelper.OrderStatus.Complete &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.ConfirmShip)
                    {
                        orderType = EnumHelper.OrderMainStatus.Complete;
                    }
                    break;
                case "bank":
                    //等待付款
                    if (orderStatus == (int)EnumHelper.OrderStatus.UnHandle &&
                        paymentStatus == (int)EnumHelper.PaymentStatus.Unpaid &&
                        shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Paying;
                    }
                    //取消订单
                    if (orderStatus == (int)EnumHelper.OrderStatus.Cancel)
                    {
                        orderType = EnumHelper.OrderMainStatus.Cancel;
                    }
                    //订单锁定
                    if ((orderStatus == (int)EnumHelper.OrderStatus.UserLock || orderStatus == (int)EnumHelper.OrderStatus.AdminLock) &&
                    paymentStatus == (int)EnumHelper.PaymentStatus.Unpaid &&
                    shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Locking;
                    }
                    //等待付款确认
                    if (orderStatus == (int)EnumHelper.OrderStatus.UnHandle &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.PreConfirm;
                    }
                    //正在处理
                    if (orderStatus == (int)EnumHelper.OrderStatus.Handling &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Handling;
                    }
                    //配货中
                    if (orderStatus == (int)EnumHelper.OrderStatus.Handling &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.Packing)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shipping;
                    }
                    //已发货
                    if (orderStatus == (int)EnumHelper.OrderStatus.Handling &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.Shipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shiped;
                    }
                    //已完成
                    if (orderStatus == (int)EnumHelper.OrderStatus.Complete &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.ConfirmShip)
                    {
                        orderType = EnumHelper.OrderMainStatus.Complete;
                    }
                    break;
                default:
                    //等待付款
                    if (orderStatus == (int)EnumHelper.OrderStatus.UnHandle &&
                        paymentStatus == (int)EnumHelper.PaymentStatus.Unpaid &&
                        shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Paying;
                    }
                    //等待处理
                    if (orderStatus == (int)EnumHelper.OrderStatus.UnHandle &&
                        paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                        shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.PreHandle;
                    }
                    //取消订单
                    if (orderStatus == (int)EnumHelper.OrderStatus.Cancel)
                    {
                        orderType = EnumHelper.OrderMainStatus.Cancel;
                    }
                    //订单锁定
                    if ((orderStatus == (int)EnumHelper.OrderStatus.UserLock || orderStatus == (int)EnumHelper.OrderStatus.AdminLock) &&
                    paymentStatus == (int)EnumHelper.PaymentStatus.Unpaid &&
                    shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Locking;
                    }
                    //正在处理
                    if (orderStatus == (int)EnumHelper.OrderStatus.Handling &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.UnShipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Handling;
                    }
                    //配货中
                    if (orderStatus == (int)EnumHelper.OrderStatus.Handling &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.Packing)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shipping;
                    }
                    //已发货
                    if (orderStatus == (int)EnumHelper.OrderStatus.Handling &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.Shipped)
                    {
                        orderType = EnumHelper.OrderMainStatus.Shiped;
                    }
                    //已完成
                    if (orderStatus == (int)EnumHelper.OrderStatus.Complete &&
                     paymentStatus == (int)EnumHelper.PaymentStatus.Paid &&
                     shippingStatus == (int)EnumHelper.ShippingStatus.ConfirmShip)
                    {
                        orderType = EnumHelper.OrderMainStatus.Complete;
                    }
                    break;
            }
            return orderType;

        }
        public string GetOrderTypeStr(string paymentGateway, int orderStatus, int paymentStatus, int shippingStatus)
        {
            string str = "";
            EnumHelper.OrderMainStatus orderType = GetOrderType(paymentGateway,
                                    orderStatus,
                                    paymentStatus,
                                    shippingStatus);
            switch (orderType)
            {
                //  订单组合状态 1 等待付款   | 2 等待处理 | 3 取消订单 | 4 订单锁定 | 5 等待付款确认 | 6 正在处理 |7 配货中  |8 已发货 |9  已完成
                case EnumHelper.OrderMainStatus.Paying:
                    str = "等待付款";
                    break;
                case EnumHelper.OrderMainStatus.PreHandle:
                    str = "等待处理";
                    break;
                case EnumHelper.OrderMainStatus.Cancel:
                    str = "取消订单";
                    break;
                case EnumHelper.OrderMainStatus.Locking:
                    str = "订单锁定";
                    break;
                case EnumHelper.OrderMainStatus.PreConfirm:
                    str = "等待付款确认";
                    break;
                case EnumHelper.OrderMainStatus.Handling:
                    str = "正在处理";
                    break;
                case EnumHelper.OrderMainStatus.Shipping:
                    str = "配货中";
                    break;
                case EnumHelper.OrderMainStatus.Shiped:
                    str = "已发货";
                    break;
                case EnumHelper.OrderMainStatus.Complete:
                    str = "已完成";
                    break;
                default:
                    str = "未知状态";
                    break;
            }
            return str;
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Order.OrderInfo> GetListByPageEX(string strWhere, string orderby, int startIndex, int endIndex)
        {
            DataSet ds = GetListByPage(strWhere, orderby, startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 得到一个对象实体(包括订单项)
        /// </summary>
        public YSWL.MALL.Model.Shop.Order.OrderInfo GetModelInfo(long OrderId)
        {
            YSWL.MALL.Model.Shop.Order.OrderInfo model = GetModel(OrderId);
            if (model != null)
            {
                YSWL.MALL.BLL.Shop.Order.OrderItems itemBll = new OrderItems();
                model.OrderItems = itemBll.GetModelList(" OrderId=" + OrderId);
                YSWL.MALL.BLL.Shop.Order.OrderOptions optionBll = new YSWL.MALL.BLL.Shop.Order.OrderOptions();
                model.OrderOptions = optionBll.Get2ListByOrderId(OrderId);
            }
            return model;
        }


        /// <summary>
        /// 得到一个对象实体(包括订单项) 缓存
        /// </summary>
        public YSWL.MALL.Model.Shop.Order.OrderInfo GetModelInfoByCache(long OrderId)
        {

            string CacheKey = "GetModelInfoByCache-" + OrderId;
            object objModel = YSWL.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetModelInfo(OrderId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        YSWL.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (YSWL.MALL.Model.Shop.Order.OrderInfo)objModel;

        }

        public void RemoveModelInfoCache(long OrderId)
        {
            YSWL.Common.DataCache.DeleteCache("OrdersModel-" + OrderId);
            YSWL.Common.DataCache.DeleteCache("GetModelInfoByCache-" + OrderId);
        }

        //更新发货数量
        public bool UpdateShipped(YSWL.MALL.Model.Shop.Order.OrderInfo orderModel)
        {
            return dal.UpdateShipped(orderModel);
        }

        /// <summary>
        /// 根据条件获取对应的订单状态的数量
        /// </summary>
        /// <param name="userid">下单人 ID</param>
        /// <param name="PaymentStatus">支付状态</param>
        /// <returns></returns>
        public int GetPaymentStatusCounts(int userid, int PaymentStatus)
        {
            return dal.GetPaymentStatusCounts(userid, PaymentStatus, (int)EnumHelper.OrderStatus.Cancel);
        }
        /// <summary>
        /// 获取待支付订单数 （排除货到付款）
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetUnPaidCounts(int userid)
        {
            return dal.GetUnPaidCounts(userid);
        }
        /// <summary>
        /// 获得商家数据列表
        /// </summary>
        /// <param name="supplierid"></param>
        /// <param name="orderStatus"></param>
        /// <param name="orderCode"></param>
        /// <param name="shipName"></param>
        /// <param name="buyerName"></param>
        /// <param name="paymentStatus"></param>
        /// <param name="shippingStatus"></param>
        /// <param name="dateRange"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="toalCount"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Order.OrderInfo> GetListByPage(int supplierid, int orderStatus, string orderCode, string shipName,
            string buyerName, int paymentStatus, int shippingStatus, string dateRange, int startIndex, int endIndex, out int toalCount)
        {
            StringBuilder strWhere = new StringBuilder();

            if (orderStatus > 0)
            {
                strWhere.Append(GetWhereByOrderStatus(orderStatus));
            }
            if (!string.IsNullOrWhiteSpace(orderCode))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" OrderCode like '%{0}%'", InjectionFilter.SqlFilter(orderCode));
            }
            if (!string.IsNullOrWhiteSpace(shipName))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ShipName like '%{0}%'", InjectionFilter.SqlFilter(shipName));
            }
            if (!string.IsNullOrWhiteSpace(buyerName))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" BuyerName like '%{0}%'", InjectionFilter.SqlFilter(buyerName));
            }
            if (paymentStatus != -1)
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" PaymentStatus = {0}", paymentStatus);
            }
            if (shippingStatus != -1)
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ShippingStatus = {0}", shippingStatus);
            }
            if (!string.IsNullOrEmpty(dateRange))
            {
                string[] date = dateRange.Split('_');
                if (date.Length > 0)
                {
                    DateTime? dateStart = Globals.SafeDateTime(date[0], null);
                    if (dateStart.HasValue)
                    {
                        if (strWhere.Length > 1)
                        {
                            strWhere.Append(" and ");
                        }
                        strWhere.AppendFormat(" CreatedDate >= CONVERT(DATETIME, '{0}') ", dateStart.Value);
                    }
                }
                if (date.Length > 1)
                {
                    DateTime? dateEnd = Globals.SafeDateTime(date[1], null);
                    if (dateEnd.HasValue)
                    {
                        if (strWhere.Length > 1)
                        {
                            strWhere.Append(" and ");
                        }
                        strWhere.AppendFormat(" CreatedDate < CONVERT(DATETIME, '{0}')", dateEnd.Value.AddDays(1));
                    }
                }
            }


            #region 商家
            if (strWhere.Length > 1) strWhere.Append(" and ");
            strWhere.AppendFormat(" SupplierId = {0}", supplierid);
            #endregion

            toalCount = GetRecordCount(strWhere.ToString());
            DataSet ds = dal.GetListByPage(strWhere.ToString(), " CreatedDate desc ", startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 商家更新订单备注
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="Remark"></param>
        /// <param name="supplierid"></param>
        /// <returns></returns>
        public bool UpdateOrderRemark(long orderId, string Remark, int supplierid)
        {
            return dal.UpdateOrderRemark(orderId, InjectionFilter.SqlFilter(Remark), string.Format(" and SupplierId= {0}", supplierid));
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        /// <param name="agentId">代理商ID</param>
        /// <param name="orderStatus"></param>
        /// <param name="orderCode"></param>
        /// <param name="shipName"></param>
        /// <param name="buyerName"></param>
        /// <param name="paymentStatus"></param>
        /// <param name="shippingStatus"></param>
        /// <param name="dateRange"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="toalCount"></param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Order.OrderInfo> GetAgentOrderListByPage(int agentId, int orderStatus, string orderCode, string shipName,
            string buyerName, int paymentStatus, int shippingStatus, string dateRange, int startIndex, int endIndex, out int toalCount)
        {
            StringBuilder strWhere = new StringBuilder();

            if (orderStatus > 0)
            {
                strWhere.Append(GetWhereByOrderStatus(orderStatus));
            }
            if (!string.IsNullOrWhiteSpace(orderCode))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" OrderCode like '%{0}%'", InjectionFilter.SqlFilter(orderCode));
            }
            if (!string.IsNullOrWhiteSpace(shipName))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ShipName like '%{0}%'", InjectionFilter.SqlFilter(shipName));
            }
            if (!string.IsNullOrWhiteSpace(buyerName))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" BuyerName like '%{0}%'", InjectionFilter.SqlFilter(buyerName));
            }
            if (paymentStatus != -1)
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" PaymentStatus = {0}", paymentStatus);
            }
            if (shippingStatus != -1)
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ShippingStatus = {0}", shippingStatus);
            }
            if (!string.IsNullOrEmpty(dateRange))
            {
                string[] date = dateRange.Split('_');
                if (date.Length > 0)
                {
                    DateTime? dateStart = Globals.SafeDateTime(date[0], null);
                    if (dateStart.HasValue)
                    {
                        if (strWhere.Length > 1)
                        {
                            strWhere.Append(" and ");
                        }
                        strWhere.AppendFormat(" CreatedDate >= CONVERT(DATETIME, '{0}') ", dateStart.Value);
                    }
                }
                if (date.Length > 1)
                {
                    DateTime? dateEnd = Globals.SafeDateTime(date[1], null);
                    if (dateEnd.HasValue)
                    {
                        if (strWhere.Length > 1)
                        {
                            strWhere.Append(" and ");
                        }
                        strWhere.AppendFormat(" CreatedDate <= CONVERT(DATETIME, '{0}')", dateEnd.Value);
                    }
                }
            }

            #region 代理商

            if (strWhere.Length > 1)
            {
                strWhere.Append(" and ");
            }
            strWhere.AppendFormat(
                "  EXISTS(SELECT * FROM  Shop_Suppliers WHERE SupplierId= T.SupplierId  AND  AgentId={0} ) ", agentId);
            #endregion
            toalCount = GetRecordCount(strWhere.ToString());
            DataSet ds = dal.GetListByPage(strWhere.ToString(), " CreatedDate desc ", startIndex, endIndex);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得商家数据列表
        /// </summary>
        public DataSet GetList(int supplierid, int orderStatus, string orderCode, string shipName,
            string buyerName, int paymentStatus, int shippingStatus, string dateRange)
        {
            StringBuilder strWhere = new StringBuilder();
            if (orderStatus > 0)
            {
                strWhere.Append(GetWhereByOrderStatus(orderStatus));
            }
            if (!string.IsNullOrWhiteSpace(orderCode))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" OrderCode like '%{0}%'", InjectionFilter.SqlFilter(orderCode));
            }
            if (!string.IsNullOrWhiteSpace(shipName))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ShipName like '%{0}%'", InjectionFilter.SqlFilter(shipName));
            }
            if (!string.IsNullOrWhiteSpace(buyerName))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" BuyerName like '%{0}%'", InjectionFilter.SqlFilter(buyerName));
            }
            if (paymentStatus != -1)
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" PaymentStatus = {0}", paymentStatus);
            }
            if (shippingStatus != -1)
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ShippingStatus = {0}", shippingStatus);
            }
            if (!string.IsNullOrEmpty(dateRange))
            {
                string[] date = dateRange.Split('_');
                if (date.Length > 0)
                {
                    DateTime? dateStart = Globals.SafeDateTime(date[0], null);
                    if (dateStart.HasValue)
                    {
                        if (strWhere.Length > 1)
                        {
                            strWhere.Append(" and ");
                        }
                        strWhere.AppendFormat(" CreatedDate >= CONVERT(DATETIME, '{0}') ", dateStart.Value);
                    }
                }
                if (date.Length > 1)
                {
                    DateTime? dateEnd = Globals.SafeDateTime(date[1], null);
                    if (dateEnd.HasValue)
                    {
                        if (strWhere.Length > 1)
                        {
                            strWhere.Append(" and ");
                        }
                        strWhere.AppendFormat(" CreatedDate <= CONVERT(DATETIME, '{0}')", dateEnd.Value);
                    }
                }
            }
            #region 商家
            if (strWhere.Length > 1) strWhere.Append(" and ");
            strWhere.AppendFormat(" SupplierId = {0}", supplierid);
            #endregion
            return dal.GetList(0, strWhere.ToString(), " CreatedDate desc ");
        }

        /// <summary>
        /// 获得代理商数据
        /// </summary>
        public DataSet GetListByAgent(int agentId, int orderStatus, string orderCode, string shipName,
            string buyerName, int paymentStatus, int shippingStatus, string dateRange)
        {
            StringBuilder strWhere = new StringBuilder();
            if (orderStatus > 0)
            {
                strWhere.Append(GetWhereByOrderStatus(orderStatus));
            }
            if (!string.IsNullOrWhiteSpace(orderCode))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" OrderCode like '%{0}%'", InjectionFilter.SqlFilter(orderCode));
            }
            if (!string.IsNullOrWhiteSpace(shipName))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ShipName like '%{0}%'", InjectionFilter.SqlFilter(shipName));
            }
            if (!string.IsNullOrWhiteSpace(buyerName))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" BuyerName like '%{0}%'", InjectionFilter.SqlFilter(buyerName));
            }
            if (paymentStatus != -1)
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" PaymentStatus = {0}", paymentStatus);
            }
            if (shippingStatus != -1)
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ShippingStatus = {0}", shippingStatus);
            }
            if (!string.IsNullOrEmpty(dateRange))
            {
                string[] date = dateRange.Split('_');
                if (date.Length > 0)
                {
                    DateTime? dateStart = Globals.SafeDateTime(date[0], null);
                    if (dateStart.HasValue)
                    {
                        if (strWhere.Length > 1)
                        {
                            strWhere.Append(" and ");
                        }
                        strWhere.AppendFormat(" CreatedDate >= CONVERT(DATETIME, '{0}') ", dateStart.Value);
                    }
                }
                if (date.Length > 1)
                {
                    DateTime? dateEnd = Globals.SafeDateTime(date[1], null);
                    if (dateEnd.HasValue)
                    {
                        if (strWhere.Length > 1)
                        {
                            strWhere.Append(" and ");
                        }
                        strWhere.AppendFormat(" CreatedDate <= CONVERT(DATETIME, '{0}')", dateEnd.Value);
                    }
                }
            }
            #region 代理商

            if (strWhere.Length > 1)
            {
                strWhere.Append(" and ");
            }
            strWhere.AppendFormat(
                "  EXISTS ( SELECT * FROM  Shop_Suppliers WHERE SupplierId= T.SupplierId  AND  AgentId={0} ) ", agentId);
            #endregion
            return dal.GetList(0, strWhere.ToString(), " CreatedDate desc ");
        }


        /// <summary>
        /// 根据商家订单状态获取查询订单的条件
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public string GetWhereByOrderStatus(int orderType)
        {
            string strWhere = "";
            switch (orderType)
            {
                //等待处理    未处理订单：
                //A. 在线支付: 已支付/未配货/未发货
                //B. 货到付款: 未支付/未配货/未发货
                case (int)EnumHelper.StoreOrderStatus.PreHandle:
                    strWhere =
                      String.Format(
                          " OrderStatus={0}  and PaymentStatus in ( {1},{2} ) and ShippingStatus={3} ",
                          (int)EnumHelper.OrderStatus.UnHandle, (int)EnumHelper.PaymentStatus.Unpaid, (int)EnumHelper.PaymentStatus.Paid,
                          (int)EnumHelper.ShippingStatus.UnShipped);
                    break;
                //未完成
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.StoreOrderStatus.NotComplete:
                    strWhere =
                      String.Format(
                      "( OrderStatus!={0}  or  ShippingStatus!={1})",
                      (int)EnumHelper.OrderStatus.Complete,
                      (int)EnumHelper.ShippingStatus.ConfirmShip);
                    break;
                //已完成
                case (int)YSWL.MALL.Model.Shop.Order.EnumHelper.StoreOrderStatus.Complete:
                    strWhere =
                  String.Format(
                      " OrderStatus={0}  and  ShippingStatus={1}",
                      (int)EnumHelper.OrderStatus.Complete,
                      (int)EnumHelper.ShippingStatus.ConfirmShip);
                    break;
                default:
                    break;
            }
            return strWhere;
        }

        public YSWL.MALL.Model.Shop.Order.OrderInfo GetOrderInfo(string ordercode)
        {
            return dal.GetOrderInfo(ordercode);
        }

        public DataSet GetSalesStatis(string strWhere)
        {
            return dal.GetSalesStatis(strWhere);
        }

        public DataSet GetMySalesStatis(string strWhere)
        {
            return dal.GetMySalesStatis(strWhere);
        }

        public Decimal GetOrderTotal(string strWhere)
        {
            return dal.GetOrderTotal(strWhere);
        }

        public int GetOrderCount(string strWhere)
        {
            return dal.GetOrderCount(strWhere);
        }

        public int GetCustomCount(string strWhere)
        {
            return dal.GetCustomCount(strWhere);
        }

        public List<long> GetBrandList(int supplierId)
        {
            DataSet ds = dal.GetBrandList(supplierId);
            return IdDataSetToList(ds);
        }

        public List<long> IdDataSetToList(DataSet ds)
        {
            List<long> idList = new List<long>();
            if (null != ds && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["BrandId"] != null)
                    {
                        long id = Convert.ToInt64(dr["BrandId"]);
                        idList.Add(id);
                    }
                }
            }
            return idList;
        }

        public YSWL.MALL.Model.Shop.Order.OrderInfo GetOrderByOrderCode(string orderCode)
        {
            return dal.GetModel(orderCode);
        }
        /// <summary>
        /// 根据购买用户Id获取是否是首单 排除已取消订单 （返回  true 为首单  false  为否）
        /// </summary>
        /// <param name="buyerID">购买用户Id</param>
        /// <returns></returns>
        public bool IsFirstOrder(int buyerID)
        {
            return dal.IsFirstOrder(buyerID);
        }

        /// <summary>
        /// 根据 优惠券 获取订单信息
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Order.OrderInfo GetOrderByCoupon(string coupon)
        {
            return dal.GetOrderByCoupon(coupon);
        }

        public List<YSWL.MALL.Model.Shop.Order.OrderInfo> GetModelList(string startDate, string endDate, int supplierId)
        {
            string strWhere = "";
            if (!string.IsNullOrWhiteSpace(startDate))
            {
                strWhere += " CreatedDate >='" + Common.InjectionFilter.SqlFilter(startDate) + "' ";
            }
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                if (strWhere.Length > 1)
                {
                    strWhere += " And ";
                }
                strWhere += " CreatedDate <='" + Common.InjectionFilter.SqlFilter(endDate) + "' ";
            }
            if (supplierId > 0)
            {
                if (strWhere.Length > 1)
                {
                    strWhere += " And ";
                }
                strWhere += string.Format(" SupplierId={0}", supplierId);
            }
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        public bool UpdateOrderTypeSub(long orderId, int orderTypeSub)
        {
            return dal.UpdateOrderTypeSub(orderId, orderTypeSub);
        }
        /// <summary>
        /// 是否允许修改当前订单的状态
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="ShippingStatus"></param>
        /// <param name="OrderStatus"></param>
        /// <returns></returns>
        public bool IsAllowModify(long OrderId, int ShippingStatus, int OrderStatus)
        {
            return dal.IsAllowModify(OrderId, ShippingStatus, OrderStatus);
        }
        /// <summary>
        ///  根据日期获取订单数及总支付金额
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet GetOrderCountAmount(DateTime startDate, DateTime endDate)
        {
            return dal.GetOrderCountAmount(startDate, endDate);
        }
        /// <summary>
        /// 获取待支付订单数 （排除货到付款）
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetUnPaidCounts()
        {
            return dal.GetRecordCount(" PaymentStatus=0 AND OrderStatus!=-1 and OrderType=1  and PaymentGateway<>'cod'  ");
        }
        /// <summary>
        /// 获取待发货订单数  （货到付款未发货，在线支付已支付未发货时）
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetUnShippingCounts()
        {
            return dal.GetRecordCount(" ShippingStatus<2 AND OrderStatus!=-1 and OrderType=1 and ( ( PaymentStatus=2 and PaymentGateway<>'cod' ) or ( PaymentStatus=0 and PaymentGateway='cod' ) ) ");
        }
        /// <summary>
        /// 获取待收货订单数 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetUnReceipt()
        {
            return dal.GetRecordCount("  ShippingStatus=2 AND OrderStatus=1 and OrderType=1  ");
        }
        /// <summary>
        /// 按天统计订单金额
        /// </summary>
        /// <returns></returns>
        public List<YSWL.MALL.ViewModel.Order.OrderPriceCount> StatOrderAmount(DateTime startDate, DateTime endDate)
        {
            DataSet ds = dal.StatOrderAmount(startDate, endDate);
            if (DataSetTools.DataSetIsNull(ds))
            {
                return null;
            }
            List<YSWL.MALL.ViewModel.Order.OrderPriceCount> list = new List<YSWL.MALL.ViewModel.Order.OrderPriceCount>();
            YSWL.MALL.ViewModel.Order.OrderPriceCount model = null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                model = new YSWL.MALL.ViewModel.Order.OrderPriceCount();
                model.DateStr = dr["CreatedDate"].ToString();
                model.Count = Common.Globals.SafeInt(dr["OrderCount"], 0);
                model.Price = Common.Globals.SafeDecimal(dr["TotalAmount"], 0);
                list.Add(model);
            }
            return list;
        }
        /// <summary>
        ///  统计用户订单数及金额
        /// </summary>
        /// <param name="top"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public DataSet StatBuyerOrderCountAmount(int top, DateTime startDate, DateTime endDate)
        {
            return dal.StatBuyerOrderCountAmount(top, startDate, endDate);
        }
        /// <summary>
        /// 获取未付款金额 (不排除货到付款)
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetUnPaidAmount(int userid)
        {
            return dal.GetUnPaidAmount(userid);
        }
        /// <summary>
        /// 获取已付款金额 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetPaidAmount(int userid)
        {
            return dal.GetPaidAmount(userid);
        }
        /// <summary>
        /// 获取订单来源
        /// </summary>
        /// <param name="referType"></param>
        /// <returns></returns>
        public string GetOrderReferTypeStr(int? referType)
        {
            if (!referType.HasValue) { return string.Empty; }
            string str = "";
            EnumHelper.ReferType referTypeE = YSWL.Common.Globals.SafeEnum<YSWL.MALL.Model.Shop.Order.EnumHelper.ReferType>(referType.Value.ToString(), YSWL.MALL.Model.Shop.Order.EnumHelper.ReferType.SalesMan, true);
            switch (referTypeE)
            {
                case EnumHelper.ReferType.None:
                    str = "";
                    break;
                case EnumHelper.ReferType.PC:
                    str = "PC";
                    break;
                case EnumHelper.ReferType.WeChat:
                    str = "微信C端";
                    break;
                case EnumHelper.ReferType.WeChatB:
                    str = "微信B端";
                    break;
                case EnumHelper.ReferType.SalesMan:
                    str = "业务员代下单";
                    break;
                case EnumHelper.ReferType.Cust:
                    str = "客服代下单";
                    break;
                case EnumHelper.ReferType.Ding:
                    str = "APP";
                    break;
                default:
                    str = "未知状态";
                    break;
            }
            return str;
        }
        /// <summary>
        /// /根据业务员id获取客户订单金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Decimal GetOrderTotal(int userId, DateTime startDate)
        {
            return dal.GetOrderTotal(string.Format("  ReferID='{0}' AND CreatedDate>='{1}'  ", userId, startDate.Date));
        }
        /// <summary>
        ///根据业务员id获取客户订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetOrderCount(int userId, DateTime startDate)
        {
            return dal.GetOrderCount(string.Format("  ReferID='{0}' AND CreatedDate>='{1}'  ", userId, startDate.Date));
        }
        /// <summary>
        ///根据业务员id获取下单客户数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetCustomCount(int userId, DateTime startDate)
        {
            return dal.GetCustomCount(string.Format("  ReferID='{0}' AND CreatedDate>='{1}'  ", userId, startDate.Date));
        }



        #endregion  ExtensionMethod

        #region Post统计接口

        /// <summary>
        ///  根据日期获取订单数及总支付金额
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">订单来源</param>
        /// <returns></returns>
        public YSWL.MALL.ViewModel.Order.OrderPriceCount GetOrderCountAmountByUser(int userId, DateTime startDate, DateTime endDate, int referType)
        {
            DataSet ds = dal.GetOrderCountAmountByUser(userId, startDate, endDate,referType);
            if (DataSetTools.DataSetIsNull(ds))
            {
                return null;
            }
            YSWL.MALL.ViewModel.Order.OrderPriceCount model = null;
            if (ds.Tables[0].Rows.Count <= 0) return null;
            DataRow dr = ds.Tables[0].Rows[0];
            model = new YSWL.MALL.ViewModel.Order.OrderPriceCount
            {
                Count = Common.Globals.SafeInt(dr["OrderCount"], 0),
                Price = Common.Globals.SafeDecimal(dr["TotalAmount"], 0)
            };
            return model;
        }

        /// <summary>
        /// 获取未付款金额 (不排除货到付款)
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">订单来源</param>
        /// <returns></returns>
        public decimal GetUnPaidAmountByUser(int userid, DateTime startDate, DateTime endDate,int referType)
        {
            return dal.GetUnPaidAmountByUser(userid, startDate, endDate,referType);
        }

        /// <summary>
        /// 获取已付款金额 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">订单来源</param>
        /// <returns></returns>
        public decimal GetPaidAmountByUser(int userid, DateTime startDate, DateTime endDate, int referType)
        {
            return dal.GetPaidAmountByUser(userid, startDate, endDate,referType);
        }

        /// <summary>
        /// 获取支付类型方式
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">订单来源</param>
        /// <returns></returns>
        public List<YSWL.MALL.ViewModel.Order.Payment> GetPaymentByUser(int userid, DateTime startDate, DateTime endDate,int referType)
        {
            DataSet ds = dal.GetPaymentByUser(userid, startDate, endDate,referType);
            if (DataSetTools.DataSetIsNull(ds))
            {
                return null;
            }
            List<YSWL.MALL.ViewModel.Order.Payment> list = new List<YSWL.MALL.ViewModel.Order.Payment>();
            YSWL.MALL.ViewModel.Order.Payment model = null;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                model = new YSWL.MALL.ViewModel.Order.Payment();
                model.PaymentName = dr["PaymentTypeName"].ToString();
                model.Amount = Common.Globals.SafeDecimal(dr["Amount"], 0);
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 获取订单项详情，方便统计数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">事件来源</param>
        /// <returns></returns>
        public List<YSWL.MALL.Model.Shop.Order.OrderItems> GetOrderItemByUser(int userId, DateTime startDate,DateTime endDate, int referType)
        {
            DataSet ds = dal.GetOrderItemByUser(userId, startDate, endDate,referType);
            return orderItemsBll.DataTableToList(ds.Tables[0]);
        }

		  /// <summary>
        /// 获取待发货订单数  （货到付款未发货，在线支付已支付未发货时）
        /// </summary>
        /// <param name="userId">购买者用户id</param>
        /// <returns>待发货订单数</returns>
        public int GetUnShippingCountsByUserId(int userId)
        {
            return dal.GetRecordCount(" ShippingStatus<2 AND OrderStatus!=-1 and OrderType=1 and ( ( PaymentStatus=2 and PaymentGateway<>'cod' ) or ( PaymentStatus=0 and PaymentGateway='cod' ) ) and BuyerID="+ userId);
        }
        /// <summary>
        /// 获取待收货订单数
        /// </summary>
        /// <param name="userId">购买者用户id</param>
        /// <returns>待收货订单数</returns>
        public int GetUnReceiptByUserId(int userId)
        {
            return dal.GetRecordCount("  ShippingStatus=2 AND OrderStatus=1 and OrderType=1 and BuyerID="+ userId);
        }

        #endregion
    }
}

