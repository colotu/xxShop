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

namespace YSWL.MALL.Web.Handlers.Shop
{
    public class OrderHandler : HandlerBase, IRequiresSessionState
    {
        private readonly BLL.Ms.Regions _regionManage = new BLL.Ms.Regions();
        private readonly BLL.Shop.Shipping.ShippingType _shippingTypeManage = new BLL.Shop.Shipping.ShippingType();
        private readonly BLL.Shop.Shipping.ShippingAddress _shippingAddressManage =  new BLL.Shop.Shipping.ShippingAddress();
        private readonly BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage =  new BLL.Shop.Shipping.ShippingRegionGroups();
        private BLL.Shop.Products.ProductInfo _productInfoManage = new BLL.Shop.Products.ProductInfo();
        private BLL.Shop.Products.SKUInfo _skuInfoManage = new BLL.Shop.Products.SKUInfo();
        private YSWL.MALL.BLL.Shop.Coupon.CouponInfo couponBll = new CouponInfo();
        private YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy groupBuyBll = new GroupBuy();
        private YSWL.MALL.BLL.Shop.Activity.ActivityInfo activInfoBll = new BLL.Shop.Activity.ActivityInfo();
        private  YSWL.MALL.BLL.Members.Users userBll=new BLL.Members.Users();
        private  YSWL.MALL.BLL.Shop.Products.BrandInfo brandInfoBll=new YSWL.MALL.BLL.Shop.Products.BrandInfo();
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

            #region 获取基础数据

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

            #region 发票信息 保存到备注中
            string invoiceHeader = context.Request.Form["InvoiceHeader"];
            if (!string.IsNullOrWhiteSpace(invoiceHeader))
            {
                invoiceHeader ="发票抬头："+ InjectionFilter.Filter(invoiceHeader);
            }
            string invoiceContent = context.Request.Form["InvoiceContent"];
            if (!string.IsNullOrWhiteSpace(invoiceContent))
            {
                invoiceContent = "     发票内容："+ InjectionFilter.Filter(invoiceContent);
            }
            string invoice = invoiceHeader + invoiceContent;
            if (!String.IsNullOrWhiteSpace(invoice)) {
                invoice = string.Format(" （{0}）", invoice);
            }
            orderRemark += invoice;
            #endregion

            #endregion

            #region 1.获取购买人

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

            #region 2.获取收货人

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
           
            #region 3.获取购物车

            #region 团购数据
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
                if (item.Quantity < 1 || item.Quantity > YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(item.SKU,shippingAddress.RegionId,item.SupplierId))
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
                //if (shoppingCartHelper != null)
                //{
                //    //ShoppingCartInfo tmpCartInfo = shoppingCartHelper.GetShoppingCart();
                //    noStockList.ForEach(info =>
                //        {
                //            //TODO: 仅自动删除无库存商品 此处需要DB真实库存
                //            //ShoppingCartItem item = tmpCartInfo[info.ItemId];
                //            //if (item==null) return;
                //            //if (info.Quantity >= item.Quantity)
                //            //{
                //            shoppingCartHelper.RemoveItem(info.ItemId);
                //            //}
                //            //else
                //            //{
                //            //    shoppingCartHelper.UpdateItemQuantity(info.ItemId, xx);
                //            //}
                //        });
                //}
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
            #endregion
 
            

            #region 4.获取配送(物流)

            //DONE: 4.获取配送(物流)
            Model.Shop.Shipping.ShippingType shippingType = GetShippingType(context);
            if (shippingType == null)
            {
                result.Accumulate(KEY_STATUS, "NOSHIPPINGTYPE");
                return result.ToString();
            }

            #endregion

            #region 5.生成订单
            //DONE: 5.生成订单
            OrderInfo mainOrder = new OrderInfo();
            #region 填充订单数据

            #region 基础数据

            #region 下单类型
            mainOrder.ReferType = Common.Globals.SafeInt(Common.Cookies.getCookie("submitorder_refertype_" + userBuyer.UserID, "value"), 0);
            #endregion
   
            bool codePre = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_CreateOrder_PreCode");
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


            #region 支付信息

            mainOrder.PaymentTypeId = paymentModeInfo.ModeId;
            mainOrder.PaymentTypeName = paymentModeInfo.Name;
            mainOrder.PaymentGateway = paymentModeInfo.Gateway;

            result.Accumulate("GATEWAY", mainOrder.PaymentGateway);

