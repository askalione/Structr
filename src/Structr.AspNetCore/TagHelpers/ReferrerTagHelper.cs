using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.AspNetCore.Internal;
using Structr.AspNetCore.Mvc;
using System;

namespace Structr.AspNetCore.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "asp-referrer")]
    public class ReferrerTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-referrer")]
        public string Referrer { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var request = ViewContext.HttpContext.Request;

            string httpReferrer = request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(httpReferrer))
            {
                string currentUrl = request.GetAbsoluteUri();
                if (httpReferrer.Equals(currentUrl, StringComparison.OrdinalIgnoreCase))
                    httpReferrer = null;
            }

            string referrer = request.GetReferrer(string.IsNullOrWhiteSpace(httpReferrer) == false ? httpReferrer : Referrer);

            if (output.Attributes.ContainsName("href"))
                output.Attributes.Remove(output.Attributes["href"]);
            output.Attributes.Add("href", referrer);

            TagBuilder cache = new TagBuilder("input");
            cache.Attributes.Add("type", "hidden");
            cache.Attributes.Add("name", ReferrerConstants.Key);
            cache.Attributes.Add("value", referrer);
            output.PostElement.AppendHtml(cache);
        }
    }
}
