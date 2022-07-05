using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.AspNetCore.Http;
using Structr.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.TagHelpers
{
    /// <summary>
    /// A <see cref="TagHelper"/> creating array of buttons and other elements forming UI pagination controls.
    /// </summary>
    [HtmlTargetElement("pagination", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PaginationTagHelper : TagHelper
    {
        /// <summary>
        /// Options influencing appearance of UI pagination controls.
        /// </summary>
        [HtmlAttributeName("asp-options")]
        public PaginationOptions Options { get; set; }

        /// <summary>
        /// An actual instance of <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/>.
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IUrlHelper _urlHelper;

        /// <summary>
        /// Initializes an instance of <see cref="PaginationTagHelper"/>.
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PaginationTagHelper(IUrlHelper urlHelper)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }

            _urlHelper = urlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Options == null)
            {
                Options = new PaginationOptions();
            }

            var pagedList = Options.PagedEnumerable;

            if (pagedList == null)
            {
                output.SuppressOutput();
                return;
            }

            if (Options.Display == PaginationDisplayMode.Never || (Options.Display == PaginationDisplayMode.IfNeeded && pagedList.TotalPages <= 1))
            {
                output.SuppressOutput();
                return;
            }

            Func<int, string> pageUrlGenerator = Options.PageUrlGenerator ?? (pageNumber => AppendPageNumberToPageUrl(pageNumber));

            var listItemLinks = new List<TagBuilder>();

            var firstPageToDisplay = 1;
            var lastPageToDisplay = pagedList.TotalPages;
            var pageNumbersToDisplay = lastPageToDisplay;
            if (Options.MaximumPageNumbersToDisplay.HasValue && pagedList.TotalPages > Options.MaximumPageNumbersToDisplay)
            {
                var maxPageNumbersToDisplay = Options.MaximumPageNumbersToDisplay.Value;
                firstPageToDisplay = pagedList.PageNumber - (maxPageNumbersToDisplay / 2);
                if (firstPageToDisplay < 1)
                    firstPageToDisplay = 1;
                pageNumbersToDisplay = maxPageNumbersToDisplay;
                lastPageToDisplay = firstPageToDisplay + pageNumbersToDisplay - 1;
                if (lastPageToDisplay > pagedList.TotalPages)
                    firstPageToDisplay = pagedList.TotalPages - maxPageNumbersToDisplay + 1;
            }

            if (Options.DisplayLinkToFirstPage == PaginationDisplayMode.Always || (Options.DisplayLinkToFirstPage == PaginationDisplayMode.IfNeeded && firstPageToDisplay > 1))
                listItemLinks.Add(PaginationHelper.First(pagedList, pageUrlGenerator, Options));

            if (Options.DisplayLinkToPreviousPage == PaginationDisplayMode.Always || (Options.DisplayLinkToPreviousPage == PaginationDisplayMode.IfNeeded && !pagedList.IsFirstPage))
                listItemLinks.Add(PaginationHelper.Previous(pagedList, pageUrlGenerator, Options));

            if (Options.DisplayLinkToIndividualPages)
            {
                if (Options.DisplayEllipsesWhenNotShowingAllPageNumbers && firstPageToDisplay > 1)
                    listItemLinks.Add(PaginationHelper.Ellipses(Options));

                foreach (var i in Enumerable.Range(firstPageToDisplay, pageNumbersToDisplay))
                {
                    if (i > firstPageToDisplay && !string.IsNullOrWhiteSpace(Options.DelimiterBetweenPageNumbers))
                        listItemLinks.Add(PaginationHelper.WrapInListItem(Options.DelimiterBetweenPageNumbers));

                    listItemLinks.Add(PaginationHelper.Page(i, pagedList, pageUrlGenerator, Options));
                }

                if (Options.DisplayEllipsesWhenNotShowingAllPageNumbers && (firstPageToDisplay + pageNumbersToDisplay - 1) < pagedList.TotalPages)
                    listItemLinks.Add(PaginationHelper.Ellipses(Options));
            }

            if (Options.DisplayLinkToNextPage == PaginationDisplayMode.Always || (Options.DisplayLinkToNextPage == PaginationDisplayMode.IfNeeded && !pagedList.IsLastPage))
                listItemLinks.Add(PaginationHelper.Next(pagedList, pageUrlGenerator, Options));

            if (Options.DisplayLinkToLastPage == PaginationDisplayMode.Always || (Options.DisplayLinkToLastPage == PaginationDisplayMode.IfNeeded && lastPageToDisplay < pagedList.TotalPages))
                listItemLinks.Add(PaginationHelper.Last(pagedList, pageUrlGenerator, Options));

            if (listItemLinks.Any())
            {
                if (!string.IsNullOrWhiteSpace(Options.FirstListItemCssClass))
                    listItemLinks.First().AddCssClass(Options.FirstListItemCssClass);

                if (!string.IsNullOrWhiteSpace(Options.LastListItemCssClass))
                    listItemLinks.Last().AddCssClass(Options.LastListItemCssClass);

                foreach (var li in listItemLinks)
                    foreach (var c in Options.LiElementCssClasses ?? Enumerable.Empty<string>())
                        li.AddCssClass(c);
            }

            var ul = new TagBuilder("ul");
            foreach (var listItemLink in listItemLinks)
                ul.InnerHtml.AppendHtml(listItemLink);
            foreach (var c in Options.UlElementCssClasses ?? Enumerable.Empty<string>())
                ul.AddCssClass(c);

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            string classes = "pagination-container";
            if (Options.ContainerCssClasses != null && Options.ContainerCssClasses.Any())
            {
                classes += " " + string.Join(" ", Options.ContainerCssClasses);
            }
            output.Attributes.Add("class", classes);
            output.Content.AppendHtml(ul);
        }

        private string AppendPageNumberToPageUrl(int pageNumber)
        {
            string action = ViewContext.RouteData.Values["action"].ToString();
            string controller = ViewContext.RouteData.Values["controller"].ToString();
            string routeParamName = !string.IsNullOrEmpty(Options.PageNumberRouteParamName) ? Options.PageNumberRouteParamName : "page";
            var routeValues = ViewContext.HttpContext.Request.Query.ToRouteValueDictionary(routeParamName, pageNumber);
            return _urlHelper.Action(action, controller, routeValues);
        }
    }

    internal static class PaginationHelper
    {
        public static TagBuilder WrapInListItem(string text)
        {
            var li = new TagBuilder("li");
            li.InnerHtml.Append(text);
            return li;
        }

        public static TagBuilder WrapInListItem(TagBuilder inner, PaginationOptions options, params string[] classes)
        {
            var li = new TagBuilder("li");
            foreach (var @class in classes)
                li.AddCssClass(@class);
            if (options.FunctionToTransformEachPageLink != null)
                return options.FunctionToTransformEachPageLink(li, inner);
            li.InnerHtml.AppendHtml(inner);
            return li;
        }

        public static TagBuilder First(IPagedEnumerable pagedEnumerable, Func<int, string> generatePageUrl, PaginationOptions options)
        {
            const int targetPageNumber = 1;
            var first = new TagBuilder("a");
            first.InnerHtml.Append(string.Format(options.LinkToFirstPageFormat, targetPageNumber));

            if (pagedEnumerable.IsFirstPage)
            {
                return WrapInListItem(first, options, "pagination-first", options.LiElementDisabledCssClass);
            }

            first.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(first, options, "pagination-first");
        }

        public static TagBuilder Previous(IPagedEnumerable pagedEnumerable, Func<int, string> generatePageUrl, PaginationOptions options)
        {
            var targetPageNumber = pagedEnumerable.PageNumber - 1;
            var previous = new TagBuilder("a");
            previous.InnerHtml.Append(string.Format(options.LinkToPreviousPageFormat, targetPageNumber));
            previous.Attributes["rel"] = "prev";

            if (pagedEnumerable.HasPreviousPage == false)
            {
                return WrapInListItem(previous, options, "pagination-previous", options.LiElementDisabledCssClass);
            }

            previous.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(previous, options, "pagination-previous");
        }

        public static TagBuilder Page(int i, IPagedEnumerable pagedEnumerable, Func<int, string> generatePageUrl, PaginationOptions options)
        {
            var format = options.FunctionToDisplayEachPageNumber
                ?? (pageNumber => string.Format(options.LinkToIndividualPageFormat, pageNumber));
            var targetPageNumber = i;
            var page = new TagBuilder("a");
            page.InnerHtml.Append(format(targetPageNumber));

            if (i == pagedEnumerable.PageNumber)
            {
                return WrapInListItem(page, options, options.LiElementActiveCssClass);
            }

            page.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(page, options);
        }

        public static TagBuilder Next(IPagedEnumerable pagedEnumerable, Func<int, string> generatePageUrl, PaginationOptions options)
        {
            var targetPageNumber = pagedEnumerable.PageNumber + 1;
            var next = new TagBuilder("a");
            next.InnerHtml.Append(string.Format(options.LinkToNextPageFormat, targetPageNumber));
            next.Attributes["rel"] = "next";

            if (pagedEnumerable.HasNextPage == false)
            {
                return WrapInListItem(next, options, "pagination-next", options.LiElementDisabledCssClass);
            }

            next.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(next, options, "pagination-next");
        }

        public static TagBuilder Last(IPagedEnumerable pagedEnumerable, Func<int, string> generatePageUrl, PaginationOptions options)
        {
            var targetPageNumber = pagedEnumerable.TotalPages;
            var last = new TagBuilder("a");
            last.InnerHtml.Append(string.Format(options.LinkToLastPageFormat, targetPageNumber));

            if (pagedEnumerable.IsLastPage)
            {
                return WrapInListItem(last, options, "pagination-last", options.LiElementDisabledCssClass);
            }

            last.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(last, options, "pagination-last");
        }

        public static TagBuilder Ellipses(PaginationOptions options)
        {
            var a = new TagBuilder("a");
            a.InnerHtml.AppendHtml(options.EllipsesFormat);

            return WrapInListItem(a, options, "pagination-ellipses", options.LiElementDisabledCssClass);
        }
    }

    /// <summary>
    /// Display mode of pagination UI controls.
    /// </summary>
    public enum PaginationDisplayMode
    {
        /// <summary>
        /// Display even if control couldn't be used. For example forward button will still be
        /// displayed even on the last page though it'll be disabled.
        /// </summary>
        Always,

        /// <summary>
        /// Never display.
        /// </summary>
        Never,

        /// <summary>
        /// Display only when control could be used. For example forward button will be
        /// displayed on every page except the last one.
        /// </summary>
        IfNeeded
    }

    /// <summary>
    /// Defines parameters influating appearance of UI pagination controls.
    /// </summary>
    public class PaginationOptions
    {
        /// <summary>
        /// An instance of <see cref="IPagedList"/> to get information about pagination from.
        /// </summary>
        public IPagedEnumerable PagedEnumerable { get; set; }

        /// <summary>
        /// Factory intended for generation of urls to different pages.
        /// </summary>
        public Func<int, string> PageUrlGenerator { get; set; }

        public IEnumerable<string> ContainerCssClasses { get; set; }
        public IEnumerable<string> UlElementCssClasses { get; set; }
        public IEnumerable<string> LiElementCssClasses { get; set; }
        public string FirstListItemCssClass { get; set; }
        public string LastListItemCssClass { get; set; }
        public string LiElementActiveCssClass { get; set; }
        public string LiElementDisabledCssClass { get; set; }

        /// <summary>
        /// Determines display mode for all pagination controls.
        /// </summary>
        public PaginationDisplayMode Display { get; set; }

        /// <summary>
        /// Determines display mode for first-page button. Default value is <see cref="PaginationDisplayMode.IfNeeded"/>.
        /// </summary>
        public PaginationDisplayMode DisplayLinkToFirstPage { get; set; }

        /// <summary>
        /// Determines display mode for last-page button. Default value is <see cref="PaginationDisplayMode.IfNeeded"/>.
        /// </summary>
        public PaginationDisplayMode DisplayLinkToLastPage { get; set; }

        /// <summary>
        /// Determines display mode for previous-page button. Default value is <see cref="PaginationDisplayMode.Never"/>.
        /// </summary>
        public PaginationDisplayMode DisplayLinkToPreviousPage { get; set; }

        /// <summary>
        /// Determines display mode for next-page button. Default value is <see cref="PaginationDisplayMode.Never"/>.
        /// </summary>
        public PaginationDisplayMode DisplayLinkToNextPage { get; set; }

        /// <summary>
        /// Determines whenever buttons to specific pages should be displayed or not. Default value is <see langword="true"/>.
        /// </summary>
        public bool DisplayLinkToIndividualPages { get; set; }

        /// <summary>
        /// Maximum count of buttons with page numbers to display. Default value is <с>3</с>.
        /// </summary>
        public int? MaximumPageNumbersToDisplay { get; set; }

        /// <summary>
        /// Determines whenever to display ellipses between sets of buttons or not. Default value is <see langword="true"/>.
        /// </summary>
        public bool DisplayEllipsesWhenNotShowingAllPageNumbers { get; set; }

        /// <summary>
        /// Format of ellipsis displayed between sets of page buttons. Default value is '<c>...</c>'
        /// </summary>
        public string EllipsesFormat { get; set; }

        /// <summary>
        /// Format of button redirecting to first page. Default value is '<c>««</c>'.
        /// </summary>
        public string LinkToFirstPageFormat { get; set; }

        /// <summary>
        /// Format of button redirecting to previous page. Default value is '<c>«</c>'.
        /// </summary>
        public string LinkToPreviousPageFormat { get; set; }

        /// <summary>
        /// Format of button redirecting to specific page. Default value is '<c>{0}</c>' with placeholder for number. 
        /// </summary>
        public string LinkToIndividualPageFormat { get; set; }

        /// <summary>
        /// Format of button redirecting to next page. Default value is '<c>»</c>'.
        /// </summary>
        public string LinkToNextPageFormat { get; set; }

        /// <summary>
        /// Format of button redirecting to last page. Default value is '<c>»»</c>'.
        /// </summary>
        public string LinkToLastPageFormat { get; set; }

        /// <summary>
        /// Allows to transform output of page numbers allowing to build for example something like "First", "Second", etc.
        /// </summary>
        public Func<int, string> FunctionToDisplayEachPageNumber { get; set; }

        /// <summary>
        /// Delimiter strings to place between page numbers.
        /// </summary>
        public string DelimiterBetweenPageNumbers { get; set; }

        /// <summary>
        /// Method allowing to format HTML for <li> and <a> inside of paging controls, using TagBuilders.
        /// </summary>
        public Func<TagBuilder, TagBuilder, TagBuilder> FunctionToTransformEachPageLink { get; set; }

        /// <summary>
        /// Route parameter name for page number. Default value is '<c>page</c>'.
        /// </summary>
        public string PageNumberRouteParamName { get; set; }

        public PaginationOptions()
        {
            LiElementActiveCssClass = "active";
            LiElementDisabledCssClass = "disabled";
            DisplayLinkToFirstPage = PaginationDisplayMode.IfNeeded;
            DisplayLinkToLastPage = PaginationDisplayMode.IfNeeded;
            DisplayLinkToPreviousPage = PaginationDisplayMode.Never;
            DisplayLinkToNextPage = PaginationDisplayMode.Never;
            DisplayLinkToIndividualPages = true;
            MaximumPageNumbersToDisplay = 3;
            DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            EllipsesFormat = "…";
            LinkToFirstPageFormat = "««";
            LinkToPreviousPageFormat = "«";
            LinkToIndividualPageFormat = "{0}";
            LinkToNextPageFormat = "»";
            LinkToLastPageFormat = "»»";
            FunctionToDisplayEachPageNumber = null;
            FirstListItemCssClass = "first";
            LastListItemCssClass = "last";
            ContainerCssClasses = null;
            UlElementCssClasses = new[] { "pagination" };
            LiElementCssClasses = new[] { "page-item" };
            FunctionToTransformEachPageLink = (li, link) =>
            {
                link.AddCssClass("page-link");
                li.InnerHtml.AppendHtml(link);
                return li;
            };
            PageUrlGenerator = null;
            PageNumberRouteParamName = "page";
        }
    }
}
