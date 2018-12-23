using System;
using System.Collections.Generic;
using System.Web.Mvc;
using YSWL.MALL.BLL.Shop.Coupon;
using YSWL.Components.Setting;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.Model.Shop.Shipping;
using YSWL.MALL.Web.Components.Setting.Shop;
using YSWL.MALL.ViewModel.Shop;
using System.Linq;
using YSWL.MALL.BLL.Shop.PrePro;
using YSWL.Json;
using YSWL.Json.Conversion;
using YSWL.MALL.BLL.Shop.Products;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public partial class OrderController : ShopControllerBaseUser
    {
        shopCom spcom = new shopCom();

        private readonly BLL.Shop.Shipping.ShippingType _shippingTypeManage = new BLL.Shop.Shipping.ShippingType();
        private readonly BLL.Shop.Shipping.ShippingAddress _addressManage = new BLL.Shop.Shipping.ShippingAddress();
        private readonly BLL.Shop.Order.Orders _orderManage = new BLL.Shop.Order.Orders();
        private readonly BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage = new BLL.Shop.Shipping.ShippingRegionGroups();
        private readonly BLL.Ms.Regions _regionManage = new BLL.Ms.Regions();
        private readonly BLL.Shop.Shipping.ShippingAddress _shippingAddressManage = new BLL.Shop.Shipping.ShippingAddress();
        private readonly BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
        private readonly BLL.Shop.Activity.ActivityInfo activInfoBll = new BLL.Shop.Activity.ActivityInfo();
        private readonly YSWL.MALL.BLL.Shop.PrePro.PreOrder preOrderBll = new YSWL.MALL.BLL.Shop.PrePro.PreOrder();

        public ActionResult Index()
        {
            return View();
        }

        #region 提交订单

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
            else
            {
                return new RedirectResult(ViewBag.BasePath);
            }
            int referId = 0;
            if (!String.IsNullOrWhiteSpace(Request.Params["r"]))
            {
                string refer = YSWL.Common.UrlOper.Base64Decrypt(Request.Params["r"]);

                referId = Common.Globals.SafeInt(refer, 0);
            }

            return SubmitOrder(sku, count, c, g, -1, referId, viewName);
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        public ActionResult SubmitOrder(string sku, int count = 1, int c = -1, int g = -1, int a = -1, int r = 0,
            string viewName = "SubmitOrder")
        {
            BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = new ShoppingCartInfo();
            BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
            Common.Cookies.setCookie("submitorder_sku", sku, 1440);
            Common.Cookies.setCookie("submitorder_count", count.ToString(), 1440);
            Common.Cookies.setCookie("submitorder_countdown", c.ToString(), 1440);
            Common.Cookies.setCookie("submitorder_groupbuy", g.ToString(), 1440);
            Common.Cookies.setCookie("submitorder_acceId", a.ToString(), 1440);
            Common.Cookies.setCookie("submitorder_referId", Request.Params["r"], 1440);
            #region 获取商品信息
            #region 获取推广信息

            if (!String.IsNullOrWhiteSpace(Request.Params["r"]))
            {
                string refer = YSWL.Common.UrlOper.Base64Decrypt(Request.Params["r"]);
                r = Common.Globals.SafeInt(refer, 0);
            }
            #endregion
            if (a > 0)
            {
                #region 组合优惠套装
                BLL.Shop.Products.ProductAccessorie prodAcceBll = new BLL.Shop.Products.ProductAccessorie();
                YSWL.MALL.Model.Shop.Products.ProductAccessorie prodAcceModel = prodAcceBll.GetModel(a);
                if (prodAcceModel == null || prodAcceModel.Type != 2) return new RedirectResult("/");
                List<Model.Shop.Products.SKUInfo> skulist = skuManage.GetSKUListByAcceId(a, 0);
                if (skulist == null || skulist.Count < 2) return new RedirectResult("/");//每组商品最少有两条数据
                cartInfo = BLL.Shop.Products.ShoppingCartHelper.GetCartInfo4SKU((currentUser != null ? currentUser.UserID : -1), skulist, prodAcceModel, GetRegionId);
                #endregion
            }
            else if (string.IsNullOrWhiteSpace(sku))
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
                Model.Shop.Products.SKUInfo skuInfo = skuManage.GetModelBySKU(sku);
                if (skuInfo == null) return new RedirectResult("/");

                Model.Shop.Products.ProductInfo productInfo = productManage.GetModel(skuInfo.ProductId);
                if (productInfo == null) return new RedirectResult("/");

                #region 限时抢购
                Model.Shop.Products.ProductInfo proSaleInfo = null;
                if (c > 0)
                {
                    proSaleInfo = productManage.GetProSaleModel(c);
                    if (proSaleInfo == null) return new RedirectResult("/");
                    //活动已过期 重定向到单品页
                    if (DateTime.Now > proSaleInfo.ProSalesEndDate)
                        return RedirectToAction("ProSaleDetail", "Product", new { area = "Shop", id = c });
                }
                #endregion

                #region 团购
                Model.Shop.Products.ProductInfo groupBuyInfo = null;
                if (g > 0)
                {
                    groupBuyInfo = productManage.GetGroupBuyModel(g);
                    if (groupBuyInfo == null) return new RedirectResult("/");

                    //活动已过期   团购数量已达上限  重定向到单品页
                    if (DateTime.Now > groupBuyInfo.GroupBuy.EndDate || groupBuyInfo.GroupBuy.BuyCount >= groupBuyInfo.GroupBuy.MaxCount)
                    {
                        return RedirectToAction("GroupBuyDetail", "Product", new { area = "Shop", id = g });
                    }
                }
                #endregion

                cartInfo = BLL.Shop.Products.ShoppingCartHelper.GetCartInfo4SKU((currentUser != null ? currentUser.UserID : -1), productInfo, skuInfo, count, proSaleInfo, groupBuyInfo, r, GetRegionId);
                #endregion
            }
            
            if (cartInfo.Quantity < 1)
                return Redirect(ViewBag.BasePath + "ShoppingCart/CartInfo");

            if (c < 1 && g < 1 && a < 1)    //限时抢购/团购/组合优惠套装　不参与批销优惠
            {
                #region 批销优惠
                try
                {
                    BLL.Shop.Sales.SalesRuleProduct salesRule = new BLL.Shop.Sales.SalesRuleProduct();
                    cartInfo = salesRule.GetWholeSale(cartInfo);
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                #endregion
            }

            #region 运费
            //运费统一由 ShowPayAndShip 计算
            ViewBag.Freight = 0;
            #endregion

            ViewBag.TotalQuantity = cartInfo.Quantity;
            ViewBag.TotalAdjustedPrice = cartInfo.TotalAdjustedPrice;
            ViewBag.ProductTotal = cartInfo.TotalSellPrice;
            ViewBag.TotalPrice = cartInfo.TotalAdjustedPrice + ViewBag.Freight;

            //商城积分
            ViewBag.GwjfTotal = cartInfo.TotalGwjf;

            //我的商城积分账户
            decimal mygwjfnow = spcom.GetPointByUsername(CurrentUser.UserName);

            ViewBag.Mygwjf = mygwjfnow.ToString();

            decimal gwjfkou = cartInfo.TotalGwjf;
            if (cartInfo.TotalGwjf > mygwjfnow)
            {
                gwjfkou = mygwjfnow;//如果商品的商城积分大于会员账户的积分，那就扣系统账户的积分
            }


            ViewBag.TotalPrice = cartInfo.TotalSellPrice + ViewBag.Freight - gwjfkou;
            //促销 - 批销规则优惠 kmmm
            ViewBag.TotalPromPrice = cartInfo.TotalSellPrice - cartInfo.TotalAdjustedPrice;
            #endregion
            

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "提交订单 " + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            //团购/限时抢购 是否能使用优惠券
            ViewBag.PromotionsIsUseCoupon = BLL.SysManage.ConfigSystem.GetBoolValueByCache("PromotionsIsUseCoupon");
            //是否开启分仓
            ViewBag.IsMultiDepot = IsMultiDepot;
            Common.Cookies.setCookie("submitorder_refertype_" + CurrentUser.UserID,
                ((int)YSWL.MALL.Model.Shop.Order.EnumHelper.ReferType.PC).ToString(), 1440);

            #region 处理默认的配送方式
            var dic = ShoppingCartHelper.GetSuppCartItems(cartInfo.Items);
            string shipStr = "";
            foreach (var di in dic)
            {
               var typeModel=  _shippingTypeManage.GetCacgeModelBySupplied(di.Key);
                if (String.IsNullOrWhiteSpace(shipStr))
                {
                    shipStr = di.Key + "-" + typeModel.ModeId;
                }
                else
                {
                    shipStr = shipStr+"|"+ di.Key + "-" + typeModel.ModeId;
                }
               
            }
            Common.Cookies.setCookie("shipStr", shipStr, 1440);
            #endregion

            return View(viewName, cartInfo);
        }

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
                    array.Add(json);
                });
            }
            result.Accumulate("STATUS", "OK");
            result.Accumulate("DATA", array.ToString());
            Response.Write(result.ToString());
            return;
        }

        public ActionResult SubmitSuccess(string id, string viewName = "SubmitSuccess")
        {
            if (string.IsNullOrWhiteSpace(id))
                return Redirect("/");

            long orderId = Common.Globals.SafeLong(id, -1);
            if (orderId < 1) return Content("ERROR_NOTSAFEORDERID");
            Model.Shop.Order.OrderInfo orderInfo = _orderManage.GetModel(orderId);
            if (orderInfo == null) return Content("ERROR_NOORDERINFO");

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
            //支付网关
            ViewBag.PaymentGateway = orderInfo.PaymentGateway;


            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "订单提交成功 " + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }

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
                )
                return Redirect(ViewBag.BasePath + "UserCenter/Orders");

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "查看订单详细信息" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            return View(viewName, orderModel);
        }
        #endregion

        #region 收货人
        public ActionResult ShowAddress(int addressId = -1, string viewName = "_ShowAddress")
        {
            YSWL.MALL.Model.Shop.Shipping.ShippingAddress model = null;
            if (addressId > 0)
            {
                model = _addressManage.GetModel(addressId);
                if (model != null && model.UserId != CurrentUser.UserID)
                {
                    LogHelp.AddInvadeLog(
                        string.Format("非法获取收货人数据|当前用户:{0}|获取收货地址:{1}|_YSWL.Web.Areas.Shop.Controllers.OrderController.ShowAddress",
                            CurrentUser.UserID, addressId), System.Web.HttpContext.Current.Request);
                    ViewBag.Freight = decimal.Zero;
                    return View(viewName);
                }
            }
            else
            {
                //默认读取当前用户设置为
                List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> listAddress = _addressManage.GetDefaultModelList(currentUser.UserID);
                if (listAddress != null && listAddress.Count > 0)
                {
                    //读取默认第一个收货地址
                    model = listAddress[0];
                }
            }



            
            //用户从未设置
            if (model == null) {
                Common.Cookies.setKeyCookie("submitorder_addrId", "0", 1440);
                ViewBag.Freight = decimal.Zero;
                return View(viewName);
            } 
            //写收货地址
            Common.Cookies.setKeyCookie("submitorder_addrId", model.ShippingId.ToString(), 1440);


            #region  配送方式
            string shipStr = Common.Cookies.getKeyCookie("shipStr");
            Dictionary<int, int> dicShip = new Dictionary<int, int>();
            if (!String.IsNullOrWhiteSpace(shipStr))
            {
                var shipArr = shipStr.Split('|');
                foreach (var item in shipArr)
                {
                    if (item.Contains('-'))
                    {
                        var itemArr = item.Split('-');
                        if (dicShip.ContainsKey(YSWL.Common.Globals.SafeInt(itemArr[0], 0)))
                        {
                            dicShip[YSWL.Common.Globals.SafeInt(itemArr[0], 0)] = YSWL.Common.Globals.SafeInt(itemArr[1], 0);
                        }
                        else
                        {
                            dicShip.Add(YSWL.Common.Globals.SafeInt(itemArr[0], 0), YSWL.Common.Globals.SafeInt(itemArr[1], 0));
                        }
                        
                    }
                }
            }
            #endregion  

            if (IsMultiDepot)
            {
                //开启了分仓  设置配送地区
                SetRegionId(model.RegionId);
            }

            //设置

            //团购地区
            #region 团购
            bool isGroupRegionId = true;
            int g = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_groupbuy", "value"), 0);//团购
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
                        isGroupRegionId = resBll.Exists(groupBuyInfo.GroupBuy.RegionId, model.RegionId);
                    }
                }
            }
            #endregion
            ViewBag.IsGroupRegionId = isGroupRegionId;


            #region 运费
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = GetCartInfoByProduct();
            if (cartInfo == null)
            {
                //终止计算运费, 运费0 在提交订单环节提示用户
                ViewBag.Freight = decimal.Zero;
                return View(viewName, model);
            }
            //有收货地址 且 已选择配送 计算差异运费
            if (model != null && model.RegionId > 0)
            {
                //ViewBag.Freight = YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper.CalcFreightGroup(cartInfo, _regionManage.GetModelByCache(model.RegionId));
                ViewBag.Freight = BLL.Shop.Products.ShoppingCartHelper.CalcFreightGroup(cartInfo, _regionManage.GetModelByCache(model.RegionId), currentUser.UserID,dicShip);
            }
            else
            {
                ViewBag.Freight = decimal.Zero;
            }
            #endregion

            return View(viewName, model);
        }

        public ActionResult AddressInfo(int addressId = -1, bool isModify = false, string viewName = "_AddressInfo")
        {
            //DONE: 地址同时支持多项选择 BEN ADD 2013-06-21 | DONE 20130830
            YSWL.MALL.ViewModel.Shop.ShippingAddressModel addressModel = new ViewModel.Shop.ShippingAddressModel();
            addressModel.ListAddress = _addressManage.GetModelList(" UserId=" + currentUser.UserID);

            ViewBag.IsModify = isModify;

            //检索上个页面选中的 收货人 编辑使用
            if (isModify && addressId > 0 && addressModel.ListAddress != null && addressModel.ListAddress.Count > 0)
            {
                addressModel.CurrentAddress = addressModel.ListAddress.Find(info => info.ShippingId == addressId);
            }
            //默认加载当前用户信息作为收货人 或 曾经选择的 收货地址 DB不存在
            if (!isModify && addressId < 0 && addressModel.CurrentAddress == null)
            {
                //TODO: 加载 UserEx 扩展表信息 BEN ADD 2013-06-21
                addressModel.CurrentAddress = new Model.Shop.Shipping.ShippingAddress
                {
                    ShippingId = addressId,
                    ShipName = CurrentUser.TrueName,
                    UserId = CurrentUser.UserID,
                    EmailAddress = CurrentUser.Email,
                    CelPhone = CurrentUser.Phone
                };
            }
            //默认空
            if (addressModel.CurrentAddress == null)
            {
                addressModel.CurrentAddress = new Model.Shop.Shipping.ShippingAddress { ShippingId = addressId };
            }

            return View(viewName, addressModel);
        }

        [HttpPost]
        public ActionResult SubmitAddressInfo(FormCollection form)
        {
            bool isModify = Common.Globals.SafeBool(form["IsModify"], false);
            Model.Shop.Shipping.ShippingAddress model = new Model.Shop.Shipping.ShippingAddress
            {
                ShippingId = Common.Globals.SafeInt(form["CurrentAddress.ShippingId"], -1),
                UserId = Common.Globals.SafeInt(form["CurrentAddress.UserId"], -1),
                ShipName = form["CurrentAddress.ShipName"],
                RegionId = Common.Globals.SafeInt(form["CurrentAddress.RegionId"], -1),
                Address = form["CurrentAddress.Address"],
                CelPhone = form["CurrentAddress.CelPhone"],
                Zipcode = form["CurrentAddress.Zipcode"]
            };

            //DONE: 跳转加addressId参数
            if (model.ShippingId > 0)
            {
                if (isModify)
                {
                    _addressManage.Update(model);

                }
                return RedirectToAction("ShowAddress", new { addressId = model.ShippingId });
            }
            else if (currentUser != null)
            {
                model.UserId = currentUser.UserID;
                model.ShippingId = _addressManage.Add(model);
                if (model.ShippingId > 0)
                {

                    return RedirectToAction("ShowAddress", new { addressId = model.ShippingId });
                }
            }
            return RedirectToAction("AddressInfo");
        }

        private int GetShipAddrIdByUserId(int userId)
        {
            List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> listAddress =
                _addressManage.GetModelList(" UserId=" + currentUser.UserID);
            if (listAddress == null || listAddress.Count < 1) return -1;
            return listAddress[0].ShippingId;
        }
        #endregion

        #region 优惠券

        public ActionResult CouponList(YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = null, string viewName = "_CouponList")
        {
            YSWL.MALL.BLL.Shop.Coupon.CouponInfo infoBll = new CouponInfo();
            List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> infoList = infoBll.GetCouponList(currentUser.UserID, 1, false);
            if (cartInfo == null)
            {
                cartInfo = GetCartInfoByProduct();
            }
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
                decimal basePrice = Common.Globals.SafeDecimal(Fm["BasePrice"], 0);
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
                if (infoModel.LimitPrice > basePrice)
                {
                    return Content("Limit");
                }
                ShoppingCartInfo cartInfo = GetCartInfoByProduct();
                int status = infoBll.GetUseStatus(cartInfo, infoModel);
                switch (status)
                {
                    case -1:
                        return Content("HasFrozen");//已冻结
                    case 0:
                        return Content("No");
                    case 1:
                        string priceStr = (basePrice - infoModel.CouponPrice).ToString("F");
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

        #region 获取促销活动赠品
        /// <summary>
        ///  获取促销活动赠品
        /// </summary>
        /// <param name="coupPrice">优惠劵金额</param>  
        /// <param name="suppId">商家Id</param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult ActivList(decimal coupPrice = 0, int suppId = 0, string viewName = "_ActivList")
        {
            ViewBag.IsMerge = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_Activity_IsMerge");
            int c = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_countdown", "value"), 0);//限时抢购
            int g = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_groupbuy", "value"), 0);//团购
            int a = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_acceId", "value"), 0);//组合套装
            string sku = Common.Globals.SafeString(Common.Cookies.getCookie("submitorder_sku", "value"), "");//sku
            int count = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_count", "value"), 1);//数量
            int referId = 0;//推广人
            string refer = Common.Cookies.getCookie("submitorder_referId", "value");
            if (!String.IsNullOrWhiteSpace(refer))
            {
                referId = Common.Globals.SafeInt(YSWL.Common.UrlOper.Base64Decrypt(refer), 0);
            }

            if (c > 0 || g > 0 || a > 0) //限时抢购/团购/组合套装 不参与促销活动
            {
                return View(viewName, null);
            }


            #region  配送方式
            string shipStr = Common.Cookies.getKeyCookie("shipStr");
            Dictionary<int, int> dicShip = new Dictionary<int, int>();
            if (!String.IsNullOrWhiteSpace(shipStr))
            {
                var shipArr = shipStr.Split('|');
                foreach (var item in shipArr)
                {
                    if (item.Contains('-'))
                    {
                        var itemArr = item.Split('-');
                        if (dicShip.ContainsKey(YSWL.Common.Globals.SafeInt(itemArr[0], 0)))
                        {
                            dicShip[YSWL.Common.Globals.SafeInt(itemArr[0], 0)] = YSWL.Common.Globals.SafeInt(itemArr[1], 0);
                        }
                        else
                        {
                            dicShip.Add(YSWL.Common.Globals.SafeInt(itemArr[0], 0), YSWL.Common.Globals.SafeInt(itemArr[1], 0));
                        }
                    }
                }
            }
            #endregion  

            ShoppingCartInfo cartInfo = BLL.Shop.Products.ShoppingCartHelper.GetCartInfoByProduct((currentUser != null ? currentUser.UserID : -1), sku, count, c, g, a, referId, GetRegionId);
            ViewModel.Shop.ActicityGiveList model = activInfoBll.GetActivityGiveList(cartInfo,
                                                                                     CurrentUser.UserID, coupPrice, GetRegionId);
            ViewBag.AdjustedFreight = BLL.Shop.Products.ShoppingCartHelper.CalcFreightGroup(cartInfo, _regionManage.GetModelByCache(GetRegionId), CurrentUser.UserID, dicShip);
            return View(viewName, model);
        }
        #endregion

        #region  获取要购买的商品 组装成购物车对象
        /// <summary>
        /// 获取要购买的商品 组装成购物车对象
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="count"></param>
        /// <param name="g"></param>
        /// <param name="c"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public ShoppingCartInfo GetCartInfoByProduct()
        {
            int c = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_countdown", "value"), 0);//限时抢购
            int g = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_groupbuy", "value"), 0);//团购
            int a = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_acceId", "value"), 0);//组合套装
            string sku = Common.Globals.SafeString(Common.Cookies.getCookie("submitorder_sku", "value"), "");//sku
            int count = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_count", "value"), 1);//数量
            int referId = 0;//推广人
            string refer = Common.Cookies.getCookie("submitorder_referId", "value");
            if (!String.IsNullOrWhiteSpace(refer))
            {
                referId = Common.Globals.SafeInt(YSWL.Common.UrlOper.Base64Decrypt(refer), 0);
            }
            ShoppingCartInfo cartInfo = BLL.Shop.Products.ShoppingCartHelper.GetCartInfoByProduct((currentUser != null ? currentUser.UserID : -1), sku, count, c, g, a, referId, GetRegionId);
            return cartInfo;
        }

        #endregion

        #region 预订订单
        public ActionResult SubmitPreOrder(int id, string sku, int count = 1, string viewName = "SubmitPreOrder")
        {
            BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
            BLL.Shop.PrePro.PreProduct preproBll = new PreProduct();
            BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();

            #region 获取预订商品信息

            YSWL.MALL.Model.Shop.PrePro.PreProduct preProductInfo = preproBll.GetModel(id);
            if (preProductInfo == null)
            {
                return new RedirectResult("/");
            }

            #endregion

            #region 获取商品信息


            #region 指定SKU提交订单 此功能已投入使用
            Model.Shop.Products.SKUInfo skuInfo = skuManage.GetModelBySKU(sku);
            if (skuInfo == null)
            {
                return new RedirectResult("/");
            }

            Model.Shop.Products.ProductInfo productInfo = productManage.GetModel(skuInfo.ProductId);
            if (productInfo == null)
            {
                return new RedirectResult("/");
            }
            #endregion

            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = BLL.Shop.Products.ShoppingCartHelper.GetCartInfoByPre((currentUser != null ? currentUser.UserID : -1), productInfo, skuInfo, count, preProductInfo);

            ViewBag.TotalQuantity = count;
            ViewBag.TotalAdjustedPrice = preProductInfo.PreAmount * count;
            ViewBag.ProductTotal = preProductInfo.PreAmount * count;
            ViewBag.TotalPrice = preProductInfo.PreAmount * count;
            #endregion

            #region  获取支付方式  排除货到付款
            List<YSWL.Payment.Model.PaymentModeInfo> paymentList = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.Web);
            ViewBag.PaymentList = paymentList.Where(c => c.Gateway != "cod").ToList();
            #endregion

            #region SEO 优化设置 
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "提交订单 " + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            return View(viewName, cartInfo);
        }

        /// <summary>
        /// ajax提交预订订单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sku"></param>
        /// <param name="productid"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="amount"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AjaxPreOrder(int id, string sku, long productid, string name, string phone, string amount, int count, string remark)
        {
            JsonObject result = new JsonObject();
            YSWL.MALL.Model.Shop.PrePro.PreOrder preInfo = new YSWL.MALL.Model.Shop.PrePro.PreOrder();
            preInfo.Amount = Common.Globals.SafeDecimal(amount, 0);
            preInfo.Count = count;
            preInfo.CreatedDate = DateTime.Now;
            preInfo.Phone = phone;
            preInfo.PreCode = "YD" + preInfo.CreatedDate.ToString("yyyyMMddHHmmssfff");
            preInfo.PreProId = id;
            preInfo.ProductId = productid;
            preInfo.ProductName = name;
            preInfo.Remark = remark;
            preInfo.SKU = sku;
            preInfo.Status = preInfo.Amount > 0 ? 0 : 1;
            preInfo.UserId = currentUser.UserID;
            preInfo.UserName = currentUser.UserName;
            long orderid = preOrderBll.Add(preInfo);
            if (orderid > 0)
            {
                result.Accumulate("STATUS", "SUCCESS");
            }
            else
            {
                result.Accumulate("STATUS", "FAIL");
            }
            return Content(result.ToString());
        }


        public ActionResult SubmitPreSuccess(string id, string viewName = "SubmitPreSuccess")
        {
            if (string.IsNullOrWhiteSpace(id))
                return Redirect("/");

            long orderId = Common.Globals.SafeLong(id, -1);
            if (orderId < 1) return Content("ERROR_NOTSAFEORDERID");
            YSWL.MALL.Model.Shop.PrePro.PreOrder orderInfo = preOrderBll.GetModel(orderId);
            if (orderInfo == null) return Content("ERROR_NOORDERINFO");

            //Safe
            if (orderInfo.UserId != currentUser.UserID)
                return Redirect(ViewBag.BasePath + "UserCenter/Orders");

            ViewBag.OrderId = orderInfo.PreOrderId;
            //订单编号
            ViewBag.OrderCode = orderInfo.PreCode;
            //应付金额
            ViewBag.OrderAmount = orderInfo.Amount;
            ////收货人
            //ViewBag.ShipName = orderInfo.ShipName;
            ////项目数量
            //ViewBag.ItemsCount = _orderItemManage.GetOrderItemCountByOrderId(orderId);


            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "预订订单提交成功 " + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }
        #endregion


        #region 网银银行卡选择
        public ActionResult BankList(string viewName = "_bankList")
        {
            List<YSWL.MALL.ViewModel.Shop.BankType> bankList = YSWL.MALL.Web.Components.BankHelper.GetAllBankList();

            return View(viewName, bankList);
        }
        #endregion 

    }
}
