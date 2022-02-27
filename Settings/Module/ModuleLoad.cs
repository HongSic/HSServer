namespace HSServer.Settings.WebModule
{
    public class ModuleLoad
    {
        public ModuleLoad(string[] MiddleWare, string[] Module, string[] Controller)
        {
            this.MiddleWare = MiddleWare;
            this.Module = Module;
            this.Controller = Controller;
        }
        public string[] Module { get; private set; }
        public string[] MiddleWare { get; private set; }
        public string[] Controller { get; private set; }
    }
}
