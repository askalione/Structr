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
    public static class ControllerExtensions
    {
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
            string areaName = area != null ? area.ToString() : null;
            var key = (!string.IsNullOrWhiteSpace(areaName) ? areaName.ToLower() + "-" : "") + controllerName.ToLower() + "-" + actionName.ToLower();
            return key;
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

            return controller.Json(new Json.JsonResult(ok, message));
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
            string referrerUrl = request.HasFormContentType ? request.Form[ReferrerDefaults.UrlFormKey].ToString() : "";
            string redirectUrl = !string.IsNullOrEmpty(referrerUrl) ? referrerUrl : url;

            var redirect = new RedirectResult(redirectUrl);
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
