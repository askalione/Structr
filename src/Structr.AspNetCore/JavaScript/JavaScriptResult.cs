using Microsoft.AspNetCore.Mvc;

namespace Structr.AspNetCore.JavaScript
{
    /// <summary>
    /// Defines kind of <see cref="ContentResult"/> that contains javascript code.
    /// </summary>
    public class JavaScriptResult : ContentResult
    {
        /// <summary>
        /// Creates an instance of <see cref="JavaScriptResult"/>.
        /// </summary>
        /// <param name="content">Javascript code.</param>
        public JavaScriptResult(string content)
        {
            Content = content;
            ContentType = "application/javascript";
        }
    }
}
