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
using YSWL.MALL.BLL.SysManage;
using YSWL.Components.Handlers.API;
using YSWL.Json;
using YSWL.Json.RPC;
using YSWL.MALL.Model.Members;
using System.Collections.Generic;
using YSWL.MALL.Model.Shop.Products;
using System.Linq;
using YSWL.MALL.ViewModel.Shop;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.BLL.Shop.Order;
using YSWL.MALL.Model.CMS;

namespace YSWL.MALL.API.Shop.v1
{
    public partial class ShopHandler
    {
        YSWL.MALL.BLL.CMS.Content contentBLL = new YSWL.MALL.BLL.CMS.Content();
        YSWL.MALL.BLL.CMS.ContentClass contentclassBLL = new YSWL.MALL.BLL.CMS.ContentClass();
        private YSWL.MALL.BLL.Shop.Products.ProductReviews reviewsBLL = new YSWL.MALL.BLL.Shop.Products.ProductReviews();
        private readonly Orders _orderManage = new Orders();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productManage = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productBLL = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        YSWL.MALL.BLL.Members.PointsRule ruleBLL = new YSWL.MALL.BLL.Members.PointsRule();
        YSWL.MALL.BLL.Shop.Sales.SalesRule salesRuleBLL = new BLL.Shop.Sales.SalesRule();
        YSWL.MALL.BLL.Shop.Sales.SalesItem salesItemBLL = new BLL.Shop.Sales.SalesItem();
        
        YSWL.MALL.BLL.Shop.Favorite favoBLL = new YSWL.MALL.BLL.Shop.Favorite();
        private readonly YSWL.MALL.BLL.Members.PointsDetail detailBLL = new YSWL.MALL.BLL.Members.PointsDetail();

        private YSWL.MALL.BLL.Shop.Products.AttributeInfo attrBLL = new YSWL.MALL.BLL.Shop.Products.AttributeInfo();
   
