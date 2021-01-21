using System;
using System.Collections.Generic;

namespace HSServer.Web
{
    public interface WebHttpSessionRaw
    {
        string Id { get; }

        string GetString(string Key);
        [Obsolete]
        int? GetInt32(string Key);
        byte[] Get(string Key);
        bool TryGetValue(string Key, out byte[] Value);

        void SetString(string Key, string Value);
        [Obsolete]
        void SetInt32(string Key, int Value);
        void Set(string Key, byte[] Value);

        void Clear();

        void Remove(string Key);

        IEnumerable<string> Keys { get; }
    }
}
