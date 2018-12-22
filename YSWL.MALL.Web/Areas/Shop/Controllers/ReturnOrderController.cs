using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Web.Mvc;
using YSWL.Accounts.Bus;
using YSWL.MALL.BLL.Shop.Gift;
using YSWL.MALL.BLL.Shop.Order;
using YSWL.Common;
using YSWL.Components.Setting;
using YSWL.Json;
using YSWL.Json.Conversion;
using YSWL.MALL.Model.Shop;
using YSWL.MALL.Model.Shop.Coupon;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.Web.Components.Setting.Shop;
using Webdiyer.WebControls.Mvc;
using System.Linq;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public class ReturnOrderController : ShopControllerBaseUser
    {
        private BLL.Shop.ReturnOrder.ReturnOrders retuOrderBll = new BLL.Shop.ReturnOrder.ReturnOrders();
        private BLL.Shop.ReturnOrder.ReturnOrderItems retuOrderItemsBll = new BLL.Shop.ReturnOrder.ReturnOrderItems();
        private  BLL.Shop.ReturnOrder.ReturnOrderAction returnactionBll = new BLL.Shop.ReturnOrder.ReturnOrderAction();
        private readonly Orders _orderManage = new Orders();
 
        #region 订单列表
        public ActionResult Index(string viewName = "Index")
        {
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "申请退换货" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }
        public PartialViewResult OrderList(int pageIndex = 1, string viewName = "_OrderList")
        {
            Orders orderBll = new Orders();
            BLL.Shop.Order.OrderItems itemBll = new BLL.Shop.Order.OrderItems();

            int _pageSize =8;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = 0;

            string where = " BuyerID=" + CurrentUser.UserID +
#if true //方案二 统一提取主订单, 然后加载子订单信息 在View中根据订单支付状态和是否有子单对应展示
                //主订单
                           " AND OrderType=1";
#else   //方案一 提取数据时 过滤主/子单数据 View中无需对应 [由于不够灵活此方案作废]
                           //主订单 无子订单
                           " AND ((OrderType = 1 AND HasChildren = 0) " +
                           //子订单 已支付 或 货到付款/银行转账 子订单
                           "OR (OrderType = 2 AND (PaymentStatus > 1 OR (PaymentGateway='cod' OR PaymentGateway='bank')) ) " +
                           //主订单 有子订单 未支付的主订单 非 货到付款/银行转账 子订单
                           "OR (OrderType = 1 AND HasChildren = 1 AND PaymentStatus < 2 AND PaymentGateway<>'cod' AND PaymentGateway<>'bank'))";
#endif

            //获取总条数
            toalCount = orderBll.GetRecordCount(where);
            if (toalCount < 1)
            {
                return PartialView(viewName);//NO DATA
            }
            List<OrderInfo> orderList = orderBll.GetListByPageEX(where, "", startIndex, endIndex);
            if (orderList != null && orderList.Count > 0)
            {
                foreach (OrderInfo item in orderList)
                {
                    //有子订单 已支付 或 货到付款/银行转账 子订单 - 加载子单
                    if (item.HasChildren && (item.PaymentStatus > 1 || (item.PaymentGateway == "cod" || item.PaymentGateway == "bank")))
                    {
                        item.SubOrders = orderBll.GetModelList(" ParentOrderId=" + item.OrderId);
                        item.SubOrders.ForEach(
                            info => info.OrderItems = itemBll.GetModelList(" OrderId=" + info.OrderId));
                    }
                    else
                    {
                        item.OrderItems = itemBll.GetModelList(" OrderId=" + item.OrderId);
                    }
                }
            }
            PagedList<OrderInfo> lists = new PagedList<OrderInfo>(orderList, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
                return PartialView(viewName, lists);
            return PartialView(viewName, lists);
        }
        #endregion

        #region 申请
        public ActionResult Apply(long Id=0,string viewName = "Apply")
        {
            OrderInfo orderModel = _orderManage.GetModelInfo(Id);
            //Safe
            if (orderModel == null ) return Redirect(ViewBag.BasePath + "ReturnOrder/Index");
            if ( !retuOrderBll.IsMeetCondition(orderModel, CurrentUser.UserID))
            {
                return Redirect(ViewBag.BasePath + "ReturnOrder/Index");
            }

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "退货申请" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName, orderModel);
        }
        #endregion

        #region Ajax方法
        [HttpPost]//判断是否符合退单的条件
        public ActionResult AjaxRetuOrder(FormCollection Fm)
        {
            long orderId = Globals.SafeLong(Fm["oi"], 0);
            int orderStates = Globals.SafeInt(Fm["os"], 0);
            if (retuOrderBll.IsMeetCondition(orderId, orderStates))
            {
                return Content("True");
            }
            return Content("False");
        }

        public ActionResult AjaxRetuApply(FormCollection fm)
        {
            #region 收集数据
            string Items = fm["Items"];
            long oId = Common.Globals.SafeLong(fm["oId"], 0);
            int serviceType = Common.Globals.SafeInt(fm["ServiceType"], 0);
            string content = Common.InjectionFilter.SqlFilter(fm["Content"]);
            int pickType = Common.Globals.SafeInt(fm["PickType"], 0);
            int regionId = Common.Globals.SafeInt(fm["RegionId"], 0);
            string address = Common.InjectionFilter.SqlFilter(fm["Address"]);
            string name = Common.InjectionFilter.SqlFilter(fm["Name"]);
            string phone = Common.InjectionFilter.SqlFilter(fm["Phone"]);
            int rgoodsType = Common.Globals.SafeInt(fm["RGoodsType"], (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.Part);//退货类型  
            string imagesurlPath = Globals.SafeString(fm["ImagesurlPath"],"");
            string imagesurlName = Globals.SafeString(fm["ImagesurlName"], "");

            #endregion
            if (rgoodsType == (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.Part)
            {
                if (String.IsNullOrWhiteSpace(Items))
                {
                    return Content("False");
                }
            }
            if (oId <= 0 || serviceType <= 0 || pickType < 0 || String.IsNullOrWhiteSpace(content) ||  String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(phone))
            {
                return Content("False");
            }
            OrderInfo orderModel = _orderManage.GetModelInfo(oId);//订单
            if (orderModel == null || orderModel.HasChildren)
            {
                return Content("False");
            }
            if (!retuOrderBll.IsMeetCondition(orderModel, CurrentUser.UserID))
            {
                return Content("IsNotMeetCondition");//不满足退单条件 
            }
            Model.Shop.ReturnOrder.ReturnOrders returnModel = new Model.Shop.ReturnOrder.ReturnOrders();
            List<Model.Shop.ReturnOrder.ReturnOrderItems> returnItems = new List<Model.Shop.ReturnOrder.ReturnOrderItems>();
            Model.Shop.ReturnOrder.ReturnOrderItems tmpItem;
           
            #region 填充退单主表
            returnModel.OrderId = orderModel.OrderId;
            returnModel.OrderCode = orderModel.OrderCode;
            returnModel.SupplierId = orderModel.SupplierId.HasValue ? orderModel.SupplierId.Value : 0;
            returnModel.SupplierName = orderModel.SupplierName;

            returnModel.Status = ((int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.Status.UnHandle);
            returnModel.RefundStatus = ((int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.RefundStatus.UnRefund);
            returnModel.ContactName = name;
            returnModel.ContactPhone = phone;
            returnModel.ReturnUserId = CurrentUser.UserID;
            returnModel.ReturnUserName = CurrentUser.UserName;
            returnModel.CreateUserId = CurrentUser.UserID;
            returnModel.CreatedDate = DateTime.Now;
            returnModel.CustomerReview = 0;
            returnModel.Description = content;
            returnModel.LogisticStatus = ((int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.LogisticStatus.UnPick);
            returnModel.ReturnType = pickType;
            returnModel.ServiceType = serviceType;
            returnModel.PickRegionId = regionId;
            returnModel.ReturnOrderCode = returnModel.ReturnOrderPrefix + returnModel.CreatedDate.ToString("yyyyMMddHHmmssfff");
            returnModel.ReturnGoodsType = rgoodsType;

            if (regionId > 0)
            {
                BLL.Ms.Regions regionsManage = new BLL.Ms.Regions();
                returnModel.PickRegion = regionsManage.GetFullNameById4Cache(regionId);
            }
            returnModel.PickAddress = address;
            #endregion

            if (orderModel.OrderItems == null || orderModel.OrderItems.Count <= 0)
            {
                return Content("False");
            }
            
            #region 金额
            decimal ActualSalesTotal = 0;//商品实际出售总价
            decimal Amount = 0;//实付金额
            decimal AmountAdjusted = 0;//应退金额
            #endregion

            switch (rgoodsType) { 
                case (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.Part:
                    #region 部分退
                long itemId = 0;
                int count = 0;
                JsonArray jsonArray = JsonConvert.Import<JsonArray>(Items);

                if (jsonArray == null)
                {
                    return Content("False");                    
                }
                ///退单商品数量(不含赠品)
                int returnProdCount = 0;
                ///订单商品数量(不含赠品)
                int orderProdCount = 0; 
                #region 匹配退单项并添加
                foreach (JsonObject jsonObject in jsonArray)
                {
                    itemId = Common.Globals.SafeLong(jsonObject["itemId"], 0);
                    count = Common.Globals.SafeInt(jsonObject["count"], 0);
                    if (itemId <= 0 || count <= 0)
                    {
                        return Content("False");
                    }

                    //安全检测
                    foreach (Model.Shop.ReturnOrder.ReturnOrderItems item in returnItems)
                    {
                        if (item.ItemId == itemId)
                        {
                            return Content("Illegal");//数据重复，属于非法请求
                        }                   
                    }

                    foreach (Model.Shop.Order.OrderItems item in orderModel.OrderItems)
                    {
                        if (item.ItemId != itemId)
                        {
                            continue;
                        }
                        if (item.Quantity < count)
                        {
                            return Content("COUNTISTOOBIG");//数量超过购买数量
                        }
                        //添加退货项
                        tmpItem = retuOrderItemsBll.GetReturnItemInfo(item, count, returnModel.ReturnOrderCode);
                        returnItems.Add(tmpItem);
                        ActualSalesTotal += item.AdjustedPrice*count;
                        AmountAdjusted += item.AdjustedPrice*count;
                        returnProdCount += count;
                        break;
                    }
                }
                #endregion

                #region 添加赠品
                foreach (Model.Shop.Order.OrderItems item in orderModel.OrderItems)
                {   
                    if (item.ProductType != 2)
                    {
                        orderProdCount += item.ShipmentQuantity;
                        continue;
                    }
                    tmpItem = retuOrderItemsBll.GetReturnItemInfo(item, item.ShipmentQuantity, returnModel.ReturnOrderCode);
                    returnItems.Add(tmpItem);
                    //ActualSalesTotal += item.AdjustedPrice*item.ShipmentQuantity;
                    //AmountAdjusted += item.AdjustedPrice*item.ShipmentQuantity; 
                }
                #endregion

                if (orderProdCount == returnProdCount) {
                    return Content("SELECTALLITEMS");//使用部分退时选中了全部商品，应选择整单退
                }
                
               #region 得到实付金额
                    //由于优惠劵可能会指定到某一分类或者某一商品,这时就应根据指定的分类或商品来均分，因此先注释掉这个算法，,先赋值为0，由审核人员自己核算，以后再修改,
                //if (!String.IsNullOrWhiteSpace(orderModel.CouponCode) && orderModel.CouponAmount.HasValue)
                //{
                //    //原订单使用了优惠劵，减去优惠劵的钱(均分), 得到实付金额
                //    Amount = ActualSalesTotal - ((orderModel.CouponAmount.Value / orderProdCount) * returnProdCount);
                //}else
                //{
                //Amount = ActualSalesTotal;
                //}
                Amount = 0;
                AmountAdjusted = 0;
                #endregion

                #endregion
                    break;
               case (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.All:
                    Amount = orderModel.Amount;
                    AmountAdjusted = orderModel.Amount;     
                    #region 整单退
                    foreach (Model.Shop.Order.OrderItems item in orderModel.OrderItems)
                    {
                            //添加退货项
                        tmpItem = retuOrderItemsBll.GetReturnItemInfo(item, item.ShipmentQuantity, returnModel.ReturnOrderCode);
                            returnItems.Add(tmpItem);
                            ActualSalesTotal +=  item.AdjustedPrice*item.ShipmentQuantity;
                    }
                    #endregion
                    break;
                default:
                    return Content("False");
            }

            #region 优惠劵信息
            returnModel.CouponAmount = orderModel.CouponAmount;
            returnModel.CouponCode = orderModel.CouponCode;
            returnModel.CouponName = orderModel.CouponName;
            returnModel.CouponValue = orderModel.CouponValue;
            returnModel.CouponValueType = orderModel.CouponValueType;
            #endregion

            if (returnItems == null || returnItems.Count<=0)
            {
                return Content("ITEMSISNULL");//项为空
            }
            //if (Amount <= 0) {
            //    return Content("AMOUNTISNULL");//总金额小于=0
            //}
            returnModel.Items = returnItems;
            returnModel.ActualSalesTotal = ActualSalesTotal;
            returnModel.Amount = Amount;
            returnModel.AmountAdjusted = AmountAdjusted;
            returnModel.AmountActual = 0;

            #region 移动文件
              if (!String.IsNullOrWhiteSpace(imagesurlPath) && !String.IsNullOrWhiteSpace(imagesurlName))
                    {
                        //创建文件夹  移动文件
                        string path = string.Format("/Upload/Shop/ReturnOrder/{0}/", DateTime.Now.ToString("yyyyMM"));
                        string mapPath = Request.MapPath(path);
                        if (!Directory.Exists(mapPath))
                        {
                            Directory.CreateDirectory(mapPath);
                        }
                        string[] pathArr = imagesurlPath.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] namesArr = imagesurlName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        if (pathArr.Length != namesArr.Length)
                        {
                            throw new ArgumentOutOfRangeException("路径与文件名长度不匹配！");
                        }
                        for (int i = 0; i < pathArr.Length; i++)
                        {
                            System.IO.File.Move(Request.MapPath(pathArr[i]), mapPath + namesArr[i]);
                            
                        }
                        returnModel.ImageUrl = path + string.Join("|" + path, namesArr);
                    }
            #endregion


            if (retuOrderBll.CreateReturnOrder(returnModel,CurrentUser) > 0)
            {
                return Content("True");
            }
            else {
                return Content("Fail");
            }   
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult AjaxCancelRetu(FormCollection Fm)
        {
            long rId = Globals.SafeLong(Fm["RId"], 0);
            Model.Shop.ReturnOrder.ReturnOrders model = retuOrderBll.GetModelByCache(rId);
            //安全处理
            if (model == null || model.ReturnUserId!=CurrentUser.UserID  || model.Status!=(int)Model.Shop.ReturnOrder.EnumHelper.Status.UnHandle) {
                return Content("False");
            }
            return retuOrderBll.CancelReturnOrder(model.ReturnOrderId,model.ReturnOrderCode, CurrentUser.UserID)? Content("True") :Content("False");
        }
        #endregion
 
        #region 列表
        public ActionResult Return(string viewName = "Return") 
        {
            return View(viewName);
        }
        public PartialViewResult ReturnList(int pageIndex = 1,string viewName = "_ReturnList")
        {
            int _pageSize = 8;

            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount ;
            List<Model.Shop.ReturnOrder.ReturnOrders> list = retuOrderBll.GetListByPage(CurrentUser.UserID, startIndex, endIndex, out toalCount);
            if (toalCount <=0)
            {
                return PartialView(viewName);
            }
            foreach (var item in list)
            {
                item.Items = retuOrderItemsBll.GetModelList(item.ReturnOrderId);
                item.MainStatusStr = YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrders.GetMainStatusStr(item.Status, item.LogisticStatus, item.RefundStatus);
            }
            PagedList<Model.Shop.ReturnOrder.ReturnOrders> pagelist = new PagedList<Model.Shop.ReturnOrder.ReturnOrders>(list, pageIndex, _pageSize, toalCount);
            return PartialView(viewName, pagelist);
        }
        #endregion


        #region  详情
        public ActionResult ReturnInfo(long retrunId=0,string viewName="ReturnInfo") 
        {
            Model.Shop.ReturnOrder.ReturnOrders model = retuOrderBll.GetModelInfo(retrunId);
            //安全
            if (model == null || model.ReturnUserId != CurrentUser.UserID) {
                return Redirect(ViewBag.BasePath + "ReturnOrder/Return");
            }
            model.ServiceTypeStr = retuOrderBll.GetServiceTypeStr(model.ServiceType);
            model.ReturnTypeStr = retuOrderBll.GetReturnTypeStr(model.ReturnType);
            model.MainStatusStr = YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrders.GetMainStatusStr(model.Status, model.LogisticStatus, model.RefundStatus);
            model.ReturnGoodsTypeStr = retuOrderBll.GetReturnGoodsTypeStr(model.ReturnGoodsType);
            return View(viewName,model);
        }
        #endregion

        public PartialViewResult ActionList(long retrunId = 0, string viewName = "_ActionList") 
        {
            List<Model.Shop.ReturnOrder.ReturnOrderAction> actionList = returnactionBll.GetModelList(" ReturnOrderId=" + retrunId);
            return PartialView(viewName, actionList);
        }
    }
}
