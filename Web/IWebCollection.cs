using System.Collections.Generic;

namespace HSServer.Web
{
    public interface IWebCollection<T> : IEnumerable<KeyValuePair<string, IList<T>>>
    {
        IList<T> this[string key] { get; set; }

        bool Exist(string Key);

        ICollection<string> Keys { get; }

        bool TryGetValue(string key, out IList<T> value);
    }
}
