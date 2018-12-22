/**
* ProductHandler.cs
*
* 功 能： Shop API
* 类 名： ProductHandler
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
using YSWL.MALL.BLL.SysManage;
using YSWL.Components.Handlers.API;
using YSWL.Json;
using YSWL.Json.RPC;
using YSWL.MALL.ViewModel.Shop;
using System.Collections.Generic;
using Webdiyer.WebControls.Mvc;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.BLL.Shop.Products;
using System.Linq;

namespace YSWL.MALL.API.Shop.v2_5
{
    public partial class ShopHandler
    {
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBLL = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productManage = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productBLL = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        private YSWL.MALL.BLL.Shop.Products.AttributeInfo attrBLL = new YSWL.MALL.BLL.Shop.Products.AttributeInfo();
        private YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct ruleProductBll = new BLL.Shop.Sales.SalesRuleProduct();
        #region 商品列表
        [JsonRpcMethod("ProductListV2.5", Idempotent = false)]
        [JsonRpcHelp("商品列表")]
        public   JsonObject ProductList(int userId,int cId = 0, string orderby = "hot",
                                 int? page = 1, int pageNum = 30, int regionId=0)
        {
            if (pageNum == 0)
            {
                pageNum = 30;
            }
            #region
            if (String.IsNullOrEmpty(orderby))
            {
                orderby = "hot";
            }
            ProductListModel model = new ProductListModel();
            YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo = null;
            if (cId > 0)
            {
                List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
                categoryInfo = cateList.FirstOrDefault(c => c.CategoryId == cId);
                if (categoryInfo != null)
                {
                    var path_arr = categoryInfo.Path.Split('|');
                    List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categorysPath =
                        cateList.Where(c => path_arr.Contains(c.CategoryId.ToString())).OrderBy(c => c.Depth)
                                .ToList();
                    model.CategoryPathList = categorysPath;
                    model.CurrentCateName = categoryInfo.Name;
                }
            }
            model.CurrentCid = cId;
            model.CurrentMod = orderby;
            JsonArray jsonArray = new JsonArray();
            JsonObject json;
            //重置页面索引
            page = page.HasValue && page.Value > 1 ? page.Value : 1;
            //计算分页起始索引
            int startIndex = page.Value > 1 ? (page.Value - 1) * pageNum + 1 : 0;
            //计算分页结束索引
            int endIndex = page.Value > 1 ? startIndex + pageNum - 1 : pageNum;
    
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> list;
            try
            {
                if (String.IsNullOrEmpty(orderby))
                {
                    return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
                }
                list = productManage.GetProductsListEx(cId, 0, "", "", orderby, startIndex, endIndex);
            }
            catch (Exception ex)
            {
                YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message),
                   ex.StackTrace, Request);
                return new Result(ResultStatus.Error, ex.Message);
            }
            if (list == null)
            {
                return new Result(ResultStatus.Success, new JsonArray());
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
                json.Put("ruleType", (int)ruleProductBll.GetRuleType(item.ProductId, userId));  //促销规则形式： -1：没有参与促销 0：打折  1：减价  2：直降
                json.Put("producttype", 1);
                json.Put("limitQty", item.RestrictionCount);//限购数量
                if (skulist != null && skulist.Count>0)
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
                        json.Put("hasStock", stock > 0?true:false);
                    }
                    else
                    {
                        skulist.ForEach(info => {
                            stock += info.Stock;
                        });
                        json.Put("hasStock", stock > 0 ? true : false);
                    }
                }                 
                else {
                    json.Put("hasStock", false);
                }
                jsonArray.Add(json);
            }
            #endregion    
            return new Result(ResultStatus.Success, jsonArray);
        }
        #endregion

        #region 促销规则商品列表
        [JsonRpcMethod("SalesRuleProductListV2.5", Idempotent = false)]
        [JsonRpcHelp("促销商品列表")]
        public JsonObject SalesRuleProductList(int userId, int? page = 1, int pageNum = 30 , int regionId=0)
        {
            if (pageNum == 0)
            {
                pageNum = 30;
            }
            JsonArray jsonArray = new JsonArray();
            JsonObject json;
            //重置页面索引
            page = page.HasValue && page.Value > 1 ? page.Value : 1;
            //计算分页起始索引
            int startIndex = page.Value > 1 ? (page.Value - 1) * pageNum + 1 : 0;
            //计算分页结束索引
            int endIndex = page.Value > 1 ? startIndex + pageNum - 1 : pageNum;
            List<Model.Shop.Products.ProductInfo> list = productManage.GetSalesRuleProductsList(startIndex, endIndex, userId);
            if (list == null)
            {
                return new Result(ResultStatus.Success, new JsonArray());
            }
            List<Model.Shop.Products.SKUInfo> skulist;
            int stock;
            bool openAlertStock = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
            foreach (YSWL.MALL.Model.Shop.Products.ProductInfo item in list)
            {

                #region 多分仓库存处理
                if (YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot()) //是否开启多分仓
                {
                    //获取用户默认收货地址
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
                json.Put("ruleType", (int)ruleProductBll.GetRuleType(item.ProductId, userId));  //促销规则形式： -1：没有参与促销 0：打折  1：减价  2：直降
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

        #region 商品详情
        [JsonRpcMethod("ProductDetailV2.5", Idempotent = false)]
        [JsonRpcHelp("商品详情")]
        public JsonObject ProductDetailV2_5(int pId = -1, int userId = -1, int regionId=0)
        {
            if (pId <= 0)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            YSWL.MALL.ViewModel.Shop.ProductModel model = new ProductModel();
            model.ProductInfo = productManage.GetModel(pId);
            if (model.ProductInfo == null)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            JsonArray jsonArray = new JsonArray();
            JsonObject json;
            JsonArray picArray = new JsonArray();
            YSWL.MALL.BLL.Shop.Products.ProductImage imageManage = new YSWL.MALL.BLL.Shop.Products.ProductImage();
            model.ProductImages = imageManage.ProductImagesList(pId);
            YSWL.MALL.ViewModel.Shop.ProductSKUModel productSKUModel = null;

            #region 多分仓库存处理
            if (YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot()) //是否开启多分仓
            {
                //获取用户默认收货地址
                //YSWL.MALL.BLL.Shop.Shipping.ShippingAddress addressBll = new YSWL.MALL.BLL.Shop.Shipping.ShippingAddress();
                //int regionId = addressBll.GetDefaultRegionId(userId);
                productSKUModel = skuBLL.GetProductSKUInfoByProductId(model.ProductInfo.ProductId, regionId, model.ProductInfo.SupplierId);
            }
            else
            {
                productSKUModel = skuBLL.GetProductSKUInfoByProductId(model.ProductInfo.ProductId);
            }
            #endregion 


            if (productSKUModel == null || productSKUModel.ListSKUInfos==null || productSKUModel.ListSKUInfos.Count < 1)
            {
                return new Result(ResultStatus.Success, null);
            }
            string salesprice = productSKUModel.ListSKUInfos[0].SalePrice.ToString("F");
            
            JsonObject attr;
            json = new JsonObject();
            json.Put("id", model.ProductInfo.ProductId);
            json.Put("name", model.ProductInfo.ProductName);
            json.Put("marketprice", ((Decimal)model.ProductInfo.MarketPrice).ToString("F"));
            json.Put("saleprice", salesprice);
            json.Put("xmltext", model.ProductInfo.Description);
            json.Put("leftTime", model.ProductInfo.ProSalesEndDate.ToString("yyyy年MM月dd日HH时mm分ss秒"));
            json.Put("pic", model.ProductInfo.ThumbnailUrl1);
            foreach (YSWL.MALL.Model.Shop.Products.ProductImage pro in model.ProductImages)
            {
                picArray.Add(pro.ThumbnailUrl1);
            }
            json.Put("bigPic", picArray);
            json.Put("sku", productSKUModel.ListSKUInfos[0].SKU);
            JsonArray skuArray = new JsonArray();
            foreach (KeyValuePair<YSWL.MALL.Model.Shop.Products.AttributeInfo, SortedSet<YSWL.MALL.Model.Shop.Products.SKUItem>>
attrSKUItem in productSKUModel.ListAttrSKUItems)
            {
                foreach (YSWL.MALL.Model.Shop.Products.SKUItem skuItem in attrSKUItem.Value)
                {
                    attr = new JsonObject();
                    attr.Put("id", skuItem.ValueId);
                    attr.Put("key", skuItem.AttributeName);
                    attr.Put("value", skuItem.ValueStr);

                    attr.Put("pic", skuItem.ImageUrl);
                    skuArray.Add(attr);
                }
            }
            if (skuArray != null && skuArray.Length > 0)
            {
                json.Put("productProperty", skuArray);
            }

            //NO SKU ERROR
            if (productSKUModel.ListSKUInfos != null &&
                productSKUModel.ListSKUInfos.Count > 0 &&
                productSKUModel.ListSKUItems != null)
            {
                json.Put("hasSKU", true);
                json.Put("hasStock", true);
                //木有开启SKU的情况
                if (productSKUModel.ListSKUItems.Count == 0)
                {
                    json.Put("hasSKU", false);
                    //判断库存是否满足
                    json.Put("hasStock", true);

                    //是否开启警戒库存判断
                    bool IsOpenAlertStock =
                        YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
                    if (IsOpenAlertStock &&  productSKUModel.ListSKUInfos[0].Stock <= productSKUModel.ListSKUInfos[0].AlertStock)
                    {
                        json.Put("hasStock", false);
                    }

                    if (productSKUModel.ListSKUInfos[0].Stock < 1)
                    {
                        json.Put("hasStock", false);
                    }
                    else
                    {
                        json.Put("stockcount", productSKUModel.ListSKUInfos[0].Stock);
                    }
                }
            }
            SKUInfoToJson(json, productSKUModel.ListSKUInfos,userId, model.ProductInfo.SupplierId);
            // result.Put("product", json);

            #region 是否已加入收藏
            //是否已经加入收藏 返回收藏id
            if (userId > 0)
            {
                json.Put("favId", favoBLL.GetFavoriteId(pId, userId, 1)); //已加入
            }
            else
            {
                json.Put("favId", 0); //未加入
            }
            #endregion

            JsonArray salesRuleArray = new JsonArray();

            #region 批发优惠列表
            if (model.ProductInfo.SupplierId <= 0)
            {
                //批发规则  只有自营商品使用 
                YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct ruleBll = new YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct();
                YSWL.MALL.ViewModel.Shop.SalesModel saleRuleModel = ruleBll.GetSalesRuleByCache(pId, userId);
                if (saleRuleModel != null && saleRuleModel.SalesItems != null && saleRuleModel.SalesItems.Count > 0)
                {
                    JsonObject salesRulejson;
                    JsonArray userRankArray;
                    //JsonObject userRankJson;
                    foreach (var item in saleRuleModel.SalesItems)
                    {
                        salesRulejson = new JsonObject();
                        salesRulejson.Put("ruleUnit", saleRuleModel.SalesRule.RuleUnit);//规则单位  0：个 1：元
                        salesRulejson.Put("rateValue", item.RateValue);//优惠数值
                        salesRulejson.Put("itemType", item.ItemType);//规则类型 0：打折 1：减价 2：固定价格
                        salesRulejson.Put("unitValue", item.UnitValue);
                        userRankArray = new JsonArray();
                        if (item.UserRankList != null)
                        {
                            foreach (var rankItem in item.UserRankList)
                            {
                                //userRankJson = new JsonObject();
                                userRankArray.Add(rankItem.Name);
                            }
                        }
                        salesRulejson.Put("userRankList", userRankArray);
                        salesRuleArray.Add(salesRulejson);
                    }
                }
            }
            json.Put("salesRule", salesRuleArray);
            #endregion

            return new Result(ResultStatus.Success, json);
        }
        private YSWL.Json.JsonObject SKUInfoToJson(YSWL.Json.JsonObject json, List<YSWL.MALL.Model.Shop.Products.SKUInfo> list,int userId,int  suppId)
        {
            if (list == null || list.Count < 1) return null;

            YSWL.Json.JsonObject jsonSKU = new YSWL.Json.JsonObject();
            long[] key;
            int index;
            #region 计算会员等级价格
            if ( suppId <= 0)
            {
                list = ruleProductBll.GetRankSales(list, userId);
            }
            #endregion

            foreach (YSWL.MALL.Model.Shop.Products.SKUInfo item in list)
            {
                if (item.SkuItems == null || item.SkuItems.Count < 1) continue;

                //无库存SKU不提供给页面
                //是否开启警戒库存判断
                bool IsOpenAlertStock = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
                if (IsOpenAlertStock && item.Stock <= item.AlertStock)
                {
                    continue;
                }
                if (item.Stock < 1)
                    continue;
 
                //组合SKU 的 ValueId
                key = new long[item.SkuItems.Count];
                index = 0;
                item.SkuItems.ForEach(xx => key[index++] = xx.ValueId);
                jsonSKU.Accumulate(string.Join(",", key), new
                {
                    sku = item.SKU,
                    count = item.Stock,
                    price = item.SalePrice,
                    rankprice = item.RankPrice
                });
            }

            //获取最小/最大价格
            list.Sort((x, y) => x.SalePrice.CompareTo(y.SalePrice));
            json.Put("defaultPrice", new
            {
                minPrice = list[0].SalePrice,
                maxPrice = list[list.Count - 1].SalePrice,
                minRankPrice = list[0].RankPrice,
                maxRankPrice = list[list.Count - 1].RankPrice
            });
            json.Put("skuData", jsonSKU);
            return json;
        }
        #endregion
 


        #region 搜索商品列表
        [JsonRpcMethod("SearchProductListV2.5", Idempotent = false)]
        [JsonRpcHelp("搜索商品列表")]
        public JsonObject SearchProductListV2_5(int cid = 0, int brandid = 0, string keyword = "", string orderby = "hot", string price = "0-0",
                                        int? page = 1, int pageNum = 10, int regionId = 0)
        {

            if (pageNum == 0)
            {
                pageNum = 10;
            }
            if (String.IsNullOrEmpty(page.ToString()))
            {
                page = 1;
            }
            ProductListModel model = new ProductListModel();
            keyword = YSWL.Common.InjectionFilter.SqlFilter(keyword);
            if (cid > 0)
            {
                List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
                YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo =
                    cateList.FirstOrDefault(c => c.CategoryId == cid);
                if (categoryInfo != null)
                {
                    var path_arr = categoryInfo.Path.Split('|');
                    List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categorysPath =
                        cateList.Where(c => path_arr.Contains(c.CategoryId.ToString())).OrderBy(c => c.Depth)
                                .ToList();
                    model.CategoryPathList = categorysPath;
                    model.CurrentCateName = categoryInfo.Name;
                }
            }
            model.CurrentCid = cid;
            model.CurrentMod = orderby;

            //重置页面索引
            page = page.HasValue && page.Value > 1 ? page.Value : 1;
            //计算分页起始索引
            int startIndex = page.Value > 1 ? (page.Value - 1) * pageNum + 1 : 0;
            //计算分页结束索引
            int endIndex = page.Value > 1 ? startIndex + pageNum - 1 : pageNum;
            YSWL.Json.JsonArray jsonArray = new JsonArray();
            JsonObject json;
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> list;
            try
            {
                if (String.IsNullOrEmpty(orderby))
                {
                    orderby = "default";
                }
                list = productManage.GetSearchListEx(cid, brandid, keyword, price, orderby, startIndex, endIndex);
            }
            catch (Exception ex)
            {
                YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message),
                   ex.StackTrace, Request);
                return new Result(ResultStatus.Error, ex.Message);
            }
            if (list == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            List<Model.Shop.Products.SKUInfo> skulist;
            int stock;
            bool openAlertStock = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
            foreach (YSWL.MALL.Model.Shop.Products.ProductInfo item in list)
            {
                #region 多分仓库存处理
                if (YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot()) //是否开启多分仓
                {
                    //获取用户默认收货地址
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
    }
}