            #endregion
 
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
            bool IsFreeShippingActiv = false;

            #region 获取促销活动赠品
            List<Model.Shop.Activity.ActivityInfo> actInfoList = null;
            //赠品
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> actProductList = null;
            //赠品 目前没有加运费 没有加重量
            if (proSaleId < 1 && groupBuyId < 1 && acceId < 1)
            {
                actInfoList = activInfoBll.GetActGiftList(shoppingCartInfo, userBuyer.UserID, mainOrder.CouponAmount.HasValue ? mainOrder.CouponAmount.Value : 0);
                if (actInfoList != null)
                {
                    //获取包邮活动
                    List<Model.Shop.Activity.ActivityInfo> freeShippingList = actInfoList.Where(p => p.RuleId == 4).ToList();
                    if (freeShippingList != null && freeShippingList.Count > 0)
                    {
                        //包邮
                        IsFreeShippingActiv = true;
                    }
                    actProductList = YSWL.MALL.BLL.Shop.Activity.ActivityInfo.GetActProList(actInfoList, shippingAddress.RegionId);
                }
            }
            #endregion



            #region 重量/运费/积分

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
                _shippingRegionManage.GetShippingRegion(shippingType.ModeId, topRegionId);
            #endregion

            mainOrder.Weight = shoppingCartInfo.TotalWeight;
            mainOrder.Freight =mainOrder.FreightAdjusted= mainOrder.FreightActual = shoppingCartInfo.CalcFreight(shippingType, shippingRegion);
            if (IsFreeShippingActiv)
            { 
                //包邮
                mainOrder.FreightAdjusted = 0;
                mainOrder.IsFreeShipping =true;  
            }
           
            mainOrder.OrderPoint = shoppingCartInfo.TotalPoints;
            #endregion



            #region 订单价格

            //订单商品总价(无任何优惠)
            mainOrder.ProductTotal = shoppingCartInfo.TotalSellPrice;

            //订单总成本价 = 商品总成本价
            mainOrder.OrderCostPrice = shoppingCartInfo.TotalCostPrice;

            //订单总金额(无任何优惠) 商品总价 + 运费
            mainOrder.OrderTotal = shoppingCartInfo.TotalSellPrice + mainOrder.Freight.Value;

            //订单最终支付金额 = 项目调整后总售价 + 调整后运费-优惠券价格
            decimal amount = shoppingCartInfo.TotalAdjustedPrice + mainOrder.FreightAdjusted.Value - mainOrder.CouponAmount.Value;
            mainOrder.Amount = amount > 0 ? amount : 0;
            //折扣金额
            mainOrder.DiscountAdjusted = shoppingCartInfo.TotalSellPrice - shoppingCartInfo.TotalAdjustedPrice;
            decimal totalRate = shoppingCartInfo.TotalRate; //总价优惠值
            if (mainOrder.Amount < 0)
            {
                LogHelp.AddInvadeLog(
                    string.Format("非法订单金额|{0}|_YSWL.Web.Handlers.Shop.OrderHandler.SubmitOrder",
                        mainOrder.Amount.ToString("F2")), HttpContext.Current.Request);
                result.Accumulate(KEY_STATUS, "ILLEGALORDERAMOUNT");
                return result.ToString();
            }

            #endregion

         
            mainOrder.OrderType = 1;

          
            mainOrder.OrderStatus = 0;//mainOrder.PaymentGateway == "cod" ? 1 : 0;

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

                     Gwjf=item.Gwjf,//添加商城积分进入订单

                    Weight = item.Weight>0?item.Weight:0,
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


            #region 添加赠品
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

                        //添加商城积分进入订单
                        Gwjf = productInfo.Gwjf.HasValue ? Globals.SafeInt(productInfo.Gwjf.Value, 0) : 0,

                        Weight = 0, //productInfo.SkuInfos[0].Weight.HasValue?productInfo.SkuInfos[0].Weight.Value:0,
                        ProductId = productInfo.ProductId,
                        Description = productInfo.Description,
                        CostPrice = productInfo.SkuInfos[0].CostPrice.HasValue ? productInfo.SkuInfos[0].CostPrice.Value : 0,
                        SellPrice =productInfo.SalePrice,// productInfo.SkuInfos[0].SalePrice,// productInfo.SalePrice,//
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

            #region 配送信息(物流)

