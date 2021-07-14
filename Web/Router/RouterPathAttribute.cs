using System;

namespace HSServer.Web.Router
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class RouterPathAttribute : Attribute
    {
        public string Path { get; private set; }
        public string Name { get; private set; }
        public bool AutoRegister { get; private set; }

        public RouterPathAttribute(string Path, bool AutoRegister = true)
        {
            this.Path = Path;
            this.AutoRegister = AutoRegister;
        }
        public RouterPathAttribute(string Path, string Name, bool AutoRegister = true) : this(Path, AutoRegister)
        {
            this.Name = Name;
        }
    }
}
