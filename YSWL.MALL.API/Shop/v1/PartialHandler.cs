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

namespace YSWL.MALL.API.Shop.v1
{
    public partial class ShopHandler
    {
        private readonly YSWL.MALL.BLL.Shop.Coupon.CouponRule cuponruleBLL = new YSWL.MALL.BLL.Shop.Coupon.CouponRule();
        private YSWL.MALL.BLL.Members.Guestbook guestbookBLL = new YSWL.MALL.BLL.Members.Guestbook();
        private YSWL.MALL.BLL.Members.UsersExp userEXBLL = new YSWL.MALL.BLL.Members.UsersExp();

        //#region 获取广告位
        //[JsonRpcMethod("AdDetail", Idempotent = true)]
        //[JsonRpcHelp("根据广告位Id获取广告位")]
        //public JsonArray AdDetail(int Aid)
        //{
        //    YSWL.MALL.BLL.Settings.Advertisement BLL = new YSWL.MALL.BLL.Settings.Advertisement();
        //    List<Advertisement> list = BLL.GetListByAidCache(Aid);
        //    YSWL.Json.JsonArray array = new JsonArray();
        //    JsonObject json;
        //    JsonObject result = new JsonObject();
        //    if (list == null)
        //    {
        //        return null;
        //    }
        //    foreach (Advertisement item in list)
        //    {
        //        json = new JsonObject();
        //        json.Put("id",item.AdvertisementId);
        //        json.Put("title",item.AlternateText);
        //        json.Put("pic",item.FileUrl);
        //        json.Put("url", item.NavigateUrl);
        //        array.Add(json);
        //    }
        //    return array;
        //}
        //#endregion
 
        //[JsonRpcMethod("HomeCategoryList", Idempotent = false)]
        //[JsonRpcHelp("分类请求列表-左菜单")]
        //public JsonArray Category(int parentId = 0, int Top = 10)
        //{
        //    List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
        //    List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = cateList.FindAll(c => c.ParentCategoryId == parentId);

        //    JsonObject json;
        //    YSWL.Json.JsonArray array = new JsonArray();
        //    JsonObject result = new JsonObject();
        //    foreach (CategoryInfo item in categoryInfos)
        //    {
        //        json = new JsonObject();
        //        json.Put("id", item.CategoryId);
        //        json.Put("name", item.Name);
        //        //json.Put("isleadnode", item.HasChildren);
        //        //json.Put("parentId", parentId);
        //        json.Put("pic", item.ImageUrl);
        //        //json.Put("tag", item.Path);
        //        array.Add(json);
        //    }
        //    return array;
        //}
 
        #region  获取地区地址
        [JsonRpcMethod("GetRegionList", Idempotent = false)]
        [JsonRpcHelp("地址列表")]
        public JsonObject GetRegionList(int regionId = 0)
        {
            List<YSWL.MALL.Model.Ms.Regions> AllRegionList = RegionBLL.GetModelList("");
            List<YSWL.MALL.Model.Ms.Regions> ProvinceList = AllRegionList.Where(c => c.Depth == 1).ToList();
            if (regionId > 0)
            {
                ProvinceList = ProvinceList.Where(c => c.RegionId == regionId).ToList();
            }
            if (ProvinceList == null)
            {
                return new Result(ResultStatus.Success, null);
            }
            JsonArray result = new JsonArray();
            JsonObject baseJson;
            foreach (Regions item in ProvinceList)
            {
                baseJson = new JsonObject();
                baseJson.Put("id", item.RegionId);
                baseJson.Put("name", item.RegionName);
                baseJson.Put("depth", item.Depth);
                if (regionId>0)
                {
                    baseJson.Put("childlist", GetDataByParentId(item.RegionId, AllRegionList));
                }   
                result.Add(baseJson);
            }
            return new Result(ResultStatus.Success, result);
        }

        private JsonArray GetDataByParentId(int ParentId, List<YSWL.MALL.Model.Ms.Regions> AllRegionList)
        {
            JsonObject currjson;
            JsonArray array = new JsonArray();
            List<YSWL.MALL.Model.Ms.Regions> list = AllRegionList.Where(c => c.ParentId == ParentId).ToList();
            if (list != null && list.Count > 0)
            {
                foreach (Regions item in list)
                {
                    currjson = new JsonObject();
                    currjson.Put("id", item.RegionId);
                    currjson.Put("name", item.RegionName);
                    currjson.Put("parentid", item.ParentId);
                    currjson.Put("depth", item.Depth);
                    currjson.Put("childlist", GetDataByParentId(item.RegionId, AllRegionList));
                    array.Add(currjson);
                }
            }
            return array;
        }

        #endregion

        #region 获取地区地址名称
        [JsonRpcMethod("GetRegionName", Idempotent = false)]
        [JsonRpcHelp("地区地址名称")]
        public JsonObject GetRegionName(int regionId = 0)
        {
            string name = RegionBLL.GetFullNameById4Cache(regionId);
            return new Result(ResultStatus.Success, name);
        }
        #endregion
 
