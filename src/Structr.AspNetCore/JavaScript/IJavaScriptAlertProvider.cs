using System.Collections.Generic;

namespace Structr.AspNetCore.JavaScript
{
    /// <summary>
    /// Provides methods for assisting in transferring alerts from server side to client.
    /// </summary>
    public interface IJavaScriptAlertProvider
    {
        /// <summary>
        /// Adds alert to transfer to client.
        /// </summary>
        /// <param name="alert">Alert to send.</param>
        void AddAlert(JavaScriptAlert alert);

        /// <summary>
        /// Gets alerts transfered from client.
        /// </summary>
        /// <returns></returns>
        IEnumerable<JavaScriptAlert> GetAlerts();
    }
}
