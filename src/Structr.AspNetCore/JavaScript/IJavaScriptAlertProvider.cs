using System.Collections.Generic;

namespace Structr.AspNetCore.JavaScript
{
    public interface IJavaScriptAlertProvider
    {
        void AddAlert(JavaScriptAlert alert);
        IEnumerable<JavaScriptAlert> GetAlerts();
    }
}
