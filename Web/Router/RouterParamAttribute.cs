using System;

namespace HSServer.Web.Router
{
    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class RouterParamAttribute : Attribute
    {
        public string Name { get; private set; }
        public RouterParamType Type { get; private set; }
        public RouterParamAttribute(string Name, RouterParamType Type)
        {
            this.Name = Name;
            this.Type = Type;
        }
    }
}
