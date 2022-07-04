using Microsoft.AspNetCore.Mvc;

namespace Structr.AspNetCore.Client.Alerts
{
    /// <summary>
    /// Defines extension methods on <see cref="IActionResult"/>.
    /// </summary>
    public static class ClientAlertActionResultExtensions
    {
        /// <summary>
        /// Appends specified alert to <see cref="IActionResult"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="alert">Allert to append.</param>
        /// <returns>A <see cref="ClientAlertResult"/> object containing original <see cref="IActionResult"/> object and a message.</returns>
        public static IActionResult AddClientAlert(this IActionResult result, ClientAlert alert)
            => new ClientAlertResult(result, alert);
    }
}
