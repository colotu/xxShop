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
using YSWL.Components;
using YSWL.MALL.Model.Ms;

namespace YSWL.MALL.API.Shop.v2
{
    public partial class ShopHandler
    {
        #region 获取广告位
        [JsonRpcMethod("AdDetailV2", Idempotent = true)]
        [JsonRpcHelp("广告位")]
        public JsonObject AdDetail()
        {
            int aid =YSWL.MALL.BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_App_Index_AdId");
            aid = aid > 0 ? aid : 59;//如果参数表没有设置广告位 
            YSWL.MALL.BLL.Settings.Advertisement BLL = new YSWL.MALL.BLL.Settings.Advertisement();
            List<Advertisement> list = BLL.GetListByAidCache(aid);
            YSWL.Json.JsonArray array = new JsonArray();
            JsonObject json;
            if (list == null)
            {
                return null;
            }
            foreach (Advertisement item in list)
            {
                json = new JsonObject();
                json.Put("id", item.AdvertisementId);
                json.Put("title", item.AdvertisementName);
                json.Put("pic", item.FileUrl);
                json.Put("url", item.NavigateUrl);
                array.Add(json);
            }
            return new Result(ResultStatus.Success, array);
        }
        #endregion


        #region 初始化数据
        [JsonRpcMethod("InitializationDataV2", Idempotent = true)]
        [JsonRpcHelp("初始化数据")]
        public JsonObject InitializationData()
        {
            JsonObject json;
            json = new JsonObject();
            json.Put("loginLogo", BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MBhop_Login_Logo"));
            json.Put("logo", BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_MBhop_Index_Logo"));
         
            return new Result(ResultStatus.Success, json);
        }
        #endregion

        #region   获取配置参数  (SAAS 接口都用)
        [JsonRpcMethod("GetConfigData", Idempotent = true)]
        [JsonRpcHelp("初始化数据")]
        public JsonObject GetConfigData(string enterpriserStr)
        {
            JsonObject json;
            json = new JsonObject();

            long enterpriseId = YSWL.Common.DEncrypt.DEncrypt.ConvertToNumber(enterpriserStr);
            if (enterpriseId == 0)
            {
                return new Result(ResultStatus.Failed, Result.FormatFailed("40", "获取初始化数据失败，请确实是否有企业标识信息"));
            }
            YSWL.Common.CallContextHelper.SetAutoTag(enterpriseId);
            json.Put("isOpenRegister", BLL.SysManage.ConfigSystem.GetBoolValueByCache("MBShop_IsOpenRegister"));
            json.Put("isOpenLogin", BLL.SysManage.ConfigSystem.GetBoolValueByCache("MBShop_IsOpenLogin"));
            json.Put("isOpenMultiDepot", YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot());
            return new Result(ResultStatus.Success, json);
        }
        #endregion 
    }
}