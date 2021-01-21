using System;
using System.Collections.Generic;
using System.Text;

namespace HSServer.Settings.WebRouter
{
    public class WebRouterLoad
    {
        public WebRouterLoad(string[] MiddleWare, string[] Module)
        {
            this.MiddleWare = MiddleWare;
            this.Module = Module;
        }
        public string[] Module { get; private set; }
        public string[] MiddleWare { get; private set; }
    }
}
