/**
* UserHandler.cs
*
* 功 能： 商城 API
* 类 名： UserHandler
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/12/24 17:04:23  Ben    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using YSWL.Accounts.Bus;
using YSWL.MALL.BLL.Members;
using YSWL.Components.Handlers.API;
using YSWL.Json;
using YSWL.Json.RPC;
using YSWL.MALL.Model.Members;
using YSWL.MALL.Model.Shop.Products;
using System.Collections.Generic;
using YSWL.MALL.Model.Shop.Order;
using YSWL.Common;
using System.Data;
using YSWL.MALL.BLL.Shop.Coupon;
using YSWL.MALL.Model.Ms;
using YSWL.MALL.BLL.Shop.Order;
using Webdiyer.WebControls.Mvc;
using YSWL.MALL.ViewModel.Shop;
using YSWL.MALL.Model.Shop.Shipping;
using YSWL.MALL.Model;
using YSWL.MALL.BLL.SysManage;
using System.Web;
using System.Linq;

namespace YSWL.MALL.API.Shop.v2
{
    public partial class ShopHandler
    {
        private  BLL.Shop.Order.Orders _orderManage = new BLL.Shop.Order.Orders();
        private YSWL.MALL.BLL.Shop.Order.OrderItems itemBll = new YSWL.MALL.BLL.Shop.Order.OrderItems();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
        private YSWL.MALL.BLL.Shop.Activity.ActivityInfo activInfoBll = new BLL.Shop.Activity.ActivityInfo();
        private YSWL.MALL.BLL.Members.Users BLLUser = new YSWL.MALL.BLL.Members.Users();
        private YSWL.MALL.BLL.Shop.Shipping.ShippingAddress _addressManage = new YSWL.MALL.BLL.Shop.Shipping.ShippingAddress();
        private YSWL.MALL.BLL.Ms.Regions RegionBLL = new YSWL.MALL.BLL.Ms.Regions();
        private YSWL.MALL.BLL.Shop.Coupon.CouponInfo couponBLL = new CouponInfo();
        private  YSWL.MALL.BLL.Shop.Shipping.ShippingType _shippingTypeManage = new YSWL.MALL.BLL.Shop.Shipping.ShippingType();
        private  YSWL.MALL.BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage = new YSWL.MALL.BLL.Shop.Shipping.ShippingRegionGroups();
        private YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy groupBuy = new YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy();
        private YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy groupBuyBll = new YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy();
        private YSWL.MALL.BLL.Shop.Order.OrderAction actionBLL = new YSWL.MALL.BLL.Shop.Order.OrderAction();
        #region 订单列表
        [JsonRpcMethod("OrderListV2", Idempotent = false)]
        [JsonRpcHelp("订单列表")]
        public JsonObject OrderList(int page = 1, int pageNum = 10, int UserID = -1,int type=1)//type   1:全部  2:待付款  3:待发货  4:待收货     默认为全部
        {
            if (pageNum < 1)
            {
                pageNum = 10;
            }
            if (page < 1)
            {
                page = 1;
            }
            if (UserID < 1)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            //计算分页起始索引
            int startIndex = page > 1 ? (page - 1) * pageNum + 1 : 0;
            //计算分页结束索引
            int endIndex = page * pageNum;

            string where = " BuyerID=" + UserID +" AND OrderType=1";
            //获取订单类型
            switch (type)
            {
                //待付款
                case 2:
                    where += " AND PaymentStatus = 0 and OrderStatus<> -1 AND PaymentGateway<> 'cod' AND PaymentGateway<> 'bank'";
                    break;
                //待发货
                case 3:
                    where += " AND  ShippingStatus<2 AND OrderStatus!=-1  and ( ( PaymentStatus=2 and PaymentGateway<>'cod' ) or ( PaymentStatus=0 and PaymentGateway='cod') ) ";
                    break;
                //待收货 
                case 4:
                    where += " AND ShippingStatus=2 and OrderStatus=1 ";
                    break;
                case 1:
                default:
                    where += "";
                    break;
            }
            YSWL.Json.JsonArray jsonArray = new JsonArray();
            JsonObject json;
            List<OrderInfo> orderList = _orderManage.GetListByPageEX(where, "", startIndex, endIndex);
            if (orderList ==null || orderList.Count<=0)
            {
                return new Result(ResultStatus.Success, jsonArray);
            }
            int prodTotal;
            JsonArray pList;
            foreach (OrderInfo item in orderList)
            {
                item.OrderItems = itemBll.GetModelList(" OrderId=" + item.OrderId);
                prodTotal = 0;
                pList = new JsonArray();
                json = new JsonObject();
                json.Put("orderid", item.OrderId);
                json.Put("orderCode", item.OrderCode);
               // json.Put("status", YSWL.Common.Globals.SafeEnum<YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus>(item.OrderStatus.ToString(), YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle, true));
                json.Put("mainStatus", _orderManage.GetOrderType(item.PaymentGateway, item.OrderStatus, item.PaymentStatus, item.ShippingStatus));
                //Paying:等待付款,PreHandle:等待处理,Cancel:取消订单,Locking:订单锁定,PreConfirm:等待付款确认,Handling:配货中，Shiped:已发货,Complete:已完成
                //等待付款  可 支付 或 取消 ,  已发货   可 确认收货
                json.Put("mainStatusStr", _orderManage.GetOrderTypeStr(item.PaymentGateway, item.OrderStatus, item.PaymentStatus, item.ShippingStatus));
                json.Put("time", item.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                json.Put("allprice", item.Amount.ToString("F"));
                json.Put("paytypename", item.PaymentTypeName);
                json.Put("paygateway", item.PaymentGateway);
                if (null != item.OrderItems && item.OrderItems.Count > 0)
                {
                    item.OrderItems.ForEach(o =>
                    {
                        prodTotal += o.ShipmentQuantity;
                        pList.Add(o.ThumbnailsUrl);
                    });
                }
                json.Put("prodTotal", prodTotal);
                json.Put("pics", pList);
                //#region 获取订单状态
                //if (item.OrderStatus == 0) //0为未处理 此时可修改可删除
                //{
                //    json.Put("flag", 1);
                //}
                //else if (item.OrderStatus == 2) //已完成
                //{
                //    json.Put("flag", 3);
                //}
                //else//!0都是不可修改的
                //{
                //    json.Put("flag", 2);
                //}
                //#endregion
                jsonArray.Add(json);
            }
            return new Result(ResultStatus.Success, jsonArray);
        }
        #endregion

        #region 最近订购过的商品
        [JsonRpcMethod("OrderedProductListV2", Idempotent = false)]
        [JsonRpcHelp("最近订购商品列表")]
        public JsonObject OrderedProd(int userId, int? page = 1, int pageNum = 30)
        {
            if (pageNum == 0)
            {
                pageNum = 30;
            }
            JsonArray jsonArray = new JsonArray();
            JsonObject json;

            //最近订购近期天数
            int days = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("MBShop_RecentlyOrderedDays"), 7);
            DateTime dt = DateTime.Now;
            DateTime startDate = dt.AddDays(-days).AddDays(1).Date;
            DateTime endDate = dt;
            //重置页面索引
            page = page.HasValue && page.Value > 1 ? page.Value : 1;
            //计算分页起始索引
            int startIndex = page.Value > 1 ? (page.Value - 1) * pageNum + 1 : 0;
            //计算分页结束索引
            int endIndex = page.Value > 1 ? startIndex + pageNum - 1 : pageNum;
            List<Model.Shop.Products.ProductInfo> list = productManage.GetOrderedProdList(userId, startDate, endDate, startIndex, endIndex);
            if (list == null)
            {
                return new Result(ResultStatus.Success, jsonArray);
            }
            List<Model.Shop.Products.SKUInfo> skulist;
            int stock;
            bool openAlertStock = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
            foreach (YSWL.MALL.Model.Shop.Products.ProductInfo item in list)
            {
                skulist = skuBLL.GetProductSkuInfo(item.ProductId);//sku信息
                json = new JsonObject();
                json.Put("id", item.ProductId);
                json.Put("name", item.ProductName);
                json.Put("pic", item.ThumbnailUrl1);
                json.Put("marketprice", item.MarketPrice.HasValue ? item.MarketPrice.Value.ToString("F") : "0.00");
                json.Put("saleprice", item.LowestSalePrice.ToString("F"));
               // json.Put("sku", skuInfoModel.SKU);
                json.Put("ruleType", (int)ruleProductBll.GetRuleType(item.ProductId, userId));  //促销规则形式： -1：没有参与促销 0：打折  1：减价  2：直降
                // json.Put("discount", GetProductSale(item.ProductId, null));
                // json.Put("discountRule", GetAllSaleRule(item.ProductId, null));
                json.Put("producttype", 1);
                json.Put("limitQty", item.RestrictionCount);//限购数量
                if (skulist != null && skulist.Count > 0)
                {
                    stock = 0;
                    if (openAlertStock) //开启警戒库存
                    {
                        skulist.ForEach(info => {
                            if (info.Stock > info.AlertStock)
                            {
                                stock += info.Stock;
                            }
                        });
                        json.Put("hasStock", stock > 0 ? true : false);
                    }
                    else
                    {
                        skulist.ForEach(info => {
                            stock += info.Stock;
                        });
                        json.Put("hasStock", stock > 0 ? true : false);
                    }
                }
                else
                {
                    json.Put("hasStock", false);
                }

                jsonArray.Add(json);
            }
            return new Result(ResultStatus.Success, jsonArray);
        }
        #endregion

        #region 再次订购
        [JsonRpcMethod("BuyAgainV2", Idempotent = false)]
        [JsonRpcHelp("再次订购")]
        public JsonObject BuyAgain(int userId, int? page = 1, int pageNum = 30)
        {
            if (pageNum == 0)
            {
                pageNum = 30;
            }
            JsonArray jsonArray = new JsonArray();
            JsonObject json;

            //再次订购近期天数
            int days = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("MBShop_AgainOrderDays"), 7);
            DateTime dt = DateTime.Now;
            DateTime startDate = dt.AddDays(-days).AddDays(1).Date;
            DateTime endDate = dt;

            string where = string.Format("  BuyerID={0}  and CreatedDate >='{1}' AND CreatedDate<'{2}' ", userId, startDate, endDate);
            //重置页面索引
            page = page.HasValue && page.Value > 1 ? page.Value : 1;
            //计算分页起始索引
            int startIndex = page.Value > 1 ? (page.Value - 1) * pageNum + 1 : 0;
            //计算分页结束索引
            int endIndex = page.Value > 1 ? startIndex + pageNum - 1 : pageNum;
            //总条数
           // int toalCount = _orderManage.GetRecordCount(where);
            //瀑布流Index

            List<Model.Shop.Order.OrderInfo> orderList = _orderManage.GetListByPageEX(where, "", startIndex, endIndex);
            if (orderList == null  || orderList.Count <=0)
            {
                return new Result(ResultStatus.Success, jsonArray);
            }
            int prodTotal;
             JsonArray pList;
            foreach (OrderInfo item in orderList)
            {
                item.OrderItems = itemBll.GetModelList(" OrderId=" + item.OrderId);
                prodTotal = 0;
                pList = new JsonArray();
                json = new JsonObject();
                json.Put("orderid", item.OrderId);
                json.Put("orderCode", item.OrderCode);
                // json.Put("status", YSWL.Common.Globals.SafeEnum<YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus>(item.OrderStatus.ToString(), YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle, true));
                json.Put("mainStatus", _orderManage.GetOrderType(item.PaymentGateway, item.OrderStatus, item.PaymentStatus, item.ShippingStatus));
                //Paying:等待付款,PreHandle:等待处理,Cancel:取消订单,Locking:订单锁定,PreConfirm:等待付款确认,Handling:配货中，Shiped:已发货,Complete:已完成
                //等待付款  可 支付 或 取消 ,  已发货   可 确认收货
                json.Put("mainStatusStr", _orderManage.GetOrderTypeStr(item.PaymentGateway, item.OrderStatus, item.PaymentStatus, item.ShippingStatus));
                json.Put("time", item.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                json.Put("allprice", item.Amount.ToString("F"));
                json.Put("paytypename", item.PaymentTypeName);
                json.Put("paygateway", item.PaymentGateway);
                if (null != item.OrderItems && item.OrderItems.Count > 0)
                {
                    item.OrderItems.ForEach(info =>
                    {
                        prodTotal += info.ShipmentQuantity;
                        pList.Add(info.ThumbnailsUrl);
                    });
                }
                json.Put("pics", pList);
                json.Put("prodTotal", prodTotal);
                jsonArray.Add(json);
            }           
            return new Result(ResultStatus.Success, jsonArray);
        }
        #endregion


        #region 再次订购
        /// <summary>
        ///  根据订单号获取可加入购物车的商品及价格库存信息
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        [JsonRpcMethod("AddCartProductListV2", Idempotent = false)]
        [JsonRpcHelp("获取可加入购物车的商品列表")]
        public JsonObject AddCartProductList(long orderId, int userId, int regionId = 0)
        {
            BLL.Shop.Order.Orders _orderManage = new BLL.Shop.Order.Orders();
         
            #region 验证数据
            if (orderId <= 0)
            {
                return new Result(ResultStatus.Failed, new JsonArray());
            }

            YSWL.MALL.Model.Shop.Order.OrderInfo orderModel = _orderManage.GetModelInfo(orderId);
            //Safe
            if (orderModel == null || orderModel.BuyerID != userId || orderModel.OrderItems == null)
            {
                return new Result(ResultStatus.Failed, new JsonArray());
            }
            #endregion
            JsonObject result = new JsonObject();
            JsonArray jsonArray = new JsonArray();
            JsonObject json;

            JsonObject skustrJson;
            JsonArray skustrArray;
            //是否清空之前的购物车
            bool IsClearShoppingCart = Common.Globals.SafeBool(BLL.SysManage.ConfigSystem.GetValueByCache("OrderProdIsClearShoppingCart"), true);//订购商品是否清空购物车
            result.Put("IsClearShoppingCart", IsClearShoppingCart);
            List<Model.Shop.Products.ProductInfo> list = new List<ProductInfo>();
            YSWL.MALL.Model.Shop.Products.ProductInfo model;
            Model.Shop.Products.SKUInfo skuInfo;
            //是否对接多仓，处理逻辑
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            foreach (YSWL.MALL.Model.Shop.Order.OrderItems item in orderModel.OrderItems)
            {
                #region 获取商品及sku信息
                skuInfo = skuBll.GetModelBySKU(item.SKU);
                //NOSKU
                if (skuInfo == null || !skuInfo.Upselling)
                {
                    continue;
                }
                model = productBll.GetModelByCache(skuInfo.ProductId);
                if (model == null || model.SaleStatus != 1 || model.SalesType != 1)//判断商品状态
                {
                    continue;
                }
                #endregion

                #region 判断库存
                skuInfo.Stock = IsMultiDepot? YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(skuInfo.SKU, regionId, model.SupplierId) : skuInfo.Stock;
                if (skuInfo.Stock == 0)
                {
                    continue;//没有库存
                }
                #endregion
                
                json = new JsonObject();       
                json.Put("id", model.ProductId);
                json.Put("name", model.ProductName);
                json.Put("pic",  model.ThumbnailUrl1);
                json.Put("marketprice", model.MarketPrice.HasValue ? model.MarketPrice.Value.ToString("F") : "0.00");
                json.Put("saleprice", skuInfo.SalePrice.ToString("F"));
                json.Put("sku", skuInfo.SKU);
                json.Put("number", item.Quantity);
                json.Put("stock", skuInfo.Stock);
                skustrArray = new JsonArray();
                //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
                List<Model.Shop.Products.SKUItem> listSkuItems = skuBll.GetSKUItemsBySkuId(skuInfo.SkuId);
                if (listSkuItems != null && listSkuItems.Count > 0)
                {
                   // skuValues = new string[listSkuItems.Count];
                    listSkuItems.ForEach(xx =>
                    {
                        skustrJson = new JsonObject();
                        skustrJson.Put("str", xx.ValueStr);
                        if (!string.IsNullOrWhiteSpace(xx.ImageUrl))
                        {
                            skustrJson.Put("pic", xx.ImageUrl);
                        }
                        else {
                            skustrJson.Put("pic", "");
                        }
                        skustrArray.Add(skustrJson);
                    });
                }
                json.Put("skuValues", skustrArray);
               // json.Put("limitQty", model.RestrictionCount);//限购数量  
                jsonArray.Add(json);
            }
            result.Put("productlist", jsonArray);
            return new Result(ResultStatus.Success, result);
        }
        #endregion


        #region 提交订单
        [JsonRpcMethod("SubmitOrderV2", Idempotent = false)]
        [JsonRpcHelp("提交订单")]
        public JsonObject SubmitOrderV2(int userId, int shipId, int regionId, int shipTypeId, int paymentId, JsonArray productList, string remark, string invoiceHeader, string invoiceContent  , string couponCode, int proSaleId = -1, int groupBuyId = -1)
        {
            JsonObject jsonObject = new JsonObject();
            string orderRemark = remark;
            if (!string.IsNullOrWhiteSpace(orderRemark))
            {
                orderRemark = InjectionFilter.Filter(orderRemark);
            }
            #region 发票信息 保存到备注中
            string invoice = "";
            if (!string.IsNullOrWhiteSpace(invoiceHeader))
            {
                invoice+= "发票抬头：" + InjectionFilter.Filter(invoiceHeader);
            }
            if (!string.IsNullOrWhiteSpace(invoiceContent))
            {
                invoice+= "     发票内容：" + InjectionFilter.Filter(invoiceContent);
            }
            if (!String.IsNullOrWhiteSpace(invoice))
            {
                invoice = string.Format(" （{0}）", invoice);
            }
            orderRemark += invoice;
            #endregion

            ShoppingCartInfo shoppingCartInfo;

            #region 2.购买人的数据
            YSWL.MALL.Model.Members.Users userModel;
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "nopersonInfo");
            }
            else
            {
                userModel = BLLUser.GetModelByCache(userId);
                if (null != userModel)
                {
                    if (userModel.UserType == "AA")
                    {
                        return new Result(ResultStatus.Failed, "status_unthorized");
                    }
                    YSWL.Common.Cookies.setCookie("SubmitOrder_ReferType_" + userModel.UserID,
    ((int)YSWL.MALL.Model.Shop.Order.EnumHelper.ReferType.Ding).ToString(), 1440);
                }
                else
                {
                    return new Result(ResultStatus.Failed, "notlogin");
                }
            }
            #endregion

            #region 4.获取收货人

            if (shipId < 1)
            {
                return new Result(ResultStatus.Failed, "noShippingAddress");
            }
            YSWL.MALL.Model.Shop.Shipping.ShippingAddress shippingAddress = _addressManage.GetModelByCache(shipId);
            if (shippingAddress == null)
            {
                return new Result(ResultStatus.Failed, "noReceiverInfo");
            }
            if (regionId < 1)
            {
                return new Result(ResultStatus.Failed, "noProvinceCity");
            }

            YSWL.MALL.Model.Ms.Regions regionInfo = RegionBLL.GetModelByCache(regionId);
            if (regionInfo == null)
            {
                return new Result(ResultStatus.Failed, "noRehionInfo");
            }
            #endregion


            #region 1.获取购物车    循环传过来的List

            //DONE: 2.获取购物车
            //YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper;
            try
            {
                shoppingCartInfo = GetShoppingCart(productList, userId, proSaleId, groupBuyId, regionId);
            }
            catch (ArgumentNullException)
            {
                return new Result(ResultStatus.Failed, "PROSALEEXPIRED");
            }
            if (shoppingCartInfo == null ||
                shoppingCartInfo.Items == null ||
                shoppingCartInfo.Items.Count < 1)
            {
                return new Result(ResultStatus.Failed, "NOSHOPPINGCARTINFO");
            }
            //DONE: 2.1 Check 商品库存
            List<ShoppingCartItem> noStockList = new List<ShoppingCartItem>();
            JsonArray notstockArray = new JsonArray();
            JsonObject notstockJson = null;
            foreach (ShoppingCartItem item in shoppingCartInfo.Items)
            {
                //检查购买数量是否大于库存
                if (item.Quantity < 1 || item.Quantity >item.Stock)
                {
                    noStockList.Add(item);
                    notstockJson = new JsonObject();
                    notstockJson.Put("id", item.ProductId);
                    notstockJson.Put("name", item.Name);
                    notstockJson.Put("sku", item.SKU);
                    notstockJson.Put("stockcount", 0);
                    notstockArray.Add(notstockJson);
                }
            }
            if (notstockArray.Length > 0)
            {
                ////自动移除Cookie/DB购物车中的无库存项目
                //if (shoppingCartHelper != null)
                //{
                //    noStockList.ForEach(info =>
                //    {
                //        //TODO: 仅自动删除无库存商品 此处需要DB真实库存
                //        shoppingCartHelper.RemoveItem(info.ItemId);

                //    });
                //}
                return new Result(ResultStatus.Failed, notstockArray);
            }
            #endregion

            #region 3.获取支付基础数据
            if (paymentId < 1)
            {
                return new Result(ResultStatus.Failed, "nopaymentmodelinfo");
            }
            YSWL.Payment.Model.PaymentModeInfo paymentModeInfo =
                   YSWL.Payment.BLL.PaymentModeManage.GetPaymentModeById(paymentId);
            if (null == paymentModeInfo)
            {
                return new Result(ResultStatus.Failed, "nopaymentmodelinfo");
            }
            #endregion



            #region 5.获取配送(物流)
            if (shipTypeId < 1)
            {
                return new Result(ResultStatus.Failed, "NOSHIPPINGTYPE");
            }
            YSWL.MALL.Model.Shop.Shipping.ShippingType shippingType = _shippingTypeManage.GetModelByCache(shipTypeId);
            if (shippingType == null)
            {
                return new Result(ResultStatus.Failed, "NOSHIPPINGTYPE");
            }
            #endregion

            #region 6.生成订单
            //DONE: 5.生成订单
            OrderInfo mainOrder = new OrderInfo();
            #region 填充订单数据

            #region 基础数据
            #region 下单类型
            mainOrder.ReferType = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ReferType.Ding;
            #endregion

            mainOrder.CreatedDate = DateTime.Now;
            bool codePre = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_CreateOrder_PreCode");
            mainOrder.CreateUserId = userModel.UserID;
            if (codePre)
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

            #endregion


            #region 优惠券数据

            mainOrder.CouponAmount = 0;
            if (!string.IsNullOrWhiteSpace(couponCode))
            {
                YSWL.MALL.Model.Shop.Coupon.CouponInfo infoModel = couponBLL.GetCouponInfo(couponCode);
                if (infoModel != null && infoModel.Status < 2 && infoModel.Status >= 0 && shoppingCartInfo.TotalAdjustedPrice > infoModel.LimitPrice)//优惠券未使用而且 商品总价大于优惠券限制金额（不含运费）
                {
                    mainOrder.CouponAmount = infoModel.CouponPrice;
                    mainOrder.CouponCode = infoModel.CouponCode;
                    mainOrder.CouponName = infoModel.CouponName;
                    mainOrder.CouponValue = infoModel.CouponPrice;
                    mainOrder.CouponValueType = 1;
                }
            }


            #endregion

            //是否包邮
            bool IsFreeShippingActiv = false;
            #region 获取促销活动赠品
            List<Model.Shop.Activity.ActivityInfo> actInfoList = null;
            //赠品
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> actProductList = null;
            //赠品 目前没有加运费 没有加重量
            if (proSaleId < 1 && groupBuyId < 1)
            {
                actInfoList = activInfoBll.GetActGiftList(shoppingCartInfo, userId, mainOrder.CouponAmount.HasValue ? mainOrder.CouponAmount.Value : 0);

                #region 排除掉赠优惠劵和包邮 2.2版本中没有此功能
                if (actInfoList != null)
                {
                    actInfoList = actInfoList.Where(o => (o.RuleId == 1 || o.RuleId == 2)).ToList();
                }
                #endregion

                if (actInfoList != null)
                {
                    //获取包邮活动
                    List<Model.Shop.Activity.ActivityInfo> freeShippingList = actInfoList.Where(p => p.RuleId == 4).ToList();
                    if (freeShippingList != null && freeShippingList.Count > 0)
                    {
                        //包邮
                        IsFreeShippingActiv = true;
                    }

                    actProductList = YSWL.MALL.BLL.Shop.Activity.ActivityInfo.GetActProList(actInfoList, 0);
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

            YSWL.MALL.Model.Shop.Shipping.ShippingRegionGroups shippingRegion =
                _shippingRegionManage.GetShippingRegion(shippingType.ModeId, topRegionId);
            #endregion

            mainOrder.Weight = shoppingCartInfo.TotalWeight;
            mainOrder.Freight = mainOrder.FreightAdjusted = mainOrder.FreightActual = shoppingCartInfo.CalcFreight(shippingType, shippingRegion);
            if (IsFreeShippingActiv)
            {
                //包邮
                mainOrder.FreightAdjusted = 0;
                mainOrder.IsFreeShipping = true;
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
            decimal amount = shoppingCartInfo.TotalAdjustedPrice + mainOrder.FreightAdjusted.Value - (mainOrder.CouponAmount.HasValue ? mainOrder.CouponAmount.Value : 0);
            mainOrder.Amount = amount > 0 ? amount : 0;
            //折扣金额
            mainOrder.DiscountAdjusted = shoppingCartInfo.TotalSellPrice - shoppingCartInfo.TotalAdjustedPrice;
            decimal totalRate = shoppingCartInfo.TotalRate; //总价优惠值

            if (mainOrder.Amount < 0)
            {
                LogHelp.AddInvadeLog(
                    string.Format("非法订单金额|{0}|_YSWL.Web.Handlers.Shop.OrderHandler.SubmitOrder",
                        mainOrder.Amount.ToString("F2")), HttpContext.Current.Request);
                return new Result(ResultStatus.Failed, "ILLEGALORDERAMOUNT");
            }

            #endregion

            #region 限时抢购
            if (proSaleId > 0)
            {
                YSWL.MALL.Model.Shop.Products.ProductInfo proSaleInfo = productManage.GetProSaleModel(proSaleId);
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
            if (groupBuyId > 0)
            {
                YSWL.MALL.Model.Shop.PromoteSales.GroupBuy buyModel = groupBuy.GetModelByCache(groupBuyId);
                if (buyModel != null)
                {
                    mainOrder.GroupBuyId = buyModel.GroupBuyId;
                    mainOrder.GroupBuyPrice = buyModel.Price;
                    mainOrder.GroupBuyStatus = 1;
                }
            }
            #endregion

            mainOrder.OrderType = 1;


            mainOrder.OrderStatus = 0;

            #endregion

            #region 购买人信息

            mainOrder.BuyerID = userModel.UserID;
            mainOrder.BuyerName = userModel.UserName;// String.IsNullOrWhiteSpace(userModel.TrueName) ? userModel.UserName : userModel.TrueName;
            mainOrder.ReferID = userModel.EmployeeID.ToString();
            //TODO: 用户Email为空时, 暂以默认Email下单 BEN ADD 20130701
            string buyEmail = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_Pay_DefaultEmail");
            buyEmail = String.IsNullOrWhiteSpace(buyEmail) ? "pay@YSWL.com" : buyEmail;
            mainOrder.BuyerEmail = string.IsNullOrWhiteSpace(userModel.Email) ? buyEmail : userModel.Email;
            mainOrder.BuyerCellPhone = userModel.Phone;

            #endregion

            #region 拆单对象
            Dictionary<int, List<YSWL.MALL.Model.Shop.Order.OrderItems>> dicSuppOrderItems = new Dictionary<int, List<YSWL.MALL.Model.Shop.Order.OrderItems>>();
            #endregion

            #region 购物车 -> 订单项目
            YSWL.MALL.Model.Shop.Order.OrderItems tmpOrderItem;
            //购物车 -> 订单项目
            shoppingCartInfo.Items.ForEach(item =>
            {
                tmpOrderItem = new YSWL.MALL.Model.Shop.Order.OrderItems
                {
                    //TODO: 警告: 商品信息根据Cookie获取, 暂未与DB及时同步
                    Name = item.Name,
                    SKU = item.SKU,
                    Quantity = item.Quantity,
                    ShipmentQuantity = item.Quantity,
                    ThumbnailsUrl = item.ThumbnailsUrl,
                    Points = item.Points,
                    Weight = item.Weight > 0 ? item.Weight : 0,
                    ProductId = item.ProductId,
                    Description = item.Description,
                    CostPrice = item.CostPrice,
                    SellPrice = item.SellPrice,
                    AdjustedPrice = item.AdjustedPrice,
                    Deduct = item.SellPrice - item.AdjustedPrice,
                    OrderCode = mainOrder.OrderCode,
                    BrandId = item.BrandId,
                    //商家信息
                    SupplierId = item.SupplierId,
                    SupplierName = item.SupplierName,
                    ProductType = 1
                };

                //将SKU信息记录到订单项目的Attribute中 简单记录 逗号分割, 复杂的可以为Json结构
                if (item.SkuValues != null && item.SkuValues.Length > 0)
                {
                    tmpOrderItem.Attribute = string.Join(",", item.SkuValues);
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
                            new List<YSWL.MALL.Model.Shop.Order.OrderItems> { tmpOrderItem });
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
                            new List<YSWL.MALL.Model.Shop.Order.OrderItems> { tmpOrderItem });
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

                    tmpOrderItem = new Model.Shop.Order.OrderItems()
                    {
                        //TODO: 警告: 商品信息根据Cookie获取, 暂未与DB及时同步
                        Name = productInfo.ProductName,
                        SKU = productInfo.SkuInfos[0].SKU,
                        Quantity = productInfo.Count,
                        ShipmentQuantity = productInfo.Count,
                        ThumbnailsUrl = productInfo.ThumbnailUrl1,
                        Points = productInfo.Points.HasValue ? Globals.SafeInt(productInfo.Points.Value, 0) : 0,
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
                                new List<YSWL.MALL.Model.Shop.Order.OrderItems> { tmpOrderItem });
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
                                new List<YSWL.MALL.Model.Shop.Order.OrderItems> { tmpOrderItem });
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
                foreach (KeyValuePair<int, List<YSWL.MALL.Model.Shop.Order.OrderItems>> item in dicSuppOrderItems)
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
                        //result.Accumulate(KEY_STATUS, "NOSUPPLIERINFO");
                        return new Result(ResultStatus.Failed, "NOSUPPLIERINFO");
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



                    #endregion

                    //订单项目
                    subOrder.OrderItems = item.Value;
                    subOrder.OrderType = 2;
                    mainOrder.SubOrders.Add(subOrder);
                }
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
                return new Result(ResultStatus.Failed, "NOSUPPLIERINFO");
                //  result.Accumulate(KEY_STATUS, "NOSUPPLIERINFO");
                // return result.ToString();
            }
            #endregion

            #region 订单分销逻辑

            #endregion

            #endregion

            #region 执行事务-创建订单
            try
            {
                mainOrder.OrderId = YSWL.MALL.BLL.Shop.Order.OrderManage.CreateOrder(mainOrder);
                if (mainOrder.OrderId > 0)
                {
                    jsonObject.Put("orderid", mainOrder.OrderId);
                    jsonObject.Put("orderCode", mainOrder.OrderCode);
                    jsonObject.Put("allprice", mainOrder.Amount.ToString("F"));
                    jsonObject.Put("paymenttype", mainOrder.PaymentTypeName);

                    #region 生成促销活动优惠劵
                    activInfoBll.GenerateData(mainOrder, actInfoList);
                    #endregion
                    //更新优惠券信息
                    if (!String.IsNullOrWhiteSpace(mainOrder.CouponCode))
                    {
                        couponBLL.UseCoupon(couponCode, mainOrder.BuyerID, mainOrder.BuyerEmail);
                    }

                    //更新团购信息
                    if (groupBuyId > 0)
                    {
                        groupBuyBll.UpdateBuyCount(groupBuyId, shoppingCartInfo.Quantity);
                    }

                    #region 订单邮件推送
                    bool IsOpenEmail = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("System_Open_OrderEmail");
                    if (IsOpenEmail)
                    {
                        YSWL.MALL.BLL.Ms.EmailTemplet templetBll = new YSWL.MALL.BLL.Ms.EmailTemplet();
                        templetBll.SendOrderEmail(mainOrder, userModel.Email);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return new Result(ResultStatus.Error, ex);
                //       LogHelp.AddErrorLog("订单创建失败: " + ex.Message, ex.StackTrace, context.Request);
            }
            #endregion

            #endregion
            return new Result(ResultStatus.Success, jsonObject);
        }
        #endregion

        #region 订单详情
        [JsonRpcMethod("OrderDetailV2", Idempotent = false)]
        [JsonRpcHelp("订单详情")]
        public JsonObject OrderDetailV2(long id = -1, int UserID = -1)
        {
            //Safe
            if (UserID < 1)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            OrderInfo orderModel = _orderManage.GetModelInfo(id);
            if (orderModel == null || orderModel.BuyerID != UserID)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            List<JsonObject> resultList = new List<JsonObject>();
            JsonObject jsonObject = new JsonObject();
            List<JsonObject> jsonList;//=new List<JsonObject>();
            JsonObject jsonItem;
            #region order_follow 部分的信息
            List<YSWL.MALL.Model.Shop.Order.OrderAction> actionList = actionBLL.GetModelList(" OrderId=" + id);
            if (null != actionList && actionList.Count > 0)
            {
                jsonList = new List<JsonObject>();
                foreach (var item in actionList)
                {
                    jsonItem = new JsonObject();
                    jsonItem.Put("time", item.ActionDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    jsonItem.Put("operation", GetActionCode(item.ActionCode));
                    jsonList.Add(jsonItem);
                }
                jsonObject.Put("order_follows", jsonList);
            }
            #endregion

            #region address_info 部分的信息
            JsonObject addressjson = new JsonObject();
            addressjson.Put("name", orderModel.ShipName);
            addressjson.Put("id", orderModel.RegionId);
            if (orderModel.RegionId > 0)
            {
                string fullName = RegionBLL.GetFullNameById4Cache(orderModel.RegionId);
                addressjson.Put("addressArea", fullName);
            }
            addressjson.Put("areaDetail", orderModel.ShipAddress);
            addressjson.Put("phone", orderModel.ShipCellPhone);
            jsonObject.Put("address_info", addressjson);
            #endregion
            jsonObject.Put("orderCode", orderModel.OrderCode);
            jsonObject.Put("mainStatus", _orderManage.GetOrderType(orderModel.PaymentGateway, orderModel.OrderStatus, orderModel.PaymentStatus, orderModel.ShippingStatus));
            //Paying:等待付款,PreHandle:等待处理,Cancel:取消订单,Locking:订单锁定,PreConfirm:等待付款确认,Handling:配货中，Shiped:已发货,Complete:已完成
            //等待付款  可 支付 或 取消 ,  已发货   可 确认收货
            jsonObject.Put("mainStatusStr", _orderManage.GetOrderTypeStr(orderModel.PaymentGateway, orderModel.OrderStatus, orderModel.PaymentStatus, orderModel.ShippingStatus));
            jsonObject.Put("time", orderModel.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            jsonObject.Put("paytypename", orderModel.PaymentTypeName);
            jsonObject.Put("paygateway", orderModel.PaymentGateway);
            jsonObject.Put("shippingName", orderModel.RealShippingModeName);
            jsonObject.Put("shiporderNumber", orderModel.ShipOrderNumber);
           
            #region payment_info 部分的信息
            JsonObject paymentjson = new JsonObject();
            paymentjson.Put("freightAdjusted", orderModel.FreightAdjusted.HasValue ? orderModel.FreightAdjusted.Value.ToString("F") : "0.00");
            paymentjson.Put("productprice", orderModel.ProductTotal.ToString("F"));
            paymentjson.Put("orderprice", orderModel.Amount.ToString("F"));
            paymentjson.Put("returnprice", (orderModel.OrderTotal - orderModel.Amount).ToString("F"));
            jsonObject.Put("payment_info", paymentjson);
            #endregion

            #region productlist 部分的信息
            if (orderModel.OrderItems.Count > 0)
            {
                jsonList = new List<JsonObject>();
                foreach (var item in orderModel.OrderItems)
                {
                    jsonItem = new JsonObject();
                    jsonItem.Put("id", item.ProductId);
                    jsonItem.Put("name", item.Name);
                    jsonItem.Put("shopCarColorKey", item.Attribute);

                    jsonItem.Put("pic", item.ThumbnailsUrl);

                    YSWL.MALL.Model.Shop.Products.ProductInfo pro = productManage.GetModelByCache(item.ProductId);
                    if (null != pro)
                    {
                        jsonItem.Put("marketprice", pro.MarketPrice.HasValue ? pro.MarketPrice.Value.ToString("F") : "0.00");
                    }
                    else
                    {
                        jsonItem.Put("marketprice", "0.00");
                    }
                    jsonItem.Put("saleprice", item.SellPrice.ToString("F"));
                    jsonItem.Put("number", item.Quantity);
                    jsonList.Add(jsonItem);
                }
                jsonObject.Put("productlist", jsonList);
            }
            #endregion
            return new Result(ResultStatus.Success, jsonObject);
        }

        //private string GetStr(string Attribute)
        //{
        //    string r = "";
        //    if (!string.IsNullOrWhiteSpace(Attribute))
        //    {
        //        string[] tmpAttr = Attribute.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //        foreach (string val in tmpAttr)
        //        {
        //            r += val + "   ";
        //        }
        //    }
        //    return r;
        //}
        #endregion


        #region 获取购物车列表
        [JsonRpcMethod("GetCartListV2", Idempotent = false)]
        [JsonRpcHelp("获取购物车列表")]  //这个接口后期需要拆分
        public JsonObject GetCartListV2(JsonArray productList,int userId)
        {
            if (productList == null)
            {
                return new Result(ResultStatus.Failed, null);
            }
            JsonObject result = new JsonObject();
            JsonArray array = new JsonArray(); ;
            JsonObject json;
            ShoppingCartInfo shoppingCartInfo;
            try
            {
                shoppingCartInfo = GetCartList(productList, userId);
            }
            catch (ArgumentNullException)
            {
                return new Result(ResultStatus.Failed, "PROSALEEXPIRED");
            }
            if (shoppingCartInfo == null ||
                shoppingCartInfo.Items == null ||
                shoppingCartInfo.Items.Count < 1)
            {
                return new Result(ResultStatus.Failed, "NOSHOPPINGCARTINFO");
            }
            bool openAlertStock = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
            foreach (var item in shoppingCartInfo.Items) {
                json = new JsonObject();
                json.Put("id", item.ProductId);
                json.Put("name", item.Name);
                json.Put("number", item.Quantity);

                json.Put("pic", item.ThumbnailsUrl);

                json.Put("marketprice", item.MarketPrice.ToString("F"));
                json.Put("saleprice", item.SellPrice.ToString("F"));
                json.Put("adjustedPrice", item.AdjustedPrice.ToString("F"));
                json.Put("sku", item.SKU);
                json.Put("limitQty", item.RestrictionCount);//限购数量
                if (openAlertStock) //开启警戒库存
                {
                    if (item.Stock > item.AlertStock)
                    {
                        json.Put("hasStock", true);
                        json.Put("stockcount", item.Stock);
                    }
                    else
                    {
                        json.Put("hasStock", false);
                    }
                }
                else
                {
                    if (item.Stock > 0)
                    {
                        json.Put("hasStock", true);
                        json.Put("stockcount", item.Stock);
                    }
                    else
                    {
                        json.Put("hasStock", false);
                    }
                }
                json.Put("saleStatus", item.SaleStatus);//商品上下架状态
                json.Put("saleDes", item.SaleDes);//优惠信息
                array.Add(json);
            }
            result.Put("productList", array);
            result.Put("totalRate", shoppingCartInfo.TotalRate);//总价优惠
            result.Put("totalSellPrice", shoppingCartInfo.TotalSellPrice);//优惠前价格
            result.Put("totalAdjustedPrice", shoppingCartInfo.TotalAdjustedPrice);//调整后价格
            return new Result(ResultStatus.Success, result);
        }
        #endregion
        private ShoppingCartInfo GetCartList(JsonArray jsonSku, int userId)
        {
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            JsonObject skuItem;
            if (null != jsonSku && jsonSku.Length > 0)
            {
                for (int i = 0; i < jsonSku.Length; i++)
                {
                    skuItem = jsonSku.GetObject(i);
                    string sku = skuItem["sku"].ToString();
                    int count = Globals.SafeInt(skuItem["number"].ToString(), 1);
                    YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo = skuBLL.GetModelBySKU(sku);
                    if (skuInfo == null) continue; //异常数据
                    YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = productManage.GetModel(skuInfo.ProductId);
                    if (productInfo == null) continue; //异常数据
                    YSWL.MALL.Model.Shop.Products.ShoppingCartItem itemInfo = new ShoppingCartItem();
                    itemInfo.MarketPrice = productInfo.MarketPrice.HasValue ? productInfo.MarketPrice.Value : 0;
                    itemInfo.Name = productInfo.ProductName;
                    itemInfo.Quantity = count;
                    itemInfo.SellPrice = skuInfo.SalePrice;
                    itemInfo.AdjustedPrice = skuInfo.SalePrice;
                    itemInfo.SKU = skuInfo.SKU;
                    itemInfo.Stock = skuInfo.Stock;
                    itemInfo.AlertStock = skuInfo.AlertStock;
                    itemInfo.ProductId = skuInfo.ProductId;
                    itemInfo.UserId = userId;
                    itemInfo.BrandId = productInfo.BrandId;
                    itemInfo.SaleStatus = productInfo.SaleStatus;
                    //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
                    //List<YSWL.MALL.Model.Shop.Products.SKUItem> listSkuItems = skuBLL.GetSKUItemsBySkuId(skuInfo.SkuId);
                    //if (listSkuItems != null && listSkuItems.Count > 0)
                    //{
                    //    itemInfo.SkuValues = new string[listSkuItems.Count];
                    //    int index = 0;
                    //    listSkuItems.ForEach(xx =>
                    //    {
                    //        itemInfo.SkuValues[index++] = xx.ValueStr;
                    //        if (!string.IsNullOrWhiteSpace(xx.ImageUrl))
                    //        {
                    //            itemInfo.SkuImageUrl = xx.ImageUrl;
                    //        }
                    //    });
                    //}

                    itemInfo.ThumbnailsUrl = productInfo.ThumbnailUrl1;
                    itemInfo.CostPrice = skuInfo.CostPrice.HasValue ? skuInfo.CostPrice.Value : 0;
                    itemInfo.Weight = skuInfo.Weight.HasValue ? skuInfo.Weight.Value : 0;
                    itemInfo.Points = (int)(productInfo.Points.HasValue ? productInfo.Points.Value : 0);
                    itemInfo.SupplierId = productInfo.SupplierId;
                    itemInfo.RestrictionCount = productInfo.RestrictionCount;
                    shoppingCartInfo.Items.Add(itemInfo);

                
                }
                #region 批销优惠
                    try
                    {
                        YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct salesRule =
                            new YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct();
                        shoppingCartInfo = salesRule.GetWholeSale(shoppingCartInfo);
                    }
                    catch (System.Exception ex)
                    {
                        throw new Exception("获取批销优惠异常,位置：" + ex.StackTrace);
                    }
                #endregion
                return shoppingCartInfo;
            }
            return null;
        }
        private ShoppingCartInfo GetShoppingCart(JsonArray jsonSku, int userId, int prosaleId = -1, int groupbuyId = -1, int regionId = 0)
        {
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            JsonObject skuItem;
            //是否对接多仓，处理逻辑
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            if (null != jsonSku && jsonSku.Length > 0)
            {
                for (int i = 0; i < jsonSku.Length; i++)
                {
                    skuItem = jsonSku.GetObject(i);
                    string sku = skuItem["SKU"].ToString();
                    int count = Globals.SafeInt(skuItem["Count"].ToString(), 1);
                    YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo = skuBLL.GetModelBySKU(sku);
                    if (skuInfo == null) return null;
                    YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = productManage.GetModel(skuInfo.ProductId);
                    if (productInfo == null) return null;
                    YSWL.MALL.Model.Shop.Products.ShoppingCartItem itemInfo = new ShoppingCartItem();
                    itemInfo.MarketPrice = productInfo.MarketPrice.HasValue ? productInfo.MarketPrice.Value : 0;
                    itemInfo.Name = productInfo.ProductName;
                    itemInfo.Quantity = count;
                    itemInfo.SellPrice = skuInfo.SalePrice;
                    itemInfo.AdjustedPrice = skuInfo.SalePrice;
                    itemInfo.SKU = skuInfo.SKU;
                    itemInfo.ProductId = skuInfo.ProductId; 
                    itemInfo.UserId = userId;
                    itemInfo.BrandId = productInfo.BrandId;

                    itemInfo.Stock = IsMultiDepot?YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(sku, regionId, productInfo.SupplierId) : skuInfo.Stock;   //获取分仓库存
                    #region 限时抢购处理

                    if (prosaleId > 0 && groupbuyId > 0) return null;//既是限时抢购又是团购
                    if (prosaleId > 0)
                    {
                        YSWL.MALL.Model.Shop.Products.ProductInfo proSaleInfo = productManage.GetProSaleModel(prosaleId);
                        if (proSaleInfo == null) return null;

                        //活动已过期 重定向到单品页
                        if (DateTime.Now > proSaleInfo.ProSalesEndDate)
                            throw new ArgumentNullException("活动已过期");

                        //重置价格为 限时抢购价
                        itemInfo.AdjustedPrice = proSaleInfo.ProSalesPrice;
                    }

                    #endregion

                    #region 团购处理

                    if (groupbuyId > 0)
                    {
                        YSWL.MALL.Model.Shop.Products.ProductInfo groupBuyInfo =
                            productManage.GetGroupBuyModel(groupbuyId);
                        if (groupBuyInfo == null) return null;

                        //活动已过期 重定向到单品页
                        if (DateTime.Now > groupBuyInfo.GroupBuy.EndDate)
                            throw new ArgumentNullException("活动已过期");

                        //重置价格为 限时抢购价
                        itemInfo.AdjustedPrice = groupBuyInfo.GroupBuy.Price;
                    }

                    #endregion


                    //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
                    List<YSWL.MALL.Model.Shop.Products.SKUItem> listSkuItems = skuBLL.GetSKUItemsBySkuId(skuInfo.SkuId);
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
                    itemInfo.Weight = skuInfo.Weight.HasValue ? skuInfo.Weight.Value : 0;
                    itemInfo.Points = (int)(productInfo.Points.HasValue ? productInfo.Points.Value : 0);

                    #region 商家Id

                    YSWL.MALL.BLL.Shop.Supplier.SupplierInfo supplierManage =
                        new YSWL.MALL.BLL.Shop.Supplier.SupplierInfo();
                    YSWL.MALL.Model.Shop.Supplier.SupplierInfo supplierInfo =
                        supplierManage.GetModelByCache(productInfo.SupplierId);
                    if (supplierInfo != null)
                    {
                        itemInfo.SupplierId = supplierInfo.SupplierId;
                        itemInfo.SupplierName = supplierInfo.Name;
                    }

                    #endregion

                    shoppingCartInfo.Items.Add(itemInfo);

                }
                #region 批销优惠
                if (prosaleId < 1 && groupbuyId < 1) //限时抢购/团购/组合优惠套装　不参与批销优惠
                {
                    try
                    {
                        YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct salesRule =
                            new YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct();
                        shoppingCartInfo = salesRule.GetWholeSale(shoppingCartInfo);
                    }
                    catch (System.Exception ex)
                    {
                        throw new Exception("获取批销优惠异常,位置：" + ex.StackTrace);
                    }
                }

                #endregion
                return shoppingCartInfo;
            }
            return null;
        }

        #region 
        /// <summary>
        /// 获取调整后总金额 
        /// </summary>
        /// <returns></returns>
        [JsonRpcMethod("GetTotalPriceV2", Idempotent = false)]
        [JsonRpcHelp("获取金额")]
        public JsonObject GetTotalPriceV2(int userId, JsonArray productList, int proSaleId = -1, int groupBuyId = -1)
        {
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "USERDATAERROR");
            }
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            try
            {
                shoppingCartInfo = GetShoppingCart(productList, userId, proSaleId, groupBuyId);
            }
            catch (ArgumentNullException)
            {
                return new Result(ResultStatus.Failed, "PROSALEEXPIRED");
            }
            if (shoppingCartInfo == null ||
               shoppingCartInfo.Items == null ||
               shoppingCartInfo.Items.Count < 1)
            {
                return new Result(ResultStatus.Failed, "NOSHOPPINGCARTINFO");
            }
            JsonObject json = new JsonObject();
            json.Put("totalAdjustedPrice", shoppingCartInfo.TotalAdjustedPrice.ToString());
            json.Put("totalSellPrice", shoppingCartInfo.TotalSellPrice.ToString());
            return new Result(ResultStatus.Success, shoppingCartInfo.TotalAdjustedPrice.ToString());
        }
        #endregion

    }
}