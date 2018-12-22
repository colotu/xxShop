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

namespace YSWL.MALL.API.Shop.v1
{
    public partial class ShopHandler
    {
        private readonly YSWL.MALL.BLL.Ms.Regions _regionManage = new YSWL.MALL.BLL.Ms.Regions();
        private readonly YSWL.MALL.BLL.Shop.Shipping.ShippingAddress _shippingAddressManage =
           new YSWL.MALL.BLL.Shop.Shipping.ShippingAddress();
        private readonly YSWL.MALL.BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage =
            new YSWL.MALL.BLL.Shop.Shipping.ShippingRegionGroups();
        private readonly YSWL.MALL.BLL.Shop.Shipping.ShippingType _shippingTypeManage = new YSWL.MALL.BLL.Shop.Shipping.ShippingType();
        private YSWL.MALL.BLL.Ms.Regions RegionBLL = new YSWL.MALL.BLL.Ms.Regions();
        private YSWL.MALL.BLL.Shop.Coupon.CouponInfo couponBLL = new CouponInfo();
        private readonly YSWL.MALL.BLL.Shop.Shipping.ShippingAddress _addressManage = new YSWL.MALL.BLL.Shop.Shipping.ShippingAddress();
        YSWL.MALL.BLL.Members.Users BLLUser = new YSWL.MALL.BLL.Members.Users();
        private YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy groupBuy = new YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy();
        private YSWL.MALL.BLL.Shop.Order.OrderAction actionBLL = new YSWL.MALL.BLL.Shop.Order.OrderAction();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();
        private YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy groupBuyBll = new YSWL.MALL.BLL.Shop.PromoteSales.GroupBuy();
        private YSWL.MALL.BLL.Shop.Activity.ActivityInfo activInfoBll = new BLL.Shop.Activity.ActivityInfo();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new BLL.Shop.Products.ProductInfo();
        #region GetCartInfo4SKU
        private ShoppingCartInfo GetCartInfo4SKU(int UserID, ProductInfo productInfo, SKUInfo skuInfo, int quantity, ProductInfo proSaleInfo = null, ProductInfo groupBuyInfo = null)
        {
            ShoppingCartInfo cartInfo = new ShoppingCartInfo();
            //TODO: 未支持多个SKU BEN ADD 2013-06-23
            ShoppingCartItem cartItem = new ShoppingCartItem();
            cartItem.MarketPrice = productInfo.MarketPrice.HasValue ? productInfo.MarketPrice.Value : 0;
            cartItem.Name = productInfo.ProductName;
            cartItem.Quantity = quantity < 1 ? 1 : quantity;
            cartItem.SellPrice = skuInfo.SalePrice;
            cartItem.AdjustedPrice = skuInfo.SalePrice;
            cartItem.SKU = skuInfo.SKU;
            cartItem.ProductId = skuInfo.ProductId;
            cartItem.UserId = UserID;

            #region 限时抢购价格处理
            if (proSaleInfo != null)
            {
                //重置价格为 限时抢购价
                cartItem.AdjustedPrice = proSaleInfo.ProSalesPrice;
            }
            #endregion

            #region 团购价格处理
            if (groupBuyInfo != null)
            {
                //重置价格为 限时抢购价
                cartItem.AdjustedPrice = groupBuyInfo.GroupBuy.Price;
            }
            #endregion

            #region 商家
            if (productInfo.SupplierId > 0)
            {
                YSWL.MALL.BLL.Shop.Supplier.SupplierInfo supplierManage = new YSWL.MALL.BLL.Shop.Supplier.SupplierInfo();
                YSWL.MALL.Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(productInfo.SupplierId);
                if (supplierInfo != null)
                {
                    cartItem.SupplierId = supplierInfo.SupplierId;
                    cartItem.SupplierName = supplierInfo.Name;
                }
            }
            #endregion

            YSWL.MALL.BLL.Shop.Products.SKUInfo skuManage = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
            //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
            List<SKUItem> listSkuItems = skuManage.GetSKUItemsBySkuId(skuInfo.SkuId);
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
            cartItem.ThumbnailsUrl = productInfo.ThumbnailUrl1;

            cartItem.CostPrice = skuInfo.CostPrice.HasValue ? skuInfo.CostPrice.Value : 0;
            cartItem.Weight = skuInfo.Weight.HasValue ? skuInfo.Weight.Value : 0;
            cartItem.Points = (int)(productInfo.Points.HasValue ? productInfo.Points : 0);
            cartInfo.Items.Add(cartItem);
            return cartInfo;
        }
        #endregion

