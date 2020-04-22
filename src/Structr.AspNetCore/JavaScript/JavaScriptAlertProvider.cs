using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Structr.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Structr.AspNetCore.JavaScript
{
    public class JavaScriptAlertProvider : IJavaScriptAlertProvider
    {
        private static readonly string _key = typeof(IJavaScriptAlertProvider).FullName;

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public JavaScriptAlertProvider(IHttpContextAccessor contextAccessor,
            ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            if (contextAccessor == null)
                throw new ArgumentNullException(nameof(contextAccessor));
            if (tempDataDictionaryFactory == null)
                throw new ArgumentNullException(nameof(tempDataDictionaryFactory));

            _contextAccessor = contextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        public void AddAlert(JavaScriptAlert alert)
        {
            if (alert == null)
                throw new ArgumentNullException(nameof(alert));

            var alerts = GetAlertsFromTempData();
            alerts.Add(alert);
            SaveAlertsToTempData(alerts);
        }

        public IEnumerable<JavaScriptAlert> GetAlerts()
        {
            var alerts = GetAlertsFromTempData();
            return alerts;
        }

        private List<JavaScriptAlert> GetAlertsFromTempData()
        {
            var tempData = GetTempData();
            var alerts = tempData.Peek<List<JavaScriptAlert>>(_key) ?? new List<JavaScriptAlert>();
            return alerts;
        }

        private void SaveAlertsToTempData(List<JavaScriptAlert> alerts)
        {
            var tempData = GetTempData();
            tempData.Remove(_key);
            tempData.Put(_key, alerts ?? new List<JavaScriptAlert>());
        }

        private ITempDataDictionary GetTempData()
        {
            var httpContext = _contextAccessor.HttpContext;
            var tempData = _tempDataDictionaryFactory.GetTempData(httpContext);

            return tempData;
        }
    }
}
