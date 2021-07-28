using System;

namespace HSServer.Web
{
    public class WebHttpCookieValue
    {
        protected WebHttpCookieValue() { }
        public WebHttpCookieValue(string Data, bool HttpOnly = false, DateTime? MaxAge = null, DateTime? Expire = null) 
        {
            this.Data = Data;
            this.Expires = Expire;
            this.HttpOnly = HttpOnly;
            this.MaxAge = MaxAge;
            this.Expires = Expire;
        }
        public WebHttpCookieValue(string Data, bool HttpOnly, int? MaxAgeSeconds, int? ExpireSeconds)
        {
            this.Data = Data;
            this.Expires = Expires;
            this.HttpOnly = HttpOnly;
            if(MaxAgeSeconds != null) MaxAge = DateTime.Now.AddSeconds(MaxAgeSeconds.Value);
            if(ExpireSeconds != null) Expires = DateTime.Now.AddSeconds(ExpireSeconds.Value);
        }

        public string Data { get; set; }
        public bool HttpOnly { get; set; }
        public DateTime? MaxAge { get; set; }
        public DateTime? Expires { get; set; }

        public string Path { get; set; }
        public string Domain { get; set; }

        public static implicit operator string(WebHttpCookieValue Value) { return Value.Data; } 
        public static implicit operator WebHttpCookieValue(string Value) { return new WebHttpCookieValue(Value); } 
    }
}
