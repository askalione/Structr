using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Structr.AspNetCore.Mvc
{
    /// <summary>
    /// Defines extension methods on <see cref="Controller"/>.
    /// </summary>
    public static class PartialViewControllerExtensions
    {
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
                    throw new Exception($"A view with the name \"{viewName}\" could not be found.");
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
                IWebHostEnvironment env = controller.HttpContext.RequestServices.GetService<IWebHostEnvironment>();
                return viewEngine.GetView(env.WebRootPath, viewName, !isPartial);
            }
            else
            {
                return viewEngine.FindView(controller.ControllerContext, viewName, !isPartial);

            }
        }
    }
}
