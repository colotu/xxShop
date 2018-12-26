using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.MALL.BLL.Shop.Coupon;
using YSWL.MALL.BLL.SysManage;
using YSWL.Components.Setting;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.Web.Components.Setting.Shop;
using YSWL.MALL.ViewModel.Shop;
using YSWL.Json;
using YSWL.MALL.BLL.Shop.Products;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class OrderController : MShopControllerBaseUser
    {
        shopCom spcom = new shopCom();

        private readonly BLL.Shop.Shipping.ShippingType _shippingTypeManage = new BLL.Shop.Shipping.ShippingType();
        private readonly BLL.Shop.Shipping.ShippingAddress _addressManage = new BLL.Shop.Shipping.ShippingAddress();
        private readonly BLL.Shop.Order.Orders _orderManage = new BLL.Shop.Order.Orders();

        private readonly BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage = new BLL.Shop.Shipping.ShippingRegionGroups();
        private readonly BLL.Ms.Regions _regionManage = new BLL.Ms.Regions();
        private readonly BLL.Shop.Shipping.ShippingAddress _shippingAddressManage = new BLL.Shop.Shipping.ShippingAddress();
        private readonly BLL.Members.UsersExp userEXBll = new BLL.Members.UsersExp();
        private readonly BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
        private readonly BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
        private readonly BLL.Shop.Activity.ActivityInfo activInfoBll = new BLL.Shop.Activity.ActivityInfo();
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 通过产品Id提交订单
        /// </summary>
        public ActionResult SubmitOrderByProductId(long productId, int count = 1, int c = 0, int g = 0, string viewName = "SubmitOrder")
        {
            string sku = string.Empty;
            YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
            Model.Shop.Products.SKUInfo skuInfo = null;

            ProductSKUModel prouctsku = skuBll.GetProductSKUInfoByProductId(productId);

            if (prouctsku != null && prouctsku.ListSKUInfos != null && prouctsku.ListSKUInfos.Count > 0)
            {
                skuInfo = prouctsku.ListSKUInfos[0];
                sku = skuInfo.SKU;
            }
            else {
                return new RedirectResult(ViewBag.BasePath);
            }
            return SubmitOrder(sku, count, c, g, viewName);
        }



        #region 提交订单
        /// <summary>
        /// 提交订单
        /// </summary>
        public ActionResult SubmitOrder(string sku, int count = 1, int c = 0, int g = 0, string viewName = "SubmitOrder")
        {
            //手机版由于页面反复跳转, 采用Session保存SKU数据
            if (!string.IsNullOrWhiteSpace(sku))
            {
                Session["SubmitOrder_SKU"] = sku;
                Session["SubmitOrder_COUNT"] = count;
                Session["SubmitOrder_CountDown"] = c;
                Session["SubmitOrder_GroupBuy"] = g;
            }
            else if (!string.IsNullOrWhiteSpace(Session["SubmitOrder_SKU"] as string))
            {
                sku = Session["SubmitOrder_SKU"] as string;
                count = Common.Globals.SafeInt(Session["SubmitOrder_COUNT"], 1);
                c = Common.Globals.SafeInt(Session["SubmitOrder_CountDown"], 0);
                g = Common.Globals.SafeInt(Session["SubmitOrder_GroupBuy"], 0);
            }
            ViewBag.SkuInfo = sku;
            ViewBag.SkuCount = count;
            ViewBag.ProSale = c;
            ViewBag.GroupBuy = g;

            Common.Cookies.setCookie("submitorder_refertype_" + CurrentUser.UserID,
                ((int)YSWL.MALL.Model.Shop.Order.EnumHelper.ReferType.WeChat).ToString(), 1440);

            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = new ShoppingCartInfo();

            if (string.IsNullOrWhiteSpace(sku))
            {
                int userId = currentUser == null ? -1 : currentUser.UserID;
                YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new BLL.Shop.Products.ShoppingCartHelper(userId);
                cartInfo = cartHelper.GetShoppingCart4Selected();
                if (cartInfo != null && cartInfo.Items != null)
                {
                    foreach (var item in cartInfo.Items)
                    {
                        item.Stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(item.SKU, GetRegionId, item.SupplierId);
                        //获取销售状态
                        item.SaleStatus = skuManage.GetSaleStatus(item.SKU);
                    }
                }
            }
            else
            {
                #region 指定SKU提交订单 此功能已投入使用
                //TODO: 未支持多个SKU BEN ADD 2013-06-23
                YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
                YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
                YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo = skuBll.GetModelBySKU(sku);
                if (skuInfo == null)
                {
                    return new RedirectResult("/");
                }

                YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = productBll.GetModel(skuInfo.ProductId);
                if (productInfo == null)
                {
                    return new RedirectResult("/");
                }

                #region 限时抢购
                Model.Shop.Products.ProductInfo proSaleInfo = null;
                if (c > 0)
                {
                    proSaleInfo = productBll.GetProSaleModel(c);
                    if (proSaleInfo == null) return new RedirectResult("/");
                    //活动已过期 重定向到单品页
                    if (DateTime.Now > proSaleInfo.ProSalesEndDate)
                        return RedirectToAction("ProSaleDetail", "Product", new { area = "MShop", id = c });
                }
                #endregion

                #region 团购
                Model.Shop.Products.ProductInfo groupBuyInfo = null;
                if (g > 0)
                {
                    groupBuyInfo = productBll.GetGroupBuyModel(g);
                    if (groupBuyInfo == null) return new RedirectResult("/");

                    //活动已过期 重定向到单品页
                    if (DateTime.Now > groupBuyInfo.GroupBuy.EndDate)
                        return RedirectToAction("GroupBuyDetail", "Product", new { area = "MShop", id = g });
                }
                #endregion

                cartInfo = BLL.Shop.Products.ShoppingCartHelper.GetCartInfo4SKU((currentUser != null ? currentUser.UserID : -1), productInfo, skuInfo, count, proSaleInfo, groupBuyInfo,0,GetRegionId);

                #endregion
            }

            if (cartInfo.Quantity < 1)
                return Redirect(ViewBag.BasePath + "ShoppingCart/CartInfo");

            if (c < 1 && g < 1)    //限时抢购/团购　不参与批销优惠
            {
                #region 批销优惠
                try
                {
                    BLL.Shop.Sales.SalesRuleProduct salesRule = new BLL.Shop.Sales.SalesRuleProduct();
                    cartInfo = salesRule.GetWholeSale(cartInfo);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion
            }


            #region 运费
            //运费由 ShowPayAndShip 输出
            ViewBag.Freight = 0;
            #endregion

            ViewBag.TotalQuantity = cartInfo.Quantity;
            ViewBag.TotalAdjustedPrice = cartInfo.TotalAdjustedPrice;
            ViewBag.ProductTotal = cartInfo.TotalSellPrice;
            //ViewBag.TotalPrice = cartInfo.TotalAdjustedPrice + ViewBag.Freight;
            //促销 - 批销规则优惠
            ViewBag.TotalPromPrice = cartInfo.TotalSellPrice - cartInfo.TotalAdjustedPrice;

            //商城积分
            ViewBag.TotalGwjf = cartInfo.TotalGwjf;

            //我的商城积分账户
            decimal mygwjfnow = spcom.GetPointByUsername(CurrentUser.UserName);

            ViewBag.Mygwjf = mygwjfnow.ToString();

            decimal gwjfkou = cartInfo.TotalGwjf;
            if (cartInfo.TotalGwjf > mygwjfnow)
            {
                gwjfkou = mygwjfnow;//如果商品的商城积分大于会员账户的商城积分，那就扣系统账户的积分
            }

            //实际付款金额
            ViewBag.TotalPrice = cartInfo.TotalAdjustedPrice + ViewBag.Freight - gwjfkou;

            //团购/限时抢购 是否能使用优惠券
            ViewBag.PromotionsIsUseCoupon = BLL.SysManage.ConfigSystem.GetBoolValueByCache("PromotionsIsUseCoupon");
            //是否开启分仓
            ViewBag.IsMultiDepot = IsMultiDepot;

            //是否开启发票项
            ViewBag.IsOpenInvoicesItem = BLL.SysManage.ConfigSystem.GetBoolValueByCache("IsOpenInvoicesItem");



            #region 处理默认的配送方式
            var dic = ShoppingCartHelper.GetSuppCartItems(cartInfo.Items);
            string shipStr = "";
            foreach (var di in dic)
            {
                var typeModel = _shippingTypeManage.GetCacgeModelBySupplied(di.Key);
                if (String.IsNullOrWhiteSpace(shipStr))
                {
                    shipStr = di.Key + "-" + typeModel.ModeId;
                }
                else
                {
                    shipStr = shipStr + "|" + di.Key + "-" + typeModel.ModeId;
                }

            }
            Common.Cookies.setKeyCookie("shipStr", shipStr, 1440);
            #endregion


            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "提交订单 " + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName, cartInfo);
        }

        private void RemoveSubmitSession()
        {
            #region Remove MShop SubmitOrderSession SKU/COUNT/CountDown/GroupBuy
            Session.Remove("SubmitOrder_SKU");
            Session.Remove("SubmitOrder_COUNT");
            Session.Remove("SubmitOrder_CountDown");
            Session.Remove("SubmitOrder_GroupBuy");
            Session.Remove("SubmitOrder_ReferType");
            #endregion
        }

        public ActionResult RemoveSubmitOrderSession()
        {
            RemoveSubmitSession();
            return Content(string.Empty);
        }

        #region SubmitSuccess
        public ActionResult SubmitSuccess(string id, string viewName = "SubmitSuccess")
        {
            if (string.IsNullOrWhiteSpace(id))
                return Redirect("/");

            long orderId = Common.Globals.SafeLong(id, -1);
            if (orderId < 1) return Content("ERROR_NOTSAFEORDERID");
            Model.Shop.Order.OrderInfo orderInfo = _orderManage.GetModel(orderId);
            if (orderInfo == null) return Content("ERROR_NOORDERINFO");

            //Remove MShop SubmitOrderSession SKU/COUNT/CountDown/GroupBuy
            RemoveSubmitSession();

            //Safe
            if (orderInfo.BuyerID != currentUser.UserID) return Redirect(ViewBag.BasePath + "UserCenter/Orders");

            ViewBag.OrderId = orderInfo.OrderId;
            //订单编号
            ViewBag.OrderCode = orderInfo.OrderCode;
            //应付金额
            ViewBag.OrderAmount = orderInfo.Amount;
            ////收货人
            //ViewBag.ShipName = orderInfo.ShipName;
            ////项目数量
            //ViewBag.ItemsCount = _orderItemManage.GetOrderItemCountByOrderId(orderId);

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "订单提交成功 " + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }
        #endregion

        #region SubmitFail 暂未使用
        [System.Obsolete]
        public ActionResult SubmitFail(string id, string viewName = "SubmitFail")
        {
            if (string.IsNullOrWhiteSpace(id))
                return Redirect("/");

            if (!string.IsNullOrWhiteSpace(id))
            {
                long orderId = Common.Globals.SafeLong(id, -1);
                if (orderId < 1) return Content("ERROR_NOTSAFEORDERID");
                Model.Shop.Order.OrderInfo orderInfo = _orderManage.GetModel(orderId);
                if (orderInfo == null) return Content("ERROR_NOORDERINFO");

                Web.LogHelp.AddErrorLog("Shop >> SubmitFail >> OrderId[" + orderId + "] Status[" + orderInfo.OrderStatus + "]",
                    "SubmitOrderFail", "Shop >> OrderController >> SubmitFail");
                ViewBag.OrderId = orderInfo.OrderId;
            }

            return View(viewName);
        }
        #endregion
        #endregion

        [HttpPost]
        //获取要购买的商品的库存
        public void GetBuyProdStock()
        {
            JsonObject result = new JsonObject();

            JsonArray array = new JsonArray();
            JsonObject json;

            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = new ShoppingCartInfo();
            cartInfo = GetCartInfoByProduct();
            if (cartInfo.Quantity < 1)
            {
                result.Accumulate("STATUS", "NO");
                result.Accumulate("DATA", "NODATA");
                Response.Write(result.ToString());
                return;
            }
            if (cartInfo.Items != null && cartInfo.Items.Count > 0)
            {
                cartInfo.Items.ForEach(item =>
                {
                    json = new JsonObject();
                    json.Put("sku", item.SKU);
                    json.Put("quantity", item.Quantity);
                    json.Put("stock", item.Stock);
                    json.Put("salestatus", item.SaleStatus);
                    json.Put("pname", item.Name);
                    json.Put("pic", item.ThumbnailsUrl);
                    array.Add(json);
                });
            }
            result.Accumulate("STATUS", "OK");
            result.Accumulate("DATA", array.ToString());
            Response.Write(result.ToString());
            return;
        } 

        #region 查看订单明细
        /// <summary>
        /// 查看订单明细
        /// </summary>
        public ActionResult OrderInfo(long OrderId = -1, string viewName = "OrderInfo")
        {
            YSWL.MALL.Model.Shop.Order.OrderInfo orderModel = _orderManage.GetModelInfo(OrderId);
            //Safe
            if (orderModel == null ||
                orderModel.BuyerID != currentUser.UserID
                ) return Redirect(ViewBag.BasePath + "UserCenter/Orders");


            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "查看订单详细信息" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            //补全区域地址
            int regionId = orderModel.RegionId;
            orderModel.ShipAddress = _regionManage.GetRegionNameByRID(regionId) + "　" + orderModel.ShipAddress;
            return View(viewName, orderModel);
        }
        #endregion

        #region 收货人

        private int GetShipAddrIdByUserId(int userId)
        {
            List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> listAddress =
                _addressManage.GetModelList(" UserId=" + currentUser.UserID);
            if (listAddress == null || listAddress.Count < 1) return -1;
            return listAddress[0].ShippingId;
        }

        public ActionResult ShowAddress(string viewName = "_ShowAddress")
        {

            List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> listAddress =
                _addressManage.GetDefaultModelList(currentUser.UserID);

            //用户从未设置
            if (listAddress == null || listAddress.Count < 1) return View(viewName);

            //补全区域地址
            listAddress[0].Address = _regionManage.GetRegionNameByRID(listAddress[0].RegionId) + "　" + listAddress[0].Address;

            if (IsMultiDepot)
            {
                //开启了分仓  设置配送地区
                SetRegionId(listAddress[0].RegionId);
            }
          
            //设置

            //团购地区
            #region 团购
            bool isGroupRegionId = true;
            int g = Common.Globals.SafeInt(Session["SubmitOrder_GroupBuy"], 0);//团购
            Model.Shop.Products.ProductInfo groupBuyInfo = null;
            if (g > 0)
            {
                groupBuyInfo = productManage.GetGroupBuyModel(g);
                if (groupBuyInfo != null)
                {
                    //团购可购买
                    if (DateTime.Now <= groupBuyInfo.GroupBuy.EndDate && groupBuyInfo.GroupBuy.BuyCount < groupBuyInfo.GroupBuy.MaxCount)
                    {
                        BLL.Ms.Regions resBll = new BLL.Ms.Regions();
                        isGroupRegionId = resBll.Exists(groupBuyInfo.GroupBuy.RegionId, listAddress[0].RegionId);
                    }
                }
            }
            #endregion
            ViewBag.IsGroupRegionId = isGroupRegionId;

            return View(viewName, listAddress);
        }

        public ActionResult AddressInfo(int id = -1, string viewName = "AddressInfo")
        {
            YSWL.MALL.Model.Shop.Shipping.ShippingAddress addressModel;
            #region TODO 地址同时支持多项选择
            //TODO: 地址同时支持多项选择 BEN ADD 2013-06-21
            //List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> listAddress =
            //    addressBll.GetModelList(" UserId=" + currentUser.UserID);
            #endregion

            if (id > 0)
            {
                addressModel = _addressManage.GetModel(id);
                if (addressModel != null && addressModel.UserId != CurrentUser.UserID)
                {
                    LogHelp.AddInvadeLog(
                        string.Format(
                            "非法获取收货人数据|当前用户:{0}|获取收货地址:{1}|_YSWL.Web.Areas.MShop.Controllers.OrderController.AddressInfo",
                            CurrentUser.UserID, id), System.Web.HttpContext.Current.Request);
                    return View(viewName, new YSWL.MALL.Model.Shop.Shipping.ShippingAddress());
                }
            }
            else
            {
                //默认加载当前用户信息作为收货人
                //TODO: 加载 UserEx 扩展表信息 BEN ADD 2013-06-21
                addressModel = new Model.Shop.Shipping.ShippingAddress
                {
                    ShipName = CurrentUser.TrueName,
                    UserId = CurrentUser.UserID,
                    EmailAddress = CurrentUser.Email,
                    CelPhone = CurrentUser.Phone
                };
            }
            return View(viewName, addressModel);
        }
        [HttpPost]
        public ActionResult SubmitAddressInfo(YSWL.MALL.Model.Shop.Shipping.ShippingAddress model)
        {
            if (model.ShippingId > 0)
            {
                Model.Shop.Shipping.ShippingAddress baseModel = _addressManage.GetModel(model.ShippingId);
                if (baseModel == null)
                {
                    return Content("False");
                }
                baseModel.ShipName = model.ShipName;
                baseModel.RegionId = model.RegionId;
                baseModel.Address = model.Address;
                baseModel.CelPhone = model.CelPhone;
                baseModel.Zipcode = model.Zipcode;

                if (_addressManage.Update(model))
                {
                    Common.Cookies.setCookie("m_so_addrId", model.ShippingId.ToString(), 1440);
                    return Content("True");
                }
            }
            else if (currentUser != null)
            {
                model.UserId = currentUser.UserID;
                if (_addressManage.Add(model) > 0)
                {
                    Common.Cookies.setCookie("m_so_addrId", model.ShippingId.ToString(), 1440);
                    return Content("True");
                }
            }
            return Content("False");
        }
        #endregion

        #region 支付和配送方式 
        #region PayAndShipInfo
        public ActionResult PayAndShipInfo(string viewName = "PayAndShipInfo")
        {
            YSWL.MALL.ViewModel.Shop.PayAndShip payAndShip = new ViewModel.Shop.PayAndShip();
            int payId = Common.Globals.SafeInt(Common.Cookies.getKeyCookie("m_so_payId"),0);
            int shipId = Common.Globals.SafeInt(Common.Cookies.getKeyCookie("m_so_shipId"), 0);
            int addrId = Common.Globals.SafeInt(Common.Cookies.getCookie("m_so_addrId", "value"), 0);

            #region 支付
            //TODO: 未使用缓存 BEN ADD 20130620
            payAndShip.ListPaymentMode = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.Wap);

            if (payId > 0)
            {
                //TODO: 未使用缓存 BEN ADD 20130620
                payAndShip.CurrentPaymentMode = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModeById(payId);
                //根据选择支付获取配送
                payAndShip.ListShippingType = _shippingTypeManage.GetListByPay(payId);
            }
            else if (payAndShip.ListPaymentMode != null && payAndShip.ListPaymentMode.Count > 0)
            {
                //默认当前第一个支付方式
                payAndShip.CurrentPaymentMode = payAndShip.ListPaymentMode[0];
                //获取配送
                payAndShip.ListShippingType = _shippingTypeManage.GetListByPay(
                    payAndShip.CurrentPaymentMode.ModeId);
            }
            else
            {
                payAndShip.CurrentPaymentMode = new Payment.Model.PaymentModeInfo
                {
                    ModeId = -1,
                    Name = "当前网站未设置任何支付方式"
                };
                payAndShip.ListPaymentMode = new List<Payment.Model.PaymentModeInfo>
                {
                    payAndShip.CurrentPaymentMode
                };
            }
            #endregion
 
            #region 配送
            if (shipId > 0)
            {
                payAndShip.CurrentShippingType = _shippingTypeManage.GetModelByCache(shipId);
            }
            else if (payAndShip.ListShippingType != null && payAndShip.ListShippingType.Count > 0)
            {
                //默认当前第一个配送方式
                payAndShip.CurrentShippingType = payAndShip.ListShippingType[0];
            }
            else
            {
                payAndShip.CurrentShippingType = new Model.Shop.Shipping.ShippingType
                {
                    ModeId = -1,
                    Name = "当前支付方式未设置任何配送",
                    Description = "请选择其它支付方式"
                };
                payAndShip.ListShippingType = new List<Model.Shop.Shipping.ShippingType>
                {
                    payAndShip.CurrentShippingType
                };
            }
            #endregion

            ViewBag.PayId = payId;
            ViewBag.ShipId = shipId;
            return View(viewName, payAndShip);
        }
        #endregion

        #region ShowPayAndShip
        public ActionResult ShowPayAndShip(string viewName = "_ShowPayAndShip")
        {
            YSWL.MALL.ViewModel.Shop.PayAndShip payAndShip = new ViewModel.Shop.PayAndShip();

            int payId = Common.Globals.SafeInt(Common.Cookies.getKeyCookie("m_so_payId"), 0);
            int shipId = Common.Globals.SafeInt(Common.Cookies.getKeyCookie("m_so_shipId"), 0); 
            int addrId = Common.Globals.SafeInt(Common.Cookies.getCookie("m_so_addrId", "value"), 0);

            #region 支付
            payAndShip.ListPaymentMode = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.Wap);
            if (payId > 0)
            {
                //TODO: 未使用缓存 BEN ADD 20130620
                payAndShip.CurrentPaymentMode = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModeById(payId);
            }
            else if (payAndShip.ListPaymentMode != null && payAndShip.ListPaymentMode.Count > 0)
            {
                //默认当前第一个支付方式
                payAndShip.CurrentPaymentMode = payAndShip.ListPaymentMode[0];

                //获取配送
                payAndShip.ListShippingType = _shippingTypeManage.GetListByPay(
                    payAndShip.CurrentPaymentMode.ModeId);
            }
            else
            {
                payAndShip.CurrentPaymentMode = new Payment.Model.PaymentModeInfo
                {
                    ModeId = -1,
                    Name = "未选择支付方式"
                };
            }
            #endregion
 
            #region 配送
            if (shipId > 0)
            {
                payAndShip.CurrentShippingType = _shippingTypeManage.GetModelByCache(shipId);
            }
            else if (payAndShip.ListShippingType != null && payAndShip.ListShippingType.Count > 0)
            {
                //默认当前第一个配送方式
                payAndShip.CurrentShippingType = payAndShip.ListShippingType[0];
            }
            else
            {
                payAndShip.CurrentShippingType = new Model.Shop.Shipping.ShippingType
                {
                    ModeId = -1,
                    Name = "未选择配送方式"
                };
            }
            #endregion

            #region 运费
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = GetCartInfoByProduct();
            if (cartInfo == null)
            {
                //终止计算运费, 运费0 在提交订单环节提示用户
                ViewBag.Freight = 0;
                return View(viewName, payAndShip);
            }
            #region 区域差异运费计算

            Model.Shop.Shipping.ShippingAddress shippingAddress = addrId < 1
                ? null
                : _shippingAddressManage.GetModel(addrId);

            //默认收货地址
            if (shippingAddress == null && currentUser != null)
            {
                List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> listAddress = _addressManage.GetDefaultModelList(currentUser.UserID);
                if (listAddress != null && listAddress.Count > 0)
                    shippingAddress = listAddress[0];
            }

            //有收货地址 且 已选择配送 计算差异运费
            if (shippingAddress != null && payAndShip.CurrentShippingType.ModeId > 0 && shippingAddress.RegionId > 0)
            {
                Model.Ms.Regions regionInfo = _regionManage.GetModelByCache(shippingAddress.RegionId);
                Model.Shop.Shipping.ShippingType shippingType = payAndShip.CurrentShippingType;

                int topRegionId;
                if (regionInfo != null)
                {
                    if (regionInfo.Depth > 1)
                    {
                        topRegionId = Common.Globals.SafeInt(regionInfo.Path.Split(new[] { ',' })[1], -1);
                    }
                    else
                    {
                        topRegionId = regionInfo.RegionId;
                    }

                    Model.Shop.Shipping.ShippingRegionGroups shippingRegion =
                        _shippingRegionManage.GetShippingRegion(shippingType.ModeId, topRegionId);
                    ViewBag.Freight = cartInfo.CalcFreight(shippingType, shippingRegion);
                }
            }
            else
            {
                ViewBag.Freight = decimal.Zero;
            }

            #endregion
            #endregion

            return View(viewName, payAndShip);
        }
        #endregion
        #endregion

        #region 优惠券
        public ActionResult ShowCoupon(YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = null, string viewName = "_ShowCoupon")
        {
            string code = "";
            if (System.Web.HttpContext.Current.Request.Cookies["m_so_code"] != null)
            {
                code = Common.InjectionFilter.SqlFilter(System.Web.HttpContext.Current.Request.Cookies["m_so_code"].Value);
            }
            ViewBag.CouponCode = code;
            ViewBag.CouponPrice = "0.00";
            YSWL.MALL.BLL.Shop.Coupon.CouponInfo infoBll = new CouponInfo();
            if (cartInfo == null)
            {
                cartInfo = GetCartInfoByProduct();
            }
            YSWL.MALL.Model.Shop.Coupon.CouponInfo infoModel = infoBll.GetCouponInfo(code);
            if (infoModel != null) {
                int status = infoBll.GetUseStatus(cartInfo, infoModel);
                if (status == 1)
                {
                    ViewBag.CouponPrice = infoModel.CouponPrice.ToString("F");
                    //ViewBag.PriceStr = (cartInfo.TotalAdjustedPrice - infoModel.CouponPrice).ToString("F");
                } 
            }      
            return View(viewName);
        }
        public ActionResult Coupon( string viewName = "Coupon")
        {
            YSWL.MALL.BLL.Shop.Coupon.CouponInfo infoBll = new CouponInfo();
            List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> infoList = infoBll.GetCouponList(currentUser.UserID, 1, false);
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo    cartInfo = GetCartInfoByProduct();
            //判断可用优惠券列表
            List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> resultList = new List<Model.Shop.Coupon.CouponInfo>();
            foreach (var item in infoList)
            {
                int status = infoBll.GetUseStatus(cartInfo, item);
                if (status == 1)
                {
                    resultList.Add(item);
                }
            }
            return View(viewName, resultList);
        }
        public ActionResult AjaxGetCoupon(FormCollection Fm)
        {
            if (!String.IsNullOrWhiteSpace(Fm["ConponCode"]))
            {
                string code = Fm["ConponCode"];
                //decimal basePrice = Common.Globals.SafeDecimal(Fm["BasePrice"], 0);

                YSWL.MALL.BLL.Shop.Coupon.CouponInfo infoBll = new CouponInfo();
                YSWL.MALL.Model.Shop.Coupon.CouponInfo infoModel = infoBll.GetCouponInfo(code);
                if (infoModel == null)
                {
                    return Content("No");
                }
                if (infoModel.UserId > 0 && infoModel.UserId != CurrentUser.UserID)
                {
                    return Content("UserError");
                }
                if (infoModel.EndDate < DateTime.Now)//已过期
                {
                    return Content("CouponExpired");
                }
                if (infoModel.StartDate > DateTime.Now)//未开始
                {
                    return Content("CouponNotStart");
                }
                if (infoModel.Status == 2)
                {
                    return Content("Used");
                }
                //if (infoModel.LimitPrice > basePrice)
                //{
                //    return Content("Limit");
                //}
                ShoppingCartInfo cartInfo = GetCartInfoByProduct();
                int status = infoBll.GetUseStatus(cartInfo, infoModel);
                switch (status)
                {
                    case -1:
                        return Content("HasFrozen");//已冻结
                    case 0:
                        return Content("No");
                    case 1:
                        string priceStr = (cartInfo.TotalAdjustedPrice - infoModel.CouponPrice).ToString("F");
                        return Content(infoModel.CouponPrice.ToString("F") + "|" + priceStr);
                    case 2:
                        return Content("Used");
                    case 3:
                        return Content("Limit");
                    case 4:
                        return Content("CategoryLimit");
                    case 5:
                        return Content("ProductLimit");
                    case 6:
                        return Content("SKULimit");
                    case 7:
                        return Content("CategoryNo");
                    case 8:
                        return Content("ProductNo");
                    case 9:
                        return Content("SKUNo");
                    default:
                        return Content("No");
                }

            }
            return Content("False");
        }
        #endregion

        #region 获取起送价格
        public ActionResult GetSentPrices()
        {
            return Content(ConfigSystem.GetDecimalValueByCache("Opertors_SentPrices").ToString());
        }
        #endregion


        #region  物流信息
        public PartialViewResult ExpressList(long OrderId = -1, string viewName = "_ExpressList")
        {
            List<YSWL.MALL.ViewModel.Shop.Express> expressList = YSWL.MALL.Web.Components.ExpressHelper.GetExpress(OrderId);
            string apiType = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("OpenAPI_Express_ApiType");
            ViewBag.ApiType = apiType;
            if (apiType == "html")
            {
                ViewBag.ExpressUrl = YSWL.MALL.Web.Components.ExpressHelper.GetHtmlExpress(OrderId);
            }
            return PartialView(viewName, expressList);
        }
        #endregion



        #region 获取促销活动赠品
        public ActionResult ActivList(decimal coupPrice = 0, string viewName = "_ActivList")
        {
            string sku = Session["SubmitOrder_SKU"] as string;
            int count = Common.Globals.SafeInt(Session["SubmitOrder_COUNT"], 1);
            int c = Common.Globals.SafeInt(Session["SubmitOrder_CountDown"], 0);
            int g = Common.Globals.SafeInt(Session["SubmitOrder_GroupBuy"], 0);
            ViewBag.IsMerge = ConfigSystem.GetBoolValueByCache("Shop_Activity_IsMerge");
            ViewBag.FreeShippingActiv = false;
            if (c > 0 || g > 0)//限时抢购/团购/组合套装 不参与促销活动
            {
                return View(viewName, null);
            }

            ShoppingCartInfo cartInfo = BLL.Shop.Products.ShoppingCartHelper.GetCartInfoByProduct((currentUser != null ? currentUser.UserID : -1), sku, count, c, g, 0, GetRegionId);
            bool isFreeShippingActiv;//包邮
            ViewModel.Shop.ActicityGiveList model = activInfoBll.GetActivityGiveList(cartInfo,
                                                                                     CurrentUser.UserID, coupPrice,
                                                                                     GetRegionId, out isFreeShippingActiv);
            ViewBag.FreeShippingActiv = isFreeShippingActiv;
            return View(viewName, model);
           // return View(viewName, null);
        }
        #region  获取要购买的商品 组装成购物车对象
        /// <summary>
        /// 获取要购买的商品 组装成购物车对象
        /// </summary>
        /// <returns></returns>
        public ShoppingCartInfo GetCartInfoByProduct()
        {
            string sku = Session["SubmitOrder_SKU"] as string;
            int count = Common.Globals.SafeInt(Session["SubmitOrder_COUNT"], 1);
            int c = Common.Globals.SafeInt(Session["SubmitOrder_CountDown"], 0);
            int g = Common.Globals.SafeInt(Session["SubmitOrder_GroupBuy"], 0);
            ShoppingCartInfo cartInfo = BLL.Shop.Products.ShoppingCartHelper.GetCartInfoByProduct((currentUser!=null?  currentUser.UserID:-1), sku, count, c, g,0,0,GetRegionId);
            return cartInfo;
        }
        #endregion

        #endregion

 
}
}
