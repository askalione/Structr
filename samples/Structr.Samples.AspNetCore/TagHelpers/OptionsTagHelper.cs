using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Structr.AspNetCore.JavaScript;
using System;
using System.Linq;

namespace Structr.Samples.AspNetCore.TagHelpers
{
    [HtmlTargetElement("options", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class OptionsTagHelper : TagHelper
    {
        private readonly IJavaScriptOptionProvider _optionProvider;

        public OptionsTagHelper(IJavaScriptOptionProvider optionProvider)
        {
            if (optionProvider == null)
            {
                throw new ArgumentNullException(nameof(optionProvider));
            }

            _optionProvider = optionProvider;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Get options, which was added in action (this.AddJavaScriptOptions({}))
            var options = _optionProvider.GetOptions();

            if (options.Any() == false)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "script";
                output.Attributes.SetAttribute("type", "text/javascript");
                output.TagMode = TagMode.StartTagAndEndTag;

                // Create youir custom options client side
                // For example:
                output.Content.AppendHtml(";(function(){");
                foreach (var option in options)
                {
                    var key = option.Key;
                    var keyOptions = JObject.FromObject(option.Value).ToString(Formatting.None);
                    output.Content.AppendHtml($"app.options('{key}', {keyOptions});");
                }
                output.Content.AppendHtml("})();");
            }
        }
    }
}
