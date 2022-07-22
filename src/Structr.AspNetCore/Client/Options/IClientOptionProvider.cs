using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace Structr.AspNetCore.Client.Options
{
    /// <summary>
    /// Provides methods assisting in passing data represented by dictionary via <see cref="HttpContext.Items"/>.
    /// </summary>
    public interface IClientOptionProvider
    {
        /// <summary>
        /// Places set of options with specified key into current <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="options">Set of options.</param>
        void AddClientOptions(string key, Dictionary<string, object> options);

        /// <summary>
        /// Gets all options with specified <paramref name="key"/> stored in current <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Dictionary containing options.</returns>
        IReadOnlyDictionary<string, object> GetClientOptions(string key);

        /// <summary>
        /// Gets all options stored in current <see cref="HttpContext"/>.
        /// </summary>
        /// <returns>Dictionary containing all options with their corresponding keys.</returns>
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> GetAllClientOptions();

        /// <summary>
        /// Builds a key for options using <paramref name="routeData"/>.
        /// </summary>
        /// <param name="routeData">The <see cref="RouteData"/>.</param>
        /// <returns>The key for options.</returns>
        string BuildClientOptionsKey(RouteData routeData);
    }
}