            mainOrder.ShippingModeId = shippingType.ModeId;
            mainOrder.ShippingModeName = shippingType.Name;
            mainOrder.RealShippingModeId = shippingType.ModeId;
            mainOrder.RealShippingModeName = shippingType.Name;
            mainOrder.ShippingStatus = 0;
            mainOrder.ExpressCompanyName = shippingType.ExpressCompanyName;
            mainOrder.ExpressCompanyAbb = shippingType.ExpressCompanyEn;

            #endregion

            #region 自动拆单
            int subOrderIndex = 1;
            //判断是否购买了多个商家的商品, 并进行拆单
            if (dicSuppOrderItems.Count > 1)
            {
                #region 拆单逻辑
                decimal discountAmount = mainOrder.CouponAmount.HasValue ? mainOrder.CouponAmount.Value : 0;
                var SuppOrderItems = from pair in dicSuppOrderItems orderby pair.Key select pair;
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

                    #region 重新计算 重量/积分/价格

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


                    //DONE: 均摊运费 (根据重量均摊)
                    subOrder.Freight = subOrder.FreightActual = mainOrder.Freight <= 0 ? 0 : (mainOrder.Freight.Value * (decimal)(subOrder.Weight.Value * 1.00 / mainOrder.Weight.Value));
                    subOrder.FreightAdjusted = mainOrder.FreightAdjusted <= 0 ? 0 : (mainOrder.FreightAdjusted.Value * (decimal)(subOrder.Weight.Value * 1.00 / mainOrder.Weight.Value));

                    //订单总金额(含优惠) 商品总价 + 运费
                    subOrder.OrderTotal = subOrder.ProductTotal + subOrder.Freight.Value;
                    //订单最终支付金额 = 商品总价(含优惠) + 调整后运费
                    subOrder.Amount += subOrder.FreightAdjusted.Value;
                    //TODO: 均分主订单的优惠给子订单, 作为退款使用
                    #endregion

                    #region 总价优惠拆单处理
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

                    #region 主订单的优惠券金额给平台
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
                    subOrder.OrderItems = item.Value;

                    subOrder.OrderType = 2;

                    mainOrder.SubOrders.Add(subOrder);
                }
                #endregion
                mainOrder.HasChildren = true;   //有子订单
            }
            else
            {
                //没有购买多个商家的商品
                mainOrder.SupplierId = shoppingCartInfo.Items[0].SupplierId;
                mainOrder.SupplierName = shoppingCartInfo.Items[0].SupplierName;
                mainOrder.HasChildren = false;  //无子订单
            }
            #endregion

            #region 填充主单商家信息
            if (!YSWL.MALL.BLL.Shop.Order.OrderManage.FillSellerInfo(mainOrder))
            {
                result.Accumulate(KEY_STATUS, "NOSUPPLIERINFO");
                return result.ToString();
            }
            #endregion

            #region 订单分销逻辑

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

        #region 获取购物车数据

