using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Structr.AspNetCore.Http;
using System;

namespace Structr.AspNetCore.Mvc
{
    /// <summary>
    /// Specifies that marked controller or action is valid only for ajax requests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
            => routeContext.HttpContext.Request.IsAjaxRequest();
    }
}