        #region 留言
        [JsonRpcMethod("LiveMessage", Idempotent = false)]
        [JsonRpcHelp("留言")]
        public JsonObject LiveMessage(int userId, string content, string email, string telephone)
        {
            if (userId < 1)
            {
                return new Result(ResultStatus.Failed, "noUserInfo");
            }
            if (string.IsNullOrWhiteSpace(content))
            {
                return new Result(ResultStatus.Failed, "noMessageInfo");
            }
            if (string.IsNullOrWhiteSpace(telephone))
            {
                return new Result(ResultStatus.Failed, "noTelephoneInfo");
            }
            YSWL.MALL.Model.Members.Guestbook gbook = new Model.Members.Guestbook();
            YSWL.MALL.Model.Members.Users userModel = BLLUser.GetModelByCache(userId);
            if (null != userModel)
            {
                gbook.CreateNickName = userModel.NickName ?? userModel.UserName;
                gbook.CreatedDate = DateTime.Now;
                gbook.ReplyCount = 0;
                gbook.Status = 0;
                gbook.Title = "APP留言，来自--" + gbook.CreateNickName;
                gbook.Description = content;
                gbook.CreatorEmail = email;
                gbook.CreatorPhone = telephone;
            }
            if (guestbookBLL.Add(gbook) > 0)
            {
                return new Result(ResultStatus.Success, "success");
            }
            return new Result(ResultStatus.Error, "error");
        }
        #endregion

        #region 订货版本
        [JsonRpcMethod("checkVersion", Idempotent = false)]
        [JsonRpcHelp("版本")]
        public JsonObject checkVersion()
        {
            string version = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_App_Version") ?? "1.0";
            string description = YSWL.MALL.BLL.SysManage.ConfigSystem.GetDescription("Shop_App_Version") ?? ""; 
          string defaultPath = YSWL.Common.ConfigHelper.GetConfigString("FilePath_Android");
          string downLoadAddress = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_App_Download_Address") ?? defaultPath; 
            JsonObject json = new JsonObject();
            json.Put("versionNum", version);
            json.Put("descr", description);
            json.Put("versionUrl", downLoadAddress);
            return new Result(ResultStatus.Success, json);
        }
        #endregion

        #region 促销规则
        public List<JsonObject> GetAllSaleRule(long productId, YSWL.MALL.Model.Members.UserRank userRank)
        {
            YSWL.MALL.Model.Shop.Sales.SalesRule salesRuleModel;
            List<YSWL.MALL.Model.Shop.Sales.SalesItem> salesItemList;
            JsonObject jsonObject;
            List<JsonObject> jsonList = new List<JsonObject>();
            //会员等级
            YSWL.MALL.Model.Shop.Sales.SalesRuleProduct salesRuleProductModle = salesRuleProductBLL.GetRuleProduct(productId, userRank);
            if (null != salesRuleProductModle)
            {
                salesRuleModel = salesRuleBLL.GetModelByCache(salesRuleProductModle.RuleId);
                if (null != salesRuleModel && salesRuleModel.RuleMode == 0) //单个商品
                {
                    salesItemList = salesItemBLL.GetModelList(string.Format(" RuleId ={0}", salesRuleModel.RuleId));
                    if (null != salesItemList)
                    {
                        foreach (var salesItem in salesItemList)
                        {
                            jsonObject = new JsonObject();
                            jsonObject.Put("saleType", salesItem.ItemType);//0：打折 1：减价 2：固定价格
                            jsonObject.Put("saleName", salesRuleModel.RuleName);//规则名称
                            jsonObject.Put("salueUnit", salesItem.UnitValue);//条件单位数值 单位数值 比如  100个或者 100元
                            jsonObject.Put("salueRuleUnit", salesRuleModel.RuleUnit);//规则单位  0：个 1：元
                            jsonObject.Put("salueValue", salesItem.RateValue);//优惠数值
                            jsonList.Add(jsonObject);
                        }
                    }

                }
            }
            return jsonList;
        }
        #endregion

        #region 优惠券规则
        [JsonRpcMethod("CuponRule", Idempotent = false)]
        [JsonRpcHelp("优惠券规则")]
        public JsonObject GetCuponRule()
        {
            JsonObject jsonObject;
            List<JsonObject> jsList;
            List<YSWL.MALL.Model.Shop.Coupon.CouponRule> ruleList = cuponruleBLL.GetModelList(" Type=1 and Status=1");
            if (null != ruleList)
            {
                jsList = new List<JsonObject>();
                foreach (var item in ruleList)
                {
                    jsonObject = new JsonObject();
                    jsonObject.Put("cuponId", item.RuleId);
                    jsonObject.Put("name", item.Name);
                    jsonObject.Put("needpoint", item.NeedPoint);
                    jsList.Add(jsonObject);
                }
                return new Result(ResultStatus.Success, jsList);
            }
            return new Result(ResultStatus.Success, "nodata");
        }
        #endregion

        #region 兑换优惠券
        [JsonRpcMethod("ExchangeCupon", Idempotent = false)]
        [JsonRpcHelp("兑换优惠券")]
        public JsonObject ExchangeCupon(int ruleId, int userId)
        {
            if (ruleId < 1 || userId < 1)
            {
                return new Result(ResultStatus.Failed, "dataError");
            }
            YSWL.MALL.Model.Shop.Coupon.CouponRule ruleModel = cuponruleBLL.GetModel(ruleId);
            YSWL.MALL.Model.Members.UsersExpModel userEXModel = userEXBLL.GetUsersModel(userId);
            if (ruleModel == null)
            {
                return new Result(ResultStatus.Failed, "rule Data Error");
            }
            if (ruleModel.NeedPoint > userEXModel.Points)
            {
                return new Result(ResultStatus.Failed, "积分不足");
            }
            if (   cuponruleBLL.GenCoupon(ruleModel, userId))
            {
                return new Result(ResultStatus.Success, "ok");
            }
            return new Result(ResultStatus.Failed, "error");
        }
        #endregion
 
    }
}