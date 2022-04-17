using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Structr.AspNetCore.TagHelpers
{
    [HtmlTargetElement(Attributes = "asp-sort")]
    public class SortTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-sort")]
        public string Sort { get; set; }

        [HtmlAttributeName("asp-default-sort")]
        public string DefaultSort { get; set; }

        [HtmlAttributeName("asp-default-order")]
        public SortOrder? DefaultOrder { get; set; }

        [HtmlAttributeName("asp-options")]
        public SortOptions Options { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IUrlHelper _urlHelper;

        public SortTagHelper(IUrlHelper urlHelper)
        {
            if (urlHelper == null)
                throw new ArgumentNullException(nameof(urlHelper));

            _urlHelper = urlHelper;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(Sort))
            {
                throw new InvalidOperationException("Sort must be not null");
            }

            if (Options == null)
            {
                Options = new SortOptions();
            }

            bool active = false;

            var sort = Sort.Trim();
            SortOrder? order = null;
            var defaultSort = DefaultSort?.Trim();
            var defaultOrder = DefaultOrder;

            var request = ViewContext.HttpContext.Request;
            var routeValues = request.Query.ToRouteValueDictionary();

            if (string.IsNullOrEmpty(defaultSort))
            {
                var routeSort = routeValues
                    .FirstOrDefault(x => x.Key.Equals(Options.SortRouteParamName, StringComparison.OrdinalIgnoreCase))
                    .Value?
                    .ToString();

                if (routeSort != null)
                {
                    active = SortEquals(routeSort, sort);
                }
            }
            else
            {
                active = SortEquals(defaultSort, sort);
            }

            if (defaultOrder == null)
            {
                if (active == true)
                {
                    var routeOrder = routeValues
                        .FirstOrDefault(x => x.Key.Equals(Options.OrderRouteParamName, StringComparison.OrdinalIgnoreCase))
                        .Value?
                        .ToString();

                    if (routeOrder != null)
                    {
                        if (Enum.TryParse(routeOrder, out SortOrder routeOrderValue))
                        {
                            order = routeOrderValue;
                        }
                    }

                }
            }
            else
            {
                if (SortEquals(defaultSort, sort) == true)
                    order = (SortOrder)defaultOrder;
            }

            routeValues.Remove(Options.SortRouteParamName);
            routeValues.Remove(Options.OrderRouteParamName);
            routeValues.Add(Options.SortRouteParamName, sort);
            routeValues.Add(Options.OrderRouteParamName, GetOrderRouteValue(order));

            string action = ViewContext.RouteData.Values["action"].ToString();
            string controller = ViewContext.RouteData.Values["controller"].ToString();

            var htmlEncoder = HtmlEncoder.Create(UnicodeRanges.All);
            var content = (await output.GetChildContentAsync(htmlEncoder)).GetContent();

            TagBuilder a = new TagBuilder("a");
            a.AddCssClass(Options.LinkCssCLass);
            a.AddCssClass(order.ToString().ToLower());
            if (active)
            {
                a.AddCssClass(Options.ActiveCssClass);
            }
            a.Attributes.Add("href", _urlHelper.Action(action, controller, routeValues));
            a.InnerHtml.Append(content);

            output.Content.SetHtmlContent(a);
        }

        private static string GetOrderRouteValue(SortOrder? order)
        {
            var result = order switch
            {
                SortOrder.Asc => SortOrder.Desc,
                SortOrder.Desc => SortOrder.Asc,
                _ => SortOrder.Asc
            };
            return result.ToString();
        }

        private static bool SortEquals(string sort1, string sort2)
        {
            if (sort1 == null && sort2 == null)
                return true;
            if ((sort1 != null && sort2 == null) || (sort1 == null && sort2 != null))
                return false;

            return sort1.Trim().Equals(sort2.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }

    public class SortOptions
    {
        public string LinkCssCLass { get; set; }
        public string ActiveCssClass { get; set; }
        public string SortRouteParamName { get; set; }
        public string OrderRouteParamName { get; set; }

        public SortOptions()
        {
            LinkCssCLass = "sort";
            ActiveCssClass = "active";
            SortRouteParamName = "sort";
            OrderRouteParamName = "order";
        }
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }
}
