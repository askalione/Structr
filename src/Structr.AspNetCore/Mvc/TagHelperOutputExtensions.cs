using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;

namespace Structr.AspNetCore.Mvc
{
    public static class TagHelperOutputExtensions
    {
        public static void AddClass(this TagHelperOutput tagHelperOutput, string classValue)
        {
            if (tagHelperOutput == null)
                throw new ArgumentNullException(nameof(tagHelperOutput));

            if (!string.IsNullOrEmpty(classValue))
            {
                tagHelperOutput.AddClass(classValue, HtmlEncoder.Default);
            }
        }
    }
}
