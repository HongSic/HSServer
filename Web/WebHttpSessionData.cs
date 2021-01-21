using System;

namespace HSServer.Web
{
    public class WebHttpSessionData
    {
        public string Value { get; private set; }
        public WebSessionDataType Type { get; private set; }

        public WebHttpSessionData() { }
        public WebHttpSessionData(string Value, WebSessionDataType Type = WebSessionDataType.String) { this.Value = Value; this.Type = Type; }

        [Obsolete]
        public dynamic Get()
        {
            if (Type == WebSessionDataType.Data) return Convert.FromBase64String(Value);
            else if (Type == WebSessionDataType.Decimal) return Convert.ToDecimal(Value);
            else return Value;
        }

        public override string ToString()
        {
            return Value;//string.Format("Type={0}, Value={1}", Type, Value);
        }

        public static implicit operator string(WebHttpSessionData data) { return data != null ? data.Value : null; }
        public static implicit operator WebHttpSessionData(string Value) { return new WebHttpSessionData(Value); }
    }
    public enum WebSessionDataType { String, Decimal, Data }
}
