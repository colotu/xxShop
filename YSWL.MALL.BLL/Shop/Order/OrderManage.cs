/**
* OrderManage.cs
*
* 功 能： [N/A]
* 类 名： OrderManage
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/5/20 18:29:57  Ben    初版
*
* Copyright (c) 2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using YSWL.MALL.BLL.Members;
using YSWL.MALL.BLL.Shop.Commission;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.DALFactory;
using YSWL.MALL.IDAL.Shop.Order;
using System.Data;
using YSWL.MALL.Model.Shop.Order;
using System;
using System.Collections.Generic;
using YSWL.MALL.ViewModel.Order;
using System.Linq;
using YSWL.MALL.BLL.SysManage;
using YSWL.Common;
using YSWL.DBUtility;

namespace YSWL.MALL.BLL.Shop.Order
{
    public  class OrderManage
    {
        private static readonly BLL.Shop.Order.Orders orderManage = new BLL.Shop.Order.Orders();
        private static readonly BLL.Shop.Shipping.ShippingType shippingTypeManage = new Shipping.ShippingType();
        private static readonly IOrderService service = DAShopOrder.CreateOrderService();
        private static readonly string ProductKey = "Mall.ALL";
        private static DataCacheCore dataCache = new DataCacheCore(new CacheOption
        {
            CacheType = SAASInfo.GetSystemBoolValue("RedisCacheUse") ? CacheType.Redis : CacheType.IIS,
            ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
            ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            CancelProductKey=true,
            DefaultDb = 2
        });
        public static bool PreCodeSwitch
        {
            get { return BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_CreateOrder_PreCode"); }
        }

        public static bool BorrowEnable
        {
            get { return BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_CreateOrder_BorrowEnable"); }
        }

        #region 创建订单

        public static long CreateOrder(Model.Shop.Order.OrderInfo orderInfo,Accounts.Bus.User currentUser=null)
        {

            //是否开启多分仓
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();//YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            int depotId = -1;
            if (IsMultiDepot && (!orderInfo.SupplierId.HasValue || orderInfo.SupplierId.Value<=0))  //开启了多仓对接 并且不是商家商品订单，就需要对接仓库库存
            {
                depotId = YSWL.MALL.BLL.Shop.DisDepot.DepotRegion.GetDepotByRegion(orderInfo.RegionId);
                BLL.Shop.DisDepot.Depot depotBll = new DisDepot.Depot();
                YSWL.MALL.Model.Shop.DisDepot.Depot depotModel= depotBll.GetModel(depotId);
                if (depotModel != null) {
                    orderInfo.DepotId = depotId;
                    orderInfo.DepotName = depotModel.Name;
                }
            }

            #region 处理线上分销商信息
            //是否开启线上分销功能
            bool IsOpenDistr = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("CRM_Distribution_IsOpen");
            if (IsOpenDistr)
            {
                orderInfo.DistributionId= YSWL.MALL.BLL.Members.UserDistribution.GetUserDistrId(orderInfo.BuyerID);
            }
            #endregion 

            orderInfo.WaveStatus = -1;
            orderInfo.OrderId = service.CreateOrder(orderInfo,depotId,currentUser);

            #region 更新缓存处理
            YSWL.MALL.BLL.Shop.Products.StockHelper.UpdateOrderStock(orderInfo);
            #endregion

            return orderInfo.OrderId;
        }

        /// <summary>
        /// Pos创建订单
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="depotId"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public static long CreateOrderPos(Model.Shop.Order.OrderInfo orderInfo,int depotId ,Accounts.Bus.User currentUser = null)
        {

            //是否开启多分仓
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();//YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            if (IsMultiDepot && (!orderInfo.SupplierId.HasValue || orderInfo.SupplierId.Value <= 0))  //开启了多仓对接 并且不是商家商品订单，就需要对接仓库库存
            {
                //depotId = YSWL.MALL.BLL.Shop.DisDepot.DepotRegion.GetDepotByRegion(orderInfo.RegionId);
                BLL.Shop.DisDepot.Depot depotBll = new DisDepot.Depot();
                YSWL.MALL.Model.Shop.DisDepot.Depot depotModel = depotBll.GetModel(depotId);
                if (depotModel != null)
                {
                    orderInfo.DepotId = depotId;
                    orderInfo.DepotName = depotModel.Name;
                }
            }

            #region 处理线上分销商信息
            //是否开启线上分销功能
            bool IsOpenDistr = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("CRM_Distribution_IsOpen");
            if (IsOpenDistr)
            {
                orderInfo.DistributionId = YSWL.MALL.BLL.Members.UserDistribution.GetUserDistrId(orderInfo.BuyerID);
            }
            #endregion 

            orderInfo.WaveStatus = -1;
            orderInfo.OrderId = service.CreateOrder(orderInfo, depotId, currentUser);

            #region 更新缓存处理
            YSWL.MALL.BLL.Shop.Products.StockHelper.UpdatePosOrderStock(orderInfo, depotId);
            #endregion

            return orderInfo.OrderId;
        }

        #endregion


        #region 配货中
        public static bool PackingOrder(Model.Shop.Order.OrderInfo orderInfo, Accounts.Bus.User currentUser)
        {
            orderInfo = orderManage.GetModelInfo(orderInfo.OrderId);
            if (orderInfo.ShippingStatus != 0 || orderInfo.OrderStatus != 0) return false;

            if (!service.PackingOrder(orderInfo, currentUser)) return false;
        

            return true;
        }


        #endregion

        #region 取消订单
        public static bool CancelOrder(Model.Shop.Order.OrderInfo orderInfo, Accounts.Bus.User currentUser)
        {
            #region 检查订单状态
            //是否对接oms
            if (YSWL.MALL.BLL.Shop.Service.CommonHelper.ConnectionOMS())
            {
                //对接了oms 
                //在线支付未支付时可取消
                if (orderInfo.OrderStatus != (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle || orderInfo.PaymentStatus != (int)Payment.Model.PaymentStatus.NotYet ||  orderInfo.PaymentGateway == "cod")
                {
                    return false;
                }
            }
            else
            {
                //未对接oms   取消  (等待处理(未审核)时可取消)
                //未对接oms    未发货时 可取消
                if (orderInfo.OrderStatus < (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle || orderInfo.OrderStatus > (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.Handling || orderInfo.ShippingStatus >= (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ShippingStatus.Shipped)
                {
                    return false;
                }
            }
           
            #endregion

            //普通用户非法取消订单
            if (currentUser.UserType == "UU" &&
                (orderInfo.OrderStatus != 0 || currentUser.UserID != orderInfo.BuyerID))
            {
                YSWL.MALL.Model.SysManage.ErrorLog model = new YSWL.MALL.Model.SysManage.ErrorLog();
                model.Loginfo = string.Format("入侵拦截:[非法取消订单][YSWL.MALL.BLL.Shop.Order.OrderManage.CancelOrder] IP:[{0}]", System.Web.HttpContext.Current.Request.UserHostAddress);
                model.StackTrace = string.Empty;
                model.Url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                YSWL.MALL.BLL.SysManage.ErrorLog.Add(model);
                return false;
            }
            bool isSuccess= service.CancelOrder(orderInfo, currentUser);
            if (isSuccess)//更新库存缓存
            {
                YSWL.MALL.BLL.Shop.Products.StockHelper.CancelOrderStock(orderInfo);
            }
            return isSuccess;
        }
        #endregion

        #region 支付订单

        public static bool PayForOrder(Model.Shop.Order.OrderInfo orderInfo, Accounts.Bus.User currentUser = null)
        {
            //在线支付未支付时可支付
            if (orderInfo.PaymentStatus != (int)Payment.Model.PaymentStatus.NotYet || orderInfo.OrderStatus != (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle || orderInfo.PaymentGateway=="cod")
            {
                return false;
            }

            //订单项检测 - 补全数据
            if (orderInfo.OrderItems == null || orderInfo.OrderItems.Count < 1)
                orderInfo = orderManage.GetModelInfoByCache(orderInfo.OrderId);

            //子订单检测 - 补全数据
            if (orderInfo.HasChildren && orderInfo.SubOrders.Count < 1)
                orderInfo.SubOrders = orderManage.GetModelList(" ParentOrderId=" + orderInfo.OrderId);

            bool IsSuccess = service.PayForOrder(orderInfo, currentUser);


            //#endregion

            return IsSuccess;
        }

        #endregion

        #region 完成订单

        public static bool CompleteOrder(OrderInfo orderInfo, Accounts.Bus.User currentUser)
        {

              int userId = -1;
            string userName = "";
            string userType = "";
         
            //普通用户/商家 非法完成订单
            if (null != currentUser)
            {
                userId = currentUser.UserID;
                userName = currentUser.UserName;
                userType = currentUser.UserType;
                if (
             (currentUser.UserType == "UU" &&
(orderInfo.OrderStatus != 1 || currentUser.UserID != orderInfo.BuyerID)) ||
             (currentUser.UserType == "SP" &&
              (orderInfo.OrderStatus != 1 || (currentUser.UserID != orderInfo.SellerID && currentUser.UserID != orderInfo.BuyerID)))//买家不是当前用户 卖家也不是当前用户
             )
                {
                    YSWL.MALL.Model.SysManage.ErrorLog model = new YSWL.MALL.Model.SysManage.ErrorLog();
                    model.Loginfo =
                        string.Format("入侵拦截:[非法完成订单][YSWL.MALL.BLL.Shop.Order.OrderManage.CompleteOrder] IP:[{0}]",
                            System.Web.HttpContext.Current.Request.UserHostAddress);
                    model.StackTrace = string.Empty;
                    model.Url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                    YSWL.MALL.BLL.SysManage.ErrorLog.Add(model);
                    return false;
                }
            }

            #region 检查订单状态
            //已发货 时 可以完成
            if (orderInfo.OrderStatus != (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.Handling || orderInfo.ShippingStatus != (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ShippingStatus.Shipped)
            {
                return false;
            }
            #endregion

            if (service.CompleteOrder(orderInfo, userId,userName,userType))
            {
                DataCache.ClearAll();

                #region 购物成长值
                //是否开启会员等级
                bool isEnable = SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
                //购物成长值比例 
                decimal rankScoreRatio = SysManage.ConfigSystem.GetDecimalValueByCache("Shop_ShoppingRankScoreRatio");
                //计算成长值
                int rankScore = (int)(orderInfo.Amount * rankScoreRatio);
                if (isEnable && rankScoreRatio > 0 && rankScore > 0)
                {
                    BLL.Members.RankDetail rankDetailBll = new RankDetail();
                    //增加购物成长值
                    rankDetailBll.AddScoreByBuy(orderInfo.BuyerID, rankScore, string.Format("订单号：{0}", orderInfo.OrderCode));
                }
                #endregion

                //#region 购物返佣金 （转移到支付完 给提成）

                bool isOpen = SysManage.ConfigSystem.GetBoolValueByCache("Shop_Commssion_IsOpen");
                if (isOpen)
                {
                    YSWL.MALL.BLL.Shop.Commission.CommissionPro comProBll = new CommissionPro();
                    comProBll.AddCommission(orderInfo);
                }

                //#endregion



                //#endregion 

                return true;
            }
            return false;
        }

        #endregion

        #region 订单统计
        public static DataSet Stat4OrderStatus(int orderStatus)
        {
            return service.Stat4OrderStatus(orderStatus);
        }
        public static DataSet Stat4OrderStatus(int orderStatus, DateTime startDate, DateTime endDate, int? supplierId = null)
        {
            return service.Stat4OrderStatus(orderStatus, startDate, endDate, supplierId);
        }

        public static DataSet StatSales(Model.Shop.Order.StatisticMode mode, System.DateTime startDate, System.DateTime endDate, int? supplierId = null)
        {
            return service.StatSales(mode, startDate, endDate, supplierId);
        }
        /// <summary>
        /// 订单统计
        /// </summary>
        /// <param name="paymentStatus">支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单</param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static DataSet Stat4OrderStatus(int paymentStatus, DateTime startDate, DateTime endDate, int referType, int? supplierId = null)
        {
            return service.Stat4OrderStatus(paymentStatus, startDate, endDate, referType, supplierId);
        }

        /// <summary>
        /// 客户活跃统计
        /// </summary>
        /// <param name="paymentStatus">支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单</param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static DataSet ActiveCount(int paymentStatus, DateTime startDate, DateTime endDate, int referType, int? supplierId = null, StatisticMode mode = StatisticMode.Day)
        {
            return service.ActiveCount(paymentStatus, startDate, endDate, referType, supplierId, mode);
        }
        /// <summary>
        /// 客户活跃统计--来源类型
        /// </summary>
        /// <param name="paymentStatus">支付状态 -1显示全部| 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static DataSet ActiveCountbyType(int paymentStatus, DateTime startDate, DateTime endDate, int? supplierId = null)
        {
            return service.ActiveCountbyType(paymentStatus, startDate, endDate, supplierId);
        }
        /// <summary>
        /// 统计订单数和销售额
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static DataSet StatOrderCountPrice(StatisticMode mode, DateTime startDate, DateTime endDate, int? supplierId = null)
        {
            return service.StatOrderCountPrice(mode, startDate, endDate, supplierId);
        }
        /// <summary>
        /// 客户数量
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="referType">来源类型 1：表示PC下单，2：表示微信下单，3：表示业务员待下单</param>
        /// <param name="paymentStatus">支付状态 0 未支付 | 1 等待确认 | 2 已支付 | 3 处理中(预留) | 4 支付异常(预留)</param>
        /// <returns></returns>

        public static int GetActiveCount(DateTime startDate, DateTime endDate, int referType, int? paymentStatus = null)
        {
            return service.GetActiveCount(startDate, endDate, referType, paymentStatus);
        }
   
        #endregion

        #region 商品销量排行统计
        public static DataSet ProductSales(Model.Shop.Order.StatisticMode mode, System.DateTime startDate, System.DateTime endDate, int supplierId = 0)
        {
            return service.ProductSales(mode, startDate, endDate, supplierId);
        }
        /// <summary>
        /// 商品销量排行 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public static DataSet ProductSaleInfo(DateTime startDate, DateTime endDate, int topCount)
        {
            return service.ProductSaleInfo(startDate, endDate, topCount);
        }
        public static List<YSWL.MALL.ViewModel.Order.OrderInfoExPage> GetListByPageEx(StatisticMode mode, DateTime startDate, DateTime endDate, string modes, int startIndex, int endIndex, int supplierId = 0)
        {
            DataSet ds = GetListByPage(mode, startDate, endDate, modes, startIndex, endIndex, supplierId);
            return DataTableToListEx(ds.Tables[0]);
        }
        public static List<YSWL.MALL.ViewModel.Order.OrderInfoExPage> DataTableToListEx(DataTable dt)
        {
            List<YSWL.MALL.ViewModel.Order.OrderInfoExPage> modelList = new List<YSWL.MALL.ViewModel.Order.OrderInfoExPage>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                YSWL.MALL.ViewModel.Order.OrderInfoExPage model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;

        }
        public static YSWL.MALL.ViewModel.Order.OrderInfoExPage DataRowToModel(DataRow row)
        {
            YSWL.MALL.ViewModel.Order.OrderInfoExPage model = new YSWL.MALL.ViewModel.Order.OrderInfoExPage();
            if (row != null)
            {
                if (row["GeneratedDate"] != null && row["GeneratedDate"].ToString() != "")
                {
                    model.GeneratedDate = DateTime.Parse(row["GeneratedDate"].ToString());
                }
                if (row["Product"] != null && row["Product"].ToString() != "")
                {
                    model.Product = int.Parse(row["Product"].ToString());
                }
                if (row["ToalQuantity"] != null && row["ToalQuantity"].ToString() != "")
                {
                    model.ToalQuantity = int.Parse(row["ToalQuantity"].ToString());
                }
                if (row["ProductName"] != null && row["ProductName"].ToString() != "")
                {
                    model.ProductName = row["ProductName"].ToString();
                }
            }
            return model;
        }
        public static DataSet GetListByPage(StatisticMode mode, DateTime startDate, DateTime endDate, string modes, int startIndex, int endIndex, int supplierId = 0)
        {
            return service.GetListByPage(mode, startDate, endDate, modes, startIndex, endIndex, supplierId);
        }
        public static int GetRecordCount(StatisticMode mode, DateTime startDate, DateTime endDate, string modes, int startIndex, int endIndex, int supplierId = 0)
        {
            return service.GetRecordCount(mode, startDate, endDate, modes, startIndex, endIndex, supplierId);
        }
        #endregion

        #region 店铺排行
        public static DataSet ShopSale(StatisticMode mode, DateTime startDate, DateTime endDate)
        {
            return service.ShopSale(mode, startDate, endDate);
        }
        public static DataSet ShopSaleInfo(StatisticMode mode, DateTime startDate, DateTime endDate)
        {
            return service.ShopSaleInfo(mode, startDate, endDate);
        }
        /// <summary>
        /// 品牌排行
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="topCount">前几条</param>
        /// <returns></returns>
        public static DataSet BrandSaleInfo(DateTime startDate, DateTime endDate, int topCount)
        {
            return service.BrandSaleInfo(startDate, endDate, topCount);
        }
        #endregion


        #region 填充卖家信息

        public static bool FillSellerInfo(OrderInfo orderInfo)
        {
            if (orderInfo.SupplierId.HasValue && orderInfo.SupplierId.Value > 0)
            {
                BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
                Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(orderInfo.SupplierId.Value);
                if (supplierInfo == null) return false;

                orderInfo.SupplierName = supplierInfo.Name;
                orderInfo.OrderTypeSub = 3;//B类订单
                #region 填充子单卖家信息

                orderInfo.SellerID = supplierInfo.UserId;
                //DONE: 卖家名称使用店铺名称
                orderInfo.SellerName = supplierInfo.ShopName;
                orderInfo.SellerEmail = supplierInfo.ContactMail;
                orderInfo.SellerCellPhone = supplierInfo.CellPhone;

                #endregion
            }
            else
            {
                if (orderInfo.SubOrders.Count < 2)
                {
                    orderInfo.OrderTypeSub = 1; //A类订单
                }
                else
                {
                    //多个订单 但是没有平台的订单
                    var sbOrders = orderInfo.SubOrders.FirstOrDefault(o => o.SupplierId == null || o.SupplierId.Value <= 0);
                    if (sbOrders == null)
                    {
                        orderInfo.OrderTypeSub = 3; //B类订单
                    }
                    else
                    {
                        orderInfo.OrderTypeSub = 2;
                    }
                }

                orderInfo.SupplierId = null;
                orderInfo.SupplierName = null;

                orderInfo.SellerID = null;
                orderInfo.SellerName = null;
                orderInfo.SellerEmail = null;
                orderInfo.SellerCellPhone = null;
            }
            return true;
        }

        #endregion


        #region 取消订单数
        public static DataSet CancleData(string startDate, string endDate, int referType = -1)
        {
            return service.CancleData(startDate, endDate, referType);
        }
        #endregion

        #region 未绑定数
        public static DataSet GetNoBindData(DateTime startDate, DateTime endDate)
        {
            return service.GetNoBindData(startDate, endDate);
        }
        #endregion

        #region 绑定错误数

        public static DataSet GetErrBindData(DateTime startDate, DateTime endDate)
        {
            return service.GetErrBindData(startDate, endDate);
        }

        #endregion

        #region 业务员新增客户数

        public static List<YSWL.MALL.ViewModel.Order.CustCount> SalesNewCustoms(DateTime startDay,
                                                                                    DateTime endDay)
        {
            DataSet ds = service.SalesNewCustoms(startDay, endDay);
            int rowsCount = ds.Tables[0].Rows.Count;
            List<YSWL.MALL.ViewModel.Order.CustCount> orderCountList = new List<ViewModel.Order.CustCount>();
            if (rowsCount > 0)
            {
                var dt = ds.Tables[0];
                YSWL.MALL.ViewModel.Order.CustCount model = null;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.ViewModel.Order.CustCount();
                    if (dt.Rows[n]["ReferID"] != null && dt.Rows[n]["ReferID"].ToString() != "")
                    {
                        model.SalesId = Int32.Parse(dt.Rows[n]["ReferID"].ToString());
                    }
                    if (dt.Rows[n]["Count"] != null && dt.Rows[n]["Count"].ToString() != "")
                    {
                        model.Count = Common.Globals.SafeInt(dt.Rows[n]["Count"], 0);
                    }
                    orderCountList.Add(model);
                }
                return orderCountList;
            }
            return null;
        }
        #endregion

        #region 总的新增客户数
        public static int GetTotalCustoms(DateTime startDay, DateTime endDay)
        {
            return service.GetTotalCustoms(startDay, endDay);
        }
        #endregion

        #region 业务员活跃客户数
        public static List<YSWL.MALL.ViewModel.Order.ActiveCount> SalesActiveCount(DateTime startDay,
                                                                                DateTime endDay)
        {
            DataSet ds = service.SalesActiveCount(startDay, endDay);
            int rowsCount = ds.Tables[0].Rows.Count;
            List<YSWL.MALL.ViewModel.Order.ActiveCount> orderCountList = new List<ViewModel.Order.ActiveCount>();
            if (rowsCount > 0)
            {
                var dt = ds.Tables[0];
                YSWL.MALL.ViewModel.Order.ActiveCount model = null;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.ViewModel.Order.ActiveCount();
                    if (dt.Rows[n]["ReferID"] != null && dt.Rows[n]["ReferID"].ToString() != "")
                    {
                        model.SalesId = Int32.Parse(dt.Rows[n]["ReferID"].ToString());
                    }
                    if (dt.Rows[n]["Count"] != null && dt.Rows[n]["Count"].ToString() != "")
                    {
                        model.Count = Common.Globals.SafeInt(dt.Rows[n]["Count"], 0);
                    }
                    orderCountList.Add(model);
                }
                return orderCountList;
            }
            return null;
        }
        #endregion

        #region 业务员业绩 （订单和销售额）

        public static YSWL.MALL.ViewModel.Order.SalesCount GetSalesCount(int SalesId, string startDay, string endDay)
        {
            DataSet ds = service.GetSalesCount(SalesId, startDay, endDay);
            int rowsCount = ds.Tables[0].Rows.Count;
            YSWL.MALL.ViewModel.Order.SalesCount salesCount = new YSWL.MALL.ViewModel.Order.SalesCount();
            if (rowsCount > 0)
            {
                var dt = ds.Tables[0];
                salesCount = new YSWL.MALL.ViewModel.Order.SalesCount();
                if (dt.Rows[0]["Count"] != null && dt.Rows[0]["Count"].ToString() != "")
                {
                    salesCount.Count = Int32.Parse(dt.Rows[0]["Count"].ToString());
                }
                if (dt.Rows[0]["Amount"] != null && dt.Rows[0]["Amount"].ToString() != "")
                {
                    salesCount.Amount = Common.Globals.SafeDecimal(dt.Rows[0]["Amount"], 0);
                }
                return salesCount;
            }
            return null;
        }

        public static List<YSWL.MALL.ViewModel.Order.DayCount> GetOrderSales(int SalesId, string startDay, string endDay, int dateType = 0)
        {
            DataSet ds = service.GetOrderSales(SalesId, startDay, endDay, dateType);
            int rowsCount = ds.Tables[0].Rows.Count;
            List<YSWL.MALL.ViewModel.Order.DayCount> salesCount = new List<YSWL.MALL.ViewModel.Order.DayCount>();
            if (rowsCount > 0)
            {
                var dt = ds.Tables[0];
                YSWL.MALL.ViewModel.Order.DayCount model = null;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.ViewModel.Order.DayCount();
                    if (dt.Rows[n]["D"] != null && dt.Rows[n]["D"].ToString() != "")
                    {
                        model.DateStr = dt.Rows[n]["D"].ToString();
                    }
                    if (dt.Rows[n]["Count"] != null && dt.Rows[n]["Count"].ToString() != "")
                    {
                        model.Count = Common.Globals.SafeInt(dt.Rows[n]["Count"], 0);
                    }
                    if (dt.Rows[n]["Amount"] != null && dt.Rows[n]["Amount"].ToString() != "")
                    {
                        model.Amount = Common.Globals.SafeDecimal(dt.Rows[n]["Amount"], 0);
                    }
                    salesCount.Add(model);
                }
                return salesCount;
            }
            return null;
        }

        #endregion

        #region 加盟商业绩

        public static YSWL.MALL.ViewModel.Order.SalesCount GetShipsCount(int modeId, string startDay, string endDay, int type = 1)
        {
            DataSet ds = service.GetShipsCount(modeId, startDay, endDay, type);
            int rowsCount = ds.Tables[0].Rows.Count;
            YSWL.MALL.ViewModel.Order.SalesCount salesCount = new YSWL.MALL.ViewModel.Order.SalesCount();
            if (rowsCount > 0)
            {
                var dt = ds.Tables[0];
                salesCount = new YSWL.MALL.ViewModel.Order.SalesCount();
                if (dt.Rows[0]["Count"] != null && dt.Rows[0]["Count"].ToString() != "")
                {
                    salesCount.Count = Int32.Parse(dt.Rows[0]["Count"].ToString());
                }
                if (dt.Rows[0]["Amount"] != null && dt.Rows[0]["Amount"].ToString() != "")
                {
                    salesCount.Amount = Common.Globals.SafeDecimal(dt.Rows[0]["Amount"], 0);
                }
                return salesCount;
            }
            return null;
        }

        public static List<YSWL.MALL.ViewModel.Order.DayCount> GetOrderShips(int modeId, string startDate, string endDate, int type = 1)
        {
            DataSet ds = service.GetOrderShips(modeId, startDate, endDate, type);
            int rowsCount = ds.Tables[0].Rows.Count;
            List<YSWL.MALL.ViewModel.Order.DayCount> salesCount = new List<YSWL.MALL.ViewModel.Order.DayCount>();
            if (rowsCount > 0)
            {
                var dt = ds.Tables[0];
                YSWL.MALL.ViewModel.Order.DayCount model = null;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.ViewModel.Order.DayCount();
                    if (dt.Rows[n]["D"] != null && dt.Rows[n]["D"].ToString() != "")
                    {
                        model.DateStr = dt.Rows[n]["D"].ToString();
                    }
                    if (dt.Rows[n]["Count"] != null && dt.Rows[n]["Count"].ToString() != "")
                    {
                        model.Count = Common.Globals.SafeInt(dt.Rows[n]["Count"], 0);
                    }
                    if (dt.Rows[n]["Amount"] != null && dt.Rows[n]["Amount"].ToString() != "")
                    {
                        model.Amount = Common.Globals.SafeDecimal(dt.Rows[n]["Amount"], 0);
                    }
                    salesCount.Add(model);
                }
                return salesCount;
            }
            return null;
        }

        public static int GetItemsCount(int modeId, string startDay, string endDay, int type = 1)
        {
            return service.GetItemsCount(modeId, startDay, endDay, type);
        }

        public static List<YSWL.MALL.ViewModel.Order.DayCount> GetItemsList(int modeId, string startDay, string endDay, int type = 1)
        {
            DataSet ds = service.GetItemsList(modeId, startDay, endDay, type);
            int rowsCount = ds.Tables[0].Rows.Count;
            List<YSWL.MALL.ViewModel.Order.DayCount> salesCount = new List<YSWL.MALL.ViewModel.Order.DayCount>();
            if (rowsCount > 0)
            {
                var dt = ds.Tables[0];
                YSWL.MALL.ViewModel.Order.DayCount model = null;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new YSWL.MALL.ViewModel.Order.DayCount();
                    if (dt.Rows[n]["D"] != null && dt.Rows[n]["D"].ToString() != "")
                    {
                        model.DateStr = dt.Rows[n]["D"].ToString();
                    }
                    if (dt.Rows[n]["Count"] != null && dt.Rows[n]["Count"].ToString() != "")
                    {
                        model.Count = Common.Globals.SafeInt(dt.Rows[n]["Count"], 0);
                    }
                    salesCount.Add(model);
                }
                return salesCount;
            }
            return null;
        }

        #endregion


        #region  OMS 调用方法
        /// <summary>
        /// 获取备货中的订单列表
        /// </summary>
        /// <param name="maxOrderId"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
        public static List<YSWL.MALL.Model.Shop.Order.OrderInfo> GetPackOrderList(int delayMin, int depotId)
        {
            //是否开启多仓功能
            bool IsOpenMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            DataSet ds = service.GetPackOrderList(delayMin, depotId, IsOpenMultiDepot);
            List<YSWL.MALL.Model.Shop.Order.OrderInfo> orderInfos = DataTableToList(ds.Tables[0]);
            YSWL.MALL.BLL.Shop.Order.OrderItems itemBll = new YSWL.MALL.BLL.Shop.Order.OrderItems();
            YSWL.MALL.BLL.Shop.Order.OrderAction actionBll=new OrderAction();
            foreach (var item in orderInfos)
            {
                item.OrderItems = itemBll.GetModelList(" OrderId=" + item.OrderId);//加载订单项
               // item.OrderActions = actionBll.GetModelList(" OrderId=" + item.OrderId);//加载订单日志
            }
            return orderInfos;
        }
        /// <summary>
        /// OMS审核订单操作
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="depotName"></param>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool CheckOrder(long orderId,string  ordercode, string depotName, int userId, string username)
        {
            return service.CheckOrder(orderId,ordercode, depotName, userId, username);
        }

        /// <summary>
        /// ToList化
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<YSWL.MALL.Model.Shop.Order.OrderInfo> DataTableToList(DataTable dt)
        {
            YSWL.MALL.BLL.Shop.Order.Orders orderBll = new YSWL.MALL.BLL.Shop.Order.Orders();
            return orderBll.DataTableToList(dt);
        }
        /// <summary>
        /// OMS 订单返回的订单备货
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static bool PackingOrder(long orderId)
        {
            return service.PackingOrder(orderId);
        }
        /// <summary>
        /// OMS 订单返回发货操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static  bool ShipOrder(long orderId, decimal freightAdjusted, decimal freightActual, string shipOrderNumber, string expressCompanyName, string expressCompanyAbb, int depotId, string depotName)
        {
            return service.ShipOrder(orderId, freightAdjusted, freightActual, shipOrderNumber, expressCompanyName, expressCompanyAbb, depotId, depotName);
        }
        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool CompleteOrder(long orderId, int userId, string username)
        {
            //获取订单详情
            YSWL.MALL.BLL.Shop.Order.Orders orderBll = new YSWL.MALL.BLL.Shop.Order.Orders();
           YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo=  orderBll.GetModelInfo(orderId);
            if (orderInfo == null)
            {
                return false;
            }

            #region 检查订单状态，防止一个订单完成两次
            if (orderInfo.OrderStatus == (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.Complete)//状态为已完成
            {
                return false;
            }
            #endregion
            bool isSuccess = service.CompleteOrder(orderInfo, userId, username);
            if (isSuccess)
            {
                #region 购物成长值
                //是否开启会员等级
                bool isEnable = SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
                //购物成长值比例 
                decimal rankScoreRatio = SysManage.ConfigSystem.GetDecimalValueByCache("Shop_ShoppingRankScoreRatio");
                //计算成长值
                int rankScore = (int)(orderInfo.Amount * rankScoreRatio);
                if (isEnable && rankScoreRatio > 0 && rankScore > 0)
                {
                    BLL.Members.RankDetail rankDetailBll = new RankDetail();
                    //增加购物成长值
                    rankDetailBll.AddScoreByBuy(orderInfo.BuyerID, rankScore, string.Format("订单号：{0}", orderInfo.OrderCode));
                }
                #endregion

                #region 购物返佣金

                bool isOpen = SysManage.ConfigSystem.GetBoolValueByCache("Shop_Commssion_IsOpen");
                if (isOpen)
                {
                    YSWL.MALL.BLL.Shop.Commission.CommissionPro comProBll = new CommissionPro();
                    comProBll.AddCommission(orderInfo);
                }
                #endregion
            }
            return isSuccess;
        }


        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool CancelOrder(long orderId, int userId, string username,int depotId=-1)
        {
            YSWL.MALL.BLL.Shop.Order.Orders orderBll = new YSWL.MALL.BLL.Shop.Order.Orders();
            YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo = orderBll.GetModelInfo(orderId);
            if (orderInfo == null)
            {
                return false;
            }
            bool isSuccess = service.CancelOrder(orderInfo, userId, username, depotId);
            if (isSuccess)//更新库存缓存 (OMS  接口调用，不在同一个IIS 内，需要远程清空缓存)
            {
                //YSWL.MALL.BLL.Shop.Products.StockHelper.CancelOrderStock(orderInfo);
                //YSWL.MALL.BLL.Shop.Service.CommonHelper.ClearCache();

                foreach (Model.Shop.Order.OrderItems item in orderInfo.OrderItems)
                {
                    string key = string.Format("StockHelper_StockCache_SKU_{0}_DepotId_{1}_SuppID_0-{2}", item.SKU, depotId, ProductKey);
                    dataCache.DeleteCache(key);
                }
               
            }
            return isSuccess;
        }
        /// <summary>
        /// 更新备注
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static bool UpdateRemark(long orderId, string remark)
        {
            return service.UpdateRemark(orderId, remark);
        }


        public static bool FillSellerInfoEx(OrderInfo orderInfo)
        {
            if (orderInfo.SupplierId.HasValue && orderInfo.SupplierId.Value > 0)
            {
                BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
                Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(orderInfo.SupplierId.Value);
                if (supplierInfo == null) return false;

                orderInfo.SupplierName = supplierInfo.Name;
                orderInfo.OrderTypeSub = 3;//B类订单
                #region 填充子单卖家信息

                orderInfo.SellerID = supplierInfo.UserId;
                //DONE: 卖家名称使用店铺名称
                orderInfo.SellerName = supplierInfo.ShopName;
                orderInfo.SellerEmail = supplierInfo.ContactMail;
                orderInfo.SellerCellPhone = supplierInfo.CellPhone;

                #endregion
            }
            else
            {
                if (orderInfo.SubOrders.Count < 2)
                {
                    orderInfo.OrderTypeSub = 1; //A类订单
                }
                else
                {
                    //多个订单 但是没有平台的订单
                    var sbOrders = orderInfo.SubOrders.FirstOrDefault(o => o.SupplierId == null || o.SupplierId.Value <= 0);
                    if (sbOrders == null)
                    {
                        orderInfo.OrderTypeSub = 3; //B类订单
                    }
                    else
                    {
                        orderInfo.OrderTypeSub = 2;
                    }
                }
                orderInfo.SellerID = null;
                orderInfo.SellerName = null;
                orderInfo.SellerEmail = null;
                orderInfo.SellerCellPhone = null;
            }
            return true;
        }
        #endregion
    }
}
