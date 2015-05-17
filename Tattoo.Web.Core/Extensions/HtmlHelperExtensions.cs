#region Using Directives

using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Tattoo.Common.Strings;
using Tattoo.Web.Core.Enumerations;
using Tattoo.Web.Core.Helpers;

#endregion

namespace Tattoo.Web.Core.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static UserHtmlHelper User(this HtmlHelper html)
        {
            return new UserHtmlHelper(html, new UrlHelper(html.ViewContext.RequestContext));
        }

        public static MvcHtmlString DateTimePicker<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return DateTimePicker(htmlHelper, expression, PickerType.Date, htmlAttributes);
        }

        public static MvcHtmlString DateTimePicker<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, PickerType type, object htmlAttributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var div = new TagBuilder("div");
            div.AddCssClass("date");
            div.AddCssClass("input-group");

            var iconClass = "fa fa-calendar fa-fw";
            string format;
            switch (type)
            {
                case PickerType.Date:
                    div.Attributes.Add("data-datepicktime", "false");
                    format = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
                    break;
                case PickerType.Time:
                    div.Attributes.Add("data-datepickdate", "false");
                    iconClass = "fa fa-clock-o fa-fw";
                    format = CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern;
                    break;
                case PickerType.DateTime:
                    div.Attributes.Add("data-datepicktime", "true");
                    format = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern + CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern;
                    break;
                default:
                    div.Attributes.Add("data-datepicktime", "false");
                    format = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
                    break;
            }
            div.Attributes.Add("data-datelanguage", CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower());
            div.Attributes.Add("data-dateformat", format.ToUpper());

            var inputAttributes =
                HtmlHelper.AnonymousObjectToHtmlAttributes(
                    new { type = "text", @class = "form-control", placeholder = metadata.Watermark });
            //var paramAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            //foreach (var pair in paramAttributes)
            //{
            //    if (inputAttributes.ContainsKey(pair.Key))
            //        inputAttributes[pair.Key] = inputAttributes[pair.Key] + " " + paramAttributes[pair.Key];
            //    else
            //        inputAttributes.Add(pair.Key, pair.Value);
            //}
            var input = htmlHelper.TextBoxFor(expression, string.Format("{{0:{0}}}", format),
                inputAttributes);

            var span = new TagBuilder("span");
            span.AddCssClass("input-group-addon");

            var icon = new TagBuilder("i");
            icon.AddCssClass(iconClass);

            span.InnerHtml = icon.ToString();

            var builder = new StringBuilder();
            builder.Append(input);
            builder.Append(span);

            div.InnerHtml = builder.ToString();

            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString ActionButtonBar<TModel>(this HtmlHelper<TModel> htmlHelper, BootstrapStyle btnStyle, string buttonText, string actionName = null, string returnActionName = null, bool isSubmit = false, object routes = null)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("form-group");

            var colDiv = new TagBuilder("div");
            colDiv.AddCssClass("col-md-offset-2 col-md-10");

            string actionButton;

            if (isSubmit)
            {
                var btn = new TagBuilder("input");
                btn.AddCssClass(string.Format("btn btn-{0}", btnStyle.ToStyleString()));
                btn.Attributes.Add("type", "submit");
                btn.Attributes.Add("value", buttonText);

                actionButton = btn.ToString(TagRenderMode.SelfClosing);
            }
            else
            {
                var link = htmlHelper.ActionLink(buttonText, actionName, routes, new { @class = string.Format("btn btn-{0}", btnStyle.ToStyleString()) });
                actionButton = link.ToString();
            }

            var backButton = htmlHelper.ActionLink(Resources.Btn_Back, returnActionName, null, new { @class = "btn btn-default" }).ToString();

            var builder = new StringBuilder();
            builder.Append(actionButton);
            builder.Append(" ");
            builder.Append(backButton);

            colDiv.InnerHtml = builder.ToString();
            div.InnerHtml = colDiv.ToString();

            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString CountryPicker<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string defaultCountry = "", bool showFlags = true, object htmlAttributes = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var sanitizedId = TagBuilder.CreateSanitizedId(fullHtmlFieldName);

            var div = new TagBuilder("div");
            div.AddCssClass("bcp-countries");
            div.AddCssClass("bcp-selectbox");

            div.GenerateId(sanitizedId);
            div.Attributes.Add("data-name", fullHtmlFieldName);
            div.Attributes.Add("data-flags", showFlags.ToString().ToLower());
            div.Attributes.Add("data-country", ((string)modelMetadata.Model) ?? defaultCountry);
            div.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);

            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString StatePicker<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, Expression<Func<TModel, TProperty>> countryExpression,
            string defaultState = "", object htmlAttributes = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var fieldName = ExpressionHelper.GetExpressionText(expression);

            var countryFieldName = ExpressionHelper.GetExpressionText(countryExpression);

            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            var sanitizedId = TagBuilder.CreateSanitizedId(fullHtmlFieldName);

            var countryFullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(countryFieldName);
            var countrySanitizedId = TagBuilder.CreateSanitizedId(countryFullHtmlFieldName);

            var div = new TagBuilder("div");
            div.AddCssClass("bcp-states");
            div.AddCssClass("bcp-selectbox");

            div.GenerateId(sanitizedId);
            div.Attributes.Add("data-name", fullHtmlFieldName);
            div.Attributes.Add("data-state", ((string)modelMetadata.Model) ?? defaultState);
            div.Attributes.Add("data-country", countrySanitizedId);
            div.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);

            return new MvcHtmlString(div.ToString());
        }

        public static MvcHtmlString StateDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, Expression<Func<TModel, TProperty>> countryExpression)
        {
            //var modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            //var fieldName = ExpressionHelper.GetExpressionText(expression);

            //var countryModelMetadata = ModelMetadata.FromLambdaExpression(countryExpression, htmlHelper.ViewData);
            //var countryFieldName = ExpressionHelper.GetExpressionText(countryExpression);

            //var fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(fieldName);
            //var sanitizedId = TagBuilder.CreateSanitizedId(fullHtmlFieldName);

            //var countryFullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(countryFieldName);
            //var countrySanitizedId = TagBuilder.CreateSanitizedId(countryFullHtmlFieldName);

            //var state = ((string)modelMetadata.Model) ?? string.Empty;
            //var country = ((string)countryModelMetadata.Model) ?? string.Empty;

            return new MvcHtmlString(string.Empty);
        }
    }
}