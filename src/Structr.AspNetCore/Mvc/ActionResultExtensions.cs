using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.JavaScript;

namespace Structr.AspNetCore.Mvc
{
    public static class ActionResultExtensions
    {
        public static IActionResult AddJavaScriptAlert(this IActionResult result, JavaScriptAlert alert)
            => new JavaScriptAlertResult(result, alert);
    }
}
