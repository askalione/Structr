using System.Dynamic;

namespace Structr.Email.Razor
{
    /// <summary>
    /// Provides a model for Razor rendering.
    /// </summary>
    public interface IRazorModel
    {
        /// <summary>
        /// Represents an object whose members can be dynamically added and removed at run.
        /// </summary>
        ExpandoObject ViewBag { get; }
    }
}
