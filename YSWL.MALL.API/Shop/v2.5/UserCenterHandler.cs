/**
* PartialHandler.cs
*
* 功 能： Shop 部分类API
* 类 名： PartialHandler
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/12/17 17:04:23  GW    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Web.SessionState;
using YSWL.Components.Handlers.API;
using YSWL.Json.RPC.Web;
using YSWL.Json.RPC;
using YSWL.Json;
using YSWL.MALL.Model.Settings;
using System.Collections.Generic;
using YSWL.MALL.Model.Shop.Products;
using System.Linq;
using System.Text.RegularExpressions;
using YSWL.MALL.Model.Ms;
using System.Text;
using YSWL.MALL.Model.Shop;
using YSWL.Accounts.Bus;

namespace YSWL.MALL.API.Shop.v2_5
{
    public partial class ShopHandler
    {
        YSWL.MALL.BLL.Shop.Favorite favoBLL = new YSWL.MALL.BLL.Shop.Favorite();
        private YSWL.MALL.BLL.Members.UsersExp bllUserExp = new YSWL.MALL.BLL.Members.UsersExp();
        #region 我的收藏
        [JsonRpcMethod("MyFavorListV2.5", Idempotent = false)]
        [JsonRpcHelp("我的收藏")]
        public JsonObject MyFavV2_5(int userId = -1, int pageIndex = 1, int pageSize = 10,int regionId=0)
        {
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "error");
            }
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendFormat(" favo.UserId ={0} and favo.Type= {1} ", userId, (int)FavoriteEnums.Product);
            pageSize = pageSize > 0 ? pageSize : 10;
            pageIndex = pageIndex > 0 ? pageIndex : 1;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 0;
            //计算分页结束索引
            int endIndex = pageIndex * pageSize;
            List<YSWL.MALL.ViewModel.Shop.FavoProdModel> favoList = favoBLL.GetFavoriteProductListByPage(strBuilder.ToString(),  startIndex, endIndex);
            List<JsonObject> jList = new List<JsonObject>();
            JsonObject itemJson;
            List<Model.Shop.Products.SKUInfo> skulist;
            int stock;
            bool openAlertStock = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
            foreach (var item in favoList)
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
                
                itemJson = new JsonObject();
                itemJson.Put("id", item.ProductId);
                itemJson.Put("favId", item.FavoriteId);
                itemJson.Put("name", item.ProductName);
                itemJson.Put("createddate", item.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                itemJson.Put("pic", item.ThumbnailUrl1);
                itemJson.Put("saleStatus", item.SaleStatus);
                itemJson.Put("ruleType", (int)ruleProductBll.GetRuleType(item.ProductId, userId));  //促销规则形式： -1：没有参与促销 0：打折  1：减价  2：直降
                itemJson.Put("marketprice", item.MarketPrice.HasValue ? item.MarketPrice.Value.ToString("F") : "0.00");
                itemJson.Put("saleprice", item.LowestSalePrice.ToString("F"));
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
                        itemJson.Put("hasStock", stock > 0 ? true : false);
                    }
                    else
                    {
                        skulist.ForEach(info => {
                            stock += info.Stock;
                        });
                        itemJson.Put("hasStock", stock > 0 ? true : false);
                    }
                }
                else
                {
                    itemJson.Put("hasStock", false);
                }
                jList.Add(itemJson);
            }
            return new Result(ResultStatus.Success, jList);
        }
        #endregion
 
    }
}