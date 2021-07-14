using System;
using System.Threading.Tasks;

namespace HSServer.Web.Router
{
    public interface RouterProc : IDisposable
    {
        Task<RouterResponseCode> Proc(RouterData Data);
    }
}
