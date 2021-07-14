using System;
using System.Threading.Tasks;

namespace HSServer.Web.Middleware
{
    public interface MiddlewareProc : IDisposable
    {
        Task<MiddlewareData> Proc(MiddlewareData Data);
    }
}
