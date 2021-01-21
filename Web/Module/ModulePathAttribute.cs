using System;

namespace HSServer.Web.Module
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ModulePathAttribute : Attribute
    {
        public string Path { get; private set; }
        public bool AutoRegister { get; private set; }

        public ModulePathAttribute(string Path, bool AutoRegister = true)
        {
            this.Path = Path;
            this.AutoRegister = AutoRegister;
        }
    }
}
