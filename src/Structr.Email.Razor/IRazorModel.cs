using System.Dynamic;

namespace Structr.Email.Razor
{
    public interface IRazorModel
    {
        ExpandoObject ViewBag { get; }
    }
}
