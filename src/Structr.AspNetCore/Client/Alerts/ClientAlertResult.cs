using Microsoft.Extensions.DependencyInjection;
using Structr.AspNetCore.Client.Alerts;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// Represents an alert to be send along with action result.
    /// </summary>
    public class ClientAlertResult : IActionResult
    {
        private readonly IActionResult _result;
        private readonly ClientAlert _alert;

        /// <summary>
        /// Creates an instance of <see cref="ClientAlert"/>.
        /// </summary>
        /// <param name="result"><see cref="IActionResult"/> to attach alert to.</param>
        /// <param name="alert">Alert to attach to <see cref="IActionResult"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ClientAlertResult(IActionResult result, ClientAlert alert)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }
            if (alert == null)
            {
                throw new ArgumentNullException(nameof(alert));
            }

            _result = result;
            _alert = alert;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var alertProvider = context.HttpContext.RequestServices.GetRequiredService<IClientAlertProvider>();
            alertProvider.AddClientAlert(_alert);
            await _result.ExecuteResultAsync(context);
        }
    }
}
