using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Structr.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Structr.AspNetCore.Client.Alerts
{
    /// <summary>
    /// An implementation of <see cref="IAlertProvider"/>.
    /// </summary>
    public class ClientAlertProvider : IClientAlertProvider
    {
        private static readonly string _key = typeof(IClientAlertProvider).FullName;

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        /// <summary>
        /// Creates an instance of <see cref="ClientAlertProvider"/>.
        /// </summary>
        /// <param name="contextAccessor">Instance of <see cref="IHttpContextAccessor"/>.</param>
        /// <param name="tempDataDictionaryFactory">Instance of <see cref="ITempDataDictionaryFactory"/>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="contextAccessor"/> is null.</exception>
        public ClientAlertProvider(IHttpContextAccessor contextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory)
        {
            if (contextAccessor == null)
            {
                throw new ArgumentNullException(nameof(contextAccessor));
            }
            if (tempDataDictionaryFactory == null)
            {
                throw new ArgumentNullException(nameof(tempDataDictionaryFactory));
            }

            _contextAccessor = contextAccessor;
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="alert"/> is null.</exception>
        public void AddClientAlert(ClientAlert alert)
        {
            if (alert == null)
            {
                throw new ArgumentNullException(nameof(alert));
            }

            List<ClientAlert> alerts = GetAlertsFromTempData();
            alerts.Add(alert);
            SaveAlertsToTempData(alerts);
        }

        public IEnumerable<ClientAlert> GetAllClientAlerts()
            => GetAlertsFromTempData();

        private List<ClientAlert> GetAlertsFromTempData()
        {
            ITempDataDictionary tempData = GetTempData();
            List<ClientAlert> alerts = tempData.Peek<List<ClientAlert>>(_key) ?? new List<ClientAlert>();
            return alerts;
        }

        private void SaveAlertsToTempData(List<ClientAlert> alerts)
        {
            ITempDataDictionary tempData = GetTempData();
            tempData.Remove(_key);
            tempData.Put(_key, alerts ?? new List<ClientAlert>());
        }

        private ITempDataDictionary GetTempData()
        {
            HttpContext httpContext = _contextAccessor.HttpContext;
            ITempDataDictionary tempData = _tempDataDictionaryFactory.GetTempData(httpContext);
            return tempData;
        }
    }
}
