using System.Collections.Generic;

namespace HSServer.Web
{
    public interface IWebHttpResponseCookie : IWebCollection<WebHttpCookieValue>
    {
        IWebHttpResponseCookie Add(string key, WebHttpCookieValue value);
        IWebHttpResponseCookie Add(string key, IList<WebHttpCookieValue> value);
        IWebHttpResponseCookie Add(string key, params WebHttpCookieValue[] value);

        bool IsReadOnly { get; }

        bool Remove(string Key, bool Commit = true);
        void Clear(bool Commit = true);

        void Commit();
        void Refresh();
    }
}
