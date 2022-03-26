using System.Collections.Generic;

namespace HSServer.Web
{
    public interface IWebCollectionReadOnly<T>
    {
        IReadOnlyList<T> this[string key] { get; }

        bool Exist(string Key);

        IReadOnlyCollection<string> Keys { get; }
        int Count { get; }

        bool TryGetValue(string key, out IReadOnlyList<T> value);
    }
}
