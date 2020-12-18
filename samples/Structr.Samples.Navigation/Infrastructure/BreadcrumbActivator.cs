using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;

namespace Structr.Samples.Navigation.Infrastructure
{
    public class BreadcrumbActivator : IBreadcrumbActivator
    {
        private readonly IActionContextAccessor _actionContextAccessor;

        public BreadcrumbActivator(IActionContextAccessor actionContextAccessor)
        {
            if (actionContextAccessor == null)
                throw new ArgumentNullException(nameof(actionContextAccessor));

            _actionContextAccessor = actionContextAccessor;
        }

        public bool Activate(Breadcrumb breadcrumb)
        {
            var actionContext = _actionContextAccessor.ActionContext;
            string action = actionContext.RouteData.Values["action"].ToString();
            string controller = actionContext.RouteData.Values["controller"].ToString();
            string area = actionContext.RouteData.Values["area"]?.ToString();

            return string.Equals(breadcrumb.Action, action, StringComparison.OrdinalIgnoreCase)
                && string.Equals(breadcrumb.Controller, controller, StringComparison.OrdinalIgnoreCase)
                && string.Equals(breadcrumb.Area, area, StringComparison.OrdinalIgnoreCase);
        }
    }
}
