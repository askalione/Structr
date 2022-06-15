using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Structr.AspNetCore.JavaScript
{
    /// <summary>
    /// Provides methods assisting in passing data represented by dictionary via <see cref="HttpContext.Items"/>.
    /// </summary>
    public interface IJavaScriptOptionProvider
    {
        /// <summary>
        /// Places set of options with specified key into current <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="options">Set of options.</param>
        void AddOptions(string key, Dictionary<string, object> options);

        /// <summary>
        /// Gets all options with specified <paramref name="key"/> stored in current <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Dictionary containing options.</returns>
        IReadOnlyDictionary<string, object> GetOptions(string key);

        /// <summary>
        /// Gets all options stored in current <see cref="HttpContext"/>.
        /// </summary>
        /// <returns>Dictionary containing all options with their corresponding keys.</returns>
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> GetOptions();
    }
}
