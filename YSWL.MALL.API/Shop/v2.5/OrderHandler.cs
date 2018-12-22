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

namespace YSWL.MALL.API.Shop.v2_5
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



        #region 最近订购过的商品
        [JsonRpcMethod("OrderedProductListV2.5", Idempotent = false)]
        [JsonRpcHelp("最近订购商品列表")]
        public JsonObject OrderedProd(int userId, int? page = 1, int pageNum = 30, int regionId = 0)
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
                #region 多分仓库存处理 
                if (YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot()) //是否开启多分仓
                {
                    ////获取用户默认收货地址
                    //YSWL.MALL.BLL.Shop.Shipping.ShippingAddress addressBll = new YSWL.MALL.BLL.Shop.Shipping.ShippingAddress();
                    //int regionId = addressBll.GetDefaultRegionId(userId);
                    skulist = skuBLL.GetProductSkuInfo(item.ProductId, regionId, item.SupplierId);//sku信息
                }
                else
                {
                    skulist = skuBLL.GetProductSkuInfo(item.ProductId);//sku信息
                }
                #endregion
                
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

        #region 获取购物车列表
        [JsonRpcMethod("GetCartListV2.5", Idempotent = false)]
        [JsonRpcHelp("获取购物车列表")]  //这个接口后期需要拆分
        public JsonObject GetCartListV2_5(JsonArray productList, int userId, int regionId)
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
                shoppingCartInfo = GetCartList(productList, userId, regionId);
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
            foreach (var item in shoppingCartInfo.Items)
            {
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
        private ShoppingCartInfo GetCartList(JsonArray jsonSku, int userId, int regionId)
        {
            ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
            JsonObject skuItem;
            if (null != jsonSku && jsonSku.Length > 0)
            {
                //是否对接多仓，处理逻辑
                bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
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
                    itemInfo.Stock = IsMultiDepot ? YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(sku, regionId, productInfo.SupplierId) : skuInfo.Stock;
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

                    //是否对接多仓，处理逻辑
                    bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
                    itemInfo.Stock = IsMultiDepot ? YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(sku, regionId, productInfo.SupplierId) : skuInfo.Stock;   //获取分仓库存
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

        #region 完成订单
        [JsonRpcMethod("CompleteOrder", Idempotent = true)]
        [JsonRpcHelp("完成订单")]
        public JsonObject CompleteOrder(long Id, int UserID)
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
            if (OrderManage.CompleteOrder(orderInfo, currentUser))
            {
                json.Put("result", "Success");
            }
            return new Result(ResultStatus.Success, json);
        }
        #endregion

    }
}