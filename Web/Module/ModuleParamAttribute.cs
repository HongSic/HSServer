using System;

namespace HSServer.Web.Module
{
    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class ModuleParamAttribute : Attribute
    {
        public string Name { get; private set; }
        public ModuleParamType Type { get; private set; }
        public ModuleParamAttribute(string Name, ModuleParamType Type)
        {
            this.Name = Name;
            this.Type = Type;
        }
    }
}
