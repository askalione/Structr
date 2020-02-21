using Microsoft.AspNetCore.Mvc;

namespace Structr.AspNetCore.Mvc
{
    public class JavaScriptResult : ContentResult
    {
        public JavaScriptResult(string content)
        {
            Content = content;
            ContentType = "application/javascript";
        }
    }
}
