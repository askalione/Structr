using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.JavaScript;

namespace Structr.AspNetCore.Mvc
{
    /// <summary>
    /// Defines extension methods on <see cref="IActionResult"/>.
    /// </summary>
    public static class ActionResultExtensions
    {
        /// <summary>
        /// Appends specified alert to <see cref="IActionResult"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="alert">Allert to append.</param>
        /// <returns>A <see cref="JavaScriptAlertResult"/> object containing original <see cref="IActionResult"/> object and a message.</returns>
        public static IActionResult AddJavaScriptAlert(this IActionResult result, JavaScriptAlert alert)
            => new JavaScriptAlertResult(result, alert);
    }
}