        protected ShoppingCartInfo GetShoppingCart(HttpContext context, YSWL.Accounts.Bus.User userBuyer,
           out BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper)
        {
            ShoppingCartInfo shoppingCartInfo = null;
            string jsonSkuStr = context.Request.Form["SkuInfos"];
            int proSaleId = Globals.SafeInt(context.Request.Form["ProSaleId"], -1);
            int groupBuyId = Globals.SafeInt(context.Request.Form["GroupBuyId"], -1);
            int acceId = Globals.SafeInt(context.Request.Form["AcceId"], -1);

            #region 推广信息
            string refer = context.Request.Form["Refer"];
            int referId = 0;
            if (!String.IsNullOrWhiteSpace(refer))
            {
                string referStr = YSWL.Common.UrlOper.Base64Decrypt(refer);
                referId = Common.Globals.SafeInt(referStr, 0);
            }

            #endregion
            BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
            if (acceId > 0)//组合优惠套装
            {
                shoppingCartHelper = null;
                BLL.Shop.Products.ProductAccessorie prodAcceBll = new BLL.Shop.Products.ProductAccessorie();
                ProductAccessorie prodAcceModel = prodAcceBll.GetModel(acceId);
                if (prodAcceModel == null || prodAcceModel.Type != 2)
                {
                    return null;
                }
                List<SKUInfo> skulist = skuManage.GetSKUListByAcceId(acceId, 0);
                if (skulist == null || skulist.Count < 2)//每组商品最少有两条数据
                {
                    return null;
                }

                decimal totalPrice = 0;//原　　价
                decimal dealsPrices = 0;//总优惠金额
                foreach (var item in skulist)
                {
                    totalPrice += item.SalePrice;
                }
                dealsPrices = totalPrice - prodAcceModel.DiscountAmount;
                //decimal dealsPrice = dealsPrices / skulist.Count;//单个商品优惠的金额
                shoppingCartInfo = new ShoppingCartInfo();
                ShoppingCartItem cartItem;
                YSWL.MALL.Model.Shop.Products.ProductInfo productInfo;
                foreach (var item in skulist)
                {
                    cartItem = new ShoppingCartItem();
                    cartItem.MarketPrice = item.MarketPrice.HasValue ? item.MarketPrice.Value : 0;
                    cartItem.Name = item.ProductName;
                    cartItem.Quantity = 1;
                    cartItem.SellPrice = item.SalePrice;
                    cartItem.AdjustedPrice = item.SalePrice; // item.SalePrice - dealsPrice;
                    cartItem.SKU = item.SKU;
                    cartItem.ProductId = item.ProductId;
                    cartItem.UserId = userBuyer.UserID;
                   
                    #region 推广信息
                    cartItem.ReferId = referId;
                    #endregion

                    productInfo = _productInfoManage.GetModelByCache(item.ProductId);
                    if (null != productInfo)
                    {
                        cartItem.BrandId = productInfo.BrandId;
                        YSWL.MALL.Model.Shop.Products.BrandInfo brandInfo =
                            brandInfoBll.GetBrandInfo(productInfo.BrandId);
                        cartItem.BrandName = brandInfo == null ? "" : brandInfo.BrandName;
                        cartItem.SaleStatus = productInfo.SaleStatus == 1 ? (item.Upselling ? 1 : 0) : productInfo.SaleStatus;
                    }
                    else
                    {
                        cartItem.BrandId = -1;
                         cartItem.SaleStatus=2;
                    }
                    //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
                    List<SKUItem> listSkuItems = item.SkuItems;
                    if (listSkuItems != null && listSkuItems.Count > 0)
                    {
                        cartItem.SkuValues = new string[listSkuItems.Count];
                        int index = 0;
                        listSkuItems.ForEach(xx =>
                        {
                            cartItem.SkuValues[index++] = xx.ValueStr;
                            if (!string.IsNullOrWhiteSpace(xx.ImageUrl))
                            {
                                cartItem.SkuImageUrl = xx.ImageUrl;
                            }
                        });
                    }
                    //TODO: 未使用SKU缩略图 BEN ADD 2013-06-30
                    cartItem.ThumbnailsUrl = item.ProductThumbnailUrl;
                    cartItem.CostPrice = item.CostPrice.HasValue ? item.CostPrice.Value : 0;
                    cartItem.Weight = (item.Weight.HasValue && item.Weight.Value>0) ? item.Weight.Value : 0;

                    #region 商家
                    if (item.SupplierId > 0)
                    {
                        BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
                        Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(item.SupplierId);
                        if (supplierInfo != null)
                        {
                            cartItem.SupplierId = supplierInfo.SupplierId;
                            cartItem.SupplierName = supplierInfo.Name;
                        }
                    }
                    #endregion

                    shoppingCartInfo.Items.Add(cartItem);
                }
                shoppingCartInfo.TotalRate = dealsPrices;

            }
            else if (string.IsNullOrWhiteSpace(jsonSkuStr))  //判断 是否使用 Cookie 加载购物车
            {
                shoppingCartHelper = new BLL.Shop.Products.ShoppingCartHelper(userBuyer.UserID);
                //DONE: 获取已选中内容的购物车进行 购物车 部分商品 下单 BEN Modify 20130923
                shoppingCartInfo = shoppingCartHelper.GetShoppingCart4Selected();
                //获取库存
                if (shoppingCartInfo != null && shoppingCartInfo.Items!= null)
                {
                    foreach (var item in shoppingCartInfo.Items)
                    {
                        //获取销售状态
                        item.SaleStatus = skuManage.GetSaleStatus(item.SKU);
                    }
                }


            }
            else
            {
                //使用 SKU 加载购物车
                shoppingCartHelper = null;
                //DONE: 目前是 SkuId 应为 SKU | DONE 20130901
                JsonArray jsonSkuArray;
                try
                {
                    jsonSkuArray = Json.Conversion.JsonConvert.Import<JsonArray>(jsonSkuStr);
                }
                catch (Exception ex)
                {
                    LogHelp.AddInvadeLog(
                       string.Format("非法SKU数据|{0}|_Maticsoft.Web.Handlers.Shop.OrderHandler.GetShoppingCart.JsonConvertSku",
                           ex.Message), HttpContext.Current.Request);
                    return null;
                }
                if (jsonSkuArray == null || jsonSkuArray.Length < 1) return null;
                JsonObject jsonSku = jsonSkuArray.GetObject(0);

                //TODO: 暂不支持多SKU下单
                string sku = jsonSku["SKU"].ToString();
                int count = Globals.SafeInt(jsonSku["Count"].ToString(), 1);
                YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo = _skuInfoManage.GetModelBySKU(sku);
                if (skuInfo == null) return null;
                YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = _productInfoManage.GetModel(skuInfo.ProductId);
                if (productInfo == null) return null;
                YSWL.MALL.Model.Shop.Products.ShoppingCartItem itemInfo = new ShoppingCartItem();
                itemInfo.MarketPrice = productInfo.MarketPrice.HasValue ? productInfo.MarketPrice.Value : 0;
                itemInfo.Name = productInfo.ProductName;
                itemInfo.Quantity = count;
                itemInfo.SellPrice = skuInfo.SalePrice;
                itemInfo.AdjustedPrice = skuInfo.SalePrice;
                itemInfo.SKU = skuInfo.SKU;
                itemInfo.ProductId = skuInfo.ProductId;
                itemInfo.UserId = userBuyer.UserID;
              
                #region 品牌信息
                itemInfo.BrandId = productInfo.BrandId;
                YSWL.MALL.Model.Shop.Products.BrandInfo brandInfo =
                         brandInfoBll.GetBrandInfo(productInfo.BrandId);
                itemInfo.BrandName = brandInfo == null ? "" : brandInfo.BrandName;
                #endregion

                #region 限时抢购处理
                if (proSaleId > 0)
                {
                    YSWL.MALL.Model.Shop.Products.ProductInfo proSaleInfo = _productInfoManage.GetProSaleModel(proSaleId);
                    if (proSaleInfo == null) return null;

                    //活动已过期 重定向到单品页
                    if (DateTime.Now > proSaleInfo.ProSalesEndDate)
                        throw new ArgumentNullException("活动已过期");

                    //重置价格为 限时抢购价
                    itemInfo.AdjustedPrice = proSaleInfo.ProSalesPrice;
                }
                #endregion

                #region 团购处理
                if (groupBuyId > 0)
                {
                    YSWL.MALL.Model.Shop.Products.ProductInfo groupBuyInfo = _productInfoManage.GetGroupBuyModel(groupBuyId);
                    if (groupBuyInfo == null) return null;

                    //活动已过期 重定向到单品页
                    if (DateTime.Now > groupBuyInfo.GroupBuy.EndDate)
                        throw new ArgumentNullException("活动已过期");

                    //重置价格为 限时抢购价
                    itemInfo.AdjustedPrice = groupBuyInfo.GroupBuy.Price;
                }
                #endregion

                #region 推广信息
                itemInfo.ReferId = referId;
                #endregion
                //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
                List<Model.Shop.Products.SKUItem> listSkuItems = _skuInfoManage.GetSKUItemsBySkuId(skuInfo.SkuId);
                if (listSkuItems != null && listSkuItems.Count > 0)
                {
                    itemInfo.SkuValues = new string[listSkuItems.Count];
                    int index = 0;
                    listSkuItems.ForEach(xx =>
                    {
                        itemInfo.SkuValues[index++] = xx.ValueStr;
                        if (!string.IsNullOrWhiteSpace(xx.ImageUrl))
                        {
                            itemInfo.SkuImageUrl = xx.ImageUrl;
                        }
                    });
                }

                itemInfo.ThumbnailsUrl = productInfo.ThumbnailUrl1;
                itemInfo.CostPrice = skuInfo.CostPrice.HasValue ? skuInfo.CostPrice.Value : 0;
                itemInfo.Weight = (skuInfo.Weight.HasValue && skuInfo.Weight.Value>0) ? skuInfo.Weight.Value : 0;
                itemInfo.Points = (int)(productInfo.Points.HasValue ? productInfo.Points.Value : 0);

                //把商城积分信息添加到购物订单里面
                itemInfo.Points = (int)(productInfo.Points.HasValue ? productInfo.Points.Value : 0);


                itemInfo.SaleStatus = productInfo.SaleStatus == 1 ? (skuInfo.Upselling ? 1 : 0) : productInfo.SaleStatus;
                #region 商家Id
                BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
                Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(productInfo.SupplierId);
                if (supplierInfo != null)
                {
                    itemInfo.SupplierId = supplierInfo.SupplierId;
                    itemInfo.SupplierName = supplierInfo.Name;
                }
                #endregion

                shoppingCartInfo = new ShoppingCartInfo();
                shoppingCartInfo.Items.Add(itemInfo);
            }

            #region 批销优惠
            if (acceId < 1 && proSaleId < 1 && groupBuyId < 1) //限时抢购/团购/组合优惠套装　不参与批销优惠
            {
                try
                {
                    BLL.Shop.Sales.SalesRuleProduct salesRule = new BLL.Shop.Sales.SalesRuleProduct();
                    shoppingCartInfo = salesRule.GetWholeSale(shoppingCartInfo);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            #endregion

            return shoppingCartInfo;
        }

        #endregion

        #region 获取支付数据
        protected Payment.Model.PaymentModeInfo GetPaymentModeInfo(HttpContext context)
        {
            int paymentModeId = Globals.SafeInt(context.Request.Form["PaymentModeId"], -1);
            if (paymentModeId < 1) return null;
            return Payment.BLL.PaymentModeManage.GetPaymentModeById(paymentModeId);
        }
        #endregion

        #region 获取购买人数据

        protected YSWL.Accounts.Bus.User GetBuyerUserInfo(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return null;
            }
            //  YSWL.Accounts.Bus.User currentUser;
            //这部分信息不要直接读取session 因为这段时间内，用户信息可能会被更改
            YSWL.Accounts.Bus.User currentUser = new YSWL.Accounts.Bus.User(context.User.Identity.Name);
            //if (context.Session[Globals.SESSIONKEY_USER] == null)
            //{
            //    currentUser = new YSWL.Accounts.Bus.User(
            //        new YSWL.Accounts.Bus.AccountsPrincipal(context.User.Identity.Name));
            //    context.Session[Globals.SESSIONKEY_USER] = currentUser;
            //}
            //else
            //{
            //    currentUser = (YSWL.Accounts.Bus.User)context.Session[Globals.SESSIONKEY_USER];
            //}
            return currentUser;
        }

        #endregion

        #region 获取收货人数据

        protected Model.Shop.Shipping.ShippingAddress GetShippingAddress(HttpContext context)
        {
            int shippingAddressId = Globals.SafeInt(context.Request.Form["ShippingAddressId"], -1);
            if (shippingAddressId < 1) return null;
            return _shippingAddressManage.GetModel(shippingAddressId);
        }

        #endregion

        #region 获取配送信息

        private Model.Shop.Shipping.ShippingType GetShippingType(HttpContext context)
        {
            int shippingTypeId = Globals.SafeInt(context.Request.Form["ShippingTypeId"], -1);
            if (shippingTypeId < 1) return null;
            return _shippingTypeManage.GetModel(shippingTypeId);
        }

        #endregion

        //#region 填充卖家信息

        //private bool FillSellerInfo(OrderInfo orderInfo)
        //{
        //    if (orderInfo.SupplierId.HasValue && orderInfo.SupplierId.Value > 0)
        //    {
        //        BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
        //        Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(orderInfo.SupplierId.Value);
        //        if (supplierInfo == null) return false;

        //        orderInfo.SupplierName = supplierInfo.Name;

        //        #region 填充子单卖家信息

        //        orderInfo.SellerID = supplierInfo.UserId;
        //        //DONE: 卖家名称使用店铺名称
        //        orderInfo.SellerName = supplierInfo.ShopName;
        //        orderInfo.SellerEmail = supplierInfo.ContactMail;
        //        orderInfo.SellerCellPhone = supplierInfo.CellPhone;

        //        #endregion
        //    }
        //    else
        //    {
        //        orderInfo.SupplierId = null;
        //        orderInfo.SupplierName = null;

        //        orderInfo.SellerID = null;
        //        orderInfo.SellerName = null;
        //        orderInfo.SellerEmail = null;
        //        orderInfo.SellerCellPhone = null;
        //    }
        //    return true;
        //}

        //#endregion

        #endregion

    }
}