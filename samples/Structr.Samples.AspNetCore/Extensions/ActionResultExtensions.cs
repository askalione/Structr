using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.JavaScript;
using Structr.AspNetCore.Mvc;

namespace Structr.Samples.AspNetCore.Extensions
{
    public static class ActionResultExtensions
    {
        public static IActionResult Info(this IActionResult result, string message)
            => result.AddJavaScriptAlert(new JavaScriptAlert("info", message));

        public static IActionResult Success(this IActionResult result, string message)
            => result.AddJavaScriptAlert(new JavaScriptAlert("success", message));

        public static IActionResult Warning(this IActionResult result, string message)
            => result.AddJavaScriptAlert(new JavaScriptAlert("warning", message));

        public static IActionResult Error(this IActionResult result, string message)
            => result.AddJavaScriptAlert(new JavaScriptAlert("error", message));
    }
}
