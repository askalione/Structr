using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Structr.AspNetCore.Referrer;
using System;
using System.Collections.Generic;

namespace Structr.Tests.AspNetCore.TestUtils
{
    internal static class ControllerFactory
    {
        public static Controller CreateController(out HttpContext httpContext,
            string? referrer = null,
            Action<IServiceCollection, HttpContext>? configureServices = null,
            string areaName = "Admin",
            string controllerName = "Test",
            string actionName = "TestAction")
        {
            httpContext = new DefaultHttpContext();

            if (referrer != null)
            {
                httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
                httpContext.Request.Form =
                    new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> {
                        { ReferrerDefaults.Key, referrer }
                    });
            }

            var routeData = new RouteData(new RouteValueDictionary(new Dictionary<string, string> {
                { "area", areaName },
                { "Controller", controllerName },
                { "Action", actionName }
            }));

            var controllerActionDescriptor = new ControllerActionDescriptor();
            controllerActionDescriptor.ActionName = "TestAction";

            var actionContext = new ActionContext(httpContext, routeData, controllerActionDescriptor);

            var serviceCollection = new ServiceCollection();
            configureServices?.Invoke(serviceCollection, httpContext);
            httpContext.RequestServices = serviceCollection.BuildServiceProvider();

            var controller = new TestController();
            controller.Url = new UrlHelper(actionContext);
            controller.ControllerContext = new ControllerContext(actionContext);

            return controller;
        }
    }
}
