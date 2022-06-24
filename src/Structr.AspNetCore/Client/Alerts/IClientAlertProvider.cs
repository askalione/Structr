using System.Collections.Generic;

namespace Structr.AspNetCore.Client.Alerts
{
    /// <summary>
    /// Provides methods for assisting in transferring alerts from server side to client.
    /// </summary>
    public interface IClientAlertProvider
    {
        /// <summary>
        /// Adds alert to transfer to client.
        /// </summary>
        /// <param name="alert">Alert to send.</param>
        void AddClientAlert(ClientAlert alert);

        /// <summary>
        /// Gets alerts transferred to client.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ClientAlert> GetAllClientAlerts();
    }
}
