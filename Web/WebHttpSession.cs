using System;
using System.Collections.Generic;

namespace HSServer.Web
{
    //WebHttpSession[""].SubKeys[""]
    [Serializable]
    public class WebHttpSession : IDisposable
    {
        //public static WebHttpSession GetWebHttpSession(WebHttpSessionRaw session) { }

        Dictionary<string, WebHttpSession> sub = new Dictionary<string, WebHttpSession>();
        Dictionary<string, string> dic = new Dictionary<string, string>();

        public WebHttpSession() : base() { }
        //public WebHttpSession(dynamic Value, WebSessionDataType Type = WebSessionDataType.String) {  base.Value = Value; this.Type = Type; } 
        public WebHttpSession(WebHttpSessionRaw session)
        {
            foreach (string key in session.Keys)
                dic.Add(key, session.GetString(key));
        }


        public string this[string Key]
        {
            get { return dic.ContainsKey(Key) ? dic[Key] : null; }
            set { Set(Key, value); }
        }

        public string Get(string Key)
        {
            if (dic.ContainsKey(Key)) return dic[Key];
            //else if (sub.ContainsKey(Key)) return sub[Key];
            else return null;
        }

        public WebHttpSession GetSub(string Key)
        {
            if (sub.ContainsKey(Key)) return sub[Key];
            else return null;
        }

        //public byte[] GetData(string Key) { try { return Convert.FromBase64String(dic[Key].ToString()); } catch { return null; } }

        //public void Set(string Key, WebHttpSession Value) { if (!Exist(Key)) sub[Key] = Value; }
        public void Set(string Key, string Value)
        {
            if (Exist(Key)) dic[Key] = Value;
            else dic.Add(Key, Value);
        }
        public WebHttpSession SetSub(string Key, WebHttpSession Value = null)
        {
            if (Value == null && sub.ContainsKey(Key))
            {
                if (sub[Key] == null) sub[Key] = new WebHttpSession();
                else sub[Key]?.ClearAll();
                return sub[Key];
            }
            else if (sub.ContainsKey(Key)) sub[Key] = Value;
            else if (Value == null) sub.Add(Key, Value = new WebHttpSession());
            else sub.Add(Key, Value);

            return Value;
        }

        public bool Exist(string Key)
        {
            if (dic.ContainsKey(Key)) return true;
            //else if (sub.ContainsKey(Key)) return true;
            else return false;
        }
        public bool ExistSub(string Key)
        {
            if (sub.ContainsKey(Key)) return true;
            else return false;
        }

        public void Remove(string Key) { dic.Remove(Key); }
        public void RemoveSub(string Key) { sub.Remove(Key); }

        public void Clear() { dic.Clear(); }
        public void ClearSub() { foreach(string key in sub.Keys) { sub[key].Dispose(); sub.Remove(key); } }
        public void ClearAll() { Clear(); ClearSub(); }

        public bool TryGetValue(string Key, out dynamic Value) 
        //public bool TryGetValue(string Key, out byte[] Value) 
        {
            if (dic.ContainsKey(Key)) { Value = dic[Key]; return true; }
            else if (sub.ContainsKey(Key)) { Value = sub[Key]; return true; }
            else { Value = null; return false; }
        }

        public void Dispose(){ ClearAll(); }

        //public static explicit operator string(WebHttpSession session) { return session.Value; }
        //public static explicit operator WebHttpSession(string value) { return session[Key]; }
    }
}
