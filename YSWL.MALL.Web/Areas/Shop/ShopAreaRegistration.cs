﻿/**
* ShopAreaRegistration.cs
*
* 功 能： Shop模块-区域路由注册器
* 类 名： ShopAreaRegistration
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/8/27 12:00:33   Ben    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System.Web.Mvc;
using YSWL.Web;

namespace YSWL.MALL.Web.Areas.Shop
{
    public class ShopAreaRegistration : AreaRegistrationBase
    {
        public ShopAreaRegistration()
            : base(AreaRoute.Shop)
        {
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            #region 注册Shop区域扩展路由
            

            #region 商品列表路由
            context.MapRoute(
                name: AreaName + "_ProductList",
                url: CurrentRoutePath + "Product/{cid}/{brandid}/{attrvalues}/{mod}/{price}",
                defaults:
                    new
                        {
                            controller = "Product",
                            action = "Index",
                            cid = 0,
                            brandid = 0,
                            attrvalues = "0",
                            mod = "default",
                            price = ""

                        },
                constraints: new
                    {
                        mod = @"default|hot|new|price|pricedesc", //大小写字母/数字/下划线
                        cid = @"[\d]{0,11}" //*表示数字长度11  //new { id = @"\d*" } 长度不限
                    }
                , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
                );
            #endregion

            #region 商品搜索列表路由
            context.MapRoute(
                name: AreaName + "_ProductSearch",
                url: CurrentRoutePath + "Search/{cid}/{brandid}/{mod}/{price}/{keyword}",
                defaults:
                    new
                    {
                        controller = "Search",
                        action = "Index",
                        cid = 0,
                        brandid = 0,
                        mod = "default",
                        price = "0-0",
                        keyword = "",

                    },
                constraints: new
                {
                    mod = @"default|hot|new|price|pricedesc", //大小写字母/数字/下划线
                    cid = @"[\d]{0,11}" //*表示数字长度11  //new { id = @"\d*" } 长度不限
                }
                , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
                );
            #endregion

            #region 商品详细页路由
            context.MapRoute(
             name: AreaName + "_ProductDetail12",
             url: CurrentRoutePath + "Product-{id}.html",
             defaults:
                 new
                 {
                     controller = "Product",
                     action = "Detail",
                     id = 0

                 },
             constraints: new
             {
                 id = @"[\d]{0,11}"
             }
             , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
             );
            #endregion

            #region 店铺首页路由
            context.MapRoute(
                name: AreaName + "_Store_Index",
                url: CurrentRoutePath + "Store/{suppId}/{cid}/{mod}/{price}/{ky}",
                defaults:
                    new
                    {
                        controller = "Store",
                        action = "Index",
                        suppId = 0,
                        cid = 0,
                        mod = "hot",
                        price = "0-0",
                        ky = ""
                    },
                constraints: new
                {
                    suppId = @"[\d]{0,11}",//*表示数字长度11  //new { id = @"\d*" } 长度不限
                    mod = @"hot|new|pricedesc", //大小写字母/数字/下划线
                    cid = @"[\d]{0,11}" //*表示数字长度11  //new { id = @"\d*" } 长度不限
                }
                , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
                );
            #endregion

            #region 商家商品列表路由
            context.MapRoute(
                name: AreaName + "_SuppProductList",
                url: CurrentRoutePath + "Store/list/{suppId}/{cid}/{mod}/{price}/{ky}",
                defaults:
                    new
                    {
                        controller = "Store",
                        action = "List",
                        suppId = 0,
                        cid = 0,
                        mod = "hot",
                        price = "0-0",
                        ky = ""
                    },
                constraints: new
                {
                    suppId = @"[\d]{0,11}",
                    mod = @"hot|new|pricedesc", //大小写字母/数字/下划线
                    cid = @"[\d]{0,11}" //*表示数字长度11  //new { id = @"\d*" } 长度不限
                }
                , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
                );
            #endregion

            #region 商品对比
            context.MapRoute(
                        name: AreaName + "_Product_Compare",
                        url: CurrentRoutePath + "Product/Compare/{type}/{prodidlist}",
                        defaults: new
                            {
                                controller = "Product",
                                action = "Compare",
                                prodidlist = "",
                                type =0 //*表示数字长度11  //new { id = @"\d*" } 长度不限
                            },
                         constraints: new
                {
                    type = @"[\d]{0,11}" //*表示数字长度11  //new { id = @"\d*" } 长度不限
                }
                        , namespaces: new[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
                        );
            #endregion

            #region 团购路由
            context.MapRoute(
                name: AreaName + "_GroupBuy",
                url: CurrentRoutePath + "ProSales/GroupBuy/{regionid}/{cid}/{mod}",
                defaults:
                    new
                    {
                        controller = "ProSales",
                        action = "GroupBuy",
                        regionid = 0,//BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_GroupBuy_DefaultRegion"),
                        cid = 0,
                        mod ="default"
                    },
                constraints: new
                {
                    mod = @"default|hot|new|price|pricedesc", //大小写字母/数字/下划线
                    cid=@"[\d]{0,11}",
                    regionid = @"[\d]{0,11}"
                }
                , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
                );

            context.MapRoute(
              name: AreaName + "_GetArea",
              url: CurrentRoutePath + "ProSales/GetArea/{regionId}",
              defaults:
                  new
                  {
                      controller = "ProSales",
                      action = "GetArea",
                      regionId = 0//BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_GroupBuy_DefaultRegion")
                  },
              constraints: new
              {
                  regionId = @"[\d]{0,11}"
              }
              , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
              );
            #endregion

            #region 退单详细页路由
            context.MapRoute(
             name: AreaName + "_ReturnInfo",
             url: CurrentRoutePath + "ReturnOrder/ReturnInfo/{retrunId}",
             defaults:
                 new
                 {
                     controller = "ReturnOrder",
                     action = "ReturnInfo",
                     retrunId = 0
                 },
             constraints: new
             {
                 id = @"[\d]{0,11}"
             }
             , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
             );
            #endregion

            #region  推广链接地址路由
            context.MapRoute(
              name: AreaName + "_CommissionPro",
              url: CurrentRoutePath + "UserCenter/CommissionPro/{cid}/{name}",
              defaults:
                  new
                  {
                      controller = "UserCenter",
                      action = "CommissionPro",
                      cid = 0,
                      name = "",
                  },
              constraints: new
              {
                  cid = @"[\d]{0,11}" //*表示数字长度11  //new { id = @"\d*" } 长度不限
              }
              , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
              );

            context.MapRoute(
              name: AreaName + "_CommissionPro_PromoList",
              url: CurrentRoutePath + "UserCenter/PromoList/{cid}/{name}",
              defaults:
                  new
                  {
                      controller = "UserCenter",
                      action = "PromoList",
                      cid = 0,
                      name = "",
                  },
              constraints: new
              {
                  cid = @"[\d]{0,11}" //*表示数字长度11  //new { id = @"\d*" } 长度不限
              }
              , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
              );
            #endregion

            #region 推荐商品列表路由
            context.MapRoute(
                name: AreaName + "_" + CustomAreaName + "_RecommendIndex",
                url: CurrentRoutePath + "rec/{type}/{cid}/{pageIndex}/{pageSize}/{vName}/{ajaxVName}",
                defaults:
                    new
                    {
                        controller = "Recommend",
                        action = "Index",
                        type = "rec",
                        cid = 0,
                        pageIndex = 1,
                        pageSize = 32,
                        vName = "Index",
                        ajaxVName = "_ProductList"
                    },
                constraints: new
                {
                    type = @"rec|hot|cheap|new|irec", //大小写字母/数字/下划线
                    cid = @"[\d]{0,11}" //*表示数字长度11  //new { id = @"\d*" } 长度不限
                }
                , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
                );
            #endregion

            #region 团购路由
            context.MapRoute(
                name: AreaName + "_Group",
                url: CurrentRoutePath + "ProSales/Group/{rid}/{cid}/{mod}/{pageIndex}/{pageSize}",
                defaults:
                    new
                    {
                        controller = "ProSales",
                        action = "Group",
                        rid = 0,
                        cid = 0,
                        mod = "default",
                        pageIndex=1,
                        pageSize=30
                    },
                constraints: new
                {
                    mod = @"default|hot|new|price|pricedesc", //大小写字母/数字/下划线
                    cid = @"[\d]{0,11}",
                    regionid = @"[\d]{0,11}"
                }
                , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
                );
            #endregion



            #region 店铺搜索列表路由
            context.MapRoute(
                name: AreaName + "_StoreListSearch",
                url: CurrentRoutePath + "Search/StoreList/{keyword}",
                defaults:
                    new
                    {
                        controller = "Search",
                        action = "StoreList",
                        keyword = ""
                    }
                , namespaces: new string[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", AreaName) }
                );
            #endregion

            #endregion
            base.RegisterArea(context);
        }
    }
}
