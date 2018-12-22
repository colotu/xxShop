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

namespace YSWL.MALL.API.Shop.v1
{
    public partial class ShopHandler
    {
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBLL = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
        private YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct salesRuleProductBLL = new BLL.Shop.Sales.SalesRuleProduct();

        #region 昵称是否存在
        //[JsonRpcMethod("HasUserByNickName", Idempotent = true)]
        //[JsonRpcHelp("昵称是否存在")]
        //public JsonObject HasUserByNickName(string NickName)
        //{
        //    if (string.IsNullOrWhiteSpace(NickName)) return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
        //    YSWL.Accounts.Bus.User BLLUser = new User();
        //    return Result.HasResult(BLLUser.HasUserByNickName(NickName));
        //}
        #endregion

        #region 获取个人信息
        ///// <summary>
        ///// 获取个人信息
        ///// </summary>
        ///// <param name="UserId">用户ID</param>
        ///// <returns>用户信息</returns>
        //[JsonRpcMethod("GetUserInfo", Idempotent = false)]
        //[JsonRpcHelp("获取个人信息")]
        //public JsonObject GetUserInfo(int UserId)
        //{
        //    //超级管理员信息保护 过滤UserId=1用户
        //    if (UserId < 2) return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
        //    try
        //    {
        //        //TODO: 用户不存在 未对应
        //        return new Result(ResultStatus.Success,
        //            GetUserInfo4Json(new User(UserId)));
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message), ex.StackTrace, Request);
        //        return new Result(ResultStatus.Error, ex);
        //    }
        //}

        //private JsonObject GetUserInfo4Json(User userInfo)
        //{
        //    if (userInfo == null || string.IsNullOrWhiteSpace(userInfo.UserType)) return null;
        //    JsonObject json = new JsonObject();
        //    json.Put("UserId", userInfo.UserID);
        //    json.Put("UserName", userInfo.UserName);
        //    json.Put("TrueName", userInfo.TrueName);
        //    json.Put("Phone", userInfo.Phone);
        //    json.Put("DepartmentID", userInfo.DepartmentID);
        //    json.Put("FileNo", userInfo.NickName);

        //    return json;
        //}
        #endregion
 
        //#region 商品收藏
        //[JsonRpcMethod("AjaxAddFav", Idempotent = false)]
        //[JsonRpcHelp("商品收藏")]
        //public JsonObject AddFav(int pId, int UserID)
        //{
        //    if (UserID < 1)
        //    {
        //        return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
        //    }
        //    if (!String.IsNullOrWhiteSpace(pId.ToString()))
        //    {
        //        int productId = YSWL.Common.Globals.SafeInt(pId, 0);
        //        YSWL.MALL.BLL.Shop.Favorite favBLL = new YSWL.MALL.BLL.Shop.Favorite();
        //        JsonArray result = new JsonArray();
        //        YSWL.MALL.Model.Shop.Favorite favMode = new YSWL.MALL.Model.Shop.Favorite();
        //        favMode.CreatedDate = DateTime.Now;
        //        favMode.TargetId = productId;
        //        favMode.Type = 1;
        //        favMode.UserId = UserID;
        //        if (favBLL.Add(favMode) > 0)
        //        {
        //            result.Add("Success");
        //        }
        //        else
        //        {
        //            result.Add("Success");
        //        }
        //        return new Result(ResultStatus.Success, result);
        //    }
        //    return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
        //}
        //#endregion

        #region 商品评论
        [JsonRpcMethod("productCommon", Idempotent = false)]
        [JsonRpcHelp("商品评论")]
        public JsonObject productCommon(int pId, int page = 1, int pageNum = 10)
        {
            if (pageNum == 0)
            {
                pageNum = 10;
            }
            if (String.IsNullOrEmpty(page.ToString()))
            {
                page = 1;
            }
            if (pId < 1)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            //计算分页起始索引
            int startIndex = page > 1 ? (page - 1) * pageNum + 1 : 0;

            //计算分页结束索引
            int endIndex = page * pageNum;
            int totalCount = 0;

            //获取总条数
            totalCount = reviewsBLL.GetRecordCount("Status=1 and ProductId=" + pId);

            if (totalCount < 1)
            {
                return new Result(ResultStatus.Success, null);
            }
            JsonObject result = new JsonObject();
            YSWL.Json.JsonArray jsonArray = new JsonArray();
            JsonObject json;
            result.Put("list_count", totalCount);
            List<YSWL.MALL.Model.Shop.Products.ProductReviews> productReviewses = reviewsBLL.GetReviewsByPage(pId, " CreatedDate desc", startIndex, endIndex);

            PagedList<YSWL.MALL.Model.Shop.Products.ProductReviews> lists = new PagedList<YSWL.MALL.Model.Shop.Products.ProductReviews>(productReviewses, page, pageNum, totalCount);
            foreach (YSWL.MALL.Model.Shop.Products.ProductReviews item in lists)
            {
                json = new JsonObject();
                json.Put("name", item.UserName);
                json.Put("commontime", item.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                json.Put("commoncontent", item.ReviewText);
                json.Put("userId", item.UserId);
                jsonArray.Add(json);
            }
            result.Put("commonlist", jsonArray);
            return new Result(ResultStatus.Success, result);
        }
        #endregion

        //[JsonRpcMethod("PRDescription", Idempotent = false)]
        //[JsonRpcHelp("商品描述")]
        //public JsonObject PRDescription(int pId)
        //{
        //    if (pId < 1)
        //    {
        //        return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
        //    }
        //    YSWL.MALL.ViewModel.Shop.ProductModel model = new ProductModel();
        //    model.ProductInfo = productManage.GetModel(pId);
        //    if (String.IsNullOrEmpty(model.ProductInfo.ToString()))
        //    {
        //        return new Result(ResultStatus.Success, null);
        //    }
        //    JsonArray jsonArray = new JsonArray();
        //    jsonArray.Add(model.ProductInfo.Description);
        //    return new Result(ResultStatus.Success, jsonArray);
        //}
 
        //[JsonRpcMethod("AddCart", Idempotent = false)]
        //[JsonRpcHelp("放入购物车")]
        //public JsonObject AddCart(int userID)
        //{
        //    if (userID == 0)
        //    {
        //        userID = -1;
        //    }
        //    YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userID);
        //    //DONE: 获取已选中内容的购物车进行 购物车 部分商品 下单 BEN Modify 20130923
        //    ShoppingCartModel model = new ShoppingCartModel();
        //    model.AllCartInfo = cartHelper.GetShoppingCart();
        //    model.SelectedCartInfo = cartHelper.GetShoppingCart4Selected();
        //    #region 批销优惠
        //    try
        //    {
        //        YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct salesRule = new YSWL.MALL.BLL.Shop.Sales.SalesRuleProduct();
        //        model.AllCartInfo = salesRule.GetWholeSale(model.AllCartInfo);
        //        model.SelectedCartInfo = salesRule.GetWholeSale(model.SelectedCartInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message), ex.StackTrace, Request);
        //        return new Result(ResultStatus.Error, ex.Message);
        //    }
        //    #endregion
        //    List<ShoppingCartModel> list = new List<ShoppingCartModel>();
        //    list.Add(model);
        //    JsonObject result = new JsonObject();
        //    if (model.AllCartInfo.Quantity <= 0)
        //    {
        //        return new Result(ResultStatus.Success, null);
        //    }
        //    result.Put("cartitem", GetProduct(model));
        //    result.Put("totalPrice", model.SelectedCartInfo.TotalAdjustedPrice.ToString("F"));
        //    return new Result(ResultStatus.Success, result);
        //}

        public JsonArray GetProduct(ShoppingCartModel model)
        {
            JsonArray jsonArray = new JsonArray();
            JsonArray valArray = new JsonArray();
            JsonObject json;
            JsonObject valJson;
            JsonObject result = new JsonObject();
            JsonArray array = new JsonArray();
            foreach (var item in model.AllCartInfo.Items)
            {
                json = new JsonObject();
                json.Put("id", item.ProductId);
                json.Put("name", item.Name);
                json.Put("pic",  item.ThumbnailsUrl);
                json.Put("price", item.AdjustedPrice.ToString("F"));
                json.Put("prodNum", item.Quantity);
                if (item.SkuValues != null && item.SkuValues.Length > 0)
                {
                    foreach (string val in item.SkuValues)
                    {
                        valJson = new JsonObject();
                        valJson.Put("value", val);
                        valArray.Add(valJson);
                    }
                }
                json.Put("product_property", valArray);
                result.Put("Product", json);
                jsonArray.Add(result);
            }
            array.Add(jsonArray);
            return array;
        }

        //[JsonRpcMethod("UpdateItemCount", Idempotent = false)]
        //[JsonRpcHelp("修改数量")]
        ////ItemId 主键
        //public JsonObject UpdateItemCount(int ItemId, int count, int UserID = -1)
        //{
        //    if (UserID < 1)
        //    {
        //        return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
        //    }
        //    if (String.IsNullOrWhiteSpace(ItemId.ToString()) || String.IsNullOrWhiteSpace(count.ToString()))
        //    {
        //        return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
        //    }
        //    else
        //    {
        //        int itemId = YSWL.Common.Globals.SafeInt(ItemId, 0);
        //        int num = YSWL.Common.Globals.SafeInt(count, 1);
        //        int userId = UserID;
        //        YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
        //        try
        //        {
        //            cartHelper.UpdateItemQuantity(itemId, count);
        //        }
        //        catch (Exception ex)
        //        {
        //            YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message), ex.StackTrace, Request);
        //            return new Result(ResultStatus.Error, ex.Message);
        //        }
        //        return AddCart(UserID);
        //    }
        //}

        //[JsonRpcMethod("RemoveItem", Idempotent = true)]
        //[JsonRpcHelp("删除商品")]
        //public JsonObject RemoveItem(int ItemId, int UserID = -1)
        //{
        //    if (UserID < 1)
        //    {
        //        return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
        //    }
        //    if (String.IsNullOrWhiteSpace(ItemId.ToString()))
        //    {
        //        return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
        //    }
        //    else
        //    {
        //        YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(UserID);
        //        try
        //        {
        //            cartHelper.RemoveItem(ItemId);
        //        }
        //        catch (Exception ex)
        //        {
        //            YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message), ex.StackTrace, Request);
        //            return new Result(ResultStatus.Error, ex.Message);
        //        }
        //        return AddCart(UserID);
        //    }
        //}

 
        #region 循环获取属性
        public List<JsonObject> GetAttrValues(long productId, string[] strAttrs)
        {
            List<JsonObject> standardList = new List<JsonObject>();
            JsonObject jsonStand;
            string sttrVlaue;
            if (strAttrs.Length < 1)
            {
                return null;
            }
            else
            {

                for (int i = 0; i < strAttrs.Length; i++)
                {
                    jsonStand = new JsonObject();
                    sttrVlaue = attrBLL.GetAttrValue(keys[i], productId);
                    jsonStand.Put(strAttrs[i], sttrVlaue);
                    standardList.Add(jsonStand);
                }
                return standardList;

            }
        }
        #endregion
 
        #region 获取商品折扣
        //获取商品
        public double GetProductSale(long id, YSWL.MALL.Model.Members.UserRank userRank)
        {
            YSWL.MALL.Model.Shop.Sales.SalesRule salesRuleModel;
            YSWL.MALL.Model.Shop.Sales.SalesItem salesItemModel;

            YSWL.MALL.Model.Shop.Sales.SalesRuleProduct salesRuleProductModle = salesRuleProductBLL.GetRuleProduct(id, userRank);
            if (null != salesRuleProductModle)
            {
                salesRuleModel = salesRuleBLL.GetModelByCache(salesRuleProductModle.RuleId);
                if (null != salesRuleModel && salesRuleModel.RuleMode == 0) //单个商品
                {
                    salesItemModel = salesItemBLL.GetLowesSales(salesRuleModel.RuleId);
                    if (null != salesItemModel && salesItemModel.RateValue > 0)
                    {
                        return salesItemModel.RateValue * 0.1;
                    }
                }
            }
            return 0;
        }
        #endregion

        #region 搜索商品列表
        [JsonRpcMethod("SearchProductList", Idempotent = false)]
        [JsonRpcHelp("搜索商品列表")]
        public JsonObject SearchProductList(int cid = 0, int brandid = 0, string keyword = "", string orderby = "hot", string price = "0-0",
                                        int? page = 1, int pageNum = 10)
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
            int toalCount = productManage.GetSearchCountEx(cid, brandid, keyword, price);
            JsonObject result = new JsonObject();
            YSWL.Json.JsonArray jsonArray = new JsonArray();
            JsonObject json;
            //ViewBag.TotalCount = toalCount;
            result.Put("list_count", toalCount);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> list;
            try
            {
                if (String.IsNullOrEmpty(orderby))
                {
                    orderby = "default";
                }
                //if (orderby.ToString().ToLower() != "default" && orderby.ToString().ToLower() != "hot" && orderby.ToString().ToLower() != "new" && orderby.ToString().ToLower() != "price" && orderby.ToString().ToLower() != "pricedesc")
                //{

                //    return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
                //}
                list = productManage.GetSearchListEx(cid, brandid, keyword, price, orderby, startIndex, endIndex);
            }
            catch (Exception ex)
            {
                YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message),
                   ex.StackTrace, Request);
                return new Result(ResultStatus.Error, ex.Message);
            }
            //获取总条数
            if (toalCount < 1)
            {
                return new Result(ResultStatus.Success, null);
            }
            if (list == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            foreach (YSWL.MALL.Model.Shop.Products.ProductInfo item in list)
            {
                json = new JsonObject();
                json.Put("id", item.ProductId);
                json.Put("name", item.ProductName);
                json.Put("pic",  item.ThumbnailUrl1);
                json.Put("marketprice", item.MarketPrice.HasValue ? item.MarketPrice.Value.ToString("F") : "0.00");
                json.Put("saleprice", item.LowestSalePrice.ToString("F"));
                json.Put("commentCount", reviewsBLL.GetRecordCount("Status=1 and ProductId=" + item.ProductId));
                jsonArray.Add(json);
            }
            result.Put("productlist", jsonArray);
            return new Result(ResultStatus.Success, result);
        }
        #endregion

        #region 商品详情
        [JsonRpcMethod("ProductDetail", Idempotent = false)]
        [JsonRpcHelp("商品详情")]
        public  JsonObject ProductDetail(int pId = -1,int userId=-1)
        {
            if (pId <= 0)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            YSWL.MALL.ViewModel.Shop.ProductModel model = new ProductModel();
            model.ProductInfo = productManage.GetModel(pId);
            if (model.ProductInfo == null) {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
           // List<YSWL.MALL.Model.Shop.Products.ProductInfo> list = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            //list.Add(model.ProductInfo);
            JsonArray jsonArray = new JsonArray();
           // JsonObject result = new JsonObject();
            JsonObject json;
            model.ProductSkus = skuBLL.GetProductSkuInfo(pId);
            if (model.ProductSkus.Count < 1)
            {
                return new Result(ResultStatus.Success, null);
            }
            string salesprice = model.ProductSkus[0].SalePrice.ToString("F");
            JsonArray picArray = new JsonArray();
            YSWL.MALL.BLL.Shop.Products.ProductImage imageManage = new YSWL.MALL.BLL.Shop.Products.ProductImage();
            model.ProductImages = imageManage.ProductImagesList(pId);
            JsonObject pidJson;
            YSWL.MALL.ViewModel.Shop.ProductSKUModel productSKUModel = skuBLL.GetProductSKUInfoByProductId(model.ProductInfo.ProductId);
            if (productSKUModel == null)
            {
                return new Result(ResultStatus.Success, null);
            }

            JsonObject attr;
         //   foreach (YSWL.MALL.Model.Shop.Products.ProductInfo item in list)
          //  {
                json = new JsonObject();
                json.Put("id", model.ProductInfo.ProductId);
                json.Put("name", model.ProductInfo.ProductName);
                json.Put("marketprice", ((Decimal)model.ProductInfo.MarketPrice).ToString("F"));
                json.Put("saleprice", salesprice);
                json.Put("xmltext", model.ProductInfo.Description);
                json.Put("leftTime", model.ProductInfo.ProSalesEndDate.ToString("yyyy年MM月dd日HH时mm分ss秒")); //TimeSpan.Parse(item.ProSalesEndDate.ToShortTimeString().ToString()).TotalSeconds);
                json.Put("pic", model.ProductInfo.ThumbnailUrl1);
                foreach (YSWL.MALL.Model.Shop.Products.ProductImage pro in model.ProductImages)
                {
                    pidJson = new JsonObject();
                    picArray.Add(pro.ThumbnailUrl1);
                }
                json.Put("bigPic", picArray);
                json.Put("commentCount", reviewsBLL.GetRecordCount("Status=1 and ProductId=" + pId));
                json.Put("sku", model.ProductSkus[0].SKU);
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
                        if (IsOpenAlertStock &&
                            productSKUModel.ListSKUInfos[0].Stock <= productSKUModel.ListSKUInfos[0].AlertStock)
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
                SKUInfoToJson(json, productSKUModel.ListSKUInfos);
               // result.Put("product", json);
                #region 是否已加入收藏
                //是否已经加入收藏
                if (userId > 0 && favoBLL.Exists(pId, userId, 1))
                {
                    json.Put("addFav", true); //已加入
                }
                else
                {
                    json.Put("addFav", false); //未加入
                }

                #endregion
            return new Result(ResultStatus.Success, json);
        }
        private YSWL.Json.JsonObject SKUInfoToJson(YSWL.Json.JsonObject json, List<YSWL.MALL.Model.Shop.Products.SKUInfo> list)
        {
            if (list == null || list.Count < 1) return null;

            YSWL.Json.JsonObject jsonSKU = new YSWL.Json.JsonObject();
            long[] key;
            int index;
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
                    price = item.SalePrice
                });
            }

            //获取最小/最大价格
            list.Sort((x, y) => x.SalePrice.CompareTo(y.SalePrice));
            json.Put("defaultPrice", new
            {
                minPrice = list[0].SalePrice,
                maxPrice = list[list.Count - 1].SalePrice
            });
            json.Put("skuData", jsonSKU);
            return json;
        }
        #endregion
   
        #region 商品列表

        [JsonRpcMethod("ProductList", Idempotent = false)]
        [JsonRpcHelp("商品列表")]
        public    JsonObject ProductList(int cId = 0, int brandid = 0, string attrvalues = "0", string orderby = "default", string price = "",
                                  int? page = 1, int pageNum = 10)
        {
            #region
            if (pageNum == 0)
            {
                pageNum = 10;
            }
            if (String.IsNullOrEmpty(page.ToString()))
            {
                page = 1;
            }
            if (String.IsNullOrEmpty(orderby))
            {
                orderby = "default";
            }
            bool openAlertStock = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
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
            //重置页面索引
            page = page.HasValue && page.Value > 1 ? page.Value : 1;
            //计算分页起始索引
            int startIndex = page.Value > 1 ? (page.Value - 1) * pageNum + 1 : 0;
            //计算分页结束索引
            int endIndex = page.Value > 1 ? startIndex + pageNum - 1 : pageNum;
            int toalCount = productManage.GetProductsCountEx(cId, brandid, attrvalues, price);
            JsonObject result = new JsonObject();
            JsonArray jsonArray = new JsonArray();
            JsonObject json;
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> prolist = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            result.Put("list_count", toalCount);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> list;
            try
            {
                if (String.IsNullOrEmpty(orderby))
                {
                    return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
                }
                //if (orderby.ToString().ToLower() != "default" && orderby.ToString().ToLower() != "hot" && orderby.ToString().ToLower() != "new" && orderby.ToString().ToLower() != "price" && orderby.ToString().ToLower() != "pricedesc")
                //{
                //    //orderby = "default";
                //    return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
                //}
                list = productManage.GetProductsListEx(cId, brandid, attrvalues, price, orderby, startIndex, endIndex);
            }
            catch (Exception ex)
            {
                YSWL.MALL.BLL.SysManage.LogHelp.AddErrorLog(string.Format(ERROR_MSG_LOG, Request.Headers[REQUEST_HEADER_METHOD], ex.Message),
                   ex.StackTrace, Request);
                return new Result(ResultStatus.Error, ex.Message);
            }
            //获取总条数
            if (toalCount < 1)
            {
                return new Result(ResultStatus.Success, null);
            }
            if (list == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            string strKey = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_App_Product_Attr") ?? "颜色，规格";
            keys = strKey.Split(',');
            YSWL.MALL.Model.Shop.Products.SKUInfo skuInfoModel;
            foreach (YSWL.MALL.Model.Shop.Products.ProductInfo item in list)
            {
                json = new JsonObject();
                var skulist = skuBLL.GetProductSkuInfo(item.ProductId);
                skuInfoModel = skulist == null || skulist.Count == 0 ? new YSWL.MALL.Model.Shop.Products.SKUInfo() : skulist[0];
                json.Put("id", item.ProductId);
                json.Put("name", item.ProductName);
                attrList = GetAttrValues(item.ProductId, keys);
                json.Put("standard", attrList);
                json.Put("pic", item.ThumbnailUrl1);
                json.Put("marketprice", item.MarketPrice.HasValue ? item.MarketPrice.Value.ToString("F") : "0.00");
                json.Put("saleprice", item.LowestSalePrice.ToString("F"));
                json.Put("commentCount", reviewsBLL.GetRecordCount("Status=1 and ProductId=" + item.ProductId));
                json.Put("sku", skuInfoModel.SKU);
                json.Put("discount", GetProductSale(item.ProductId,null));
                json.Put("discountRule", GetAllSaleRule(item.ProductId,null));
                //json.Put("producttype", item.ProductType);
                json.Put("producttype", 1);

                json.Put("limitQty", productBLL.GetRestrictionCount(skuInfoModel.ProductId));//限购数量
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
                jsonArray.Add(json);
            }
            result.Put("productlist", jsonArray);
            #endregion
            JsonObject attrJson;
            JsonArray jsonAttrKey = new JsonArray();
            YSWL.MALL.BLL.Shop.Products.AttributeInfo attributeBLL = new YSWL.MALL.BLL.Shop.Products.AttributeInfo();
            List<YSWL.MALL.Model.Shop.Products.AttributeInfo> attributeInfo = attributeBLL.GetAttributeListByCateID(cId, true);
            foreach (YSWL.MALL.Model.Shop.Products.AttributeInfo item in attributeInfo)
            {

                attrJson = new JsonObject();
                attrJson.Put("key", item.AttributeName);
                attrJson.Put("values", GetValueInfo(item.AttributeId.ToString()));
                jsonAttrKey.Add(attrJson);
            }
            result.Put("list_filter", jsonAttrKey);
            return new Result(ResultStatus.Success, result);
        }

        public JsonArray GetValueInfo(string attributeId)
        {
            JsonObject attrJson;
            JsonArray array = new JsonArray();
            YSWL.MALL.BLL.Shop.Products.AttributeValue valueBLL = new YSWL.MALL.BLL.Shop.Products.AttributeValue();
            List<YSWL.MALL.Model.Shop.Products.AttributeValue> valueInfo = valueBLL.GetModelList(" AttributeId=" + attributeId);
            foreach (YSWL.MALL.Model.Shop.Products.AttributeValue value in valueInfo)
            {
                attrJson = new JsonObject();
                attrJson.Put("id", value.ValueId);
                attrJson.Put("name", value.ValueStr);
                array.Add(attrJson);
            }
            return array;
        }
 
        #endregion
 
        #region 限时抢购商品列表
        [JsonRpcMethod("CountDownList", Idempotent = false)]
        [JsonRpcHelp("秒杀商品列表")]
        public JsonObject CountDownList(int page = 1, int pageNum = 10)
        {
            if (pageNum < 1)
            {
                pageNum = 10;
            }
            if (page < 1)
            {
                page = 1;
            }
            ProductListModel model = new ProductListModel();
            //计算分页起始索引
            int startIndex = page > 1 ? (page - 1) * pageNum + 1 : 0;
            //计算分页结束索引
            int endIndex = page > 1 ? startIndex + pageNum - 1 : pageNum;
            int toalCount = productManage.GetProSalesCount();
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> list = productManage.GetProSalesList(startIndex, endIndex);
            JsonObject result = new JsonObject();
            YSWL.Json.JsonArray jsonArray = new JsonArray();
            JsonObject json;
            //获取总条数
            if (toalCount < 1)
            {
                return new Result(ResultStatus.Success, null);
            }
            result.Put("list_count", toalCount);
            foreach (YSWL.MALL.Model.Shop.Products.ProductInfo item in list)
            {
                json = new JsonObject();
                json.Put("id", item.ProductId);
                json.Put("sku", item.ProductCode);
                json.Put("countDownId", item.CountDownId);
                json.Put("name", item.ProductName);
                json.Put("pic", item.ThumbnailUrl1);
                json.Put("marketprice", item.MarketPrice.HasValue ? item.MarketPrice.Value.ToString("F") : "");
                json.Put("saleprice", item.ProSalesPrice.ToString("F"));
                json.Put("lefttime", item.ProSalesEndDate.ToString("yyyy年MM月dd日HH时mm分ss秒"));
                json.Put("limitQty", item.LimitCount);
                jsonArray.Add(json);
            }
            result.Put("productlist", jsonArray);
            return new Result(ResultStatus.Success, result);
        }
        #endregion

        #region 团购列表
        [JsonRpcMethod("GroupBuy", Idempotent = false)]
        [JsonRpcHelp("团购列表")]
        public JsonObject GroupBuy(int cid, int regionid = -1, string mod = "default", int? pageIndex = 1, int pageSize = 16)
        {
            if (regionid <= 0)
            {
                regionid = YSWL.MALL.BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_GroupBuy_DefaultRegion");
            }
            pageSize = pageSize > 0 ? pageSize : 10;
            regionid = regionid <= 0 ? 643 : regionid;//防止从cache中未取到参数报错
            //  regionid = 643;
            List<YSWL.MALL.Model.Shop.PromoteSales.GroupBuy> groupList;//new List<Model.Shop.PromoteSales.GroupBuy>();
            //重置页面索引
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 0;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;
            groupList = groupBuy.GetListByPage(null, cid, regionid, mod, startIndex, endIndex);
            List<JsonObject> objectList = new List<JsonObject>();
            JsonObject jsonObject;
            if (null != groupList)
            {
                foreach (var item in groupList)
                {
                    jsonObject = new JsonObject();
                    YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = productManage.GetModelByCache(item.ProductId);
                    if (null != productInfo)
                    {
                        jsonObject.Put("sku", productInfo.ProductCode);
                        jsonObject.Put("marketprice", productInfo.MarketPrice.HasValue ? productInfo.MarketPrice.Value.ToString("F") : "0");
                    }
                    jsonObject.Put("groupBuyid", item.GroupBuyId);
                    jsonObject.Put("name", item.ProductName);
                    jsonObject.Put("pic", item.GroupBuyImage);
                    jsonObject.Put("saleprice", item.Price.ToString("F"));
                    jsonObject.Put("maxCount", item.MaxCount);//总限购数量
                    jsonObject.Put("groupCount", item.GroupCount);//团购满足数量
                    jsonObject.Put("limitQty", item.LimitQty);//单个商品限购数量
                    jsonObject.Put("startDate", item.StartDate.ToString("yyyy年MM月dd日HH时mm分ss秒"));
                    jsonObject.Put("endDate", item.EndDate.ToString("yyyy年MM月dd日HH时mm分ss秒"));
                    jsonObject.Put("id", item.ProductId);
                    objectList.Add(jsonObject);
                }
                return new Result(ResultStatus.Success, objectList);
            }
            return new Result(ResultStatus.Success, "[]");
        }
        #endregion
 

    }
}