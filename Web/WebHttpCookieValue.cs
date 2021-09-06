using System;
using System.Text;

namespace HSServer.Web
{
    public class WebHttpCookieValue
    {
        protected WebHttpCookieValue() { }
        public WebHttpCookieValue(string Data, bool HttpOnly)
        {
            this.Data = Data;
            this.HttpOnly = HttpOnly;
        }
        public WebHttpCookieValue(string Data, bool HttpOnly, int? MaxAgeSeconds, DateTime? Expire, string Path = null) : this(Data, HttpOnly)
        {
            this.Path = Path;
            this.MaxAge = MaxAgeSeconds;
            this.Expires = Expire;
        }
        public WebHttpCookieValue(string Data, bool HttpOnly, int? MaxAgeSeconds, int? ExpireSeconds, string Path = null) : this(Data, HttpOnly)
        {
            this.Path = Path;
            this.MaxAge = MaxAgeSeconds;
            if (ExpireSeconds != null) Expires = DateTime.Now.AddSeconds(ExpireSeconds.Value);
        }

        public string Data { get; set; }
        public bool HttpOnly { get; set; }
        public int? MaxAge { get; set; }
        public DateTime? Expires { get; set; }

        public string SameSite { get; set; }
        public bool? Session { get; set; }
        public bool? HostOnly { get; set; }

        public string Path { get; set; }
        public string Domain { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Data);
            if (HttpOnly) sb.Append("; HttpOnly");
            if (Path != null) sb.Append("; Path=").Append(Path);
            if (Session != null) sb.Append("; Session=").Append(Session);
            if (HostOnly != null) sb.Append("; HostOnly=").Append(HostOnly);
            if (SameSite != null) sb.Append("; SameSite=").Append(SameSite);
            if (Domain != null) sb.Append("; Domain=").Append(Domain);
            if (MaxAge != null) sb.Append("; Max-Age=").Append(MaxAge);
            if (Expires != null) sb.Append("; Expires=").Append(Expires.Value.ToString("R"));
            return sb.ToString();
        }

        public static implicit operator string(WebHttpCookieValue Value) { return Value == null ? null : Value.Data; } 
        public static implicit operator WebHttpCookieValue(string Value) { return new WebHttpCookieValue(Value, false); } 

        public static WebHttpCookieValue Empty(bool HttpOnly = false, string Path = null) => new WebHttpCookieValue("", HttpOnly, 0, new DateTime(0), Path);
    }
}
