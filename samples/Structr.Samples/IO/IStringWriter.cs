using System.Threading.Tasks;

namespace Structr.Samples.IO
{
    public interface IStringWriter
    {
        Task WriteLineAsync(string value);
    }
}
