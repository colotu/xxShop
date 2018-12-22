/**
* ThemeViewEngine.cs
*
* 功 能： 模版引擎
* 类 名： ThemeViewEngine
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V1.00  2013/02/22 14:44:17  qihq    初版
* V1.05  2013/03/08 18:19:59  Ben     修复根目录的默认路由
* V1.20  2013/05/14 11:34:56  Ben     启用Shop区域, 并对各区域追加缓存优化
* V2.00  2013/05/16 21:53:50  Ben     重构模版引擎, 发布第二代模版引擎
* V3.00  2013/11/21 21:35:10  Ben     重构/新增子引擎模块, 发布第三代模版引擎
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Linq;
using System.Web.Mvc;
using YSWL.Components;
using YSWL.Web;
using System.Collections.Generic;

namespace YSWL.ViewEngine
{
    /// <summary>
    /// 模版引擎
    /// </summary>
    /// <remarks>仅Area使用模版引擎</remarks>
    public class ThemeViewEngine : RazorViewEngine
    {
        #region AreaLocationFormat
        protected const string BaseAreaViewLocationFormat = "~/Areas/{{2}}/Themes/{0}/Views/{{1}}/{{0}}.cshtml";
        protected const string BaseAreaMasterLocationFormat = "~/Areas/{{2}}/Themes/{0}/Views/Shared/{{0}}.cshtml";
        #endregion

        /// <summary>
        /// 主模版名称
        /// </summary>
        protected readonly string MainThemeName;

        /// <summary>
        /// 子模版引擎
        /// </summary>
        protected ISubAreaViewEngine SubAreaViewEngine;

        /// <summary>
        /// 构造模版引擎
        /// </summary>
        public ThemeViewEngine(string themeName, ISubAreaViewEngine subAreaViewEngine = null)
            : base()
        {
            MainThemeName = themeName;
            string areaViewLocationFormat = string.Format(BaseAreaViewLocationFormat, MainThemeName);
            string areaMasterLocationFormat = string.Format(BaseAreaMasterLocationFormat, MainThemeName);
            AreaViewLocationFormats = new string[]
                {
                    areaViewLocationFormat,
                    areaMasterLocationFormat
                };
            AreaMasterLocationFormats = new string[]
                {
                    areaViewLocationFormat,
                    areaMasterLocationFormat
                };
            AreaPartialViewLocationFormats = new string[]
                {
                    areaViewLocationFormat,
                    areaMasterLocationFormat
                };

            string viewLocationFormat = "~/Views/{1}/{0}.cshtml";
            string masterLocationFormat = "~/Views/Shared/{0}.cshtml";
            ViewLocationFormats = new string[]
                {
                    viewLocationFormat,
                    masterLocationFormat
                };
            MasterLocationFormats = new string[]
                {
                    viewLocationFormat,
                    masterLocationFormat
                };
            PartialViewLocationFormats = new string[]
                {
                    viewLocationFormat,
                    masterLocationFormat
                };
            FileExtensions = new string[] { "cshtml" };

            //默认引擎
            if (subAreaViewEngine == null)
            {
                subAreaViewEngine = new DefaultSubAreaViewEngine();
            }
            //挂载子引擎
            subAreaViewEngine.SubEngineMap = subAreaViewEngine.SubEngineMap ??
                    new Dictionary<AreaRoute, Func<ControllerContext, string, string, string[]>>
                    {
                        {AreaRoute.None, (context, baseAreaViewLocationFormat, tmpTag) => ViewLocationFormats}
                    };
            SubAreaViewEngine = subAreaViewEngine;
        }

        #region FindView
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName,
                                                  string masterName, bool useCache)
        {
            if (controllerContext == null) throw new ArgumentNullException("controllerContext");
            
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException("FindView ArgumentException: viewName must be specified.", "viewName");

            AreaRoute areaRoute = MvcApplication.GetCurrentAreaRoute(controllerContext);

            return FindView(areaRoute, controllerContext, viewName, useCache, false, masterName);
        }
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (controllerContext == null) throw new ArgumentNullException("controllerContext");
            if (string.IsNullOrEmpty(partialViewName))
                throw new ArgumentException("partialViewName must be specified.", "partialViewName");

            AreaRoute areaRoute = MvcApplication.GetCurrentAreaRoute(controllerContext);
            return FindView(areaRoute, controllerContext, partialViewName, useCache, true);
        }

        private ViewEngineResult FindView(AreaRoute areaRoute, ControllerContext controllerContext, string viewName,
             bool useCache, bool isPartialView, string masterName = null)
        {
            // Get the name of the controller from the path
            string controller = controllerContext.RouteData.Values["controller"].ToString();
            string controllerspaces = controllerContext.RouteData.DataTokens["Space"] as string;
            string viewSpaces = string.Empty;

            //检测命名空间, 识别第二代MVC结构
            //if (namespaces != null && namespaces.Length > 0)
            //{
            //    foreach (string ns in namespaces)
            //    {
            //        if (!ns.EndsWith(".*") && !ns.EndsWith(".Controllers"))
            //        {
            //            controllerFolder = ns.Substring(ns.LastIndexOf('.') + 1);
            //            break;
            //        }
            //    }
            //}
            if (!string.IsNullOrWhiteSpace(controllerspaces))
            {
                viewSpaces = controllerspaces;
            }

            string area = areaRoute.ToString();

            //DONE: 非空时标识使用自定义引擎 子引擎的缓存差异标识 注册子引擎路由时追加, 此参数在回调时回传
            string tmpTag = SubAreaViewEngine.SubEngineMap.ContainsKey(areaRoute)
                ? SubAreaViewEngine.GetTagString(areaRoute, controllerContext)
                : string.Empty;

            // Create the key for caching purposes
            string keyPath = System.IO.Path.Combine(area, controller, MainThemeName, viewSpaces, viewName, tmpTag);

            // Try the cache
            if (useCache)
            {
                //If using the cache, check to see if the location is cached.
                string cacheLocation = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, keyPath);
                if (!string.IsNullOrWhiteSpace(cacheLocation))
                {
                    return isPartialView
                        ? new ViewEngineResult(CreatePartialView(controllerContext, cacheLocation), this)
                        : new ViewEngineResult(CreateView(controllerContext, cacheLocation, masterName), this);
                }
            }

            // Remember the attempted paths, if not found display the attempted paths in the error message.
            var attempts = new List<string>();

            // Check ViewName 是否是具体路径 具体路径时, 不再以FormatViewPath方式查找
            bool isSpecificPath = viewName.StartsWith("~") || viewName.StartsWith("/");
            if (isSpecificPath)
            {
                if (FileExists(controllerContext, viewName))
                {
                    ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, keyPath, viewName);

                    return isPartialView
                        ? new ViewEngineResult(CreatePartialView(controllerContext, viewName), this)
                        : new ViewEngineResult(CreateView(controllerContext, viewName, masterName), this);
                }

                // If not found, add to the list of attempts.
                attempts.Add(viewName);
            }
            else
            {
                string[] locationFormats;
                if (areaRoute == AreaRoute.None)
                {
                    locationFormats = ViewLocationFormats;
                }
                else if (SubAreaViewEngine.SubEngineMap.ContainsKey(areaRoute) &&
                        !string.IsNullOrWhiteSpace(tmpTag))
                {
                    locationFormats = SubAreaViewEngine.SubEngineMap[areaRoute](controllerContext,
                        BaseAreaViewLocationFormat, tmpTag);
                }
                else if (SubAreaViewEngine.SubEngineMap.ContainsKey(areaRoute))
                {
                    locationFormats = SubAreaViewEngine.SubEngineMap[areaRoute](controllerContext,
                        BaseAreaViewLocationFormat, string.Empty);
                }
                else
                {
                    locationFormats = AreaViewLocationFormats;
                }

                // for each of the paths defined, format the string and see if that path exists. When found, cache it.
                foreach (string rootPath in locationFormats)
                {
                    string currentPath;
                    if (areaRoute == AreaRoute.None)
                    {
                        //默认模版路径
                        currentPath = string.Format(rootPath, viewName, controller);
                    }
                    else if (string.IsNullOrWhiteSpace(viewSpaces))
                    {
                        //第一代MVC(默认)路径
                        currentPath = string.Format(rootPath, viewName, controller, area);
                    }
                    else
                    {
                        //第二代MVC路径
                        currentPath = string.Format(rootPath, controller + "/" + viewName, viewSpaces, area);
                    }

                    if (FileExists(controllerContext, currentPath))
                    {
                        ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, keyPath, currentPath);

                        return isPartialView
                            ? new ViewEngineResult(CreatePartialView(controllerContext, currentPath), this)
                            : new ViewEngineResult(CreateView(controllerContext, currentPath, masterName), this);
                    }

                    // If not found, add to the list of attempts.
                    attempts.Add(currentPath);
                }
            }

            // if not found by now, simply return the attempted paths.
            return new ViewEngineResult(attempts.Distinct().ToList());
        }
        #endregion

        #region 版本保护

        protected void UseAction(ControllerContext controllerContext,
            Action<ControllerContext, AreaRoute> action)
        {
            AreaRoute areaRoute = AreaRoute.None;
            if (controllerContext.RouteData != null && controllerContext.RouteData.DataTokens != null &&
                controllerContext.RouteData.DataTokens.ContainsKey("area"))
            {
                areaRoute = MvcApplication.GetCurrentAreaRoute(
                    controllerContext.RouteData.DataTokens["area"]);
            }
            //TODO: 暂时仅对CMS/SNS/Shop/MShop进行版本保护 BEN ADD 20130312
            switch (areaRoute)
            {
                case AreaRoute.SNS:
                case AreaRoute.CMS:
                case AreaRoute.Shop:
                case AreaRoute.MShop:
                    action(controllerContext, areaRoute);
                    break;
                case AreaRoute.None:
                case AreaRoute.UserCenter:
                case AreaRoute.Tao:
                case AreaRoute.COM:
                default:
                    break;
            }
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            if (!controllerContext.IsChildAction ||
                controllerContext.RouteData.Values == null ||
                controllerContext.RouteData.DataTokens == null)
                return base.CreateView(controllerContext, viewPath, masterPath);

            UseAction(controllerContext, Analytic.CreateBegin);

            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            if (!controllerContext.IsChildAction ||
                controllerContext.RouteData.Values == null ||
                controllerContext.RouteData.DataTokens == null)
                return base.CreatePartialView(controllerContext, partialPath);

            UseAction(controllerContext, Analytic.CreateBegin);

            return base.CreatePartialView(controllerContext, partialPath);
        }

        public override void ReleaseView(ControllerContext controllerContext, IView view)
        {
            if (!controllerContext.IsChildAction ||
                controllerContext.RouteData.Values == null ||
                controllerContext.RouteData.DataTokens == null)
            {
                base.ReleaseView(controllerContext, view);
                return;
            }

            UseAction(controllerContext, Analytic.CreateEnd);

            base.ReleaseView(controllerContext, view);
        }
        #endregion
    }

    #region 子区域模版引擎

    #region DefaultSubAreaViewEngine
    /// <summary>
    /// 默认子模版引擎
    /// </summary>
    public class DefaultSubAreaViewEngine : ISubAreaViewEngine
    {
        #region ISubAreaViewEngine 成员

        public Dictionary<AreaRoute, Func<ControllerContext, string, string, string[]>> SubEngineMap { get; set; }

        public string GetTagString(AreaRoute areaRoute, ControllerContext controllerContext)
        {
            return string.Empty;
        }

        #endregion
    }
    #endregion

    #region ISubAreaViewEngine
    /// <summary>
    /// 子区域模版引擎
    /// </summary>
    public interface ISubAreaViewEngine
    {
        /// <summary>
        /// 子引擎Map
        /// </summary>
        /// <remarks>
        /// key:AreaRoute
        /// value: 实际路径 Func(容器, 模版原始format)
        /// </remarks>
        Dictionary<AreaRoute, Func<ControllerContext, string, string, string[]>> SubEngineMap { get; set; }

        /// <summary>
        /// 获取标识内容
        /// </summary>
        string GetTagString(AreaRoute areaRoute, ControllerContext controllerContext);
    }
    #endregion

    #endregion


}
