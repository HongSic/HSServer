using System;

namespace HSServer.Web.Router
{
    public class RouterException : Exception
    {
        public IRouter Router { get; private set; }
        public RouterAttribute[] Attribute { get; private set; }
        public string Tag { get; private set; }
        internal RouterException(IRouter Router, Exception innerException) : base(innerException.Message, innerException) 
        {
            this.Router = Router;

            var type = Router.GetType();
            this.Tag = type.Name;

            var attrs = type.GetCustomAttributes(Web.Router.Router.AttributeType, true);
            Attribute = new RouterAttribute[attrs.Length];
            for (int i = 0; i < Attribute.Length; i++) Attribute[i] = (RouterAttribute)attrs[i];
        }

        public override string Message => $"Router Exception!! [{Tag}]: {base.Message}";
    }
}
