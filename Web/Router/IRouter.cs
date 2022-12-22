using HS.Utils;
using System;
using System.Threading.Tasks;
using HS.Utils.Resource;

namespace HSServer.Web.Router
{
    public interface IRouter : IDisposable
    {
        void Attach(LanguageManager Language);
        Task<ModuleResponseCode> Route(RouterData Data);
        void Detach();
    }
}
