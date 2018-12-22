/**
* Authorize.cs
*
* 功 能： 授权
* 类 名： Authorize
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/4/27 21:56:59  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Web.Mvc;
using YSWL.Components;
using YSWL.Web;

namespace YSWL.ViewEngine
{
    internal static class Analytic
    {

        internal static void CreateBegin(ControllerContext controllerContext, AreaRoute areaRoute)
        {
            string action = controllerContext.RouteData.Values["action"] as string;
            switch (action)
            {
                case "Footer":
                    switch (areaRoute)
                    {
                        case AreaRoute.SNS:
                        case AreaRoute.CMS:
                            controllerContext.HttpContext.Response.Output.WriteLine("<div id='footer' class='footer'>");
                            break;
                        case AreaRoute.Shop:
                            controllerContext.HttpContext.Response.Output.WriteLine("<div id='ft' >");
                            break;
                        case AreaRoute.MShop:
                            controllerContext.HttpContext.Response.Output.WriteLine("<div id='footer' class='footer'>");
                            break;
                    }
                    break;
            }
        }

        private static string _productInfo;
        internal static void CreateEnd(ControllerContext controllerContext, AreaRoute areaRoute)
        {
            string action = controllerContext.RouteData.Values["action"] as string;
            switch (action)
            {
                case "Footer":
                    if (string.IsNullOrEmpty(_productInfo))
                    {
                        _productInfo = MvcApplication.ProductInfo;
                    }
                    switch (areaRoute)
                    {
                        case AreaRoute.CMS:
                            controllerContext.HttpContext.Response.Output.WriteLine(
                                     "<div class=\"bot_menu\">{0}<br/>{1}</div>",
                                     string.Format("{0} {1}", MvcApplication.WebPowerBy, MvcApplication.WebRecord), MvcApplication.PageFootJs);
                            break;
                        case AreaRoute.SNS:
                            controllerContext.HttpContext.Response.Output.WriteLine(
                                    "<div class='clear'></div><div class='footer_bot' style='margin-top: -23px;margin-bottom: 23px'>{0}<br/>{1}<div class='clear'></div></div></div>",
                                    string.Format("{0}<br/>{1}", MvcApplication.WebPowerBy, MvcApplication.WebRecord), MvcApplication.PageFootJs);
                            break;
                        case AreaRoute.Shop:
                            controllerContext.HttpContext.Response.Output.WriteLine(
                                    "<div class='copyright'>{0}<br/>{1}</div>",
                                    string.Format("<p> <span class='mr15'>{0}</span> <span>{1}</span></p>", MvcApplication.WebPowerBy, MvcApplication.WebRecord), MvcApplication.PageFootJs);
                            break;
                        case AreaRoute.MShop:
                            controllerContext.HttpContext.Response.Output.WriteLine(
                                "<div class='copyright'>{0}{1}</div>",
                                (string.IsNullOrWhiteSpace(MvcApplication.WebPowerBy)
                                        ? string.Empty
                                        : string.Format("<p> <span class='mr15'>{0}</span> </p>",
                                            MvcApplication.WebPowerBy)), MvcApplication.PageFootJs);
                            break;
                    }
                    break;
            }
        }
    }
}
