using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Adds javascript options to context associated with controller.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="options">Options to add represented by object.</param>
        public static void AddJavaScriptOptions(this Controller controller, object options)
        {
            var key = GetJavaScriptOptionKey(controller);
            AddJavaScriptOptions(controller, key, options);
        }

        /// <param name="key">Key to use for storing options.</param>
        /// <inheritdoc cref="AddJavaScriptOptions(Controller, object)"/>
        public static void AddJavaScriptOptions(this Controller controller, string key, object options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var optionsType = options.GetType();
            var optionsProps = optionsType.GetProperties();
            var optionsAsDictionary = optionsProps.ToDictionary(x => x.Name, x => x.GetValue(options, null));
            AddJavaScriptOptions(controller, key, optionsAsDictionary);
        }

        /// <param name="options">Options to add represented by dictionary.</param>
        /// <inheritdoc cref="AddJavaScriptOptions(Controller, object)"/>
        public static void AddJavaScriptOptions(this Controller controller, Dictionary<string, object> options)
        {
            var key = GetJavaScriptOptionKey(controller);
            AddJavaScriptOptions(controller, key, options);
        }

        /// <inheritdoc cref="AddJavaScriptOptions(Controller, Dictionary{string, object})"/>
        /// <inheritdoc cref="AddJavaScriptOptions(Controller, string, object)"/>
        public static void AddJavaScriptOptions(this Controller controller, string key, Dictionary<string, object> options)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

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
            var key = (string.IsNullOrWhiteSpace(areaName) == false ? FormatJavaScriptOptionKey(areaName) + delimiter : "")
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

        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/> object, with serialized success marker.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="ok">Success marker.</param>
        /// <returns>An instance of <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="controller"/> is <see langword="null"/>.</exception>
        public static Microsoft.AspNetCore.Mvc.JsonResult JsonResult(this Controller controller, bool ok)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            return controller.Json(new Json.JsonResult(ok));
        }

        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/> object, with serialized success marker and message.
        /// </summary>
        /// <param name="message">Message to attach to result.</param>
        /// <inheritdoc cref="JsonResult(Controller, bool)"/>
        public static Microsoft.AspNetCore.Mvc.JsonResult JsonResult(this Controller controller, bool ok, string message)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            return controller.Json(new Json.JsonResult(ok, message));
        }


        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/> object, with serialized success marker, message and data.
        /// </summary>
        /// <param name="data">Data object to append to result.</param>
        /// <inheritdoc cref="JsonResult(Controller, bool, string)"/>
        public static Microsoft.AspNetCore.Mvc.JsonResult JsonResult(this Controller controller, bool ok, string message, object data)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            return controller.Json(new Json.JsonResult(ok, message, data));
        }

        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/> object representing successful response,
        /// with serialized success marker equals <see langword="true"/> and message.
        /// </summary>
        /// <inheritdoc cref="JsonResult(Controller, bool, string)"/>
        public static Microsoft.AspNetCore.Mvc.JsonResult JsonSuccess(this Controller controller, string message)
            => JsonResult(controller, true, message);

        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/> object representing successful response,
        /// with serialized success marker equals <see langword="true"/>, message and data.
        /// </summary>
        /// <inheritdoc cref="JsonResult(Controller, bool, string, object)"/>
        public static Microsoft.AspNetCore.Mvc.JsonResult JsonSuccess(this Controller controller, string message, object data)
            => JsonResult(controller, true, message, data);

        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/> object representing failed response,
        /// with serialized success marker equals <see langword="false"/> and error message.
        /// </summary>
        /// <inheritdoc cref="JsonResult(Controller, bool, string)"/>
        public static Microsoft.AspNetCore.Mvc.JsonResult JsonError(this Controller controller, string message)
            => JsonResult(controller, false, message);

        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/> object representing failed response,
        /// with serialized success marker equals <see langword="false"/> and error messages.
        /// </summary>
        /// <param name="messages">Error messages to attach to result.</param>
        /// <inheritdoc cref="JsonResult(Controller, bool, string)"/>
        public static Microsoft.AspNetCore.Mvc.JsonResult JsonError(this Controller controller, IEnumerable<string> messages)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            return controller.Json(new Json.JsonResult(messages));
        }

        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/> object representing failed response,
        /// with serialized success marker equals <see langword="false"/>, error messages and data.
        /// </summary>
        /// <inheritdoc cref="JsonResult(Controller, bool, string, object)"/>
        /// <inheritdoc cref="JsonError(Controller, IEnumerable{string})"/>
        public static Microsoft.AspNetCore.Mvc.JsonResult JsonError(this Controller controller, IEnumerable<string> messages, object data)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            return controller.Json(new Json.JsonResult(messages, data));
        }

        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Mvc.JsonResult"/> object representing successful response,
        /// with serialized success marker equals <see langword="true"/> and data object.
        /// </summary>
        /// <inheritdoc cref="JsonResult(Controller, bool, string, object)"/>
        public static Microsoft.AspNetCore.Mvc.JsonResult JsonData(this Controller controller, object data)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            return controller.Json(new Json.JsonResult(true, data));
        }

        #endregion

        #region Redirect
        /// <summary>
        /// Creates an instance of <see cref="RedirectAjaxResult"/> while checking if url is local.
        /// In case of local url the redirect is performed to specified <paramref name="url"/>.
        /// Otherwise creates response for redirecting to application's root.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="url">Url to redirect to.</param>
        /// <returns>An instace of <see cref="RedirectResult"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="controller"/> is <see langword="null"/>.</exception>
        public static RedirectAjaxResult LocalRedirectAjax(this Controller controller, string url)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            return RedirectAjax(controller, controller.Url.IsLocalUrl(url) ? url : "/");
        }

        /// <summary>
        /// Creates an instance of <see cref="RedirectAjaxResult"/>.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="url">Url to redirect to.</param>
        /// <returns>An instace of <see cref="RedirectResult"/>.</returns>
        public static RedirectAjaxResult RedirectAjax(this Controller controller, string url)
            => new RedirectAjaxResult(url);

        /// <summary>
        /// Creates <see cref="RedirectResult"/> object specifying redirect to url depending on
        /// presence '__Referrer' key in <see cref="HttpRequest.Form"/>. In case of existing of such key
        /// the corresponding url from From value will be used to redirect to. In other case the
        /// provided <paramref name="url"/> will be.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="url">Fallback url to redirect to in case of lack of '__Referrer' key in <see cref="HttpRequest.Form"/></param>
        /// <returns>An instance of <see cref="RedirectResult"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <remarks>The extension method could be helpful when, for example, redirect back to entity's
        /// details form from edit form is needed after pressing "Ok" button or "Cancel" button. In such
        /// cases use of <see cref="TagHelpers.ReferrerTagHelper"/> will be helpful.</remarks>
        public static RedirectResult RedirectToReferrer(this Controller controller, string url)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var request = controller.HttpContext.Request;
            string referrer = request.HasFormContentType ? request.Form[ReferrerConstants.Key].ToString() : "";
            string urlRedirect = !string.IsNullOrEmpty(referrer) ? referrer : url;

            var redirect = new RedirectResult(urlRedirect);
            return redirect;
        }

        #endregion

        #region Render

        /// <summary>
        /// Render a partial view using provided model.
        /// </summary>
        /// <typeparam name="TModel">Type of model to render view with.</typeparam>
        /// <param name="controller"></param>
        /// <param name="viewName">Name of view. If not specified then action name will be taken.</param>
        /// <param name="model">Model to render view with.</param>
        /// <returns>Rendered view as a <see langword="string"/>.</returns>
        public static async Task<string> RenderPartialViewAsync<TModel>(this Controller controller, string viewName, TModel model)
        {
            return await RenderViewAsync(controller, viewName, model, true);
        }

        /// <summary>
        /// Render a view using provided model.
        /// </summary>
        /// <param name="isPartial">Determines if current view is a partial view.</param>
        /// <inheritdoc cref="RenderPartialViewAsync{TModel}(Controller, string, TModel)"/>
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
                {
                    throw new Exception($"A view with the name {viewName} could not be found");
                }

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
