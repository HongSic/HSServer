using HS.Utils;
using HS.Utils.Resource;

namespace HSServer.Web.Router
{
    public interface RouterData
    {
        //protected RouterData() { }
        //public RouterData(Dictionary<string, string> Params, LanguageManager STR_LANG, WebHttpContext Context, DBManager DB)
        //{ this.Params = Params; this.STR_LANG = STR_LANG; this.Context = Context; this.DB = DB; }
        string Path { get; }
        LanguageManager STR_LANG { get; }
        WebHttpContext Context { get; }
        object ExtraData { get; }
    }
}
