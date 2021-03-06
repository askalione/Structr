using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Structr.AspNetCore.JavaScript
{
    public class JavaScriptOptionProvider : IJavaScriptOptionProvider
    {
        private static readonly object _key = typeof(IJavaScriptOptionProvider);

        private readonly IHttpContextAccessor _contextAccessor;

        public JavaScriptOptionProvider(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor == null)
                throw new ArgumentNullException(nameof(contextAccessor));

            _contextAccessor = contextAccessor;
        }

        public void AddOptions(string key, Dictionary<string, object> options)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (options.Any() == false)
                throw new ArgumentOutOfRangeException(nameof(options), "Options cannot be empty");

            var existingOptions = GetOptionsFromContext(key);
            AppendOptions(existingOptions, options);
        }

        public IReadOnlyDictionary<string, object> GetOptions(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var options = GetOptionsFromContext(key);
            return new ReadOnlyDictionary<string, object>(options);
        }

        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> GetOptions()
        {
            var allOptionsFromContext = GetOptionsFromContext();
            var allOptions = new Dictionary<string, IReadOnlyDictionary<string, object>>();
            foreach (var key in allOptionsFromContext.Keys)
            {
                allOptions.Add(key, new ReadOnlyDictionary<string, object>(allOptionsFromContext[key]));
            }
            return new ReadOnlyDictionary<string, IReadOnlyDictionary<string, object>>(allOptions);
        }

        private void AppendOptions(Dictionary<string, object> existingOptions, Dictionary<string, object> newOptions)
        {
            foreach (var key in newOptions.Keys)
                existingOptions[key] = newOptions[key];
        }

        private Dictionary<string, object> GetOptionsFromContext(string key)
        {
            var options = GetOptionsFromContext();

            if (!options.ContainsKey(key))
                options.Add(key, new Dictionary<string, object>());

            return options[key];
        }

        private Dictionary<string, Dictionary<string, object>> GetOptionsFromContext()
        {
            var httpContext = _contextAccessor.HttpContext;

            if (!httpContext.Items.ContainsKey(_key))
                httpContext.Items.Add(_key, new Dictionary<string, Dictionary<string, object>>());

            return httpContext.Items[_key] as Dictionary<string, Dictionary<string, object>>;
        }
    }
}