        #region
        [JsonRpcMethod("DelShippAddress", Idempotent = false)]
        [JsonRpcHelp("删除地址")]
        public JsonObject DelShippAddress(int id, int UserID)
        {
            if (id < 1)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }

            YSWL.MALL.BLL.Shop.Shipping.ShippingAddress addressManage = new YSWL.MALL.BLL.Shop.Shipping.ShippingAddress();
            YSWL.MALL.Model.Shop.Shipping.ShippingAddress model = addressManage.GetModel(id);
            if (model != null && UserID == model.UserId)
            {
                try
                {
                    addressManage.Delete(id);
                    return new Result(ResultStatus.Success, "Success");
                }
                catch (Exception ex)
                {
                    YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message), ex.StackTrace, Request);
                    return new Result(ResultStatus.Error, ex);
                }

            }
            return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
        }
        #endregion

        #region 取消订单
        [JsonRpcMethod("CancelOrder", Idempotent = true)]
        [JsonRpcHelp("取消订单")]
        public JsonObject CancelOrder(long Id, int UserID)
        {
            if (UserID <= 0)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            User currentUser = new User();
            YSWL.MALL.Model.Members.Users userModel = BLLUser.GetModelByCache(UserID);
            if (null != userModel)
            {
                currentUser = new YSWL.Accounts.Bus.User(
                    new YSWL.Accounts.Bus.AccountsPrincipal(userModel.UserName));
            }
            JsonObject json = new JsonObject();
            Orders orderBLL = new Orders();
            long orderId = Globals.SafeLong(Id, 0);
            OrderInfo orderInfo = orderBLL.GetModelInfo(orderId);
            if (orderInfo == null || orderInfo.BuyerID != UserID)
            {
                json.Put("result", "Error");
            }

            if (OrderManage.CancelOrder(orderInfo, currentUser))
            {
                json.Put("result", "Success");
            }
            return new Result(ResultStatus.Success, json);
        }
        #endregion

        #region 订单详情
        [JsonRpcMethod("OrderDetail", Idempotent = false)]
        [JsonRpcHelp("订单详情")]
        public JsonObject OrderDetail(long id = -1, int UserID = -1)
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
            string strKey= YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_App_Product_Attr") ?? "颜色，规格";
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

            #region 获取订单状态

            if (orderModel.OrderStatus == 0 && orderModel.PaymentStatus == 0 && orderModel.PaymentGateway != "cod" && orderModel.PaymentGateway != "bank") //0为未处理 此时可修改可删除
           {
                jsonObject.Put("flag", 1);
            }else if (orderModel.OrderStatus == 2) //已完成
            {
                jsonObject.Put("flag", 3);
            }
            else//!0都是不可修改的
            {
                jsonObject.Put("flag", 2);
            }
            #endregion
            string   statusStr = YSWL.Common.Globals.SafeEnum<YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus>(orderModel.OrderStatus.ToString(), YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle, true).ToString();
            if (orderModel.OrderStatus == (int)Model.Shop.Order.EnumHelper.OrderStatus.UnHandle)//未处理  //由于ios没有加 货到付款的判断  接口临时做处理 后期整理
            {
                //未支付  不是货到付款
                if (orderModel.PaymentStatus == (int)Model.Shop.Order.EnumHelper.PaymentStatus.Unpaid && orderModel.PaymentGateway != "cod" && orderModel.PaymentGateway != "bank")
                {
                    statusStr = Model.Shop.Order.EnumHelper.OrderStatus.UnHandle.ToString();//可以支付或取消
                }
                else
                {
                    statusStr = Model.Shop.Order.EnumHelper.OrderStatus.Handling.ToString();
                }
            }
            jsonObject.Put("status", statusStr);
           // jsonObject.Put("status", YSWL.Common.Globals.SafeEnum<YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus>(orderModel.OrderStatus.ToString(), YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle, true));
            jsonObject.Put("paytypename", orderModel.PaymentTypeName);
            jsonObject.Put("paygateway", orderModel.PaymentGateway);

            #region payment_info 部分的信息
            JsonObject paymentjson = new JsonObject();
            paymentjson.Put("yunfei", orderModel.FreightAdjusted.HasValue ? orderModel.FreightAdjusted.Value.ToString("F") : "0.00");
            paymentjson.Put("productprice", orderModel.ProductTotal.ToString("F"));
            paymentjson.Put("orderprice", orderModel.Amount.ToString("F"));
            paymentjson.Put("returnprice", (orderModel.OrderTotal - orderModel.Amount).ToString("F"));
            jsonObject.Put("payment_info", paymentjson);
            #endregion

            #region productlist 部分的信息
            if (orderModel.OrderItems.Count > 0)
            {
                keys = strKey.Split(',');
                jsonList = new List<JsonObject>();
                foreach (var item in orderModel.OrderItems)
                {
                    jsonItem = new JsonObject();
                    jsonItem.Put("id", item.ProductId);
                    jsonItem.Put("name", item.Name);
                    jsonItem.Put("shopCarColorKey", GetSKUStr(item.SKU));
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
        #endregion

        #region 获得订单追踪状态码信息
        /// <summary>
        /// 获得订单追踪状态码信息
        /// </summary>
        /// <param name="actionCode"></param>
        /// <returns></returns>
        protected string GetActionCode(object actionCode)
        {
            if (actionCode == null)
            {
                return "";
            }
            return YSWL.MALL.BLL.Shop.Order.OrderAction.GetActionCode(actionCode.ToString());
        }
        #endregion

        #region 订单列表
        [JsonRpcMethod("OrderList", Idempotent = false)]
        [JsonRpcHelp("订单列表")]
        public JsonObject OrderList(int page = 1, int pageNum = 10, int UserID = -1)
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
            Orders orderBLL = new Orders();
            YSWL.MALL.BLL.Shop.Order.OrderItems itemBLL = new YSWL.MALL.BLL.Shop.Order.OrderItems();

            //计算分页起始索引
            int startIndex = page > 1 ? (page - 1) * pageNum + 1 : 0;

            //计算分页结束索引
            int endIndex = page * pageNum;
            int toalCount = 0;

            string where = " BuyerID=" + UserID +
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
            toalCount = orderBLL.GetRecordCount(where);
            if (toalCount < 1)
            {
                return new Result(ResultStatus.Success, null);
            }
            YSWL.Json.JsonArray jsonArray = new JsonArray();
            JsonObject json;
            //result.Put("list_count", toalCount);
            List<OrderInfo> orderList = orderBLL.GetListByPageEX(where, "", startIndex, endIndex);
            if (orderList != null && orderList.Count > 0)
            {
                foreach (OrderInfo item in orderList)
                {
                    item.OrderItems = itemBLL.GetModelList(" OrderId=" + item.OrderId);
                }
            }
            PagedList<OrderInfo> lists = new PagedList<OrderInfo>(orderList, page, pageNum, toalCount);
            string statusStr;
            foreach (OrderInfo item in lists)
            {
                json = new JsonObject();
                json.Put("orderid", item.OrderId);
                json.Put("orderCode", item.OrderCode);
                //对应  api 在线支付的订单 在APP端仍然显示 未支付的问题  临时处理
                statusStr = YSWL.Common.Globals.SafeEnum<YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus>(item.OrderStatus.ToString(), YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle, true).ToString();
                if (item.OrderStatus== (int)Model.Shop.Order.EnumHelper.OrderStatus.UnHandle)//未处理  //由于ios没有加 货到付款的判断  接口临时做处理 后期整理
                {
                    //未支付  不是货到付款
                    if (item.PaymentStatus == (int)Model.Shop.Order.EnumHelper.PaymentStatus.Unpaid && item.PaymentGateway != "cod" && item.PaymentGateway != "bank")
                    {
                        statusStr = Model.Shop.Order.EnumHelper.OrderStatus.UnHandle.ToString();//可以支付或取消
                    }
                    else {
                        statusStr = Model.Shop.Order.EnumHelper.OrderStatus.Handling.ToString();
                    }
                }
                json.Put("status", statusStr);
                json.Put("time", item.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                json.Put("allprice", item.Amount.ToString("F"));
                json.Put("paytypename", item.PaymentTypeName);
                json.Put("paygateway", item.PaymentGateway);
               // 
                JsonArray pList;
                #region 拆单情况先不考虑
                //if (null != item.SubOrders && item.SubOrders.Count > 0)//拆单情况先不考虑
                //{
                //    foreach (var orderInfo in item.SubOrders)
                //    {
                //        if (null != orderInfo.OrderItems && orderInfo.OrderItems.Count > 0)
                //        {
                //            JsonObject picJson=new JsonObject();
                //            //json.Put("pic");
                //            foreach (var itemPic in orderInfo.OrderItems)
                //            {
                //                picJson.Put("img",itemPic.ThumbnailsUrl);
                //            }
                //        }
                //    }
                //} 
                #endregion
                if (null != item.OrderItems && item.OrderItems.Count > 0)//有图片的情况
                {
                    pList = new JsonArray();
                    foreach (var itemPic in item.OrderItems)
                    {
                        pList.Add(itemPic.ThumbnailsUrl);
                    }
                    json.Put("pics", pList);
                }
                else
                {
                    json.Put("pics", null);
                }

                #region 获取订单状态
                if (item.OrderStatus == 0 && item.PaymentStatus ==0 && item.PaymentGateway != "cod" && item.PaymentGateway != "bank") //0为未处理 此时可修改可删除
                {
                        json.Put("flag", 1);
                }
                else if (item.OrderStatus == 2) //已完成
                {
                    json.Put("flag", 3);
                }
                else//!0都是不可修改的
                {
                    json.Put("flag", 2);
                }
                #endregion
                // json.Put("ShipName", item.ShipName);
                jsonArray.Add(json);
            }
            return new Result(ResultStatus.Success, jsonArray);
        }
        #endregion

        #region 提交订单
        [JsonRpcMethod("SubmitOrder", Idempotent = false)]
        [JsonRpcHelp("提交订单")]
        public JsonObject SubmitOrder(int userId, int shipId, int regionId, int shipTypeId, int paymentId, JsonArray productList, string remark, string couponCode, int proSaleId = -1, int groupBuyId = -1)
        {
            JsonObject jsonObject = new JsonObject();
            string orderRemark = remark;
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
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper;
            try
            {
                shoppingCartInfo = GetShoppingCart(productList, userId, out shoppingCartHelper, proSaleId, groupBuyId);
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
                if (item.Quantity < 1 || item.Quantity > skuBLL.GetStockBySKU(item.SKU, regionId))
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
                //自动移除Cookie/DB购物车中的无库存项目
                if (shoppingCartHelper != null)
                {
                    noStockList.ForEach(info =>
                    {
                        //TODO: 仅自动删除无库存商品 此处需要DB真实库存
                        shoppingCartHelper.RemoveItem(info.ItemId);

                    });
                }
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
           mainOrder.CreateUserId=userModel.UserID;
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
                if (infoModel != null &&  infoModel.Status < 2 && infoModel.Status>=0 && shoppingCartInfo.TotalAdjustedPrice > infoModel.LimitPrice)//优惠券未使用而且 商品总价大于优惠券限制金额（不含运费）
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

                    actProductList = YSWL.MALL.BLL.Shop.Activity.ActivityInfo.GetActProList(actInfoList,0);
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

            //DONE: 货到付款 下单后直接进入 6.正在处理 流程 其它流程不变 BEN MODIFY 20131205
            mainOrder.OrderStatus = mainOrder.PaymentGateway == "cod" ? 1 : 0;

            #endregion

            #region 购买人信息

            mainOrder.BuyerID = userModel.UserID;
            mainOrder.BuyerName = userModel.UserName;
            mainOrder.ReferID = userModel.EmployeeID.ToString();
            //TODO: 用户Email为空时, 暂以默认Email下单 BEN ADD 20130701
            string buyEmail = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_Pay_DefaultEmail");
            buyEmail = String.IsNullOrWhiteSpace(buyEmail) ? "pay@YSWL.com" : buyEmail;
            mainOrder.BuyerEmail = string.IsNullOrWhiteSpace(userModel.Email) ?buyEmail: userModel.Email;
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
                    Weight = item.Weight>0?item.Weight:0,
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

                    tmpOrderItem = new Model.Shop.Order.OrderItems ()
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
                        templetBll.SendOrderEmail(mainOrder,userModel.Email);
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

        #region
        [JsonRpcMethod("GetPayList", Idempotent = false)]
        [JsonRpcHelp("获取支付方式")]
        public JsonObject GetPayList()
        {
            List<YSWL.Payment.Model.PaymentModeInfo> paylist = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.Wap);
            if (paylist == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            JsonArray result = new JsonArray();
            JsonObject baseJson;
            foreach (YSWL.Payment.Model.PaymentModeInfo item in paylist)
            {
                #region 不返回微信支付
                if (item.Gateway.StartsWith("wechat"))
                {
                    continue;
                }
                #endregion

                baseJson = new JsonObject();
                baseJson.Put("id", item.ModeId);
                baseJson.Put("name", item.Name);
                baseJson.Put("description", item.Description);
                result.Add(baseJson);
            }
            return new Result(ResultStatus.Success, result);
        }
        [JsonRpcMethod("GetShipList", Idempotent = false)]
        [JsonRpcHelp("获取配送方式")]
        public JsonObject GetShipList(int payId = -1)
        {
            List<YSWL.MALL.Model.Shop.Shipping.ShippingType> list = _shippingTypeManage.GetListByPay(payId);
            if (list == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            JsonArray result = new JsonArray();
            JsonObject baseJson;
            foreach (YSWL.MALL.Model.Shop.Shipping.ShippingType item in list)
            {
                baseJson = new JsonObject();
                baseJson.Put("id", item.ModeId);
                baseJson.Put("name", item.Name);
                baseJson.Put("description", item.Description);    
                result.Add(baseJson);
            }
            return new Result(ResultStatus.Success, result);
        }
      
        public JsonArray GetDataByParentId(string strWhere)
        {
            JsonObject currjson;
            JsonArray array = new JsonArray();
            List<YSWL.MALL.Model.Ms.Regions> list = RegionBLL.GetListInfo(strWhere);
            if (list == null)
            {
                return null;
            }
            if (list != null && list.Count > 0)
            {
                foreach (Regions item in list)
                {
                    currjson = new JsonObject();
                    currjson.Put("id", item.RegionId);
                    currjson.Put("name", item.RegionName);
                    currjson.Put("parentid", item.ParentId);
                    currjson.Put("depth", item.Depth);
                    currjson.Put("childlist", GetDataByParentId("ParentId=" + item.RegionId));
                    array.Add(currjson);
                }
            }
            return array;
        }
        #endregion

        #region 获取运费
        /// <summary>
        /// 获取运费
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="shipId">收货地址Id</param>
        /// <param name="shipTypeId">配送方式Id</param>
        /// <param name="productList">购物车商品</param>
        /// <returns></returns>
        [JsonRpcMethod("GetFreight", Idempotent = false)]
        [JsonRpcHelp("获取运费")]
        public JsonObject GetFreight(int userId, int shipId, int shipTypeId, JsonArray productList)
        {
            JsonObject jsonObject = new JsonObject();
            decimal Freight = 0;
            #region 获取购物车
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper;
            try
            {
                shoppingCartInfo = GetShoppingCart(productList, userId, out shoppingCartHelper, -1, -1);
            }
            catch (ArgumentNullException)
            {
                jsonObject.Put("freight", Freight);
                return new Result(ResultStatus.Success, jsonObject);
                //return new Result(ResultStatus.Success, "PROSALEEXPIRED");
            }
            #endregion

            #region 获取收货地址及地区
            YSWL.MALL.Model.Shop.Shipping.ShippingAddress shippingAddress = _addressManage.GetModelByCache(shipId);
            if (shippingAddress == null)
            {
                jsonObject.Put("freight", Freight);
                return new Result(ResultStatus.Success, jsonObject);
                //return new Result(ResultStatus.Failed, "noReceiverInfo");
            }
            YSWL.MALL.Model.Ms.Regions regionInfo = RegionBLL.GetModelByCache(shippingAddress.RegionId);
            if (regionInfo == null)
            {
                jsonObject.Put("freight", Freight);
                return new Result(ResultStatus.Success, jsonObject);
                //return new Result(ResultStatus.Failed, "noRehionInfo");
            }
            #endregion

            #region 获取配送(物流)
            YSWL.MALL.Model.Shop.Shipping.ShippingType shippingType = _shippingTypeManage.GetModelByCache(shipTypeId);
            if (shippingType == null)
            {
                jsonObject.Put("freight", Freight);
                return new Result(ResultStatus.Success, jsonObject);
                //return new Result(ResultStatus.Failed, "NOSHIPPINGTYPE");
            }
            #endregion

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

            Freight = shoppingCartInfo.CalcFreight(shippingType, shippingRegion);
            jsonObject.Put("freight", Freight);
            return new Result(ResultStatus.Success, jsonObject);
        }
        #endregion


        #region 获取购物车数据

        private   ShoppingCartInfo GetShoppingCart(JsonArray jsonSku, int userId,
           out YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper, int prosaleId = -1, int groupbuyId = -1)
        {
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo(); 
            shoppingCartHelper = new BLL.Shop.Products.ShoppingCartHelper(userId);
            JsonObject skuItem;
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

        #endregion

        #region 根据订单号获取订单Id
        [JsonRpcMethod("GetOrder", Idempotent = false)]
        [JsonRpcHelp("获取订单")]
        public JsonObject GetOrder(string orderCode)
        {
            if (string.IsNullOrWhiteSpace(orderCode))
            {
                return new Result(ResultStatus.Success, null);
            }
            YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo = _orderManage.GetOrderByOrderCode(orderCode);
            if (null != orderInfo)
            {
                return new Result(ResultStatus.Success, orderInfo.OrderId);
            }
            return new Result(ResultStatus.Success, null);
        }
        #endregion

        #region 赠品功能
        #region 详情
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="productList">购物车商品</param>
        /// <param name="coupPrice">优惠劵金额</param>
        /// <returns></returns>
        [JsonRpcMethod("GetGiftInfo", Idempotent = false)]
        [JsonRpcHelp("获取赠品")]
        public JsonObject GetGiftInfo(int userId, JsonArray productList, decimal coupPrice= 0)
        {
            JsonObject jsonObject = new JsonObject();
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper = new BLL.Shop.Products.ShoppingCartHelper(userId);
            try
            {
                shoppingCartInfo = GetShoppingCart(productList, userId, out shoppingCartHelper);
            }
            catch (ArgumentNullException)
            {
                return new Result(ResultStatus.Success, "PROSALEEXPIRED");
            }
            if (shoppingCartInfo == null ||
                shoppingCartInfo.Items == null ||
                shoppingCartInfo.Items.Count < 1)
            {
                return new Result(ResultStatus.Success, "NOSHOPPINGCARTINFO");
            }
            List<Model.Shop.Activity.ActivityInfo> actInfoList = activInfoBll.GetActGiftList(shoppingCartInfo, userId, coupPrice);
            JsonObject result = new JsonObject();
            JsonArray jsonPro = new JsonArray();
            JsonArray jsonAct = new JsonArray();

            #region 排除掉赠优惠劵和包邮 2.2版本中没有此功能
            if (actInfoList != null)
            {
                actInfoList = actInfoList.Where(o => (o.RuleId == 1 || o.RuleId == 2)).ToList();
            }
            #endregion

            if (actInfoList != null)
            {
                JsonObject json = null;
                List<YSWL.MALL.Model.Shop.Products.ProductInfo> actProductList = YSWL.MALL.BLL.Shop.Activity.ActivityInfo.GetActProList(actInfoList,0);
                if (actProductList != null)
                {
                    foreach (var item in actProductList)
                    {
                        json = new JsonObject();
                        json.Put("id", item.ProductId);
                        json.Put("name", item.ProductName);
                        json.Put("saleprice", item.SalePrice);
                        json.Put("pic", item.ThumbnailUrl1);
                        json.Put("number", item.Count);
                        jsonPro.Add(json);
                    }
                }
                //获取赠送优惠券
                List<YSWL.MALL.Model.Shop.Coupon.CouponRule> activCouList = YSWL.MALL.BLL.Shop.Activity.ActivityInfo.GetActCoupon(actInfoList);
                if (activCouList != null)
                {
                    foreach (var item in activCouList)
                    {
                        json = new JsonObject();
                        json.Put("id", item.RuleId);
                        json.Put("name", item.Name);
                        json.Put("price", item.CouponPrice);
                        json.Put("number", item.SendCount);
                        json.Put("limitStr", GetLimitStr(item));
                        jsonAct.Add(json);
                    }
                }
            }
            result.Put("productList", jsonPro);
            result.Put("couponList", jsonAct);
            return new Result(ResultStatus.Success, result);
        }
        #endregion
        /// <summary>
        /// 优惠券使用限制
        /// </summary>
        /// <param name="infoModel"></param>
        /// <returns></returns>
        public static string GetLimitStr(YSWL.MALL.Model.Shop.Coupon.CouponRule infoModel)
        {
            if (infoModel == null)
                return "无限制";
            if (String.IsNullOrWhiteSpace(infoModel.ProductSku))
            {
                YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
            }
            if (infoModel.ProductId > 0)
            {
                YSWL.MALL.BLL.Shop.Products.ProductInfo infoBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
                YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = infoBll.GetModelByCache(infoModel.ProductId);
                return "购买指定商品： " + productInfo.ProductName + " 使用";
            }
            if (infoModel.CategoryId > 0)
            {
                YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new YSWL.MALL.BLL.Shop.Products.CategoryInfo();
                YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo = cateBll.GetModelByCache(infoModel.CategoryId);
                return "购买指定分类：" + categoryInfo.Name + " 商品使用";
            }
            return "无限制";
        }
        #endregion


        #region 可用优惠券

        /// <summary>
        /// 可用优惠券
        /// </summary>
        /// <returns></returns>
        [JsonRpcMethod("GetCunpons", Idempotent = false)]
        [JsonRpcHelp("可用优惠券")]
        public JsonObject GetCunpons(int userId, JsonArray productList)
        {
            JsonObject jsonObject;
            List<JsonObject> jList;
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "userdataError");
            }
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper = new BLL.Shop.Products.ShoppingCartHelper(userId);
            try
            {
                shoppingCartInfo = GetShoppingCart(productList, userId, out shoppingCartHelper);
            }
            catch (ArgumentNullException)
            {
                return new Result(ResultStatus.Success, "PROSALEEXPIRED");
            }
            if (shoppingCartInfo == null ||
                shoppingCartInfo.Items == null ||
                shoppingCartInfo.Items.Count < 1)
            {
                return new Result(ResultStatus.Success, "NOSHOPPINGCARTINFO");
            }
            List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> infoList = couponBLL.GetCouponList(userId, 1, false);
            YSWL.MALL.BLL.Shop.Coupon.CouponClass classBLL = new YSWL.MALL.BLL.Shop.Coupon.CouponClass();
            foreach (var Info in infoList)
            {
                YSWL.MALL.Model.Shop.Coupon.CouponClass classModel = classBLL.GetModelByCache(Info.ClassId);
                Info.ClassName = classModel == null ? "" : classModel.Name;
                Info.LimitStatus = couponBLL.GetUseStatus(shoppingCartInfo, Info);
            }
            if (infoList.Count > 0)
            {
                jList = new List<JsonObject>();
                infoList = infoList.Where(c => c.LimitStatus == 1).ToList();
                foreach (var item in infoList)
                {
                    jsonObject = new JsonObject();
                    jsonObject.Put("id", item.RuleId);
                    jsonObject.Put("classId", item.ClassId);
                    jsonObject.Put("className", item.ClassName);
                    jsonObject.Put("couponCode", item.CouponCode);
                    jsonObject.Put("couponName", item.CouponName);
                    jsonObject.Put("status", item.Status);
                    jsonObject.Put("limitstatus", item.LimitStatus);
                    jsonObject.Put("endDate", item.StartDate.ToString("yyyy-MM-dd") + "至" + item.EndDate.ToString("yyyy-MM-dd"));
                    jsonObject.Put("couponPrice", item.CouponPrice);
                    jsonObject.Put("limitPrice", item.LimitPrice);
                    jList.Add(jsonObject);
                }
                return new Result(ResultStatus.Success, jList);
            }
            return new Result(ResultStatus.Failed, "nodata");//NO DATA
        }
        #endregion

        #region 
        /// <summary>
        /// 获取调整后总金额 
        /// </summary>
        /// <returns></returns>
        [JsonRpcMethod("GetTotalPrice", Idempotent = false)]
        [JsonRpcHelp("获取调整后总金额")]
        public JsonObject GetTotalPrice(int userId, JsonArray productList,int proSaleId = -1, int groupBuyId = -1) 
        {
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "USERDATAERROR");
            }
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper = new BLL.Shop.Products.ShoppingCartHelper(userId);
            try
            {
                shoppingCartInfo = GetShoppingCart(productList, userId, out shoppingCartHelper, proSaleId, groupBuyId);
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
            return new Result(ResultStatus.Success, shoppingCartInfo.TotalAdjustedPrice.ToString());
        }
        #endregion


        #region
        //根据sku 获取规格  字符串
        private string GetSKUStr(string sku)
        {
            List<YSWL.MALL.Model.Shop.Products.SKUItem> itemList = skuBll.GetSKUItemsBySku(sku);
            if (itemList == null || itemList.Count == 0)
            {
                return null;
            }
            string str = "";
            foreach (var item in itemList)
            {
                str += item.AttributeName + ":" + item.AV_ValueStr + ",";
            }
            return str;
        }
        #endregion

        #region 获取库存
        [JsonRpcMethod("GetStockList", Idempotent = false)]
        [JsonRpcHelp("获取库存")]
        public JsonObject GetStockList(JsonArray productList)
        {
            if (productList == null)
            {
                return new Result(ResultStatus.Failed, null);
            }
          bool  openAlertStock= YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
            string strKey = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_App_Product_Attr") ?? "颜色，规格";
            JsonArray array = new JsonArray();
            JsonObject json = null;
            string sku = "";
            int number = 1;
            int itemid = 0;//这个值接口是不需要的，但是由于android需要 ，就做一下回传
            keys = strKey.Split(',');
            YSWL.MALL.Model.Shop.Products.SKUInfo skuInfoModel = null;
            Model.Shop.Products.ProductInfo model = null;
            foreach (JsonObject jsonObject in productList)
            {
                sku = Common.InjectionFilter.SqlFilter(jsonObject["sku"] == null ? "" : jsonObject["sku"].ToString());
                number = Common.Globals.SafeInt(jsonObject["number"], 1);
                itemid = Common.Globals.SafeInt(jsonObject["itemid"], 0);
                json = new JsonObject();
                if (String.IsNullOrWhiteSpace(sku) || itemid <= 0)
                {
                    continue; //异常数据
                }
                skuInfoModel = skuBLL.GetModelBySKU(sku);
                if (skuInfoModel == null)
                {
                    continue; //异常数据
                }
                model = productBll.GetModel(skuInfoModel.ProductId);
                if (model == null)
                {
                    continue; //异常数据
                }
                if (model.SaleStatus != 1)
                {
                    continue;//商品已下架或已删除
                }
                json.Put("itemid", itemid);
                json.Put("id", model.ProductId);
                json.Put("name", model.ProductName);
                json.Put("number", number);
                json.Put("pic", model.ThumbnailUrl1);
                json.Put("marketprice", model.MarketPrice.HasValue ? model.MarketPrice.Value.ToString("F") : "0.00");
                json.Put("standard", GetAttrValues(model.ProductId, keys));
                json.Put("saleprice", skuInfoModel.SalePrice.ToString("F"));
                json.Put("sku", skuInfoModel.SKU);
                json.Put("discount", GetProductSale(model.ProductId, null));
                json.Put("discountRule", GetAllSaleRule(model.ProductId, null));
                //json.Put("producttype", model.SuppId <= 0 ? 1 : 2);
                json.Put("producttype", 1);

                json.Put("limitQty", model.RestrictionCount);//限购数量
                if (openAlertStock) //开启警戒库存
                {
                    if (skuInfoModel.Stock > skuInfoModel.AlertStock)
                    {
                        json.Put("hasStock", true);
                        json.Put("stockcount", skuInfoModel.Stock);
                    }
                    else
                    {
                        json.Put("hasStock", false);
                    }
                }
                else
                {
                    if (skuInfoModel.Stock > 0)
                    {
                        json.Put("hasStock", true);
                        json.Put("stockcount", skuInfoModel.Stock);
                    }
                    else
                    {
                        json.Put("hasStock", false);
                    }
                }
                array.Add(json);
            }
            return new Result(ResultStatus.Success, array);
        }
        #endregion


    }
}