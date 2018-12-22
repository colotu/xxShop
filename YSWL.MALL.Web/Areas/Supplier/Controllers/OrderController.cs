﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Order;
using NPOI.HSSF.UserModel;
using Webdiyer.WebControls.Mvc;

namespace YSWL.MALL.Web.Areas.Supplier.Controllers
{
    public class OrderController : SupplierControllerBase
    {
        //
        // GET: /Shop/SPOrder/

        BLL.Members.PointsDetail pointdetBll = new BLL.Members.PointsDetail();

        BLL.Members.Users userbll = new BLL.Members.Users();

        BLL.Shop.Supplier.SupplierInfo suppinfobll = new BLL.Shop.Supplier.SupplierInfo();
        private readonly BLL.Shop.Order.OrderItems itemBll = new BLL.Shop.Order.OrderItems();
        private readonly BLL.Shop.Order.OrderRemark remarkBll = new BLL.Shop.Order.OrderRemark();
        private YSWL.MALL.BLL.Shop.Shipping.ShippingType typeBll = new YSWL.MALL.BLL.Shop.Shipping.ShippingType();
        private static YSWL.MALL.BLL.Shop.Order.Orders orderBll = new BLL.Shop.Order.Orders();
        private YSWL.MALL.BLL.Shop.Order.OrderAction actionBll = new BLL.Shop.Order.OrderAction();
        #region 订单管理
        #region 订单列表
        public ActionResult Orders(int os = -1, string viewName = "Orders")
        {
            ViewBag.OrderStatus = os;
            return View(viewName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="os"> OrderStatus 订单组合状态</param>
        /// <param name="oc">OrderCode 订单号</param>
        /// <param name="sn">shipName收货人</param>
        /// <param name="bn">buyerName会员名</param>
        /// <param name="pays">PaymentStatus付款状态</param>
        /// <param name="ss">ShippingStatus发货状态</param>
        /// <param name="dr">dateRange下单日期范围</param>
        /// <param name="p">pageIndex 页码</param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult OrderList(int os = -1, string oc = "", string sn = "",
            string bn = "", int pays = -1, int ss = -1, string dr = "", int p = 1, string viewName = "_OrderList")
        {
            ViewBag.IsConnectionOMS = YSWL.MALL.BLL.Shop.Service.CommonHelper.ConnectionOMS();
            int _pageSize = 10;

            //计算分页起始索引
            int startIndex = p > 1 ? (p - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = p * _pageSize;
            int toalCount;//获取总条数 
            List<Model.Shop.Order.OrderInfo> orderList = orderBll.GetListByPage(SupplierId, os
                                               , oc, sn, bn, pays, ss, dr, startIndex,
                                               endIndex, out toalCount);
            if (orderList == null)
                return PartialView(viewName);
            PagedList<Model.Shop.Order.OrderInfo> lists = new PagedList<Model.Shop.Order.OrderInfo>(orderList, p, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        #endregion

        #region 订单详情

        /// <summary>
        ///  订单详情
        /// </summary>
        /// <param name="oi">订单id</param>
        /// <returns></returns>
        public ActionResult OrderShow(int oi = 0, string viewName = "OrderShow")
        {

            OrderInfo model = orderBll.GetModelByCache(oi);
            if (model != null && model.SupplierId == SupplierId)
            {
                YSWL.MALL.ViewModel.Shop.OrderDetailModel ordermodel = new YSWL.MALL.ViewModel.Shop.OrderDetailModel();
                ordermodel.OrderInfo = model;
                ordermodel.ListOrderItems = itemBll.GetModelListByCache(" OrderId=" + oi);
                ordermodel.ListOrderAction = actionBll.GetModelList(" OrderId=" + oi);
                ordermodel.ListOrderRemark = remarkBll.GetModelList(" OrderId=" + oi);
                ViewBag.OrderMainStatus = ((int)orderBll.GetOrderType(model.PaymentGateway, model.OrderStatus, model.PaymentStatus, model.ShippingStatus)).ToString();
                return View(viewName, ordermodel);
            }
            return Content("该信息不存在或者您没有权限访问！");
        }

        #endregion

        #region
        /// <summary>
        /// 配货页面
        /// </summary>
        /// <param name="oi"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult OrderItemInfo(long oi = 0, string viewName = "OrderItemInfo")
        {
            OrderInfo model = orderBll.GetModel(oi);
            if (model != null && model.SupplierId == SupplierId)
            {
                YSWL.MALL.ViewModel.Shop.OrderDetailModel ordermodel = new YSWL.MALL.ViewModel.Shop.OrderDetailModel();
                ordermodel.OrderInfo = model;
                ordermodel.ListOrderItems = itemBll.GetModelListByCache(" OrderId=" + oi);
                return View(viewName, ordermodel);
            }
            return Content("该信息不存在或者您没有权限访问！");
        }
        /// <summary>
        /// 发货页面
        /// </summary>
        /// <param name="oi"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult OrderShip(long oi = 0, string viewName = "OrderShip")
        {
            //加载物流方式
            ViewBag.ShipTypeList = typeBll.GetModelList("");
            OrderInfo model = orderBll.GetModel(oi);
            if (model != null && model.SupplierId == SupplierId)
            {
                YSWL.MALL.ViewModel.Shop.OrderDetailModel ordermodel = new YSWL.MALL.ViewModel.Shop.OrderDetailModel();
                ordermodel.OrderInfo = model;
                ordermodel.ListOrderItems = itemBll.GetModelListByCache(" OrderId=" + oi);
                return View(viewName, ordermodel);
            }
            return Content("该信息不存在或者您没有权限访问！");
        }
        #endregion
        #region ajax
        [HttpPost]
        public ActionResult OrderStatus(FormCollection fm)
        {
            if (!string.IsNullOrWhiteSpace(fm["action"]))
            {
                string action = fm["action"];
                long orderId = Globals.SafeLong(fm["oi"], 0);
                if (orderId <= 0)
                {
                    return Content("NO");
                }
                OrderInfo orderInfo = orderBll.GetModelInfo(orderId);
                if (orderInfo == null || orderInfo.SupplierId != SupplierId)
                {
                    return Content("NOTPERMISSIONS");//没有权限
                }
                switch (action)
                {
                    case "CancelOrder": //取消订单
                        if (BLL.Shop.Order.OrderManage.CancelOrder(orderInfo, CurrentUser))
                        {
                            return Content("OK");
                        }
                        break;
                    case "Success"://完成订单  
                        string orderCode = fm["oc"];
                        if (string.IsNullOrWhiteSpace(orderCode))
                            return Content("NO");
                        
                        if (BLL.Shop.Order.OrderManage.CompleteOrder(orderInfo, CurrentUser))
                        {
                            return Content("OK");
                        }
                        break;
                }

            }
            return Content("NO");
        }
        /// <summary>
        /// 修改订单备注
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateRemark(FormCollection fm)
        {
            long orderid = Globals.SafeLong(fm["oi"], 0);
            if (orderid <= 0)
            {
                return Content("NO");
            }
            string remark = fm["remark"];
            if (orderBll.UpdateOrderRemark(orderid, remark, SupplierId))
            {
                //清除缓存
                orderBll.RemoveModelInfoCache(orderid);
               // BLL.Shop.Service.OMSServiceHelper.UpdateRemark(orderid, InjectionFilter.SqlFilter(remark));
                return Content("OK");
            }
            return Content("NO");
        }
        /// <summary>
        /// 修改收货人信息
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateShip(FormCollection fm)
        {
            long orderid = Globals.SafeLong(fm["oi"], 0);
            int regionId = Globals.SafeInt(fm["rid"], 0);
            string ShipName = InjectionFilter.SqlFilter(fm["sn"]);
            string ShipCellPhone = InjectionFilter.SqlFilter(fm["sc"]);
            string ShipAddress = InjectionFilter.SqlFilter(fm["sa"]);
            string TelPhone = InjectionFilter.SqlFilter(fm["tp"]);
            string ShipZipCode = InjectionFilter.SqlFilter(fm["szc"]);
            if (orderid <= 0 || regionId <= 0 || String.IsNullOrWhiteSpace(ShipName) || String.IsNullOrWhiteSpace(ShipAddress) || String.IsNullOrWhiteSpace(ShipCellPhone))
            {
                return Content("NO");
            }
            YSWL.MALL.Model.Shop.Order.OrderInfo orderModel = orderBll.GetModel(orderid);
            if (orderModel == null)
                return Content("NO");


            orderModel.RegionId = regionId;
            orderModel.ShipRegion = new BLL.Ms.Regions().GetRegionNameByRID(regionId);
            orderModel.ShipName = ShipName;
            orderModel.ShipAddress = ShipAddress;
            orderModel.ShipTelPhone = TelPhone;
            orderModel.ShipCellPhone = ShipCellPhone;
            orderModel.ShipZipCode = ShipZipCode;

            if (orderBll.Update(orderModel))
            {
                YSWL.Common.DataCache.DeleteCache("OrdersModel-" + orderModel.OrderId);
                //加操作日志
                YSWL.MALL.Model.Shop.Order.OrderAction actionModel = new Model.Shop.Order.OrderAction();
                int actionCode = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SellerUpdateShip;
                actionModel.ActionCode = actionCode.ToString();
                actionModel.ActionDate = DateTime.Now;
                actionModel.OrderCode = orderModel.OrderCode;
                actionModel.OrderId = orderModel.OrderId;
                actionModel.Remark = "修改收货信息";
                actionModel.UserId = CurrentUser.UserID;
                actionModel.Username = CurrentUser.NickName;
                actionBll.Add(actionModel);
                //清除缓存
                orderBll.RemoveModelInfoCache(orderModel.OrderId);
                //BLL.Shop.Service.OMSServiceHelper.UpdateShipInfo(orderModel.OrderId, CurrentUser.UserID, CurrentUser.NickName, regionId, ShipName, ShipAddress, TelPhone, ShipCellPhone, ShipZipCode);
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }
        }

        /// <summary>
        /// 配货
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Picking(FormCollection fm)
        {
            long orderid = Globals.SafeLong(fm["oi"], 0);
            if (orderid <= 0)
            {
                return Content("NO");
            }
            OrderInfo orderModel = orderBll.GetModel(orderid);
            if (orderModel == null)
            {
                return Content("NO");
            }
            if (orderModel.ShippingStatus != 0)
            {
                return Content("ISNOTMODIFY");
            }
            //已配货
            orderModel.ShippingStatus = (int)EnumHelper.ShippingStatus.Packing;
            orderModel.OrderStatus = (int)EnumHelper.OrderStatus.Handling;
            
            if (orderBll.Update(orderModel))
            {
                //添加订单日志
                OrderAction actionModel = new OrderAction();
                int actionCode = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SellerPacking;
                actionModel.ActionCode = actionCode.ToString();
                actionModel.ActionDate = DateTime.Now;
                actionModel.OrderCode = orderModel.OrderCode;
                actionModel.OrderId = orderModel.OrderId;
                actionModel.Remark = "配货操作";
                actionModel.UserId = CurrentUser.UserID;
                actionModel.Username = CurrentUser.NickName;
                actionBll.Add(actionModel);
                //BLL.Shop.Service.OMSServiceHelper.SpPickOrder(orderModel.OrderId, CurrentUser.UserID, CurrentUser.NickName);
             
                //清除缓存
                orderBll.RemoveModelInfoCache(orderModel.OrderId);
                return Content("OK");
            }
            return Content("NO");
        }
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Shiped(FormCollection fm)
        {
            long orderid = Globals.SafeLong(fm["oi"], 0);
            int regionId = Globals.SafeInt(fm["rid"], 0);
            string ShipName = InjectionFilter.SqlFilter(fm["sn"]);
            string ShipCellPhone = InjectionFilter.SqlFilter(fm["sc"]);
            string ShipAddress = InjectionFilter.SqlFilter(fm["sa"]);
            string TelPhone = InjectionFilter.SqlFilter(fm["tp"]);
            string ShipZipCode = InjectionFilter.SqlFilter(fm["szc"]);

            if (orderid <= 0 || regionId <= 0 || String.IsNullOrWhiteSpace(ShipName) || String.IsNullOrWhiteSpace(ShipAddress) || String.IsNullOrWhiteSpace(ShipCellPhone))
            {
                return Content("NO");
            }

            string OrderNumber = InjectionFilter.SqlFilter(fm["on"]);
            decimal FreightActual = Globals.SafeDecimal(fm["factual"], 0);
            int modeId = Globals.SafeInt(fm["mi"], 0);
            OrderInfo orderModel = orderBll.GetModel(orderid);
            if (orderModel == null || orderModel.SupplierId != SupplierId)
            {
                return Content("NO");
            }
            if (orderModel.ShippingStatus != 0 && orderModel.ShippingStatus != (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ShippingStatus.Packing)
            {
                return Content("ISNOTMODIFY");
            }
            orderModel.RegionId = regionId;
            orderModel.ShipRegion = new BLL.Ms.Regions().GetRegionNameByRID(regionId);
            orderModel.ShipName = ShipName;
            orderModel.ShipAddress = ShipAddress;
            orderModel.ShipTelPhone = TelPhone;
            orderModel.ShipCellPhone = ShipCellPhone;
            orderModel.ShipZipCode = ShipZipCode;
            YSWL.MALL.Model.Shop.Shipping.ShippingType typeModel = typeBll.GetModelByCache(modeId);
            if (typeModel == null)
            {
                return Content("SHIPPTYPEISNULL");//配送方式不存在
            }
            orderModel.ExpressCompanyName = typeModel.ExpressCompanyName;
            orderModel.ExpressCompanyAbb = typeModel.ExpressCompanyEn;
            orderModel.ShippingModeId = typeModel.ModeId;
            orderModel.ShippingModeName = typeModel.Name;
            orderModel.RealShippingModeName = typeModel.Name;
            orderModel.ShipOrderNumber = OrderNumber;
            //orderModel.FreightAdjusted = FreightAdjusted;
            orderModel.FreightActual = FreightActual;// typeBll.GetFreight(typeModel, Globals.SafeInt(orderModel.Weight, 0));

            //已发货
            orderModel.ShippingStatus = (int)EnumHelper.ShippingStatus.Shipped;
            orderModel.OrderStatus = (int)EnumHelper.OrderStatus.Handling;
           // BLL.Shop.Service.OMSServiceHelper.SpShipOrder(orderModel.OrderId, CurrentUser.UserID, CurrentUser.NickName, orderModel.FreightAdjusted.HasValue?orderModel.FreightAdjusted.Value:0, orderModel.FreightActual.HasValue?orderModel.FreightActual.Value:0, OrderNumber, typeModel.ExpressCompanyName, typeModel.ExpressCompanyEn);


            if (orderBll.UpdateShipped(orderModel))
            {
                //添加订单日志
                OrderAction actionModel = new OrderAction();
                int actionCode = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SellerShipped;
                actionModel.ActionCode = actionCode.ToString();
                actionModel.ActionDate = DateTime.Now;
                actionModel.OrderCode = orderModel.OrderCode;
                actionModel.OrderId = orderModel.OrderId;
                actionModel.Remark = "发货操作";
                actionModel.UserId = CurrentUser.UserID;
                actionModel.Username = CurrentUser.NickName;
                actionBll.Add(actionModel);
 
                //清除缓存
                orderBll.RemoveModelInfoCache(orderModel.OrderId);
                //BLL.Shop.Service.OMSServiceHelper.UpdateShipInfo(orderModel.OrderId, CurrentUser.UserID, CurrentUser.NickName, regionId, ShipName, ShipAddress, TelPhone, ShipCellPhone, ShipZipCode);
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }
        }
        /// <summary>
        /// 计算运费
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Freight(FormCollection fm)
        {
            int modeId = Globals.SafeInt(fm["mid"], 0);
            int weight = Globals.SafeInt(fm["weight"], -1);
            if (modeId <= 0 || weight <= -1)
            {
                return Content("NO");
            }
            Model.Shop.Shipping.ShippingType typeModel = typeBll.GetModelByCache(modeId);

            if (typeModel != null)
            {
                return Content(typeBll.GetFreight(typeModel, Globals.SafeInt(weight, 0)).ToString("F"));
            }
            else
            {
                return Content("0.00");
            }
        }
        #endregion

        #region 获取状态
        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetOrderStatus(int OrderStatus)
        {
            // -4 系统锁定   | -3 后台锁定 | -2 用户锁定 | -1 死单（取消） | 0 未处理 | 1 进行中 |2 完成  
            string str = string.Empty;

            switch (OrderStatus.ToString())
            {
                case "-4":
                    str = "系统锁定";
                    break;
                case "-3":
                    str = "后台锁定";
                    break;
                case "-2":
                    str = "用户锁定";
                    break;
                case "-1":
                    str = "死单（取消）";
                    break;
                case "0":
                    str = "未处理";
                    break;
                case "1":
                    str = "进行中";
                    break;
                case "2":
                    str = "完成";
                    break;
                default:
                    str = "未知状态";
                    break;
            }
            return str;
        }

        /// <summary>
        /// 获取发货状态
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetShippingStatus(object target)
        {
            //  配送状态 0 未发货 | 1 打包中 | 2 已发货 | 3 已确认收货 | 4 拒收退货中 | 5 拒收已退货
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                switch (target.ToString())
                {
                    case "0":
                        str = "未发货";
                        break;
                    case "1":
                        str = "打包中";
                        break;
                    case "2":
                        str = "已发货";
                        break;
                    case "3":
                        str = "已确认收货";
                        break;
                    case "4":
                        str = "拒收退货中";
                        break;
                    case "5":
                        str = "拒收已退货";
                        break;
                    default:
                        str = "未知状态";
                        break;
                }
            }
            return str;
        }

        public static string GetActionCode(string actionCode)
        {
            return BLL.Shop.Order.OrderAction.GetActionCode(actionCode);
        }

        public static string GetOrderType(string paymentGateway, int orderStatus, int paymentStatus, int shippingStatus)
        {
            // 1 等待买家付款   | 2 等待发货 | 3 已发货 | 4 退款中 | 5 成功订单 | 6 已退款 |7 已退货  |8 已关闭  
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(paymentGateway))
            {
                Model.Shop.Order.EnumHelper.OrderMainStatus orderType = orderBll.GetOrderType(paymentGateway,
                                       orderStatus,
                                        paymentStatus,
                                        shippingStatus);
                switch (orderType)
                {
                    //  订单组合状态 1 等待付款   | 2 等待处理 | 3 取消订单 | 4 订单锁定 | 5 等待付款确认 | 6 正在处理 |7 配货中  |8 已发货 |9  已完成
                    case Model.Shop.Order.EnumHelper.OrderMainStatus.Paying:
                        str = "等待付款";
                        break;
                    case Model.Shop.Order.EnumHelper.OrderMainStatus.PreHandle:
                        str = "等待处理";
                        break;
                    case Model.Shop.Order.EnumHelper.OrderMainStatus.Cancel:
                        str = "取消订单";
                        break;
                    case Model.Shop.Order.EnumHelper.OrderMainStatus.Locking:
                        str = "订单锁定";
                        break;
                    case Model.Shop.Order.EnumHelper.OrderMainStatus.PreConfirm:
                        str = "等待付款确认";
                        break;
                    case Model.Shop.Order.EnumHelper.OrderMainStatus.Handling:
                        str = "正在处理";
                        break;
                    case Model.Shop.Order.EnumHelper.OrderMainStatus.Shipping:
                        str = "配货中";
                        break;
                    case Model.Shop.Order.EnumHelper.OrderMainStatus.Shiped:
                        str = "已发货";
                        break;
                    case EnumHelper.OrderMainStatus.Complete:
                        str = "已完成";
                        break;
                    default:
                        str = "未知状态";
                        break;
                }
            }
            return str;
        }

        #endregion

        #region 导出订单
        //-姓名
        //-地址
        //-联系电话
        //-邮编
        //-商品金额
        //-付款金额
        //-运费金额
        //-付款时间
        //-购买商品内容
        //-订单备注
        [HttpPost]
        public ActionResult Export(int os = -1, string oc = "", string sn = "",
            string bn = "", int pays = -1, int ss = -1, string dr = "")
        {
            DataSet dataSet = orderBll.GetList(SupplierId, os
                                               , oc, sn, bn, pays, ss, dr);  
            if (Common.DataSetTools.DataSetIsNull(dataSet))
            {
                 return Content("抱歉, 当前没有可以导出的数据!");
            }
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("订单编号", typeof(string)));
            dataTable.Columns.Add(new DataColumn("姓名", typeof(string)));
            dataTable.Columns.Add(new DataColumn("地址", typeof(string)));
            dataTable.Columns.Add(new DataColumn("联系电话", typeof(string)));
            dataTable.Columns.Add(new DataColumn("邮编", typeof(string)));
            dataTable.Columns.Add(new DataColumn("商品总额", typeof(string)));
            dataTable.Columns.Add(new DataColumn("运费", typeof(string)));
            dataTable.Columns.Add(new DataColumn("订单总额", typeof(string)));
            dataTable.Columns.Add(new DataColumn("优惠金额", typeof(string)));
            dataTable.Columns.Add(new DataColumn("应付金额", typeof(string)));
            dataTable.Columns.Add(new DataColumn("下单时间", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("购买商品内容", typeof(string)));
            dataTable.Columns.Add(new DataColumn("订单备注", typeof(string)));

         
            DataRow tmpRow;
            BLL.Ms.Regions regionBll = new BLL.Ms.Regions();
            string remark;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                tmpRow = dataTable.NewRow();
                tmpRow["订单编号"] = row.Field<string>("OrderCode");
                tmpRow["姓名"] = row.Field<string>("ShipName");
                tmpRow["地址"] = regionBll.GetRegionNameByRID(row.Field<int>("RegionId")) + " " +
                    row.Field<string>("ShipAddress");
                tmpRow["联系电话"] = row.Field<string>("ShipCellPhone");
                tmpRow["邮编"] = row.Field<string>("ShipZipCode");
                tmpRow["商品总额"] = row.Field<decimal>("ProductTotal").ToString("F");
                tmpRow["运费"] = row.Field<decimal>("Freight").ToString("F");
                tmpRow["订单总额"] = row.Field<decimal>("OrderTotal").ToString("F");
                tmpRow["优惠金额"] = (row.Field<decimal>("OrderTotal") - row.Field<decimal>("Amount")).ToString("F");
                tmpRow["应付金额"] = row.Field<decimal>("Amount").ToString("F");
              
                //商品总额, 运费金额, 订单总额, 优惠金额, 应付金额

                //TODO: 没有付款时间, 只有创建时间和最后操作时间
                tmpRow["下单时间"] = row.Field<DateTime>("CreatedDate");

                tmpRow["购买商品内容"] = GetProductContent(row.Field<long>("OrderId"));

                remark = row.Field<string>("Remark") + " | " + row.Field<string>("Remark");
                tmpRow["订单备注"] = remark == " | " ? "" : remark;
                dataTable.Rows.Add(tmpRow);
            }
            MemoryStream stream = DataSetToExcel(dataTable);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/vnd.ms-excel", string.Format("ExportOrder_{0}.xls", DateTime.Now.ToString("yyyy-MM-dd_HHmmss")));
        }

        BLL.Shop.Order.OrderItems orderItemManage = new BLL.Shop.Order.OrderItems();
        private string GetProductContent(long orderId)
        {
            StringBuilder sb = new StringBuilder();
            List<OrderItems> list = orderItemManage.GetModelListByCache(" OrderId=" + orderId);
            if (list == null || list.Count < 1) return string.Empty;
            list.ForEach(info => sb.AppendFormat("{0} x{1} |", info.Name, info.Quantity));
            return sb.ToString().TrimEnd('|');
        }

        private MemoryStream DataSetToExcel(DataTable data)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            string nowDate = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.HSSF.UserModel.HSSFSheet sheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet(
                string.Format("导出订单_{0}",
                nowDate));
            NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(0);
            foreach (DataColumn column in data.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            int rowIndex = 1;
            foreach (DataRow row in data.Rows)
            {
                NPOI.HSSF.UserModel.HSSFRow dataRow = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(rowIndex);
                foreach (DataColumn column in data.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                dataRow = null;
                rowIndex++;
            }
            //自动调整列宽
            for (int i = 0; i < data.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            workbook.Write(ms);
            return ms;
        }

        #endregion

        #endregion

        
        #region  店铺会员管理
        public ActionResult userListByEmpid(int os = -1, string viewName = "userListByEmpid")
        {
            ViewBag.OrderStatus = os;
            return View(viewName);
        }
        /// <summary>
        /// 
        /// </summary>
      
        /// <param name="dr">dateRange下单日期范围</param>
        /// <param name="p">pageIndex 页码</param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult userListByEmpidList(string sEmpid = "", string sn = "", string dr = "", int p = 1, string viewName = "_userListByEmpidList")
        {
            int _pageSize = 10;

            //计算分页起始索引
            int startIndex = p > 1 ? (p - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = p * _pageSize;
            int toalCount;//获取总条数 
                          //List<Model.Members.Users > userlst = userbll.GetListByPage(SupplierId, os , oc, sn, bn, pays, ss, dr, startIndex,endIndex, out toalCount);
            List<Model.Members.Users> userlst = userbll.GetListByPageByEmpid(currentUser.UserID.ToString(),sn,dr, startIndex, endIndex, out toalCount);
            if (userlst == null)
                return PartialView(viewName);
            PagedList<Model.Members.Users> lists = new PagedList<Model.Members.Users>(userlst, p, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        #endregion


        #region 店铺消费积分管理
        public ActionResult SuppPointD(int os = -1, string viewName = "SuppPointD")
        {
            ViewBag.SupsunPoint = suppinfobll.GetModel(SupplierId).RegisteredCapital.ToString();
            return View(viewName);
        }
        /// <summary>
        /// 
        /// </summary>

        /// <param name="dr">dateRange下单日期范围</param>
        /// <param name="p">pageIndex 页码</param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult SuppPointDList(string sEmpid = "", string xfuserid = "", string dr = "", int p = 1, string viewName = "_SuppPointDList")
        {

            if (xfuserid.Length > 0)
            {
                xfuserid= userbll.GetUserIdByUserName(xfuserid).ToString();
            }
            int _pageSize = 10;

            //计算分页起始索引
            int startIndex = p > 1 ? (p - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = p * _pageSize;
            int toalCount;//获取总条数 
                          //List<Model.Members.Users > userlst = userbll.GetListByPage(SupplierId, os , oc, sn, bn, pays, ss, dr, startIndex,endIndex, out toalCount);
            List<Model.Members.PointsDetail> userPointlst = pointdetBll.GetPointListByPageByEmpid(currentUser.UserID.ToString(), xfuserid, dr, startIndex, endIndex, out toalCount);
            if (userPointlst == null)
                return PartialView(viewName);
            PagedList<Model.Members.PointsDetail> lists = new PagedList<Model.Members.PointsDetail>(userPointlst, p, _pageSize, toalCount);
            int xfpoint = 0;
            if (lists.Count > 0)
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    lists[i].ExtData = userbll.GetUserName(lists[i].UserID);
                    xfpoint += lists[i].Score;
                }
            }
            ViewBag.xfsunconnt = xfpoint.ToString();

            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }

        #endregion
    }
}
