using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace GSLogistics.Website.Common
{
    public enum DateTimePickerView
    {
        Hour = 0,
        Day = 1,
        Month = 2,
        Year = 3,
        Decade = 4
    }



    public static class DateTimeEditorExtensions
    {
        public static MvcHtmlString DatePicker<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return DateTimePicker(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), minView: DateTimePickerView.Month);
        }

        public static MvcHtmlString TimePicker<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, DateTime?>> expression, IDictionary<string, object> htmlAttributes = null)
        {
            return DateTimePicker(htmlHelper, expression, htmlAttributes, minView: DateTimePickerView.Hour, startView: DateTimePickerView.Hour);
        }

        /// <summary>
        /// Creates a date time picker.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        /// <remarks>
        /// This works with this:
        /// http://www.malot.fr/bootstrap-datetimepicker/
        /// </remarks>
        public static MvcHtmlString DateTimePicker<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, string jsFormat = "mm/dd/yyyy", string cFormat = "d", bool? autoClose = true, bool? todayHighlight = true, DateTimePickerView? minView = DateTimePickerView.Hour, DateTimePickerView? startView = DateTimePickerView.Month)
        {
            ModelMetadata metadata = HtmlExtensionsHelper.GetModelMetadata(htmlHelper, expression);
            string propertyNameVar = metadata.PropertyName;

            var parameters = new { PropertyName = propertyNameVar, JsFormat = jsFormat, CFormat = cFormat, AutoClose = autoClose, TodayHighlight = todayHighlight, MinView = minView, StartView = startView, HtmlAttributes = htmlAttributes };

            return htmlHelper.EditorFor(expression, ViewNames.DateTimePicker, parameters);
        }
    }
}
