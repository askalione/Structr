using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Structr.AspNetCore.Internal;
using Structr.AspNetCore.JavaScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Structr.AspNetCore.Mvc
{
    /// <summary>
    /// Defines extension methods on <see cref="Controller"/>.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Creates a <see cref="JavaScriptResult"/> with specified content.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="content">Javascript code.</param>
        /// <returns>A <see cref="JavaScriptResult"/> object.</returns>
        public static JavaScriptResult JavaScript(this Controller controller, string content)
            => new JavaScriptResult(content);

        #region JavaScriptOptions

        public static void AddJavaScriptOptions(this Controller controller, object options)
        {
            var key = GetJavaScriptOptionKey(controller);
            AddJavaScriptOptions(controller, key, options);
        }

        public static void AddJavaScriptOptions(this Controller controller, string key, object options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var optionsType = options.GetType();
            var optionsProps = optionsType.GetProperties();
            var optionsAsDictionary = optionsProps.ToDictionary(x => x.Name, x => x.GetValue(options, null));
            AddJavaScriptOptions(controller, key, optionsAsDictionary);
        }

        public static void AddJavaScriptOptions(this Controller controller, Dictionary<string, object> options)
        {
            var key = GetJavaScriptOptionKey(controller);
            AddJavaScriptOptions(controller, key, options);
        }

        public static void AddJavaScriptOptions(this Controller controller, string key, Dictionary<string, object> options)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            var optionProvider = GetJavaScriptOptionProvider(controller);
            optionProvider.AddOptions(key, options);
        }

        private static string GetJavaScriptOptionKey(this Controller controller)
        {
            var routeData = controller.RouteData;
            var actionName = routeData.Values["Action"].ToString();
            var controllerName = routeData.Values["Controller"].ToString();
            var area = routeData.Values["area"];
            string areaName = area?.ToString();
            var delimiter = JavaScriptOptionConstants.Delimiter;
            var key = (!string.IsNullOrWhiteSpace(areaName) ? FormatJavaScriptOptionKey(areaName) + delimiter : "")
                + FormatJavaScriptOptionKey(controllerName)
                + delimiter
                + FormatJavaScriptOptionKey(actionName);
            return key;
        }

        private static string FormatJavaScriptOptionKey(string value)
        {
            var str = "";
            for (var i = 0; i < value.Length; i++)
            {
                if (i == 0)
                {
                    str += char.ToLower(value[i]);
                }
                else
                {
                    if (char.IsUpper(value[i]))
                        str += "-";
                    str += char.ToLower(value[i]);
                }
            }

            return str;
        }

        private static IJavaScriptOptionProvider GetJavaScriptOptionProvider(Controller controller)
        {
            var optionProvider = controller
                .HttpContext
                .RequestServices
                .GetRequiredService<IJavaScriptOptionProvider>();
            return optionProvider;
        }

        #endregion

        #region Json

        public static Microsoft.AspNetCore.Mvc.JsonResult JsonResult(this Controller controller, bool ok)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.Json(new Json.JsonResult(ok));
        }

        public static Microsoft.AspNetCore.Mvc.JsonResult JsonResult(this Controller controller, bool ok, string message)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.Json(new Json.JsonResult(ok, message));
        }

        public static Microsoft.AspNetCore.Mvc.JsonResult JsonResult(this Controller controller, bool ok, string message, object data)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.Json(new Json.JsonResult(ok, message, data));
        }

        public static Microsoft.AspNetCore.Mvc.JsonResult JsonSuccess(this Controller controller, string message)
            => JsonResult(controller, true, message);

        public static Microsoft.AspNetCore.Mvc.JsonResult JsonSuccess(this Controller controller, string message, object data)
            => JsonResult(controller, true, message, data);

        public static Microsoft.AspNetCore.Mvc.JsonResult JsonError(this Controller controller, string message)
            => JsonResult(controller, false, message);

        public static Microsoft.AspNetCore.Mvc.JsonResult JsonError(this Controller controller, IEnumerable<string> messages)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.Json(new Json.JsonResult(messages));
        }

        public static Microsoft.AspNetCore.Mvc.JsonResult JsonError(this Controller controller, IEnumerable<string> messages, object data)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.Json(new Json.JsonResult(messages, data));
        }

        public static Microsoft.AspNetCore.Mvc.JsonResult JsonData(this Controller controller, object data)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.Json(new Json.JsonResult(true, data));
        }

        #endregion

        #region Redirect

        public static RedirectAjaxResult LocalRedirectAjax(this Controller controller, string url)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return RedirectAjax(controller, controller.Url.IsLocalUrl(url) ? url : "/");
        }

        public static RedirectAjaxResult RedirectAjax(this Controller controller, string url)
            => new RedirectAjaxResult(url);

        public static RedirectResult RedirectToReferrer(this Controller controller, string url)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            var request = controller.HttpContext.Request;
            string referrer = request.HasFormContentType ? request.Form[ReferrerConstants.Key].ToString() : "";
            string urlRedirect = !string.IsNullOrEmpty(referrer) ? referrer : url;

            var redirect = new RedirectResult(urlRedirect);
            return redirect;
        }

        #endregion

        #region Render

        public static async Task<string> RenderPartialViewAsync<TModel>(this Controller controller, string viewName, TModel model)
        {
            return await RenderViewAsync(controller, viewName, model, true);
        }

        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool isPartial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = GetViewEngineResult(controller, viewName, isPartial, viewEngine);

                if (viewResult.Success == false)
                    throw new Exception($"A view with the name {viewName} could not be found");

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

        private static ViewEngineResult GetViewEngineResult(Controller controller, string viewName, bool isPartial, IViewEngine viewEngine)
        {
            if (viewName.StartsWith("~/"))
            {
                var env = controller.HttpContext.RequestServices.GetService<IWebHostEnvironment>();
                return viewEngine.GetView(env.WebRootPath, viewName, !isPartial);
            }
            else
            {
                return viewEngine.FindView(controller.ControllerContext, viewName, !isPartial);

            }
        }

        #endregion
    }
}