        [JsonRpcMethod("HotKeyword", Idempotent = false)]
        [JsonRpcHelp("热门搜索")]
        public JsonObject HotKeyword(int Cid = 0, int Top = 30)
        {
            if(Top==0)
            {
                Top = 10;
            }
            YSWL.MALL.BLL.Shop.Products.HotKeyword keywordBLL = new YSWL.MALL.BLL.Shop.Products.HotKeyword();
            List<YSWL.MALL.Model.Shop.Products.HotKeyword> keywords = keywordBLL.GetKeywordsList(Cid, Top);
         
            if (keywords == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            JsonArray result = new JsonArray();
            foreach (HotKeyword item in keywords)
            {
                //json = new JsonObject();
                //json.Put("keywords", item.Keywords);
                //result.Add(json);
                result.Add(String.IsNullOrWhiteSpace(item.Keywords) ? "" : item.Keywords);
            }
            return new Result(ResultStatus.Success, result);
        }

        [JsonRpcMethod("ProductRec", Idempotent = false)]
        [JsonRpcHelp("促销商品")]
        public JsonObject ProductRec(int IndexRec, int Cid = 0, int Top = 5)
        {
            if (Top == 0)
            {
                Top = 10;
            }
            ProductRecType Type = YSWL.Common.Globals.SafeEnum<YSWL.MALL.Model.Shop.Products.ProductRecType>(IndexRec.ToString(), ProductRecType.IndexRec, true);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productBLL.GetProductRecList(Type, Cid, Top);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> prolist = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            if (productList == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            JsonArray result = new JsonArray();
            JsonObject json;
            foreach (var item in productList)
            {
                json = new JsonObject();
                json.Put("id", item.ProductId);
                json.Put("name", item.ProductName);
                json.Put("pic",  item.ThumbnailUrl1);
                result.Add(json);
            }
            return new Result(ResultStatus.Success, result);
        }


        [JsonRpcMethod("ShakeProduct", Idempotent = false)]
        [JsonRpcHelp("摇摇")]
        public JsonObject ShakeProduct(int IndexRec, int Cid = 0, int Top = 1)
        {
            ProductRecType Type = YSWL.Common.Globals.SafeEnum<YSWL.MALL.Model.Shop.Products.ProductRecType>(IndexRec.ToString(), ProductRecType.IndexRec, true);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> productList = productBLL.GetProductRanListByRec(Type, Cid, Top);
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> prolist = new List<YSWL.MALL.Model.Shop.Products.ProductInfo>();
            if (productList == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            JsonArray result = new JsonArray();
            JsonObject json;
            foreach (var item in productList)
            {
                json = new JsonObject();
                json.Put("id", item.ProductId);
                json.Put("title", item.ProductName);
                json.Put("pic",  item.ThumbnailUrl1);
                json.Put("marketprice", item.MarketPrice.HasValue?item.MarketPrice.Value.ToString("F"):"0.00");
                json.Put("saleprice", item.LowestSalePrice.ToString("F"));
                result.Add(json);
            }
            return new Result(ResultStatus.Success, result);
        }
 
        [JsonRpcMethod("CategoryList", Idempotent = false)]
        [JsonRpcHelp("类别列表")]
        public JsonObject CategoryList(int Cid, int Top = 7)
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = cateList.FindAll(c => c.ParentCategoryId == Cid);
            if (categoryInfos == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            JsonObject baseJson;
            JsonArray result = new JsonArray();
            foreach (CategoryInfo item in categoryInfos)
            {
                baseJson = new JsonObject();
                baseJson.Put("id", item.CategoryId);
                baseJson.Put("title", item.Name);
                baseJson.Put("haschild", item.HasChildren);
                baseJson.Put("description", item.Description);
                baseJson.Put("pic", item.ImageUrl);
                if (item.HasChildren == true)
                {
                    baseJson.Put("childlist", GetCategory(item.CategoryId, cateList));
                }
                result.Add(baseJson);
            }
            return new Result(ResultStatus.Success, result);
        }
        public JsonArray GetCategory(int parentId, List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList)
        {
            JsonObject currjson;
            JsonArray array = new JsonArray();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = cateList.FindAll(info => info.ParentCategoryId == parentId);
            if (categoryInfos != null && categoryInfos.Count() > 0)
            {
                foreach (YSWL.MALL.Model.Shop.Products.CategoryInfo item in categoryInfos)
                {
                    currjson = new JsonObject();
                    currjson.Put("id", item.CategoryId);
                    currjson.Put("haschild", item.HasChildren);
                    currjson.Put("parentId", item.ParentCategoryId);
                    currjson.Put("title", item.Name);
                    currjson.Put("pic", item.ImageUrl);
                    if (item.HasChildren == true)
                    {
                        currjson.Put("childlist", GetCategory(item.CategoryId, cateList));
                    }
                    array.Add(currjson);
                }
            }
            return array;
        }

        [JsonRpcMethod("HelpList", Idempotent = false)]
        [JsonRpcHelp("帮助列表")]
        public JsonObject HelpList(int classid)
        {
            if (classid < 1)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            YSWL.MALL.Model.CMS.ContentClass classmodel;
            JsonObject json;
            JsonArray result = new JsonArray();
            List<YSWL.MALL.Model.CMS.ContentClass> list = contentclassBLL.GetModelList(classid, out  classmodel);
            if (list == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            foreach (ContentClass item in list)
            {
                json = new JsonObject();
                json.Put("id", item.ClassID);
                json.Put("title", item.ClassName);
                json.Put("childlist", ContentTitleList(item.ClassID));
                result.Add(json);
            }
            return new Result(ResultStatus.Success, result);
        }
        //文章列表
        public JsonArray ContentTitleList(int classid)
        {

            JsonObject json;
            JsonArray array = new JsonArray();
            List<YSWL.MALL.Model.CMS.Content> list = contentBLL.GetModelList(classid, 0);

            foreach (Content item in list)
            {
                json = new JsonObject();
                json.Put("id", item.ContentID);
                json.Put("title", item.Title);
                array.Add(json);
            }
            return array;
        }

        [JsonRpcMethod("HelpDetail", Idempotent = false)]
        [JsonRpcHelp("帮助内容")]
        public JsonObject HelpDetail(int contentid)
        {
            if (contentid < 1)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }         
            JsonObject result = new JsonObject();
            YSWL.MALL.Model.CMS.Content model = contentBLL.GetModelByCache(contentid);
            if (model == null)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed(ERROR_CODE_ARGUMENT, ERROR_MSG_ARGUMENT));
            }
            JsonObject json = new JsonObject();
            json.Put("title", model.Title);
            json.Put("content", model.Description);
            return new Result(ResultStatus.Success, json);
        }

 
        #region 获取属性的变量
 
        private string[] keys;// = strKey.Split(',');
        List<JsonObject> attrList;
        #endregion
 
        #region 首页
        [JsonRpcMethod("HomeIndex", Idempotent = false)]
        [JsonRpcHelp("首页")]
        public JsonObject HomeIndex()
        {
            JsonObject jsonObject = new JsonObject();
            List<JsonObject> jsObjectList;
            JsonObject json;

            #region home_banner 部分

            int aid = YSWL.MALL.BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_App_Index_AdId");
            aid = aid > 0 ? aid : 0;//如果参数表没有设置广告位 
            YSWL.MALL.BLL.Settings.Advertisement BLLadPosition = new BLL.Settings.Advertisement();
            List<YSWL.MALL.Model.Settings.Advertisement> listAds = BLLadPosition.GetListByAidCache(aid);
            if (listAds == null || listAds.Count < 1)
            {
                jsonObject.Put("home_banner", null);
            }
            else
            {
                jsObjectList = new List<JsonObject>();
                foreach (var item in listAds)
                {
                    json = new JsonObject();
                    json.Put("id", item.AdvertisementId);
                    json.Put("pic", item.FileUrl);
                    json.Put("url", item.NavigateUrl);
                    json.Put("title", item.AdvertisementName); 
                    jsObjectList.Add(json);
                }
                jsonObject.Put("home_banner", jsObjectList);
            }
            #endregion

            #region brandlist 部分 
            YSWL.MALL.BLL.Shop.Products.BrandInfo BLLBrand = new BLL.Shop.Products.BrandInfo();
            List<YSWL.MALL.Model.Shop.Products.BrandInfo> listBrands = BLLBrand.GetBrandList("", -1);
            if (null == listBrands || listBrands.Count < 1)
            {
                jsonObject.Put("brandlist", null);
            }
            else
            {
                jsObjectList = new List<JsonObject>();
                foreach (var item in listBrands)
                {
                    json = new JsonObject();
                    json.Put("id", item.BrandId);
                    json.Put("pic", item.Logo);
                    jsObjectList.Add(json);
                }
                jsonObject.Put("brandlist", jsObjectList);
            }
            #endregion

            #region classifies 部分
            YSWL.MALL.BLL.Shop.Products.CategoryInfo BLLCate = new BLL.Shop.Products.CategoryInfo() ;
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = BLLCate.GetMainCateList(0);
            if (null == cateList)
            {
                jsonObject.Put("classifies", null);
            }
            else
            {
                jsObjectList = new List<JsonObject>();
                foreach (var cate in cateList)
                {
                    json = new JsonObject();
                    json.Put("id", cate.CategoryId);
                    json.Put("name", cate.Name);
                    json.Put("des", cate.Description);
                    json.Put("pic", cate.ImageUrl);
                    jsObjectList.Add(json);
                }
                jsonObject.Put("classifies", jsObjectList);
            }
            #endregion

            #region galleryproduct  部分

            List<JsonObject> galleryList = new List<JsonObject>();
            string recHotType = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_App_Index_recHotType") ?? "hot";
            int recHotCate = Common.Globals.SafeInt(YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_App_Index_recHotCid"), 0);
            string recType = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_App_Index_recType") ?? "new";
            string recCates = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_App_Index_Cid"); //"2,61,31"
            int recTop = YSWL.MALL.BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_App_Index_recCounts");
            JsonObject cxjpJson = new JsonObject();
            List<JsonObject> proList = new List<JsonObject>();
            JsonObject proJson;

            #region 畅销精品
            ProductRecType hotType = GetType(recHotType);
            ProductRecType cateRecType = GetType(recType); 
            YSWL.MALL.BLL.Shop.Products.ProductInfo productBLL = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
            recHotCate = recHotCate < 0 ? 0 : recHotCate;
            recTop = recTop < 0 ? 0 : recTop;
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> listProducts = productBLL.GetProductRecList(hotType, recHotCate, recTop);
            if (null != listProducts)
            {
                foreach (var item in listProducts)
                {
                    proJson = new JsonObject();
                    proJson.Put("id", item.ProductId);
                    proJson.Put("name", item.ProductName);
                    proJson.Put("pic", item.ThumbnailUrl1);
                    proJson.Put("saleprice", item.LowestSalePrice.ToString("F"));
                    proJson.Put("discount", GetProductSale(item.ProductId,null));
                    proList.Add(proJson);
                }
            }
            cxjpJson.Put("cid", recHotCate);
            cxjpJson.Put("name", "畅销精品");
            cxjpJson.Put("productlist", proList);
            galleryList.Add(cxjpJson);
            #endregion

            #region 推荐的分类数据 
            
           // List<JsonObject> productList;
            //JsonObject productJson;
            YSWL.MALL.Model.Shop.Products.CategoryInfo modelCate;
            if (!String.IsNullOrWhiteSpace(recCates))
            {
                string[] catesId = recCates.Split(',');
                for (int i = 0; i < catesId.Length; i++)
                {
                    proList = new List<JsonObject>();
                    cxjpJson = new JsonObject();
                    int cateId = Common.Globals.SafeInt(catesId[i], 0);
                    if (cateId <= 0)
                    {
                        continue;
                    }
                    modelCate = BLLCate.GetModelByCache(cateId);
                    if (modelCate==null)
                    {
                         continue;
                    }
                    List<YSWL.MALL.Model.Shop.Products.ProductInfo> listPro = productBLL.GetProductRecList(cateRecType, cateId, recTop);
                    if (null != listPro && listPro.Count > 0)
                    {
                        foreach (var item in listPro)
                        {
                            proJson = new JsonObject();
                            proJson.Put("id", item.ProductId);
                            proJson.Put("name", item.ProductName);
                            proJson.Put("pic", item.ThumbnailUrl1);
                            proJson.Put("saleprice", item.LowestSalePrice.ToString("F"));
                            proJson.Put("discount", GetProductSale(item.ProductId,null));
                            proList.Add(proJson);
                        }
                    }
                    cxjpJson.Put("cid", modelCate.CategoryId);
                    cxjpJson.Put("name", modelCate.Name);
                    cxjpJson.Put("productlist", proList);
                    galleryList.Add(cxjpJson);
                }
            }
            #endregion

            jsonObject.Put("galleryproduct", galleryList);
            #endregion
            return new Result(ResultStatus.Success, jsonObject);
        }

        private ProductRecType GetType(string type)
        {
            switch (type)
            {//rec，new，hot，cheap，
                case "rec":
                    return ProductRecType.Recommend;//推荐
                case "indexRec":
                    return ProductRecType.IndexRec;//首页推荐
                case "new":
                    return ProductRecType.Latest;//最新
                case "hot":
                    return ProductRecType.Hot;//热卖
                case "cheap":
                    return ProductRecType.Cheap;//特价
                default:
                    return ProductRecType.IndexRec;
            }
        }
        #endregion

        [JsonRpcMethod("GetPhone", Idempotent = false)]
        [JsonRpcHelp("获取电话")]
        public JsonObject GetPhone() 
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.Put("phone", BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MShop_Phone"));
            return new Result(ResultStatus.Success, jsonObject);
        }


        #region   获取升级信息
        [JsonRpcMethod("GetUpgradeInfo", Idempotent = false)]
        [JsonRpcHelp("获取升级信息")]
        public JsonObject GetUpgradeInfo()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject.Put("versionNum", YSWL.Common.ConfigHelper.GetConfigString("App_Shop_VersionNum"));//版本号
            jsonObject.Put("versionDesc", YSWL.Common.ConfigHelper.GetConfigString("App_Shop_VersionDesc"));//版本描述
            jsonObject.Put("versionUrl", YSWL.Common.ConfigHelper.GetConfigString("FilePath_Android"));  //APP 下载地址
            jsonObject.Put("isEnforce", YSWL.Common.ConfigHelper.GetConfigString("App_Shop_IsEnforce"));//是否强制升级
            return new Result(ResultStatus.Success, jsonObject);
        }
        #endregion 
    }
}