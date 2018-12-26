/**
* SubmitOrderHandler.cs
*
* 功 能： 生成订单
* 类 名： SubmitOrderHandler
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/5/20 17:06:54  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Web;
using System.Web.SessionState;
using YSWL.MALL.BLL.Ms;
using YSWL.MALL.BLL.Shop.Coupon;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.Shop.PromoteSales;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.Model.Shop.Products;
using YSWL.Json;
using System.Collections.Generic;
using System.Linq;
using ProductAccessorie = YSWL.MALL.Model.Shop.Products.ProductAccessorie;
using SKUInfo = YSWL.MALL.Model.Shop.Products.SKUInfo;
using SKUItem = YSWL.MALL.Model.Shop.Products.SKUItem;
using YSWL.Json.Conversion;

namespace YSWL.MALL.Web.Handlers.Shop.V2
{
    public class OrderHandler : YSWL.MALL.Web.Handlers.Shop.OrderHandler
    {
        BLL.Members.Users userbll = new BLL.Members.Users();//zhou 20181225新增
        BLL.Shop.Supplier.SupplierInfo suppinfobll = new BLL.Shop.Supplier.SupplierInfo();//zhou 20181225新增
        shopCom spcom = new shopCom();

        BLL.Members.UsersExp userexpbll = new BLL.Members.UsersExp();

        private readonly BLL.Ms.Regions _regionManage = new BLL.Ms.Regions();
        private readonly BLL.Shop.Shipping.ShippingType _shippingTypeManage = new BLL.Shop.Shipping.ShippingType();
        private readonly BLL.Shop.Shipping.ShippingAddress _shippingAddressManage = new BLL.Shop.Shipping.ShippingAddress();
        private readonly BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage = new BLL.Shop.Shipping.ShippingRegionGroups();
        private BLL.Shop.Products.ProductInfo _productInfoManage = new BLL.Shop.Products.ProductInfo();
        private BLL.Shop.Products.SKUInfo _skuInfoManage = new BLL.Shop.Products.SKUInfo();
        private YSWL.MALL.BLL.Shop.Coupon.CouponInfo couponBll = new CouponInfo();
        private YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy groupBuyBll = new GroupBuy();
        private YSWL.MALL.BLL.Shop.Activity.ActivityInfo activInfoBll = new BLL.Shop.Activity.ActivityInfo();
        private YSWL.MALL.BLL.Members.Users userBll = new BLL.Members.Users();
        private YSWL.MALL.BLL.Shop.Products.BrandInfo brandInfoBll = new YSWL.MALL.BLL.Shop.Products.BrandInfo();
        private BLL.Shop.Order.OrderLookupItems lookUpItemBll = new BLL.Shop.Order.OrderLookupItems();
        private BLL.Shop.Order.OrderLookupList lookUpListBll = new BLL.Shop.Order.OrderLookupList();
       
        #region IHttpHandler 成员

        public override bool IsReusable
        {
            get { return false; }
        }

        public override void ProcessRequest(HttpContext context)
        {
            string action = context.Request.Form["Action"];

            context.Response.Clear();
            context.Response.ContentType = "application/json";

            try
            {
                switch (action)
                {
                    case "SubmitOrder":
                        context.Response.Write(SubmitOrder(context));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                YSWL.Log.LogHelper.AddErrorLog(ex.Message, ex.StackTrace);
                LogHelp.AddErrorLog(ex.Message,ex.StackTrace);
                JsonObject json = new JsonObject();
                json.Put(KEY_STATUS, STATUS_ERROR);
                json.Put(KEY_DATA, ex);
                context.Response.Write(json.ToString());
            }
        }

        #endregion

        #region 提交订单

        private string SubmitOrder(HttpContext context)
        {
            JsonObject result = new JsonObject();

            #region 一.获取基础数据(支付/备注/发票/购买者/收货者)

            #region (支付方式/备注/发票信息)
            Payment.Model.PaymentModeInfo paymentModeInfo = GetPaymentModeInfo(context);
            if (paymentModeInfo == null)
            {
                result.Accumulate(KEY_STATUS, "NOPAYMENTMODEINFO");
                return result.ToString();
            }


            string orderRemark = context.Request.Form["Remark"];
            if (!string.IsNullOrWhiteSpace(orderRemark))
            {
                orderRemark = InjectionFilter.Filter(orderRemark);
            }

           
            #endregion

            #region 一.1 发票信息
            List<Model.Shop.Order.OrderOptions> orderOptionsList = null;
            //系统是否开启发票项
            if (BLL.SysManage.ConfigSystem.GetBoolValueByCache("IsOpenInvoicesItem"))
            {
                //开启
                string invoiceInfo = Globals.SafeString(Cookies.getKeyCookie("m_so_invoice"), "");
                if (!String.IsNullOrWhiteSpace(invoiceInfo))
                {
                    //设置了发票信息
                    JsonObject invoiceInfoJson = JsonConvert.Import<JsonObject>(invoiceInfo);
                    //客户是否需要开发票  
                    if (Globals.SafeBool(invoiceInfoJson["IsOpen"].ToString(), false))
                    {
                        //需要
                        orderOptionsList = GetInvoiceInfo(invoiceInfoJson);
                        if (orderOptionsList == null)
                        {
                            result.Accumulate(KEY_STATUS, "NOINVOICEINFO");
                            return result.ToString();
                        }
                    }
                }
            }
            #endregion

            #region 一.2 获取购买人

            //DONE: 1.获取购买人
            YSWL.Accounts.Bus.User userBuyer = GetBuyerUserInfo(context);
            if (userBuyer == null)
            {
                result.Accumulate(KEY_STATUS, STATUS_NOLOGIN);
                return result.ToString();
            }
            if (userBuyer.UserType == "AA")
            {
                result.Accumulate(KEY_STATUS, STATUS_UNAUTHORIZED);
                return result.ToString();
            }

            #endregion

            #region 一.3 获取收货人

            //DONE: 3.获取收货人
            Model.Shop.Shipping.ShippingAddress shippingAddress = GetShippingAddress(context);
            if (shippingAddress == null)
            {
                result.Accumulate(KEY_STATUS, "NOSHIPPINGADDRESS");
                return result.ToString();
            }
            Model.Ms.Regions regionInfo = _regionManage.GetModelByCache(shippingAddress.RegionId);
            if (regionInfo == null)
            {
                result.Accumulate(KEY_STATUS, "NOREGIONINFO");
                return result.ToString();
            }
            #endregion

            #endregion//todo:结束region?
            #region 二.获取购物车

            #region 二.1 验证团购数据 
            //团购
            int groupBuyId = Common.Globals.SafeInt(context.Request.Form["GroupBuyId"], -1);
            YSWL.MALL.Model.Shop.PromoteSales.GroupBuy buyModel = null;
            if (groupBuyId > 0)
            {
                buyModel = groupBuyBll.GetModelByCache(groupBuyId);
                if (buyModel != null) {
                    if (buyModel.BuyCount >= buyModel.MaxCount)
                    {
                        //已达团购上限 GROUPBUYREACHEDMAX
                        result.Accumulate(KEY_STATUS, "GROUPBUYREACHEDMAX");
                        return result.ToString();
                    }
                    if (!_regionManage.Exists(buyModel.RegionId, shippingAddress.RegionId))
                    {
                        //收货地区不在团购地区范围内
                        result.Accumulate(KEY_STATUS, "OUTGROUPBUYREGION");
                        return result.ToString();
                    }
                }
            }

            #endregion

            #region 二.2获取购物车内商品信息并做处理
            //DONE: 2.获取购物车
            BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper;
            ShoppingCartInfo shoppingCartInfo;
            try
            {
                shoppingCartInfo = GetShoppingCart(context, userBuyer, out shoppingCartHelper);
            }
            catch (ArgumentNullException)
            {
                result.Accumulate(KEY_STATUS, "PROSALEEXPIRED");
                return result.ToString();
            }
            if (shoppingCartInfo == null ||
                shoppingCartInfo.Items == null ||
                shoppingCartInfo.Items.Count < 1)
            {
                result.Accumulate(KEY_STATUS, "NOSHOPPINGCARTINFO");
                return result.ToString();
            }
            //DONE: 2.1 Check 商品库存
            List<ShoppingCartItem> noStockList = new List<ShoppingCartItem>();

            //大于限购数量
            List<ShoppingCartItem> greaRestCountList = new List<ShoppingCartItem>();
            int restCount = 0;
            int totalInvalid = 0;
            foreach (ShoppingCartItem item in shoppingCartInfo.Items)
            {
                //检查购买数量是否大于库存
                if (item.Quantity < 1 || item.Quantity > YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(item.SKU, shippingAddress.RegionId, item.SupplierId))
                {
                    noStockList.Add(item);
                }
                restCount = _productInfoManage.GetRestrictionCount(item.ProductId);
                if (restCount > 0 && item.Quantity > restCount)
                {
                    greaRestCountList.Add(item);
                }

                //检测已失效商品  (已删除或已下架的商品)
                if (item.SaleStatus != 1) {
                    ++totalInvalid;
                }

            }
            if (totalInvalid > 0) {
                result.Accumulate(KEY_STATUS, "INVALID");//含有已失效商品
                return result.ToString();
            }
            if (noStockList.Count > 0)
            {
                result.Accumulate(KEY_STATUS, "NOSTOCK");
                result.Accumulate(KEY_DATA, noStockList);
                //自动移除Cookie/DB购物车中的无库存项目

                return result.ToString();
            }
            if (greaRestCountList.Count > 0)
            {
                result.Accumulate(KEY_STATUS, "GreaRestCount");//超出限购数量
                result.Accumulate(KEY_DATA, greaRestCountList);
                //自动修改Cookie/DB购物车中超出限购数量的项目
                if (shoppingCartHelper != null)
                {
                    greaRestCountList.ForEach(info =>
                    {
                        //TODO: 修改购物车中的数量
                        shoppingCartHelper.UpdateItemQuantity(info.ItemId, _productInfoManage.GetRestrictionCount(info.ProductId));
                    });
                }
                return result.ToString();
            }
            //客户下单件数低于n件时，提示其n件起送，并使其不能下单成功
            int minCount = YSWL.MALL.BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_Order_MinCount");
            if (minCount > 0 && shoppingCartInfo.Quantity < minCount)
            {
                result.Accumulate(KEY_STATUS, "LESSTHANMINCOUNT");
                result.Accumulate(KEY_DATA, minCount);
                return result.ToString();
            }
            decimal totalRate = shoppingCartInfo.TotalRate; //总价优惠值
            #endregion
            #endregion

            #region 三.生成订单
            //DONE: 5.生成订单
            OrderInfo mainOrder = new OrderInfo();

            #region 填充订单数据

            #region 三.1填充订单基础数据

            #region 下单类型
            mainOrder.ReferType = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_refertype_" + userBuyer.UserID, "value"), 0);
            #endregion

            #region 订单前缀 备注
            bool codePre = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_CreateOrder_PreCode");//是否添加订单前缀
            mainOrder.CreateUserId = CurrentUser.UserID;
            mainOrder.CreatedDate = DateTime.Now;
            if (codePre)//订单需要加前缀
            {
                mainOrder.OrderCode = mainOrder.ReferOrderPrefix + mainOrder.CreatedDate.ToString("yyyyMMddHHmmssfff");
            }
            else
            {
                mainOrder.OrderCode = mainOrder.CreatedDate.ToString("yyyyMMddHHmmssfff");
            }
            mainOrder.Remark = orderRemark;
            #endregion

            #region  配送信息
            string shipStr = context.Request.Form["ShipStr"];
            Dictionary<int, int> dicShip = new Dictionary<int, int>();
            if (!String.IsNullOrWhiteSpace(shipStr))
            {
                var shipArr = shipStr.Split('|');
                foreach (var item in shipArr)
                {
                    if (item.Contains('-'))
                    {
                        var itemArr = item.Split('-');  
                        dicShip.Add(YSWL.Common.Globals.SafeInt(itemArr[0],0), YSWL.Common.Globals.SafeInt(itemArr[1],0));
                    } 
                } 
            }
            #endregion



            #region 支付信息

            mainOrder.PaymentTypeId = paymentModeInfo.ModeId;
            mainOrder.PaymentTypeName = paymentModeInfo.Name;
            mainOrder.PaymentGateway = paymentModeInfo.Gateway;

            result.Accumulate("GATEWAY", mainOrder.PaymentGateway);

            #endregion

            #region 订单类型 订单状态
            mainOrder.OrderType = 1;
            mainOrder.OrderStatus = 0;//mainOrder.PaymentGateway == "cod" ? 1 : 0;
            #endregion

            #region 收货人信息

            mainOrder.RegionId = shippingAddress.RegionId;
            mainOrder.ShipRegion = shippingAddress.RegionFullName; //_regionManage.GetFullNameById4Cache(regionInfo.RegionId);
            mainOrder.ShipName = shippingAddress.ShipName;
            mainOrder.ShipEmail = shippingAddress.EmailAddress;
            mainOrder.ShipCellPhone = shippingAddress.CelPhone;
            mainOrder.ShipTelPhone = shippingAddress.TelPhone;
            mainOrder.ShipAddress = shippingAddress.Address;
            mainOrder.ShipZipCode = shippingAddress.Zipcode;

            #endregion

            #region 购买人信息
            mainOrder.BuyerID = userBuyer.UserID;
            mainOrder.BuyerName = userBuyer.UserName; //String.IsNullOrWhiteSpace(userBuyer.TrueName) ? userBuyer.UserName : userBuyer.TrueName;
            mainOrder.ReferID = userBuyer.EmployeeID.ToString();
            //TODO: 用户Email为空时, 暂以默认Email下单 BEN ADD 20130701
            string buyEmail = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_Pay_DefaultEmail");
            buyEmail = String.IsNullOrWhiteSpace(buyEmail) ? "pay@ys56.com" : buyEmail;
            mainOrder.BuyerEmail = string.IsNullOrWhiteSpace(userBuyer.Email) ? buyEmail : userBuyer.Email;
            mainOrder.BuyerCellPhone = userBuyer.Phone;

            #endregion

            #endregion

            #region 三.2促销相关

            #region 限时抢购
            //限时抢购
            int proSaleId = Common.Globals.SafeInt(context.Request.Form["ProSaleId"], -1);
            if (proSaleId > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductInfo proSaleInfo = _productInfoManage.GetProSaleModel(proSaleId);
                if (proSaleInfo != null)
                {
                    mainOrder.ActivityName = string.Format("限时抢购[{0}]", proSaleInfo.CountDownId);
                    //活动优惠金额 = 总金额(含运费无任何优惠) - 最终支付(含运费优惠后)
                    mainOrder.ActivityFreeAmount = mainOrder.OrderTotal - mainOrder.Amount;
                    mainOrder.ActivityStatus = 1;
                }
            }
            #endregion

            #region 团购数据
            if (buyModel != null)
            {
                mainOrder.GroupBuyId = buyModel.GroupBuyId;
                mainOrder.GroupBuyPrice = buyModel.Price;
                mainOrder.GroupBuyStatus = 1;
            }
            #endregion

            #region 组合套餐
            //组合套餐
            int acceId = Globals.SafeInt(context.Request.Form["AcceId"], -1);
            if (acceId > 0)
            {
                BLL.Shop.Products.ProductAccessorie prodAcceBll = new BLL.Shop.Products.ProductAccessorie();
                ProductAccessorie prodAcceModel = prodAcceBll.GetModel(acceId);
                if (prodAcceModel != null)
                {
                    mainOrder.ActivityName = string.Format("组合优惠[{0}]", acceId);
                    //活动优惠金额 = 总金额(含运费无任何优惠) - 最终支付(含运费优惠后)
                    mainOrder.ActivityFreeAmount = mainOrder.OrderTotal - mainOrder.Amount;
                    mainOrder.ActivityStatus = 1;
                }
            }
            #endregion


            #region 优惠券数据
            mainOrder.CouponAmount = 0;

            //优惠劵
            string couponCode = context.Request.Form["Coupon"];
            #region 团购/限时抢购 是否能使用优惠券
            //团购/限时抢购 是否能使用优惠券
            bool promotionsIsUseCoupon = BLL.SysManage.ConfigSystem.GetBoolValueByCache("PromotionsIsUseCoupon");
            if (!String.IsNullOrWhiteSpace(couponCode) && (proSaleId > 0 || groupBuyId > 0) && !promotionsIsUseCoupon)
            {
                result.Accumulate(KEY_STATUS, "NOTCANUSECOUPON");
                return result.ToString();
                // couponCode = "";
            }
            #endregion

            Model.Shop.Coupon.CouponInfo infoModel = couponBll.GetCouponInfo(couponCode);

            if (infoModel != null && infoModel.Status < 2 && infoModel.Status >= 0)
            {
                mainOrder.CouponAmount = infoModel.CouponPrice;
                mainOrder.CouponCode = infoModel.CouponCode;
                mainOrder.CouponName = infoModel.CouponName;
                mainOrder.CouponValue = infoModel.CouponPrice;
                mainOrder.CouponValueType = 1;
            }

            #endregion

            //是否包邮
            //bool IsFreeShippingActiv = false;

            #region 获取促销活动的赠品信息
            List<Model.Shop.Activity.ActivityInfo> actInfoList = null;
            //赠品
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> actProductList = null;
            //赠品 目前没有加运费 没有加重量
            if (proSaleId < 1 && groupBuyId < 1 && acceId < 1)
            {
                actInfoList = activInfoBll.GetActGiftList(shoppingCartInfo, userBuyer.UserID, mainOrder.CouponAmount.HasValue ? mainOrder.CouponAmount.Value : 0);
                if (actInfoList != null)
                {
                    ////获取包邮活动 20160902 根据购物车获取包邮活动 并且主订单包邮 不符合当前业务逻辑 予以废除
                    //List<Model.Shop.Activity.ActivityInfo> freeShippingList = actInfoList.Where(p => p.RuleId == 4).ToList();
                    //if (freeShippingList != null && freeShippingList.Count > 0)
                    //{
                    //    //包邮
                    //    IsFreeShippingActiv = true;
                    //}
                    actProductList = YSWL.MALL.BLL.Shop.Activity.ActivityInfo.GetActProList(actInfoList, shippingAddress.RegionId);
                }
            }
            #endregion

            #endregion

            #region 拆单对象
            Dictionary<int, List<OrderItems>> dicSuppOrderItems = new Dictionary<int, List<OrderItems>>();
            #endregion

            #region 购物车 -> 订单项目
            OrderItems tmpOrderItem;

            //购物车 -> 订单项目
            shoppingCartInfo.Items.ForEach(item =>
            {
                tmpOrderItem = new OrderItems
                {
                    //TODO: 警告: 商品信息根据Cookie获取, 暂未与DB及时同步
                    Name = item.Name,
                    SKU = item.SKU,
                    Quantity = item.Quantity,
                    ShipmentQuantity = item.Quantity,
                    ThumbnailsUrl = item.ThumbnailsUrl,
                    Points = item.Points,

                    //添加商城积分
                    Gwjf = item.Gwjf,

                    Weight = item.Weight > 0 ? item.Weight : 0,
                    ProductId = item.ProductId,
                    Description = item.Description,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    AdjustedPrice = item.AdjustedPrice,
                    Deduct = item.SellPrice - item.AdjustedPrice,
                    OrderCode = mainOrder.OrderCode,
                    //品牌信息
                    BrandId = item.BrandId,
                    BrandName = item.BrandName,
                    //商家信息
                    SupplierId = item.SupplierId,
                    SupplierName = item.SupplierName,

                    ProductType = 1,
                    //推广信息（分销返佣）
                    ReferId = item.ReferId == 0 ? (userBll.GetInviteUserId(userBuyer.UserID)) : item.ReferId
                };
                //将SKU信息记录到订单项目的Attribute中 简单记录 逗号分割, 复杂的可以为Json结构
                if (item.SkuValues != null && item.SkuValues.Length > 0)
                {
                    tmpOrderItem.Attribute = string.Join(",", item.SkuValues);
                }

                //填充订单项
                mainOrder.OrderItems.Add(tmpOrderItem);
                //

                //填充商家订单项
                if (tmpOrderItem.SupplierId.HasValue && tmpOrderItem.SupplierId.Value > 0)
                {
                    if (dicSuppOrderItems.ContainsKey(tmpOrderItem.SupplierId.Value))
                    {
                        dicSuppOrderItems[tmpOrderItem.SupplierId.Value].Add(tmpOrderItem);
                    }
                    else
                    {
                        dicSuppOrderItems.Add(tmpOrderItem.SupplierId.Value,
                            new List<OrderItems> { tmpOrderItem });
                    }
                }
                else
                {
                    if (dicSuppOrderItems.ContainsKey(0))
                    {
                        dicSuppOrderItems[0].Add(tmpOrderItem);
                    }
                    else
                    {
                        dicSuppOrderItems.Add(0,
                            new List<OrderItems> { tmpOrderItem });
                    }
                }
            });
            #endregion


            #region 将赠品添加到子单
            if (actProductList != null && actProductList.Count > 0)
            {
                BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
                Model.Shop.Supplier.SupplierInfo supplierInfo;
                BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
                foreach (var productInfo in actProductList)
                {
                    supplierInfo = null;
                    if (productInfo.SupplierId > 0)
                    {
                        supplierInfo = supplierManage.GetModelByCache(productInfo.SupplierId);
                    }

                    tmpOrderItem = new OrderItems
                    {
                        //TODO: 警告: 商品信息根据Cookie获取, 暂未与DB及时同步
                        Name = productInfo.ProductName,
                        SKU = productInfo.SkuInfos[0].SKU,
                        Quantity = productInfo.Count,
                        ShipmentQuantity = productInfo.Count,
                        ThumbnailsUrl = productInfo.ThumbnailUrl1,
                        Points = productInfo.Points.HasValue ? Globals.SafeInt(productInfo.Points.Value, 0) : 0,

                        //商城积分
                        Gwjf = productInfo.Gwjf.HasValue ? Globals.SafeInt(productInfo.Gwjf.Value, 0) : 0,

                        Weight = 0, //productInfo.SkuInfos[0].Weight.HasValue?productInfo.SkuInfos[0].Weight.Value:0,
                        ProductId = productInfo.ProductId,
                        Description = productInfo.Description,
                        CostPrice = productInfo.SkuInfos[0].CostPrice.HasValue ? productInfo.SkuInfos[0].CostPrice.Value : 0,
                        SellPrice = productInfo.SalePrice,// productInfo.SkuInfos[0].SalePrice,// productInfo.SalePrice,//
                        AdjustedPrice = productInfo.SalePrice,//促销价// productInfo.SkuInfos[0].SalePrice,
                        Deduct = productInfo.SkuInfos[0].SalePrice - productInfo.SalePrice,
                        OrderCode = mainOrder.OrderCode,
                        BrandId = productInfo.BrandId,
                        ProductType = 2
                    };
                    //商家信息
                    if (supplierInfo != null)
                    {
                        tmpOrderItem.SupplierId = supplierInfo.SupplierId;
                        tmpOrderItem.SupplierName = supplierInfo.Name;
                    }
                    //将SKU信息记录到订单项目的Attribute中 简单记录 逗号分割, 复杂的可以为Json结构
                    List<SKUItem> listSkuItems = skuManage.GetSKUItemsBySkuId(productInfo.SkuInfos[0].SkuId);
                    if (listSkuItems != null && listSkuItems.Count > 0)
                    {
                        string skuValues = string.Empty;
                        listSkuItems.ForEach(xx =>
                        {
                            skuValues += xx.ValueStr + ",";
                        });
                        tmpOrderItem.Attribute = skuValues.TrimEnd(',');
                    }
                    //填充订单项
                    mainOrder.OrderItems.Add(tmpOrderItem);

                    //填充商家订单项
                    if (tmpOrderItem.SupplierId.HasValue && tmpOrderItem.SupplierId.Value > 0)
                    {
                        if (dicSuppOrderItems.ContainsKey(tmpOrderItem.SupplierId.Value))
                        {
                            dicSuppOrderItems[tmpOrderItem.SupplierId.Value].Add(tmpOrderItem);
                        }
                        else
                        {
                            dicSuppOrderItems.Add(tmpOrderItem.SupplierId.Value,
                                new List<OrderItems> { tmpOrderItem });
                        }
                    }
                    else
                    {
                        if (dicSuppOrderItems.ContainsKey(0))
                        {
                            dicSuppOrderItems[0].Add(tmpOrderItem);
                        }
                        else
                        {
                            dicSuppOrderItems.Add(0,
                                new List<OrderItems> { tmpOrderItem });
                        }
                    }
                }
            }
            #endregion


            //---------------------------------

            Model.Members.UsersExpModel userexpmodel = userexpbll.GetUsersExpModelByCache(mainOrder.BuyerID);

            //if (paymentModeInfo.Gateway.Trim() == "xianjinjifencount" || paymentModeInfo.Gateway.Trim() == "gouwujifencount")//如果是选择现金支付或购物积分支付，必须是VIP会员才可以
            //{
            //    if (userexpmodel.BodilyForm.ToUpper() != "VIP")
            //    {
            //        result.Accumulate(KEY_STATUS, "您还不是VIP会员，不能选择“现金积分支付或购物积分支付”请重新选择！");
            //        return result.ToString();
            //    }
            //}

            string Wdbh = context.Request.Form["Wdbh"];

            if (!string.IsNullOrWhiteSpace(Wdbh))
            {
                Wdbh = InjectionFilter.Filter(Wdbh);

                //string strwdbhname = spcom.GetIsWdbh(Wdbh);
                //if (strwdbhname == "不是生活馆")
                //{
                //    result.Accumulate(KEY_STATUS, "您输入的服务店铺编号不存在，请重新输入！");
                //    return result.ToString();
                //}
            }
            else
            {
                Wdbh = null;
                //if (userexpmodel.BodilyForm.ToString() == "VIP")//如果是VIP会员，必须要填写店铺编号
                //{
                //    result.Accumulate(KEY_STATUS, "VIP会员，必须要填写店铺编号！");
                //    return result.ToString();
                //}
            }
            //---------------------------------------



            #region 平台配送商品运费 运费计算
            int terraceWeight = 0; //平台配送商品总重
            decimal terracePrice = 0; //平台配送商品总运费
            if (dicSuppOrderItems.Count > 1)
            {
                foreach (KeyValuePair<int, List<OrderItems>> item in dicSuppOrderItems)
                {
                    if (YSWL.MALL.BLL.Shop.Supplier.SupplierConfig.GetShipTypeBycahe(item.Key) <= 0)//根据商家id获取配送方式
                    {
                        item.Value.ForEach(info =>
                        {
                            terraceWeight += info.Weight * info.Quantity;
                        });
                    }
                }

                var terraceShip = _shippingTypeManage.GetModelByCache(dicShip[0]);
                if (terraceShip == null)
                {
                    result.Accumulate(KEY_STATUS, "NOSHIPTYPE");
                    return result.ToString();
                }

                #region 区域差异运费计算
                int topRegionId;
                if (regionInfo.Depth > 1)
                {
                    topRegionId = Globals.SafeInt(regionInfo.Path.Split(new[] { ',' })[1], -1);
                }
                else
                {
                    topRegionId = regionInfo.RegionId;
                }

                Model.Shop.Shipping.ShippingRegionGroups shippingRegion =
                    _shippingRegionManage.GetShippingRegion(terraceShip.ModeId, topRegionId);
                #endregion

                if (terraceWeight > 0)
                {
                    if (shippingRegion != null)//使用地区价格
                    {
                        terracePrice = shippingRegion.Price; //商品首重价格

                        #region 平台商品续重价格
                        if ((terraceShip.AddWeight ?? 0) != 0 && (shippingRegion.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                        {
                            if (terraceWeight - terraceShip.Weight > 0)
                            {
                                terracePrice = terracePrice +
                                               Math.Ceiling((decimal)(terraceWeight - terraceShip.Weight) /
                                                            terraceShip.AddWeight.Value) * shippingRegion.AddPrice.Value; //商品续重价格
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        terracePrice = terraceShip.Price; //商品首重价格

                        #region 平台商品续重价格
                        if ((terraceShip.AddWeight ?? 0) != 0 && (terraceShip.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                        {
                            if (terraceWeight - terraceShip.Weight > 0)
                            {
                                terracePrice = terracePrice +
                                               Math.Ceiling((decimal)(terraceWeight - terraceShip.Weight) /
                                                            terraceShip.AddWeight.Value) * terraceShip.AddPrice.Value; //商品续重价格
                            }
                        }
                        #endregion
                    }
                }




            }
            #endregion

            decimal shopPrice = 0;
            #region 自动拆单
            int subOrderIndex = 1;
            //判断是否购买了多个配送方式家的商品, 并进行拆单
            if (dicSuppOrderItems.Count > 1)
            {
                #region 拆单逻辑
                decimal discountAmount = mainOrder.CouponAmount.HasValue ? mainOrder.CouponAmount.Value : 0;
                var SuppOrderItems = from pair in dicSuppOrderItems orderby pair.Key select pair;//排序确保平台商品优先计算
                foreach (KeyValuePair<int, List<OrderItems>> item in SuppOrderItems)
                {
                    //根据主订单构造子订单
                    OrderInfo subOrder = new OrderInfo(mainOrder);
                    #region 子订单基础数据
                    //DONE: 防止运算过快产生相同订单号
                    subOrder.CreatedDate = mainOrder.CreatedDate.AddMilliseconds(subOrderIndex++);
                    if (codePre)
                    {
                        subOrder.OrderCode = subOrder.ReferOrderPrefix + subOrder.CreatedDate.ToString("yyyyMMddHHmmssfff");
                    }
                    else
                    {
                        subOrder.OrderCode = subOrder.CreatedDate.ToString("yyyyMMddHHmmssfff");
                    }
                    #endregion

                    #region 子单平台自营商品添加发票信息
                    if (item.Key < 1)
                    {
                        subOrder.OrderOptions = orderOptionsList;
                        mainOrder.OrderOptions = orderOptionsList;
                    }
                    #endregion


                    #region Reset 重量/运费/积分/价格
                    subOrder.Weight = 0;
                    subOrder.FreightAdjusted =
                        subOrder.FreightActual =
                            subOrder.Freight = 0;
                    subOrder.OrderPoint = 0;

                    subOrder.ProductTotal = 0;
                    subOrder.OrderCostPrice = 0;
                    subOrder.OrderOptionPrice = 0;
                    subOrder.OrderProfit = 0;
                    subOrder.Amount = 0;
                    subOrder.DiscountAdjusted = 0;
                    #endregion

                    #region 清空限时抢购
                    subOrder.ActivityName = null;
                    subOrder.ActivityFreeAmount = null;
                    subOrder.ActivityStatus = 0;
                    #endregion

                    #region 清空团购数据
                    subOrder.GroupBuyId = null;
                    subOrder.GroupBuyPrice = null;
                    subOrder.GroupBuyStatus = 0;
                    #endregion

                    #region 填充子单商家信息
                    subOrder.SupplierId = item.Key;
                    if (!YSWL.MALL.BLL.Shop.Order.OrderManage.FillSellerInfo(subOrder))
                    {
                        result.Accumulate(KEY_STATUS, "NOSUPPLIERINFO");
                        return result.ToString();
                    }

                    #endregion

                    #region 获取配送(物流)

                    //DONE: 获取配送(物流)
                    Model.Shop.Shipping.ShippingType subshipType = _shippingTypeManage.GetModelByCache(dicShip[item.Key]);

                    #endregion

                    #region 子订单配送信息(物流)

                    if (subshipType == null)
                    {
                        result.Accumulate(KEY_STATUS, "NOSHIPTYPE");
                        return result.ToString();
                    }


                    subOrder.ShippingModeId = subshipType.ModeId;
                    subOrder.ShippingModeName = subshipType.Name;
                    subOrder.RealShippingModeId = subshipType.ModeId;
                    subOrder.RealShippingModeName = subshipType.Name;
                    subOrder.ShippingStatus = 0;
                    subOrder.ExpressCompanyName = subshipType.ExpressCompanyName;
                    subOrder.ExpressCompanyAbb = subshipType.ExpressCompanyEn;

                    #endregion

                    #region 子单商品数据
                    subOrder.OrderItems = item.Value;
                    #endregion


                    #region 重新计算 重量/积分/价格/运费

                    item.Value.ForEach(info =>
                        {
                            info.OrderCode = subOrder.OrderCode;
                            subOrder.Weight += info.Weight * info.Quantity;
                            subOrder.OrderPoint += info.Points * info.Quantity;

                            //订单商品总价(无优惠)
                            subOrder.ProductTotal += info.SellPrice * info.Quantity;
                            //订单总成本价 = 项目总成本价
                            subOrder.OrderCostPrice += info.CostPrice * info.Quantity;
                            //订单最终支付金额 = 商品总价
                            subOrder.Amount += info.AdjustedPrice * info.Quantity;

                            subOrder.DiscountAdjusted += (info.SellPrice - info.AdjustedPrice) * info.Quantity;
                        });


                    #region 子单运费计算
                    //平台配送商品按重量分摊运费
                    if (YSWL.MALL.BLL.Shop.Supplier.SupplierConfig.GetShipTypeBycahe(item.Key) <= 0)
                    {
                        if (terraceWeight == 0)
                        {
                            subOrder.Freight = 0;
                        } else {
                            subOrder.Freight = subOrder.FreightAdjusted =
                            terracePrice <= 0 ? 0 : (terracePrice * (decimal)(subOrder.Weight.Value * 1.00 / terraceWeight));
                        }

                        bool isfrees = activInfoBll.IsFreeShippingActiv(subOrder, CurrentUser.UserID, subOrder.CouponAmount ?? 0);
                        if (isfrees)
                        {
                            //terracePrice = 0;
                            subOrder.FreightAdjusted = 0;
                            subOrder.IsFreeShipping = true;
                        }
                    }
                    else//商家配送商品按商家重量单独计算运费
                    {
                        #region 商家商品计算运费 
                        decimal price = GetFreight(subOrder, regionInfo,dicShip);
                        subOrder.Freight = subOrder.FreightAdjusted = subOrder.FreightActual = price;

                        #region 子单包邮运费
                        bool isfrees = activInfoBll.IsFreeShippingActiv(subOrder, CurrentUser.UserID, subOrder.Amount);

                        if (isfrees)
                        {
                            subOrder.FreightAdjusted = 0;
                            subOrder.IsFreeShipping = true;
                        }
                        #endregion
                        shopPrice += price;
                        #endregion

                    }
                    #endregion

                    //订单总金额(含优惠) 商品总价 + 运费
                    subOrder.OrderTotal = subOrder.ProductTotal + subOrder.Freight.Value;
                    //订单最终支付金额 = 商品总价(含优惠) + 调整后运费
                    subOrder.Amount += subOrder.FreightAdjusted.Value;
                    //TODO: 均分主订单的优惠给子订单, 作为退款使用
                    #endregion

                    #region 总价优惠拆单处理 优先扣减平台自营金额 不足再扣减
                    if (totalRate > 0)
                    {
                        if (subOrder.Amount > 0)
                        {
                            decimal subTotalRate = totalRate - subOrder.Amount > 0 ? totalRate - subOrder.Amount : 0;
                            subOrder.DiscountAdjusted = subTotalRate > 0
                                                            ? subOrder.DiscountAdjusted + subOrder.Amount
                                                            : subOrder.DiscountAdjusted + totalRate;
                            subOrder.Amount = subTotalRate > 0 ? 0 : subOrder.Amount - totalRate;
                            totalRate = subTotalRate;
                        }
                    }
                    #endregion

                    #region 主订单的优惠券金额优先给平台
                    //DONE: 主订单的优惠券金额给平台

                    if (discountAmount > 0)
                    {
                        if (subOrder.Amount > 0)
                        {
                            decimal subCoupontAmount = discountAmount - subOrder.Amount > 0 ? discountAmount - subOrder.Amount : 0;
                            subOrder.CouponAmount = subCoupontAmount > 0 ? subOrder.Amount : discountAmount;
                            subOrder.CouponValue = subCoupontAmount > 0 ? subOrder.Amount : subOrder.CouponValue;
                            subOrder.Amount = subCoupontAmount > 0 ? 0 : subOrder.Amount - discountAmount;
                            subOrder.CouponValueType = 1;
                            discountAmount = subCoupontAmount;
                        }
                        else
                        {
                            subOrder.CouponAmount = null;
                            subOrder.CouponCode = null;
                            subOrder.CouponName = null;
                            subOrder.CouponValue = null;
                            subOrder.CouponValueType = null;
                        }
                    }
                    #endregion
                    //订单项目
                    subOrder.OrderType = 2;

                    mainOrder.SubOrders.Add(subOrder);

                }
                #endregion

                mainOrder.HasChildren = true;   //有子订单
                mainOrder.Freight = mainOrder.FreightActual = mainOrder.FreightAdjusted = terracePrice + shopPrice;
            }
            else
            {
                //没有购买多个商家的商品
                mainOrder.SupplierId = shoppingCartInfo.Items[0].SupplierId ?? 0;
                mainOrder.SupplierName = shoppingCartInfo.Items[0].SupplierName;
                mainOrder.HasChildren = false;  //无子订单


                #region 主单为平台时添加发票信息
                if (mainOrder.SupplierId < 1)
                {
                    mainOrder.OrderOptions = orderOptionsList;
                }
                #endregion

                #region 运费计算(单个商家时) 运费计算

                decimal price = GetFreight(mainOrder, regionInfo,dicShip);

                mainOrder.Freight = mainOrder.FreightActual = mainOrder.FreightAdjusted = price;

                #endregion
            }
            #endregion


            #region 填充主单商家信息
            if (!YSWL.MALL.BLL.Shop.Order.OrderManage.FillSellerInfoEx(mainOrder))
            {
                result.Accumulate(KEY_STATUS, "NOSUPPLIERINFO");
                return result.ToString();
            }
            #endregion

            #region 订单分销逻辑

            #endregion

            #endregion

            #region 重量/运费/积分

            mainOrder.Weight = shoppingCartInfo.TotalWeight;

            if (mainOrder.HasChildren)
            {
                mainOrder.FreightAdjusted = 0;
                bool isFreeShipping = false;
                foreach (var subOrder in mainOrder.SubOrders)
                {
                    mainOrder.FreightAdjusted += subOrder.FreightAdjusted;
                    #region  只要子单中有一单包邮 主订单被设置为包邮
                    if (subOrder.IsFreeShipping)
                    {
                        isFreeShipping = true;
                    }
                    #endregion
                }

                mainOrder.IsFreeShipping = isFreeShipping;

                #region 配送信息(物流) 有子单时被设置为第一个子单的物流信息

                mainOrder.ShippingModeId = mainOrder.SubOrders[0].ShippingModeId;
                mainOrder.ShippingModeName = mainOrder.SubOrders[0].ShippingModeName;
                mainOrder.RealShippingModeId = mainOrder.SubOrders[0].RealShippingModeId;
                mainOrder.RealShippingModeName = mainOrder.SubOrders[0].RealShippingModeName;
                mainOrder.ShippingStatus = 0;
                mainOrder.ExpressCompanyName = mainOrder.SubOrders[0].ExpressCompanyName;
                mainOrder.ExpressCompanyAbb = mainOrder.SubOrders[0].ExpressCompanyAbb;
                #endregion
            }
            else
            {

                bool isfrees = activInfoBll.IsFreeShippingActiv(mainOrder, CurrentUser.UserID, mainOrder.Amount);
                if (isfrees)
                {
                    mainOrder.FreightAdjusted = 0;
                    mainOrder.IsFreeShipping = true;
                }


                #region 配送信息(物流)
                Model.Shop.Shipping.ShippingType shippingType = _shippingTypeManage.GetModelByCache(dicShip[mainOrder.SupplierId.Value]);

                if (shippingType == null)
                {
                    result.Accumulate(KEY_STATUS, "NOSHIPTYPE");
                    return result.ToString();
                }

                mainOrder.ShippingModeId = shippingType.ModeId;
                mainOrder.ShippingModeName = shippingType.Name;
                mainOrder.RealShippingModeId = shippingType.ModeId;
                mainOrder.RealShippingModeName = shippingType.Name;
                mainOrder.ShippingStatus = 0;
                mainOrder.ExpressCompanyName = shippingType.ExpressCompanyName;
                mainOrder.ExpressCompanyAbb = shippingType.ExpressCompanyEn;
                #endregion
            }


            //if (IsFreeShippingActiv)
            //{
            //    //包邮
            //    mainOrder.FreightAdjusted = 0;
            //    mainOrder.IsFreeShipping = true;
            //}

            mainOrder.OrderPoint = shoppingCartInfo.TotalPoints;


            #region 订单价格

            //订单商品总价(无任何优惠)
            mainOrder.ProductTotal = shoppingCartInfo.TotalSellPrice;

            //订单总成本价 = 商品总成本价
            mainOrder.OrderCostPrice = shoppingCartInfo.TotalCostPrice;

            //订单总金额(无任何优惠) 商品总价 + 运费


            if (!mainOrder.Freight.HasValue)
            {
                mainOrder.Freight = 0;
                mainOrder.FreightAdjusted = 0;
            }
            if (!mainOrder.CouponAmount.HasValue)
            {
                mainOrder.CouponAmount = 0;
            }
            mainOrder.OrderTotal = shoppingCartInfo.TotalSellPrice + mainOrder.Freight.Value;
            if (mainOrder.IsFreeShipping)
            {
                mainOrder.FreightAdjusted = 0;
            }
            

            //商城积分抵扣金额开始
            mainOrder.Gwjf = shoppingCartInfo.TotalGwjf;

            decimal userGwJF = spcom.GetPointByUsername(userBuyer.UserName);//获取商城积分


            //获取用户的商城积分
            if (userGwJF < mainOrder.Gwjf)
            {
                mainOrder.Gwjf = userGwJF;
            }

            decimal amount = 0;

            if (paymentModeInfo.Gateway.ToLower() == "gouwujifencount")//如果使用购物积分来兑换商品时，不适用商城积分 20180312 zhou 修改
            { 
                //订单最终支付金额 = 项目调整后总售价 + 调整后运费-优惠券价格
                 amount = shoppingCartInfo.TotalAdjustedPrice + mainOrder.FreightAdjusted.Value - mainOrder.CouponAmount.Value;
            }
            else
            {
                //订单最终支付金额 = 项目调整后总售价 + 调整后运费-优惠券价格---商城积分
                amount = shoppingCartInfo.TotalAdjustedPrice + mainOrder.FreightAdjusted.Value - mainOrder.CouponAmount.Value - mainOrder.Gwjf;//减去商城积分
            }
            //商城积分抵扣金额结束


            //订单最终支付金额 = 项目调整后总售价 + 调整后运费-优惠券价格
           // decimal amount = shoppingCartInfo.TotalAdjustedPrice + mainOrder.FreightAdjusted.Value - mainOrder.CouponAmount.Value;
            mainOrder.Amount = amount > 0 ? amount : 0;
            //折扣金额
            mainOrder.DiscountAdjusted = shoppingCartInfo.TotalSellPrice - shoppingCartInfo.TotalAdjustedPrice;
            
            if (mainOrder.Amount < 0)
            {
                LogHelp.AddInvadeLog(
                    string.Format("非法订单金额|{0}|_YSWL.Web.Handlers.Shop.OrderHandler.SubmitOrder",
                        mainOrder.Amount.ToString("F2")), HttpContext.Current.Request);
                result.Accumulate(KEY_STATUS, "ILLEGALORDERAMOUNT");
                return result.ToString();
            }

            mainOrder.Wdbh = Wdbh;//店铺编号加入订单
            mainOrder.RemrkOne = "";//
            mainOrder.RemrkTwo = "";//

            #endregion


            #region  zhou20181225 新增  获取会员所属的商家ID和订单消费积分（订单金额）
            //---------------------zhou20181225 新增
            //获取会员所属的商家ID
            string strsuppEmpid = userbll.GetEmployeeIDByUserid(mainOrder.BuyerID.ToString()).ToString();//获取所有店铺的会员编号
            string suppid = suppinfobll.GetSuppidBywhere(" UserId='" + strsuppEmpid + "'");//店铺会员编号获得店铺ID

            mainOrder.Dpxfjf = mainOrder.OrderTotal;

            mainOrder.Wdbh = suppid;
            //-----------------
            #endregion


            #endregion
            #region 执行事务-创建订单
            try
            {
                mainOrder.OrderId = BLL.Shop.Order.OrderManage.CreateOrder(mainOrder);
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog("订单创建失败: " + ex.Message, ex.StackTrace, context.Request);
            }
            #endregion

            #endregion

            #region 6.提示生成成功, 进行支付
            //移除下单类型Session

            //DONE: 6.提示生成成功, 进行支付
            result.Accumulate(KEY_DATA, new
                {
                    mainOrder.OrderId,
                    mainOrder.OrderCode,
                    mainOrder.Amount,
                    mainOrder.PaymentTypeId,
                    mainOrder.PaymentTypeName
                });

            if (mainOrder.OrderId == -1)
            {
                result.Accumulate(KEY_STATUS, STATUS_FAILED);
                return result.ToString();
            }
            //更新优惠券信息
            if (!String.IsNullOrWhiteSpace(mainOrder.CouponCode))
            {
                couponBll.UseCoupon(mainOrder.CouponCode, mainOrder.BuyerID, mainOrder.BuyerEmail);
            }
            //更新团购信息
            if (groupBuyId > 0)
            {
                groupBuyBll.UpdateBuyCount(groupBuyId, shoppingCartInfo.Quantity);
            }

            #region 生成促销活动优惠劵
            activInfoBll.GenerateData(mainOrder,actInfoList);    
            #endregion

            #region 订单邮件推送
           bool IsOpenEmail =YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Open_OrderEmail");
           if (IsOpenEmail && !String.IsNullOrWhiteSpace(userBuyer.Email))
            {
                YSWL.MALL.BLL.Ms.EmailTemplet templetBll=new EmailTemplet();
                templetBll.SendOrderEmail(mainOrder, userBuyer.Email);
            }

            #endregion 

           #region  更新SKU库存缓存
           //YSWL.MALL.BLL.Shop.Products.StockHelper.UpdateOrderStock(mainOrder);
           #endregion

           //清空Cookie/DB购物车
            if (shoppingCartHelper != null  && shoppingCartHelper != null)
            {
                //自动移除Cookie/DB购物车中已下单的项目
                    shoppingCartInfo.Items.ForEach(info =>
                        {
                            shoppingCartHelper.RemoveItem(info.ItemId);
                        });
                //shoppingCartHelper.ClearShoppingCart(); 移除全部项
            }
            result.Accumulate(KEY_STATUS, STATUS_SUCCESS);

            #endregion

            return result.ToString();
        }
        #endregion

        #region 发票数据
        private List<Model.Shop.Order.OrderOptions> GetInvoiceInfo(JsonObject invoiceInfoJson)
        {
            int headerId = Globals.SafeInt(invoiceInfoJson["HeaderId"].ToString(), 0);//发票抬头项ID
            int contentId = Globals.SafeInt(invoiceInfoJson["ContentId"].ToString(), 0);//发票内容项ID
            int sysSetHeaderId = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("Shop_InvoiceInfo_HeaderId"), 9);//发票抬头默认ListId
            int sysSetContentId = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("Shop_InvoiceInfo_ContentId"), 10);//发票内容默认ListId
            string name = InjectionFilter.SqlFilter(invoiceInfoJson["Name"].ToString());
            OrderLookupList headerLookUpListModel = lookUpListBll.GetModelByCache(sysSetHeaderId);
            OrderLookupList conentLookUpListModel = lookUpListBll.GetModelByCache(sysSetContentId);
            if (headerLookUpListModel == null || conentLookUpListModel == null)
            {
                return null;//返回异常
            }
            YSWL.MALL.Model.Shop.Order.OrderLookupItems headerItemModel = lookUpItemBll.GetModelByCache(headerId);
            YSWL.MALL.Model.Shop.Order.OrderLookupItems contentItemModel = lookUpItemBll.GetModelByCache(contentId);
            if (headerItemModel == null || contentItemModel == null)
            {
                //返回
                return null;
            }
            List<Model.Shop.Order.OrderOptions> orderOptionsList = new List<OrderOptions>();
            Model.Shop.Order.OrderOptions model = new OrderOptions();
            //发票抬头
            model.LookupListId = headerLookUpListModel.LookupListId;
            model.LookupItemId = headerItemModel.LookupItemId;
            model.CustomerTitle = name;
            model.ListDescription = headerLookUpListModel.Name;
            model.ItemDescription = headerItemModel.Name;
            orderOptionsList.Add(model);

            //发票内容
            model = new OrderOptions();
            model.LookupListId = conentLookUpListModel.LookupListId;
            model.LookupItemId = contentItemModel.LookupItemId;
            model.ListDescription = conentLookUpListModel.Name;
            model.ItemDescription = contentItemModel.Name;
            orderOptionsList.Add(model);
            return orderOptionsList;
        }
        #endregion

        /// <summary>
        /// 计算运费(已包含区域运费)
        /// </summary>
        /// <param name="orderInfo">订单</param>
        /// <param name="regionInfo">区域</param>
        /// <returns>计算运费(无折扣信息)</returns>
        private  decimal GetFreight(OrderInfo orderInfo, Model.Ms.Regions regionInfo,Dictionary<int,int> dicShip)
        {
            decimal price = 0;
            int weight = 0;

            foreach (var item in orderInfo.OrderItems)
            {
                weight += item.Weight*item.Quantity;
            }

            Model.Shop.Shipping.ShippingType shipType = _shippingTypeManage.GetModelByCache(dicShip[orderInfo.SupplierId ?? 0]);

            if (shipType==null)
            {
                return 0;
            }

            #region 获取区域运费model
            int topRegionId;
            if (regionInfo.Depth > 1)
            {
                topRegionId = Globals.SafeInt(regionInfo.Path.Split(new[] { ',' })[1], -1);
            }
            else
            {
                topRegionId = regionInfo.RegionId;
            }

            Model.Shop.Shipping.ShippingRegionGroups shippingRegion =
                _shippingRegionManage.GetShippingRegion(shipType.ModeId, topRegionId);
            #endregion

            if (weight > 0)
            {
                if (shippingRegion != null) //区域运费计算
                {
                    #region 首重
                    price = shippingRegion.Price;
                    #endregion

                    #region 续重
                    if ((shipType.AddWeight ?? 0) != 0 && (shippingRegion.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                    {
                        if (weight - shipType.Weight > 0)
                        {
                            price = price +
                                    Math.Ceiling((decimal)(weight - shipType.Weight) / shipType.AddWeight.Value) *
                                    shippingRegion.AddPrice.Value; //商品续重价格
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 首重
                    price = shipType.Price;
                    #endregion

                    #region 续重
                    if ((shipType.AddWeight ?? 0) != 0 && (shipType.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                    {
                        if (weight - shipType.Weight > 0)
                        {
                            price = price +
                                    Math.Ceiling((decimal)(weight - shipType.Weight) / shipType.AddWeight.Value) *
                                    shipType.AddPrice.Value; //商品续重价格
                        }
                    }
                    #endregion
                }
            }

            
            return price;
        }
          
            
    }
    
}