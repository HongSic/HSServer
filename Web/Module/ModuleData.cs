﻿using HS.Utils;

namespace HSServer.Web.Module
{
    public interface ModuleData
    {
        //protected ModuleData() { }
        //public ModuleData(Dictionary<string, string> Params, LanguageManager STR_LANG, WebHttpContext Context, DBManager DB)
        //{ this.Params = Params; this.STR_LANG = STR_LANG; this.Context = Context; this.DB = DB; }
        string Path { get; }
        LanguageManager STR_LANG { get; }
        WebHttpContext Context { get; }
        object ExtraData { get; }
    }
}
