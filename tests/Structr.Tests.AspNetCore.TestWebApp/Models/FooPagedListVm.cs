using Structr.Collections;

namespace Structr.Tests.AspNetCore.TestWebApp.Models
{
    public class FooPagedListVm
    {
        public FooPagedListQueryVm Query { get; set; } = default!;
        public IPagedList<FooVm> Foos { get; set; } = default!;
    }
}
