using System;

namespace HSServer.Web.Middleware
{
    public class MiddlewareException : Exception
    {
        public IMiddleware Middleware { get; private set; }
        public MiddlewareAttribute[] Attribute { get; private set; }
        public string Tag { get; private set; }
        internal MiddlewareException(IMiddleware Middleware, Exception innerException) : base(innerException.Message, innerException) 
        { 
            this.Middleware = Middleware;

            var type = Middleware.GetType();
            this.Tag = type.Name;

            var attrs = type.GetCustomAttributes(Web.Middleware.Middleware.AttributeType, true);
            Attribute = new MiddlewareAttribute[attrs.Length];
            for (int i = 0; i < Attribute.Length; i++) Attribute[i] = (MiddlewareAttribute)attrs[i];
        }
        public override string Message => $"Middleware Exception!! [{Tag}]: {base.Message}";
    }
}
