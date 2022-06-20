using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace Structr.AspNetCore.Routing
{
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
