using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace Structr.AspNetCore.Routing
{
    /// <summary>
    /// An implementation of <see cref="IOutboundParameterTransformer"/> that slugifies URL string to make it more user-frendly.
    /// </summary>
    /// <remarks>Example: <c>/Users/AccountInfo => /Users/Account-Info</c></remarks>
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if (value == null)
            {
                return null;
            }

            var slugify = Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2");
            return slugify;
        }
    }
}
