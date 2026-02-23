using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace IncosafCMS.Web
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            //if (String.IsNullOrEmpty(cssClass))
            //    cssClass = "active";

            //string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            //string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            //if (String.IsNullOrEmpty(controller))
            //    controller = currentController;

            //if (String.IsNullOrEmpty(action))
            //    action = currentAction;

            //return controller == currentController && action == currentAction ?
            //    cssClass : String.Empty;

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            ViewContext viewContext = html.ViewContext;
            bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            string[] acceptedActions = action.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controller.Trim().Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object attributes = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = Nullable.GetUnderlyingType(metadata.ModelType) ?? metadata.ModelType;
            var enumValues = Enum.GetValues(enumType).Cast<object>();
            var items = enumValues.Select(item =>
            {
                var type = item.GetType();
                var member = type.GetMember(item.ToString());
                var attribute = member[0].GetCustomAttribute<DescriptionAttribute>();
                string text = attribute != null ? ((DescriptionAttribute)attribute).Description : item.ToString();
                string value = ((int)item).ToString();
                bool selected = item.Equals(metadata.Model);
                return new SelectListItem
                {
                    Text = text,
                    Value = value,
                    Selected = selected
                };
            });
            return htmlHelper.DropDownListFor(expression, items, string.Empty, attributes);
        }
        public static MvcHtmlString TextBoxWithFormatFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.TextBox(htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(metadata.PropertyName), string.Format(metadata.DisplayFormatString, metadata.Model), htmlAttributes);
        }

        public static object EnumWithDescription(Type enumType)
        {
            var enumValues = Enum.GetValues(enumType).Cast<object>();

            //var items = enumValues.Select(item =>
            //{
            //    var type = item.GetType();
            //    var member = type.GetMember(item.ToString());
            //    var attribute = member[0].GetCustomAttribute<DescriptionAttribute>();
            //    string text = attribute != null ? ((DescriptionAttribute)attribute).Description : item.ToString();
            //    string value = ((int)item).ToString();
            //    return new SelectListItem
            //    {
            //        Text = text,
            //        Value = value
            //    };
            //});

            var items = enumValues.Select(item =>
            {
                var type = item.GetType();
                var member = type.GetMember(item.ToString());
                var attribute = member[0].GetCustomAttribute<DescriptionAttribute>();
                string text = attribute != null ? ((DescriptionAttribute)attribute).Description : item.ToString();
                var value = ((int)item);
                return new 
                {
                    Text = text,
                    Value = value
                };
            });

            return items;
        }
    }
}
