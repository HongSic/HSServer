using System.Collections.Generic;

namespace HSServer.Web
{
    public interface IWebCollection
    {
        string this[string key] { get; }

        bool Exist(string Key);

        ICollection<string> Keys { get; }
        int Count { get; }

        bool TryGetValue(string key, out string value);
    }
}
