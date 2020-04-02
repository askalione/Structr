using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Structr.AspNetCore.JavaScript
{
    public class JavaScriptAlertProvider : IJavaScriptAlertProvider
    {
        private static readonly object _key = typeof(IJavaScriptAlertProvider);

        private readonly IHttpContextAccessor _contextAccessor;

        public JavaScriptAlertProvider(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor == null)
                throw new ArgumentNullException(nameof(contextAccessor));

            _contextAccessor = contextAccessor;
        }

        public void AddAlert(JavaScriptAlert alert)
        {
            if (alert == null)
                throw new ArgumentNullException(nameof(alert));

            var alerts = GetAlertsFromContext();
            alerts.Add(alert);
        }

        public IEnumerable<JavaScriptAlert> GetAlerts()
        {
            var alerts = GetAlertsFromContext();
            return alerts;
        }

        private List<JavaScriptAlert> GetAlertsFromContext()
        {
            var httpContext = _contextAccessor.HttpContext;

            if (!httpContext.Items.ContainsKey(_key))
                httpContext.Items.Add(_key, new List<JavaScriptAlert>());

            return httpContext.Items[_key] as List<JavaScriptAlert>;
        }
    }
}
