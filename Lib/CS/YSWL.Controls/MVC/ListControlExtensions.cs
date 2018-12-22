
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public static class ListControlExtensions
    {
        #region 方案一
        //    public static MvcHtmlString CheckBoxList(this HtmlHelper helper,
        //string name, IEnumerable<SelectListItem> items, object htmlAttributes)
        //    {
        //        var str = new StringBuilder();
        //        str.AppendFormat("<div {0}>", htmlAttributes);

        //        foreach (var item in items)
        //        {
        //            str.Append("<div><input type=\"checkbox\" name=\"");
        //            str.Append(name);
        //            str.Append("\" value=\"");
        //            str.Append(item.Value);
        //            str.Append("\"");

        //            if (item.Selected)
        //                str.Append(" checked=\"chekced\"");

        //            str.Append(" />");
        //            str.Append(item.Text);
        //            str.Append("</div>");
        //        }

        //        str.Append("</div>");

        //        return MvcHtmlString.Create(str.ToString());
        //    }

        #endregion

        #region 方案二
        //// Methods
        //public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name)
        //{
        //    return htmlHelper.CheckBox(name, null);
        //}

        //public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked)
        //{
        //    return htmlHelper.CheckBox(name, isChecked, null);
        //}

        //public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes)
        //{
        //    return CheckBoxHelper(htmlHelper, null, name, null, htmlAttributes);
        //}

        //public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        //{
        //    return htmlHelper.CheckBox(name, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        //}

        //public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, IDictionary<string, object> htmlAttributes)
        //{
        //    return CheckBoxHelper(htmlHelper, null, name, new bool?(isChecked), htmlAttributes);
        //}

        //public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, object htmlAttributes)
        //{
        //    return htmlHelper.CheckBox(name, isChecked, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        //}

        //public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression)
        //{
        //    return htmlHelper.CheckBoxFor<TModel>(expression, ((IDictionary<string, object>)null));
        //}

        //public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, IDictionary<string, object> htmlAttributes)
        //{
        //    bool flag;
        //    if (expression == null)
        //    {
        //        throw new ArgumentNullException("expression");
        //    }
        //    ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, bool>(expression, htmlHelper.ViewData);
        //    bool? isChecked = null;
        //    if ((metadata.Model != null) && bool.TryParse(metadata.Model.ToString(), out flag))
        //    {
        //        isChecked = new bool?(flag);
        //    }
        //    return CheckBoxHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), isChecked, htmlAttributes);
        //}

        //public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlAttributes)
        //{
        //    return htmlHelper.CheckBoxFor<TModel>(expression, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        //}

        //private static MvcHtmlString CheckBoxHelper(HtmlHelper htmlHelper, ModelMetadata metadata, string name, bool? isChecked, IDictionary<string, object> htmlAttributes)
        //{
        //    RouteValueDictionary dictionary = ToRouteValueDictionary(htmlAttributes);
        //    bool hasValue = isChecked.HasValue;
        //    if (hasValue)
        //    {
        //        dictionary.Remove("checked");
        //    }
        //    bool? nullable = isChecked;
        //    return InputHelper(htmlHelper, InputType.CheckBox, metadata, name, "true", !hasValue, nullable.HasValue ? nullable.GetValueOrDefault() : false, true, false, dictionary);
        //}

        //private static RouteValueDictionary ToRouteValueDictionary(IDictionary<string, object> dictionary)
        //{
        //    if (dictionary != null)
        //    {
        //        return new RouteValueDictionary(dictionary);
        //    }
        //    return new RouteValueDictionary();
        //}


        //private static MvcHtmlString InputHelper(HtmlHelper htmlHelper, InputType inputType, ModelMetadata metadata, string name, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, IDictionary<string, object> htmlAttributes)
        //{
        //    ModelState state;
        //    string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
        //    if (string.IsNullOrEmpty(fullHtmlFieldName))
        //    {
        //        throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
        //    }
        //    TagBuilder tagBuilder = new TagBuilder("input");
        //    tagBuilder.MergeAttributes<string, object>(htmlAttributes);
        //    tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
        //    tagBuilder.MergeAttribute("name", fullHtmlFieldName, true);
        //    string str2 = Convert.ToString(value, CultureInfo.CurrentCulture);
        //    bool flag = false;
        //    switch (inputType)
        //    {
        //        case InputType.CheckBox:
        //            {
        //                bool? modelStateValue = htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof(bool)) as bool?;
        //                if (modelStateValue.HasValue)
        //                {
        //                    isChecked = modelStateValue.Value;
        //                    flag = true;
        //                }
        //                break;
        //            }
        //        case InputType.Password:
        //            if (value != null)
        //            {
        //                tagBuilder.MergeAttribute("value", str2, isExplicitValue);
        //            }
        //            goto Label_016C;

        //        case InputType.Radio:
        //            break;

        //        default:
        //            {
        //                string str4 = (string)htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof(string));
        //                tagBuilder.MergeAttribute("value", str4 ?? (useViewData ? htmlHelper.EvalString(fullHtmlFieldName) : str2), isExplicitValue);
        //                goto Label_016C;
        //            }
        //    }
        //    if (!flag)
        //    {
        //        string a = htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof(string)) as string;
        //        if (a != null)
        //        {
        //            isChecked = string.Equals(a, str2, StringComparison.Ordinal);
        //            flag = true;
        //        }
        //    }
        //    if (!flag && useViewData)
        //    {
        //        isChecked = htmlHelper.EvalBoolean(fullHtmlFieldName);
        //    }
        //    if (isChecked)
        //    {
        //        tagBuilder.MergeAttribute("checked", "checked");
        //    }
        //    tagBuilder.MergeAttribute("value", str2, isExplicitValue);
        //Label_016C:
        //    if (setId)
        //    {
        //        tagBuilder.GenerateId(fullHtmlFieldName);
        //    }
        //    if (htmlHelper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out state) && (state.Errors.Count > 0))
        //    {
        //        tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
        //    }
        //    tagBuilder.MergeAttributes<string, object>(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));
        //    if (inputType == InputType.CheckBox)
        //    {
        //        StringBuilder builder2 = new StringBuilder();
        //        builder2.Append(tagBuilder.ToString(TagRenderMode.SelfClosing));
        //        TagBuilder builder3 = new TagBuilder("input");
        //        builder3.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
        //        builder3.MergeAttribute("name", fullHtmlFieldName);
        //        builder3.MergeAttribute("value", "false");
        //        builder2.Append(builder3.ToString(TagRenderMode.SelfClosing));
        //        return MvcHtmlString.Create(builder2.ToString());
        //    }
        //    return tagBuilder.ToMvcHtmlString(TagRenderMode.SelfClosing);
        //} 
        #endregion
    }
}