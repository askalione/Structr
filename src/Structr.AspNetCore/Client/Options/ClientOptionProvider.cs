using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Structr.AspNetCore.Client.Options
{
    /// <summary>
    /// An implementation of <see cref="IClientOptionProvider"/>.
    /// </summary>
    public class ClientOptionProvider : IClientOptionProvider
    {
        private static readonly object _key = typeof(IClientOptionProvider);

        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Creates an instance of <see cref="ClientOptionProvider"/>.
        /// </summary>
        /// <param name="contextAccessor">Instance of <see cref="IHttpContextAccessor"/>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="contextAccessor"/> is null.</exception>
        public ClientOptionProvider(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor == null)
            {
                throw new ArgumentNullException(nameof(contextAccessor));
            }

            _contextAccessor = contextAccessor;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="key"/> or <paramref name="options"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">When <paramref name="options"/> is empty.</exception>
        public void AddClientOptions(string key, Dictionary<string, object> options)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (options.Any() == false)
            {
                throw new ArgumentOutOfRangeException(nameof(options), "Options cannot be empty.");
            }

            Dictionary<string, object> existingOptions = GetOptionsFromContext(key);
            AppendOptions(existingOptions, options);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="key"/> is <see langword="null"/>.</exception>
        public IReadOnlyDictionary<string, object> GetClientOptions(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Dictionary<string, object> options = GetOptionsFromContext(key);
            return new ReadOnlyDictionary<string, object>(options);
        }

        /// <inheritdoc/>
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> GetAllClientOptions()
        {
            Dictionary<string, Dictionary<string, object>> allOptionsFromContext = GetOptionsFromContext();
            Dictionary<string, IReadOnlyDictionary<string, object>> allOptions = new Dictionary<string, IReadOnlyDictionary<string, object>>();
            foreach (var key in allOptionsFromContext.Keys)
            {
                allOptions.Add(key, new ReadOnlyDictionary<string, object>(allOptionsFromContext[key]));
            }
            return new ReadOnlyDictionary<string, IReadOnlyDictionary<string, object>>(allOptions);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">When <paramref name="routeData"/> is <see langword="null"/>.</exception>
        public string BuildClientOptionsKey(RouteData routeData)
        {
            if (routeData == null)
            {
                throw new ArgumentNullException(nameof(routeData));
            }

            var actionName = routeData.Values["Action"].ToString();
            var controllerName = routeData.Values["Controller"].ToString();
            var area = routeData.Values["area"];
            string areaName = area?.ToString();
            var delimiter = ClientOptionDefaults.Delimiter;
            var key = (string.IsNullOrWhiteSpace(areaName) == false ? FormatClientOptionKey(areaName) + delimiter : "")
                + FormatClientOptionKey(controllerName)
                + delimiter
                + FormatClientOptionKey(actionName);
            return key;
        }

        private static string FormatClientOptionKey(string value)
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
                    {
                        str += "-";
                    }
                    str += char.ToLower(value[i]);
                }
            }

            return str;
        }

        private void AppendOptions(Dictionary<string, object> existingOptions, Dictionary<string, object> newOptions)
        {
            foreach (string key in newOptions.Keys)
            {
                existingOptions[key] = newOptions[key];
            }
        }

        private Dictionary<string, object> GetOptionsFromContext(string key)
        {
            Dictionary<string, Dictionary<string, object>> options = GetOptionsFromContext();

            if (options.ContainsKey(key) == false)
            {
                options.Add(key, new Dictionary<string, object>());
            }

            return options[key];
        }

        private Dictionary<string, Dictionary<string, object>> GetOptionsFromContext()
        {
            HttpContext httpContext = _contextAccessor.HttpContext;

            if (httpContext.Items.ContainsKey(_key) == false)
            {
                httpContext.Items.Add(_key, new Dictionary<string, Dictionary<string, object>>());
            }

            return httpContext.Items[_key] as Dictionary<string, Dictionary<string, object>>;
        }
    }
}
