using System.Collections.Generic;

namespace HSServer.Web
{
    public interface IWebHttpHeaders : IWebCollection// : IDictionary<string, string>
    {
        new string this[string key] { get; set; }
        void Add(string Key, string Value);
        bool Remove(string Key);
        void Clear();

        bool IsReadOnly { get; }

        //IEnumerator<IWebHttpHeader> GetEnumerator();
    }
}
