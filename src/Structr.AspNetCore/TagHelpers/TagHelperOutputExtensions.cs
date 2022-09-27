using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

namespace Structr.AspNetCore.TagHelpers
{
    /// <summary>
    /// Defines extension methods on <see cref="TagHelperOutput"/>.
    /// </summary>
    public static class TagHelperOutputExtensions
    {
        private static readonly HtmlEncoder _encoder = HtmlEncoder.Default;

        /// <summary>
        /// Adds specified class to TagHelper output using built-in instance of the System.Text.Encodings.Web.HtmlEncoder.
        /// </summary>
        /// <param name="tagHelperOutput">The <see cref="TagHelperOutput"/>.</param>
        /// <param name="classValue">Css class to add.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="tagHelperOutput"/> is <see langword="null"/>.</exception>
        public static void AddClass(this TagHelperOutput tagHelperOutput, string classValue)
        {
            if (tagHelperOutput == null)
            {
                throw new ArgumentNullException(nameof(tagHelperOutput));
            }

            if (string.IsNullOrEmpty(classValue) == false)
            {
                List<string> cssClasses = SplitClasses(classValue);
                foreach (string cssClass in cssClasses)
                {
                    tagHelperOutput.AddClass(cssClass, _encoder);
                }
            }
        }

        /// <summary>
        /// Removes specified class from TagHelper output using built-in instance of the System.Text.Encodings.Web.HtmlEncoder.
        /// </summary>
        /// <param name="tagHelperOutput">The <see cref="TagHelperOutput"/>.</param>
        /// <param name="classValue">Css class to remove.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="tagHelperOutput"/> is <see langword="null"/>.</exception>
        public static void RemoveClass(this TagHelperOutput tagHelperOutput, string classValue)
        {
            if (tagHelperOutput == null)
            {
                throw new ArgumentNullException(nameof(tagHelperOutput));
            }

            if (string.IsNullOrEmpty(classValue) == false)
            {
                List<string> cssClasses = SplitClasses(classValue);
                foreach (string cssClass in cssClasses)
                {
                    tagHelperOutput.RemoveClass(cssClass, _encoder);
                }                    
            }
        }

        private static List<string> SplitClasses(string classValue)
            => classValue.Split(' ').Select(x => x.Trim()).ToList();
    }
}
