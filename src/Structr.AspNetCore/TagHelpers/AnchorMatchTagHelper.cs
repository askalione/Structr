using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Structr.AspNetCore.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "asp-match-class")]
    public class AnchorMatchTagHelper : AnchorTagHelper
    {
        [HtmlAttributeName("asp-match-class")]
        public string MatchClass { get; set; }

        public AnchorMatchTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Match())
            {
                output.AddClass(MatchClass ?? "");
            }
        }

        private bool Match()
        {
            var match = MatchArea()
                 && MatchController()
                 && MatchAction()
                 && MatchRouteValues();
            return match;
        }

        private bool MatchAction()
        {
            var action = ViewContext.RouteData.Values["Action"].ToString();
            var match = string.IsNullOrEmpty(Action) || Action.Equals(action, StringComparison.OrdinalIgnoreCase);
            return match;
        }

        private bool MatchController()
        {
            var controller = ViewContext.RouteData.Values["Controller"].ToString();
            var match = string.IsNullOrEmpty(Controller) || Controller.Equals(controller, StringComparison.OrdinalIgnoreCase);
            return match;
        }

        private bool MatchArea()
        {
            var area = ViewContext.RouteData.Values["Area"]?.ToString();
            var match = string.IsNullOrEmpty(Area) || Area.Equals(area, StringComparison.OrdinalIgnoreCase);
            return match;
        }

        private bool MatchRouteValues()
        {
            if (RouteValues.Any())
            {
                var routeValues = ViewContext.RouteData.Values.ToDictionary(x => x.Key, x => x.Value.ToString())
                    .Concat(ViewContext.HttpContext.Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString()))
                    .GroupBy(d => d.Key)
                    .ToDictionary(d => d.Key, d => d.First().Value);

                foreach (var routeValueKey in RouteValues.Keys)
                {
                    if (!routeValues.TryGetValue(routeValueKey, out string routeValue)
                        || !routeValue.Equals(RouteValues[routeValueKey], StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }

            }

            return true;
        }
    }
}
