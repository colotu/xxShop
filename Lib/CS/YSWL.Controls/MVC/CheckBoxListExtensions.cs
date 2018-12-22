/**
* CheckBoxListExtensions.cs
*
* 功 能： 实现MVC版的CheckBoxList
* 类 名： CheckBoxListExtensions
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/9/28 14:28:04   Ben    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace YSWL.Controls.MVC
{
    public static class CheckBoxListExtensions
    {
        #region -- CheckBoxList (Horizontal) --

        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">SelectListItem.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
                                                 string name,
                                                 IEnumerable<SelectListItem> listInfo)
        {
            return htmlHelper.CheckBoxList(name, listInfo, (IDictionary<string, object>)null, 0);
        }

        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">SelectListItem.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
                                                 string name,
                                                 IEnumerable<SelectListItem> listInfo,
                                                 object htmlAttributes)
        {
            return htmlHelper.CheckBoxList(name, listInfo,
                                           (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes), 0);
        }

        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="number">每个Row的显示个数.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
                                                 string name,
                                                 IEnumerable<SelectListItem> listInfo,
                                                 IDictionary<string, object> htmlAttributes,
                                                 int number)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("No Tag Name", "name");
            }
            if (listInfo == null)
            {
                return null;
            }
            if (listInfo.Count() < 1)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;

            foreach (SelectListItem info in listInfo)
            {
                lineNumber++;

                TagBuilder builder = new TagBuilder("input");
                if (info.Selected)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);
                sb.Append(builder.ToString(TagRenderMode.Normal));

                TagBuilder labelBuilder = new TagBuilder("label");
                labelBuilder.MergeAttribute("for", name);
                labelBuilder.InnerHtml = info.Text;
                sb.Append(labelBuilder.ToString(TagRenderMode.Normal));

                if (number == 0 || (lineNumber % number == 0))
                {
                    sb.Append("<br />");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion

        #region -- CheckBoxListVertical --

        /// <summary>
        /// Checks the box list vertical.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxListVertical(this HtmlHelper htmlHelper,
                                                         string name,
                                                         IEnumerable<SelectListItem> listInfo,
                                                         IDictionary<string, object> htmlAttributes,
                                                         int columnNumber = 1)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("No Tag Name", "name");
            }
            if (listInfo == null)
            {
                return null;
            }
            if (listInfo.Count() < 1)
            {
                return null;
            }

            int dataCount = listInfo.Count();

            // calculate number of rows
            int rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(columnNumber)));
            if (dataCount <= columnNumber || dataCount - columnNumber == 1)
            {
                rows = dataCount;
            }

            TagBuilder wrapBuilder = new TagBuilder("div");
            wrapBuilder.MergeAttribute("style", "floatleft; light-height25px; padding-right5px;");

            string wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
            string wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag),
                                             " <div style=\"clear:both;\"></div>");
            string wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));

            StringBuilder sb = new StringBuilder();
            sb.Append(wrapStart);

            int lineNumber = 0;

            foreach (var info in listInfo)
            {
                TagBuilder builder = new TagBuilder("input");
                if (info.Selected)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);
                sb.Append(builder.ToString(TagRenderMode.Normal));

                TagBuilder labelBuilder = new TagBuilder("label");
                labelBuilder.MergeAttribute("for", name);
                labelBuilder.InnerHtml = info.Text;
                sb.Append(labelBuilder.ToString(TagRenderMode.Normal));

                lineNumber++;

                if (lineNumber.Equals(rows))
                {
                    sb.Append(wrapBreak);
                    lineNumber = 0;
                }
                else
                {
                    sb.Append("<br/>");
                }
            }
            sb.Append(wrapClose);
            return MvcHtmlString.Create(sb.ToString());
        }

        #endregion

        #region -- CheckBoxList (Horizonal, Vertical) --

        /// <summary>
        /// Checks the box list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="position">The position.</param>
        /// <param name="number">每行/每列个数</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
                                                 string name,
                                                 IEnumerable<SelectListItem> listInfo,
                                                 IDictionary<string, object> htmlAttributes,
                                                 Position position = Position.Horizontal,
                                                 int number = 0)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("No Tag Name", "name");
            }
            if (listInfo == null)
            {
                return null;
            }
            if (listInfo.Count() < 1)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;

            switch (position)
            {
                case Position.Horizontal:

                    foreach (SelectListItem info in listInfo)
                    {
                        lineNumber++;
                        sb.Append(CreateCheckBoxItem(info, name, htmlAttributes));

                        if (number == 0 || (lineNumber % number == 0))
                        {
                            sb.Append("<br />");
                        }
                    }
                    break;

                case Position.Vertical:

                    int dataCount = listInfo.Count();

                    // 计算最大显示列数(rows)
                    int rows = number > 0 ? Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(number))) : 0;
                    if (dataCount <= number || dataCount - number == 1)
                    {
                        rows = dataCount;
                    }

                    TagBuilder wrapBuilder = new TagBuilder("div");
                    wrapBuilder.MergeAttribute("style", "floatleft; light-height25px; padding-right5px;");

                    string wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
                    string wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag),
                                                     " <div style=\"clear:both;\"></div>");
                    string wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));

                    sb.Append(wrapStart);

                    foreach (SelectListItem info in listInfo)
                    {
                        lineNumber++;
                        sb.Append(CreateCheckBoxItem(info, name, htmlAttributes));

                        if (lineNumber.Equals(rows))
                        {
                            sb.Append(wrapBreak);
                            lineNumber = 0;
                        }
                        else
                        {
                            sb.Append("<br/>");
                        }
                    }
                    sb.Append(wrapClose);
                    break;
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        internal static string CreateCheckBoxItem(SelectListItem info, string name,
                                                  IDictionary<string, object> htmlAttributes)
        {
            StringBuilder sb = new StringBuilder();

            TagBuilder builder = new TagBuilder("input");
            if (info.Selected)
            {
                builder.MergeAttribute("checked", "checked");
            }
            builder.MergeAttributes<string, object>(htmlAttributes);
            builder.MergeAttribute("type", "checkbox");
            builder.MergeAttribute("value", info.Value);
            builder.MergeAttribute("name", name);
            sb.Append(builder.ToString(TagRenderMode.Normal));

            TagBuilder labelBuilder = new TagBuilder("label");
            labelBuilder.MergeAttribute("for", name);
            labelBuilder.InnerHtml = info.Text;
            sb.Append(labelBuilder.ToString(TagRenderMode.Normal));

            return sb.ToString();
        }

        #endregion
    }
}