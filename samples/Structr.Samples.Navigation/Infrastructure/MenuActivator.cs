using Microsoft.AspNetCore.Http;
using System;

namespace Structr.Samples.Navigation.Infrastructure
{
    public class MenuActivator : IMenuActivator
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MenuActivator(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
                throw new ArgumentNullException(nameof(httpContextAccessor));

            _httpContextAccessor = httpContextAccessor;
        }

        public bool Activate(MenuItem item)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var id = httpContext.Items[MenuConstants.Key]?.ToString();

            return item.Id.Equals(id, StringComparison.OrdinalIgnoreCase);
        }
    }
}
