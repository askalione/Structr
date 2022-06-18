using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;

namespace Structr.AspNetCore.Mvc
{
    /// <summary>
    /// Defines extension methods on <see cref="TagHelperOutput"/>.
    /// </summary>
    public static class TagHelperOutputExtensions
    {
        /// <summary>
        /// Add specified class to TagHelper output.
        /// </summary>
        /// <param name="tagHelperOutput"></param>
        /// <param name="classValue">Css class to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddClass(this TagHelperOutput tagHelperOutput, string classValue)
        {
            if (tagHelperOutput == null)
            {
                throw new ArgumentNullException(nameof(tagHelperOutput));
            }

            if (string.IsNullOrEmpty(classValue) == false)
            {
                tagHelperOutput.AddClass(classValue, HtmlEncoder.Default);
            }
        }
    }
}
