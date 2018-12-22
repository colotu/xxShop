/**
* HtmlExtensions.cs
*
* 功 能： HtmlExtensions
* 类 名： HtmlExtensions
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/9/23 20:24:47   Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using YSWL.Components;
using YSWL.Web;

namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString YSWLSEO(this HtmlHelper htmlHelper, bool PoweredBy = true, string Title = null, string Keywords = null, string Description = null)
        {
            bool hasTitle = !string.IsNullOrWhiteSpace(Title);
            bool hasKeywords = !string.IsNullOrWhiteSpace(Keywords);
            bool hasDescription = !string.IsNullOrWhiteSpace(Description);

            Title += string.Empty;
            string newline = htmlHelper.ViewContext.HttpContext.Response.Output.NewLine + "        ";
            System.Text.StringBuilder baseHead = new System.Text.StringBuilder();
            baseHead.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />{0}", newline);

            if (!hasTitle && htmlHelper.ViewData.ContainsKey("Title"))
                Title = htmlHelper.ViewData["Title"] + Title;

            baseHead.AppendFormat("<title>{1}</title>{0}", newline, Title);
            
            if (!hasKeywords && htmlHelper.ViewData.ContainsKey("Keywords"))
                Keywords = htmlHelper.ViewData["Keywords"] + Keywords;

            if (!hasDescription && htmlHelper.ViewData.ContainsKey("Description"))
                Description = htmlHelper.ViewData["Description"] + Description;

            baseHead.AppendFormat("<meta name=\"keywords\" content=\"{1}\">{0}", newline, Keywords);
            baseHead.AppendFormat("<meta name=\"description\" content=\"{1}\">{0}", newline, Description);

            AreaRoute areaRoute = AreaRoute.None;
            if (htmlHelper.ViewContext.RouteData != null &&
                htmlHelper.ViewContext.RouteData.DataTokens != null &&
                htmlHelper.ViewContext.RouteData.DataTokens.ContainsKey("area"))
            {
                areaRoute = MvcApplication.GetCurrentAreaRoute(
                    htmlHelper.ViewContext.RouteData.DataTokens["area"]);
            }
            switch (areaRoute)
            {
                case AreaRoute.SNS:
                    //baseHead.AppendFormat(
                    //    "<link href=\"/Areas/SNS/Themes/" + ThemeName + "/Content/css/style.css\" rel=\"stylesheet\" type=\"text/css\" />");
                    break;
                case AreaRoute.CMS:
                    baseHead.AppendFormat(
                        "<link href=\"/Areas/CMS/Themes/" + MvcApplication.ThemeName + "/Content/Css/Site.css\" rel=\"stylesheet\" type=\"text/css\" />");
                    break;
                case AreaRoute.None:
                case AreaRoute.Shop:
                case AreaRoute.UserCenter:
                case AreaRoute.Tao:
                case AreaRoute.COM:
                default:
                    break;
            }
            return new MvcHtmlString(baseHead.ToString());
        }

    }
}
