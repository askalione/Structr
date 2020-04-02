using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.AspNetCore.Mvc;

namespace Structr.AspNetCore.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-append-if, asp-append-class")]
    public class AppendClassTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-append-if")]
        public bool Append { get; set; }

        [HtmlAttributeName("asp-append-class")]
        public string Class { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Append == true && string.IsNullOrEmpty(Class) == false)
            {
                output.AddClass(Class);
            }
        }
    }
}
