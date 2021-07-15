using HS.Utils;
using System;
using System.Threading.Tasks;

namespace HSServer.Web.Middleware
{
    public interface IMiddleware : IDisposable
    {
        void Attach(LanguageManager Language);
        Task<MiddlewareData> Start(MiddlewareData Data);

        void Detach();
    }
}
