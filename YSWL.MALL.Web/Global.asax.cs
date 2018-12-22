/**
* MvcApplication.cs
*
* 功 能： Global
* 类 名： MvcApplication
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

using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using YSWL.Components;
using YSWL.MALL.Model.SysManage;
using YSWL.ViewEngine;
using YSWL.MALL.BLL.SysManage;
using System.Collections.Generic;
using System;
using YSWL.Web;

namespace YSWL.MALL.Web
{ 
    #region IApplicationOption

    public class ApplicationOption : IApplicationOption
    {
        private static readonly WebSiteSet WebSiteSet = new WebSiteSet(ApplicationKeyType.System);
        #region IApplicationOption 成员

        public string GetAreaThemeName(AreaRoute areaRoute, ControllerContext context = null)
        {
            //  if (isAutoConn) return "Y1";
            switch (areaRoute)
            {
                case AreaRoute.CMS:
                    string cmstheme = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("CMS_Theme");
                    return string.IsNullOrWhiteSpace(cmstheme) ? "Default" : cmstheme;
                case AreaRoute.Shop:
                    string shoptheme = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_Theme");//
                    return string.IsNullOrWhiteSpace(shoptheme) ? MvcApplication.ThemeName : shoptheme;
                case AreaRoute.SNS:
                    string snstheme = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("SNS_Theme");
                    return string.IsNullOrWhiteSpace(snstheme) ? "M1" : snstheme;
                case AreaRoute.MPage:
                    string mpageTheme = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("MPage_Theme");
                    return string.IsNullOrWhiteSpace(mpageTheme) ? "Wap21" : mpageTheme;
                case AreaRoute.Supplier:
                    string supTheme = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Supplier_Theme");
                    return string.IsNullOrWhiteSpace(supTheme) ? "M1" : supTheme;
                case AreaRoute.MShop:
                    string mshopTheme = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("MShop_Theme");
                    return string.IsNullOrWhiteSpace(mshopTheme) ? "MC01" : mshopTheme;
                case AreaRoute.MSNS:
                    string msnstheme = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("MSNS_Theme");
                    return string.IsNullOrWhiteSpace(msnstheme) ? "VSNS" : msnstheme;
                case AreaRoute.BShop:
                    string btheme = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("BShop_Theme");
                    return string.IsNullOrWhiteSpace(btheme) ? "PB01" : btheme;
                default:
                    return MvcApplication.ThemeName;
            }
        }

        public string PageFootJs
        {
            get
            {
                return WebSiteSet.PageFootJs;
            }
        }

        public string SiteName
        {
            get { return  WebSiteSet.WebName; }
        }

        public string ThemeName
        {
            get
            {
                return  ConfigSystem.GetValueByCache("ThemeCurrent");
            }
        }

        public string WebPowerBy
        {
            get { return WebSiteSet.WebPowerBy; }
        }

        public string WebRecord
        {
            get { return WebSiteSet.WebRecord; }
        }

        #endregion
    }

    #endregion

    #region 子区域模版引擎

    public class SubAreaViewEngine : ISubAreaViewEngine
    {
        ApplicationOption applicationOption = new ApplicationOption();
        #region SubAreaViewEngine
        //TODO: 使用KeyValuePar将ky改造成ko对象集合 BEN ADD 20131122
        public SubAreaViewEngine()
        {
            _subEngineMap.Add(AreaRoute.COM,
                (context, baseLocationFormat, themeName) => new string[]
                {
                    string.Format(baseLocationFormat, "Default"),
                    string.Format(baseLocationFormat, "Default").Replace("{1}", "Shared")
                });
            _subEngineMap.Add(AreaRoute.CMS,
                (context, baseLocationFormat, themeName) => new string[]
                {
                    string.Format(baseLocationFormat, themeName),
                    string.Format(baseLocationFormat, themeName).Replace("{1}", "Shared")
                });
            _subEngineMap.Add(AreaRoute.SNS,
                (context, baseLocationFormat, themeName) => new string[]
                {
                    string.Format(baseLocationFormat, themeName),
                    string.Format(baseLocationFormat,  themeName).Replace("{1}", "Shared")
                });
            _subEngineMap.Add(AreaRoute.Shop,
                (context, baseLocationFormat, themeName) => new string[]
                {
                    string.Format(baseLocationFormat, themeName),
                    string.Format(baseLocationFormat, themeName).Replace("{1}", "Shared")
                });
            _subEngineMap.Add(AreaRoute.MShop,
                (context, baseLocationFormat, themeName) => new string[]
                {
                    string.Format(baseLocationFormat, themeName),
                    string.Format(baseLocationFormat, themeName).Replace("{1}", "Shared")
                });
            _subEngineMap.Add(AreaRoute.MPage,
                (context, baseLocationFormat, themeName) => new string[]
                {
                    string.Format(baseLocationFormat, themeName),
                    string.Format(baseLocationFormat, themeName).Replace("{1}", "Shared")
                });
            _subEngineMap.Add(AreaRoute.Supplier,
                (context, baseLocationFormat, themeName) => new string[]
                {
                    string.Format(baseLocationFormat, "M1"),
                    string.Format(baseLocationFormat, "M1").Replace("{1}", "Shared")
                });
            _subEngineMap.Add(AreaRoute.MSNS,
                (context, baseLocationFormat, themeName) => new string[]
                {
                    string.Format(baseLocationFormat, "VSNS"),
                    string.Format(baseLocationFormat, "VSNS").Replace("{1}", "Shared")
                });
            _subEngineMap.Add(AreaRoute.MBShop,
                (context, baseLocationFormat, themeName) => new string[]
                {
                    string.Format(baseLocationFormat, themeName),
                    string.Format(baseLocationFormat, themeName).Replace("{1}", "Shared")
                });
            _subEngineMap.Add(AreaRoute.MB,
          (context, baseLocationFormat, themeName) => new string[]
          {
                    string.Format(baseLocationFormat, themeName),
                    string.Format(baseLocationFormat, themeName).Replace("{1}", "Shared")
          });
            _subEngineMap.Add(AreaRoute.BShop,
               (context, baseLocationFormat, themeName) => new string[]
               {
                    string.Format(baseLocationFormat, themeName),
                    string.Format(baseLocationFormat, themeName).Replace("{1}", "Shared")
               });
        }

        #endregion

        #region ISubAreaViewEngine 成员

        private Dictionary<AreaRoute, Func<ControllerContext, string, string, string[]>> _subEngineMap =
            new Dictionary<AreaRoute, Func<ControllerContext, string, string, string[]>>();
        public Dictionary<AreaRoute, Func<ControllerContext, string, string, string[]>> SubEngineMap
        {
            get { return _subEngineMap; }
            set { _subEngineMap = value; }
        }

        public string GetTagString(AreaRoute areaRoute, ControllerContext context)
        {
            if (areaRoute != AreaRoute.CMS && areaRoute != AreaRoute.SNS && areaRoute != AreaRoute.MShop &&
               areaRoute != AreaRoute.MPage && areaRoute != AreaRoute.Supplier && areaRoute != AreaRoute.Shop && areaRoute != AreaRoute.MB && areaRoute != AreaRoute.MBShop)
                 return string.Empty;

            return applicationOption.GetAreaThemeName(areaRoute);
        }

        #endregion
    }

    public class WeChatTheme
    {
        public string OpenId { set; get; }
        public string ThemeName { set; get; }
        public int targetId { get; set; }
        public string UserType { get; set; }
    }
    #endregion

    public class MvcApplication : YSWL.Components.MvcApplication
    {
        #region 构造
        /// <summary>
        /// 静态构造
        /// </summary>
        static MvcApplication()
        {
            ApplicationOption = new ApplicationOption();
        }
        /// <summary>
        /// 动态构造
        /// </summary>
        public MvcApplication()
        {
            SubAreaViewEngine = new SubAreaViewEngine();
        }
        #endregion

        #region 子区域模版引擎

        /// <summary>
        /// 子模版引擎
        /// </summary>
        protected static ISubAreaViewEngine SubAreaViewEngine;

        #endregion

        #region 注册忽略路由
        /// <summary>
        /// 注册忽略路由
        /// </summary>
        /// <remarks>
        /// 最高优先级
        /// </remarks>
        public override void RegisterIgnoreRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("pay/{*pathInfo}");
            routes.IgnoreRoute("wechat/{*pathInfo}");
            routes.IgnoreRoute("api/{*pathInfo}");
            routes.IgnoreRoute("tools/{*pathInfo}");
            base.RegisterIgnoreRoutes(routes);
        }
        #endregion


        #region 注册主路由
        /// <summary>
        /// 注册主路由
        /// </summary>
        /// <remarks>最低优先级</remarks>
        public override void RegisterRoutes(RouteCollection routes)
        {
            switch (MainAreaRoute)
            {
                //默认路由
                case AreaRoute.None:
                    routes.MapRoute(
                        name: "Default", // 路由名称
                        url: "{controller}/{action}/{id}", // 带有参数的 URL
                        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
                        , namespaces: new string[] { "YSWL.MALL.Web.Controllers" }
                        ).DataTokens.Add("area", "None");
                    break;
                //区域主路由
                default:
                    string area = MainAreaRoute.ToString();

                    routes.MapRoute(
                        name: "Default",
                        url: "{controller}/{action}/{viewname}/{id}",
                        defaults: new
                        {
                            area = area,
                            controller = "Home",
                            action = "Index",
                            viewname = UrlParameter.Optional,
                            id = UrlParameter.Optional
                        },
                        constraints: new
                        {
                            viewname = @"^[A-Za-z_]+${0,50}", //大小写字母/下划线
                            id = @"[\d]{0,11}" //*表示数字长度11  //new { id = @"\d*" } 长度不限
                        }
                        , namespaces: new[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", area) }
                        ).DataTokens.Add("area", area);

                    routes.MapRoute(
                        name: "Default_Base",
                        url: "{controller}/{action}/{id}",
                        defaults: new { area = area, controller = "Home", action = "Index", id = UrlParameter.Optional }
                        , namespaces: new[] { string.Format("YSWL.MALL.Web.Areas.{0}.Controllers", area) }
                        ).DataTokens.Add("area", area);
                    break;
            }
        }
        #endregion


        protected override void ApplicationStart()
        {
            #region 获取程序集版本号
            Version assemblyVersion = AssemblyVersion;
            Version = assemblyVersion.Major + "." + assemblyVersion.Minor +
                      (assemblyVersion.Build > 0 ? "." + assemblyVersion.Build : string.Empty);
            #endregion

            #region 获取产品信息
            ProductInfo = AssemblyProduct;
            #endregion

            #region 注册模版引擎
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemeViewEngine(ThemeName, SubAreaViewEngine));
            #endregion
        }

        #region 获取产品信息
        public static Version AssemblyVersion
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            }
        }
        public static string AssemblyDescription
        {
            get
            {
                var descriptionAttribute = System.Reflection.Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(System.Reflection.AssemblyDescriptionAttribute), false)
                    .OfType<System.Reflection.AssemblyDescriptionAttribute>()
                    .FirstOrDefault();
                if (descriptionAttribute != null)
                    return descriptionAttribute.Description;
                return string.Empty;
            }
        }
        public static string AssemblyProduct
        {
            get
            {
                // 获取此程序集上的所有 Product 属性
                object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyProductAttribute), false);
                // 如果 Product 属性不存在，则返回一个空字符串
                if (attributes.Length == 0)
                    return string.Empty;
                // 如果有 Product 属性，则返回该属性的值
                return ((System.Reflection.AssemblyProductAttribute)attributes[0]).Product;
            }
        }
        #endregion

        #region 获取主区域
        protected override string MainArea
        {
            get

            {
                return ConfigSystem.GetValueByCache("MainArea");
            }
        }
        #endregion


        public static string MShopThemeColor
        {
            get
            {
                string themeColor = ConfigSystem.GetValueByCache("MShop_Theme_Color");
                return String.IsNullOrWhiteSpace(themeColor) ? "green" : themeColor;
            }
        }
        public static string MBThemeColor
        {
            get
            {
                string themeColor = ConfigSystem.GetValueByCache("MB_Theme_Color");
                return String.IsNullOrWhiteSpace(themeColor) ? "green" : themeColor;
            }
        }
        public static string ShopThemeColor
        {
            get
            {
                string themeColor = ConfigSystem.GetValueByCache("Shop_Theme_Color");
                return String.IsNullOrWhiteSpace(themeColor) ? "green" : themeColor;
            }
        }
    }
}