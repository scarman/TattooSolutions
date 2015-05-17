#region Using Directives

using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Restaurant.Common.Enumerations;

#endregion

namespace Restaurant.Web.Core.Helpers
{
    public static class AppHelpers
    {
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
            switch (type)
            {
                case PickerType.Date:
                    div.Attributes.Add("data-datepicktime", "false");
                    break;
                case PickerType.Time:
                    div.Attributes.Add("data-datepickdate", "false");
                    break;
                case PickerType.DateTime:
                    div.Attributes.Add("data-datepicktime", "true");
                    break;
                default:
                    div.Attributes.Add("data-datepicktime", "false");
                    break;
            }
            div.Attributes.Add("data-datelanguage", CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
            div.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);

            var input = htmlHelper.TextBoxFor(expression,
                new { type = "text", @class = "form-control", placeholder = metadata.Watermark });

            var span = new TagBuilder("span");
            span.AddCssClass("input-group-addon");

            var icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-calendar fa-fw");

            span.InnerHtml = icon.ToString();

            var builder = new StringBuilder();
            builder.Append(input);
            builder.Append(span);

            div.InnerHtml = builder.ToString();

            return new MvcHtmlString(div.ToString());
        }
    }
}