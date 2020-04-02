using Microsoft.AspNetCore.Mvc.Filters;
using Structr.Samples.Navigation.Infrastructure;
using System;

namespace Structr.Samples.Navigation.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MenuAttribute : ActionFilterAttribute
    {
        public string Id { get; }

        public MenuAttribute(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            Id = id;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items[MenuConstants.Key] = Id;
        }
    }
}
