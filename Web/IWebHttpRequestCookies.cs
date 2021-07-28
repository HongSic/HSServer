using System.Collections.Generic;

namespace HSServer.Web
{
    public interface IWebHttpRequestCookies : IReadOnlyCollection<KeyValuePair<string, string>>
    {
        string this[string key] { get; }

        bool Exist(string Key);

        IReadOnlyCollection<string> Keys { get; }
        int Count { get; }

        bool TryGetValue(string key, out string value);
    }
}
