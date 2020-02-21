using System.Collections.Generic;

namespace Structr.AspNetCore.JavaScript
{
    public interface IJavaScriptOptionProvider
    {
        void AddOptions(string key, Dictionary<string, object> options);
        IReadOnlyDictionary<string, object> GetOptions(string key);
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> GetOptions();
    }
}
