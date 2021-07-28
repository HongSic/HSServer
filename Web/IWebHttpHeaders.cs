using System.Collections.Generic;

namespace HSServer.Web
{
    public interface IWebHttpHeaders : IEnumerable<KeyValuePair<string, IList<string>>>// : IDictionary<string, string>
    {
        IList<string> this[string key] { get; set; }
        void Add(string Key, string Value);
        void Add(string Key, params string[] Values);
        void Add(string Key, IList<string> Values);
        bool Remove(string Key);
        void Clear();

        ICollection<string> Keys { get; }

        bool Exist(string Key);

        bool IsReadOnly { get; }

        //IEnumerator<IWebHttpHeader> GetEnumerator();
    }
}
