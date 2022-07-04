using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.Client.Alerts;

namespace Structr.Samples.AspNetCore.Extensions
{
    public static class ActionResultExtensions
    {
        public static IActionResult Info(this IActionResult result, string message)
            => result.AddClientAlert(new ClientAlert("info", message));

        public static IActionResult Success(this IActionResult result, string message)
            => result.AddClientAlert(new ClientAlert("success", message));

        public static IActionResult Warning(this IActionResult result, string message)
            => result.AddClientAlert(new ClientAlert("warning", message));

        public static IActionResult Error(this IActionResult result, string message)
            => result.AddClientAlert(new ClientAlert("error", message));
    }
}
