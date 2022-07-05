using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Structr.AspNetCore.TagHelpers
{
    /// <summary>
    /// A <see cref="TagHelper"/> that adds CSS class to target element when
    /// "asp-append-if" is set to <see langword="true"/>.
    /// </summary>
    [HtmlTargetElement(Attributes = "asp-append-if, asp-append-class")]
    public class AppendClassTagHelper : TagHelper
    {
        /// <summary>
        /// Determines whenever class should or shouldn't be added.
        /// </summary>
        [HtmlAttributeName("asp-append-if")]
        public bool Append { get; set; }

        /// <summary>
        /// A CSS class to add to html element.
        /// </summary>
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
