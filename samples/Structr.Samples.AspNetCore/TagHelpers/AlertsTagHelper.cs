using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.AspNetCore.Client.Alerts;
using System;
using System.Linq;

namespace Structr.Samples.AspNetCore.TagHelpers
{
    [HtmlTargetElement("alerts", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AlertsTagHelper : TagHelper
    {
        private readonly IClientAlertProvider _clientAlertProvider;

        public AlertsTagHelper(IClientAlertProvider alertProvider)
        {
            if (alertProvider == null)
            {
                throw new ArgumentNullException(nameof(alertProvider));
            }

            _clientAlertProvider = alertProvider;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Get alerts, which was added in action (View.Success("Message"))
            var alerts = _clientAlertProvider.GetAllClientAlerts();

            if (alerts.Any() == false)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "script";
                output.Attributes.SetAttribute("type", "text/javascript");
                output.TagMode = TagMode.StartTagAndEndTag;

                // Create your custom alerts client side
                // For example:
                output.Content.AppendHtml(";(function(){");
                foreach (var alert in alerts)
                {
                    var type = alert.Type.ToString().ToLower();
                    var message = alert.Message.Replace("'", "\'");
                    output.Content.AppendHtml($"app.alert('{type}', '{message}');");
                }
                output.Content.AppendHtml("})();");
            }
        }
    }
}
