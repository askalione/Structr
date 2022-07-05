using Structr.AspNetCore.TagHelpers;

namespace Structr.Tests.AspNetCore.TestWebApp.Models
{
    public class OrderedListQueryVm
    {
        public string? Sort { get; set; }
        public SortOrder Order { get; set; }
    }
}